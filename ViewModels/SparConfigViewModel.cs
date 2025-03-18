using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using Project_9.Commands;
using Project_9.Models;
using Project_9.Models.Commands;
using Project_9.Services;
using ReactiveUI;

namespace Project_9.ViewModels;

public class SparConfigViewModel(Wing wing, UndoRedoService undoRedo) : ViewModelBase
{
	private Spar _selectedSpar;

	public List<Spar> Spars => wing.Spars;

	public Spar SelectedSpar {
		get => _selectedSpar;
		set => RaiseAndSetIfChanged(ref _selectedSpar, value);
	}

	public ICommand AddRectSparCommand {
		get => ReactiveCommand.Create(() =>
		{
			var ribCount = wing.Ribs.Ribs.Count;
			var rectangle = new Rect(new Point(0.0, 0.0), new Size(0.1, 0.1));

			var newSpar = new RectSpar(
				ribCount: ribCount,
				startRib: 0,
				endRib: ribCount - 1,
				startChordOffset: 0.25,
				endChordOffset: 0.25,
				alignment: Spar.AlignmentType.Linear,
				rectangle: rectangle
			);

			undoRedo.Execute(new AddSparCommand(wing.Spars, newSpar));
		});
	}

	public ICommand AddCircleSparCommand {
		get => ReactiveCommand.Create(() =>
		{
			var ribCount = wing.Ribs.Ribs.Count;
			var newSpar = new CircleSpar(
				ribCount: ribCount,
				startRib: 0,
				endRib: ribCount - 1,
				startChordOffset: 0.25,
				endChordOffset: 0.25,
				alignment: Spar.AlignmentType.Linear,
				center: new Point(),
				radius: 0.1
			);

			undoRedo.Execute(new AddSparCommand(wing.Spars, newSpar));
		});
	}

	public ICommand RemoveSparCommand {
		get => ReactiveCommand.Create<Spar>(spar => { undoRedo.Execute(new RemoveSparCommand(wing.Spars, spar)); });
	}
}