using System.Diagnostics;

namespace MauiBlazorClient.Services;

public class ProcessService : IProcessService
{
	public Task Start(string fileName, string? arguments)
	{
		if (Path.Exists(fileName))
		{
			return Task.Run(() => Process.Start(fileName, arguments ?? string.Empty));
		}
		else
		{
			return Task.Run(() => StartUsingShell(fileName, arguments));
		}
	}

	private Process? StartUsingShell(string fileName, string? arguments)
	{
		ProcessStartInfo startInfo = new()
		{
			FileName = fileName,
			Arguments = arguments ?? string.Empty,
			UseShellExecute = true
		};
		return Process.Start(startInfo);
	}
}
