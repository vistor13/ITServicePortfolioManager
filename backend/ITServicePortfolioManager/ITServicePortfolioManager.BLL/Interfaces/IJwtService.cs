using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.BLL.Services;

public interface IJwtService
{
    string GenerateToken(UserEntity user);
}