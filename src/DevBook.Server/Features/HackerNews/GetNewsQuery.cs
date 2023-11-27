using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.HackerNews;

internal sealed record GetNewsQuery : IQuery<IEnumerable<NewsArticle>>;

internal sealed class GetNewsQueryHandler(IHackerNewsApi _hackerNewsApi) : IQueryHandler<GetNewsQuery, IEnumerable<NewsArticle>>
{
	public async Task<IEnumerable<NewsArticle>> Handle(GetNewsQuery request, CancellationToken cancellationToken)
	{
		return await _hackerNewsApi.GetNews();
	}
}
