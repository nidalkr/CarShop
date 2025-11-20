namespace CarShop.Application.Modules.Users.Commands.Update;

using CarShop.Application.Modules.Users.Dtos;

public sealed class UpdateUserCommandHandler(IAppDbContext ctx,IPasswordHasher<CarShopUserEntity> hasher)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.Username.Trim();

        var user = await ctx.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct)
            ?? throw new CarShopNotFoundException("Korisnik nije pronađen.");

        if (await ctx.Users.AnyAsync(x => x.Id != request.Id && x.Email.ToLower() == email && !x.IsDeleted, ct))
            throw new CarShopConflictException("Email je već registrovan.");

        if (await ctx.Users.AnyAsync(x => x.Id != request.Id && x.Username.ToLower() == username.ToLower() && !x.IsDeleted, ct))
            throw new CarShopConflictException("Korisničko ime je već zauzeto.");

        var role = await ctx.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.RoleId && !x.IsDeleted, ct)
            ?? throw new CarShopNotFoundException("Uloga nije pronađena.");

        user.Username = username;
        user.Email = email;
        user.FirstName = request.FirstName.Trim();
        user.LastName = request.LastName.Trim();
        user.Phone = request.Phone?.Trim();
        user.Address = request.Address.Trim();
        user.RoleId = request.RoleId;
        user.IsActive = request.IsActive;

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
            user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

        await ctx.SaveChangesAsync(ct);

        user.Role = role;
        return UserDto.FromEntity(user);
    }
}