using CarShop.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Controllers;

[ApiController]
[Route("api/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly DatabaseContext _ctx;
    public CategoriesController(DatabaseContext ctx) => _ctx = ctx;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LookupItemDto>>> Get([FromQuery] string? search, CancellationToken ct)
    {
        var query = _ctx.Categories
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(x => x.CategoryName.ToLower().Contains(s));
        }

        var items = await query
            .OrderBy(x => x.CategoryName)
            .Select(x => new LookupItemDto { Id = x.Id, Name = x.CategoryName })
            .ToListAsync(ct);

        return Ok(items);
    }
}
