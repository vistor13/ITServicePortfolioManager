using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public record ResultResponse(long Id , double CompanyIncome, IEnumerable<double> ProvidersIncome, int[][] Portfolio)
{
    public static ResultResponse ToResponse(SolveResultDto dto)
    {
        var rows = dto.Result.Portfolio.GetLength(0);
        var cols = dto.Result.Portfolio.GetLength(1);
        var portfolio = new int[rows][];

        for (var i = 0; i < rows; i++)
        {
            portfolio[i] = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                portfolio[i][j] = dto.Result.Portfolio[i, j];
            }
        }

        return new ResultResponse(dto.Id, dto.Result.CompanyIncome, dto.Result.ProvidersIncome, portfolio);
    }
};
