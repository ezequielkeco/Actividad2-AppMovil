using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Helpers;
using Marila_Garden_App.Services;
using Marila_Garden_App.Views.Authentication;

namespace Marila_Garden_App.ViewModels.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;

        public LoginViewModel(
            DatabaseService databaseService,
            INavigationService navigationService,
            ISessionService sessionService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
            _sessionService = sessionService;
        }

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

            var user =
                await _databaseService.GetUserByUserNameOrEmailAsync(UserName);

            if (user is null)
            {
                UserNameError =
                    "No existe una cuenta con ese usuario o correo electrónico.";

                return;
            }

            bool passwordCorrect =
                PasswordHasher.Verify(
                    Password,
                    user.PasswordHash);

            if (!passwordCorrect)
            {
                PasswordError =
                    "La contraseña es incorrecta.";

                return;
            }

            _sessionService.StartSession(user);

            Shell.Current.FlyoutBehavior =
                FlyoutBehavior.Flyout;

            await _navigationService.GoToAsync("//Home");
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await _navigationService.GoToAsync(
                nameof(RegisterPage));
        }
    }
}