using DevBook.Grpc;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.Profiles;

internal class GetAppSetups : IQuery<GetAllResponse>;

internal class GetAppSetupHandler : IQueryHandler<GetAppSetups, GetAllResponse>
{
	public async Task<GetAllResponse> Handle(GetAppSetups request, CancellationToken cancellationToken)
	{
		var response = new GetAllResponse();
		response.Items.AddRange([new AppSetupDto { Id = Guid.NewGuid().ToString(), Name = "Karel", Path = "Path", Arguments = "Args" }]);
		return await Task.FromResult(response);
	}
}
