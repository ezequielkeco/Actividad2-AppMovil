using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Marila_Garden_App.ViewModels;
using Marila_Garden_App.Services;

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

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            builder.Services.AddTransient<RequestsHistoryViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RequestViewModel>();

            return builder.Build();
        }
    }
}