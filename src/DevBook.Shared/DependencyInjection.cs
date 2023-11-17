using DevBook.Shared.Contracts;
using MediatR;
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

	public static IServiceCollection AddPipelineBehavior(this IServiceCollection services, Type pipelineBehavior)
	{
		services.AddScoped(typeof(IPipelineBehavior<,>), pipelineBehavior);

		return services;
	}
}
