using qualityControl.MOBILE.Pages;
using qualityControl.MOBILE.Services;
using qualityControl.MOBILE.ViewModels;

namespace qualityControl.MOBILE
{
    public static class MauiProgram
    {
        public static IServiceProvider Services { get; private set; } = default!;
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                fonts.AddFont("Ubuntu-Bold.ttf", "Ubold");
                fonts.AddFont("Ubuntu-BoldItalic.ttf", "Uboldi");
                fonts.AddFont("Ubuntu-Italic.ttf", "Uital");
                fonts.AddFont("Ubuntu-Light.ttf", "Ulight");
                fonts.AddFont("Ubuntu-LightItalic.ttf", "Ulighti");
                fonts.AddFont("Ubuntu-Medium.ttf", "Umed");
                fonts.AddFont("Ubuntu-MediumItalic.ttf", "Umedi");
                fonts.AddFont("Ubuntu-Regular.ttf", "Uregu");
                });

            #if ANDROID && DEBUG
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                var baseUri = new Uri("https://10.0.2.2:1234");
                builder.Services.AddSingleton(new HttpClient(handler)
                {
                    BaseAddress = baseUri,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            #elif ANDROID
                var baseUri = new Uri("https://10.0.2.2:1234");
                builder.Services.AddSingleton(new HttpClient { BaseAddress = baseUri, Timeout = TimeSpan.FromSeconds(30)});
            #else
                var baseUri = new Uri("https://localhost:1234");
                builder.Services.AddSingleton(new HttpClient { BaseAddress = baseUri, Timeout = TimeSpan.FromSeconds(30)});
            #endif

            builder.Services.AddSingleton<ItemService>();
            builder.Services.AddSingleton<WorkOrderService>();
            builder.Services.AddSingleton<ProductionService>();
            builder.Services.AddSingleton<QualityControlService>();

            builder.Services.AddTransient<WorkOrderViewModel>();
            builder.Services.AddTransient<DetailViewModel>();
            builder.Services.AddTransient<QualityControlViewModel>();
            builder.Services.AddTransient<QualityControlAdderViewModel>();

            builder.Services.AddTransient<WorkOrderPage>();
            builder.Services.AddTransient<WorkOrderDetailPage>();
            builder.Services.AddTransient<QualityControlAdderPage>();

            var app = builder.Build();
            Services = app.Services;
            return app;
        }
    }
}
