using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.Request;

public partial class RequestPage : ContentPage, IQueryAttributable
{
    private readonly ISessionService _sessionService;

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "User";

    private bool _isProcessingBackNavigation;

    protected override bool OnBackButtonPressed()
    {
        if (_isProcessingBackNavigation)
            return true;

        _ = HandleBackNavigationAsync();

        return true;
    }

    private async Task HandleBackNavigationAsync()
    {
        if (_isProcessingBackNavigation)
            return;

        _isProcessingBackNavigation = true;

        try
        {
            if (BindingContext is not RequestViewModel viewModel)
                return;

            bool canLeave = await viewModel.ConfirmDiscardChangesAsync();

            if (!canLeave)
                return;

            bool wasEditing = viewModel.IsEditMode;

            viewModel.ResetToCreateMode();

            if (wasEditing)
            {
                await Shell.Current.GoToAsync("//RequestsHistory");
            }
            else
            {
                await Shell.Current.GoToAsync("//Home");
            }
        }
        finally
        {
            _isProcessingBackNavigation = false;
        }
    }

    private bool _openedForEdit;

    public RequestPage(ISessionService sessionService)
    {
        InitializeComponent();

        _sessionService = sessionService;

        BindingContext = this;

        BindingContext = App.Current!.Handler!.MauiContext!.Services.GetService<RequestViewModel>();

        DesiredDatePicker.MinimumDate = DateTime.Today;
        DesiredDatePicker.Date = DateTime.Today;

        SelectedDateFieldLabel.Text = "Selecciona una fecha";
        SelectedDateLabel.Text = "No has seleccionado una fecha";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_openedForEdit &&
            BindingContext is RequestViewModel viewModel)
        {
            viewModel.ResetToCreateMode();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _openedForEdit = false;
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

    public async void ApplyQueryAttributes(
    IDictionary<string, object> query)
    {
        if (BindingContext is not RequestViewModel viewModel)
            return;

        if (query.TryGetValue("requestId", out object? value) &&
            int.TryParse(value?.ToString(), out int requestId))
        {
            _openedForEdit = true;

            await viewModel.LoadRequestForEditAsync(requestId);
        }
        else
        {
            _openedForEdit = false;
            viewModel.ResetToCreateMode();
        }
    }
}