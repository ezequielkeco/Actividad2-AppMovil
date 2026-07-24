using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Home;

namespace Marila_Garden_App.Views.Home;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
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