namespace TowerApi.Domain.Entities;

public class Tower
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public TowerStatus Status { get; private set; }
    public DateTimeOffset LastMaintenance { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public Tower(string name, double latitude, double longitude, TowerStatus status = TowerStatus.Active)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name is required", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Status = status;
        LastMaintenance = DateTimeOffset.UtcNow;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateStatus(TowerStatus status)
    {
        Status = status;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetLastMaintenance(DateTimeOffset date)
    {
        LastMaintenance = date;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name is required", nameof(name));
        Name = name;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}