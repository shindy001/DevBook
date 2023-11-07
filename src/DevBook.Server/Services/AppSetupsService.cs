using DevBook.Grpc;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace DevBook.Server.Services;

public class AppSetupsService : AppSetupsGrpcService.AppSetupsGrpcServiceBase
{
	private readonly ILogger<AppSetupsService> _logger;
	public AppSetupsService(ILogger<AppSetupsService> logger)
	{
		_logger = logger;
	}

	public override Task<AppSetups> GetAll(GetAllRequest request, ServerCallContext context)
	{
		return base.GetAll(request, context);
	}

	public override Task<AppSetup> GetById(GetByIdRequest request, ServerCallContext context)
	{
		return base.GetById(request, context);
	}

	public override Task<Empty> Create(CreateRequest request, ServerCallContext context)
	{
		return base.Create(request, context);
	}

	public override Task<Empty> Update(UpdateReuqest request, ServerCallContext context)
	{
		return base.Update(request, context);
	}

	public override Task<Empty> Delete(DeleteRequest request, ServerCallContext context)
	{
		return base.Delete(request, context);
	}
}
