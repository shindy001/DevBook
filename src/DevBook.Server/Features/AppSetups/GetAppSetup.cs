using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.AppSetups;

internal record GetAppSetupQuery(string Id) : IQuery<OneOf<AppSetup, NotFound>>;

internal sealed class GetAppSetupQueryHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetupQuery, OneOf<AppSetup, NotFound>>
{
	public async Task<OneOf<AppSetup, NotFound>> Handle(GetAppSetupQuery request, CancellationToken cancellationToken)
	{
		var appSetup = await _dbContext.AppSetups.FindAsync([Guid.Parse(request.Id)], cancellationToken: cancellationToken);

		return appSetup is null
			? new NotFound()
			: appSetup;
	}
}
