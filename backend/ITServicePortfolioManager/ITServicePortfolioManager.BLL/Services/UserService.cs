using ITServicePortfolioManager.BLL.Interfaces;
using ITServicePortfolioManager.BLL.Models.Dto.Auth;
using ITServicePortfolioManager.BLL.Services.Common;
using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;
using ErrorOr;

namespace ITServicePortfolioManager.BLL.Services;

public class UserService(IUserRepository userRepository, IJwtService jwtService) : IUserService
{
    public async Task<ErrorOr<Success>> Reqister(RegistrationDto registrationDto)
    {
        var existingUser = await userRepository.GetByUserName(registrationDto.UserName);
        if (existingUser is not null)
            return Error.Conflict(
                "UserAlreadyExists",
                Messages.Messages.Error.UserAlreadyExist
            );
        
        var hashedPassword = PasswordHashProvider.GenerateHashPassword(registrationDto.Password);

        var userEntity = new UserEntity
        {
            UserName = registrationDto.UserName,
            HashedPassword = hashedPassword
        };

        await userRepository.Create(userEntity);

        return Result.Success;
    }

    public async Task<ErrorOr<string>> Login(LoginDto loginDto)
    {
        var entity = await userRepository.GetByUserName(loginDto.UserName);
        if (entity is null)
            return Error.NotFound(
                "UserNotFound",
                Messages.Messages.Error.UserNotFound
            );
        
        if (!PasswordHashProvider.Verify(loginDto.Password, entity.HashedPassword))
            return Error.Validation(
                "IncorrectPassword",
                Messages.Messages.Error.IncorrectPassword
            );
       return jwtService.GenerateToken(entity);
    }
}