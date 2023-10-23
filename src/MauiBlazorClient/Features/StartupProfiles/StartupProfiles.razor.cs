using MauiBlazorClient.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Features.StartupProfiles
{
	public partial class StartupProfiles
	{
		[Inject] private IMediator Mediator { get; set; } = default!;

		private Model _model = new();
		private bool _loading;

		protected override async Task OnInitializedAsync()
		{
			_loading = true;
			_model = await Mediator.Send(new Query());
			_loading = false;
		}

		public class Query : IRequest<Model> { }

		public class Model
		{
			public List<StartupProfile> StartupProfiles { get; set; } = [];

			public record StartupProfile(string Id, string Name, IEnumerable<string> AppSetupNames);
		}

		public class Handler(
			IAppSetupsService _appSetupsService,
			IStartupProfilesService _startupProfilesService) : IRequestHandler<Query, Model>
		{
			public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
			{
				var startupProfileDtos = await _startupProfilesService.List();
				var model = new Model { StartupProfiles = [] };
				foreach (var startuProfileDto in startupProfileDtos)
				{
					var appSetupNames = (await _appSetupsService.GetByIds([.. startuProfileDto.AppSetupIds])).Select(x => x.Name);
					model.StartupProfiles.Add(new Model.StartupProfile(startuProfileDto.Id, startuProfileDto.Name, appSetupNames));
				}
				return model;
			}
		}
	}
}