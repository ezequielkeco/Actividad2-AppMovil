using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Base;

namespace Marila_Garden_App.ViewModels.Services;

public partial class ServiceDetailViewModel
    : AuthenticatedViewModelBase
{
    [ObservableProperty]
    private ServiceInfo? service;

    [ObservableProperty]
    private int selectedImageIndex;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isImageViewerVisible;

    [ObservableProperty]
    private string selectedImage = string.Empty;

    public bool HasService =>
        Service is not null;

    public ServiceDetailViewModel(
        ISessionService sessionService,
        INavigationService navigationService)
        : base(sessionService, navigationService)
    {
    }

    partial void OnServiceChanged(ServiceInfo? value)
    {
        SelectedImageIndex = 0;

        OnPropertyChanged(nameof(HasService));
    }

    public void LoadService(string serviceId)
    {
        IsLoading = true;

        Service = ServiceCatalog.GetById(serviceId);

        IsLoading = false;
    }

    [RelayCommand]
    private void OpenImage(string? image)
    {
        if (string.IsNullOrWhiteSpace(image))
            return;

        SelectedImage = image;
        IsImageViewerVisible = true;
    }

    [RelayCommand]
    private void CloseImage()
    {
        IsImageViewerVisible = false;
        SelectedImage = string.Empty;
    }

    [RelayCommand]
    private async Task RequestCurrentService()
    {
        if (Service is null ||
            string.IsNullOrWhiteSpace(Service.Id))
        {
            return;
        }

        string serviceId =
            Uri.EscapeDataString(Service.Id);

        await NavigationService.GoToAsync(
            $"//Request?serviceId={serviceId}");
    }
}