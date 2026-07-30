using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Base;

namespace Marila_Garden_App.ViewModels.Services;

public partial class ServicesViewModel : AuthenticatedViewModelBase
{
    public IReadOnlyList<ServiceInfo> Services { get; } =
        ServiceCatalog.GetAll();
    public ServicesViewModel(
        ISessionService sessionService,
        INavigationService navigationService)
        : base(sessionService, navigationService)
    {
    }
}