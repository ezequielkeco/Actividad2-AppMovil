using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Helpers;


namespace Marila_Garden_App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private const string ValidUserName = "Ezequiel";
        private const string ValidPassword = "123456";

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string userNameError = string.Empty;

        [ObservableProperty]
        private string passwordError = string.Empty;

        [ObservableProperty]
        private string authenticationError = string.Empty;

        partial void OnUserNameChanged(string value)
        {
            UserNameError = string.Empty;
            AuthenticationError = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            PasswordError = string.Empty;
            AuthenticationError = string.Empty;
        }

        [RelayCommand]
        private async Task Login()
        {
            UserNameError = string.Empty;
            PasswordError = string.Empty;
            AuthenticationError = string.Empty;

            bool isValid = true;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                UserNameError = "El nombre de usuario es obligatorio.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError = "La contraseña es obligatoria.";
                isValid = false;
            }

            if (!isValid)
                return;

            if (UserName.Trim() == ValidUserName &&
                Password.Trim() == ValidPassword)
            {
                SessionHelper.IsLoggedIn = true;
                SessionHelper.UserName = UserName.Trim();

                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

                await Shell.Current.GoToAsync("//Home");
            }
            else
            {
                if (UserName.Trim() != ValidUserName)
                    UserNameError = "El nombre de usuario no es válido.";

                if (Password.Trim() != ValidPassword)
                    PasswordError = "La contraseña no es válida.";
            }
        }
    }
}