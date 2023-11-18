using DevBook.Shared.Contracts;
using MediatR;

namespace DevBook.Server.Infrastructure;

internal sealed class UnitOfWorkQueryPipelineBehavior<TRequest, TResponse>
	: IQueryPipelineBehavior<TRequest, TResponse>
	where TRequest : IQuery<TResponse>
{
	private readonly IUnitOfWork _unitOfWork;

	public UnitOfWorkQueryPipelineBehavior(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		_unitOfWork.AsNoTrackingQuery();
		return await next();
	}
}
