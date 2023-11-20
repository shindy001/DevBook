using DevBook.Grpc;
using DevBook.Shared.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.Features.AppSetups;

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

	public override async Task<Empty> Create(CreateRequest request, ServerCallContext context)
	{
		await _executor.ExecuteCommand(new CreateAppSetup(request.Name, request.Path, request.Arguments));
		return new Empty();
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
