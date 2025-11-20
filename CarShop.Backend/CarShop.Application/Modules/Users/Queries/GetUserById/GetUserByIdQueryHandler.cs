using CarShop.Application.Modules.Users.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Application.Modules.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await ctx.Users
            .AsNoTracking()
            .Include(x => x.Role) // ako ti treba RoleName u DTO-u
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct)
            ?? throw new CarShopNotFoundException("Korisnik nije pronađen.");

        return UserDto.FromEntity(user);
    }
}