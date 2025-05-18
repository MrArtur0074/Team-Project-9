namespace Coswalt.Services;

public interface IUndoableCommand
{
	void Execute();
	void Undo();
}