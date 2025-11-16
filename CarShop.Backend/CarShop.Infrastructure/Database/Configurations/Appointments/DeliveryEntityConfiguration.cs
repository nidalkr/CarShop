using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Appointments
{
    public sealed class DeliveryEntityConfiguration:IEntityTypeConfiguration<DeliveryEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryEntity> builder)
        {
            builder.ToTable("Delivery");
            builder.HasKey(x => x.Id);

            builder.HasOne(x=> x.Order).WithMany().HasForeignKey(x=> x.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=> x.Status).WithMany().HasForeignKey(x=> x.StatusId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Address).IsRequired().HasMaxLength(300);
            
        }
    }
}
