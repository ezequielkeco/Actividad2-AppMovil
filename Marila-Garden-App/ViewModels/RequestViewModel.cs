using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels
{
    public partial class RequestViewModel : ObservableObject
    {
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

        public List<string> ServiceTypes { get; } = new()
        {
            "Diseño de jardín",
            "Mantenimiento",
            "Poda profesional",
            "Plantación"
        };

        [RelayCommand]
        private void SaveRequest()
        {
            ClearMessages();

            if (!ValidateForm())
                return;

            var request = new ServiceRequest
            {
                FullName = fullName.Trim(),
                Phone = phone.Trim(),
                ServiceType = selectedServiceType,
                DesiredDate = selectedDate,
                Comments = comments.Trim()
            };

            ServiceRequestMemoryService.Add(request);

            successMessage = "Solicitud registrada con éxito.";

            ClearForm();
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                fullNameError = "El nombre completo es obligatorio.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                phoneError = "El teléfono es obligatorio.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(selectedServiceType))
            {
                serviceTypeError = "Debes seleccionar un tipo de servicio.";
                isValid = false;
            }

            if (selectedDate.Date < DateTime.Today)
            {
                dateError = "La fecha no puede ser anterior al día de hoy.";
                isValid = false;
            }

            if (!string.IsNullOrWhiteSpace(comments) && comments.Length > 300)
            {
                commentsError = "Los comentarios no deben superar los 300 caracteres.";
                isValid = false;
            }

            return isValid;
        }

        private void ClearMessages()
        {
            fullNameError = string.Empty;
            phoneError = string.Empty;
            serviceTypeError = string.Empty;
            dateError = string.Empty;
            commentsError = string.Empty;
            successMessage = string.Empty;
        }

        private void ClearForm()
        {
            fullName = string.Empty;
            phone = string.Empty;
            selectedServiceType = string.Empty;
            selectedDate = DateTime.Today;
            comments = string.Empty;
        }
    }
}
