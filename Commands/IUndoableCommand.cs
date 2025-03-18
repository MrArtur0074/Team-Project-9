namespace Project_9.Services;

public interface IUndoableCommand
{
	void Execute();
	void Undo();
}