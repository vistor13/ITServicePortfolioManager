namespace ITServicePortfolioManager.BLL.Models.Dto;

public class DiscountResultCollectionDto
{ 
    public List<ResultWithDiscountDto> Results { get; set; }
    
    public DiscountTargetInfo Target { get; set; }
}