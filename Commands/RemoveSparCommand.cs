using System.Collections.Generic;
using Coswalt.Models;
using Coswalt.Services;

namespace Coswalt.Commands;

public class RemoveSparCommand(List<Spar> spars, Spar spar) : IUndoableCommand
{
	public void Execute() => spars.Remove(spar);
	public void Undo() => spars.Add(spar);
}