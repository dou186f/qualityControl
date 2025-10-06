using qualityControl.MOBILE.ViewModels;

namespace qualityControl.MOBILE.Pages;
public partial class WorkOrderPage : ContentPage
{
	public WorkOrderPage(WorkOrderViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm; 

        vm.PropertyChanged += async (_, e) =>
        {
            if (e.PropertyName == nameof(vm.ErrorMessage) && !string.IsNullOrWhiteSpace(vm.ErrorMessage))
            {
                await DisplayAlert("Error", vm.ErrorMessage, "OK");
            }
        };
	}

	protected override async void OnAppearing()
{
    base.OnAppearing();

    if (BindingContext is WorkOrderViewModel vm &&
        vm.WorkOrders.Count == 0 &&
        !vm.IsBusy)
    {
        await vm.RunSearch();
    }
}
}