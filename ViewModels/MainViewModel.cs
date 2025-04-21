using System;
using System.Collections.Generic;
using System.Windows.Input;
using Oswalt.Models;
using Oswalt.Services;
using ReactiveUI;
using System.Windows;


namespace Project9.ViewModels;

public class MainViewModel : ViewModelBase
{
	private          ViewModelBase             _currentViewModel;
	private          int                       _currentStepIndex;
	private readonly List<Func<ViewModelBase>> _steps;
	private readonly Wing                      _wing            = new StraightWing("New Wing", 200, 1000, 0.0);
	private readonly UndoRedoService           _undoRedoService = new();

    public MainViewModel(IDwgExportService dwgExportService)
    {
        _steps = [
            () => new WingGeometryViewModel(_wing, _undoRedoService),
        () => new AirfoilConfigViewModel(_wing, _undoRedoService),
        () => new RibPlacementViewModel(_wing, _undoRedoService),
        () => new SparConfigViewModel(_wing, _undoRedoService),
        () => new DwgExportViewModel(_wing, dwgExportService)
        ];

        CurrentViewModel = _steps[0]();
    }


    public ViewModelBase CurrentViewModel {
		get => _currentViewModel;
		private set => RaiseAndSetIfChanged(ref _currentViewModel, value);
	}

	public ICommand NavigateNext {
		get => ReactiveCommand.Create(() =>
		{
            Application.Current.Dispatcher.Invoke(() =>
            {
				if (_currentStepIndex < _steps.Count - 1) {
					_currentStepIndex++;
					CurrentViewModel = _steps[_currentStepIndex]();
				}
			});
		}, outputScheduler: RxApp.MainThreadScheduler);
	}

	public ICommand NavigatePrevious {
		get => ReactiveCommand.Create(() =>
		{
            Application.Current.Dispatcher.Invoke(() =>
            {
				if (_currentStepIndex > 0) {
					_currentStepIndex--;
					CurrentViewModel = _steps[_currentStepIndex]();
				}
			});
		}, outputScheduler: RxApp.MainThreadScheduler);
	}

    public ICommand UndoCommand
    {
        get => ReactiveCommand.Create(
            _undoRedoService.Undo,
            outputScheduler: RxApp.MainThreadScheduler
        );
    }


    public ICommand RedoCommand
    {
        get => ReactiveCommand.Create(
            _undoRedoService.Redo,
            outputScheduler: RxApp.MainThreadScheduler
        );
    }

}