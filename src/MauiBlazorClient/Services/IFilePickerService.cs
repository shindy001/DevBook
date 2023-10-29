namespace MauiBlazorClient.Services;

public interface IFilePickerService
{
	public Task<(bool success, string path)> PickOneAsync(params string[] allowedFileExtensions);
}
