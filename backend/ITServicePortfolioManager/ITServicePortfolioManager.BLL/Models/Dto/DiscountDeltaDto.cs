namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record DiscountDeltaDto(
    double Discount,
    double CompanyIncomeDeltaPercent,
    double ProvidersIncomeDeltaPercent,
    double TotalDeltaPercent
);
