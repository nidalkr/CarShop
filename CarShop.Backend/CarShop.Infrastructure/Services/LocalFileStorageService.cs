using CarShop.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CarShop.Infrastructure.Services;

public sealed class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    public async Task<string> SaveCarImageAsync(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("File is empty.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
        if (!allowed.Contains(ext))
            throw new InvalidOperationException("Only jpg, jpeg, png, webp are allowed.");

        var wwwroot = Path.Combine(env.ContentRootPath, "wwwroot");
        var folder = Path.Combine(wwwroot, "images", "cars");
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(folder, fileName);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return $"images/cars/{fileName}";
    }

    public Task DeleteCarImageAsync(string imageUrl, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return Task.CompletedTask;

        var cleaned = imageUrl.Replace('\\', '/').TrimStart('/');
        if (!cleaned.StartsWith("images/cars/"))
            throw new InvalidOperationException("Invalid image path.");

        var wwwroot = Path.Combine(env.ContentRootPath, "wwwroot");
        var fullPath = Path.Combine(wwwroot, cleaned.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
