using System.Collections.Generic;
using Oswalt.Services;

namespace Oswalt.Models.Commands;

public class AddSparCommand(List<Spar> spars, Spar spar) : IUndoableCommand
{
	public void Execute() => spars.Add(spar);
	public void Undo() => spars.Remove(spar);
}