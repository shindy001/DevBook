using DevBook.Shared;

namespace DevBook.Server.Features.StartupProfiles;

internal sealed record StartupProfile(
	string Name,
	Guid[] AppSetupIds)
	: Entity(Guid.NewGuid());
