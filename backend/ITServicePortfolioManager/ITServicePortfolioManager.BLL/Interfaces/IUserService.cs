using ErrorOr;
using ITServicePortfolioManager.BLL.Models.Dto.Auth;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IUserService
{
    Task<ErrorOr<Success>> Reqister(RegistrationDto registrationDto);
    Task<ErrorOr<string>> Login(LoginDto loginDto);
}