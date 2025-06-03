using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscountDeltaPopularServicesResponse(List<DiscountDeltaResponse> GeneralDeltas,
List<DiscountDeltaResponse> DetailedDeltas,
ResultWithDiscountResponse? BestResult,
List<int>? indexesServices)
{
    public static DiscountDeltaPopularServicesResponse MapToResponse(CombinedDiscountDeltaDto dto)
    {
        return new DiscountDeltaPopularServicesResponse(
            dto.GeneralDeltas.Select(MapOne).ToList(),
            dto.DetailedDeltas.Select(MapOne).ToList(),
            dto.ResultDto is not null ? MapBest(dto.ResultDto) : null,
            indexesServices: dto.Target?.GroupIndexes ?? new List<int>()
        );
    }

    private static DiscountDeltaResponse MapOne(DiscountDeltaDto d) =>
        new(
            d.Discount,
            d.CompanyIncomeDeltaPercent,
            d.ProvidersIncomeDeltaPercent,
            d.TotalDeltaPercent
        );

    private static ResultWithDiscountResponse MapBest(ResultWithDiscountDto dto) =>
        new(
            dto.Discount,
            dto.ResultDto.CompanyIncome,
            dto.ResultDto.ProvidersIncome,
            ConvertMatrix(dto.ResultDto.Portfolio)
        );

    private static int[][] ConvertMatrix(int[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var result = new int[rows][];
        for (var i = 0; i < rows; i++)
        {
            result[i] = new int[cols];
            for (var j = 0; j < cols; j++)
            {
                result[i][j] = matrix[i, j];
            }
        }
        return result;
    }
}
