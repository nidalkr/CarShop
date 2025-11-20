namespace CarShop.Application.Modules.Users.Queries.GetUsers;

using CarShop.Application.Modules.Users.Dtos;

public sealed class GetUsersQuery : BasePagedQuery<UserDto>
{
    public string? Search { get; init; }
    public bool? IsActive { get; init; }
}