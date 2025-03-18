using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Project_9.Models;
using Project_9.Services;
using ReactiveUI;

namespace Project_9.ViewModels;

public class DwgExportViewModel(Wing wing, IDwgExportService dwgExportService, TopLevel topLevel) : ViewModelBase
{
	private readonly IDwgExportService _dwgExportService = dwgExportService;

	public ICommand ExportCommand {
		get => ReactiveCommand.CreateFromTask(async () => {
			var filePath = await FileService.SaveFilePickerAsync(
				topLevel,
				"Export DWG",
				new FilePickerFileType("AutoCAD DWG Files") {
					Patterns = ["*.dwg"],
					MimeTypes = ["application/acad"]
				});

			if (filePath != null) {
				await _dwgExportService.ExportAsync(wing, filePath);
			}
		});
	}
}