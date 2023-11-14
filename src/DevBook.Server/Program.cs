using DevBook.Server.Services;
using DevBook.Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddDevBookLogging();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddCommandsAndQueriesExecutor(typeof(Program).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
}

app.MapGrpcService<AppSetupsService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
