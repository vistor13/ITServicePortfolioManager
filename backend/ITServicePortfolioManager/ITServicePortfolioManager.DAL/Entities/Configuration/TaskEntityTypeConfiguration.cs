using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITServicePortfolioManager.DAL.Entities.Configuration;

public class TaskEntityTypeConfiguration: IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(b => b.CountProvider)
            .IsRequired();

        builder.Property(b => b.TotalHumanResource)
            .IsRequired();
        builder.HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);
        builder.OwnsMany(x => x.Groups, portfolioBuilder =>
        {
            portfolioBuilder.ToJson();
        });
    }
}