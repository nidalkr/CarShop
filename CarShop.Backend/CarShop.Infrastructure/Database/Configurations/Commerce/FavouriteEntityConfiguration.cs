using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Infrastructure.Database.Configurations.Commerc
{
    public sealed class FavouriteEntityConfiguration:IEntityTypeConfiguration<FavouriteEntity>
    {
        public void Configure(EntityTypeBuilder<FavouriteEntity> builder)
        {
            builder.ToTable("Favourites");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Car).WithMany().HasForeignKey(x => x.CarId).OnDelete(DeleteBehavior.Cascade);

            //Unique combination, cannot multiplay favourites!
            builder.HasIndex(x => new { x.UserId, x.CarId }).IsUnique();
        }
    }
}
