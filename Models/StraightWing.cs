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
			if (value is < WingParameters.MinRootChord or > WingParameters.MaxRootChord) {
				throw new ArgumentOutOfRangeException(
					$"Chord must be in range [{WingParameters.MinRootChord}; {WingParameters.MaxRootChord}]");
			}
			_chord = value;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="StraightWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="chord">The length of the wing chord.</param>
	public StraightWing(int span, float incidenceAngle, Airfoil rootAirfoil, Airfoil tipAirfoil, int chord) 
		: base(span, incidenceAngle, rootAirfoil, tipAirfoil) {
		Chord = chord;
	}

	public override float GetArea() {
		return Span * _chord;
	}

	public override float GetAspectRatio() {
		return (float) Span / _chord;
	}
}