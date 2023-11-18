using DevBook.Grpc;
using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;

namespace DevBook.Server.Features.Profiles;

internal record GetAppSetup(string Id) : IQuery<GetByIdResponse>;

internal class GetAppSetupHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetup, GetByIdResponse>
{
	public async Task<GetByIdResponse> Handle(GetAppSetup request, CancellationToken cancellationToken)
	{
		var appSetup = await _dbContext.AppSetups.FindAsync(Guid.Parse(request.Id));

		return appSetup is null
			? new GetByIdResponse { Message = $"AppSetup with Id '{request.Id}' not found." }
			: new GetByIdResponse { AppSetup = appSetup?.ToDto() };
	}
}
