using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services
{
	public interface IStartupProfilesService
	{
		Task<StartupProfileDto> Get(string id);
		Task<IEnumerable<StartupProfileDto>> List();
		Task Create(string name, List<string> appSetupIds);
		Task Update(string id, string name, List<string> appSetupIds);
		Task Delete(string id);
	}
}