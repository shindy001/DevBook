using DevBook.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DevBook.Server.Infrastructure;

internal sealed class UnitOfWork(DevBookDbContext _devBookDbContext) : IUnitOfWork
{
	public void AsNoTrackingQuery()
	{
		_devBookDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
	}

	public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		=> _devBookDbContext.ChangeTracker.QueryTrackingBehavior is QueryTrackingBehavior.NoTracking
			? throw new InvalidOperationException("Cannot call CommitAsync when NoTracking EFCore behavior is set")
			: await _devBookDbContext.SaveChangesAsync(cancellationToken);
}
