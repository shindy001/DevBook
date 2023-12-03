using DevBook.Grpc.AppSetups;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.IntegrationTests.Features.AppSetups;

public class AppSetupsServiceTests : IntegrationTestBase
{
	private readonly AppSetupsGrpcService.AppSetupsGrpcServiceClient _client;
	private readonly AppSetupsFixtureDriver _driver;

	public AppSetupsServiceTests(ITestOutputHelper outputHelper) :base(outputHelper)
	{
		_client = new AppSetupsGrpcService.AppSetupsGrpcServiceClient(Channel);
		_driver = new AppSetupsFixtureDriver(_client);
	}

	#region GetById

	[Fact]
	public async Task GetById_WhenCalled_ReturnsAppSetup()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		var response = await _client.GetByIdAsync(new GetByIdRequest { Id = expectedItemId });

		// Assert
		response.Should().NotBeNull();
		response.Item.Id.Should().Be(expectedItemId);
		response.Item.Name.Should().Be("AppSetup1");
		response.Item.Path.Should().Be(@"c:\work\app.exe");
		response.Item.Arguments.Should().BeNull();
	}

	[Fact]
	public async Task GetById_WhenCalledWithNonExistingIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.GetByIdAsync(new GetByIdRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	[Fact]
	public async Task GetById_WhenCalledWithIncorrectIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.GetByIdAsync(new GetByIdRequest { Id = "super incorrect id" });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	#endregion

	#region GetAll

	[Fact]
	public async Task GetAll_WhenCalled_ReturnsAppSetups()
	{
		// Arrange
		var expectedItemId1 = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedItemId2 = await _driver.Prepare("AppSetup2", @"c:\project\proj1.exe");

		// Act
		var response = await _client.GetAllAsync(new GetAllRequest());

		// Assert
		response.Should().NotBeNull();
		response.Items.Should().HaveCount(2);
		response.Items[0].Id.Should().Be(expectedItemId1);
		response.Items[0].Name.Should().Be("AppSetup1");
		response.Items[0].Path.Should().Be(@"c:\work\app.exe");
		response.Items[0].Arguments.Should().BeNull();

		response.Items[1].Id.Should().Be(expectedItemId2);
		response.Items[1].Name.Should().Be("AppSetup2");
		response.Items[1].Path.Should().Be(@"c:\project\proj1.exe");
		response.Items[1].Arguments.Should().BeNull();
	}

	[Fact]
	public async Task GetAll_WhenCalledWithoutItemsInDB_ReturnsEmptyCollection()
	{
		// Arrange
		// Act
		var response = await _client.GetAllAsync(new GetAllRequest());

		// Assert
		response.Should().NotBeNull();
		response.Items.Should().BeEmpty();
	}

	#endregion

	#region Create

	[Fact]
	public async Task Create_WhenCalled_Succeeds()
	{
		// Arrange
		// Act
		var response = await _client.CreateAsync(new CreateRequest { Name = "AppSetup1", Path = @"c:\work\app.exe" });

		// Assert
		response.Should().NotBeNull();
		Guid.TryParse(response.Id, out _).Should().BeTrue();
	}

	[Fact]
	public async Task Create_WhenCalledWithMissingNameParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.CreateAsync(new CreateRequest { Path = @"c:\work\app.exe" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Name\":\"\\u0027Name\\u0027 must not be empty.\"}");
	}

	[Fact]
	public async Task Create_WhenCalledWithMissingPathParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.CreateAsync(new CreateRequest { Name = "AppSetup1" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Path\":\"\\u0027Path\\u0027 must not be empty.\"}");
	}

	#endregion

	#region Update

	[Fact]
	public async Task Update_WhenCalled_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.UpdateAsync(new UpdateRequest { Id = expectedItemId, Name = "UpdatedName", Path = "UpdatedPath" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("UpdatedName");
		actualItemResponse.Item.Path.Should().Be("UpdatedPath");
	}

	[Fact]
	public async Task Update_WhenCalledWithArgumentsParam_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.UpdateAsync(new UpdateRequest { Id = expectedItemId, Name = "UpdatedName", Path = "UpdatedPath", Arguments = "UpdatedArguments" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("UpdatedName");
		actualItemResponse.Item.Path.Should().Be("UpdatedPath");
		actualItemResponse.Item.Arguments.Should().Be("UpdatedArguments");
	}

	[Fact]
	public async Task Update_WhenCalledWithMissingNameParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		Func<Task> act = async () => await _client.UpdateAsync(new UpdateRequest { Id = expectedItemId, Path = "UpdatedPath" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Name\":\"\\u0027Name\\u0027 must not be empty.\"}");
	}

	[Fact]
	public async Task Update_WhenCalledWithMissingPathParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		Func<Task> act = async () => await _client.UpdateAsync(new UpdateRequest { Id = expectedItemId, Name = "UpdatedName" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Path\":\"\\u0027Path\\u0027 must not be empty.\"}");
	}

	[Fact]
	public async Task Update_WhenCalledWithIncorrectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		Func<Task> act = async () => await _client.UpdateAsync(new UpdateRequest { Id = "super incorrect id", Name = "UpdatedName", Path = "UpdatedPath" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Id\":\"Invalid Id value super incorrect id.\"}");
	}

	#endregion

	#region Patch

	[Fact]
	public async Task Patch_WhenCalled_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.PatchAsync(new PatchRequest { Id = expectedItemId, Name = "PatchedName", Path = "PatchedPath", Arguments = "PatchedArguments" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("PatchedName");
		actualItemResponse.Item.Path.Should().Be("PatchedPath");
		actualItemResponse.Item.Arguments.Should().Be("PatchedArguments");
	}

	[Fact]
	public async Task Patch_WhenCalledWithNameParam_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.PatchAsync(new PatchRequest { Id = expectedItemId, Name = "PatchedName" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("PatchedName");
		actualItemResponse.Item.Path.Should().Be(@"c:\work\app.exe");
		actualItemResponse.Item.Arguments.Should().BeNull();
	}

	[Fact]
	public async Task Patch_WhenCalledWithPathParam_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.PatchAsync(new PatchRequest { Id = expectedItemId, Path = "PatchedPath" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("AppSetup1");
		actualItemResponse.Item.Path.Should().Be("PatchedPath");
		actualItemResponse.Item.Arguments.Should().BeNull();
	}

	[Fact]
	public async Task Patch_WhenCalledWithArgumentsParam_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		await _client.PatchAsync(new PatchRequest { Id = expectedItemId, Arguments = "PatchedArguments" });
		var actualItemResponse = _client.GetById(new GetByIdRequest() { Id = expectedItemId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("AppSetup1");
		actualItemResponse.Item.Path.Should().Be(@"c:\work\app.exe");
		actualItemResponse.Item.Arguments.Should().Be("PatchedArguments");
	}

	[Fact]
	public async Task Patch_WhenCalledWithNonExistingIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.PatchAsync(new PatchRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	[Fact]
	public async Task Patch_WhenCalledWithIncorectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.PatchAsync(new PatchRequest { Id = "super incorrect id" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Id\":\"Invalid Id value super incorrect id.\"}");
	}

	#endregion

	#region Delete

	[Fact]
	public async Task Delete_WhenCalled_Succeeds()
	{
		// Arrange
		var expectedItemId = await _driver.Prepare("AppSetup1", @"c:\work\app.exe");

		// Act
		var response = await _client.DeleteAsync(new DeleteRequest { Id = expectedItemId });

		// Assert
		response.Should().BeOfType(typeof(Empty));
	}

	[Fact]
	public async Task Delete_WhenCalledWithNonExistingIdParam_Succeeds()
	{
		// Arrange
		// Act
		var result = await _client.DeleteAsync(new DeleteRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		result.Should().BeOfType(typeof(Empty));
	}

	[Fact]
	public async Task Delete_WhenCalledWithIncorectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _client.DeleteAsync(new DeleteRequest { Id = "super incorrect id" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Id\":\"Invalid Id value super incorrect id.\"}");
	}

	#endregion
}
