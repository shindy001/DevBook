using DevBook.Grpc.HackerNews;

namespace MauiBlazorClient.Services;

public interface IHackerNewsService
{
	Task<IEnumerable<NewsArticleDto>> GetNews();
}
