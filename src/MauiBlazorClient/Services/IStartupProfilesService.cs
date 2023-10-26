using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services
{
	public interface IStartupProfilesService
	{
		Task<StartupProfileDto> Get(string id);
		Task<IEnumerable<StartupProfileDto>> List();
		Task Create(string name, IEnumerable<string> appSetupIds);
		Task Update(string id, string name, IEnumerable<string> appSetupIds);
		Task Delete(string id);
	}
}