using Microsoft.Extensions.Logging;
using Serilog;

namespace DevBook.Shared;

public static class LoggingBuilderExtensions
{
	/// <summary>
	/// Adds console and file logging, saves logs to [AppDomain.CurrentDomain.BaseDirectory]/logs/[log Name]
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="logFileName"></param>
	public static ILoggingBuilder AddDevBookLogging(this ILoggingBuilder builder, string logFileName = "Application_log_.txt")
	{
		var appDir = AppDomain.CurrentDomain.BaseDirectory;

		Log.Logger = new LoggerConfiguration()
			.WriteTo.Console()
			.WriteTo.File(Path.Combine(appDir, "logs", logFileName), rollingInterval: RollingInterval.Day)
			.CreateLogger();

		builder.AddSerilog(Log.Logger);
		Log.Information("Ah, there you are!");

		return builder;
	}
}
