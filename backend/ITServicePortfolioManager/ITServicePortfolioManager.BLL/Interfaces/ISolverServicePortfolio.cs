using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface ISolverServicePortfolio
{
    Task<ResultDto> CreateServicePortfoliosAsync(TaskDto dto, string algorithmType);
    Task<DiscountedResultCollectionDto> GetFinalResultWithDiscountForMorePopularServices(TaskDto dto, string algorithmType);
    Task<DiscountedResultCollectionDto> GetFinalResultWithDiscountForProviderWithMinimalIncome(TaskDto dto,string algorithmType);
}