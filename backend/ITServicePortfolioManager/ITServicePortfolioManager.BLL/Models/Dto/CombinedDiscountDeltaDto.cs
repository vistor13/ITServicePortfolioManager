namespace ITServicePortfolioManager.BLL.Models.Dto;

public sealed record CombinedDiscountDeltaDto(
    List<DiscountDeltaDto> GeneralDeltas,
    List<DiscountDeltaDto> DetailedDeltas,
    ResultWithDiscountDto ResultDto,
    DiscountTargetInfo Target
);