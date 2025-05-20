using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Services;
using ITServicePortfolioManager.BLL.Services.Common;
using Microsoft.Extensions.DependencyInjection;

namespace ITServicePortfolioManager.BLL;

public static class DependencyInjectionExtension
{
    public static void AddBLLLayer(this IServiceCollection services)
    {
        services.AddScoped<ISolverServicePortfolio, SolverServicePortfolio>();
        services.AddScoped<IDiscountAnalysisService, DiscountAnalysisService>();
    }
}