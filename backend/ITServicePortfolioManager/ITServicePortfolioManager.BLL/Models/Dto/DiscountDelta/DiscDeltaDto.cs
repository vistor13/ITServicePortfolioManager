namespace ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

public sealed record DiscDeltaDto(
    double Discount,
    double CompanyIncomeDeltaPercent,
    double ProvidersIncomeDeltaPercent,
    double TotalDeltaPercent
);
