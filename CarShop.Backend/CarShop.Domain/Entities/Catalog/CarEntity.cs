using CarShop.Domain.Common;
using CarShop.Domain.Entities.Appointments;
using CarShop.Domain.Entities.Commerce;

namespace CarShop.Domain.Entities.Catalog;

public sealed class CarEntity : BaseEntity
{
    public int BrandId { get; set; }
    public BrandEntity Brand { get; set; } = default!;

    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = default!;

    public int CarStatusId { get; set; }
    public StatusEntity CarStatus { get; set; } = default!;

    public string Condition { get; set; } = default!;
    public string StockNumber { get; set; } = default!;
    public string InventoryLocation { get; set; } = default!;
    public int Doors { get; set; }
    public int Seats { get; set; }
    public string Model { get; set; } = default!;
    public string Vin { get; set; } = default!;
    public int ProductionYear { get; set; }
    public int Mileage { get; set; }
    public string Color { get; set; } = default!;
    public string Transmission { get; set; } = default!;
    public string FuelType { get; set; } = default!;
    public string Drivetrain { get; set; } = default!;
    public string Engine { get; set; } = default!;
    public int HorsePower { get; set; } = default!;
    public string? EpaFuelEconomy { get; set; }
    public decimal? Msrp { get; set; }
    public int QuantityInStock { get; set; }

    public string? Description { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountedPrice { get; set; }
    public DateTime DateAdded { get; set; }

    public ICollection<InquiryEntity> Inquiries { get; private set; } = new List<InquiryEntity>();
    public ICollection<CartEntity> Carts { get; private set; } = new List<CartEntity>();
    public ICollection<ReviewEntity> Reviews { get; private set; } = new List<ReviewEntity>();
    public ICollection<FavouriteEntity> Favourites { get; private set; } = new List<FavouriteEntity>();
    public ICollection<OrderEntity> Orders { get; private set; } = new List<OrderEntity>();
    public ICollection<TestDriveEntity> TestDrives { get; private set; } = new List<TestDriveEntity>();
    public ICollection<ServiceAppointmentEntity> ServiceAppointments { get; private set; } = new List<ServiceAppointmentEntity>();
    public ICollection<ServiceRecordEntity> ServiceRecords { get; private set; } = new List<ServiceRecordEntity>();

    public ICollection<CarFeatureEntity> Features { get; private set; } = new List<CarFeatureEntity>();
    public ICollection<CarImageEntity> Images { get; private set; } = new List<CarImageEntity>();

    // DDD helpers
    public void AddImage(string imageUrl, bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

        if (isPrimary)
        {
            foreach (var img in Images) img.IsPrimary = false;
        }

        Images.Add(new CarImageEntity
        {
            ImageUrl = imageUrl.Trim(),
            IsPrimary = isPrimary
        });
    }

    public void EnsurePrimaryImage()
    {
        if (!Images.Any()) return;
        if (Images.Any(x => x.IsPrimary)) return;
        Images.First().IsPrimary = true;
    }

    public void ClearFeatures() => Features.Clear();

    public void AddFeature(string feature)
    {
        if (string.IsNullOrWhiteSpace(feature)) return;
        var f = feature.Trim();

        if (Features.Any(x => string.Equals(x.Feature, f, StringComparison.OrdinalIgnoreCase)))
            return;

        Features.Add(new CarFeatureEntity { Feature = f });
    }
}

