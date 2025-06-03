namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record ServiceGroupStatsDto(
    double TotalPrice,
    int TotalLabour,
    double TotalIncome,
    double Discount);
