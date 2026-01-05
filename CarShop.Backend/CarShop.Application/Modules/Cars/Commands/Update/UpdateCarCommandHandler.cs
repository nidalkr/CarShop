using CarShop.Application.Abstractions;
using CarShop.Application.Modules.Cars.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Application.Modules.Cars.Commands.Update;

public sealed class UpdateCarCommandHandler(IAppDbContext ctx, TimeProvider clock)
    : IRequestHandler<UpdateCarCommand, CarDetailsDto>
{
    public async Task<CarDetailsDto> Handle(UpdateCarCommand request, CancellationToken ct)
    {
        var vin = request.Vin.Trim();

        var car = await ctx.Cars
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.CarStatus)
            .Include(x => x.Images)
            .Include(x => x.Features)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct)
            ?? throw new Exception("Vozilo nije pronađeno!");

        if (await ctx.Cars.AnyAsync(x => x.Id != request.Id && x.Vin == vin && !x.IsDeleted, ct))
            throw new Exception("Vozilo sa istim VIN brojem već postoji!");

        _ = await ctx.Brands.FirstOrDefaultAsync(x => x.Id == request.BrandId && !x.IsDeleted, ct)
            ?? throw new Exception("Brand nije pronađen.");

        _ = await ctx.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId && !x.IsDeleted, ct)
            ?? throw new Exception("Kategorija nije pronađena.");

        _ = await ctx.Statuses.FirstOrDefaultAsync(x => x.Id == request.CarStatusId && !x.IsDeleted, ct)
            ?? throw new Exception("Status nije pronađen.");

        car.BrandId = request.BrandId;
        car.CategoryId = request.CategoryId;
        car.CarStatusId = request.CarStatusId;

        car.Model = request.Model.Trim();
        car.Vin = vin;
        car.ProductionYear = request.ProductionYear;
        car.Mileage = request.Mileage;
        car.Color = request.Color.Trim();
        car.Transmission = request.Transmission.Trim();
        car.FuelType = request.FuelType.Trim();
        car.Drivetrain = request.Drivetrain.Trim();
        car.Engine = request.Engine.Trim();
        car.HorsePower = request.HorsePower;
        car.Condition = request.Condition.Trim();
        car.StockNumber = request.StockNumber.Trim();
        car.InventoryLocation = request.InventoryLocation.Trim();
        car.Doors = request.Doors;
        car.Seats = request.Seats;
        car.QuantityInStock = request.QuantityInStock;
        car.EpaFuelEconomy = request.EpaFuelEconomy;
        car.Msrp = request.Msrp;
        car.Description = request.Description?.Trim();
        car.Price = request.Price;
        car.DiscountedPrice = request.DiscountedPrice;
        car.DateAdded = request.DateAdded ?? clock.GetUtcNow().UtcDateTime;



        if (!string.IsNullOrWhiteSpace(request.PrimaryImageUrl))
        {
            var primary = request.PrimaryImageUrl.Trim();

            
            var existing = car.Images.FirstOrDefault(x => x.ImageUrl == primary);
            if (existing is not null)
            {
                foreach (var img in car.Images) img.IsPrimary = false;
                existing.IsPrimary = true;
            }
            else
            {
                
                car.AddImage(primary, isPrimary: true);
            }
        }

        
        if (request.GalleryImageUrls?.Any() == true)
        {
            var set = new HashSet<string>(car.Images.Select(x => x.ImageUrl));

            foreach (var url in request.GalleryImageUrls
                         .Where(x => !string.IsNullOrWhiteSpace(x))
                         .Select(x => x.Trim())
                         .Distinct())
            {
                if (set.Contains(url)) continue;
                car.AddImage(url, isPrimary: false);
            }
        }

        car.EnsurePrimaryImage();

        if (request.Features?.Any() == true)
        {
            foreach (var f in request.Features)
                car.AddFeature(f);
        }


        await ctx.SaveChangesAsync(ct);

        return CarDetailsDto.FromEntity(car);
    }
}
