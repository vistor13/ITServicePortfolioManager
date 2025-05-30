using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Services;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ISolverServicePortfolio
{
    Task<SolveResultDto> CreateServicePortfoliosAsync(TaskDto dto);
    Task<CombinedDiscountDeltaDto> GetGeneralAndDetailedSimulation(TaskDto dto, long id, SolverPortfolioService.DiscountStrategyDelegate discountStrategy, string strategyData);
    Task<SolveResultDto> GetSolveAsync(long id); 
    Task<List<TaskForResponseDto>> GetTasksByUserIdAsync(long UserId); 
    Task<SolveResultDto> GetSolveByTaskIdAsync(long taskId);
}