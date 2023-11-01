using MauiBlazorClient.Services;
using MauiBlazorClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MauiBlazorClient.Features.Dashboard;

public partial class StartupProfilesWidget
{
	[Inject] private IDialogService DialogService { get; set; } = default!;
	[Inject] private IMediator Mediator { get; set; } = default!;
	[Inject] private IAppStore AppStore { get; set; } = default!;

	private Model _model = new();
	private Model.StartupProfileOption? _selectedOption;

	protected override async Task OnInitializedAsync()
	{
		_model = await Mediator.Send(new GetModelQuery());
		_selectedOption = _model.StartupProfileOptions.FirstOrDefault(x => x.Id.Equals(AppStore.DashboardData.SelectedProfileId));
	}

	private async Task LaunchProfileApps()
	{
		if (_selectedOption is null)
		{
			return;
		}

		var result  = await Mediator.Send(new LaunchProfileAppsCommand(_selectedOption.Id));
		if (!result.Success)
		{
			var parameters = new DialogParameters<ErrorDialog>();
			parameters.Add(x => x.ErrorMessage, result.Errors is null ? null : string.Join(Environment.NewLine, result.Errors));
			var dialog = await DialogService.ShowAsync<ErrorDialog>("Error", parameters, new DialogOptions() { CloseOnEscapeKey = true });
		}
	}

	private void OnSelectedOptionChanged(Model.StartupProfileOption option)
	{
		_selectedOption = option;
		AppStore.DashboardData.SelectedProfileId = option.Id;
	}

	public record GetModelQuery : IRequest<Model>;
	public record LaunchProfileAppsCommand(string Id) : IRequest<LaunchProfileAppsCommand.Result>
	{
		public record Result(bool Success, string[]? Errors = null);
	}

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

	public class LaunchProfileAppsHandler(
		IStartupProfilesService _startupProfilesService,
		IAppSetupsService _appSetupsService,
		IProcessService _processService)
	: IRequestHandler<LaunchProfileAppsCommand, LaunchProfileAppsCommand.Result>
	{
		public async Task<LaunchProfileAppsCommand.Result> Handle(LaunchProfileAppsCommand request, CancellationToken cancellationToken)
		{
			var profile = await _startupProfilesService.Get(request.Id);
			return profile?.AppSetupIds?.Any() == true
				? await LaunchApps(profile.AppSetupIds)
				: new(Success: true);
		}

		private async Task<LaunchProfileAppsCommand.Result> LaunchApps(IEnumerable<string> appSetupIds)
		{
			var appSetups = await _appSetupsService.GetByIds(appSetupIds.ToArray());

			foreach (var appSetup in appSetups)
			{
				var result = await LaunchApp(appSetup.Path, appSetup.Arguments);
				if (!result.Success)
				{
					return result;
				}
			}

			return new(Success: true);
		}

		private async Task<LaunchProfileAppsCommand.Result> LaunchApp(string appSetupPath, string? appSetupArguments)
		{
			try
			{
				await _processService.Start(appSetupPath, appSetupArguments);
				return new(Success: true);
			}
			catch (Exception e)
			{
				// Log error
				return new(Success: false, Errors: [$"Error while trying to start appSetup: '{appSetupPath}'. Details: {e.Message}"]);
			}
		}
	}
}
