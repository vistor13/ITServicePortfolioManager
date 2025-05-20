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
        endPoints.MapPost("solve", ServicePortfolioSolver);
        endPoints.MapGet("getSolve", GetSolve);
        endPoints.MapPost("apply-discounts/popular", ApplyDiscountsToPopularServices);
        endPoints.MapPost("apply-discounts/low-income", ApplyDiscountsToLowIncomeProvider);
        
    }

    private static async Task<IResult> ServicePortfolioSolver
    (
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm)
    {
        var dto = TaskRequest.MapToDto(request);
        var result = await servicePortfolio.CreateServicePortfoliosAsync(dto, typeAlgorithm);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
    
    private static async Task<IResult> ApplyDiscountsToPopularServices(
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm,
        [FromQuery] long id)
    {
        var dto = TaskRequest.MapToDto(request);
    
        var result = await servicePortfolio.GetGeneralAndDetailedSimulation(
            dto,
            typeAlgorithm,
            id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountForPopularServices(providers, discount, (int[,])data),
            "popular"
        );
        return Results.Ok(CombinedDiscountDeltaResponse.MapToResponse(result));
    }
    
    private static async Task<IResult> ApplyDiscountsToLowIncomeProvider(
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm,
        [FromQuery] long id)
    {
        var dto = TaskRequest.MapToDto(request);
        var result = await servicePortfolio.GetGeneralAndDetailedSimulation(
            dto,
            typeAlgorithm,
            id,
            (providers, discount, data) =>
                DiscountStrategyExecutor.AddDiscountToProviderWithMinimalIncome(providers, discount, (IEnumerable<double>)data),
            "minimal"
        );
        return Results.Ok(CombinedDiscountDeltaResponse.MapToResponse(result));
    }

    private static async Task<IResult> GetSolve(
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] long idSolve)
    {
        var result = await servicePortfolio.GetSolveAsync(idSolve);
        return Results.Ok(ResultResponse.ToResponse(result));
    }
}