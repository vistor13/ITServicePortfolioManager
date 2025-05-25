using System.Security.Claims;
using ITServicePortfolioManager.Api.Contracts.Request;
using ITServicePortfolioManager.Api.Contracts.Response;
using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace ITServicePortfolioManager.Api.Endpoints;

public static class SolverEndpoint
{
    public static void MapSolverEndpoint(this IEndpointRouteBuilder app)
    {
        var endPoints = app.MapGroup("api/portfolio/").WithTags("Solver");
        endPoints.MapPost("solve", ServicePortfolioSolver).RequireAuthorization();
        endPoints.MapGet("getSolve", GetSolve).RequireAuthorization();
        endPoints.MapPost("apply-discounts/popular", ApplyDiscountsToPopularServices).RequireAuthorization();
        endPoints.MapPost("apply-discounts/low-income", ApplyDiscountsToLowIncomeProvider).RequireAuthorization();
        
    }

    private static async Task<IResult> ServicePortfolioSolver
    (
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm,
        HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        
        var dto = TaskRequest.MapToDto(request, Int64.Parse(userId));
        var result = await servicePortfolio.CreateServicePortfoliosAsync(dto, typeAlgorithm);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
    
    private static async Task<IResult> ApplyDiscountsToPopularServices(
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm,
        [FromQuery] long id,
        HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        var dto = TaskRequest.MapToDto(request,Int64.Parse(userId));
    
        var result = await servicePortfolio.GetGeneralAndDetailedSimulation(
            dto,
            typeAlgorithm,
            id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountForPopularServices(providers, discount, (int[,])data),
            "popular"
        );
        return Results.Ok(DiscountDeltaPopularServicesResponse.MapToResponse(result));
    }
    
    private static async Task<IResult> ApplyDiscountsToLowIncomeProvider(
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm,
        [FromQuery] long id,
        HttpContext httpContext)
    {
        var userId = httpContext.User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Results.Unauthorized();
        var dto = TaskRequest.MapToDto(request,Int64.Parse(userId));
        var result = await servicePortfolio.GetGeneralAndDetailedSimulation(
            dto,
            typeAlgorithm,
            id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountToProviderWithMinimalIncome(providers, discount, (IEnumerable<double>)data),
            "minimal"
        );
        return Results.Ok(DiscountDeltaLowIncomeResponse.MapToResponse(result));
    }

    private static async Task<IResult> GetSolve(
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] long idSolve)
    {
        var result = await servicePortfolio.GetSolveAsync(idSolve);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
}