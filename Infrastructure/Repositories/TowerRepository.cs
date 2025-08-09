using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;
using TowerApi.Infrastructure.Data;

namespace TowerApi.Infrastructure.Repositories;

public class TowerRepository(SqlServerDbContext context) : ITowerRepository
{
    public async Task<Tower?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await context.Towers.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Tower>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Towers.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<long>> GetIdsAsync(CancellationToken cancellationToken = default)
    {
        return await context.Towers.AsNoTracking().OrderBy(t => t.Id).Select(t => t.Id).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        await context.Towers.AddAsync(tower, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        context.Towers.Update(tower);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) return;
        context.Towers.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}