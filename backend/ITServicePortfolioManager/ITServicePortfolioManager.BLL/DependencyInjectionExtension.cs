using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto.Auth;
using ITServicePortfolioManager.BLL.Services;
using ITServicePortfolioManager.BLL.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITServicePortfolioManager.BLL;

public static class DependencyInjectionExtension
{
    public static void AddBLLLayer(this IServiceCollection services)
    {
        services.AddScoped<ISolverServicePortfolio, SolverPortfolioService>();
        services.AddScoped<IDiscountAnalysisService, DiscountAnalysisService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
    }
    
    public static void ConfigureAuth0(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection("Jwt"));
    }
}