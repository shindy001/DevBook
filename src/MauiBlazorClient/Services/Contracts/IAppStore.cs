using MauiBlazorClient.Settings;

namespace MauiBlazorClient.Services.Contracts;

public interface IAppStore
{
	public DashboardData DashboardData { get; }
}
