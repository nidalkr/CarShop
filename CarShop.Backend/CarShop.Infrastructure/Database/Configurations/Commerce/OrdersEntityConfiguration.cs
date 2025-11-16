using CarShop.Domain.Entities.Commerc;

public sealed class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Car)
            .WithMany()
            .HasForeignKey(x => x.CarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ShippingAddress).IsRequired().HasMaxLength(300);
    }
}
