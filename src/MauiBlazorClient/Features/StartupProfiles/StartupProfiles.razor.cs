using MauiBlazorClient.Services;
using MauiBlazorClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MauiBlazorClient.Features.StartupProfiles;

public partial class StartupProfiles
{
	[Inject] private IMediator Mediator { get; set; } = default!;
	[Inject] private IDialogService DialogService { get; set; } = default!;

	private Model _model = new();
	private bool _loading;
	private readonly DialogOptions _dialogOptions = new DialogOptions() { CloseOnEscapeKey = true };

	protected override async Task OnInitializedAsync() => await LoadData();

	private async Task LoadData()
	{
		_loading = true;
		StateHasChanged();
		_model = await Mediator.Send(new GetModelQuery());
		_loading = false;
	}

	private async Task OpenCreateDialog()
	{
		var parameters = new DialogParameters<CreateOrUpdateDialog>();
		parameters.Add(x => x.AvailableAppSetups, _model.AvailableAppSetups);

		var dialog = await DialogService.ShowAsync<CreateOrUpdateDialog>("Create", _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await LoadData();
		}
	}

	private async Task OpenEditDialog(Model.StartupProfile startupProfile)
	{
		var parameters = new DialogParameters<CreateOrUpdateDialog>();
		parameters.Add(x => x.StartupProfile, startupProfile);
		parameters.Add(x => x.AvailableAppSetups, _model.AvailableAppSetups);

		var dialog = await DialogService.ShowAsync<CreateOrUpdateDialog>("Update", parameters, _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await LoadData();
		}
	}

	private async Task Delete(Model.StartupProfile startupProfile)
	{
		var parameters = new DialogParameters<ConfirmationDialog>();
		parameters.Add(x => x.ContentText, $"Are you sure you want to delete '{startupProfile.Name}' ?");

		var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters, _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await Mediator.Send(new DeleteCommand(startupProfile.Id));
			await LoadData();
		}
	}

	public class GetModelQuery : IRequest<Model> { }

	public record DeleteCommand(string Id) : IRequest;

	public class Model
	{
		public List<StartupProfile> StartupProfiles { get; set; } = [];
		public List<AvailableAppSetup> AvailableAppSetups { get; set; } = [];

		public record StartupProfile(string Id, string Name, IEnumerable<string> AppSetupIds);
		public record AvailableAppSetup(string Id, string Name)
		{
			public override string ToString() => Name;
		};
	}

	public class GetModelQueryHandler(
		IAppSetupsService _appSetupsService,
		IStartupProfilesService _startupProfilesService) : IRequestHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var startupProfileDtos = await _startupProfilesService.List();
			var appSetupDtos = await _appSetupsService.List();
			var availableAppSetups = appSetupDtos?.Select(x => new Model.AvailableAppSetup(x.Id, x.Name)) ?? [];
			var model = new Model { StartupProfiles = [], AvailableAppSetups = availableAppSetups.ToList() };

			foreach (var startuProfileDto in startupProfileDtos)
			{
				var appSetupNames = (await _appSetupsService.GetByIds([.. startuProfileDto.AppSetupIds])).Select(x => x.Name);
				model.StartupProfiles.Add(new Model.StartupProfile(startuProfileDto.Id, startuProfileDto.Name, appSetupNames));
			}
			return model;
		}
	}

	public class DeleteCommandHandler(IStartupProfilesService _startupProfilesService) : IRequestHandler<DeleteCommand>
	{
		public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
		{
			await _startupProfilesService.Delete(request.Id);
		}
	}
}
