using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories;
using TowerApi.Infrastructure.Repositories;

namespace TowerApi.Application.Services;

public class TowerService(ITowerRepository repository)
{
    public async Task<IEnumerable<Tower>> GetAllTowersAsync(CancellationToken ct = default)
        => await repository.GetAllAsync(ct);

    public async Task<Tower?> GetTowerByIdAsync(long id, CancellationToken ct = default)
        => await repository.GetByIdAsync(id, ct);

    public async Task AddTowerAsync(Tower tower, CancellationToken ct = default)
        => await repository.AddAsync(tower, ct);

    public async Task UpdateTowerAsync(Tower tower, CancellationToken ct = default)
        => await repository.UpdateAsync(tower, ct);

    public async Task DeleteTowerAsync(long id, CancellationToken ct = default)
        => await repository.DeleteAsync(id, ct);
}