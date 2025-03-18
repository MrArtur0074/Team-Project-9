using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using Project_9.Models;
using Project_9.Services;
using ReactiveUI;

namespace Project_9.ViewModels;

public class MainViewModel : ViewModelBase
{
	private          ViewModelBase             _currentViewModel;
	private          int                       _currentStepIndex;
	private readonly List<Func<ViewModelBase>> _steps;
	private readonly Wing                      _wing            = new StraightWing("New Wing", 200, 1000, 0.0);
	private readonly UndoRedoService           _undoRedoService = new();
	private readonly TopLevel                  _topLevel;
	private readonly IDwgExportService         _dwgExportService;

	public MainViewModel(TopLevel topLevel, IDwgExportService dwgExportService) {
		_topLevel = topLevel;
		_dwgExportService = dwgExportService;

		_steps = [
			() => new WingGeometryViewModel(_wing, _undoRedoService),
			() => new AirfoilConfigViewModel(_wing, _undoRedoService, _topLevel),
			() => new RibPlacementViewModel(_wing, _undoRedoService),
			() => new SparConfigViewModel(_wing, _undoRedoService),
			() => new DwgExportViewModel(_wing, dwgExportService, _topLevel)
		];

		CurrentViewModel = _steps[0]();
	}

	public ViewModelBase CurrentViewModel {
		get => _currentViewModel;
		private set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
	}

	public void NavigateNext() {
		if (_currentStepIndex < _steps.Count - 1) {
			_currentStepIndex++;
			CurrentViewModel = _steps[_currentStepIndex]();
		}
	}

	public void NavigatePrevious() {
		if (_currentStepIndex > 0) {
			_currentStepIndex--;
			CurrentViewModel = _steps[_currentStepIndex]();
		}
	}

	public ICommand UndoCommand => ReactiveCommand.Create(_undoRedoService.Undo);
	public ICommand RedoCommand => ReactiveCommand.Create(_undoRedoService.Redo);
}