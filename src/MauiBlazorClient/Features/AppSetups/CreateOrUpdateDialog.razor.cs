using MauiBlazorClient.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MauiBlazorClient.Features.AppSetups;

public partial class CreateOrUpdateDialog
{
	[CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;
	[Inject] private IMediator Mediator { get; set; } = default!;
	[Inject] private IFilePickerService FilePickerService { get; set; } = default!;

	[Parameter] public AppSetups.Model.AppSetup? AppSetup { get; set; }

	private string _id = string.Empty;
	private string _name = string.Empty;
	private string _path = string.Empty;
	private string? _arguments = string.Empty;

	private MudForm _form = default!;
	private bool _isValidForm;

	protected override void OnParametersSet()
	{
		_id = AppSetup?.Id ?? string.Empty;
		_name = AppSetup?.Name ?? string.Empty;
		_path = AppSetup?.Path ?? string.Empty;
		_arguments = AppSetup?.Arguments ?? string.Empty;
	}

	private async Task SubmitOnEnterPress(KeyboardEventArgs e)
	{
		if (e.Code == "Enter")
		{
			await Submit();
		}
	}

	private async Task Submit()
	{
		await _form.Validate();
		if (_isValidForm)
		{
			await Mediator.Send(new Command { Id = _id, Name = _name, Path = _path, Arguments = _arguments });
			MudDialog.Close(DialogResult.Ok(true));
		}
	}

	private void Cancel() => MudDialog.Cancel();

	private async Task PickPath()
	{
		var (success, path) = await FilePickerService.PickOneAsync();
		if (success)
		{
			_path = path;
		}
	}

	public record Command : IRequest
	{
		public string? Id { get; set; }
		public required string Name { get; set; }
		public required string Path { get; set; }
		public string? Arguments { get; set; }
	}

	public class Handler(IAppSetupsService _appSetupsService) : IRequestHandler<Command>
	{
		public async Task Handle(Command request, CancellationToken cancellationToken)
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
}
