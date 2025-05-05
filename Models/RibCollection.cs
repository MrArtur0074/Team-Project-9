using System;
using System.Collections;
using System.Collections.Generic;
using Oswalt.Constants;

namespace Oswalt.Models;

/// <summary>
/// Represents a collection of ribs in a wing.
/// Includes permanent root and tip ribs.
/// </summary>
public class RibCollection : IEnumerable<double>
{
	private readonly double       _halfSpan;
	private readonly List<double> _ribs = new();

	public double this[int index] {
		get => _ribs[index];
	}

	public int Count {
		get => _ribs.Count;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="RibCollection"/> class with a specified span.
	/// </summary>
	/// <param name="span">The span of the rib row.</param>
	/// <exception cref="ArgumentException">Thrown when the span is out of the defined range.</exception>
	public RibCollection(double span) {
		if (span is < WingConstraints.MinWingSpan or > WingConstraints.MaxWingSpan) {
			throw new ArgumentException(
				$"Span must be between {WingConstraints.MinWingSpan} and {WingConstraints.MaxWingSpan} mm.");
		}
		_halfSpan = span / 2.0;
		_ribs.Add(0.0);
		_ribs.Add(_halfSpan);
	}

	/// <summary>
	/// Inserts a rib with the specified index.
	/// </summary>
	/// <param name="index">The index of the added rib. 0 denotes the root rib.</param>
	/// <returns><c>true</c> if the rib was added successfully; otherwise, <c>false</c>.</returns>
	public bool AddRib(int index) {
		if (index > 0 && index < _ribs.Count) {
			var prevRibPos = _ribs[index - 1];
			var nextRibPos = _ribs[index];
			var space = (nextRibPos - prevRibPos) / 2.0;

			if (space + MathConstants.Tolerance < WingConstraints.MinInterRibSpace) {
				return false;
			}

			var position = prevRibPos + space;
			_ribs.Insert(index, position);
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
	/// <param name="shift">The amount to shift the rib position.</param>
	/// <returns><c>true</c> if the shift was successful; otherwise, <c>false</c>.</returns>
	public bool ChangeRibPosition(int index, double shift) {
		if (index <= 0 || index >= _ribs.Count - 1) {
			return false;
		}

		var leftSpace = _ribs[index] - _ribs[index - 1];
		var rightSpace = _ribs[index + 1] - _ribs[index];
		var newLeftSpace = leftSpace + shift;
		var newRightSpace = rightSpace - shift;

		if (newLeftSpace + MathConstants.Tolerance < WingConstraints.MinInterRibSpace
		    || newRightSpace + MathConstants.Tolerance < WingConstraints.MinInterRibSpace) {
			return false;
		}

		var newPosition = _ribs[index] + shift;

		return true;
	}

	/// <summary>
	/// Resets the rib collection to evenly distribute a specified number of ribs.
	/// </summary>
	/// <param name="n">The number of ribs to distribute, including root and tip ribs.</param>
	/// <returns><c>true</c> if the reset was successful; otherwise, <c>false</c>.</returns>
	public bool ResetWithRibsNumber(int n) {
		if (n < 2) {
			return false;
		}

		var space = _halfSpan / (n - 1);

		if (space + MathConstants.Tolerance < WingConstraints.MinInterRibSpace) {
			return false;
		}

		_ribs.Clear();
		for (var i = 0; i < n; ++i) _ribs.Add(space * i);

		return true;
	}

	/// <summary>
	/// Clears the rib collection, resetting the ribs to only the root and tip ribs.
	/// </summary>
	public void Clear() {
		_ribs.Clear();
		_ribs.Add(0.0);
		_ribs.Add(_halfSpan);
	}

	public IEnumerator<double> GetEnumerator() {
		return _ribs.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}