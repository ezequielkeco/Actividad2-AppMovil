using Marila_Garden_App.Helpers;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.RequestsHistory;

public partial class RequestsHistoryPage : ContentPage
{
    public RequestsHistoryPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = SessionHelper.UserName;

        LoadRequests();
    }
    private void LoadRequests()
    {
        var requests = ServiceRequestMemoryService.GetAll();

        RequestsCollectionView.ItemsSource = requests;

        EmptyStateLayout.IsVisible = requests.Count == 0;
        RequestsCollectionView.IsVisible = requests.Count > 0;
    }
    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}