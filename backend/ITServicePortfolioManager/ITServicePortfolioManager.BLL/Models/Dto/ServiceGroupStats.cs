namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record ServiceGroupStats(
    double TotalPrice,
    int TotalLabour,
    double TotalIncome,
    double Discount);
