using CarShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.Database.Configurations.Catalog;

public sealed class CarFeatureEntityConfiguration : IEntityTypeConfiguration<CarFeatureEntity>
{
    public void Configure(EntityTypeBuilder<CarFeatureEntity> b)
    {
        b.ToTable("CarFeatures");

        b.Property(x => x.Feature).IsRequired().HasMaxLength(200);

        b.HasOne(x => x.Car)
            .WithMany(x => x.Features)
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.CarId, x.Feature }).IsUnique();
    }
}
