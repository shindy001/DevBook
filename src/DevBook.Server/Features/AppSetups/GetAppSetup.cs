using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

internal record GetAppSetup(string Id) : IQuery<OneOf<AppSetup, NotFound>>;

internal class GetAppSetupHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetup, OneOf<AppSetup, NotFound>>
{
	public async Task<OneOf<AppSetup, NotFound>> Handle(GetAppSetup request, CancellationToken cancellationToken)
	{
		var appSetup = await _dbContext.AppSetups.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);

		return appSetup is null
			? new NotFound()
			: appSetup;
	}
}
