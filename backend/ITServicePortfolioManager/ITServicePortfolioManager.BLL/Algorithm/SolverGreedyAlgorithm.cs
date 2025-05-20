using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Algorithm;

public static class SolverGreedyAlgorithm
{
    public static ResultDto SolveUsingGreedyAlgorithm(int totalHumanResource, List<ProviderGroupStatsDto> providers)
    {
        var portfolio = AddTheBest(providers, totalHumanResource);
        var incomeCompany = CalculateIncomeCompany(providers , portfolio);
        var incomeProviders = CalculateIncomeProviders(providers , portfolio);
        return new ResultDto(incomeCompany, incomeProviders, portfolio);
    }

    private static int[,] AddTheBest(List<ProviderGroupStatsDto> providers, int totalHumanResource)
    {
        var numberGroups = providers[0].GroupStats.Count;
        var numberProviders = providers.Count;
        var eta = CalculateEta(providers);
        var portfolio = new int[numberGroups, numberProviders];
        var usedResources = 0;

        var flattened = providers
            .SelectMany((provider, i) => provider.GroupStats
                .Select((group, j) => new { GroupIndex = j, ProviderIndex = i, EtaValue = eta[i][j], LabourIntensity = group.TotalLabour }))
            .OrderByDescending(x => x.EtaValue)
            .ToList();

        foreach (var item in flattened
                     .Where(item => totalHumanResource - usedResources >= item.LabourIntensity))
        {
            portfolio[item.GroupIndex, item.ProviderIndex] = 1;
            usedResources += item.LabourIntensity;
        }

        return portfolio;
    }
    public static double CalculateIncomeCompany(List<ProviderGroupStatsDto> providers, int[,] portfolio) =>
     providers.SelectMany((provider, i) => provider.GroupStats
                .Select((group, j) =>
                    group.TotalPrice * (1 - group.Discount) * portfolio[j,i])).Sum();
    
    public static List<double> CalculateIncomeProviders(List<ProviderGroupStatsDto> providers, int[,] portfolio) =>
        providers.Select((provider, i) => provider.GroupStats.Select((serviceGroup, j) => (serviceGroup.TotalIncome - serviceGroup.TotalPrice * (1 - serviceGroup.Discount)) * portfolio[j, i]).Sum()).ToList();


    private static List<List<double>> TotalPrice(List<ProviderGroupStatsDto> providers) 
        => providers.Select(provider => provider.GroupStats.Select(group => group.TotalPrice * (1 - group.Discount)).ToList()).ToList();
    

    public static List<List<double>> CalculateEta(List<ProviderGroupStatsDto> providers)
    {
        var priceInGroups = TotalPrice(providers);
        return providers
            .Select((provider, i) => provider.GroupStats
                .Select((serviceGroup, j) => priceInGroups[i][j] / providers[i].GroupStats[j].TotalLabour)
                .ToList()).ToList();
    }

}