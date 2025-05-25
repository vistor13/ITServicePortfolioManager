using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Response;
public sealed record DiscountDeltaLowIncomeResponse(
    List<DiscountDeltaResponse> GeneralDeltas,
    List<DiscountDeltaResponse> DetailedDeltas,
    ResultWithDiscountResponse? BestResult,
    int? IndexProvider)
{
    public static DiscountDeltaLowIncomeResponse MapToResponse(CombinedDiscountDeltaDto dto)
    {
        return new DiscountDeltaLowIncomeResponse(
            dto.GeneralDeltas.Select(MapOne).ToList(),
            dto.DetailedDeltas.Select(MapOne).ToList(),
            dto.ResultDto is not null ? MapBest(dto.ResultDto) : null,
            IndexProvider: dto.Target?.ProviderIndex ?? new int()
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
