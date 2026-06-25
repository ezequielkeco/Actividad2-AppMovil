using Marila_Garden_App.Helpers;

namespace Marila_Garden_App.Views.Request;

public partial class RequestPage : ContentPage
{
	public RequestPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = SessionHelper.UserName;
    }
    private void OnMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}