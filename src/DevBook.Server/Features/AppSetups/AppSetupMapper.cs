using DevBook.Grpc.AppSetups;

namespace DevBook.Server.Features.AppSetups;

internal static class AppSetupMapper
{
	public static AppSetupDto ToDto(AppSetup appSetup)
	{
		return new AppSetupDto
		{
			Id = appSetup.Id.ToString(),
			Name = appSetup.Name,
			Path = appSetup.Path,
			Arguments = appSetup.Arguments,
		};
	}
}
