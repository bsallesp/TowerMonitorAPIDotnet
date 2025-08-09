using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories.Interfaces;
using TowerApi.Infrastructure.Repositories;

namespace TowerApi.API.Background;

public class TowerStatusSimulatorHostedService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly Dictionary<long, TowerLiveStatus> _towerStates = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var nextHeartbeat = DateTimeOffset.UtcNow;
        var nextIncidents = DateTimeOffset.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTimeOffset.UtcNow;

            if (now >= nextHeartbeat)
            {
                using var scope = scopeFactory.CreateScope();
                await SendHeartbeatBatchAsync(scope.ServiceProvider, stoppingToken);
                nextHeartbeat = now.AddSeconds(50 + Random.Shared.Next(20));
            }

            if (now >= nextIncidents)
            {
                using var scope2 = scopeFactory.CreateScope();
                await SendIncidentBatchAsync(scope2.ServiceProvider, stoppingToken);
                nextIncidents = now.AddSeconds(8 + Random.Shared.Next(5));
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task SendHeartbeatBatchAsync(IServiceProvider sp, CancellationToken ct)
    {
        var towerRepo = sp.GetRequiredService<ITowerRepository>();
        var liveRepo = sp.GetRequiredService<ITowerLiveStatusRepository>();
        var ids = await towerRepo.GetIdsAsync(ct);
        if (ids.Count == 0) return;

        var now = DateTimeOffset.UtcNow;
        var tasks = new List<Task>();

        foreach (var id in ids)
        {
            if (!_towerStates.TryGetValue(id, out var state))
            {
                state = new TowerLiveStatus
                {
                    Id = Guid.NewGuid().ToString(),
                    TowerId = id,
                    Status = TowerStatus.Online,
                    TemperatureC = 20 + Random.Shared.NextDouble() * 10,
                    BatteryPercent = 80 + Random.Shared.Next(21),
                    Rssi = -90 + Random.Shared.Next(20),
                    Timestamp = now
                };
                _towerStates[id] = state;
            }
            else
            {
                if (state.Status == TowerStatus.Online)
                {
                    state.TemperatureC += Random.Shared.NextDouble() * 2 - 1;
                    state.BatteryPercent -= (int)(Random.Shared.NextDouble() * 0.5);
                    state.Rssi += Random.Shared.Next(-1, 2);
                }
                state.Timestamp = now;
            }

            tasks.Add(liveRepo.UpdateAsync(state, ct));
        }

        await Task.WhenAll(tasks);
    }

    private async Task SendIncidentBatchAsync(IServiceProvider sp, CancellationToken ct)
    {
        var towerRepo = sp.GetRequiredService<ITowerRepository>();
        var liveRepo = sp.GetRequiredService<ITowerLiveStatusRepository>();
        var ids = await towerRepo.GetIdsAsync(ct);
        if (ids.Count == 0) return;

        var now = DateTimeOffset.UtcNow;
        var tasks = new List<Task>();

        foreach (var id in ids)
        {
            if (!_towerStates.TryGetValue(id, out var state))
                continue;

            switch (state.Status)
            {
                case TowerStatus.Online when Random.Shared.Next(100) < 3:
                    state.Status = TowerStatus.Offline;
                    state.BatteryPercent = Math.Max(0, state.BatteryPercent - Random.Shared.Next(5, 15));
                    state.Rssi = -200;
                    state.TemperatureC = 0;
                    break;
                case TowerStatus.Offline when Random.Shared.Next(100) < 20:
                    state.Status = TowerStatus.Online;
                    state.BatteryPercent = 50 + Random.Shared.Next(50);
                    state.Rssi = -95 + Random.Shared.Next(10);
                    state.TemperatureC = 18 + Random.Shared.NextDouble() * 10;
                    break;
                case TowerStatus.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state.Timestamp = now;
            tasks.Add(liveRepo.UpdateAsync(state, ct));
        }

        await Task.WhenAll(tasks);
    }
}
