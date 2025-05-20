using ITServicePortfolioManager.DAL.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ITServicePortfolioManager.DAL.Entities.CommonEntity;

[EntityTypeConfiguration(typeof(GroupServiceEntityTypeConfiguration))]
public class GroupServiceEntity
{
    public long IndexProvider { get; set; }
    public List<ServiceEntity> Services { get; set; } = [];
}