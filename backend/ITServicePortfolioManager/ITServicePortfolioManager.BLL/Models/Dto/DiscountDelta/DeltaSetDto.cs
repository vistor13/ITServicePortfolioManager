using ITServicePortfolioManager.BLL.Models.Dto.ResultFormating.WithDiscount;

namespace ITServicePortfolioManager.BLL.Models.Dto.DiscountDelta;

public sealed record DeltaSetDto(
    List<DiscDeltaDto> GeneralDeltas,
    List<DiscDeltaDto> DetailedDeltas,
    DiscountedResultDto DiscountedResultDto,
    DiscountTargetInfo Target
);