using MauiBlazorClient.Settings;
using System.ComponentModel;
using System.Text.Json;

namespace MauiBlazorClient.Services;

public sealed class AppStore : IAppStore
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

		this.DashboardData.PropertyChanged += async (s, a) => await OnDataChanged(s, a);
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

	private async Task OnDataChanged(object? sender, PropertyChangedEventArgs e)
	{
		try
		{
			await SaveData();
		}
		catch (Exception ex)
		{
			// Log error
		}
	}
}
