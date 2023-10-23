using Bogus;
using MauiBlazorClient.Services.DTO;

namespace MauiBlazorClient.Services
{
	public sealed class StartupProfilesService : IStartupProfilesService
	{
		private readonly List<StartupProfileDto> _startupProfiles = [];

		public StartupProfilesService()
		{
			var faker = new Faker<StartupProfileDto>()
				.RuleFor(x => x.Id, Guid.NewGuid().ToString())
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

		public Task Create(string name, List<string> appSetupIds)
		{
			_startupProfiles.Add(new StartupProfileDto { Id = Guid.NewGuid().ToString(), Name = name });
			return Task.CompletedTask;
		}

		public Task Update(string id, string name, List<string> appSetupIds)
		{
			var appSetup = _startupProfiles.Single(x => x.Id == id);
			if (appSetup is not null)
			{
				appSetup = new StartupProfileDto { Id = appSetup.Id, Name = name };
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
}