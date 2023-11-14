namespace DevBook.Shared.Contracts;

public interface IExecutor
{
	Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query);

	Task ExecuteCommand(ICommand command);

	Task<TResult> ExecuteCommand<TResult>(ICommand<TResult> command);
}
