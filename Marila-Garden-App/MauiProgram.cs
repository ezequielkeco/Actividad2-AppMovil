using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels;
using Marila_Garden_App.ViewModels.Authentication;
using Marila_Garden_App.ViewModels.Home;
using Marila_Garden_App.ViewModels.Services;
using Marila_Garden_App.ViewModels.Request;
using Marila_Garden_App.ViewModels.RequestsHistory;
using Marila_Garden_App.Views.Authentication;
using Marila_Garden_App.Views.Home;
using Marila_Garden_App.Views.Request;
using Marila_Garden_App.Views.RequestsHistory;
using Marila_Garden_App.Views.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Marila_Garden_App.ViewModels.Shared;
using Marila_Garden_App.ViewModels.Assistant;

namespace Marila_Garden_App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.ConfigureMauiHandlers(handlers =>
            {
#if ANDROID

                EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
                {
                    handler.PlatformView.Background = null;
                });

                PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
                {
                    handler.PlatformView.Background = null;
                });

                DatePickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
                {
                    handler.PlatformView.Background = null;
                });

                EditorHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
                {
                    handler.PlatformView.Background = null;
                });

#endif
            });

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton<IAnimationService, AnimationService>();
            builder.Services.AddSingleton<INavigationHistoryService, NavigationHistoryService>();
            builder.Services.AddSingleton<FlyoutHeaderViewModel>();
            builder.Services.AddSingleton<IServiceRecommendationService, ServiceRecommendationService>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RequestViewModel>();
            builder.Services.AddTransient<RequestsHistoryViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<ServicesViewModel>();
            builder.Services.AddTransient<ServiceDetailViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ServicesPage>();
            builder.Services.AddTransient<ServiceDetailPage>();
            builder.Services.AddTransient<RequestsHistoryPage>();
            builder.Services.AddTransient<ServiceAssistantViewModel>();

            return builder.Build();
        }
    }
}