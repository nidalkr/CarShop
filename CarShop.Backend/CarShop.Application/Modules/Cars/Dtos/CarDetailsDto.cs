using CarShop.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarShop.Application.Modules.Cars.Dtos
{
    public sealed class CarDetailsDto
    {
        public int Id { get; init; }

        public int BrandId { get; init; }
        public string BrandName { get; init; } = string.Empty;

        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = string.Empty;

        public int CarStatusId { get; init; }
        public string CarStatusName { get; init; } = string.Empty;

        
        public string Condition { get; init; } = string.Empty;   
        public string StockNumber { get; init; } = string.Empty;
        public string InventoryLocation { get; init; } = string.Empty;

        public int Doors { get; init; }
        public int Seats { get; init; }

        public string Model { get; init; } = string.Empty;
        public string Vin { get; init; } = string.Empty;
        public int ProductionYear { get; init; }
        public int Mileage { get; init; }

        public string Color { get; init; } = string.Empty;
        public string Transmission { get; init; } = string.Empty;
        public string FuelType { get; init; } = string.Empty;
        public string Drivetrain { get; init; } = string.Empty;
        public string Engine { get; init; } = string.Empty;
        public int HorsePower { get; init; }

        
        public string? EpaFuelEconomy { get; init; } 
        public decimal? Msrp { get; init; }
        public int QuantityInStock { get; init; }

        public string? PrimaryImageUrl { get; init; }
        public IReadOnlyCollection<string> ImageUrls { get; init; } = Array.Empty<string>();
        public IReadOnlyCollection<string> Features { get; init; } = Array.Empty<string>();

        public string? Description { get; init; }
        public decimal Price { get; init; }
        public decimal? DiscountedPrice { get; init; }
        public DateTime DateAdded { get; init; }

        public DateTime CreatedAtUtc { get; init; }
        public DateTime? ModifiedAtUtc { get; init; }

        public static CarDetailsDto FromEntity(CarEntity car)
        {
            var images = car.Images ?? new List<CarImageEntity>();
            var features = car.Features ?? new List<CarFeatureEntity>();

            return new CarDetailsDto
            {
                Id = car.Id,

                BrandId = car.BrandId,
                BrandName = car.Brand?.BrandName ?? string.Empty,

                CategoryId = car.CategoryId,
                CategoryName = car.Category?.CategoryName ?? string.Empty,

                CarStatusId = car.CarStatusId,
                CarStatusName = car.CarStatus?.StatusName ?? string.Empty,

                
                Condition = car.Condition,
                StockNumber = car.StockNumber,
                InventoryLocation = car.InventoryLocation,
                Doors = car.Doors,
                Seats = car.Seats,

                Model = car.Model,
                Vin = car.Vin,
                ProductionYear = car.ProductionYear,
                Mileage = car.Mileage,

                Color = car.Color,
                Transmission = car.Transmission,
                FuelType = car.FuelType,
                Drivetrain = car.Drivetrain,
                Engine = car.Engine,
                HorsePower = car.HorsePower,

                
                EpaFuelEconomy = car.EpaFuelEconomy,
                Msrp = car.Msrp,
                QuantityInStock = car.QuantityInStock,

                PrimaryImageUrl = images.FirstOrDefault(x => x.IsPrimary)?.ImageUrl,
                ImageUrls = images
                    .OrderByDescending(x => x.IsPrimary)
                    .Select(x => x.ImageUrl)
                    .ToList(),

                Features = features
                    .Select(f => f.Feature)
                    .ToList(),

                Description = car.Description,
                Price = car.Price,
                DiscountedPrice = car.DiscountedPrice,
                DateAdded = car.DateAdded,

                CreatedAtUtc = car.CreatedAtUtc,
                ModifiedAtUtc = car.ModifiedAtUtc
            };
        }
    }
}
