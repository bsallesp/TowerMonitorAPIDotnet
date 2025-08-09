using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TowerApi.Domain.Entities;

namespace TowerApi.Infrastructure.Data.Configurations;

public class TowerConfiguration : IEntityTypeConfiguration<Tower>
{
    public void Configure(EntityTypeBuilder<Tower> builder)
    {
        builder.ToTable("Towers");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.License).HasMaxLength(255);
        builder.Property(t => t.Address).HasMaxLength(500);
        builder.Property(t => t.City).HasMaxLength(100);
        builder.Property(t => t.County).HasMaxLength(100);
        builder.Property(t => t.State).HasMaxLength(50);
        builder.Property(t => t.StructureType).HasMaxLength(100);
    }
}