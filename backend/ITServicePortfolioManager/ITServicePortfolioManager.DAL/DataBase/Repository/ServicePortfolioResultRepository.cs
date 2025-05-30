using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class ServicePortfolioResultRepository(ApplicationDbContext context)
    : RepositoriesBase<ServicePortfolioResultEntity>(context), IServicePortfolioResultRepository
{
    public async Task<ServicePortfolioResultEntity> GetByTaskIdAsync(long TaskId) =>
        (await context.ServicePortfolioResults.AsNoTracking().FirstOrDefaultAsync(solve=> solve.TaskId== TaskId))!;
}
