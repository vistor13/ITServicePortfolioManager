using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IDiscountAnalysisService
{
    DiscountDeltaCollectionDto CalculateIncomeChanges(DiscountResultCollectionDto discountResults);
}