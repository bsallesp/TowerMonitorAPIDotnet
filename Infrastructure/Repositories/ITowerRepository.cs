using TowerApi.Domain.Entities;

namespace TowerApi.Infrastructure.Repositories;

public interface ITowerRepository
{
    Task<Tower?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Tower>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<long>> GetIdsAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tower tower, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tower tower, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}