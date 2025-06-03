using ITServicePortfolioManager.BLL.Models.Dto;
using ITServicePortfolioManager.BLL.Models.Dto.Task;

namespace ITServicePortfolioManager.Api.Contracts.Request;

public sealed record TaskRequest(int CountProvider, int TotalHumanResource, List<ServiceGroupRequest> Services)
{
    public static TaskDto MapToDto(TaskRequest request, long UserId, string AlgrithmType)
    {
        var providers = new List<ProviderDto>();

        for (var i = 0; i < request.CountProvider; i++)
        {
            var groupedServiceDtos = request.Services
                .Select(group => group.Services
                    .Select(service => new ServiceDto(
                        service.Price,
                        service.LabourIntensity,
                        service.IncomeForProviders[i],
                        service.Discount))
                    .ToList())
                .ToList();

            providers.Add(new ProviderDto(groupedServiceDtos));
        }

        return new TaskDto(request.TotalHumanResource, providers,UserId,AlgrithmType);
    }
}
