using CarShop.Application.Abstractions;
using CarShop.Application.Modules.Cars.Dtos;
using CarShop.Domain.Entities.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Application.Modules.Cars.Commands.Create;

public sealed class CreateCarCommandHandler(IAppDbContext ctx, TimeProvider clock)
    : IRequestHandler<CreateCarCommand, CarDetailsDto>
{
    public async Task<CarDetailsDto> Handle(CreateCarCommand request, CancellationToken ct)
    {
        var vin = request.Vin.Trim();

        if (await ctx.Cars.AnyAsync(x => x.Vin == vin && !x.IsDeleted, ct))
            throw new Exception("Vozilo s istim VIN brojem već postoji!");

        _ = await ctx.Brands.FirstOrDefaultAsync(x => x.Id == request.BrandId && !x.IsDeleted, ct)
            ?? throw new Exception("Brand nije pronađen!");

        _ = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && !x.IsDeleted, ct)
            ?? throw new Exception("Kategorija nije pronađena!");

        _ = await ctx.Statuses.FirstOrDefaultAsync(x => x.Id == request.CarStatusId && !x.IsDeleted, ct)
            ?? throw new Exception("Status nije pronađen.");

        if (string.IsNullOrWhiteSpace(request.PrimaryImageUrl))
            throw new Exception("PrimaryImageUrl je obavezan.");

        var car = new CarEntity
        {
            BrandId = request.BrandId,
            CategoryId = request.CategoryId,
            CarStatusId = request.CarStatusId,
            Model = request.Model.Trim(),
            Vin = vin,
            ProductionYear = request.ProductionYear,
            Mileage = request.Mileage,
            Color = request.Color.Trim(),
            Transmission = request.Transmission.Trim(),
            FuelType = request.FuelType.Trim(),
            Drivetrain = request.Drivetrain.Trim(),
            Engine = request.Engine.Trim(),
            HorsePower = request.HorsePower,
            Description = request.Description?.Trim(),
            Price = request.Price,
            DiscountedPrice = request.DiscountedPrice,
            DateAdded = request.DateAdded ?? clock.GetUtcNow().UtcDateTime
        };

        
        car.AddImage(request.PrimaryImageUrl.Trim(), isPrimary: true);


        if (request.GalleryImageUrls?.Any() == true)
        {
            foreach (var url in request.GalleryImageUrls
                         .Where(x => !string.IsNullOrWhiteSpace(x))
                         .Select(x => x.Trim())
                         .Distinct())
            {
                if (url == request.PrimaryImageUrl.Trim())
                    continue;

                car.AddImage(url, isPrimary: false);
            }
        }        

        car.EnsurePrimaryImage();

        if (request.Features is not null)
        {
            foreach (var feature in request.Features)
            {
                car.AddFeature(feature);
            }
        }

        
        car.Condition = request.Condition.Trim();
        car.StockNumber = request.StockNumber.Trim();
        car.InventoryLocation = request.InventoryLocation.Trim();
        car.Doors = request.Doors;
        car.Seats = request.Seats;
        car.QuantityInStock = request.QuantityInStock;
        car.EpaFuelEconomy = request.EpaFuelEconomy;
        car.Msrp = request.Msrp;

        ctx.Cars.Add(car);
        await ctx.SaveChangesAsync(ct);        

        return CarDetailsDto.FromEntity(car);
    }
}
