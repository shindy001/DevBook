﻿using DevBook.Shared.Contracts;
using MauiBlazorClient.Services.Contracts;
using Microsoft.AspNetCore.Components;

namespace MauiBlazorClient.Shared;

public partial class NewsOverlayDrawerWidget
{
	[Inject] private IExecutor Executor { get; init; } = default!;

	private Model _model = new();
	private bool _loading = false;

	protected override async Task OnInitializedAsync()
	{
		_loading = true;
		_model = await Executor.ExecuteQuery(new GetModelQuery());
		_loading = false;
	}

	public record Model
	{
		public List<NewsArticle> NewsArticles { get; init; } = [];

		public record NewsArticle(string Title, string Url);
	}

	public record GetModelQuery : IQuery<Model>;

	public class GetModelQueryHandler(IHackerNewsService _hackerNewsService)
		: IQueryHandler<GetModelQuery, Model>
	{
		public async Task<Model> Handle(GetModelQuery request, CancellationToken cancellationToken)
		{
			var newsArticlesDtos = await _hackerNewsService.GetNews();
			return new Model { NewsArticles = newsArticlesDtos?.Select(x => new Model.NewsArticle(x.Title, x.Url)).ToList() ?? [] };
		}
	}
}
