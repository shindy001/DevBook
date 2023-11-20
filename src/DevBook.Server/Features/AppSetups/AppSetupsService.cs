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

	public override async Task<Empty> Update(UpdateRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteCommand(new UpdateAppSetup(request.Item.Id, request.Item.Name, request.Item.Path, request.Item.Arguments));
		return result.Match(
			success => new Empty(),
			_ => throw new RpcException(new Status(StatusCode.NotFound, $"Item with id '{request.Item.Id}' not found.")));
	}

	public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
	{
		return base.Delete(request, context);
	}
}
