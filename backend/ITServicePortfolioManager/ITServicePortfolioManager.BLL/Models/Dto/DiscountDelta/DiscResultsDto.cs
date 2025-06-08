using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating.WithDiscount;

namespace ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

public class DiscResultsDto
{ 
    public List<DiscountedResultDto> Results { get; set; }
    
    public DiscountTargetInfo Target { get; set; }
}