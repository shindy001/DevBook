using DevBook.Server.Common;
using DevBook.Server.Features.AppSetups;
using DevBook.Server.Features.StartupProfiles;
using DevBook.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(cfg => cfg.Interceptors.Add<GrpgGlobalExceptionInterceptor>());
builder.Services.AddGrpcReflection();

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
}

using var serviceScope = app.Services.CreateScope();
serviceScope.InitializeDb(applyMigrations: true);

app.MapGrpcService<AppSetupsService>();
app.MapGrpcService<StartupProfilesService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
