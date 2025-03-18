using System.Windows.Input;
using Project_9.Models;
using Project_9.Models.Commands;
using Project_9.Services;
using ReactiveUI;

namespace Project_9.ViewModels;

public class RibPlacementViewModel(Wing wing, UndoRedoService undoRedo) : ViewModelBase
{
	private int _selectedRibIndex;
	
	public RibCollection Ribs => wing.Ribs;
	
	public int SelectedRibIndex {
		get => _selectedRibIndex;
		set => RaiseAndSetIfChanged(ref _selectedRibIndex, value);
	}

	public ICommand AddRibCommand {
		get => ReactiveCommand.Create<int>(index => {
			undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Add, index));
		});
	}

	public ICommand RemoveRibCommand {
		get => ReactiveCommand.Create<int>(index => {
			undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Remove, index));
		});
	}

	public ICommand MoveRibCommand {
		get => ReactiveCommand.Create<(int index, double shift)>(args => {
			undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Move, args.index, args.shift));
		});
	}

	public ICommand ResetRibsCommand {
		get => ReactiveCommand.Create<int>(n => {
			undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Reset, -1, 0.0, n));
		});
	}

	public ICommand ClearRibsCommand {
		get => ReactiveCommand.Create(() => {
			undoRedo.Execute(new RibCommand(Ribs, RibCommand.RibAction.Clear));
		});
	}
}