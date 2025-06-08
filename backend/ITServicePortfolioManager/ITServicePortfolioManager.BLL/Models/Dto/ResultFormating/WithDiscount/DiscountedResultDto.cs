namespace ITServicePortfolioManager.BLL.Models.Dto.ResultFormating.WithDiscount;

public sealed record DiscountedResultDto(ResultDto ResultDto, double Discount);
