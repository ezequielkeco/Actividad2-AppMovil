namespace Marila_Garden_App.Views.Splash;

public partial class SplashPage : ContentPage
{
	public SplashPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        SplashContent.Opacity = 0;
        SplashContent.Scale = 0.95;

        await SplashContent.FadeToAsync(1, 500);
        await SplashContent.ScaleToAsync(1, 500, Easing.CubicOut);

        await Task.Delay(800);

        await SplashContent.FadeToAsync(0, 350);

        await Shell.Current.GoToAsync("//Login");
    }
}