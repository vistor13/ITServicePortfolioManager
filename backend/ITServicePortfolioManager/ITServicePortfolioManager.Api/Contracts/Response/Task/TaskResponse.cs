using ITServicePortfolioManager.BLL.Models.Dto;

namespace ITServicePortfolioManager.Api.Contracts.Response.Task;

public sealed record TaskResponse(int CountProvider, int TotalHumanResource, List<ServiceGroupResponse> Services, long Id, string Algorithm)
{
    public static TaskResponse MapToResponse(TaskForResponseDto taskDto)
    {
        var groupCount = taskDto.Providers.First().ServicesGroups.Count;
        var providerCount = taskDto.Providers.Count;

        var services = new List<ServiceGroupResponse>();

        for (var groupIndex = 0; groupIndex < groupCount; groupIndex++)
        {
            var serviceCount = taskDto.Providers[0].ServicesGroups[groupIndex].Count;
            var serviceResponses = new List<ServiceResponse>();

            for (var serviceIndex = 0; serviceIndex < serviceCount; serviceIndex++)
            {
                var incomes = new List<double>();
                var price = 0;
                var labour = 0;
                double discount = 0;

                for (var providerIndex = 0; providerIndex < providerCount; providerIndex++)
                {
                    var service = taskDto.Providers[providerIndex].ServicesGroups[groupIndex][serviceIndex];

                    if (providerIndex == 0)
                    {
                        price = service.Price;
                        labour = service.LabourIntensity;
                        discount = service.Discount;
                    }

                    incomes.Add(service.IncomeForProvider);
                }

                serviceResponses.Add(new ServiceResponse(
                    price,
                    labour,
                    incomes,
                    discount
                ));
            }

            services.Add(new ServiceGroupResponse(serviceResponses));
        }

        return new TaskResponse(
            CountProvider: providerCount,
            TotalHumanResource: taskDto.TotalHumanResource,
            Services: services,
            Id: taskDto.Id,
            Algorithm: taskDto.Algorithm
        );
    }

}
