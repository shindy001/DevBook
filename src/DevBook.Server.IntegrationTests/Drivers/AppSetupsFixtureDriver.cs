using DevBook.Grpc.AppSetups;

namespace DevBook.Server.IntegrationTests.Drivers;

internal class AppSetupsFixtureDriver(AppSetupsGrpcService.AppSetupsGrpcServiceClient _client)
{
	public async Task<string> Prepare(string name, string path, string? arguments = null)
	{
		var response = await _client.CreateAsync(new CreateRequest
		{
			Name = name,
			Path = path,
			Arguments = arguments
		});

		return response.Id;
	}
}
