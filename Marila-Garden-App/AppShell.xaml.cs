using Marila_Garden_App.Views.Authentication;
using Marila_Garden_App.Views.Request;
using Marila_Garden_App.ViewModels.Request;
using Marila_Garden_App.Services;

namespace Marila_Garden_App
{
    public partial class AppShell : Shell
    {
        private readonly ISessionService _sessionService;
        public AppShell(ISessionService sessionService)
        {
            InitializeComponent();

            _sessionService = sessionService;

            Routing.RegisterRoute(
                nameof(RegisterPage),
                typeof(RegisterPage));
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (CurrentPage is not RequestPage requestPage)
                return;

            if (requestPage.BindingContext is not RequestViewModel viewModel)
                return;

            if (!viewModel.HasUnsavedChanges)
                return;

            string targetRoute =
                args.Target?.Location?.OriginalString ?? string.Empty;

            string cleanTargetRoute = targetRoute
                .Split('?')[0]
                .TrimEnd('/');

            bool isRequestFormRoute =
                cleanTargetRoute.EndsWith(
                    "/Request",
                    StringComparison.OrdinalIgnoreCase);

            if (isRequestFormRoute)
                return;

            if (!args.CanCancel)
                return;

            ShellNavigatingDeferral deferral = args.GetDeferral();

            try
            {
                bool canLeave =
                    await viewModel.ConfirmDiscardChangesAsync();

                if (!canLeave)
                {
                    args.Cancel();
                    return;
                }

                viewModel.ResetToCreateMode();
            }
            finally
            {
                deferral.Complete();
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            _sessionService.EndSession();

            FlyoutBehavior = FlyoutBehavior.Disabled;
            FlyoutIsPresented = false;

            await GoToAsync("//Login");
        }
    }
}
