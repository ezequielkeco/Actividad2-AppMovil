using CommunityToolkit.Mvvm.ComponentModel;
using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Services;

public partial class ServiceDetailViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ServiceInfo? service;

    [ObservableProperty]
    private bool isLoading;

    public bool HasService => Service is not null;

    public ServiceDetailViewModel(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    partial void OnServiceChanged(ServiceInfo? value)
    {
        OnPropertyChanged(nameof(HasService));
    }

    public void LoadService(string serviceId)
    {
        IsLoading = true;

        Service = ServiceCatalog.GetById(serviceId);

        IsLoading = false;
    }
}