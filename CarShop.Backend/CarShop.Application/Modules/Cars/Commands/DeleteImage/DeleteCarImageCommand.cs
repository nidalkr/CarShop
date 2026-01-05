using MediatR;

namespace CarShop.Application.Modules.Cars.Commands.DeleteImage;

public sealed class DeleteCarImageCommand : IRequest
{
    public string ImageUrl { get; init; } = string.Empty;
}
