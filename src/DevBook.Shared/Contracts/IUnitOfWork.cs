namespace DevBook.Shared.Contracts;

public interface IUnitOfWork
{
	void AsNoTrackingQuery();
	Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
