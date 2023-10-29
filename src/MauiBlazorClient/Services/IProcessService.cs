namespace MauiBlazorClient.Services;

public interface IProcessService
{
	Task Start(string fileName, string? arguments);
}
