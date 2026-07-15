using Marila_Garden_App.Helpers;
using Marila_Garden_App.Models;
using Marila_Garden_App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Marila_Garden_App.Views.RequestsHistory;

public partial class RequestsHistoryPage : ContentPage
{
    private readonly RequestsHistoryViewModel _viewModel;

    public RequestsHistoryPage()
    {
        InitializeComponent();

        _viewModel = App.Current!
            .Handler!
            .MauiContext!
            .Services
            .GetRequiredService<RequestsHistoryViewModel>();

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = SessionHelper.UserName;

        await _viewModel.LoadRequestsCommand.ExecuteAsync(null);
    }

    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void RequestsCollectionView_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not ServiceRequest request)
            return;

        if (BindingContext is RequestsHistoryViewModel viewModel)
        {
            await viewModel.EditRequestCommand.ExecuteAsync(request);
        }

        if (sender is CollectionView collectionView)
        {
            collectionView.SelectedItem = null;
        }
    }
}