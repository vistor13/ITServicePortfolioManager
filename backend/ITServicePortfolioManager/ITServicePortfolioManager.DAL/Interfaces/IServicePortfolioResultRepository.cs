using ITServicePortfolioManager.DAL.Entities;

namespace ITServicePortfolioManager.DAL.Interfaces;

public interface IServicePortfolioResultRepository : IRepositoriesBase<ServicePortfolioResultEntity>
{
    Task<ServicePortfolioResultEntity> GetByTaskIdAsync(long TaskId);
}
