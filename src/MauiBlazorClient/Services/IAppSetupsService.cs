using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services
{
	public interface IAppSetupsService
	{
		Task<AppSetupDto> Get(string id);
		Task<IEnumerable<AppSetupDto>> List();
		Task Create(string name, string path, string arguments);
		Task Update(string id, string name, string path, string? arguments);
		Task Delete(string id);
	}
}