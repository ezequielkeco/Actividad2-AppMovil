using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Microsoft.Maui.ApplicationModel;

namespace Marila_Garden_App.ViewModels
{
    public partial class RequestViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public RequestViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string fullName = string.Empty;

        [ObservableProperty]
        private string phone = string.Empty;

        [ObservableProperty]
        private string selectedServiceType = string.Empty;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private string comments = string.Empty;

        [ObservableProperty]
        private string fullNameError = string.Empty;

        [ObservableProperty]
        private string phoneError = string.Empty;

        [ObservableProperty]
        private string serviceTypeError = string.Empty;

        [ObservableProperty]
        private string dateError = string.Empty;

        [ObservableProperty]
        private string commentsError = string.Empty;

        [ObservableProperty]
        private string successMessage = string.Empty;

        public bool HasSuccessMessage => !string.IsNullOrWhiteSpace(SuccessMessage);

        public List<string> ServiceTypes { get; } = new()
        {
            "Diseño de jardín",
            "Mantenimiento",
            "Poda profesional",
            "Plantación"
        };

        partial void OnFullNameChanged(string value)
        {
            FullNameError = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnPhoneChanged(string value)
        {
            PhoneError = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnSelectedServiceTypeChanged(string value)
        {
            ServiceTypeError = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnSelectedDateChanged(DateTime value)
        {
            DateError = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnCommentsChanged(string value)
        {
            CommentsError = string.Empty;
            SuccessMessage = string.Empty;
        }

        partial void OnSuccessMessageChanged(string value)
        {
            OnPropertyChanged(nameof(HasSuccessMessage));
        }

        [RelayCommand]
        private async Task SaveRequest()
        {
            ClearMessages();

            if (!ValidateForm())
                return;

            var request = new ServiceRequest
            {
                FullName = FullName.Trim(),
                Phone = Phone.Trim(),
                ServiceType = SelectedServiceType,
                DesiredDate = SelectedDate,
                Comments = Comments.Trim()
            };

            await _databaseService.AddRequestAsync(request);

            ClearForm();

            SuccessMessage = "✅ Solicitud registrada correctamente.";

            _ = HideSuccessMessageAsync();
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(FullName))
            {
                FullNameError = "El nombre completo es obligatorio.";
                isValid = false;
            }
            else if (FullName.Trim().Length < 5)
            {
                FullNameError = "El nombre debe tener al menos 5 caracteres.";
                isValid = false;
            }
            else if (FullName.Any(char.IsDigit))
            {
                FullNameError = "El nombre no debe contener números.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(Phone))
            {
                PhoneError = "El teléfono es obligatorio.";
                isValid = false;
            }
            else
            {
                string cleanPhone = new string(Phone.Where(char.IsDigit).ToArray());

                if (cleanPhone.Length != 10)
                {
                    PhoneError = "El teléfono debe tener 10 dígitos.";
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(SelectedServiceType))
            {
                ServiceTypeError = "Debes seleccionar un tipo de servicio.";
                isValid = false;
            }

            if (SelectedDate.Date < DateTime.Today)
            {
                DateError = "La fecha no puede ser anterior al día de hoy.";
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(Comments) && Comments.Length > 300)
            {
                CommentsError = "Los comentarios no deben superar los 300 caracteres.";
                isValid = false;
            }

            return isValid;
        }

        private void ClearMessages()
        {
            FullNameError = string.Empty;
            PhoneError = string.Empty;
            ServiceTypeError = string.Empty;
            DateError = string.Empty;
            CommentsError = string.Empty;
            SuccessMessage = string.Empty;
        }

        private void ClearForm()
        {
            FullName = string.Empty;
            Phone = string.Empty;
            SelectedServiceType = string.Empty;
            SelectedDate = DateTime.Today;
            Comments = string.Empty;
        }

        private async Task HideSuccessMessageAsync()
        {
            await Task.Delay(3000);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                SuccessMessage = string.Empty;
            });
        }
    }
}