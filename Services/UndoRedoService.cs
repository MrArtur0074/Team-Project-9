using System.Collections.Generic;

namespace Project_9.Services;

public class UndoRedoService
{
	private readonly Stack<IUndoableCommand> _undoStack = new();
	private readonly Stack<IUndoableCommand> _redoStack = new();

	public void Execute(IUndoableCommand command) {
		command.Execute();
		_undoStack.Push(command);
		_redoStack.Clear();
	}

	public void Undo() {
		if (_undoStack.Count > 0) {
			var command = _undoStack.Pop();
			command.Undo();
			_redoStack.Push(command);
		}
	}

	public void Redo() {
		if (_redoStack.Count > 0) {
			var command = _redoStack.Pop();
			command.Execute();
			_undoStack.Push(command);
		}
	}

	public bool CanUndo => _undoStack.Count > 0;
	public bool CanRedo => _redoStack.Count > 0;
}