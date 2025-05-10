using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Response;

public sealed record ResultWithDiscountResponse(ResultResponse Result, double Discount)
{
    public static ResultWithDiscountResponse ToResponse(ResultWithDiscountDto dto)
    {
        return new ResultWithDiscountResponse(
            ResultResponse.ToResponse(dto.ResultDto),
            dto.Discount
        );
    }
}
