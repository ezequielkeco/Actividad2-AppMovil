using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Request;

namespace Marila_Garden_App.Views.Request;

public partial class RequestPage : ContentPage, IQueryAttributable
{

    private bool _isProcessingBackNavigation;

    private readonly IAnimationService _animationService;

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
    private bool _openedWithService;

    public RequestPage(RequestViewModel viewModel, IAnimationService animationService)
    {
        InitializeComponent();

        BindingContext = viewModel;
        _animationService = animationService;

        DesiredDatePicker.MinimumDate = DateTime.Today;
        DesiredDatePicker.Date = DateTime.Today;

        SelectedDateFieldLabel.Text = "Selecciona una fecha";
        SelectedDateLabel.Text = "No has seleccionado una fecha";

        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(
    object? sender,
    System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(RequestViewModel.HasSuccessMessage))
            return;

        if (sender is not RequestViewModel viewModel)
            return;

        if (!viewModel.HasSuccessMessage)
            return;

        MainThread.BeginInvokeOnMainThread(
            ShowSuccessMessageAsync);
    }

    private async void ShowSuccessMessageAsync()
    {
        await RequestScrollView.ScrollToAsync(
            0,
            0,
            true);

        SuccessMessageBorder.Opacity = 0;
        SuccessMessageBorder.TranslationY = -12;

        await Task.WhenAll(
            SuccessMessageBorder.FadeToAsync(
                1,
                250,
                Easing.CubicOut),

            SuccessMessageBorder.TranslateToAsync(
                0,
                0,
                250,
                Easing.CubicOut));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_openedForEdit &&
            !_openedWithService &&
            BindingContext is RequestViewModel viewModel)
        {
            viewModel.ResetToCreateMode();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _openedForEdit = false;
        _openedWithService = false;
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

        if (query.TryGetValue(
                "requestId",
                out object? requestValue) &&
            int.TryParse(
                requestValue?.ToString(),
                out int requestId))
        {
            _openedForEdit = true;
            _openedWithService = false;

            await viewModel.LoadRequestForEditAsync(requestId);

            return;
        }

        if (query.TryGetValue(
                "serviceId",
                out object? serviceValue))
        {
            _openedForEdit = false;
            _openedWithService = true;

            string serviceId =
                Uri.UnescapeDataString(
                    serviceValue?.ToString()
                    ?? string.Empty);

            viewModel.PrepareForService(serviceId);

            return;
        }

        _openedForEdit = false;
        _openedWithService = false;

        viewModel.ResetToCreateMode();
    }

    protected override void OnHandlerChanging(
    HandlerChangingEventArgs args)
    {
        if (args.OldHandler is not null &&
            BindingContext is RequestViewModel viewModel)
        {
            viewModel.PropertyChanged -=
                OnViewModelPropertyChanged;
        }

        base.OnHandlerChanging(args);
    }

}