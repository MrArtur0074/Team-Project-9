using System.Collections.Generic;
using Project_9.Models;
using Project_9.Services;

namespace Project_9.Commands;

public class RemoveSparCommand(List<Spar> spars, Spar spar) : IUndoableCommand
{
	public void Execute() => spars.Remove(spar);
	public void Undo() => spars.Add(spar);
}