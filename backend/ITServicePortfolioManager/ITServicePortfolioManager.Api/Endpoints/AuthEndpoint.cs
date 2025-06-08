using ITServicePortfolioManager.Api.Contracts.Request.Auth;
using ITServicePortfolioManager.Api.Extensions;
using ITServicePortfolioManager.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ITServicePortfolioManager.Api.Endpoints;

public static class AuthEndpoint
{
    public static void MapAuthEndpoint(this IEndpointRouteBuilder app)
    {
        var endPoints = app.MapGroup("api/auth/").WithTags("Auth");
        endPoints.MapPost("login", Login);
        endPoints.MapPost("register", Register);
        
    }
    private static async Task<IResult> Login
    (
        [FromBody] LoginRequest request,
        [FromServices] IUserService userService)
    {
        var result = await userService.Login(LoginRequest.MapToDto(request));
        return result.ToResult();
    }
    private static async Task<IResult> Register
    (
        [FromBody] RegistrationRequest request,
        [FromServices] IUserService userService)
    {
        var result = await userService.Reqister(RegistrationRequest.MapToDto(request));
        return result.ToResult();
    }
}