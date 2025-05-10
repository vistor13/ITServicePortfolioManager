namespace ITServicePortfolioManager.Api.Contracts.Request;

public sealed record ServiceRequest(
    int Price, 
    int LabourIntensity, 
    double IncomeForProvider,
    int IndexGroup,
    double Discount);