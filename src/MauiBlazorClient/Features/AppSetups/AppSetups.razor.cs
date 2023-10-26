﻿using MauiBlazorClient.Services;
using MauiBlazorClient.Shared;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MauiBlazorClient.Features.AppSetups;

public partial class AppSetups
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
		var dialog = await DialogService.ShowAsync<CreateOrUpdateDialog>("Create", _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await LoadData();
		}
	}

	private async Task OpenEditDialog(Model.AppSetup appSetup)
	{
		var parameters = new DialogParameters<CreateOrUpdateDialog>();
		parameters.Add(x => x.AppSetup, appSetup);

		var dialog = await DialogService.ShowAsync<CreateOrUpdateDialog>("Update", parameters, _dialogOptions);
		var result = await dialog.Result;
		if (!result.Canceled)
		{
			await LoadData();
		}
	}

	private async Task Delete(Model.AppSetup appSetup)
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

	public record DeleteCommand(string Id) : IRequest;

	public record GetModelQuery : IRequest<Model> { }

	public record Model
	{
		public List<AppSetup> AppSetups { get; set; } = [];

		public record AppSetup(string Id, string Name, string Path, string? Arguments);
	}

	public class GetModelQueryHandler(IAppSetupsService _appSetupsService) : IRequestHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var appSetupDtos = await _appSetupsService.List();
			return new Model { AppSetups = appSetupDtos.Select(x => new Model.AppSetup(x.Id, x.Name, x.Path, x.Arguments)).ToList() };
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
