namespace MauiBlazorClient.Services.DTO
{
	public record AppSetupDto
	{
		public required string Id { get; set; }
		public required string Name { get; set; }
		public required string Path { get; set; }
		public string? Arguments { get; set; }
	}
}