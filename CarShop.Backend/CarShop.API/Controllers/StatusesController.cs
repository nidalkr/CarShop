using CarShop.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarShop.API.Controllers;

[ApiController]
[Route("api/statuses")]
public sealed class StatusesController : ControllerBase
{
    private readonly DatabaseContext _ctx;
    public StatusesController(DatabaseContext ctx) => _ctx = ctx;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<StatusLookupDto>>> Get([FromQuery] string? search, CancellationToken ct)
    {
        var query = _ctx.Statuses
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(x => x.StatusName.ToLower().Contains(s));
        }

        var items = await query
            .OrderBy(x => x.StatusName)
            .Select(x => new StatusLookupDto
            {
                Id = x.Id,
                Name = x.StatusName,
                Description = x.Description
            })
            .ToListAsync(ct);

        return Ok(items);
    }
}

public sealed class StatusLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
