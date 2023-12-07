using MauiBlazorClient.Settings;

namespace MauiBlazorClient.Services;

public interface IAppStore
{
	public DashboardData DashboardData { get; }
}
