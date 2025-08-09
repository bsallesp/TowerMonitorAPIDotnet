namespace TowerApi.Application.Services.Interfaces;

public interface ITowerStatusSimulatorService
{
    Task GenerateBatchAsync(int count, CancellationToken ct = default);
}