using DevBook.Grpc;
using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.AppSetups;

internal class GetAppSetups : IQuery<GetAllResponse>;

internal class GetAppSetupsHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetups, GetAllResponse>
{
	public Task<GetAllResponse> Handle(GetAppSetups request, CancellationToken cancellationToken)
	{
		var appSetups = _dbContext.AppSetups.Select(x => x.ToDto());
		var response = new GetAllResponse();
		response.Items.AddRange(appSetups);

		return Task.FromResult(response);
	}
}
