using ITServicePortfolioManager.Api.Contracts.Request;
using ITServicePortfolioManager.Api.Contracts.Response;
using ITServicePortfolioManager.Api.Contracts.Response.Task;
using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto.Task;
using ITServicePortfolioManager.BLL.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace ITServicePortfolioManager.Api.Endpoints;

public static class ServicePackagesEndpoint
{
    public static void MapServicePackagesEndpoint(this IEndpointRouteBuilder app)
    {
        var endPoints = app.MapGroup("api/service-packages/").WithTags("ServicePackages");

        endPoints.MapPost("create", ServicePortfolioSolver).RequireAuthorization();
        endPoints.MapGet("", GetSolveByIdAsync).RequireAuthorization();
        endPoints.MapGet("by-task", GetSolveByTaskIdAsync).RequireAuthorization();
        endPoints.MapPost("discounts/apply/popular", ApplyDiscountsToPopularServices).RequireAuthorization();
        endPoints.MapPost("discounts/apply/low-income", ApplyDiscountsToLowIncomeProvider).RequireAuthorization();
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
        
        var dto = TaskRequest.MapToDto(request,typeAlgorithm);
        var result = await servicePortfolio.CreateServicePortfoliosAsync(dto,Int64.Parse(userId));
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
        
        var dto = TaskRequest.MapToDto(request,typeAlgorithm);
    
        var result = await servicePortfolio.GetFullSimulation(dto, id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountForPopularServices(providers, discount, (int[,])data),
            "popular"
        );
        return Results.Ok(PopularServicesDeltaResponse.MapToResponse(result));
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
        
        var dto = TaskRequest.MapToDto(request,typeAlgorithm);
        var result = await servicePortfolio.GetFullSimulation(dto, id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountToProviderWithMinimalIncome(providers, discount, (IEnumerable<double>)data),
            "minimal"
        );
        return Results.Ok(LowIncomeDeltaResponse.MapToResponse(result));
    }

    private static async Task<IResult> GetSolveByIdAsync(
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] long idSolve)
    {
        var result = await servicePortfolio.GetSolveAsync(idSolve);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
    private static async Task<IResult> GetSolveByTaskIdAsync(
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] long taskId)
    {
        var result = await servicePortfolio.GetSolveByTaskIdAsync(taskId);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
    
}