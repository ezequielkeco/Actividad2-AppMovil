using Marila_Garden_App.ViewModels;

namespace Marila_Garden_App.Views.Authentication;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}