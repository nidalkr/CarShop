using CarShop.Application.Modules.Users.Commands.Create;
using CarShop.Application.Modules.Users.Commands.Delete;
using CarShop.Application.Modules.Users.Commands.Update;
using CarShop.Application.Modules.Users.Dtos;
using CarShop.Application.Modules.Users.Queries.GetUserById;
using CarShop.Application.Modules.Users.Queries.GetUsers;

[ApiController]
[Route("api/users")]
[Authorize]
public sealed class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResult<UserDto>>> GetUsers([FromQuery] GetUsersQuery query, CancellationToken ct)
    {
        return Ok(await mediator.Send(query, ct));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id, CancellationToken ct)
    {
        return Ok(await mediator.Send(new GetUserByIdQuery { Id = id }, ct));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var created = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetUserById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserCommand command, CancellationToken ct)
    {
        command.Id = id;
        var updated = await mediator.Send(command, ct);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        await mediator.Send(new DeleteUserCommand { Id = id }, ct);
        return NoContent();
    }
}