using DevBook.Server.Features.AppSetups;

namespace DevBook.Server.AppSetups;

internal static class DependencyInjection
{
	internal static IEndpointRouteBuilder UseAppSetupsFeature(this IEndpointRouteBuilder builder)
	{
		builder.MapGrpcService<AppSetupsService>();

		return builder;
	}
}
