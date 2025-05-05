using System.Collections;
using System.Collections.Generic;
using Oswalt.Constants;

namespace Oswalt.Models;

/// <summary>
/// Represents a collection of ribs using normalized positions [0.0, 1.0].
/// Includes permanent root and tip ribs.
/// </summary>
public class RibCollection : IEnumerable<double>
{
	private readonly List<double> _ribs = new();

	public double this[int index] => _ribs[index];

	public int Count => _ribs.Count;

	/// <summary>
	/// Initializes a new instance of the <see cref="RibCollection"/> class.
	/// </summary>
	public RibCollection() {
		_ribs.Add(0.0);
		_ribs.Add(1.0);
	}

	/// <summary>
	/// Inserts a rib with the specified index.
	/// </summary>
	/// <param name="index">The index of the added rib. 0 denotes the root rib.</param>
	/// <returns><c>true</c> if the rib was added successfully; otherwise, <c>false</c>.</returns>
	public bool AddRib(int index) {
		if (index > 0 && index < _ribs.Count) {
			var prev = _ribs[index - 1];
			var next = _ribs[index];
			var space = (next - prev) / 2.0;

			if (space + MathConstants.Tolerance < WingConstraints.MinInterRibSpace) {
				return false;
			}

			var newPos = prev + space;
			_ribs.Insert(index, newPos);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Removes a rib at the specified index.
	/// </summary>
	/// <param name="index">The index of removed rib. 0 denotes the root rib.</param>
	/// <returns><c>true</c> if the rib was removed successfully; otherwise, <c>false</c>.</returns>
	public bool RemoveRib(int index) {
		if (index <= 0 || index >= _ribs.Count - 1) {
			return false;
		}
		_ribs.RemoveAt(index);
		return true;
	}

	/// <summary>
	/// Adjusts the space between ribs by shifting the position of a specific rib.
	/// </summary>
	/// <param name="index">The index of the rib to shift.</param>
	/// <param name="shift">The distance to shift the rib position.</param>
	/// <returns><c>true</c> if the shift was successful; otherwise, <c>false</c>.</returns>
	public bool ChangeRibPosition(int index, double shift) {
		if (index <= 0 || index >= _ribs.Count - 1) {
			return false;
		}

		var left = _ribs[index - 1];
		var right = _ribs[index + 1];
		var current = _ribs[index];
		var newPos = current + shift;

		if (newPos - left + MathConstants.Tolerance < WingConstraints.MinInterRibSpace ||
		    right - newPos + MathConstants.Tolerance < WingConstraints.MinInterRibSpace ||
		    newPos <= left || newPos >= right) {
			return false;
		}

		_ribs[index] = newPos;
		return true;
	}

	/// <summary>
	/// Resets the rib collection to evenly distribute a specified number of ribs.
	/// </summary>
	/// <param name="n">The number of ribs to distribute, including root and tip ribs.</param>
	/// <returns><c>true</c> if the reset was successful; otherwise, <c>false</c>.</returns>
	public bool ResetWithRibsNumber(int n) {
		if (n < 2) return false;

		var spacing = 1.0 / (n - 1);
		if (spacing + MathConstants.Tolerance < WingConstraints.MinInterRibSpace) {
			return false;
		}

		_ribs.Clear();
		for (int i = 0; i < n; i++) {
			_ribs.Add(i * spacing);
		}
		return true;
	}

	/// <summary>
	/// Clears the rib collection, resetting the ribs to only the root and tip ribs.
	/// </summary>
	public void Clear() {
		_ribs.Clear();
		_ribs.Add(0.0);
		_ribs.Add(1.0);
	}

	public IEnumerator<double> GetEnumerator() => _ribs.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}