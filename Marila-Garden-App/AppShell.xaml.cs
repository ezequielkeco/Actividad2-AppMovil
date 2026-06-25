using Marila_Garden_App.Helpers;

namespace Marila_Garden_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            SessionHelper.IsLoggedIn = false;
            SessionHelper.UserName = string.Empty;

            FlyoutBehavior = FlyoutBehavior.Disabled;
            FlyoutIsPresented = false;

            await GoToAsync("//Login");
        }
    }
}
