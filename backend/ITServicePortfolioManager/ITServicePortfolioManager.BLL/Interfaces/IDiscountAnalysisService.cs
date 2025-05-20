using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IDiscountAnalysisService
{
    DiscountDeltaCollectionDto CalculateIncomeChanges(DiscountResultCollectionDto discountResults);
}