using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;
using TowerApi.Infrastructure.Data;

namespace TowerApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TowerController(SqlServerDbContext context) : ControllerBase
{
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Tower>>> GetAll(CancellationToken cancellationToken)
    {
        var towers = await context.Towers.AsNoTracking().ToListAsync(cancellationToken);
        return Ok(towers);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] long? afterId, [FromQuery] int pageSize = 100, CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, 1, 500);
        var query = context.Towers.AsNoTracking().OrderBy(t => t.Id);
        if (afterId.HasValue) query = query.Where(t => t.Id > afterId.Value).OrderBy(t => t.Id);
        var items = await query.Take(pageSize + 1).ToListAsync(ct);
        var hasMore = items.Count > pageSize;
        if (hasMore) items.RemoveAt(pageSize);
        var nextCursor = hasMore ? items.Last().Id : (long?)null;
        return Ok(new { items, nextCursor });
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Tower>> GetById(long id, CancellationToken cancellationToken)
    {
        var tower = await context.Towers.FindAsync([id], cancellationToken);
        if (tower == null) return NotFound();
        return Ok(tower);
    }
}