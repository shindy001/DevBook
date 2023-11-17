using MediatR;

namespace DevBook.Shared.Contracts;

public interface ICommandPipelineBehavior<TRequest, TResponse>
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommandBase
{ }
