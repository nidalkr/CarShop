namespace CarShop.Application.Modules.Users.Queries.GetUserById;

using CarShop.Application.Modules.Users.Dtos;

public sealed class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; init; }
}