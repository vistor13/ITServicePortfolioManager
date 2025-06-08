using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IDiscountAnalysisService
{
    List<DiscDeltaDto>  CalculateIncomeChanges(DiscResultsDto discountResults);
}