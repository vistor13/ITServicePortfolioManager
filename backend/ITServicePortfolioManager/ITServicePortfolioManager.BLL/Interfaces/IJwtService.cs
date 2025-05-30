using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.BLL.Interfaces;

public interface IJwtService
{
    string GenerateToken(UserEntity user);
}