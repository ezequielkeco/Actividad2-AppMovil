using Marila_Garden_App.ViewModels.Services;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.Services;

public partial class ServicesPage : ContentPage
{
    private readonly ServicesViewModel _viewModel;
	public ServicesPage(ServicesViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshSessionData();
    }
  
    private void OnMenuClicked(
        object sender,
        EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}