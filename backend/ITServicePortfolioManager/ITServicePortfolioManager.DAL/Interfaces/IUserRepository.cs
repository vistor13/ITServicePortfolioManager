using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.DAL.Interfaces;

public interface IUserRepository : IRepositoriesBase<UserEntity>
{
    Task<UserEntity> GetByUserName(string email);
}
