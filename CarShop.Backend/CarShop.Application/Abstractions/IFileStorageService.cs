using Microsoft.AspNetCore.Http;

namespace CarShop.Application.Abstractions;

public interface IFileStorageService
{
    Task<string> SaveCarImageAsync(IFormFile file, CancellationToken ct);
    Task DeleteCarImageAsync(string imageUrl, CancellationToken ct);
}
