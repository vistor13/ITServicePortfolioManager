namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscountDeltaResponse(
    double Discount,
    double CompanyIncomeDeltaPercent,
    double ProvidersIncomeDeltaPercent,
    double TotalDeltaPercent
);
