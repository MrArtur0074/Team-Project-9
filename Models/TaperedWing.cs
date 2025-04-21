using System;

namespace Oswalt.Models;

/// <summary>
/// Represents a tapered wing with properties and methods for defining its
/// geometric characteristics and calculating aerodynamic parameters
/// </summary>
public class TaperedWing : Wing
{
	private double _taperRatio;

	/// <summary>
	/// Gets or sets the taper ratio of the wing.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the taper ratio is out of the defined range.
	/// </exception>
	public double TaperRatio {
		get => _taperRatio;
		set {
			if (value is < WingConstraints.MinTaperRatio or > WingConstraints.MaxTaperRatio) {
				throw new ArgumentOutOfRangeException(
					$"Taper ratio must be between {WingConstraints.MinTaperRatio} and {WingConstraints.MaxTaperRatio}");
			}
			_taperRatio = value;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="TaperedWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="taperRatio">The taper ratio of the wing.</param>
	public TaperedWing(
		string name,
		int rootChord,
		int span,
		double incidenceAngle,
		Airfoil rootAirfoil,
		Airfoil tipAirfoil,
		double taperRatio
	) : base(name, rootChord, span, incidenceAngle, rootAirfoil, tipAirfoil) {
		TaperRatio = taperRatio;
	}

	public override double GetArea() {
		return (1.0 + _taperRatio) * RootChord * Span;
	}

	public override double GetAspectRatio() {
		return Span / (1.0 + _taperRatio) * RootChord;
	}
}