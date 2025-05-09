using ITServicePortfolioManager.DAL.Entities;
using ITServicePortfolioManager.DAL.Interfaces;

namespace ITServicePortfolioManager.DAL.DataBase.Repository;

public class TaskRepository (ApplicationDbContext context)
    : RepositoriesBase<TaskEntity>(context), ITaskRepository;
