using Marila_Garden_App.Services;

namespace Marila_Garden_App.Views.Services;

public partial class ServicesPage : ContentPage
{
    private readonly ISessionService _sessionService;
    public string UserName =>
        _sessionService.CurrentUser?.FullName
        ?? "Usuario";
	public ServicesPage(ISessionService sessionService)
	{
		InitializeComponent();

        _sessionService = sessionService;
        BindingContext = this;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        OnPropertyChanged(nameof(UserName));
    }
    private void OnMenuClicked(
        object sender,
        EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}