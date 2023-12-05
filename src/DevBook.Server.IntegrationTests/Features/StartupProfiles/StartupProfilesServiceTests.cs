using DevBook.Grpc.AppSetups;
using DevBook.Grpc.StartupProfiles;
using DevBook.Server.IntegrationTests.Drivers;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.IntegrationTests.Features.StartupProfiles;

public class StartupProfilesServiceTests : IntegrationTestBase
{
	private readonly AppSetupsGrpcService.AppSetupsGrpcServiceClient _appSetupsClient;
	private readonly StartupProfilesGrpcService.StartupProfilesGrpcServiceClient _startupProfilesClient;
	private readonly AppSetupsFixtureDriver _appSetupsDriver;
	private readonly StartupProfilesFixtureDriver _startupProfilesDriver;

	public StartupProfilesServiceTests(ITestOutputHelper outputHelper) :base(outputHelper)
	{
		_appSetupsClient = new AppSetupsGrpcService.AppSetupsGrpcServiceClient(Channel);
		_startupProfilesClient = new StartupProfilesGrpcService.StartupProfilesGrpcServiceClient(Channel);
		_appSetupsDriver = new AppSetupsFixtureDriver(_appSetupsClient);
		_startupProfilesDriver = new StartupProfilesFixtureDriver(_startupProfilesClient);
	}

	#region GetById

	[Fact]
	public async Task GetById_WhenCalled_ReturnsStartupProfile()
	{
		// Arrange
		var expectedItemId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		var response = await _startupProfilesClient.GetByIdAsync(new Grpc.StartupProfiles.GetByIdRequest { Id = expectedItemId });

		// Assert
		response.Should().NotBeNull();
		response.Item.Id.Should().Be(expectedItemId);
		response.Item.Name.Should().Be("Profile1");
		response.Item.AppSetupIds.Should().BeEmpty();
	}

	[Fact]
	public async Task GetById_WhenCalled_ReturnsStartupProfileWithAppSetupIds()
	{
		// Arrange
		var expectedAppSetupId = await _appSetupsDriver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", [expectedAppSetupId]);

		// Act
		var response = await _startupProfilesClient.GetByIdAsync(new Grpc.StartupProfiles.GetByIdRequest { Id = expectedStartupProfileId });

		// Assert
		response.Should().NotBeNull();
		response.Item.Id.Should().Be(expectedStartupProfileId);
		response.Item.Name.Should().Be("Profile1");
		response.Item.AppSetupIds.Should().BeEquivalentTo([expectedAppSetupId]);
	}

	[Fact]
	public async Task GetById_WhenCalledWithNonExistingIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.GetByIdAsync(new Grpc.StartupProfiles.GetByIdRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	[Fact]
	public async Task GetById_WhenCalledWithIncorrectIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.GetByIdAsync(new Grpc.StartupProfiles.GetByIdRequest { Id = "super incorrect id" });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	#endregion

	#region GetAll

	[Fact]
	public async Task GetAll_WhenCalled_ReturnsStartupProfiles()
	{
		// Arrange
		var expectedItemId1 = await _startupProfilesDriver.Prepare("Profile1", []);
		var expectedItemId2 = await _startupProfilesDriver.Prepare("Profile2", []);

		// Act
		var response = await _startupProfilesClient.GetAllAsync(new Grpc.StartupProfiles.GetAllRequest());

		// Assert
		response.Should().NotBeNull();
		response.Items.Should().HaveCount(2);
		response.Items[0].Id.Should().Be(expectedItemId1);
		response.Items[0].Name.Should().Be("Profile1");
		response.Items[0].AppSetupIds.Should().BeEmpty();

		response.Items[1].Id.Should().Be(expectedItemId2);
		response.Items[1].Name.Should().Be("Profile2");
		response.Items[0].AppSetupIds.Should().BeEmpty();
	}

	[Fact]
	public async Task GetAll_WhenCalledWithoutItemsInDB_ReturnsEmptyCollection()
	{
		// Arrange
		// Act
		var response = await _startupProfilesClient.GetAllAsync(new Grpc.StartupProfiles.GetAllRequest());

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
		var response = await _startupProfilesClient.CreateAsync(new Grpc.StartupProfiles.CreateRequest { Name = "Profile1" });

		// Assert
		response.Should().NotBeNull();
		Guid.TryParse(response.Id, out _).Should().BeTrue();
	}

	[Fact]
	public async Task Create_WhenCalledWithNonExistingAppSetupId_IgnoresNonExistingAppSetupId()
	{
		// Arrange
		// Act
		var createRequest = new Grpc.StartupProfiles.CreateRequest { Name = "Profile1" };
		createRequest.AppSetupIds.AddRange([Guid.NewGuid().ToString(), Guid.NewGuid().ToString()]);

		var response = await _startupProfilesClient.CreateAsync(createRequest);
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = response.Id });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("Profile1");
		actualItemResponse.Item.AppSetupIds.Should().BeEmpty();
	}

	[Fact]
	public async Task Create_WhenCalledWithMissingNameParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.CreateAsync(new Grpc.StartupProfiles.CreateRequest { });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Name\":\"\\u0027Name\\u0027 must not be empty.\"}");
	}

	[Fact]
	public async Task Create_WhenCalledWithInvalidAppSetupId_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		var createRequest = new Grpc.StartupProfiles.CreateRequest { Name = "AppSetup1" };
		createRequest.AppSetupIds.Add("super incorrect appSetup id");

		Func<Task> act = async () => await _startupProfilesClient.CreateAsync(createRequest);

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"AppSetupIds[0]\":\"Invalid Id value super incorrect appSetup id.\"}");
	}

	#endregion

	#region Update

	[Fact]
	public async Task Update_WhenCalled_Succeeds()
	{
		// Arrange
		var expectedAppSetupId = await _appSetupsDriver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		var updateRequest = new Grpc.StartupProfiles.UpdateRequest { Id = expectedStartupProfileId, Name = "UpdatedName" };
		updateRequest.AppSetupIds.Add(expectedAppSetupId);

		await _startupProfilesClient.UpdateAsync(updateRequest);
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = expectedStartupProfileId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("UpdatedName");
		actualItemResponse.Item.AppSetupIds.Should().BeEquivalentTo([expectedAppSetupId]);
	}

	[Fact]
	public async Task Update_WhenCalledWithNonExistingAppSetupId_IgnoresNonExistingAppSetupId()
	{
		// Arrange
		var expectedAppSetupId = await _appSetupsDriver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", [expectedAppSetupId]);

		// Act
		var updateRequest = new Grpc.StartupProfiles.UpdateRequest { Id = expectedStartupProfileId, Name = "UpdatedName" };
		updateRequest.AppSetupIds.AddRange([expectedAppSetupId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()]);

		await _startupProfilesClient.UpdateAsync(updateRequest);
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = expectedStartupProfileId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("UpdatedName");
		actualItemResponse.Item.AppSetupIds.Should().BeEquivalentTo([expectedAppSetupId]);
	}

	[Fact]
	public async Task Update_WhenCalledWithoutAppSetupIds_ResetsAppSetupIdsToEmpty()
	{
		// Arrange
		var expectedAppSetupId = await _appSetupsDriver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", [expectedAppSetupId]);

		// Act
		await _startupProfilesClient.UpdateAsync(new Grpc.StartupProfiles.UpdateRequest { Id = expectedStartupProfileId, Name = "UpdatedName" });
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = expectedStartupProfileId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("UpdatedName");
		actualItemResponse.Item.AppSetupIds.Should().BeEmpty();
	}

	[Fact]
	public async Task Update_WhenCalledWithMissingNameParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		Func<Task> act = async () => await _startupProfilesClient.UpdateAsync(new Grpc.StartupProfiles.UpdateRequest { Id = expectedItemId });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Name\":\"\\u0027Name\\u0027 must not be empty.\"}");
	}

	[Fact]
	public async Task Update_WhenCalledWithIncorrectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		Func<Task> act = async () => await _startupProfilesClient.UpdateAsync(new Grpc.StartupProfiles.UpdateRequest { Id = "super incorrect id", Name = "UpdatedName" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Id\":\"Invalid Id value super incorrect id.\"}");
	}

	[Fact]
	public async Task Update_WhenCalledWithIncorrectAppSetupIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		var expectedItemId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		var update = new Grpc.StartupProfiles.UpdateRequest { Id = expectedItemId, Name = "UpdatedName" };
		update.AppSetupIds.Add("super incorrect appSetup id");

		Func<Task> act = async () => await _startupProfilesClient.UpdateAsync(update);

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"AppSetupIds[0]\":\"Invalid Id value super incorrect appSetup id.\"}");
	}

	#endregion

	#region Patch

	[Fact]
	public async Task Patch_WhenCalled_Succeeds()
	{
		// Arrange
		var expectedAppSetupId = await _appSetupsDriver.Prepare("AppSetup1", @"c:\work\app.exe");
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		var patchRequest = new Grpc.StartupProfiles.PatchRequest { Id = expectedStartupProfileId, Name = "PatchedName" };
		patchRequest.AppSetupIds.Add(expectedAppSetupId);

		await _startupProfilesClient.PatchAsync(patchRequest);
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = expectedStartupProfileId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("PatchedName");
		actualItemResponse.Item.AppSetupIds.Should().BeEquivalentTo([expectedAppSetupId]);
	}

	[Fact]
	public async Task Patch_WhenCalledWithJustId_Succeeds()
	{
		// Arrange
		var expectedStartupProfileId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		await _startupProfilesClient.PatchAsync(new Grpc.StartupProfiles.PatchRequest { Id = expectedStartupProfileId });
		var actualItemResponse = _startupProfilesClient.GetById(new Grpc.StartupProfiles.GetByIdRequest() { Id = expectedStartupProfileId });

		// Assert
		actualItemResponse.Should().NotBeNull();
		actualItemResponse.Item.Name.Should().Be("Profile1");
		actualItemResponse.Item.AppSetupIds.Should().BeEmpty();
	}

	[Fact]
	public async Task Patch_WhenCalledWithNonExistingIdParam_ThrowsNotFoundRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.PatchAsync(new Grpc.StartupProfiles.PatchRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		(await act.Should().ThrowAsync<RpcException>())
			.And.StatusCode.Should().Be(StatusCode.NotFound);
	}

	[Fact]
	public async Task Patch_WhenCalledWithIncorectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.PatchAsync(new Grpc.StartupProfiles.PatchRequest { Id = "super incorrect id" });

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
		var expectedItemId = await _startupProfilesDriver.Prepare("Profile1", []);

		// Act
		var response = await _startupProfilesClient.DeleteAsync(new Grpc.StartupProfiles.DeleteRequest { Id = expectedItemId });

		// Assert
		response.Should().BeOfType(typeof(Empty));
	}

	[Fact]
	public async Task Delete_WhenCalledWithNonExistingIdParam_Succeeds()
	{
		// Arrange
		// Act
		var result = await _startupProfilesClient.DeleteAsync(new Grpc.StartupProfiles.DeleteRequest { Id = Guid.NewGuid().ToString() });

		// Assert
		result.Should().BeOfType(typeof(Empty));
	}

	[Fact]
	public async Task Delete_WhenCalledWithIncorectIdParam_ThrowsInvalidArgumentRpcException()
	{
		// Arrange
		// Act
		Func<Task> act = async () => await _startupProfilesClient.DeleteAsync(new Grpc.StartupProfiles.DeleteRequest { Id = "super incorrect id" });

		// Assert
		var result = await act.Should().ThrowAsync<RpcException>();
		result.And.StatusCode.Should().Be(StatusCode.InvalidArgument);
		result.And.Status.Detail.Should().Be("{\"Id\":\"Invalid Id value super incorrect id.\"}");
	}

	#endregion
}
