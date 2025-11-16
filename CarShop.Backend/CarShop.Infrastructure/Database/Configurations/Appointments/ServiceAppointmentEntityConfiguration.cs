using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Appointments
{
    public sealed class ServiceAppointmentEntityConfiguration:IEntityTypeConfiguration<ServiceAppointmentEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceAppointmentEntity> builder)
        {
            builder.ToTable("ServiceAppointments");
            builder.HasKey(x=> x.Id);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=> x.Car).WithMany().HasForeignKey(x=> x.CarId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ServiceType).IsRequired().HasMaxLength(100);
            builder.Property(x=> x.CustomerNotes).HasMaxLength(1000);
        }
    }
}
