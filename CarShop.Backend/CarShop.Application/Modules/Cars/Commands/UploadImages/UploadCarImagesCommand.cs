using CarShop.Application.Modules.Cars.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarShop.Application.Modules.Cars.Commands.UploadImages;

public sealed class UploadCarImagesCommand : IRequest<UploadCarImagesResponseDto>
{
    public IReadOnlyCollection<IFormFile> Files { get; init; } = Array.Empty<IFormFile>();
}
