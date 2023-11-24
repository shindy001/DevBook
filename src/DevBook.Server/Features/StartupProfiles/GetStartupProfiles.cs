using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Features.StartupProfiles;

internal sealed record GetStartupProfilesQuery : IQuery<IEnumerable<StartupProfile>>;

internal sealed class GetStartupProfilesQueryHandler(DevBookDbContext _dbContext) : IQueryHandler<GetStartupProfilesQuery, IEnumerable<StartupProfile>>
{
	public async Task<IEnumerable<StartupProfile>> Handle(GetStartupProfilesQuery request, CancellationToken cancellationToken)
		=> await _dbContext.StartupProfiles.ToListAsync(cancellationToken: cancellationToken);
}
