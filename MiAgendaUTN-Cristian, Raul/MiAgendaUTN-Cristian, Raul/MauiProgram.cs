using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using MiAgendaUTN_Cristian__Raul.Services;
using MiAgendaUTN_Cristian__Raul.Components;

namespace MiAgendaUTN_Cristian__Raul
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

            builder.Services.AddBlazorWebView();
            builder.Services.AddSingleton<IDataStore, SqliteDataStore>();
            builder.Services.AddSingleton<TareaViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}