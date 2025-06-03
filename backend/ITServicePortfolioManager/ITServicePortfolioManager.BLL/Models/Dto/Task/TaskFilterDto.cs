namespace ITServicePortfolioManager.BLL.Models.Dto.Task;

public sealed record TaskFilterDto(DateTime? FromDate, DateTime? ToDate, bool? SortDescending, string? AlgorithmName);
