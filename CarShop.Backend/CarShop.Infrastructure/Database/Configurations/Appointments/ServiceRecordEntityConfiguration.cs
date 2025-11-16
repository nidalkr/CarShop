using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Appointments
{
    public sealed class ServiceRecordEntityConfiguration:IEntityTypeConfiguration<ServiceRecordEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceRecordEntity> builder)
        {
            builder.ToTable("ServiceRecords");
            builder.HasKey(x => x.Id);

            builder.HasOne(x=> x.Appointment).WithMany(a=> a.ServiceRecords).HasForeignKey(x=>x.AppointmentId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.WorkDescription).IsRequired().HasMaxLength(2000);
            
        }
    }
}
