using Microsoft.OpenApi.Models;

namespace ITServicePortfolioManager.Api.Extensions;

public static class SwaggerExtension
{

    public static void AddSwaggerGenServicePortfolio(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Write your JWT",
                    Name = "Authorization",
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http
                });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
    }
}