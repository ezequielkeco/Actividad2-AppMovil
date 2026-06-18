using Marila_Garden_App.Helpers;

namespace Marila_Garden_App.Views.Home;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        UserNameLabel.Text = SessionHelper.UserName;
    }
}