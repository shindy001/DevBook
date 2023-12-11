namespace MauiBlazorClient.Services.Contracts;

public interface IProcessService
{
	Task Start(string fileName, string? arguments);
}
