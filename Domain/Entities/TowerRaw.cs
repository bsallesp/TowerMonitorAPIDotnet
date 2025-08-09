namespace TowerApi.Domain.Entities
{
    public class TowerRaw
    {
        public int Id { get; set; }
        public string Radio { get; set; }
        public int? Mcc { get; set; }
        public int? Net { get; set; }
        public int? Area { get; set; }
        public long? Cell { get; set; }
        public double? Unit { get; set; }
        public double? Lon { get; set; }
        public double? Lat { get; set; }
        public int? Range { get; set; }
        public string Samples { get; set; }
        public string Changeable { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public string AverageSignal { get; set; }
        public string StructureHeight { get; set; }
    }
}