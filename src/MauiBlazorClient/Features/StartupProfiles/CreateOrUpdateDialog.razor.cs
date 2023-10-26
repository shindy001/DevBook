using MauiBlazorClient.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MauiBlazorClient.Features.StartupProfiles;

public partial class CreateOrUpdateDialog
{
	[CascadingParameter] private MudDialogInstance MudDialog { get; set; } = default!;
	[Inject] private IMediator Mediator { get; set; } = default!;

	[Parameter] public StartupProfiles.Model.StartupProfile? StartupProfile { get; set; }
	[Parameter] public List<StartupProfiles.Model.AvailableAppSetup> AvailableAppSetups { get; set; } = [];

	private string _id = string.Empty;
	private string _name = string.Empty;
	private IEnumerable<StartupProfiles.Model.AvailableAppSetup> _selectedAppSetups = [];

	private MudForm _form = default!;
	private bool _isValidForm;

	protected override void OnParametersSet()
	{
		_id = StartupProfile?.Id ?? string.Empty;
		_name = StartupProfile?.Name ?? string.Empty;
		_selectedAppSetups = AvailableAppSetups.Where(x => StartupProfile?.AppSetupIds.Contains(x.Id) == true) ?? [];
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
			await Mediator.Send(new Command { Id = _id, Name = _name, AppSetupIds = _selectedAppSetups.Select(x => x.Id).ToList() });
			MudDialog.Close(DialogResult.Ok(true));
		}
	}

	private void Cancel() => MudDialog.Cancel();

	public record Command : IRequest
	{
		public string? Id { get; set; }
		public required string Name { get; set; }
		public List<string> AppSetupIds { get; set; } = [];
	}

	public class Handler(IStartupProfilesService _startupProfilesService) : IRequestHandler<Command>
	{
		public async Task Handle(Command request, CancellationToken cancellationToken)
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
}
