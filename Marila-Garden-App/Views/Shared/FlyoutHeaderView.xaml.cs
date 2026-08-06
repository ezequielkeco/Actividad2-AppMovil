using Marila_Garden_App.ViewModels.Shared;

namespace Marila_Garden_App.Views.Shared;

public partial class FlyoutHeaderView : ContentView
{
    private FlyoutHeaderViewModel? _viewModel;

    public FlyoutHeaderView()
    {
        InitializeComponent();
    }

    public void Configure(
        FlyoutHeaderViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;

        _viewModel.RefreshSessionData();
    }
}