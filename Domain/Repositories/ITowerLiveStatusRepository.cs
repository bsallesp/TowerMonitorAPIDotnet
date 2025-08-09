namespace TowerApi.Domain.Repositories;

using TowerApi.Domain.Entities;

public interface ITowerLiveStatusRepository
{
    Task<TowerLiveStatus?> GetByIdAsync(Guid towerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TowerLiveStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TowerLiveStatus status, CancellationToken cancellationToken = default);
    Task UpdateAsync(TowerLiveStatus status, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid towerId, CancellationToken cancellationToken = default);
}