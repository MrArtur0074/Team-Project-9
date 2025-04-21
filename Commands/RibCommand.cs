using System;
using System.Collections.Generic;
using Oswalt.Constants;
using Oswalt.Services;

namespace Oswalt.Models.Commands;

public class RibCommand(
	RibCollection ribs,
	RibCommand.RibAction action,
	int index = -1,
	double shift = 0,
	int n = 0)
	: IUndoableCommand
{
	public enum RibAction
	{
		Add,
		Remove,
		Move,
		Reset,
		Clear
	}

	private readonly List<double> _previousState = new List<double>(ribs.Ribs);

	public void Execute() {
		switch (action) {
			case RibAction.Add:
				ribs.AddRib(index);
				break;
			case RibAction.Remove:
				ribs.RemoveRib(index);
				break;
			case RibAction.Move:
				ribs.ChangeRibPosition(index, shift);
				break;
			case RibAction.Reset:
				ribs.ResetWithRibsNumber(n);
				break;
			case RibAction.Clear:
				ribs.Clear();
				break;
		}
	}

	public void Undo() {
		ribs.Clear();
		foreach (var position in _previousState) {
			if (position == 0 || Math.Abs(position - 1) < MathConstants.Tolerance) continue;
			ribs.AddRib(ribs.Ribs.Count - 1);
			ribs.ChangeRibPosition(
				ribs.Ribs.Count - 2,
				position - ribs.Ribs[^2]
			);
		}
	}
}