using CarShop.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarShop.Infrastructure.Database.Configurations.Catalog;

public sealed class CarEntityConfiguration : IEntityTypeConfiguration<CarEntity>
{
    public void Configure(EntityTypeBuilder<CarEntity> b)
    {
        b.ToTable("Cars");

        b.HasKey(x => x.Id);

       
        b.HasIndex(x => x.Vin).IsUnique();

        
        b.Property(x => x.Vin)
            .IsRequired()
            .HasMaxLength(17);

        b.Property(x => x.Model)
            .IsRequired()
            .HasMaxLength(150);

        b.Property(x => x.Condition)
            .IsRequired()
            .HasMaxLength(30); 

        b.Property(x => x.StockNumber)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.InventoryLocation)
            .IsRequired()
            .HasMaxLength(100);

        b.Property(x => x.Color)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.Transmission)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.FuelType)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.Drivetrain)
            .IsRequired()
            .HasMaxLength(50);

        b.Property(x => x.Engine)
            .IsRequired()
            .HasMaxLength(50);

        
        b.Property(x => x.HorsePower)
            .IsRequired();

        
        b.Property(x => x.EpaFuelEconomy)
            .HasMaxLength(50);

        
        b.Property(x => x.Doors)
            .IsRequired();

        b.Property(x => x.Seats)
            .IsRequired();

        b.Property(x => x.ProductionYear)
            .IsRequired();

        b.Property(x => x.Mileage)
            .IsRequired();

        b.Property(x => x.QuantityInStock)
            .IsRequired();

        
        b.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        b.Property(x => x.DiscountedPrice)
            .HasColumnType("decimal(18,2)");

        b.Property(x => x.Msrp)
            .HasColumnType("decimal(18,2)");

        
        b.Property(x => x.Description)
            .HasMaxLength(2000);

        
        b.Property(x => x.DateAdded)
            .HasDefaultValueSql("GETUTCDATE()");

        
        b.HasOne(x => x.Brand)
            .WithMany(x => x.Cars)
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Category)
            .WithMany(x => x.Cars)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.CarStatus)
            .WithMany()
            .HasForeignKey(x => x.CarStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        
        b.HasMany(x => x.Features)
            .WithOne(x => x.Car)
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(x => x.Images)
            .WithOne(x => x.Car)
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
