using ITServicePortfolioManager.DAL.DataBase.Repository;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ITServicePortfolioManager.DAL;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IServicePortfolioResultRepository, ServicePortfolioResultRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}