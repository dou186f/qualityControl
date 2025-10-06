namespace qualityControl.SHARED.Dtos
{
    public class QcChecklistItemDto
    {
        public int QcRef { get; set; }
        public string Name { get; set; } = "";
        public int? SetRef { get; set; }
        public double? MinVal { get; set; }
        public double? MaxVal { get; set; }
        public bool? Result { get; set; }
        public int? ResultLogicalRef { get; set; }
    }
}
