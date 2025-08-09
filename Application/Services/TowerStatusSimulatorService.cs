using TowerApi.Application.Services.Interfaces;
using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories.Interfaces;
using TowerApi.Infrastructure.Repositories;

namespace TowerApi.Application.Services;

public class TowerStatusSimulatorService(ITowerRepository towerRepository, ITowerLiveStatusRepository liveRepository) : ITowerStatusSimulatorService
{
    public async Task GenerateBatchAsync(int count, CancellationToken ct = default)
    {
        var towers = await towerRepository.GetIdsAsync(ct);
        if (towers.Count == 0) return;
        var rnd = new Random();
        var now = DateTimeOffset.UtcNow;
        var tasks = new List<Task>(count);
        for (var i = 0; i < count; i++)
        {
            var towerId = towers[rnd.Next(towers.Count)];
            var status = new TowerLiveStatus
            {
                Id = Guid.NewGuid().ToString(),
                TowerId = towerId,
                Status = rnd.Next(0, 100) < 92 ? TowerStatus.Online : TowerStatus.Offline,
                TemperatureC = 20 + rnd.NextDouble() * 25,
                BatteryPercent = 40 + rnd.Next(61),
                Rssi = -120 + rnd.Next(60),
                Timestamp = now.AddSeconds(-rnd.Next(0, 60))
            };
            tasks.Add(liveRepository.UpdateAsync(status, ct));
        }
        await Task.WhenAll(tasks);
    }
}