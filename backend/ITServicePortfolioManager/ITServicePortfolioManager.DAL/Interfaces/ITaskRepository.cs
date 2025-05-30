using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.DAL.Interfaces;

public interface ITaskRepository : IRepositoriesBase<TaskEntity>
{
    Task<List<TaskEntity>> GetByUserIdAsync(long UserId);
}
