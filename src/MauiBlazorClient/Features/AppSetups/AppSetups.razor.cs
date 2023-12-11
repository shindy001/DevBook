using DevBook.Shared.Contracts;
using MauiBlazorClient.Services;
using MauiBlazorClient.Services.Contracts;
using MauiBlazorClient.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MauiBlazorClient.Features.AppSetups;

public partial class AppSetups
{
	[Inject] private IExecutor Executor { get; init; } = default!;
	[Inject] private IDialogService DialogService { get; init; } = default!;
	[Inject] private IFilePickerService FilePickerService { get; init; } = default!;

	private Model _model = new();
	private bool _loading;
	private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true };

	private MudForm _createOrUpdateForm = default!;
	private bool _isValidCreateOrUpdateForm;
	private bool _isCreateOrUpdateDialogVisible;

	private readonly Model.AppSetup EditModel = new();

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

	private void OpenEditDialog(Model.AppSetup appSetup)
	{
		EditModel.Id = appSetup.Id;
		EditModel.Name = appSetup.Name;
		EditModel.Path = appSetup.Path;
		EditModel.Arguments = appSetup.Arguments;
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
					Path = EditModel.Path, Arguments = EditModel.Arguments
				});

			await LoadData();
			_isCreateOrUpdateDialogVisible = false;
			EditModel.Reset();
		}
	}

	private async Task SubmitDelete(Model.AppSetup appSetup)
	{
		var parameters = new DialogParameters<ConfirmationDialog>();
		parameters.Add(x => x.ContentText, $"Are you sure you want to delete '{appSetup.Name}' ?");

		var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Delete", parameters, _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await Executor.ExecuteCommand(new DeleteCommand(appSetup.Id));
			await LoadData();
		}
	}

	private void CancelCreateOrUpdateDialog()
	{
		_isCreateOrUpdateDialogVisible = false;
		EditModel.Reset();
	}

	private async Task PickPath()
	{
		var (success, path) = await FilePickerService.PickOneAsync();
		if (success)
		{
			EditModel.Path = path;
		}
	}

	public record Model
	{
		public List<AppSetup> AppSetups { get; set; } = [];

		public record AppSetup
		{
			public string? Id { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Path { get; set; } = string.Empty;
			public string? Arguments { get; set; }

			public void Reset()
			{
				Id = null;
				Name = string.Empty;
				Path = string.Empty;
				Arguments = null;
			}
		}
	}

	public record GetModelQuery : IQuery<Model> { }

	public record CreateOrUpdateCommand : ICommand
	{
		public string? Id { get; set; }
		public required string Name { get; set; }
		public required string Path { get; set; }
		public string? Arguments { get; set; }
	}

	public record DeleteCommand(string Id) : ICommand;

	public class GetModelQueryHandler(IAppSetupsService _appSetupsService) : IQueryHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var appSetupDtos = await _appSetupsService.GetAll();
			return new Model { AppSetups = appSetupDtos.Select(
				x => new Model.AppSetup { Id = x.Id, Name = x.Name, Path = x.Path, Arguments = x.Arguments })
				.ToList() };
		}
	}

	public class CreateOrUpdateCommandHandler(IAppSetupsService _appSetupsService) : ICommandHandler<CreateOrUpdateCommand>
	{
		public async Task Handle(CreateOrUpdateCommand request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(request.Id))
			{
				await _appSetupsService.Create(request.Name, request.Path, request.Arguments);
			}
			else
			{
				await _appSetupsService.Update(request.Id, request.Name, request.Path, request.Arguments);
			}
		}
	}

	public class DeleteCommandHandler(IAppSetupsService _appSetupsService) : ICommandHandler<DeleteCommand>
	{
		public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
		{
			await _appSetupsService.Delete(request.Id);
		}
	}
}
