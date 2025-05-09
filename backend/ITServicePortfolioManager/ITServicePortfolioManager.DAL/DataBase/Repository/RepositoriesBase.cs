using ITServicePortfolioManager.DAL.Entities.CommonEntity;
using ITServicePortfolioManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class RepositoriesBase<TEntity>(ApplicationDbContext context)
    : IRepositoriesBase<TEntity> where TEntity : BaseEntity
{
    public async Task<TEntity?> GetById(long id)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));
    }
    
    public async Task<List<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().ToListAsync();
    }

    public async Task Delete(long id)
    {
        var entity = await GetById(id);
        if (entity is null) return;

        context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(long id, TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync();
    }
}