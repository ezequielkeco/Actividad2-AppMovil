using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Shared;

public partial class FlyoutHeaderViewModel : ObservableObject
{
    private readonly ISessionService _sessionService;

    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";

    public event EventHandler? LogoutRequested;

    public FlyoutHeaderViewModel(
        ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public void RefreshSessionData()
    {
        OnPropertyChanged(nameof(UserName));
    }

    [RelayCommand]
    private void Logout()
    {
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }
}