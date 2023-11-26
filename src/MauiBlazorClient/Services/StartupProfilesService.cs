using DevBook.Grpc.StartupProfiles;

namespace MauiBlazorClient.Services;

public sealed class StartupProfilesService(StartupProfilesGrpcService.StartupProfilesGrpcServiceClient _startupProfilesGrpcService) : IStartupProfilesService
{
	public async Task<StartupProfileDto> Get(string id)
	{
		var result = await _startupProfilesGrpcService.GetByIdAsync(new GetByIdRequest { Id = id });
		return result.Item;
	}

	public async Task<IEnumerable<StartupProfileDto>> GetAll()
	{
		var result = await _startupProfilesGrpcService.GetAllAsync(new GetAllRequest());
		return result.Items;
	}

	public async Task Create(string name, IEnumerable<string> appSetupIds)
	{
		var request = new CreateRequest { Name = name };
		request.AppSetupIds.AddRange(appSetupIds);
		await _startupProfilesGrpcService.CreateAsync(request);
	}

	public async Task Update(string id, string name, IEnumerable<string> appSetupIds)
	{
		var request = new UpdateRequest { Id = id, Name = name };
		request.AppSetupIds.AddRange(appSetupIds);
		await _startupProfilesGrpcService.UpdateAsync(request);
	}

	public async Task Delete(string id)
	{
		await _startupProfilesGrpcService.DeleteAsync(new DeleteRequest { Id = id });
	}
}
