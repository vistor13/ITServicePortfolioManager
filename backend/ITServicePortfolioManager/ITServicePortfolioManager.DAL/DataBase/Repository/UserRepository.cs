using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class UserRepository(ApplicationDbContext context) : RepositoriesBase<UserEntity>(context), IUserRepository
{
    public async Task<UserEntity?> GetByUserName(string email) =>
        await context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.UserName == email);
}
