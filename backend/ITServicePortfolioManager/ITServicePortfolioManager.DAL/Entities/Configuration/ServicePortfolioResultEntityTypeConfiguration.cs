using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITServicePortfolioManager.DAL.Entities.Configuration;

public class ServicePortfolioResultEntityTypeConfiguration: IEntityTypeConfiguration<ServicePortfolioResultEntity>
{
    public void Configure(EntityTypeBuilder<ServicePortfolioResultEntity> builder)
    {
        builder.Property(b => b.CompanyIncome)
            .IsRequired();
        builder.HasOne(p => p.Task)
            .WithOne()
            .HasForeignKey<ServicePortfolioResultEntity>(p => p.TaskId);

        builder.OwnsMany(x => x.Portfolio, portfolioBuilder =>
        {
            portfolioBuilder.ToJson();
        });
    }
}