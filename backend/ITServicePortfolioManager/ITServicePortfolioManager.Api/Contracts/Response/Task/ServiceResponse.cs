namespace ITServicePortfolioManager.Api.Contracts.Response.Task;

public sealed record ServiceResponse(int Price, int LabourIntensity, List<double> IncomeForProviders, double Discount);