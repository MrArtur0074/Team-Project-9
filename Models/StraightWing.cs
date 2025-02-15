using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents a straight wing, a specific type of wing characterized by
/// a non-tapered, linear geometry.
/// </summary>
public class StraightWing : Wing, ISingleChordWing
{
	private int _chord;
	
	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the chord length is out of the defined range
	/// </exception>
	public int Chord {
		get => _chord;
		set {
			if (value is < WingConstraints.MinRootChord or > WingConstraints.MaxRootChord) {
				throw new ArgumentOutOfRangeException(
					$"Chord must be in range [{WingConstraints.MinRootChord}; {WingConstraints.MaxRootChord}]");
			}
			_chord = value;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="StraightWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="chord">The length of the wing chord.</param>
	public StraightWing(
		string name,
		int span,
		double incidenceAngle,
		Airfoil rootAirfoil,
		Airfoil tipAirfoil,
		int chord
	) : base(name, span, incidenceAngle, rootAirfoil, tipAirfoil) {
		Chord = chord;
	}

	public override double GetArea() {
		return Span * _chord;
	}

	public override double GetAspectRatio() {
		return (double)Span / _chord;
	}
}