namespace CarShop.Application.Modules.Users.Commands.Update;

using CarShop.Application.Modules.Users.Dtos;

public sealed class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? NewPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Phone { get; set; }
    public string Address { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}