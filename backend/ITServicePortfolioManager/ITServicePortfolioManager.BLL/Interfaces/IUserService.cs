using ITServicePortfolioManager.BLL.Models.Dto.Auth;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IUserService
{
    Task Reqister(RegistrationDto registrationDto);
    Task<string> Login(LoginDto loginDto);
}