using System.ComponentModel.DataAnnotations;

namespace DevBook.Server.Configuration;

internal sealed class Settings
{
	public const string SectionName = "Settings";

	[Required(AllowEmptyStrings = false)]
	public required string HackerNewsBaseAddress { get; init; }
}
