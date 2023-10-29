using MauiBlazorClient.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Features.Dashboard
{
	public partial class StartupProfilesWidget
	{
		[Inject] private IMediator Mediator { get; set; } = default!;
		private Model _model = new();
		private Model.StartupProfileOption? _selectedOption;

		protected override async Task OnInitializedAsync()
		{
			_model = await Mediator.Send(new GetModelQuery());
		}

		private Task LaunchProfileApps()
		{
			if (_selectedOption is not null)
			{
				// TODO
				// Mediator.Send(new LaunchStartupProfile(_selectedOption.Id)
				// Handler:
				//	Get StartupProfile by id
				//	Extract appSetupIds and get them
				//	Launch apps via ProcessService
			}
			return Task.CompletedTask;
		}

		public record GetModelQuery : IRequest<Model>;

		public record Model
		{
			public List<StartupProfileOption> StartupProfileOptions { get; set; } = [];

			public record StartupProfileOption(string Id, string Name)
			{
				public override string ToString() => Name;
			}
		}

		public class GetModelQueryHandler(IStartupProfilesService _startupProfilesService) : IRequestHandler<GetModelQuery, Model>
		{
			public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
			{
				var startupProfileDtos = await _startupProfilesService.List();
				return new Model { StartupProfileOptions = startupProfileDtos?.Select(x => new Model.StartupProfileOption(x.Id, x.Name)).ToList() ?? [] };
			}
		}
	}
}
