using DevBook.Grpc;
using DevBook.Server.Common;

namespace DevBook.Server.Features.AppSetups;

internal sealed record AppSetup : Entity
{
	public required string Name { get; init; }
	public required string Path { get; init; }
	public string? Arguments { get; init; }

	public AppSetup(string name, string path, string? arguments = null)
		:base(Guid.NewGuid())
	{
		this.Name = name;
		this.Path = path;
		this.Arguments = arguments;
	}

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
