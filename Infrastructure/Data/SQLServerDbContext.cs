using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;

namespace TowerApi.Infrastructure.Data;

public class SQLServerDbContext : DbContext
{
    public SQLServerDbContext(DbContextOptions<SQLServerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tower> Towers => Set<Tower>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SQLServerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}