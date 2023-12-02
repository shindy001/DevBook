using DevBook.Grpc.AppSetups;

namespace DevBook.Server.IntegrationTests.Features.AppSetups;

public class GetAppSetupTests : IntegrationTestBase
{
	private readonly AppSetupsGrpcService.AppSetupsGrpcServiceClient _client;

	public GetAppSetupTests(ITestOutputHelper outputHelper)
		:base(outputHelper)
	{
		_client = new AppSetupsGrpcService.AppSetupsGrpcServiceClient(Channel);
	}

	[Fact]
	public async Task GetAppSetup()
	{
		// Arrange
		await _client.CreateAsync(new CreateRequest
		{
			Name = "AppSetup1",
			Path = @"c:\work\app.exe"
		});
		var expectedItem = (await _client.GetAllAsync(new GetAllRequest())).Items.First();

		// Act
		var response = await _client.GetByIdAsync(new GetByIdRequest { Id = expectedItem.Id });

		// Assert
		response.Should().NotBeNull();
		response.Item.Id.Should().Be(expectedItem.Id);
		response.Item.Name.Should().Be("AppSetup1");
		response.Item.Path.Should().Be(@"c:\work\app.exe");
		response.Item.Arguments.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAllAppSetups()
	{
		// Arrange
		await _client.CreateAsync(new CreateRequest 
		{
			Name = "AppSetup1",
			Path = @"c:\work\app.exe"
		});
		await _client.CreateAsync(new CreateRequest
		{
			Name = "AppSetup2",
			Path = @"c:\project\proj1.exe"
		});

		// Act
		var response = await _client.GetAllAsync(new GetAllRequest());

		// Assert
		response.Should().NotBeNull();
		response.Items.Should().HaveCount(2);
		response.Items[0].Name.Should().Be("AppSetup1");
		response.Items[0].Path.Should().Be(@"c:\work\app.exe");
		response.Items[0].Arguments.Should().BeEmpty();

		response.Items[1].Name.Should().Be("AppSetup2");
		response.Items[1].Path.Should().Be(@"c:\project\proj1.exe");
		response.Items[1].Arguments.Should().BeEmpty();
	}
}
