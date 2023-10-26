namespace MauiBlazorClient.Services.DTO
{
	public sealed record StartupProfileDto
	{
		public required string Id { get; set; }
		public required string Name { get; set; }
		public IEnumerable<string> AppSetupIds { get; set; } = [];
	}
}