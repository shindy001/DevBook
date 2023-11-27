using DevBook.Server.Common;
using DevBook.Server.Configuration;
using DevBook.Server.Features.AppSetups;
using DevBook.Server.Features.HackerNews;
using DevBook.Server.Features.StartupProfiles;
using DevBook.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var settingsSection = builder.Configuration.GetSection(Settings.SectionName);
var settings = settingsSection.Get<Settings>();
builder.Services.AddOptions<Settings>()
	.Bind(settingsSection)
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddGrpc(cfg => cfg.Interceptors.Add<GrpgGlobalExceptionInterceptor>());
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure();

builder.Services.AddScoped<IHackerNewsApi, HackerNewsApi>();
builder.Services.AddHttpClient<IHackerNewsApi, HackerNewsApi>(client => client.BaseAddress = new Uri(settings!.HackerNewsBaseAddress));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
}

using var serviceScope = app.Services.CreateScope();
serviceScope.InitializeDb(applyMigrations: true);

app.MapGrpcService<AppSetupsService>();
app.MapGrpcService<StartupProfilesService>();
app.MapGrpcService<HackerNewsService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
