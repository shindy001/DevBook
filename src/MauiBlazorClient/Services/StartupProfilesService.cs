using Bogus;
using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services;

public sealed class StartupProfilesService : IStartupProfilesService
{
	private readonly List<StartupProfileDto> _startupProfiles = [];

	public StartupProfilesService()
	{
		var faker = new Faker<StartupProfileDto>()
			.RuleFor(x => x.Id, f => Guid.NewGuid().ToString())
			.RuleFor(x => x.Name, f => f.Lorem.Slug());
		_startupProfiles.AddRange(faker.Generate(10));
	}

	public Task<StartupProfileDto> Get(string id)
	{
		return Task.FromResult(_startupProfiles.Single(x => x.Id == id));
	}

	public Task<IEnumerable<StartupProfileDto>> List()
	{
		return Task.FromResult(_startupProfiles.AsEnumerable());
	}

	public Task Create(string name, IEnumerable<string> appSetupIds)
	{
		_startupProfiles.Add(new StartupProfileDto { Id = Guid.NewGuid().ToString(), Name = name, AppSetupIds = appSetupIds });
		return Task.CompletedTask;
	}

	public Task Update(string id, string name, IEnumerable<string> appSetupIds)
	{
		var itemIndex = _startupProfiles.FindIndex(x => x.Id == id);
		if (itemIndex is not -1)
		{
			_startupProfiles[itemIndex] = new StartupProfileDto { Id = id, Name = name, AppSetupIds = appSetupIds };
		}
		return Task.CompletedTask;
	}

	public Task Delete(string id)
	{
		var appSetup = _startupProfiles.Single(x => x.Id == id);
		if (appSetup is not null)
		{
			_startupProfiles.Remove(appSetup);
		}
		return Task.CompletedTask;
	}
}