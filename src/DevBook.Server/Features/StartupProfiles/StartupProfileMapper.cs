using DevBook.Grpc.StartupProfiles;

namespace DevBook.Server.Features.StartupProfiles;

internal static class StartupProfileMapper
{
	public static StartupProfileDto ToDto(StartupProfile startupProfile)
	{
		var dto = new StartupProfileDto
		{
			Id = startupProfile.Id.ToString(),
			Name = startupProfile.Name
		};
		dto.AppSetupIds.AddRange(startupProfile.AppSetupIds.Select(x => x.ToString()));

		return dto;
	}
}
