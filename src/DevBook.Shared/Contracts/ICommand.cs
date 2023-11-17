using MediatR;

namespace DevBook.Shared.Contracts;

/// <summary>
/// Marker interface
/// </summary>
public interface ICommandBase;

public interface ICommand : ICommandBase, IRequest;

public interface ICommand<out TResult> : ICommandBase, IRequest<TResult>;
