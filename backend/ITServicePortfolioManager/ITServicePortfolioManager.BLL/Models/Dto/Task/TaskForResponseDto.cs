using ITServicePortfolioManager.BLL.Models.Dto.Service;
using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.BLL.Models.Dto.Task;

public sealed record TaskForResponseDto(
    int TotalHumanResource,
    List<ProviderDto> Providers,
    string Algorithm, 
    long Id)
{
    public static TaskForResponseDto ToDto(TaskEntity taskEntity)
    {
        var groupedByProvider = taskEntity.Groups
            .GroupBy(g => g.IndexProvider)
            .Select(group => new ProviderDto(
                group.Select(g => g.Services.Select(service =>
                        new ServiceDto(
                            service.Price,
                            service.LabourIntensity,
                            service.IncomeForProvider,
                            service.Discount
                        )).ToList()
                ).ToList()
            )).ToList();


        return new TaskForResponseDto( taskEntity.TotalHumanResource,groupedByProvider,taskEntity.NameAlgorithm,taskEntity.Id);
    }
}