using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Base;

public abstract partial class AuthenticatedViewModelBase : ObservableObject
{
    private readonly ISessionService _sessionService;

    protected INavigationService NavigationService { get; }

    protected AuthenticatedViewModelBase(
        ISessionService sessionService,
        INavigationService navigationService)
    {
        _sessionService = sessionService;
        NavigationService = navigationService;
    }

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";

    public void RefreshSessionData()
    {
        OnPropertyChanged(nameof(UserName));
    }

    [RelayCommand]
    private async Task OpenServiceDetail(ServiceInfo? service)
    {
        if (service is null ||
            string.IsNullOrWhiteSpace(service.Id))
        {
            return;
        }

        string serviceId =
            Uri.EscapeDataString(service.Id);

        await NavigationService.GoToAsync(
            $"ServiceDetailPage?serviceId={serviceId}");
    }
}