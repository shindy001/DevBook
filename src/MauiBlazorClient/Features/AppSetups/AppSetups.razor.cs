using MauiBlazorClient.Services;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Features.AppSetups
{
	public partial class AppSetups
	{
		[Inject] private IMediator Mediator { get; set; } = default!;

		private Model _model = new();
		private bool _loading = true;

		protected override async Task OnInitializedAsync()
		{
			_model = await Mediator.Send(new Query());
			_loading = false;
		}

		public class Query() : IRequest<Model> { }

		public class Model()
		{
			public List<AppSetup> AppSetups { get; set; } = [];

			public record AppSetup(string Id, string Name, string Path, string? Arguments);
		}

		public class Handler(IAppSetupsService _appSetupsService) : IRequestHandler<Query, Model>
		{
			public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
			{
				var appSetupDtos = await _appSetupsService.List();
				return new Model { AppSetups = appSetupDtos.Select(x => new Model.AppSetup(x.Id, x.Name, x.Path, x.Arguments)).ToList() };
			}
		}
	}
}
