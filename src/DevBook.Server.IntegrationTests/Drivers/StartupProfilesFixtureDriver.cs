using DevBook.Grpc.StartupProfiles;

namespace DevBook.Server.IntegrationTests.Drivers;

internal class StartupProfilesFixtureDriver(StartupProfilesGrpcService.StartupProfilesGrpcServiceClient _client)
{
	public async Task<string> Prepare(string name, string[] appSetupIds)
	{
		var request = new CreateRequest
		{
			Name = name,
		};
		request.AppSetupIds.AddRange(appSetupIds);

		var response = await _client.CreateAsync(request);
		return response.Id;
	}
}
