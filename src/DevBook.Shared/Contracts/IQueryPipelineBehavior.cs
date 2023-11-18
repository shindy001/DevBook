using MediatR;

namespace DevBook.Shared.Contracts;

public interface IQueryPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest: IQuery<TResponse>
{
}
