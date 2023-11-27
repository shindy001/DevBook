using DevBook.Grpc.HackerNews;
using DevBook.Server.Features.AppSetups;
using DevBook.Shared.Contracts;
using Grpc.Core;

namespace DevBook.Server.Features.HackerNews;

internal sealed class HackerNewsService(IExecutor _executor) : HackerNewsGrpcService.HackerNewsGrpcServiceBase
{
	public override async Task<GetNewsResponse> GetNews(GetNewsRequest request, ServerCallContext context)
	{
		var news = await _executor.ExecuteQuery(new GetNewsQuery());
		var response = new GetNewsResponse();
		response.Items.Add(news.Select(NewsMapper.ToDto));
		return response;
	}
}
