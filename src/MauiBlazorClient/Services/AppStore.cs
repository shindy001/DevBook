using MauiBlazorClient.Services.Contracts;
using MauiBlazorClient.Settings;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Text.Json;

namespace MauiBlazorClient.Services;

public sealed class AppStore : IAppStore
{
	private readonly string DataFile = "DevBook\\DevBookSettings.json";
	private string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DataFile);
	private readonly JsonSerializerOptions JsonSerializerOptions = new() { WriteIndented = true };
	private readonly ILogger<AppStore> _logger;

	public DevBookSettings DevBookSettings { get; private set; } = new();
	public DashboardData DashboardData => DevBookSettings.DashboardData;

	public AppStore(ILogger<AppStore> logger)
	{
		_logger = logger;
		Initialize();
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

	private void Initialize()
	{
		if (File.Exists(DataPath))
		{
			var file = File.ReadAllText(DataPath);
			this.DevBookSettings = JsonSerializer.Deserialize<DevBookSettings>(file) ?? this.DevBookSettings;
		}

		this.DashboardData.PropertyChanged += async (s, a) => await OnDataChanged(s, a);
	}

	private async Task OnDataChanged(object? sender, PropertyChangedEventArgs e)
	{
		try
		{
			await SaveData();
		}
		catch (Exception ex)
		{
			_logger.LogError("Error while saving AppStore data: {ex}", ex);
			throw;
		}
	}
}
