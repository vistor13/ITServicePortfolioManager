using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Services;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ISolverServicePortfolio
{
    Task<SolveResultDto> CreateServicePortfoliosAsync(TaskDto dto, string algorithmType);

    Task<CombinedDiscountDeltaDto> GetGeneralAndDetailedSimulation(
        TaskDto dto,
        string algorithmType,
        long id,
        SolverServicePortfolio.DiscountStrategyDelegate discountStrategy,
        string strategyData);
    
    Task<SolveResultDto> GetSolveAsync(long id);
}