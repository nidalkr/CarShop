using CarShop.Domain.Common;

namespace CarShop.Domain.Entities.Catalog;

public sealed class CarImageEntity : BaseEntity
{
    public int CarId { get; set; }
    public CarEntity Car { get; set; } = default!;

    // the path -> "images/cars/xxxx.jpg"
    public string ImageUrl { get; set; } = default!;
    public bool IsPrimary { get; set; }
}
