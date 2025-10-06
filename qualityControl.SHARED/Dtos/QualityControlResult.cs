namespace qualityControl.SHARED.Dtos
{
    public sealed class QualityControlResult
    {
        public int LogicalRef { get; set; }
        public int WorkOrderRef { get; set; }
        public int QcRef { get; set; }
        public string? Name { get; set; }
        public int? SetRef { get; set; }
        public bool Result {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
