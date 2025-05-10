using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Request;

public sealed record TaskRequest(
    int CountProvider,
    int TotalHumanResource,
    List<ServiceRequest> Services)
{
    public static TaskDto MapToDto(TaskRequest request)
    {
        var providers = new List<ProviderDto>();

        var grouped = request.Services
            .GroupBy(s => s.IndexGroup)
            .OrderBy(g => g.Key)
            .Select(g => g.Select(s => new ServiceDto(
             s.Price, s.LabourIntensity,  s.IncomeForProvider,s.Discount
            )).ToList())
            .ToList();

        for (var i = 0; i < request.CountProvider; i++)
        {
            var provider = new ProviderDto(grouped);
            providers.Add(provider);
        }

        return new TaskDto(request.TotalHumanResource, providers);
    }
};