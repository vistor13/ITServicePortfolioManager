using ITServicePortfolioManager.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.DataBase;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TaskEntity> Tasks { get; set; } = null!;

    public DbSet<ServicePortfolioResultEntity> ServicePortfolioResults { get; set; } = null!;
}