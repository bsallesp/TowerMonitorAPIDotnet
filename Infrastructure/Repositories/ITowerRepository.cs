using TowerApi.Domain.Entities;

namespace TowerApi.Domain.Repositories;

public interface ITowerRepository
{
    Task<Tower?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tower>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Tower tower, CancellationToken cancellationToken = default);
    Task UpdateAsync(Tower tower, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}