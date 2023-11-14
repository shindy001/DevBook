using DevBook.Shared.Contracts;
using MauiBlazorClient.Services;
using MauiBlazorClient.Services.DTO;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Features.Dashboard;

public partial class StartupProfilesWidget
{
	[Inject] private IExecutor Executor { get; set; } = default!;
	[Inject] private IAppStore AppStore { get; set; } = default!;

	private Model _model = new();
	private Model.StartupProfileOption? _selectedOption;

	protected override async Task OnInitializedAsync()
	{
		_model = await Executor.ExecuteQuery(new GetModelQuery());
		_selectedOption = _model.StartupProfileOptions.FirstOrDefault(x => x.Id.Equals(AppStore.DashboardData.SelectedProfileId));
	}

	private async Task LaunchProfileApps()
	{
		if (_selectedOption is null)
		{
			return;
		}

		await Executor.ExecuteCommand(new LaunchProfileAppsCommand(_selectedOption.Id));
	}

	private void OnSelectedOptionChanged(Model.StartupProfileOption option)
	{
		_selectedOption = option;
		AppStore.DashboardData.SelectedProfileId = option.Id;
	}

	public record GetModelQuery : IQuery<Model>;
	public record LaunchProfileAppsCommand(string Id) : ICommand;

	public record Model
	{
		public List<StartupProfileOption> StartupProfileOptions { get; set; } = [];

		public record StartupProfileOption(string Id, string Name)
		{
			public override string ToString() => Name;
		}
	}

	public class GetModelQueryHandler(IStartupProfilesService _startupProfilesService)
		: IQueryHandler<GetModelQuery, Model>
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
		: ICommandHandler<LaunchProfileAppsCommand>
	{
		public async Task Handle(LaunchProfileAppsCommand request, CancellationToken cancellationToken)
		{
			var profile = await _startupProfilesService.Get(request.Id);
			if (profile?.AppSetupIds?.Any() == true)
			{
				await LaunchApps(profile.AppSetupIds);
			}
		}

		private async Task LaunchApps(IEnumerable<string> appSetupIds)
		{
			var appSetups = await _appSetupsService.GetByIds(appSetupIds.ToArray());

			foreach (var appSetup in appSetups)
			{
				await LaunchApp(appSetup);
			}
		}

		private async Task LaunchApp(AppSetupDto appSetup)
		{
			try
			{
				await _processService.Start(appSetup.Path, appSetup.Arguments);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException($"Cannot start AppSetup '{appSetup.Name}' with path '{appSetup.Path}'. Details: {e.Message}");
			}
		}
	}
}
