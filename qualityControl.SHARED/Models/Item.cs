namespace qualityControl.SHARED.Models
{
    public class Item
    {
        public int LogicalRef { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? QccSetRef { get; set; }
    }
}
