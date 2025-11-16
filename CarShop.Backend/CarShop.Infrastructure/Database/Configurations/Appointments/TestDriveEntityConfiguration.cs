using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Appointments
{
    public sealed class TestDriveEntityConfiguration : IEntityTypeConfiguration<TestDriveEntity>
    {
        public void Configure(EntityTypeBuilder<TestDriveEntity> builder)
        {
            builder.ToTable("TestDrives");

            builder.HasKey(x => x.Id);

            builder.HasOne(x=> x.User).WithMany().HasForeignKey(x=> x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=> x.Car).WithMany().HasForeignKey(x=> x.CarId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Notes).HasMaxLength(1000);
        }
    }
}
