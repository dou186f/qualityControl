using qualityControl.MOBILE.ViewModels;

namespace qualityControl.MOBILE.Pages;
public partial class QualityControlAdderPage : ContentPage
{
	public QualityControlAdderPage()
	: this(MauiProgram.Services.GetRequiredService<QualityControlAdderViewModel>()) { }

	public QualityControlAdderPage(QualityControlAdderViewModel  vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}