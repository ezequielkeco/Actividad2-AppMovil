using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Base;
using Marila_Garden_App.Views.Assistant;

namespace Marila_Garden_App.ViewModels.Home;

public partial class HomeViewModel : AuthenticatedViewModelBase
{
    public IReadOnlyList<ServiceInfo> Services { get; } =
        ServiceCatalog.GetAll();
    public HomeViewModel(
        ISessionService sessionService,
        INavigationService navigationService)
        : base(sessionService, navigationService)
    {
    }

    [RelayCommand]
    private async Task RequestService()
    {
        await NavigationService.GoToAsync("//Request");
    }

    [RelayCommand]
    private async Task OpenServiceAssistant()
    {
        await NavigationService.GoToAsync(
            nameof(ServiceAssistantPage));
    }
}