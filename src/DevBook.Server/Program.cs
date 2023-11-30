using DevBook.Server.AppSetups;
using DevBook.Server.Configuration;
using DevBook.Server.HackerNews;
using DevBook.Server.Infrastructure;
using DevBook.Server.StartupProfiles;

var builder = WebApplication.CreateBuilder(args);

var settingsSection = builder.Configuration.GetSection(Settings.SectionName);
var settings = settingsSection.Get<Settings>();
builder.Services.AddOptions<Settings>()
	.Bind(settingsSection)
	.ValidateDataAnnotations()
	.ValidateOnStart();

builder.Services.AddInfrastructure();
builder.Services.AddHackerNewsApi(settings!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
}

app.InitializeDb(applyMigrations: true);

app.UseAppSetupsFeature();
app.UseStartupProfilesFeature();
app.UseHackerNewsFeature();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

public partial class Program { }
