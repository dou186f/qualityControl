namespace qualityControl.SHARED.Dtos
{
    public sealed class WorkOrderDto
    {
        public int LogicalRef { set; get; }
        public string? LineNo { set; get; }
        public DateTime? OpBegDate { set; get; }
        public int ProdordRef { set; get; }
        public int ItemRef { set; get; }
        public string? ItemName { set; get; }
        public string? ItemCode { set; get; }
        public double? ProductionActamount { set; get; }
        public double? ProductionPlnamount { set; get; }

        public string ActPlnPercentageDisplay
        {
            get
            {
                if (ProductionPlnamount is null or <= 0 || ProductionActamount is null)
                {
                    return $"{ProductionActamount?.ToString() ?? "N/A"}/{ProductionPlnamount?.ToString() ?? "N/A"} (N/A)";
                }
                double? percentage = (double?) ProductionActamount / ProductionPlnamount * 100;
                string percentText = $"{percentage:F1}%";
                if (percentage >= 100)
                    percentText += " (finished)";
                return $"{ProductionActamount}/{ProductionPlnamount} ~ {percentText}";
            }
        }
    }
}
