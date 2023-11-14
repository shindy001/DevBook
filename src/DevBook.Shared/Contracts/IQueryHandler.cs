using MediatR;

namespace DevBook.Shared.Contracts;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
	where TQuery : IQuery<TResult>;
