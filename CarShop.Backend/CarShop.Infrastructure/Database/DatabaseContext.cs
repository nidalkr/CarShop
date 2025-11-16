using CarShop.Application.Abstractions;
using CarShop.Domain.Entities.Commerce;

namespace CarShop.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    //Identity
    public DbSet<CarShopUserEntity> Users => Set<CarShopUserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<UserRoleEntity> UserRoles => Set<UserRoleEntity>();

    //Catalog
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<CarEntity> Cars => Set<CarEntity>();

    //Commerce
    public DbSet<ReviewEntity> Reviews => Set<ReviewEntity>();
    public DbSet<InquiryEntity> Inquiries => Set<InquiryEntity>();
    public DbSet<StatusEntity> Statuses => Set<StatusEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();
    public DbSet<FavouriteEntity> Favs => Set<FavouriteEntity>();
    public DbSet<CartEntity> Carts => Set<CartEntity>();

    //Appointments
    public DbSet<ServiceAppointmentEntity> Services => Set<ServiceAppointmentEntity>();
    public DbSet<ServiceRecordEntity> serviceRecords => Set<ServiceRecordEntity>();
    public DbSet<TestDriveEntity> Tests => Set<TestDriveEntity>();
    public DbSet<DeliveryEntity> Deliveries => Set<DeliveryEntity>();

    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}