namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscountedResultResponse(
    double Discount,
    double CompanyIncome,
    List<double> ProvidersIncome,
    int[][] Portfolio
);
