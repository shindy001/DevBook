using DevBook.Grpc.HackerNews;

namespace MauiBlazorClient.Services;

internal sealed class HackerNewsService(HackerNewsGrpcService.HackerNewsGrpcServiceClient _hackerNewsGrpcService) : IHackerNewsService
{
	public async Task<IEnumerable<NewsArticleDto>> GetNews()
	{
		var result = await _hackerNewsGrpcService.GetNewsAsync(new GetNewsRequest());
		return result.Items;
	}
}
