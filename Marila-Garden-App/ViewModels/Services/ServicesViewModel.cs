using CommunityToolkit.Mvvm.ComponentModel;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Services;

public partial class ServicesViewModel : ObservableObject
{
    private readonly ISessionService _sessionService;

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";

    public ServicesViewModel(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void RefreshSessionData()
    {
        OnPropertyChanged(nameof(UserName));
    }
}