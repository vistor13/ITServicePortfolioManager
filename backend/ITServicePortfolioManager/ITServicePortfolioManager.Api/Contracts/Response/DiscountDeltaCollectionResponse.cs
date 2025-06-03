using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscountDeltaCollectionResponse(
    List<DiscountDeltaResponse> Deltas
){
    public static DiscountDeltaCollectionResponse MapToResponse(DiscountDeltaCollectionDto dto)
    {
        var responses = dto.Deltas
            .Select(d => new DiscountDeltaResponse(
                d.Discount,
                d.CompanyIncomeDeltaPercent,
                d.ProvidersIncomeDeltaPercent,
                d.TotalDeltaPercent
            ))
            .ToList();

        return new DiscountDeltaCollectionResponse(responses);
    }

}