namespace Oswalt.Services;

public interface IUndoableCommand
{
	void Execute();
	void Undo();
}