using DevBook.Grpc;
using DevBook.Shared.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.Features.Profiles;

internal sealed class AppSetupsService(IExecutor _executor) : AppSetupsGrpcService.AppSetupsGrpcServiceBase
{
	public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
	{
		return await _executor.ExecuteQuery(new GetAppSetups());
	}

	public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
	{
		var response = await _executor.ExecuteQuery(new GetAppSetup(request.Id));
		return response.Match(
			response => response,
			_ => throw new RpcException(new Status(StatusCode.NotFound, $"Item with id '{request.Id}' not found.")));
	}

	public override Task<Empty> Create(CreateRequest request, ServerCallContext context)
	{
		throw new Exception("karel");
		return base.Create(request, context);
	}

	public override Task<Empty> Update(UpdateRequest request, ServerCallContext context)
	{
		return base.Update(request, context);
	}

	public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
	{
		return base.Delete(request, context);
	}
}
