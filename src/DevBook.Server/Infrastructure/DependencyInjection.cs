using Microsoft.EntityFrameworkCore;
using DevBook.Shared;
using DevBook.Shared.Contracts;
using FluentValidation;
using DevBook.Server.Common;

namespace DevBook.Server.Infrastructure;

internal static class DependencyInjection
{
	internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
	{
		var assembly = typeof(Program).Assembly;
		services.AddLogging(cfg => cfg.AddDevBookLogging());

		services.AddDbContextPool<DevBookDbContext>(
			o => o.UseSqlite(GetSqliteConnectionString(),
			b => b.MigrationsAssembly(assembly.GetName().Name)));

		services.AddGrpc(cfg => cfg.Interceptors.Add<GrpgGlobalExceptionInterceptor>());
		services.AddGrpcReflection();

		services.AddCommandsAndQueriesExecutor(assembly);

		services.AddValidatorsFromAssembly(assembly);
		services.AddPipelineBehavior(typeof(CommandValidationPipelineBehavior<,>));

		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddPipelineBehavior(typeof(UnitOfWorkCommandPipelineBehavior<,>));
		services.AddPipelineBehavior(typeof(UnitOfWorkQueryPipelineBehavior<,>));

		return services;
	}

	internal static IApplicationBuilder InitializeDb(this IApplicationBuilder builder, bool applyMigrations = false)
	{
		using var scope = builder.ApplicationServices.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<DevBookDbContext>();
		db.Database.EnsureCreated();

		if (applyMigrations)
		{
			db.Database.Migrate();
		}

		return builder;
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
