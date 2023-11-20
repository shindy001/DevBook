using DevBook.Grpc;
using DevBook.Server.Common;

namespace DevBook.Server.Features.AppSetups;

internal sealed record AppSetup(
	string Name,
	string Path,
	string? Arguments)
	: Entity(Guid.NewGuid())
{
	public AppSetupDto ToDto()
	{
		return new AppSetupDto
		{
			Id = Id.ToString(),
			Name = Name,
			Path = Path,
			Arguments = Arguments,
		};
	}
}
