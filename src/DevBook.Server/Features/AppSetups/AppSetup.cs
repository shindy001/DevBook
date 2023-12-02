using DevBook.Shared;

namespace DevBook.Server.Features.AppSetups;

public sealed record AppSetup(
	string Name,
	string Path,
	string? Arguments)
	: Entity(Guid.NewGuid());
