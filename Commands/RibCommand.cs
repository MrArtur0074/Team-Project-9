using Coswalt.Services;

namespace Coswalt.Models.Commands;

public class RibCommand(
	RibCollection ribs,
	RibAction action,
	int index = -1,
	double shift = 0,
	int n = 0)
	: IUndoableCommand
{
	private readonly RibCollection _backup = new();
	private          bool          _executed;

	public void Execute() {
		_backup.Clear();
		foreach (var rib in ribs)
			_backup.AddRib(_backup.Count - 1);
		for (int i = 1; i < ribs.Count - 1; i++)
			_backup.ChangeRibPosition(i, ribs[i] - _backup[i]);

		switch (action) {
			case RibAction.Add:
				_executed = ribs.AddRib(index);
				break;
			case RibAction.Remove:
				_executed = ribs.RemoveRib(index);
				break;
			case RibAction.Move:
				_executed = ribs.ChangeRibPosition(index, shift);
				break;
			case RibAction.Reset:
				_executed = ribs.ResetWithRibsNumber(n);
				break;
			case RibAction.Clear:
				ribs.Clear();
				_executed = true;
				break;
			default:
				_executed = false;
				break;
		}
	}


	public void Undo() {
		if (!_executed) return;

		ribs.Clear();
		for (var i = 0; i < _backup.Count; i++) {
			ribs.AddRib(ribs.Count - 1);
		}

		for (int i = 1; i < _backup.Count - 1; i++) {
			double delta = _backup[i] - ribs[i];
			ribs.ChangeRibPosition(i, delta);
		}
	}
}

public enum RibAction
{
	Add,
	Remove,
	Move,
	Reset,
	Clear
}