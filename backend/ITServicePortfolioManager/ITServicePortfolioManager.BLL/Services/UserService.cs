using ITServicePortfolioManager.BLL.Models.Dto.Auth;
using ITServicePortfolioManager.BLL.Services.Common;
using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.BLL.Services;

public class UserService(IUserRepository userRepository, IJwtService jwtService) : IUserService
{
    public async Task Reqister(RegistrationDto registrationDto)
    {
        var entity = await userRepository.GetByUserName(registrationDto.UserName);
        if (entity is not null)
        {
             throw new Exception("This UserName register");
        }
        var hashedPassword = PasswordHashProvider.GenerateHashPassword(registrationDto.Password);
        var userEntity = new UserEntity()
        {
            UserName = registrationDto.UserName,
            HashedPassword = hashedPassword
        };

        await userRepository.Create(userEntity);
    }

    public async Task<string> Login(LoginDto loginDto)
    {
        var entity = await userRepository.GetByUserName(loginDto.UserName);
        if (!PasswordHashProvider.Verify(loginDto.Password, entity.HashedPassword))
        {
            throw new Exception("Password dont correct");
        };
       return jwtService.GenerateToken(entity);
    }
}