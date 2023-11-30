using DevBook.Server.Features.HackerNews;

namespace DevBook.Server.IntegrationTests.Fakes;

internal class HackerNewsApiFake : IHackerNewsApi
{
	public Task<IEnumerable<NewsArticle>> GetNews()
	{
		throw new NotImplementedException();
	}
}
