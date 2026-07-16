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

    private async void OnDeleteRequestClicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton deleteButton)
            return;

        if (deleteButton.CommandParameter is not ServiceRequest request)
            return;

        if (BindingContext is not RequestsHistoryViewModel viewModel)
            return;

        bool confirmed = await viewModel.ConfirmDeleteRequestAsync(request);

        if (!confirmed)
            return;

        Element? currentElement = deleteButton;

        while (currentElement is not null &&
               currentElement is not Border)
        {
            currentElement = currentElement.Parent;
        }

        Border? requestCard = currentElement as Border;

        try
        {
            if (requestCard is not null)
            {
                await Task.WhenAll(
                    requestCard.FadeToAsync(0, 220, Easing.CubicIn),
                    requestCard.ScaleToAsync(0.96, 220, Easing.CubicIn)
                );
            }

            await viewModel.DeleteConfirmedRequestAsync(request);
        }
        finally
        {
            if (requestCard is not null)
            {
                requestCard.Opacity = 1;
                requestCard.Scale = 1;
                requestCard.IsVisible = true;
            }
        }
    }
}