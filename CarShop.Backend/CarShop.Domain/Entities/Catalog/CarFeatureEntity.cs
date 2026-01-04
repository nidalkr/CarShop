using CarShop.Domain.Common;

namespace CarShop.Domain.Entities.Catalog;

public sealed class CarFeatureEntity : BaseEntity
{
    public int CarId { get; set; }
    public CarEntity Car { get; set; } = default!;
    public string Feature { get; set; } = default!;
}
