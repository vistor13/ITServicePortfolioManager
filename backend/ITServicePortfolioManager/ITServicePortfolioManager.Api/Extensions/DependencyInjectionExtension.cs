using ITServicePortfolioManager.DAL.DataBase;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.Api.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration["Database:ConnectionString"],
                b => b.MigrationsAssembly("ITServicePortfolioManager.DAL"));
        });
        return services;
    }
}