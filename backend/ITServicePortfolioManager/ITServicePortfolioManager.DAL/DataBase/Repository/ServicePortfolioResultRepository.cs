using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class ServicePortfolioResultRepository(ApplicationDbContext context)
    : RepositoriesBase<ServicePortfolioResultEntity>(context), IServicePortfolioResultRepository;
