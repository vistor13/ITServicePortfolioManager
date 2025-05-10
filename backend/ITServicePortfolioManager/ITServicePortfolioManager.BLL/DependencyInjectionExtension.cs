using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ITServicePortfolioManager.BLL;

public static class DependencyInjectionExtension
{
    public static void AddBLLLayer(this IServiceCollection services)
    {
        services.AddScoped<ISolverServicePortfolio, SolverServicePortfolio>();
    }
}