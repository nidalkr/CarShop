using CarShop.Domain.Entities.Commerce;

namespace CarShop.Application.Abstractions;

// Application layer
public interface IAppDbContext
{

    DbSet<CarShopUserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    DbSet<UserRoleEntity> UserRoles { get; }

    DbSet<BrandEntity> Brands { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CarEntity> Cars { get; }
    DbSet<CarImageEntity> CarImages { get; }
    DbSet<CarFeatureEntity> CarFeatures { get; }


    DbSet<ReviewEntity> Reviews { get; }
    DbSet<InquiryEntity> Inquiries { get; }
    DbSet<CartEntity> Carts { get; }

    DbSet<StatusEntity> Statuses { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}