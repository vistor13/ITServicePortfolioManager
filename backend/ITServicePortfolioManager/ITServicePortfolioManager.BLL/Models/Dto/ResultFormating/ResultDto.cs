using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Entities.CommonEntity;

namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record ResultDto(double CompanyIncome, List<double> ProvidersIncome, int[,] Portfolio)
{
    public static ServicePortfolioResultEntity ToEntity(ResultDto dto, long TaskId)
    {
        var providersIncome = dto.ProvidersIncome.ToArray();
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
            ProvidersIncome = providersIncome,
            Portfolio = portfolio
        };

    }
    
    public static ResultDto ToDto(ServicePortfolioResultEntity entity)
    {
        var providersIncome = entity.ProvidersIncome.ToList();

        var maxGroupIndex = entity.Portfolio.Any() ? entity.Portfolio.Max(p => p.GroupIndex) : -1;
        var maxProviderIndex = entity.Portfolio.Any() ? entity.Portfolio.Max(p => p.ProviderIndex) : -1;

        int[,] portfolio;

        if (maxGroupIndex == -1 || maxProviderIndex == -1)
        {
           
            portfolio = new int[0, 0];
        }
        else
        {
            portfolio = new int[maxGroupIndex + 1, maxProviderIndex + 1];
            foreach (var cell in entity.Portfolio)
            {
                portfolio[cell.GroupIndex, cell.ProviderIndex] = cell.Value;
            }
        }

        return new ResultDto(entity.CompanyIncome, providersIncome, portfolio);
    }

};

