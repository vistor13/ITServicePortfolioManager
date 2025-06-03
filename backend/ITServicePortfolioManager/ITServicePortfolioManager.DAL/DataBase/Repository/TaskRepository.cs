using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class TaskRepository(ApplicationDbContext context)
    : RepositoriesBase<TaskEntity>(context), ITaskRepository
{
    public async Task<List<TaskEntity>> GetByUserIdAsync(long UserId) =>
        (await context.Tasks.AsNoTracking().Where(task => task.UserId== UserId).ToListAsync())!;
    public async Task<List<TaskEntity>> GetFilteredTasksAsync(DateTime? FromDate, DateTime? ToDate, bool? SortDescending, string? AlgorithmName)
    {
        var query = context.Tasks.AsQueryable();

        if (FromDate.HasValue)
            query = query.Where(t => t.CreatedAt >= FromDate.Value);

        if (ToDate.HasValue)
            query = query.Where(t => t.CreatedAt <= ToDate.Value);

        if (!string.IsNullOrEmpty(AlgorithmName))
            query = query.Where(t => t.NameAlgorithm == AlgorithmName);

        query = SortDescending == true ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt);

        return await query.ToListAsync();
    }
}
