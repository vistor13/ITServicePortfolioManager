namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record ResultWithDiscountResponse(
    double Discount,
    double CompanyIncome,
    List<double> ProvidersIncome,
    int[][] Portfolio
);
