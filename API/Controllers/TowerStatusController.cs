using Microsoft.AspNetCore.Mvc;
using TowerApi.Application.Services;

namespace TowerApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TowerStatusController(TowerStatusAggregationService aggregationService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await aggregationService.GetAllWithLiveStatusAsync(ct);
        return Ok(result);
    }
}