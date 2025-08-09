namespace TowerApi.Domain.Entities;

public class Tower
{
    public long Id { get; set; }
    public string License { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string State { get; set; }
    public double? StructureHeight { get; set; }
    public string StructureType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}