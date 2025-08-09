using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories;

namespace TowerApi.Application.Services;

public class TowerService
{
    private readonly ITowerRepository _repository;

    public TowerService(ITowerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Tower>> GetAllTowersAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<Tower?> GetTowerByIdAsync(Guid id, CancellationToken ct = default)
        => await _repository.GetByIdAsync(id, ct);

    public async Task AddTowerAsync(Tower tower, CancellationToken ct = default)
        => await _repository.AddAsync(tower, ct);

    public async Task UpdateTowerAsync(Tower tower, CancellationToken ct = default)
        => await _repository.UpdateAsync(tower, ct);

    public async Task DeleteTowerAsync(Guid id, CancellationToken ct = default)
        => await _repository.DeleteAsync(id, ct);
}