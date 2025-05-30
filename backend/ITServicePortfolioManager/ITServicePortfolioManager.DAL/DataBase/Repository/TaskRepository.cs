using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class TaskRepository(ApplicationDbContext context)
    : RepositoriesBase<TaskEntity>(context), ITaskRepository
{
    public async Task<List<TaskEntity>> GetByUserIdAsync(long UserId) =>
        (await context.Tasks.AsNoTracking().Where(task => task.UserId== UserId).ToListAsync())!;
    
}
