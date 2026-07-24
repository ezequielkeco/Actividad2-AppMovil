using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Base;

public abstract class AuthenticatedFormViewModelBase : FormViewModelBase
{
    protected ISessionService SessionService { get; }

    protected AuthenticatedFormViewModelBase(
        IDialogService dialogService,
        ISessionService sessionService)
        : base(dialogService)
    {
        SessionService = sessionService;
    }

    public string UserName =>
        SessionService.CurrentUser?.FullName ?? "Usuario";
}
