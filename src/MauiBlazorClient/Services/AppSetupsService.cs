using Bogus;
using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services;

internal sealed class AppSetupsService : IAppSetupsService
{
	private readonly List<AppSetupDto> _appSetups = [];

	public AppSetupsService()
	{
		var faker = new Faker<AppSetupDto>()
			.RuleFor(x => x.Id, f => Guid.NewGuid().ToString())
			.RuleFor(x => x.Name, f => f.System.FileName())
			.RuleFor(x => x.Path, f => f.System.FilePath())
			.RuleFor(x => x.Arguments, f => f.Lorem.Word());
		_appSetups.AddRange(faker.Generate(10));
	}

	public Task<AppSetupDto> Get(string id)
	{
		return Task.FromResult(_appSetups.Single(x => x.Id == id));
	}

	public Task<IEnumerable<AppSetupDto>> GetByIds(params string[] ids)
	{
		return Task.FromResult(_appSetups.Where(x => ids.Contains(x.Id)).AsEnumerable());
	}

	public Task<IEnumerable<AppSetupDto>> List()
	{
		return Task.FromResult(_appSetups.AsEnumerable());
	}

	public Task Create(string name, string path, string? arguments)
	{
		_appSetups.Add(new AppSetupDto { Id = Guid.NewGuid().ToString(), Name = name, Path = path, Arguments = arguments });
		return Task.CompletedTask;
	}

	public Task Update(string id, string name, string path, string? arguments)
	{
		var itemIndex = _appSetups.FindIndex(x => x.Id == id);
		if (itemIndex is not -1)
		{
			_appSetups[itemIndex] = new AppSetupDto { Id = id, Name = name, Path = path, Arguments = arguments };
		}
		return Task.CompletedTask;
	}

	public Task Delete(string id)
	{
		var appSetup = _appSetups.Single(x => x.Id == id);
		if (appSetup is not null)
		{
			_appSetups.Remove(appSetup);
		}
		return Task.CompletedTask;
	}
}