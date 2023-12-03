using DevBook.Server.Features.HackerNews;
using DevBook.Server.Infrastructure;
using DevBook.Server.IntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevBook.Server.IntegrationTests.Helpers;

public delegate void LogMessage(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception? exception);

/// <summary>
/// Creates fixture with grpc test server setup, every instance also has different db (integrationTests-[Guid].db).
/// DB is deleted on dispose.
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public sealed class DevBookTestFixture<TStartup> : IDisposable where TStartup : class
{
	private WebApplicationFactory<Program>? _app;
	private TestServer? _server;
	private HttpClient? _client;
	private HttpMessageHandler? _handler;
	private Action<IWebHostBuilder>? _configureWebHost;

	// Use guid postfix for test db in case of parallel test runs
	private readonly string TestDbName = $"integrationTests-{Guid.NewGuid()}.db";

	public event LogMessage? LoggedMessage;
	public LoggerFactory LoggerFactory { get; }
	public HttpMessageHandler Handler
	{
		get
		{
			EnsureServer();
			return _handler!;
		}
	}

	public DevBookTestFixture()
	{
		LoggerFactory = new LoggerFactory();
		LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) =>
		{
			LoggedMessage?.Invoke(logLevel, category, eventId, message, exception);
		}));
	}

	public void ConfigureWebHost(Action<IWebHostBuilder> configure)
	{
		_configureWebHost = configure;
	}

	public IDisposable GetTestContext(ITestOutputHelper outputHelper)
	{
		return new GrpcTestContext<TStartup>(this, outputHelper);
	}

	private void EnsureServer()
	{
		if (_client == null)
		{

			_app = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureServices(services =>
					{
						services.AddSingleton<ILoggerFactory>(LoggerFactory);

						ReplaceDbWithTestDb(services);
						ReplaceHackerNewApiWithFake(services);
					});

					builder.UseTestServer();
					_configureWebHost?.Invoke(builder);
				});

			_client = _app.CreateClient();
			_server = (TestServer)_app.Services.GetRequiredService<IServer>();
			_handler = _server.CreateHandler();
		}
	}

	private void ReplaceDbWithTestDb(IServiceCollection services)
	{
		var dbContextDescriptor = services.SingleOrDefault(
			d => d.ServiceType == typeof(DbContextOptions<DevBookDbContext>));
		if (dbContextDescriptor != null)
		{
			services.Remove(dbContextDescriptor);
		}

		services.AddDbContextPool<DevBookDbContext>(
		o => o.UseSqlite($"Data Source={TestDbName};Pooling=False;", // Disabled pooling so the db file is unlocked after dbcontext dispose and can be deleted
		b => b.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));
	}

	private void ReplaceHackerNewApiWithFake(IServiceCollection services)
	{
		var hackerNewsApiDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHackerNewsApi));
		if (hackerNewsApiDescriptor != null)
		{
			services.Remove(hackerNewsApiDescriptor);
		}
		services.AddScoped<IHackerNewsApi, HackerNewsApiFake>();
	}

	public void Dispose()
	{
		_handler?.Dispose();
		_client?.Dispose();
		_server?.Dispose();
		_app?.Dispose();

		// Clean test db file after run - does not work when pooled dbcontext is used(default behavior), pooling locks db file
		File.Delete(TestDbName);
	}
}
