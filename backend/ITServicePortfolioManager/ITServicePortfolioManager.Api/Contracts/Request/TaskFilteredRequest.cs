using ITServicePortfolioManager.BLL.Models.Dto.Task;

namespace ITServicePortfolioManager.Api.Contracts.Request;

public sealed record TaskFilteredRequest(
    DateTime? FromDate,
    DateTime? ToDate,
    bool? SortDescending,
    string? AlgorithmName)
{
    public static TaskFilterDto ToDto(TaskFilteredRequest request)
    {
        return new TaskFilterDto(request.FromDate, request.ToDate, request.SortDescending, request.AlgorithmName);
    }
}