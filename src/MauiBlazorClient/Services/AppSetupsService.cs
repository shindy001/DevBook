using DevBook.Grpc.AppSetups;

namespace MauiBlazorClient.Services;

internal sealed class AppSetupsService(AppSetupsGrpcService.AppSetupsGrpcServiceClient _appSetupsGrpcService) : IAppSetupsService
{
	public async Task<AppSetupDto> GetById(string id)
	{
		var result = await _appSetupsGrpcService.GetByIdAsync(new GetByIdRequest { Id = id });
		return result.Item;
	}

	public async Task<IEnumerable<AppSetupDto>> GetByIds(params string[] ids)
	{
		var result = await _appSetupsGrpcService.GetAllAsync(new GetAllRequest());
		return result.Items.Where(x => ids.Contains(x.Id));
	}

	public async Task<IEnumerable<AppSetupDto>> GetAll()
	{
		var result = await _appSetupsGrpcService.GetAllAsync(new GetAllRequest());
		return result.Items;
	}

	public async Task Create(string name, string path, string? arguments)
	{
		await _appSetupsGrpcService.CreateAsync(new CreateRequest { Name =  name, Path = path, Arguments = arguments ?? string.Empty });
	}

	public async Task Update(string id, string name, string path, string? arguments)
	{
		await _appSetupsGrpcService.UpdateAsync(new UpdateRequest { Id = id, Name = name, Path = path, Arguments = arguments ?? string.Empty });
	}

	public async Task Delete(string id)
	{
		await _appSetupsGrpcService.DeleteAsync(new DeleteRequest { Id = id });
	}
}
