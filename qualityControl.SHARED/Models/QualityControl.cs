namespace qualityControl.SHARED.Models
{
    public class QualityControl
    {
        public int LogicalRef { get; set; }
        public string? Name { get; set; }
        public int SetRef { get; set; }
        public double? MinVal { get; set; }
        public double? MaxVal { get; set; }
    }
}
