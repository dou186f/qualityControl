using qualityControl.MOBILE.ViewModels;

namespace qualityControl.MOBILE.Pages;

public partial class WorkOrderDetailPage : ContentPage
{
	public WorkOrderDetailPage()
	: this(MauiProgram.Services.GetRequiredService<DetailViewModel>()) { }

	public WorkOrderDetailPage(DetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}