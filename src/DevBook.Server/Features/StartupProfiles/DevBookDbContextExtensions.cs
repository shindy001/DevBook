using DevBook.Server.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Features.StartupProfiles;

public static class DevBookDbContextExtensions
{
	public static async Task<Guid[]> GetExistingAppSetupGuids(this DevBookDbContext _dbContext, IEnumerable<Guid> guids, CancellationToken cancellationToken)
	{
		return await _dbContext.AppSetups
			.Select(x => x.Id)
			.Where(id => guids.Contains(id))
			.ToArrayAsync(cancellationToken: cancellationToken);
	}
}
