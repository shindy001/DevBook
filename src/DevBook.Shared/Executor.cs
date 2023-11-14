using DevBook.Shared.Contracts;
using MediatR;

namespace DevBook.Shared;

public sealed class Executor(IMediator _mediator) : IExecutor
{
	public async Task ExecuteCommand(ICommand command)
		=> await _mediator.Send(command);

	public async Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command)
		=> await _mediator.Send(command);

	public async Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
		=> await _mediator.Send(query);
}
