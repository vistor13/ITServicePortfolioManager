using ITServicePortfolioManager.DAL.Entities.CommonEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITServicePortfolioManager.DAL.Entities.Configuration;

public class GroupServiceEntityTypeConfiguration : IEntityTypeConfiguration<GroupServiceEntity>
{
    public void Configure(EntityTypeBuilder<GroupServiceEntity> builder)
    {
        builder.OwnsMany(x => x.Services, portfolioBuilder =>
        {
            portfolioBuilder.ToJson();
        });
    }
}