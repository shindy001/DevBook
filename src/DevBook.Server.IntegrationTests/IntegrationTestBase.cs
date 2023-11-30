using DevBook.Server.IntegrationTests.Features.Helpers;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace DevBook.Server.IntegrationTests;

public class IntegrationTestBase : IClassFixture<DevBookTextFixture<Program>>, IDisposable
{
	private GrpcChannel? _channel;
	private IDisposable? _testContext;

	protected DevBookTextFixture<Program> Fixture { get; set; }

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

	public IntegrationTestBase(DevBookTextFixture<Program> fixture, ITestOutputHelper outputHelper)
	{
		Fixture = fixture;
		_testContext = Fixture.GetTestContext(outputHelper);
	}

	public void Dispose()
	{
		_testContext?.Dispose();
		_channel = null;
	}
}
