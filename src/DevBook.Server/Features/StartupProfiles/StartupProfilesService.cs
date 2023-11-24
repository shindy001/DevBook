using DevBook.Grpc.StartupProfiles;
using DevBook.Server.Common;
using DevBook.Shared.Contracts;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.Features.StartupProfiles;

internal sealed class StartupProfilesService(IExecutor _executor) : StartupProfilesGrpcService.StartupProfilesGrpcServiceBase
{
	public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
	{
		var startupProfiles = await _executor.ExecuteQuery(new GetStartupProfilesQuery());
		var response = new GetAllResponse();
		response.Items.Add(startupProfiles.Select(StartupProfileMapper.ToDto));
		return response;
	}

	public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteQuery(new GetStartupProfileQuery(request.Id));
		return result.Match(
			startupProfile => new GetByIdResponse { Item = StartupProfileMapper.ToDto(startupProfile) },
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Create(CreateRequest request, ServerCallContext context)
	{
		await _executor.ExecuteCommand(new CreateStartupProfileCommand(request.Name, [.. request.AppSetupIds]));
		return new Empty();
	}

	public override async Task<Empty> Update(UpdateRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteCommand(new UpdateStartupProfileCommand(request.Id, request.Name, [.. request.AppSetupIds]));
		return result.Match(
			success => new Empty(),
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Patch(PatchRequest request, ServerCallContext context)
	{
		var result = await _executor.ExecuteCommand(new PatchStartupProfileCommand(request.Id, request.Name, [.. request.AppSetupIds]));
		return result.Match(
			success => new Empty(),
			_ => throw RpcExceptions.NotFound(request.Id));
	}

	public override async Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
	{
		await _executor.ExecuteCommand(new DeleteStartupProfileCommand(request.Id));
		return new Empty();
	}
}
