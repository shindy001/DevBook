using DevBook.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevBook.Shared;

public static class DependencyInjection
{
	public static IServiceCollection AddCommandsAndQueriesExecutor(this IServiceCollection services, params Assembly[] assemblies)
	{
		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

		services.AddScoped<IExecutor, Executor>();

		return services;
	}
}
