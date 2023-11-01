using MauiBlazorClient.Services.DTO;
using System.ComponentModel;
using System.Text.Json;

namespace MauiBlazorClient.Services
{
	public interface IAppStore
	{
		public DashboardData DashboardData { get; }
	}

	public sealed class AppStore : IAppStore, IDisposable
	{
		private readonly string DataFile = "DevBook\\DevBookSettings.json";
		private string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataFile);
		private readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };

		public DevBookSettings DevBookSettings { get; private set; } = new();

		public DashboardData DashboardData => DevBookSettings.DashboardData;

		public void Initialize()
		{
			if (File.Exists(DataPath))
			{
				var file = File.ReadAllText(DataPath);
				this.DevBookSettings = JsonSerializer.Deserialize<DevBookSettings>(file) ?? this.DevBookSettings;
			}
			this.DashboardData.PropertyChanged += OnDataChanged;
		}

		public async Task SaveData()
		{
			var json = JsonSerializer.Serialize(DevBookSettings, JsonSerializerOptions);
			var directory = Path.GetDirectoryName(DataPath);
			if (directory != null && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			await File.WriteAllTextAsync(DataPath, json);
		}

		private void OnDataChanged(object? sender, PropertyChangedEventArgs e)
		{
			_ = SaveData();
		}

		public void Dispose()
		{
			if (DevBookSettings?.DashboardData != null)
			{
				this.DashboardData.PropertyChanged -= OnDataChanged;
			}
		}
	}
}
