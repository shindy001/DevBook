using DevBook.Shared.Contracts;
using MauiBlazorClient.Services.Contracts;
using MauiBlazorClient.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MauiBlazorClient.Features.StartupProfiles;

public partial class StartupProfiles
{
	[Inject] private IExecutor Executor { get; init; } = default!;
	[Inject] private IDialogService DialogService { get; init; } = default!;

	private Model _model = new();
	private bool _loading;
	private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true };

	private MudForm _createOrUpdateForm = default!;
	private bool _isValidCreateOrUpdateForm;
	private bool _isCreateOrUpdateDialogVisible;

	private readonly Model.StartupProfile EditModel = new();

	protected override async Task OnInitializedAsync() => await LoadData();

	private async Task LoadData()
	{
		_loading = true;
		StateHasChanged();
		_model = await Executor.ExecuteQuery(new GetModelQuery());
		_loading = false;
	}

	private void OpenCreateDialog()
	{
		EditModel.Reset();
		_isCreateOrUpdateDialogVisible = true;
	}

	private void OpenEditDialog(Model.StartupProfile startupProfile)
	{
		EditModel.Id = startupProfile.Id;
		EditModel.Name = startupProfile.Name;
		EditModel.AppSetupIds = _model.AvailableAppSetups.Select(x => x.Id).Where(id => startupProfile?.AppSetupIds.Contains(id) == true) ?? [];
		_isCreateOrUpdateDialogVisible = true;
	}

	private async Task SubmitOnEnterPress(KeyboardEventArgs e)
	{
		if (e.Code == "Enter")
		{
			await SubmitCreateOrUpdate();
		}
	}

	private async Task SubmitCreateOrUpdate()
	{
		await _createOrUpdateForm.Validate();
		if (_isValidCreateOrUpdateForm)
		{
			await Executor.ExecuteCommand(
				new CreateOrUpdateCommand
				{
					Id = EditModel.Id,
					Name = EditModel.Name,
					AppSetupIds = EditModel.AppSetupIds.ToList()
				});

			await LoadData();
			_isCreateOrUpdateDialogVisible = false;
			EditModel.Reset();
		}
	}

	private async Task SubmitDelete(Model.StartupProfile startupProfile)
	{
		var parameters = new DialogParameters<ConfirmationDialog>();
		parameters.Add(x => x.ContentText, $"Are you sure you want to delete '{startupProfile.Name}' ?");

		var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters, _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await Executor.ExecuteCommand(new DeleteCommand(startupProfile.Id));
			await LoadData();
		}
	}

	private void CancelCreateOrUpdateDialog()
	{
		_isCreateOrUpdateDialogVisible = false;
		EditModel.Reset();
	}

	public class GetModelQuery : IQuery<Model> { }

	public record CreateOrUpdateCommand : ICommand
	{
		public string? Id { get; set; }
		public required string Name { get; set; }
		public List<string> AppSetupIds { get; set; } = [];
	}

	public record DeleteCommand(string Id) : ICommand;

	public class Model
	{
		public List<StartupProfile> StartupProfiles { get; set; } = [];
		public List<AvailableAppSetup> AvailableAppSetups { get; set; } = [];

		public record StartupProfile
		{
			public string Id { get; set; } = string.Empty;
			public string Name { get; set; } = string.Empty;
			public IEnumerable<string> AppSetupIds { get; set; } = Enumerable.Empty<string>();

			public void Reset()
			{
				Id = string.Empty;
				Name = string.Empty;
				AppSetupIds = Enumerable.Empty<string>();
			}
		}

		public record AvailableAppSetup(string Id, string Name)
		{
			public override string ToString() => Name;
		};
	}

	public class GetModelQueryHandler(
		IAppSetupsService _appSetupsService,
		IStartupProfilesService _startupProfilesService) : IQueryHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var startupProfileDtos = await _startupProfilesService.GetAll();
			var appSetupDtos = await _appSetupsService.GetAll();
			var availableAppSetups = appSetupDtos?.Select(x => new Model.AvailableAppSetup(x.Id, x.Name)) ?? [];
			var model = new Model { StartupProfiles = [], AvailableAppSetups = availableAppSetups.ToList() };

			foreach (var startuProfileDto in startupProfileDtos)
			{
				var appSetupNames = (await _appSetupsService.GetByIds([.. startuProfileDto.AppSetupIds])).Select(x => x.Name);
				model.StartupProfiles.Add(
					new Model.StartupProfile
					{
						Id = startuProfileDto.Id,
						Name = startuProfileDto.Name,
						AppSetupIds = appSetupNames
					});
			}
			return model;
		}
	}

	public class CreateOrUpdateCommandHandler(IStartupProfilesService _startupProfilesService)
		: ICommandHandler<CreateOrUpdateCommand>
	{
		public async Task Handle(CreateOrUpdateCommand request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.Id))
			{
				await _startupProfilesService.Create(request.Name, request.AppSetupIds);
			}
			else
			{
				await _startupProfilesService.Update(request.Id, request.Name, request.AppSetupIds);
			}
		}
	}

	public class DeleteCommandHandler(IStartupProfilesService _startupProfilesService)
		: ICommandHandler<DeleteCommand>
	{
		public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
		{
			await _startupProfilesService.Delete(request.Id);
		}
	}
}
