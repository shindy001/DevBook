using DevBook.Grpc.StartupProfiles;

namespace MauiBlazorClient.Services.Contracts;

public interface IStartupProfilesService
{
	Task<StartupProfileDto> Get(string id);
	Task<IEnumerable<StartupProfileDto>> GetAll();
	Task Create(string name, IEnumerable<string> appSetupIds);
	Task Update(string id, string name, IEnumerable<string> appSetupIds);
	Task Delete(string id);
}
