using MediatR;

namespace DevBook.Shared.Contracts;

public interface IQuery<out TResult> : IRequest<TResult>;
