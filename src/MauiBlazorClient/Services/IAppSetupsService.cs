using DevBook.Grpc.AppSetups;

namespace MauiBlazorClient.Services;

public interface IAppSetupsService
{
	Task<AppSetupDto> GetById(string id);
	Task<IEnumerable<AppSetupDto>> GetByIds(params string[] ids);
	Task<IEnumerable<AppSetupDto>> GetAll();
	Task Create(string name, string path, string? arguments);
	Task Update(string id, string name, string path, string? arguments);
	Task Delete(string id);
}
