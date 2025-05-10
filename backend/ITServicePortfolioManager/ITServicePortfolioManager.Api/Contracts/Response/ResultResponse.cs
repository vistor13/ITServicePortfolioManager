using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public record ResultResponse(double CompanyIncome, IEnumerable<double> ProvidersIncome, int[][] Portfolio)
{
    public static ResultResponse ToResponse(ResultDto dto)
    {
        var rows = dto.Portfolio.GetLength(0);
        var cols = dto.Portfolio.GetLength(1);
        var portfolio = new int[rows][];

        for (var i = 0; i < rows; i++)
        {
            portfolio[i] = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                portfolio[i][j] = dto.Portfolio[i, j];
            }
        }

        return new ResultResponse(dto.CompanyIncome, dto.ProvidersIncome, portfolio);
    }
};
