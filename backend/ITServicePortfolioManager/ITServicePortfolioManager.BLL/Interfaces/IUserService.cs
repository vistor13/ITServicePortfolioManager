using ITServicePortfolioManager.BLL.Models.Dto.Auth;

namespace ITServicePortfolioManager.BLL.Services;

public interface IUserService
{
    Task Reqister(RegistrationDto registrationDto);
    Task<string> Login(LoginDto loginDto);
}