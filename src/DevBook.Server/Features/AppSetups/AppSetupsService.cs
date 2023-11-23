using DevBook.Grpc.AppSetups;
using DevBook.Server.Common;
using DevBook.Shared.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.Features.AppSetups;

internal sealed class AppSetupsService(IExecutor _executor) : AppSetupsGrpcService.AppSetupsGrpcServiceBase
{
	public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
	{
		var appSetups = await _executor.ExecuteQuery(new GetAppSetupsQuery());
		var response = new GetAllResponse();
		response.Items.Add(appSetups.Select(AppSetupMapper.ToDto));
		return response;
	}

	public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteQuery(new GetAppSetupQuery(request.Id));
		return result.Match(
			appSetup => new GetByIdResponse { Item = AppSetupMapper.ToDto(appSetup) },
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Create(CreateRequest request, ServerCallContext context)
	{
		await _executor.ExecuteCommand(new CreateAppSetupCommand(request.Name, request.Path, request.Arguments));
		return new Empty();
	}

	public override async Task<Empty> Update(UpdateRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteCommand(new UpdateAppSetupCommand(request.Id, request.Name, request.Path, request.Arguments));
		return result.Match(
			success => new Empty(),
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Patch(PatchRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteCommand(new PatchAppSetupCommand(request.Id, request.Name, request.Path, request.Arguments));
		return result.Match(
			success => new Empty(),
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
	{
		await _executor.ExecuteCommand(new DeleteAppSetupCommand(request.Id));
		return new Empty();
	}
}
