using DevBook.Server.IntegrationTests.Helpers;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace DevBook.Server.IntegrationTests;

/// <summary>
/// Creates new DevBookFixture (webapp) for each test
/// </summary>
public class IntegrationTestBase : IDisposable
{
	private GrpcChannel? _channel;
	private IDisposable? _testContext;

	protected DevBookTestFixture<Program> Fixture { get; set; }

	protected ILoggerFactory LoggerFactory => Fixture.LoggerFactory;

	protected GrpcChannel Channel => _channel ??= CreateChannel();

	protected GrpcChannel CreateChannel()
	{
		return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
		{
			LoggerFactory = LoggerFactory,
			HttpHandler = Fixture.Handler
		});
	}

	public IntegrationTestBase(ITestOutputHelper outputHelper)
	{
		Fixture = new DevBookTestFixture<Program>();
		_testContext = Fixture.GetTestContext(outputHelper);
	}

	public void Dispose()
	{
		_testContext?.Dispose();
		_channel = null;
	}
}
