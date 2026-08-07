using Marila_Garden_App.ViewModels.Assistant;

namespace Marila_Garden_App.Views.Assistant;

public partial class ServiceAssistantPage : ContentPage
{
    private readonly ServiceAssistantViewModel _viewModel;

    public ServiceAssistantPage(
        ServiceAssistantViewModel viewModel)
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