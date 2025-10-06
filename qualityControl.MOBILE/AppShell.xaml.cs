using qualityControl.MOBILE.Pages;

namespace qualityControl.MOBILE
{
    public partial class AppShell : Shell
    {
        public const string WorkOrderDetailRoute = "workorderDetail";
        public const string QualityControlAdderRoute = "qualityControlAdder";
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(WorkOrderDetailRoute, typeof(WorkOrderDetailPage));
            Routing.RegisterRoute(QualityControlAdderRoute, typeof(QualityControlAdderPage));
        }
    }
}
