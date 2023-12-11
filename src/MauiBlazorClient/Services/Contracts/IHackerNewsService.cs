using DevBook.Grpc.HackerNews;

namespace MauiBlazorClient.Services.Contracts;

public interface IHackerNewsService
{
	Task<IEnumerable<NewsArticleDto>> GetNews();
}
