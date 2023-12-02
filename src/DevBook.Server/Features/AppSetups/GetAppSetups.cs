using DevBook.Server.Infrastructure;
using DevBook.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Features.AppSetups;

internal sealed record GetAppSetupsQuery : IQuery<IEnumerable<AppSetup>>;

internal sealed class GetAppSetupsQueryHandler(DevBookDbContext _dbContext) : IQueryHandler<GetAppSetupsQuery, IEnumerable<AppSetup>>
{
	public async Task<IEnumerable<AppSetup>> Handle(GetAppSetupsQuery request, CancellationToken cancellationToken)
	{
		return await _dbContext.AppSetups.ToListAsync(cancellationToken: cancellationToken);
	}
}
