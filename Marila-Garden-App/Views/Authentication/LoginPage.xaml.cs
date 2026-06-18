using Marila_Garden_App.Helpers;

namespace Marila_Garden_App.Views.Authentication;

public partial class LoginPage : ContentPage
{
    private const string ValidUserName = "Ezequiel";
    private const string ValidPassword = "123456";

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string userName = UserNameEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlertAsync("Campos requeridos", "Debes ingresar el usuario y contraseña.", "Aceptar");
            return;
        }

        if (userName == ValidUserName && password == ValidPassword)
        {
            SessionHelper.IsLoggedIn = true;

            SessionHelper.UserName = userName;

            await Shell.Current.GoToAsync("//Home");
        }
        else
        {
            await DisplayAlertAsync("Acceso denegado", "Nombre de usuario o contraseña incorrectos.", "Aceptar");
        }
    }

}