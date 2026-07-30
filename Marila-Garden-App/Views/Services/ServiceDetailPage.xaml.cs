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
}