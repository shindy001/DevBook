using DevBook.Grpc.HackerNews;
using DevBook.Server.Features.HackerNews;
using NSubstitute;

namespace DevBook.Server.IntegrationTests.Features.HackerNews;

public class HackerNewsServiceTests : IntegrationTestBase
{
	private readonly IHackerNewsApi _hackerNewsApi = Substitute.For<IHackerNewsApi>();
	private readonly HackerNewsGrpcService.HackerNewsGrpcServiceClient _client;

	public HackerNewsServiceTests(ITestOutputHelper outputHelper) : base(outputHelper)
	{
		this.Fixture.ReplaceService<IHackerNewsApi>(_hackerNewsApi);
		_client = new HackerNewsGrpcService.HackerNewsGrpcServiceClient(Channel);
	}

	[Fact]
	public async Task GetNews_WhenCalled_ReturnsNewsArticles()
	{
		// Arrange
		var article = new NewsArticle(1, "Article1", "https://article1.test.com");
		_hackerNewsApi.GetNews().Returns([article]);

		// Act
		var response = await _client.GetNewsAsync(new GetNewsRequest());

		// Assert
		response.Should().NotBeNull();
		response.Items.First().Title.Should().Be(article.Title);
		response.Items.First().Url.Should().Be(article.Url);
	}
}
