using Marila_Garden_App.ViewModels.Services;

namespace Marila_Garden_App.Views.Services;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ServiceDetailPage : ContentPage
{
    private readonly ServiceDetailViewModel _viewModel;

    private string serviceId = string.Empty;

    public string ServiceId
    {
        get => serviceId;
        set
        {
            serviceId = Uri.UnescapeDataString(value ?? string.Empty);
            _viewModel.LoadService(serviceId);
        }
    }

    public ServiceDetailPage(
        ServiceDetailViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private async void OnMenuClicked(
        object sender,
        EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshSessionData();
    }
}