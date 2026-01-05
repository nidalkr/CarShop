namespace CarShop.Application.Modules.Cars.Dtos;

public sealed class UploadCarImagesResponseDto
{
    public IReadOnlyCollection<string> ImageUrls { get; init; } = Array.Empty<string>();
}
