namespace CarShop.Application.Modules.Users.Commands.Delete;

public sealed class DeleteUserCommand :IRequest
{
    public int Id { get; init; }
}
