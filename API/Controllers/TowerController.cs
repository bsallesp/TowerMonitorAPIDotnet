using Microsoft.AspNetCore.Mvc;
using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories;

namespace TowerApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TowerController(ITowerRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tower>>> GetAll()
    {
        var towers = await repository.GetAllAsync();
        return Ok(towers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tower>> GetById(Guid id)
    {
        var tower = await repository.GetByIdAsync(id);
        if (tower == null)
            return NotFound();
        return Ok(tower);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Tower tower)
    {
        await repository.AddAsync(tower);
        return CreatedAtAction(nameof(GetById), new { id = tower.Id }, tower);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] Tower tower)
    {
        if (id != tower.Id)
            return BadRequest();

        await repository.UpdateAsync(tower);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}