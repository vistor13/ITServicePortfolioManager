using ITServicePortfolioManager.BLL.Models.Dto.Auth;

namespace ITServicePortfolioManager.Api.Contracts.Request.Auth;

public sealed record LoginRequest(string UserName, string Password)
{
    public static LoginDto MapToDto(LoginRequest request) =>
        new LoginDto(request.UserName, request.Password);
};

