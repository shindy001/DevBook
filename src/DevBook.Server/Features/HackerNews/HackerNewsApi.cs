using System.Collections.Concurrent;
using System.Text.Json;

namespace DevBook.Server.Features.HackerNews;

internal interface IHackerNewsApi
{
	Task<IEnumerable<NewsArticle>> GetNews();
}

internal sealed class HackerNewsApi(HttpClient _httpClient, ILogger<HackerNewsApi> _logger) : IHackerNewsApi, IDisposable
{
	private readonly int NewsCount = 10;
	private const string DefaultHackerNewsArticleUrl = "https://news.ycombinator.com/item?id=";

	public async Task<IEnumerable<NewsArticle>> GetNews()
	{
		try
		{
			var newsIds = await _httpClient.GetFromJsonAsync<long[]>($"v0/newstories.json",
				new JsonSerializerOptions(JsonSerializerDefaults.Web));

			ConcurrentBag<NewsArticle> articles = [];

			Parallel.ForEach(newsIds?.Take(NewsCount) ?? [], id =>
			{
				var newsArticle = GetArticle(id).GetAwaiter().GetResult();
				if (newsArticle != null)
				{
					articles.Add(newsArticle);
				}
			});
		
			return articles;
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error while getting Hacker News: {ex.Message}", ex);
			throw;
		}
	}

	private async Task<NewsArticle?> GetArticle(long id)
	{
		try
		{
			var article = await _httpClient.GetFromJsonAsync<NewsArticle>($"v0/item/{id}.json",
				new JsonSerializerOptions(JsonSerializerDefaults.Web));

			if (article is not null && string.IsNullOrWhiteSpace(article?.Url))
			{
				article = new NewsArticle(id, article?.Title ?? string.Empty, $"{DefaultHackerNewsArticleUrl}{id}");
			}

			return article;
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error while getting Hacker News Article {id}: {ex.Message}", ex);
			throw;
		}
	}

	public void Dispose() => _httpClient?.Dispose();
}
