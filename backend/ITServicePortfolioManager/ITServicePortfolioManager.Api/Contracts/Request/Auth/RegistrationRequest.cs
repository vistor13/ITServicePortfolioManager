using ITServicePortfolioManager.BLL.Models.Dto.Auth;

namespace ITServicePortfolioManager.Api.Contracts.Request.Auth;

public sealed record RegistrationRequest(string UserName, string Password)
{
    public static RegistrationDto MapToDto(RegistrationRequest request) =>
        new RegistrationDto(request.UserName, request.Password);
};
