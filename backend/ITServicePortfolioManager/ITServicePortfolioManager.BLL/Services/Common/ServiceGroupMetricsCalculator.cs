using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Services.Common;
public static class ServiceGroupMetricsCalculator
{
    public static List<ProviderGroupStatsDto> AnalyzeGroups(List<ProviderDto> providers)
    {
        return providers
            .Select((provider) =>
            {
                var stats = provider.ServicesGroups
                    .Select(group =>
                    {
                        var totalPrice = group.Sum(s => s.Price);
                        var totalLabour = group.Sum(s => s.LabourIntensity);
                        var totalIncome = group.Sum(s => s.IncomeForProvider);
                        var discount = group.Sum(s => s.Discount)/group.Count;
                        return new ServiceGroupStats(totalPrice, totalLabour, totalIncome, discount);
                    }).ToList();

                return new ProviderGroupStatsDto(stats);
            })
            .ToList();
    }
}
