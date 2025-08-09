using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;
using TowerApi.Domain.Repositories;
using TowerApi.Infrastructure.Data;

namespace TowerApi.Infrastructure.Repositories;

public class TowerRepository : ITowerRepository
{
    private readonly SQLServerDbContext _context;

    public TowerRepository(SQLServerDbContext context)
    {
        _context = context;
    }

    public async Task<Tower?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Towers.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Tower>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Towers.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        await _context.Towers.AddAsync(tower, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Tower tower, CancellationToken cancellationToken = default)
    {
        _context.Towers.Update(tower);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null) return;
        _context.Towers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}