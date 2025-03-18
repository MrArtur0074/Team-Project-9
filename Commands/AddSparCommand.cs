using System.Collections.Generic;
using Project_9.Services;

namespace Project_9.Models.Commands;

public class AddSparCommand(List<Spar> spars, Spar spar) : IUndoableCommand
{
	public void Execute() => spars.Add(spar);
	public void Undo() => spars.Remove(spar);
}