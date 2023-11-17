﻿namespace DevBook.Shared.Contracts;

public interface IUnitOfWork
{
	Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
