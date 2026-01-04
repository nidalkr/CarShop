using CarShop.Application.Modules.Cars.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Application.Modules.Cars.Commands.Create
{
    public sealed class CreateCarCommand : IRequest<CarDetailsDto>
    {
        public int BrandId { get; init; }
        public int CategoryId { get; init; }
        public int CarStatusId { get; init; }

        public string Condition { get; init; } = default!;
        public string StockNumber { get; init; } = default!;
        public string InventoryLocation { get; init; } = default!;
        public int QuantityInStock { get; init; }

        public int Doors { get; init; }
        public int Seats { get; init; }

        public string Model { get; init; } = default!;
        public string Vin { get; init; } = default!;
        public int ProductionYear { get; init; }
        public int Mileage { get; init; }
        public string Color { get; init; } = default!;
        public string Transmission { get; init; } = default!;
        public string FuelType { get; init; } = default!;
        public string Drivetrain { get; init; } = default!;
        public string Engine { get; init; } = default!;
        public int HorsePower { get; init; }

        public string? EpaFuelEconomy { get; init; }
        public decimal? Msrp { get; init; }

        public string? Description { get; init; }
        public decimal Price { get; init; }
        public decimal? DiscountedPrice { get; init; }
        public DateTime? DateAdded { get; init; }

        public string PrimaryImageUrl { get; init; } = string.Empty;
        public IReadOnlyCollection<string>? GalleryImageUrls { get; init; }
        public IReadOnlyCollection<string>? Features { get; init; }
    }
}
