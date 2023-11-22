using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Features.AppSetups;

internal class GetAppSetups : IQuery<IEnumerable<AppSetup>>;

internal class GetAppSetupsHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetups, IEnumerable<AppSetup>>
{
	public async Task<IEnumerable<AppSetup>> Handle(GetAppSetups request, CancellationToken cancellationToken)
	{
		return await _dbContext.AppSetups.ToListAsync();
	}
}
