using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Home;

public partial class HomeViewModel : ObservableObject
{
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";

    public HomeViewModel(ISessionService sessionService,
           INavigationService navigationService)
    {
        _sessionService = sessionService;
        _navigationService = navigationService;
    }

    public void RefreshSessionData()
    {
        OnPropertyChanged(nameof(UserName));
    }

    [RelayCommand]
    private async Task RequestService()
    {
        await _navigationService.GoToAsync("//Request");
    }
}