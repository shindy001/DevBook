using MauiBlazorClient.Services;
using MauiBlazorClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MauiBlazorClient.Features.AppSetups;

public partial class AppSetups
{
	[Inject] private IMediator Mediator { get; set; } = default!;
	[Inject] private IDialogService DialogService { get; set; } = default!;
	[Inject] private IFilePickerService FilePickerService { get; set; } = default!;

	private Model _model = new();
	private bool _loading;
	private readonly DialogOptions _dialogOptions = new() { CloseOnEscapeKey = true };

	private MudForm _createOrUpdateForm = default!;
	private bool _isValidCreateOrUpdateForm;
	private bool _isCreateOrUpdateDialogVisible;
	private string? _formIdField = null;
	private string? _formNameField = null;
	private string? _formPathField = null;
	private string? _formArgumentsField = null;

	protected override async Task OnInitializedAsync() => await LoadData();

	private async Task LoadData()
	{
		_loading = true;
		StateHasChanged();
		_model = await Mediator.Send(new GetModelQuery());
		_loading = false;
	}

	private void ResetCreateOrUpdateDialogData()
	{
		_formIdField = null;
		_formNameField = null;
		_formPathField = null;
		_formArgumentsField = null;
	}

	private void OpenCreateDialog()
	{
		ResetCreateOrUpdateDialogData();
		_isCreateOrUpdateDialogVisible = true;
	}

	private void OpenEditDialog(Model.AppSetup appSetup)
	{
		_formIdField = appSetup.Id;
		_formNameField = appSetup.Name;
		_formPathField = appSetup.Path;
		_formArgumentsField = appSetup.Arguments;
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
			await Mediator.Send(new CreateOrUpdateCommand { Id = _formIdField, Name = _formNameField, Path = _formPathField, Arguments = _formArgumentsField });
			await LoadData();
			_isCreateOrUpdateDialogVisible = false;
			ResetCreateOrUpdateDialogData();
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
			await Mediator.Send(new DeleteCommand(appSetup.Id));
			await LoadData();
		}
	}

	private void CancelCreateOrUpdateDialog()
	{
		_isCreateOrUpdateDialogVisible = false;
		ResetCreateOrUpdateDialogData();
	}

	private async Task PickPath()
	{
		var (success, path) = await FilePickerService.PickOneAsync();
		if (success)
		{
			_formPathField = path;
		}
	}

	public record Model
	{
		public List<AppSetup> AppSetups { get; set; } = [];

		public record AppSetup(string Id, string Name, string Path, string? Arguments);
	}

	public record GetModelQuery : IRequest<Model> { }

	public record CreateOrUpdateCommand : IRequest
	{
		public string? Id { get; set; }
		public required string Name { get; set; }
		public required string Path { get; set; }
		public string? Arguments { get; set; }
	}

	public record DeleteCommand(string Id) : IRequest;

	public class GetModelQueryHandler(IAppSetupsService _appSetupsService) : IRequestHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var appSetupDtos = await _appSetupsService.List();
			return new Model { AppSetups = appSetupDtos.Select(x => new Model.AppSetup(x.Id, x.Name, x.Path, x.Arguments)).ToList() };
		}
	}

	public class CreateOrUpdateCommandHandler(IAppSetupsService _appSetupsService) : IRequestHandler<CreateOrUpdateCommand>
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

	public class DeleteCommandHandler(IAppSetupsService _appSetupsService) : IRequestHandler<DeleteCommand>
	{
		public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
		{
			await _appSetupsService.Delete(request.Id);
		}
	}
}
