using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.Views.Assistant;

namespace Marila_Garden_App.ViewModels.Assistant;

public partial class ServiceAssistantResultViewModel
    : ObservableObject
{
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ServiceInfo? service;

    public ServiceAssistantResultViewModel(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    public void LoadService(string serviceId)
    {
        Service =
            ServiceCatalog.GetById(serviceId);
    }

    [RelayCommand]
    private async Task RequestService()
    {
        if (Service is null ||
            string.IsNullOrWhiteSpace(Service.Id))
        {
            return;
        }

        string serviceId =
            Uri.EscapeDataString(Service.Id);

        await _navigationService.GoToAsync(
            $"//Request?serviceId={serviceId}");
    }

    [RelayCommand]
    private async Task RestartAssistant()
    {
        await _navigationService.GoToAsync(
            nameof(ServiceAssistantPage));
    }
}