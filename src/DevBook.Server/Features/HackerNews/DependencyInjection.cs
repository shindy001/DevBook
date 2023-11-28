using DevBook.Server.Features.HackerNews;
using DevBook.Server.Configuration;

namespace DevBook.Server.HackerNews;

internal static class DependencyInjection
{
	internal static IServiceCollection AddHackerNewsApi(this IServiceCollection services, Settings settings)
	{
		services.AddScoped<IHackerNewsApi, HackerNewsApi>();
		services.AddHttpClient<IHackerNewsApi, HackerNewsApi>(client => client.BaseAddress = new Uri(settings.HackerNewsBaseAddress));

		return services;
	}

	internal static IEndpointRouteBuilder UseHackerNewsFeature(this IEndpointRouteBuilder builder)
	{
		builder.MapGrpcService<HackerNewsService>();

		return builder;
	}
}
