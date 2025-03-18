using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Project_9.Services;

public static class FileService
{
	public static async Task<string?> OpenFilePickerAsync(TopLevel topLevel, FilePickerFileType filter) {
		var files = await topLevel.StorageProvider.OpenFilePickerAsync(
			new FilePickerOpenOptions { FileTypeFilter = [filter] });

		return files.Count > 0 ? files[0].Path.AbsolutePath : null;
	}

	public static async Task<string?> SaveFilePickerAsync(TopLevel topLevel, string title, FilePickerFileType filter) {
		IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(
			new FilePickerSaveOptions {
				Title = title,
				FileTypeChoices = [filter],
				DefaultExtension = filter.Patterns?.FirstOrDefault()?.TrimStart('*'),
				ShowOverwritePrompt = true
			});

		return file?.Path.AbsolutePath;
	}
}