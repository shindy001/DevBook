using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using OneOf;
using OneOf.Types;

namespace DevBook.Server.Features.StartupProfiles;

internal record GetStartupProfileQuery(string Id) : IQuery<OneOf<StartupProfile, NotFound>>;

internal sealed class GetStartupProfileQueryHandler(DevBookDbContext _dbContext) : IQueryHandler<GetStartupProfileQuery, OneOf<StartupProfile, NotFound>>
{
	public async Task<OneOf<StartupProfile, NotFound>> Handle(GetStartupProfileQuery request, CancellationToken cancellationToken)
	{
		StartupProfile? startupProfile = null;
		if (Guid.TryParse(request.Id, out var guid))
		{
			startupProfile = await _dbContext.StartupProfiles.FindAsync([guid], cancellationToken: cancellationToken);
		}

		return startupProfile is null
			? new NotFound()
			: startupProfile;
	}
}
