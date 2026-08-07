using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Services;

namespace Marila_Garden_App.Views.Services;

[QueryProperty(nameof(ServiceId), "serviceId")]
public partial class ServiceDetailPage : ContentPage
{
    private readonly ServiceDetailViewModel _viewModel;
    private readonly IAnimationService _animationService;
    private IDispatcherTimer _carouselTimer;

    private string serviceId = string.Empty;

    public string ServiceId
    {
        get => serviceId;
        set
        {
            serviceId = Uri.UnescapeDataString(value ?? string.Empty);
            _viewModel.LoadService(serviceId);
        }
    }

    public ServiceDetailPage(
        ServiceDetailViewModel viewModel,
        IAnimationService animationService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _animationService = animationService;

        BindingContext = viewModel;
        
        ServiceImagesCarousel.PositionChanged +=
            OnServiceImagePositionChanged;

    }

    private void OnMenuClicked(
        object sender,
        EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    private async void OnBackClicked(
        object sender,
        EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.RefreshSessionData();

        if (_carouselTimer == null)
        {
            _carouselTimer = Dispatcher.CreateTimer();
            _carouselTimer.Interval = TimeSpan.FromSeconds(4);
            _carouselTimer.Tick += OnCarouselTimerTick;
        }

        if (!_carouselTimer.IsRunning)
        {
            _carouselTimer.Start();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if(_carouselTimer?.IsRunning == true)
        {
            _carouselTimer.Stop();
        }

    }

    private async void OnServiceImagePositionChanged(
        object sender,
        PositionChangedEventArgs e)
    {
        if (_carouselTimer?.IsRunning != true)
            return;

        _carouselTimer.Stop();
        _carouselTimer.Start();

    }

    private void OnCarouselTimerTick(
        object? sender,
        EventArgs e)
    {
        if (_viewModel.IsImageViewerVisible)
            return;

        int imageCount =
            _viewModel.Service?.Images.Count ?? 0;

        if (imageCount <= 1)
            return;

        int nextPosition =
            (ServiceImagesCarousel.Position + 1)
            % imageCount;

        ServiceImagesCarousel.Position = nextPosition;
    }
}