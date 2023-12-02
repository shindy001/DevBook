using DevBook.Shared;

namespace DevBook.Server.Features.StartupProfiles;

public sealed record StartupProfile(
	string Name,
	Guid[] AppSetupIds)
	: Entity(Guid.NewGuid());
