using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.DAL.Interfaces;

public interface ITaskRepository : IRepositoriesBase<TaskEntity>
{
    Task<List<TaskEntity>> GetByUserIdAsync(long UserId);

     Task<List<TaskEntity>> GetFilteredTasksAsync(long UserId,DateTime? FromDate, DateTime? ToDate, bool? SortDescending, string? AlgorithmName);
}
