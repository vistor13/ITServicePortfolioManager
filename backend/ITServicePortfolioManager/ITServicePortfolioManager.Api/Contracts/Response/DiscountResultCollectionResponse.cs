using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record DiscountResultCollectionResponse(List<ResultWithDiscountResponse> Result)
{
    public static DiscountResultCollectionResponse ToResponse(DiscountedResultCollectionDto dto)
    {
        var resultResponses = dto.Results
            .Select(ResultWithDiscountResponse.ToResponse)
            .ToList();

        return new DiscountResultCollectionResponse(resultResponses);
    }
};
