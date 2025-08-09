namespace TowerApi.Domain.Entities;

public class TowerLiveStatus
{
    public long TowerId { get; set; }
    public string Status { get; set; } = "Unknown";
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
}
