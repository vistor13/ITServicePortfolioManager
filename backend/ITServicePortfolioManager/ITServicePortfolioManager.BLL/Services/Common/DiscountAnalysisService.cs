using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

namespace ITServicePortfolioManager.BLL.Services.Common;

public class DiscountAnalysisService : IDiscountAnalysisService
{
    public DiscountDeltaCollectionDto CalculateIncomeChanges(DiscountResultCollectionDto discountResults)
    {
        var baseResult = discountResults.Results[0].ResultDto;

        var baseCompanyIncome = baseResult.CompanyIncome;
        var baseProvidersIncomeSum = baseResult.ProvidersIncome.Sum();

        var deltas = 
            (from item in discountResults.Results 
                let currentResult = item.ResultDto 
                let currentCompanyIncome = currentResult.CompanyIncome 
                let currentProvidersIncomeSum = currentResult.ProvidersIncome.Sum() 
                let companyIncomeDeltaPercent = (currentCompanyIncome - baseCompanyIncome) / baseCompanyIncome * 100
                let providersIncomeDeltaPercent = (currentProvidersIncomeSum - baseProvidersIncomeSum) / baseProvidersIncomeSum * 100 
                let totalDeltaPercent = companyIncomeDeltaPercent + providersIncomeDeltaPercent 
                select new DiscountDeltaDto(
                    Discount: item.Discount,
                    CompanyIncomeDeltaPercent: companyIncomeDeltaPercent, 
                    ProvidersIncomeDeltaPercent: providersIncomeDeltaPercent,
                    TotalDeltaPercent: totalDeltaPercent)).ToList();

        return new DiscountDeltaCollectionDto(deltas);
    }

    private List<List<double>> CalculateProviderIncomeChangesByIndex(DiscountResultCollectionDto discountResults)
    {
        var providerIncomeChangesByIndex = new List<List<double>>();

        var baseProvidersIncome = discountResults.Results[0].ResultDto.ProvidersIncome;

        foreach (var item in discountResults.Results)
        {
            var currentProvidersIncome = item.ResultDto.ProvidersIncome;
            var changesForCurrentResult = new List<double>();

            for (var i = 0; i < currentProvidersIncome.Count; i++)
            {
                var baseValue = baseProvidersIncome[i];
                var currentValue = currentProvidersIncome[i];

                var percentChange = (currentValue - baseValue) / baseValue * 100;
                changesForCurrentResult.Add(percentChange);
            }

            providerIncomeChangesByIndex.Add(changesForCurrentResult);
        }

        return providerIncomeChangesByIndex;
    }
}