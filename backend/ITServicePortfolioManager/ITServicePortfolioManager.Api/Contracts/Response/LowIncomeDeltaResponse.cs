using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;
using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating.WithDiscount;

namespace ITServicePortfolioManager.Api.Contracts.Response;
public sealed record LowIncomeDeltaResponse(
    List<DiscDeltaResponse> GeneralDeltas,
    List<DiscDeltaResponse> DetailedDeltas,
    DiscountedResultResponse? BestResult,
    int? IndexProvider)
{
    public static LowIncomeDeltaResponse MapToResponse(DeltaSetDto setDto)
    {
        return new LowIncomeDeltaResponse(
            setDto.GeneralDeltas.Select(MapOne).ToList(),
            setDto.DetailedDeltas.Select(MapOne).ToList(),
            setDto.DiscountedResultDto is not null ? MapBest(setDto.DiscountedResultDto) : null,
            IndexProvider: setDto.Target?.ProviderIndex ?? new int()
        );
    }

    private static DiscDeltaResponse MapOne(DiscDeltaDto d) =>
        new(
            d.Discount,
            d.CompanyIncomeDeltaPercent,
            d.ProvidersIncomeDeltaPercent,
            d.TotalDeltaPercent
        );

    private static DiscountedResultResponse MapBest(BLL.Models.Dto.ResultFormating.WithDiscount.DiscountedResultDto dto) =>
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
