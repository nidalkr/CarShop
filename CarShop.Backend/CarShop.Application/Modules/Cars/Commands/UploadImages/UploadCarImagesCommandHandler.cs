using CarShop.Application.Abstractions;
using CarShop.Application.Modules.Cars.Dtos;
using MediatR;

namespace CarShop.Application.Modules.Cars.Commands.UploadImages;

public sealed class UploadCarImagesCommandHandler(IFileStorageService storage)
    : IRequestHandler<UploadCarImagesCommand, UploadCarImagesResponseDto>
{
    public async Task<UploadCarImagesResponseDto> Handle(UploadCarImagesCommand request, CancellationToken ct)
    {
        if (request.Files is null || request.Files.Count == 0)
            throw new Exception("No files provided.");

        var urls = new List<string>();
        foreach (var file in request.Files)
        {
            if (file is null || file.Length == 0) continue;
            urls.Add(await storage.SaveCarImageAsync(file, ct));
        }

        return new UploadCarImagesResponseDto { ImageUrls = urls };
    }
}
