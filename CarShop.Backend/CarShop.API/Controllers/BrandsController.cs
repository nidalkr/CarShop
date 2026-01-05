using CarShop.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Controllers;

[ApiController]
[Route("api/brands")]
public sealed class BrandsController : ControllerBase
{
    private readonly DatabaseContext _ctx;
    public BrandsController(DatabaseContext ctx) => _ctx = ctx;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<LookupItemDto>>> Get([FromQuery] string? search, CancellationToken ct)
    {
        var query = _ctx.Brands
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(x => x.BrandName.ToLower().Contains(s));
        }

        var items = await query
            .OrderBy(x => x.BrandName)
            .Select(x => new LookupItemDto { Id = x.Id, Name = x.BrandName })
            .ToListAsync(ct);

        return Ok(items);
    }
}

public sealed class LookupItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
