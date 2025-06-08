using ITServicePortfolioManager.BLL.Algorithm;
using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models;
using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;
using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating;
using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating.WithDiscount;
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ITServicePortfolioManager.BLL.Services.Common;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.BLL.Services;


public class SolverPortfolioService(
    ITaskRepository taskRepository,
    IServicePortfolioResultRepository servicePortfolioResultRepository,
    IDiscountAnalysisService discountAnalysisService) : ISolverServicePortfolio
{
    private const double Step001 = 0.01;
    private const double Step05 = 0.05;
    private const double Step10 = 0.10;
    public delegate (List<ProviderDto> providers, DiscountTargetInfo Target) DiscountStrategyDelegate(List<ProviderDto> providers, double discount, object extraData);

    public async Task<SolveResultDto> CreateServicePortfoliosAsync(TaskDto dto, long UserId)
    {
        var taskEntity = await taskRepository.Create(TaskDto.ToEntity(dto,UserId));
        var solution = SolveOptimizationTask(dto.Algorithm, dto.TotalHumanResource, dto.Providers);
        var resultEntity = await servicePortfolioResultRepository.Create(ResultDto.ToEntity(solution, taskEntity.Id));
        return new SolveResultDto(solution, resultEntity.Id);
    }

    public async Task<DeltaSetDto> GetFullSimulation(
        TaskDto dto,
        long id,
        DiscountStrategyDelegate discountStrategy, 
        string strategyData)
    {
        var allSimResults = await SimulateDiscountsAsync(
            dto,
            id,
            discountStrategy,
            strategyData
        );
        var allDeltas = discountAnalysisService.CalculateIncomeChanges(allSimResults);
        var generalDeltas = allDeltas
            .Where(d => Math.Abs(Math.Round(d.Discount / Step05) * Step05 - d.Discount) < 1e-6)
            .OrderBy(d => d.Discount)
            .ToList();

        var best = generalDeltas.OrderByDescending(x => x.TotalDeltaPercent).First();
        if (best.TotalDeltaPercent <= 0)
        {
            return new DeltaSetDto(generalDeltas, new List<DiscDeltaDto>(), null, null);
        }

        var bestDiscount = best.Discount;

        var from = Math.Max(0, bestDiscount - Step10);
        if (from == 0)
        {
            from = Step001;
        }

        var to = Math.Min(0.99, bestDiscount + Step10);
        var detailedDeltas = new List<DiscDeltaDto>();

        if (from > 0 && allDeltas.Count > 0)
        {
            detailedDeltas.Add(allDeltas[0]);
        }

        detailedDeltas.AddRange(
            allDeltas
                .Where(d => d.Discount >= from && d.Discount <= to)
                .ToList()
        );


        var bestDetailedDelta = detailedDeltas.OrderByDescending(d => d.TotalDeltaPercent).First();
        var bestResultWithDiscount = allSimResults.Results[allDeltas.IndexOf(bestDetailedDelta)];

        return new DeltaSetDto(generalDeltas, detailedDeltas, bestResultWithDiscount,allSimResults.Target);
    }
    
    public async Task<SolveResultDto> GetSolveAsync(long id)
    {
        var resultEntity = await servicePortfolioResultRepository.GetById(id);
        return new SolveResultDto(ResultDto.ToDto(resultEntity), resultEntity.Id);
    }

    
    public async Task<SolveResultDto> GetSolveByTaskIdAsync(long taskId)
    {
        var resultEntity = await servicePortfolioResultRepository.GetByTaskIdAsync(taskId);
        return new SolveResultDto(ResultDto.ToDto(resultEntity), resultEntity.Id);
    }
    
    private ResultDto SolveOptimizationTask(string algorithmType, int totalHumanResources,
        List<ProviderDto> providersDto)
    {
        if (!Enum.TryParse<AlgorithmType>(algorithmType, ignoreCase: true, out var resultAlgorithm))
        {
            throw new ArgumentException($"Invalid algorithm type: {algorithmType}");
        }

        var providers = ServiceGroupMetricsCalculator.AnalyzeGroups(providersDto);

        return resultAlgorithm switch
        {
            AlgorithmType.Greedy => SolverGreedyAlgorithm.SolveUsingGreedyAlgorithm(totalHumanResources, providers),
            AlgorithmType.Genetic => SolverGeneticAlgorithm.SolveUsingGeneticAlgorithm(providers, totalHumanResources),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithmType))
        };
    }

    private async Task<DiscResultsDto> SimulateDiscountsAsync(
        TaskDto dto,
        long id,
        DiscountStrategyDelegate discountStrategy,
        string strategyData,
        int startStepIndex = 1,
        int stepsCount = 95,
        double discountStepValue = 0.01)
    {
        var result = await GetSolveAsync(id);

        object data = strategyData switch
        {
            "popular" => result.Result.Portfolio,
            "minimal" => result.Result.ProvidersIncome,
            _ => throw new ArgumentException("Unknown strategy data type", nameof(strategyData))
        };

        var results = new DiscResultsDto
        {
            Results = [new DiscountedResultDto(result.Result, 0)]
        };

        foreach (var discount in Enumerable.Range(startStepIndex, stepsCount).Select(i => i * discountStepValue))
        {
            var newProviders = discountStrategy(dto.Providers, discount, data);
            var solution = SolveOptimizationTask(dto.Algorithm, dto.TotalHumanResource, newProviders.providers);
            results.Results.Add(new DiscountedResultDto(solution, Math.Round(discount, 2)));
            results.Target = newProviders.Target;
        }

        return results;
    }


}
