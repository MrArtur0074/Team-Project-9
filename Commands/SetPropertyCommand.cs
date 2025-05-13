using Coswalt.Services;

namespace Coswalt.Models;

public class SetPropertyCommand<T>(object target, string propertyName, T newValue) : IUndoableCommand
{
	private T? _oldValue;

	public void Execute() {
		var prop = target.GetType().GetProperty(propertyName);
		_oldValue = (T)prop?.GetValue(target)!;
		prop?.SetValue(target, newValue);
	}

	public void Undo() {
		var prop = target.GetType().GetProperty(propertyName);
		prop?.SetValue(target, _oldValue);
	}
}