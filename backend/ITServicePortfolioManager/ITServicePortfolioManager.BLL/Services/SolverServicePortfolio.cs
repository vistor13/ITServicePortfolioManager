using ITServicePortfolioManager.BLL.Algorithm;
using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models;
using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Services.Common;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.BLL.Services;


public class SolverServicePortfolio(ITaskRepository taskRepository,IServicePortfolioResultRepository servicePortfolioResultRepository) : ISolverServicePortfolio
{
    public async Task<ResultDto> CreateServicePortfoliosAsync(TaskDto dto, string algorithmType)
    {
        if (!Enum.TryParse<AlgorithmType>(algorithmType, ignoreCase: true, out var result))
        {
            throw new ArgumentException($"Invalid algorithm type: {algorithmType}");
        }

        var taskEntity = await taskRepository.Create(TaskDto.ToEntity(dto,algorithmType));
        
        var solution = result switch
        {
            AlgorithmType.Greedy => SolverGreedyAlgorithm.SolveUsingGreedyAlgorithm(dto),
            AlgorithmType.Genetic => SolverGeneticAlgorithm.SolveUsingGeneticAlgorithm(dto.Providers, dto.TotalHumanResource),
            _ => throw new ArgumentOutOfRangeException()
        };
       
        await servicePortfolioResultRepository.Create(ResultDto.ToEntity(solution, taskEntity.Id));
        return solution;
    }

    
    public async Task<DiscountedResultCollectionDto> GetFinalResultWithDiscountForMorePopularServices(TaskDto dto, string algorithmType)
    {
       
        var result = await CreateServicePortfoliosAsync(dto,algorithmType);
        var results = new DiscountedResultCollectionDto
        {
            Results = [new ResultWithDiscountDto(result, 0)]
        };
        
        foreach (var discount in Enumerable.Range(1, 9).Select(i => i * 0.1))
        {
            var newProvider = DiscountStrategyExecutor.AddDiscountForPopularServices(dto.Providers, discount, result.Portfolio);
            var resultDto = await CreateServicePortfoliosAsync(new TaskDto(dto.TotalHumanResource , newProvider), algorithmType);
            results.Results.Add(new ResultWithDiscountDto(resultDto, discount));
        }

        return results;
    }

    public async Task<DiscountedResultCollectionDto> GetFinalResultWithDiscountForProviderWithMinimalIncome(TaskDto dto,string algorithmType)
    {
       
        var result = await CreateServicePortfoliosAsync(dto,algorithmType);
        var results = new DiscountedResultCollectionDto
        {
            Results = [new ResultWithDiscountDto(result, 0)]
        };
        
        foreach (var discount in Enumerable.Range(1, 9).Select(i => i * 0.1))
        {
            var newProvider = DiscountStrategyExecutor.AddDiscountToProviderWithMinimalIncome(dto.Providers, discount, result.ProvidersIncome);
            var resultDto = await CreateServicePortfoliosAsync(new TaskDto(dto.TotalHumanResource , newProvider), algorithmType);
            results.Results.Add(new ResultWithDiscountDto(resultDto, discount));
        }

        return results;
    }

}