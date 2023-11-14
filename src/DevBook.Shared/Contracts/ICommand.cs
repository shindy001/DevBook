using MediatR;

namespace DevBook.Shared.Contracts;

public interface ICommand : IRequest;

public interface ICommand<out TResult> : IRequest<TResult>;
