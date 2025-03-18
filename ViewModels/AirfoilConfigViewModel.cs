using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Project_9.Models;
using Project_9.Services;
using ReactiveUI;

namespace Project_9.ViewModels;

public class AirfoilConfigViewModel(Wing wing, UndoRedoService undoRedo, TopLevel topLevel) : ViewModelBase
{
	public Airfoil RootAirfoil => wing.RootAirfoil;
	public Airfoil TipAirfoil => wing.TipAirfoil;
	
	public ICommand ImportRootAirfoilCommand {
		get => ReactiveCommand.CreateFromTask(() => {
			ProcessImport(nameof(Wing.RootAirfoil));
			return Task.CompletedTask;
		});
	}

	public ICommand ImportTipAirfoilCommand {
		get => ReactiveCommand.CreateFromTask(() => {
			ProcessImport(nameof(Wing.TipAirfoil));
			return Task.CompletedTask;
		});
	}

	private async void ProcessImport(string paramName) {
		var file = await FileService.OpenFilePickerAsync(
			topLevel,
			new FilePickerFileType("Airfoil DAT Files") {
				Patterns = ["*.dat"],
				MimeTypes = ["text/plain"]
			});
		if (file is null) {
			throw new FileLoadException("Failed to load the file.");
		}
		Airfoil airfoil = AirfoilParserService.Parse(file);
		undoRedo.Execute(new SetPropertyCommand<Airfoil>(wing, paramName, airfoil));
	}
}