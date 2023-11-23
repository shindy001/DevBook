using Microsoft.EntityFrameworkCore;
using DevBook.Shared;
using DevBook.Shared.Contracts;
using DevBook.Server.Common;
using FluentValidation;

namespace DevBook.Server.Infrastructure;

internal static class DependencyInjection
{
	internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		var assembly = typeof(Program).Assembly;
		services.AddLogging(cfg => cfg.AddDevBookLogging());
		services.AddCommandsAndQueriesExecutor(assembly);

		services.AddDbContextPool<DevBookDbContext>(
			o => o.UseSqlite(GetSqliteConnectionString(),
			b => b.MigrationsAssembly(assembly.GetName().Name)));

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddValidatorsFromAssembly(assembly);
		services.AddPipelineBehavior(typeof(CommandValidationPipelineBehavior<,>));
		services.AddPipelineBehavior(typeof(UnitOfWorkCommandPipelineBehavior<,>));
		services.AddPipelineBehavior(typeof(UnitOfWorkQueryPipelineBehavior<,>));

		return services;
	}

	internal static IServiceScope InitializeDb(this IServiceScope scope, bool applyMigrations = false)
	{
		var db = scope.ServiceProvider.GetRequiredService<DevBookDbContext>();
		db.Database.EnsureCreated();

		if (applyMigrations)
		{
			db.Database.Migrate();
		}

		return scope;
	}

	private static string GetSqliteConnectionString()
	{
		var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		var dbPath = Path.Combine(appData, "DevBook", $"DevBook.db");

		if (!Directory.Exists(dbPath))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
		}

		return $"Data Source={dbPath}";
	}
}
