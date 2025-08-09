namespace TowerApi.Domain.Repositories.Interfaces;

using TowerApi.Domain.Entities;

public interface ITowerLiveStatusRepository
{
    Task UpdateAsync(TowerLiveStatus status, CancellationToken ct = default);
    Task<IReadOnlyList<TowerLiveStatus>> GetAllAsync(CancellationToken ct = default);
}