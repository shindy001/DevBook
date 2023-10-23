namespace MauiBlazorClient.Services
{
	public sealed class FilePickerService : IFilePickerService
	{
		public async Task<(bool success, string path)> PickOneAsync(params string[] allowedFileExtensions)
		{
			var fileTypes = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.WinUI, allowedFileExtensions }
				});

			var result = await FilePicker.Default.PickAsync(new() { FileTypes = fileTypes });
			return result is null
				? (false, string.Empty)
				: (true, result.FullPath);
		}
	}
}
