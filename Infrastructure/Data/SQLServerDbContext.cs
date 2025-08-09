using Microsoft.EntityFrameworkCore;
using TowerApi.Domain.Entities;
using TowerApi.Infrastructure.Data.Configurations;

namespace TowerApi.Infrastructure.Data;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : DbContext(options)
{
    public DbSet<Tower> Towers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TowerConfiguration());
    }

}