namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscDeltaResponse(
    double Discount,
    double CompanyIncomeDeltaPercent,
    double ProvidersIncomeDeltaPercent,
    double TotalDeltaPercent
);
