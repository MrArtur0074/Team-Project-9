using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents a tapered wing with properties and methods for defining its
/// geometric characteristics and calculating aerodynamic parameters
/// </summary>
public class TaperedWing : Wing, ITaperedWing
{
	private int    _rootChord;
	private double _taperRatio;

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the root chord length is out of the defined range.
	/// </exception>
	public int RootChord {
		get => _rootChord;
		set {
			if (value is < WingConstraints.MinRootChord or > WingConstraints.MaxRootChord) {
				throw new ArgumentOutOfRangeException(
					$"Root chord length must be in range " +
					$"[{WingConstraints.MinRootChord}; {WingConstraints.MaxRootChord}]");
			}
			_rootChord = value;
		}
	}

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the taper ratio is out of the defined range.
	/// </exception>
	public double TaperRatio {
		get => _taperRatio;
		set {
			if (value is < WingConstraints.MinTaperRatio or > WingConstraints.MaxTaperRatio) {
				throw new ArgumentOutOfRangeException(
					$"Taper ratio must be in range [{WingConstraints.MinTaperRatio}; {WingConstraints.MaxTaperRatio}]");
			}
			_taperRatio = value;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="TaperedWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="rootChord">The root chord length of the wing.</param>
	/// <param name="taperRatio">The taper ratio of the wing.</param>
	public TaperedWing(
		string name,
		int span, 
		double incidenceAngle, 
		Airfoil rootAirfoil, 
		Airfoil tipAirfoil, 
		int rootChord, 
		double taperRatio
	) : base(name, span, incidenceAngle, rootAirfoil, tipAirfoil) {
		RootChord = rootChord;
		TaperRatio = taperRatio;
	}

	public override double GetArea() {
		return (1.0 + _taperRatio) * _rootChord * Span;
	}

	public override double GetAspectRatio() {
		return Span / (1.0 + _taperRatio) * _rootChord;
	}
}