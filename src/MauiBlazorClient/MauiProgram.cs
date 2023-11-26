using DevBook.Grpc.AppSetups;
using DevBook.Grpc.StartupProfiles;
using DevBook.Shared;
using MauiBlazorClient.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;
using MudBlazor.Services;

namespace MauiBlazorClient;

public static class MauiProgram
{
	// Should be set on host env, otherwise falls back to localhost address
	internal static readonly string GrpcServerAddress = Environment.GetEnvironmentVariable("DEVBOOK_GRPC_ADDRESS") ?? "https://localhost:5112";

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
		builder.Services.AddLogging(b => b.AddDevBookLogging());

		builder.ConfigureLifecycleEvents(events =>
		{
			events.AddWindows(windowLifecycleBuilder =>
			{
				windowLifecycleBuilder.OnWindowCreated(window =>
				{
					var minWindowWidth = 1000;
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
		services.AddCommandsAndQueriesExecutor(typeof(MauiProgram).Assembly);

		services.AddSingleton<IProcessService, ProcessService>();
		services.AddSingleton<IFilePickerService, FilePickerService>();

		// GRPC clients
		services.AddGrpcClient<AppSetupsGrpcService.AppSetupsGrpcServiceClient>(o => o.Address = new Uri(GrpcServerAddress));
		services.AddGrpcClient<StartupProfilesGrpcService.StartupProfilesGrpcServiceClient>(o => o.Address = new Uri(GrpcServerAddress));

		services.AddSingleton<IAppSetupsService, AppSetupsService>();
		services.AddSingleton<IStartupProfilesService, StartupProfilesService>();

		var appStore = new AppStore();
		appStore.Initialize();

		services.AddSingleton<IAppStore>(appStore);
	}
}
