using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Commerce
{
    public sealed class TransactionEntityConfiguration:IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(t => t.Id);

            builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Status).WithMany().HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x=> x.PaymentMethod).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FinancingProvider).HasMaxLength(100);
        }
    }
}
