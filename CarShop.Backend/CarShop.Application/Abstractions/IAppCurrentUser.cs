namespace CarShop.Application.Abstractions;

/// <summary>
/// Represents the currently logged-in user in the system.
/// </summary>
public interface IAppCurrentUser
{
    /// <summary>
    /// User identifier (UserId).
    /// </summary>
    int? UserId { get; }

    /// <summary>
    /// Unique username of the user.
    /// </summary>
    string? Username { get; }

    /// <summary>
    /// User Email. (optional)
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Role identifier to which the user belongs.
    /// </summary>
    int? RoleId { get; }

    /// <summary>
    /// Indicates whether the user is logged in.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Indicates whether the user account is active.
    /// </summary>
    bool IsActive { get; }
}