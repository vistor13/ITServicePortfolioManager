using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Algorithm;

public static class SolverGreedyAlgorithm
{
    public static ResultDto SolveUsingGreedyAlgorithm(TaskDto dto)
    {
        var portfolio = AddTheBest(dto.Providers, dto.TotalHumanResource);
        var incomeCompany = CalculateIncomeCompany(dto.Providers, portfolio);
        var incomeProviders = CalculateIncomeProviders(dto.Providers, portfolio);
        return new ResultDto(incomeCompany, incomeProviders, portfolio);
    }

    private static int[,] AddTheBest(List<ProviderDto> providers, int totalHumanResource)
    {
        var numberGroups = providers[0].ServicesGroups.Count;
        var numberProviders = providers.Count;
        var eta = CalculateEta(providers);
        var portfolio = new int[numberGroups, numberProviders];
        var usedResources = 0;

        var flattened = providers
            .SelectMany((provider, i) => provider.ServicesGroups
                .Select((group, j) => new { GroupIndex = j, ProviderIndex = i, EtaValue = eta[i][j], LabourIntensity = group.Sum(x => x.LabourIntensity) }))
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
    public static double CalculateIncomeCompany(List<ProviderDto> providers, int[,] portfolio) =>
     providers.SelectMany((provider, i) => provider.ServicesGroups
                .SelectMany((group, j) =>
                    group.Select(service => service.Price * (1 - service.Discount) * portfolio[j,i]))).Sum();
    
    public static List<double> CalculateIncomeProviders(List<ProviderDto> providers, int[,] portfolio) =>
        providers.Select((provider, i) => 
            provider.ServicesGroups.SelectMany((serviceGroup, j) => 
                    serviceGroup.Select(service => 
                            (service.IncomeForProvider - service.Price * (1 - service.Discount)) * portfolio[j, i])).Sum()).ToList();


    private static List<List<double>> TotalPrice(List<ProviderDto> providers)=>
        providers.Select(provider => provider.ServicesGroups
            .Select(group => group.Sum(service => service.Price * (1 - service.Discount)))
            .ToList()).ToList();

    public static List<List<int>> TotalLabourIntensity(List<ProviderDto> providers) =>
        providers.Select(provider => provider.ServicesGroups
            .Select(group => group.Sum(service => service.LabourIntensity))
            .ToList()).ToList();

    public static List<List<double>> CalculateEta(List<ProviderDto> providers)
    {
        var priceInGroups = TotalPrice(providers);
        var laboursInGroups = TotalLabourIntensity(providers);
        return providers
            .Select((provider, i) => provider.ServicesGroups
                .Select((serviceGroup, j) => priceInGroups[i][j] / laboursInGroups[i][j])
                .ToList()).ToList();
    }

}