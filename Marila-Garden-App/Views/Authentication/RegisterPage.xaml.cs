using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Authentication;

namespace Marila_Garden_App.Views.Authentication;

public partial class RegisterPage : ContentPage
{
    private readonly INavigationService _navigationService;

    public RegisterPage(
        RegisterViewModel viewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _navigationService = navigationService;
    }

    private async void OnBackToLoginClicked(
        object sender,
        EventArgs e)
    {
        await _navigationService.GoBackAsync();
    }
}