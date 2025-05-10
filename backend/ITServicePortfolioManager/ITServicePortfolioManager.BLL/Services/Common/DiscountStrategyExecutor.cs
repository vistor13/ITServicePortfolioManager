using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.BLL.Services.Common
{
    public static class DiscountStrategyExecutor
    {
        private const int RowDimension = 0; 
        private const int ColomnDimension = 1;  
        public static List<ProviderDto> AddDiscountToProviderWithMinimalIncome(List<ProviderDto> providers, double discount, IEnumerable<double> income)
        {
            var targetIncomeIndex = income
                .Select((value, index) => new { Value = value, Index = index })
                .OrderBy(x => x.Value)
                .First().Index;
        
            providers[targetIncomeIndex].ServicesGroups
                .SelectMany(group => group)
                .ToList()
                .ForEach(service => service.Discount = discount);

            return providers;
        }

        public static List<ProviderDto> AddDiscountForPopularServices(List<ProviderDto> providers, double discount, int[,] portfolio)
        {
            var group = portfolio.GetLength(RowDimension);
            var provider = portfolio.GetLength(ColomnDimension);
        
            var countService = Enumerable.Range(0, group)
                .Select(i => Enumerable.Range(0, provider).Count(j => portfolio[i, j] == 1))
                .ToList();

            var targetIncomeIndexes = countService
                .Select((value, index) => new { Value = value, Index = index })
                .Where(x => x.Value == countService.Max())
                .Select(x => x.Index)
                .ToList();

            providers
                .SelectMany(provider => 
                    targetIncomeIndexes
                        .Where(groupIndex => groupIndex < provider.ServicesGroups.Count)
                        .SelectMany(groupIndex => provider.ServicesGroups[groupIndex])
                )
                .ToList()
                .ForEach(service => service.Discount = discount);
        
            return providers;
        }
    }
}