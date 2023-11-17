using DevBook.Shared.Contracts;

namespace DevBook.Server.Infrastructure;

internal sealed class UnitOfWork(DevBookDbContext _devBookDbContext) : IUnitOfWork
{
	public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		=> await _devBookDbContext.SaveChangesAsync(cancellationToken);
}
