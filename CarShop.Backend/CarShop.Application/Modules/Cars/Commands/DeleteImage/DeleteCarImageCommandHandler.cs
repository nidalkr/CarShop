using CarShop.Application.Abstractions;
using MediatR;

namespace CarShop.Application.Modules.Cars.Commands.DeleteImage;

public sealed class DeleteCarImageCommandHandler(IFileStorageService storage) : IRequestHandler<DeleteCarImageCommand>
{
    public async Task Handle(DeleteCarImageCommand request, CancellationToken ct)
    {
        await storage.DeleteCarImageAsync(request.ImageUrl, ct);
    }
}
