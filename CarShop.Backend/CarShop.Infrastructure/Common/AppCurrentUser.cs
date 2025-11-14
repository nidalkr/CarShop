using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CarShop.Application.Abstractions;

namespace CarShop.Infrastructure.Common;

/// <summary>
/// Implementation of IAppCurrentUser that reads data from a JWT token.
/// </summary>
public sealed class AppCurrentUser(IHttpContextAccessor httpContextAccessor)
    : IAppCurrentUser
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    public int? UserId =>
        int.TryParse(_user?.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : null;

    public string? Email =>
        _user?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated =>
       _user?.Identity?.IsAuthenticated ?? false;

    public string? Username =>
    _user?.FindFirstValue(ClaimTypes.Name);
    public int? RoleId =>
          int.TryParse(_user?.FindFirstValue("role_id"), out var roleId)
              ? roleId
              : null;

    public bool IsActive =>
        _user?.FindFirstValue("is_active")?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
    private static bool ParseBooleanClaim(string? value) =>
        value is not null && value.Equals("true", StringComparison.OrdinalIgnoreCase);

    private static string? EmptyToNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value;
}