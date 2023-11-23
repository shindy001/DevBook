using Grpc.Core;

namespace DevBook.Server.Common;

internal static class RpcExceptions
{
	public static RpcException NotFound(string id) => new RpcException(new Status(StatusCode.NotFound, $"Item with id '{id}' not found."));
}
