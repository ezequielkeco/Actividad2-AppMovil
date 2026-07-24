using CommunityToolkit.Mvvm.ComponentModel;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Home;

public partial class HomeViewModel : ObservableObject
{
    private readonly ISessionService _sessionService;

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";

    public HomeViewModel(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void RefreshSessionData()
    {
        OnPropertyChanged(nameof(UserName));
    }
}