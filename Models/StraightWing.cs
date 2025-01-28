using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents a straight wing, a specific type of wing characterized by
/// a non-tapered, linear geometry.
/// Inherits properties and methods for calculating aerodynamic parameters
/// from the <see cref="Wing"/> class.
/// </summary>
public class StraightWing : Wing, ISingleChordWing
{
	private int _chord;
	
	/// <summary>
	/// Gets or sets the chord length of the wing in millimeters.
	/// </summary>
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

	/// <summary>
	/// Initializes a new instance of the <see cref="StraightWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="span">The span of the wing in millimeters.</param>
	/// <param name="incidenceAngle">The incidence angle of the wing in degrees.</param>
	/// <param name="rootAirfoil">The root airfoil of the wing.</param>
	/// <param name="tipAirfoil">The tip airfoil of the wing.</param>
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