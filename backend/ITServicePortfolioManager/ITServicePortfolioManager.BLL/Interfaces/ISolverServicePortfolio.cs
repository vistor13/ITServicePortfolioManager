using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;
using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating;
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ITServicePortfolioManager.BLL.Services;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ISolverServicePortfolio
{
    Task<SolveResultDto> CreateServicePortfoliosAsync(TaskDto dto, long UserId);

    Task<DeltaSetDto> GetFullSimulation(TaskDto dto, long id,
        SolverPortfolioService.DiscountStrategyDelegate discountStrategy, string strategyData);

    Task<SolveResultDto> GetSolveAsync(long id);
    Task<SolveResultDto> GetSolveByTaskIdAsync(long taskId);
}