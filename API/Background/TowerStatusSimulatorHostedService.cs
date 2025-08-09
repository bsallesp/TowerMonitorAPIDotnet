using TowerApi.Application.Services.Interfaces;

namespace TowerApi.API.Background;

public class TowerStatusSimulatorHostedService(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var simulator = scope.ServiceProvider.GetRequiredService<ITowerStatusSimulatorService>();
            await simulator.GenerateBatchAsync(50, stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}