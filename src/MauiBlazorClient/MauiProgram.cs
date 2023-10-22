using MauiBlazorClient.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using MudBlazor.Services;

namespace MauiBlazorClient
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

			builder.Services.AddMauiBlazorWebView();
			builder.Services.AddMudServices();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

#if WINDOWS
			builder.ConfigureLifecycleEvents(events =>
			{
				events.AddWindows(windowLifecycleBuilder =>
				{
					windowLifecycleBuilder.OnWindowCreated(window =>
					{
						var minWindowWidth = 960;
						var minWindowHeigh = 660;
						var restrictedWindow = new Window()
						{
							Width = minWindowWidth,
							Height = minWindowHeigh,
							MinimumWidth = minWindowWidth,
							MinimumHeight = minWindowHeigh,
							MaximumWidth = double.PositiveInfinity,
							MaximumHeight = double.PositiveInfinity
						};
						window.UpdateSize(restrictedWindow);
						window.UpdateMinimumSize(restrictedWindow);
						window.UpdateMaximumSize(restrictedWindow);
					});
				});
			});
#endif

			RegisterServices(builder.Services);

			return builder.Build();
		}

		private static void RegisterServices(IServiceCollection services)
		{
			services.AddSingleton<IAppSetupsService>(new AppSetupsService());
		}
	}
}
