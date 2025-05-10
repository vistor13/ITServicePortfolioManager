using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Entities.CommonEntity;

namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record ResultDto(double CompanyIncome, IEnumerable<double> ProvidersIncome, int[,] Portfolio)
{
    public static ServicePortfolioResultEntity ToEntity(ResultDto dto, long TaskId)
    {
        var incomeProviderEntity = dto.ProvidersIncome.Select(income => new ProviderIncomeEntity() { Income = income }).ToList();
        var portfolio = Enumerable.Range(0, dto.Portfolio.GetLength(0))
            .SelectMany(row => Enumerable.Range(0, dto.Portfolio.GetLength(1))
                .Select(col => new PortfolioCellEntity
                {
                    GroupIndex = row,
                    ProviderIndex = col,
                    Value = dto.Portfolio[row, col]
                }))
            .ToList();
        return new ServicePortfolioResultEntity()
        {
            CompanyIncome = dto.CompanyIncome,
            TaskId = TaskId,
            ProvidersIncome = incomeProviderEntity,
            Portfolio = portfolio
        };

    }
};
