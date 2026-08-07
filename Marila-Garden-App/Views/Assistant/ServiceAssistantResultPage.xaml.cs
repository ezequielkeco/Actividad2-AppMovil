using Marila_Garden_App.ViewModels.Assistant;

namespace Marila_Garden_App.Views.Assistant;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ServiceAssistantResultPage : ContentPage
{
    private readonly ServiceAssistantResultViewModel _viewModel;

    private string serviceId = string.Empty;

    public string ServiceId
    {
        get => serviceId;
        set
        {
            serviceId =
                Uri.UnescapeDataString(
                    value ?? string.Empty);

            _viewModel.LoadService(serviceId);
        }
    }

    public ServiceAssistantResultPage(
        ServiceAssistantResultViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void OnMenuClicked(
        object sender,
        EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}