using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;
using TowerApi.Infrastructure.Data;

namespace TowerApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TowerController(SqlServerDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tower>>> GetAll(CancellationToken cancellationToken)
    {
        var towers = await context.Towers.ToListAsync(cancellationToken);
        return Ok(towers);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Tower>> GetById(long id, CancellationToken cancellationToken)
    {
        var tower = await context.Towers.FindAsync(new object[] { id }, cancellationToken);
        if (tower == null)
            return NotFound();

        return Ok(tower);
    }
}