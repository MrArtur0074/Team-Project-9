using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Project_9.Services;
using Project_9.ViewModels;
using Project_9.Views;
using Splat;

namespace Project_9;

public class App : Application
{
	public override void Initialize() {
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted() {
		Locator.CurrentMutable.RegisterLazySingleton(() => new UndoRedoService());
		Locator.CurrentMutable.RegisterLazySingleton(() => new DwgExportService());
		Locator.CurrentMutable.RegisterLazySingleton(() => new RibsInterpolationService());

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			// var undoRedoService = Locator.Current.GetService<UndoRedoService>();
			var dwgExportService = Locator.Current.GetService<DwgExportService>();

			desktop.MainWindow = new MainWindow {
				DataContext = new MainViewModel(desktop.MainWindow, dwgExportService)
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}