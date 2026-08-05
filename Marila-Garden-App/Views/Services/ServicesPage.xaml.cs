using Marila_Garden_App.ViewModels.Services;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.Services;

public partial class ServicesPage : ContentPage
{
    private readonly ServicesViewModel _viewModel;
    private readonly IAnimationService _animationService;
	public ServicesPage(ServicesViewModel viewModel, IAnimationService animationService)
	{
		InitializeComponent();

        _viewModel = viewModel;
        _animationService = animationService;

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

    private async void OnServiceCardTapped(
    object sender,
    TappedEventArgs e)
    {
        if (e.Parameter is not Border serviceCard)
            return;

        if (serviceCard.BindingContext is not ServiceInfo service)
            return;

        await _animationService.PressAsync(serviceCard);

        await _viewModel
            .OpenServiceDetailCommand
            .ExecuteAsync(service);
    }
}