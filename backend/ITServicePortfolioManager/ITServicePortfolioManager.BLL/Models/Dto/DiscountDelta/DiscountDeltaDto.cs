namespace ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

public sealed record DiscountDeltaDto(
    double Discount,
    double CompanyIncomeDeltaPercent,
    double ProvidersIncomeDeltaPercent,
    double TotalDeltaPercent
);
