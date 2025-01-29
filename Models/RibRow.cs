using System;
using System.Collections.Generic;
using System.Linq;

namespace Project_9.Models;

/// <summary>
/// Represents a collection of ribs in a wing.
/// </summary>
public class RibRow
{
	private readonly int            _span;
	private readonly SortedSet<int> _ribs = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="RibRow"/> class with a specified span.
	/// </summary>
	/// <param name="span">The span of the rib row.</param>
	/// <exception cref="ArgumentException">Thrown when the span is less than or equal to 0.</exception>
	public RibRow(int span) {
		if (span < 0) {
			throw new ArgumentException("_span must be grater than 0.");
		}
		_span = span;
		_ribs.Add(0);
		_ribs.Add(_span);
	}

	/// <summary>
	/// Adds a rib at the specified position in the rib row.
	/// </summary>
	/// <param name="position">The position to add the rib. It must be between 0 and the span.</param>
	/// <returns><c>true</c> if the rib was added successfully; otherwise, <c>false</c>.</returns>
	public bool AddRib(int position) {
		if (position < 0 || position > _span) {
			return false;
		}
		return _ribs.Add(position);
	}

	/// <summary>
	/// Removes a rib from the specified position.
	/// </summary>
	/// <param name="position">The position of the rib to remove.</param>
	/// <returns><c>true</c> if the rib was removed successfully; otherwise, <c>false</c>.</returns>
	public bool RemoveRib(int position) {
		return _ribs.Remove(position);
	}

	/// <summary>
	/// Gets the positions of all ribs in the rib row.
	/// </summary>
	/// <returns>An array of integers representing the positions of the ribs.</returns>
	public int[] GetRibs() {
		return _ribs.ToArray();
	}

	/// <summary>
	/// Clears the rib row, resetting the ribs to only the first and last positions.
	/// </summary>
	public void Clear() {
		_ribs.Clear();
		_ribs.Add(0);
		_ribs.Add(_span);
	}
}