using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Marila_Garden_App.Services;
using Microsoft.Maui.ApplicationModel;

namespace Marila_Garden_App.ViewModels.Base
{
    public partial class FormViewModelBase : ObservableObject
    {
        private readonly IDialogService _dialogService;

        protected FormViewModelBase(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        protected Task<bool> ConfirmAsync(
            string title,
            string message,
            string accept,
            string cancel)
        {
            return _dialogService.ConfirmAsync(
                title,
                message,
                accept,
                cancel);
        }

        [ObservableProperty]
        private bool isEditMode;

        [ObservableProperty]
        private string pageTitle = string.Empty;

        [ObservableProperty]
        private string submitButtonText = string.Empty;

        [ObservableProperty]
        private string successMessage = string.Empty;

        [ObservableProperty]
        private bool hasUnsavedChanges;

        public bool HasSuccessMessage =>
            !string.IsNullOrWhiteSpace(SuccessMessage);

        partial void OnSuccessMessageChanged(string value)
        {
            OnPropertyChanged(nameof(HasSuccessMessage));
        }

        public async Task<bool> ConfirmDiscardChangesAsync()
        {
            if (!HasUnsavedChanges)
                return true;

            bool confirmed = await _dialogService.ConfirmAsync(
                "Descartar cambios",
                "Los cambios no se han guardado. ¿Deseas salir sin guardar?",
                "Salir",
                "Continuar editando");

            if (confirmed)
            {
                HasUnsavedChanges = false;
            }

            return confirmed;
        }

        protected async Task HideSuccessMessageAsync(
            int milliseconds = 3000)
        {
            await Task.Delay(milliseconds);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                SuccessMessage = string.Empty;
            });
        }

        protected void ConfigureCreateMode(
            string pageTitle,
            string submitButtonText)
        {
            IsEditMode = false;
            PageTitle = pageTitle;
            SubmitButtonText = submitButtonText;
            HasUnsavedChanges = false;
        }

        protected void ConfigureEditMode(
            string pageTitle,
            string submitButtonText)
        {
            IsEditMode = true;
            PageTitle = pageTitle;
            SubmitButtonText = submitButtonText;
            HasUnsavedChanges = false;
        }
    }
}
