// CarShopUserEntity.cs
using CarShop.Domain.Common;

namespace CarShop.Domain.Entities.Identity;

public sealed class CarShopUserEntity : BaseEntity
{
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Phone { get; set; }
    public string Address { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } = new List<RefreshTokenEntity>();
}