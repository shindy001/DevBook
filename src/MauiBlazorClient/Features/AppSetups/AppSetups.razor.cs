using MauiBlazorClient.Services;
using MauiBlazorClient.Services.DTO;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Features.AppSetups
{
	public partial class AppSetups
	{
		[Inject]
		private IAppSetupsService AppSetupsService { get; set; } = default!;

		private List<AppSetupDto> _appSetups = [];
		private bool _loading = true;

		protected override async Task OnInitializedAsync()
		{
			await Task.Delay(3000);
			_appSetups = (await this.AppSetupsService.List()).ToList();
			_loading = false;
		}
	}
}
