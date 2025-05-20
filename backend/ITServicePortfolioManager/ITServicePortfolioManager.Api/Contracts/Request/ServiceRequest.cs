namespace ITServicePortfolioManager.Api.Contracts.Request;

public sealed record ServiceRequest(
    int Price, 
    int LabourIntensity, 
    List<double> IncomeForProviders,
    double Discount);