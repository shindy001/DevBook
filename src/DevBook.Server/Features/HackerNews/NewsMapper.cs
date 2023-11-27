using DevBook.Grpc.HackerNews;
using DevBook.Server.Features.HackerNews;

namespace DevBook.Server.Features.AppSetups;

internal static class NewsMapper
{
	public static NewsArticleDto ToDto(NewsArticle newsArticle)
	{
		return new NewsArticleDto
		{
			Title = newsArticle.Title,
			Url = newsArticle.Url
		};
	}
}
