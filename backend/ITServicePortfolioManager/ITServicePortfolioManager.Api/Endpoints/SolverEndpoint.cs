using ITServicePortfolioManager.Api.Contracts.Request;
using ITServicePortfolioManager.Api.Contracts.Response;
using ITServicePortfolioManager.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITServicePortfolioManager.Api.Endpoints;

public static class SolverEndpoint
{
    public static void MapSolverEndpoint(this IEndpointRouteBuilder app)
    {
        var endPoints = app.MapGroup("api/portfolio/").WithTags("Solver");
        endPoints.MapPost("solve", ServicePortfolioSolver
        );
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
        [FromQuery] string typeAlgorithm)
    {
        var dto = TaskRequest.MapToDto(request);
        var result = await servicePortfolio.GetFinalResultWithDiscountForMorePopularServices(dto, typeAlgorithm);
        return Results.Ok(DiscountResultCollectionResponse.ToResponse(result));
    }
    
    private static async Task<IResult> ApplyDiscountsToLowIncomeProvider(
        [FromBody] TaskRequest request,
        [FromServices] ISolverServicePortfolio servicePortfolio,
        [FromQuery] string typeAlgorithm)
    {
        var dto = TaskRequest.MapToDto(request);
        var result = await servicePortfolio.GetFinalResultWithDiscountForProviderWithMinimalIncome(dto, typeAlgorithm);
        return Results.Ok(DiscountResultCollectionResponse.ToResponse(result));
    }
}