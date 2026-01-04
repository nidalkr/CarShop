using CarShop.Application.Abstractions;
using CarShop.Application.Common.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Application.Modules.Cars.Commands.Delete;

public sealed class DeleteCarCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DeleteCarCommand>
{
    public async Task Handle(DeleteCarCommand request, CancellationToken ct)
    {
        var car = await ctx.Cars
            .Include(x => x.Images)
            .Include(x => x.Features)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct)
            ?? throw new CarShopNotFoundException("Vehicle was not found!");

        if (car.Images?.Any() == true)
        {
            ctx.CarImages.RemoveRange(car.Images);
        }
        if (car.Features?.Any() == true)
        {
            ctx.CarFeatures.RemoveRange(car.Features);
        }

        ctx.Cars.Remove(car);
        await ctx.SaveChangesAsync(ct);
    }
}
