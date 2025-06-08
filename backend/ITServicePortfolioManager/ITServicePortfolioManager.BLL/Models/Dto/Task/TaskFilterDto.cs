namespace ITServicePortfolioManager.BLL.Models.Dto.Task;

public sealed record TaskFilterDto(long UserId,DateTime? FromDate, DateTime? ToDate, bool? SortDescending, string? AlgorithmName);
