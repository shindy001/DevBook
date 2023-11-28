using DevBook.Server.Features.StartupProfiles;

namespace DevBook.Server.StartupProfiles;

internal static class DependencyInjection
{
	internal static IEndpointRouteBuilder UseStartupProfilesFeature(this IEndpointRouteBuilder builder)
	{
		builder.MapGrpcService<StartupProfilesService>();

		return builder;
	}
}
