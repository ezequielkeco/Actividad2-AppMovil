using Marila_Garden_App.Helpers;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Marila_Garden_App.Views.Request;

public partial class RequestPage : ContentPage
{
    public RequestPage()
    {
        InitializeComponent();

        BindingContext = App.Current!.Handler!.MauiContext!.Services.GetService<RequestViewModel>();

        DesiredDatePicker.MinimumDate = DateTime.Today;
        DesiredDatePicker.Date = DateTime.Today;

        SelectedDateFieldLabel.Text = "Selecciona una fecha";
        SelectedDateLabel.Text = "No has seleccionado una fecha";
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

    private void DesiredDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        UpdateSelectedDateLabel();
    }

    private void DesiredDatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DatePicker.Date))
        {
            UpdateSelectedDateLabel();
        }
    }

    private void UpdateSelectedDateLabel()
    {
        DateTime selectedDate = DesiredDatePicker.Date ?? DateTime.Today;

        string formattedDate = $"{selectedDate.Day:00}/{selectedDate.Month:00}/{selectedDate.Year}";

        SelectedDateFieldLabel.Text = formattedDate;
        SelectedDateLabel.Text = $"Fecha seleccionada: {formattedDate}";

        if (BindingContext is RequestViewModel viewModel)
        {
            viewModel.SelectedDate = selectedDate;
        }
    }
}