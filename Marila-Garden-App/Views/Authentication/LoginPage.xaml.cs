using Marila_Garden_App.ViewModels.Authentication;

namespace Marila_Garden_App.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}