using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_9.Models;

public class RibRow
{
	private readonly int            _span;
	private readonly SortedSet<int> _ribs = new();
	
	public RibRow(int span) {
		if (span < 0) {
			throw new ArgumentException("_span must be grater than 0.");
		}
		_span = span;
		_ribs.Add(0);
		_ribs.Add(_span);
	}

	public bool AddRib(int position) {
		if (position < 0 || position > _span) {
			return false;
		}
		return _ribs.Add(position);
	}

	public bool RemoveRib(int position) {
		return _ribs.Remove(position);
	}

	public int[] GetRibs() {
		return _ribs.ToArray();
	}

	public void Clear() {
		_ribs.Clear();
		_ribs.Add(0);
		_ribs.Add(_span);
	}
}