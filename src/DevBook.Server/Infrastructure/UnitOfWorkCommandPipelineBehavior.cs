using DevBook.Shared.Contracts;
using MediatR;

namespace DevBook.Server.Infrastructure;

internal sealed class UnitOfWorkCommandPipelineBehavior<TRequest, TResponse>
	: ICommandPipelineBehavior<TRequest, TResponse>
	where TRequest : ICommandBase
{
	private readonly IUnitOfWork _unitOfWork;

	public UnitOfWorkCommandPipelineBehavior(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var response = await next();
		await _unitOfWork.CommitAsync(cancellationToken: cancellationToken);
		return response;
	}
}
