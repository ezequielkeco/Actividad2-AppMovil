using Marila_Garden_App.Views.Authentication;
using Marila_Garden_App.Views.Request;
using Marila_Garden_App.Views.Services;
using Marila_Garden_App.ViewModels.Request;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Shared;
using Marila_Garden_App.Views.Assistant;

namespace Marila_Garden_App
{
    public partial class AppShell : Shell
    {
        private readonly ISessionService _sessionService;
        private readonly IDialogService _dialogService;
        private readonly INavigationHistoryService _navigationHistoryService;
        private readonly FlyoutHeaderViewModel _flyoutHeaderViewModel;

        private bool _isNavigatingBackThroughHistory;
        public AppShell(ISessionService sessionService,
                        IDialogService dialogService,
                        INavigationHistoryService navigationHistoryService,
                        FlyoutHeaderViewModel flyoutHeaderViewModel)
        {
            InitializeComponent();

            _sessionService = sessionService;
            _dialogService = dialogService;
            _navigationHistoryService = navigationHistoryService;
            _flyoutHeaderViewModel = flyoutHeaderViewModel;

            FlyoutHeader.Configure(_flyoutHeaderViewModel);

            _flyoutHeaderViewModel.LogoutRequested +=
                OnFlyoutLogoutRequested;

            Routing.RegisterRoute(
                nameof(RegisterPage),
                typeof(RegisterPage));

            Routing.RegisterRoute(
                nameof(ServiceDetailPage),
                typeof(ServiceDetailPage));

            Routing.RegisterRoute(
                nameof(ServiceAssistantPage),
                typeof(ServiceAssistantPage));

            Routing.RegisterRoute(
                nameof(ServiceAssistantResultPage),
                typeof(ServiceAssistantResultPage));
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

        private bool IsCurrentPageHome()
        {
            string currentRoute =
                CurrentState?.Location?.OriginalString
                ?? string.Empty;

            string cleanRoute = currentRoute
                .Split('?')[0]
                .TrimEnd('/');

            return cleanRoute.EndsWith(
                "/Home",
                StringComparison.OrdinalIgnoreCase);
        }

        protected override bool OnBackButtonPressed()
        {
            if (IsCurrentPageHome())
            {
                _ = ConfirmLogoutFromBackAsync();

                return true;
            }

            string currentRoute =
                CurrentState?.Location?.OriginalString
                ?? string.Empty;

            string? currentMainRoute =
                GetMainRoute(currentRoute);

            if (currentMainRoute is null)
            {
                return base.OnBackButtonPressed();
            }

            _ = NavigateBackThroughHistoryAsync();

            return true;
        }

        private async Task NavigateBackThroughHistoryAsync()
        {
            if (_isNavigatingBackThroughHistory)
                return;

            string previousRoute =
                _navigationHistoryService.PeekPrevious()
                ?? "//Home";

            _isNavigatingBackThroughHistory = true;

            try
            {
                await GoToAsync(previousRoute);
            }
            finally
            {
                _isNavigatingBackThroughHistory = false;
            }
        }

        private async Task ConfirmLogoutFromBackAsync()
        {
            bool confirmed =
                await _dialogService.ConfirmAsync(
                    "Cerrar sesión",
                    "¿Deseas cerrar sesión?",
                    "Cerrar sesión",
                    "Cancelar");

            if (!confirmed)
                return;

            await LogoutAsync();
        }

        private async Task LogoutAsync()
        {
            _sessionService.EndSession();
            _navigationHistoryService.Clear();

            FlyoutBehavior = FlyoutBehavior.Disabled;
            FlyoutIsPresented = false;

            await GoToAsync("//Login");
        }

        protected override void OnNavigated(
            ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

            string currentRoute =
                CurrentState?.Location?.OriginalString
                ?? string.Empty;

            string? mainRoute =
                GetMainRoute(currentRoute);

            if (mainRoute is null)
                return;

            if (_isNavigatingBackThroughHistory)
            {
                RemoveCurrentHistoryEntry(mainRoute);

                return;
            }

            _navigationHistoryService.Push(mainRoute);
        }

        private void RemoveCurrentHistoryEntry(
            string arrivedRoute)
        {
            string? currentRoute =
                _navigationHistoryService.Peek();

            if (currentRoute is not null &&
                !string.Equals(
                    currentRoute,
                    arrivedRoute,
                    StringComparison.OrdinalIgnoreCase))
            {
                _navigationHistoryService.Pop();
            }

            string? previousRoute =
                _navigationHistoryService.Peek();

            if (previousRoute is null ||
                !string.Equals(
                    previousRoute,
                    arrivedRoute,
                    StringComparison.OrdinalIgnoreCase))
            {
                _navigationHistoryService.Push(arrivedRoute);
            }
        }

        private static string? GetMainRoute(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
                return null;

            string cleanRoute = route
                .Split('?')[0]
                .Trim()
                .TrimEnd('/');

            if (cleanRoute.EndsWith(
                    "/Home",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "//Home";
            }

            if (cleanRoute.EndsWith(
                    "/Services",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "//Services";
            }

            if (cleanRoute.EndsWith(
                    "/Request",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "//Request";
            }

            if (cleanRoute.EndsWith(
                    "/RequestsHistory",
                    StringComparison.OrdinalIgnoreCase))
            {
                return "//RequestsHistory";
            }

            return null;
        }

        protected override void OnPropertyChanged(
            string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(FlyoutIsPresented) &&
                FlyoutIsPresented)
            {
                _flyoutHeaderViewModel.RefreshSessionData();
            }
        }

        private async void OnFlyoutLogoutRequested(
            object? sender,
            EventArgs e)
        {
            bool confirmed =
                await _dialogService.ConfirmAsync(
                    "Cerrar sesión",
                    "¿Deseas cerrar sesión?",
                    "Cerrar sesión",
                    "Cancelar");

            if (!confirmed)
                return;

            await LogoutAsync();
        }
    }
}
