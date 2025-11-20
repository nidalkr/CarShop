namespace CarShop.Application.Modules.Users.Commands.Delete;

public sealed class DeleteUserCommandHandler(IAppDbContext ctx) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request,CancellationToken ct)
    {
        var user = await ctx.Users.Include(x => x.RefreshTokens).FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct) ??
            throw new CarShopNotFoundException("Korisnik nije pronadjen");

        if (user.RefreshTokens.Count > 0)
            ctx.RefreshTokens.RemoveRange(user.RefreshTokens);

        ctx.Users.Remove(user);
        await ctx.SaveChangesAsync(ct);
    }
}
