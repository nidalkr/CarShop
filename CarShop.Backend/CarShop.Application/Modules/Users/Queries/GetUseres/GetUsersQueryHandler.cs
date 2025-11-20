using CarShop.Application.Modules.Users.Dtos;

namespace CarShop.Application.Modules.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(IAppDbContext ctx)
    : IRequestHandler<GetUsersQuery, PageResult<UserDto>>
{
    public async Task<PageResult<UserDto>> Handle(GetUsersQuery request, CancellationToken ct)
    {
        var query = ctx.Users
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLowerInvariant();

            query = query.Where(x =>
                x.Username.ToLower().Contains(search)
                || x.Email.ToLower().Contains(search)
                || x.FirstName.ToLower().Contains(search)
                || x.LastName.ToLower().Contains(search));
        }

        query = query.OrderBy(x => x.Username);

        // umjesto .Select(UserDto.Projection)
        var dtoQuery = query.Select(x => new UserDto
        {
            Id = x.Id,
            Username = x.Username,
            Email = x.Email,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Phone = x.Phone,
            Address = x.Address,
            RoleId = x.RoleId,
            RoleName = x.Role != null ? x.Role.RoleName : null,
            IsActive = x.IsActive,
            RegistrationDate = x.RegistrationDate,
            LastLoginDate = x.LastLoginDate
        });

        return await PageResult<UserDto>.FromQueryableAsync(
            dtoQuery,
            request.Paging,
            ct);
    }
}
