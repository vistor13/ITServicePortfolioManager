using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Entities.CommonEntity;

namespace ITServicePortfolioManager.BLL.Models.Dto.Task;

public sealed record TaskDto(
    int TotalHumanResource,
    List<ProviderDto> Providers,
    string Algorithm)
{
  
    public static TaskEntity ToEntity(TaskDto taskDto, long UserId)
    {
        var groupServices = taskDto.Providers
            .SelectMany((provider, index) =>
                provider.ServicesGroups.Select(group =>
                    new GroupServiceEntity
                    {
                        Services = group.Select(service => new ServiceEntity
                        {
                            Discount = service.Discount,
                            IncomeForProvider = service.IncomeForProvider,
                            LabourIntensity = service.LabourIntensity,
                            Price = service.Price
                        }).ToList(),
                        IndexProvider = index
                    })).ToList();

        return new TaskEntity
        {
            CountProvider = taskDto.Providers.Count,
            Groups = groupServices,
            TotalHumanResource = taskDto.TotalHumanResource,
            NameAlgorithm = taskDto.Algorithm,
            UserId = UserId
        };
    }
};