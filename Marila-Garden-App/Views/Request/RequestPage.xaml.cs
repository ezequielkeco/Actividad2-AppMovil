using Marila_Garden_App.Helpers;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.Request;

public partial class RequestPage : ContentPage
{
	public RequestPage()
	{
		InitializeComponent();

        DesiredDatePicker.MinimumDate = DateTime.Today;
        DesiredDatePicker.Date = DateTime.Today;

        SelectedDateFieldLabel.Text = "Selecciona una fecha";
        SelectedDateLabel.Text = "No has seleccionado una fecha";

    }
    private void DesiredDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DatePicker.Date))
        {
            UpdateSelectedDateLabel();
        }
    }
    private void DesiredDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        UpdateSelectedDateLabel();
    }

    private void DesiredDatePicker_Unfocused(object sender, FocusEventArgs e)
    {
        UpdateSelectedDateLabel();
    }
    private void UpdateSelectedDateLabel()
    {
        DateTime selectedDate = DesiredDatePicker.Date ?? DateTime.Today;

        string formattedDate = $"{selectedDate.Day:00}/{selectedDate.Month:00}/{selectedDate.Year}";

        SelectedDateFieldLabel.Text = formattedDate;
        SelectedDateLabel.Text = $"Fecha seleccionada: {formattedDate}";
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = SessionHelper.UserName;
    }
    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private void OnDateFieldTapped(object sender, TappedEventArgs e)
    {
        DesiredDatePicker.Focus();
    }
    private async void OnSaveRequestClicked(object sender, EventArgs e)
    {
        string fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
        string phone = PhoneEntry.Text?.Trim() ?? string.Empty;
        string serviceType = ServiceTypePicker.SelectedItem?.ToString() ?? string.Empty;
        string comments = CommentsEditor.Text?.Trim() ?? string.Empty;
        DateTime desiredDate = DesiredDatePicker.Date ?? DateTime.Today;

        if (string.IsNullOrWhiteSpace(fullName))
        {
            await DisplayAlertAsync("Campo requerido", "Debes ingresar el nombre completo.", "Aceptar");
            return;
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlertAsync("Campo requerido", "Debes ingresar el teléfono.", "Aceptar");
            return;
        }

        if (string.IsNullOrWhiteSpace(serviceType))
        {
            await DisplayAlertAsync("Campo requerido", "Debes seleccionar el tipo de servicio.", "Aceptar");
            return;
        }

        if (desiredDate.Date < DateTime.Today)
        {
            await DisplayAlertAsync("Fecha inválida", "La fecha deseada no puede ser anterior al día de hoy.", "Aceptar");
            return;
        }

        var request = new ServiceRequest
        {
            FullName = fullName,
            Phone = phone,
            ServiceType = serviceType,
            DesiredDate = desiredDate,
            Comments = comments
        };

        ServiceRequestMemoryService.Add(request);

        await DisplayAlertAsync("Solicitud registrada", "Tu solicitud fue guardada correctamente.", "Aceptar");

        ClearForm();

        await Shell.Current.GoToAsync("//RequestsHistory");
    }

    private void ClearForm()
    {
        FullNameEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        ServiceTypePicker.SelectedIndex = -1;
        DesiredDatePicker.Date = DateTime.Today;
        SelectedDateFieldLabel.Text = "Selecciona una fecha";
        SelectedDateLabel.Text = "No has seleccionado una fecha";
        CommentsEditor.Text = string.Empty;
    }
}