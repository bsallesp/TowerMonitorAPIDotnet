using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TowerApi.Domain.Entities;

public class TowerLiveStatus
{
    [JsonProperty(PropertyName="id")] public string Id { get; set; }
    public long TowerId { get; set; }
    public TowerStatus Status { get; set; }
    public double TemperatureC { get; set; }
    public int BatteryPercent { get; set; }
    public int Rssi { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}