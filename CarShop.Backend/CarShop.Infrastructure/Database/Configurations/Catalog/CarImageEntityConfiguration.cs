using CarShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.Database.Configurations.Catalog;

public sealed class CarImageEntityConfiguration : IEntityTypeConfiguration<CarImageEntity>
{
    public void Configure(EntityTypeBuilder<CarImageEntity> b)
    {
        b.ToTable("CarImages");

        b.Property(x => x.ImageUrl).IsRequired().HasMaxLength(500);

        b.HasOne(x => x.Car)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => new { x.CarId, x.IsPrimary });
    }
}
