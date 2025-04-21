using System.Collections.Generic;
using Oswalt.Models;
using Oswalt.Services;

namespace Oswalt.Commands;

public class RemoveSparCommand(List<Spar> spars, Spar spar) : IUndoableCommand
{
	public void Execute() => spars.Remove(spar);
	public void Undo() => spars.Add(spar);
}