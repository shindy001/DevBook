﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiBlazorClient.Settings;

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

	private bool _isDarkModeEnabled = false;

	public bool IsDarkModeEnabled
	{
		get => _isDarkModeEnabled;
		set => SetProperty(ref _isDarkModeEnabled, value);
	}
}
