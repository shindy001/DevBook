using Grpc.Core;
using Grpc.Core.Interceptors;

namespace DevBook.Server.Common;

internal sealed class GrpgGlobalExceptionInterceptor(ILogger<GrpgGlobalExceptionInterceptor> _logger, IWebHostEnvironment _environment) : Interceptor
{
	public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
		ServerCallContext context,
		UnaryServerMethod<TRequest, TResponse> continuation)
	{
		try
		{
			return await base.UnaryServerHandler(request, context, continuation);
		}
		catch(CommandValidationException e)
		{
			throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message), CreateTrailers(e.Errors));
		}
		catch (Exception e) when (e is not RpcException)
		{
			_logger.LogError(e, "Internal server error");

			if (_environment.IsDevelopment())
			{
				throw new RpcException(new Status(StatusCode.Internal, $"Internal server error. ErrorMessage: {e.Message}"));
			}
			else
			{
				throw new RpcException(new Status(StatusCode.Internal, $"Internal server error. Please try again later."));
			}
		}

	}

	private static Metadata CreateTrailers(IDictionary<string, string> errors)
	{
		var trailers = new Metadata();
		foreach(var error in errors)
		{
			trailers.Add(error.Key, error.Value);
		}
		return trailers;
	}

}
