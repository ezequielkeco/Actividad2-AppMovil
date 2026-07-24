using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Helpers;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Marila_Garden_App.ViewModels.Authentication
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private string fullName = string.Empty;

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private string fullNameError = string.Empty;

        [ObservableProperty]
        private string userNameError = string.Empty;

        [ObservableProperty]
        private string emailError = string.Empty;

        [ObservableProperty]
        private string passwordError = string.Empty;

        [ObservableProperty]
        private string confirmPasswordError = string.Empty;

        [ObservableProperty]
        private string generalError = string.Empty;

        public bool HasGeneralError =>
            !string.IsNullOrWhiteSpace(GeneralError);

        partial void OnGeneralErrorChanged(string value)
        {
            OnPropertyChanged(nameof(HasGeneralError));
        }

        [ObservableProperty]
        private bool isBusy;

        public RegisterViewModel(
            DatabaseService databaseService,
            INavigationService navigationService)
        {
            _databaseService = databaseService;
            _navigationService = navigationService;
        }

        partial void OnFullNameChanged(string value)
        {
            FullNameError = string.Empty;
            GeneralError = string.Empty;
        }

        partial void OnUserNameChanged(string value)
        {
            UserNameError = string.Empty;
            GeneralError = string.Empty;
        }

        partial void OnEmailChanged(string value)
        {
            EmailError = string.Empty;
            GeneralError = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            PasswordError = string.Empty;
            GeneralError = string.Empty;
        }

        partial void OnConfirmPasswordChanged(string value)
        {
            ConfirmPasswordError = string.Empty;
            GeneralError = string.Empty;
        }

        [RelayCommand]
        private async Task Register()
        {
            if (IsBusy)
                return;

            ClearErrors();

            if (!ValidateForm())
                return;

            try
            {
                IsBusy = true;

                string normalizedUserName =
                    UserName.Trim().ToLowerInvariant();

                string normalizedEmail =
                    Email.Trim().ToLowerInvariant();

                if (await _databaseService
                    .UserNameExistsAsync(normalizedUserName))
                {
                    UserNameError =
                        "Este nombre de usuario ya está registrado.";

                    return;
                }

                if (await _databaseService
                    .EmailExistsAsync(normalizedEmail))
                {
                    EmailError =
                        "Este correo electrónico ya está registrado.";

                    return;
                }

                User user = new()
                {
                    FullName = FullName.Trim(),
                    UserName = normalizedUserName,
                    Email = normalizedEmail,
                    PasswordHash = PasswordHasher.Hash(Password),
                    CreatedAt = DateTime.Now
                };

                await _databaseService.CreateUserAsync(user);

                ClearForm();

                await _navigationService.GoBackAsync();
            }
            catch (SQLite.SQLiteException exception)
            {
                GeneralError =
                    "No fue posible crear la cuenta. Verifica los datos e inténtalo nuevamente.";

                System.Diagnostics.Debug.WriteLine(exception);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(FullName))
            {
                FullNameError =
                    "El nombre completo es obligatorio.";

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                UserNameError =
                    "El nombre de usuario es obligatorio.";

                isValid = false;
            }
            else if (UserName.Trim().Length < 3)
            {
                UserNameError =
                    "El nombre de usuario debe tener al menos 3 caracteres.";

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                EmailError =
                    "El correo electrónico es obligatorio.";

                isValid = false;
            }
            else if (!IsValidEmail(Email))
            {
                EmailError =
                    "Ingresa un correo electrónico válido.";

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordError =
                    "La contraseña es obligatoria.";

                isValid = false;
            }
            else if (Password.Length < 6)
            {
                PasswordError =
                    "La contraseña debe tener al menos 6 caracteres.";

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ConfirmPasswordError =
                    "Debes confirmar la contraseña.";

                isValid = false;
            }
            else if (Password != ConfirmPassword)
            {
                ConfirmPasswordError =
                    "Las contraseñas no coinciden.";

                isValid = false;
            }

            return isValid;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address =
                    new(email.Trim());

                return address.Address ==
                    email.Trim();
            }
            catch
            {
                return false;
            }
        }

        private void ClearErrors()
        {
            FullNameError = string.Empty;
            UserNameError = string.Empty;
            EmailError = string.Empty;
            PasswordError = string.Empty;
            ConfirmPasswordError = string.Empty;
            GeneralError = string.Empty;
        }

        private void ClearForm()
        {
            FullName = string.Empty;
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
    }
}
