using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiBlazorClient.Services.DTO
{
	public sealed class DevBookSettings
	{
		public DashboardData DashboardData { get; init; } = new();
	}

	public sealed class DashboardData : ObservableObject
	{
		private string _selectedProfileId = string.Empty;

		public string SelectedProfileId
		{
			get => _selectedProfileId;
			set => SetProperty(ref _selectedProfileId, value);
		}
	}
}
