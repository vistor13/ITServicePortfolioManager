using ITServicePortfolioManager.DAL.Entities.CommonEntity;

namespace ITServicePortfolioManager.DAL.Interfaces;

public interface IRepositoriesBase<TEntity> where TEntity : BaseEntity

{
    Task<List<TEntity>> GetAll();
    Task<TEntity?> GetById(long id);
    Task Delete(long id);
    Task<TEntity> Create(TEntity entity);
    Task Update(long id, TEntity entity);
}