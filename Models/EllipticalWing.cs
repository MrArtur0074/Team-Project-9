using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents an elliptical wing, a specialized type of wing with 
/// an elliptical planform for optimized aerodynamic performance.
/// </summary>
public class EllipticalWing : Wing, ISingleChordWing, IEllipticalWing
{
	private int    _chord;
	private double _sweepCoefficient;
	private double _tipExclusionRatio;

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
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the sweep coefficient is out of the defined range.
	///</exception>
	public double SweepCoefficient {
		get => _sweepCoefficient;
		set {
			if (value is < WingConstraints.MinSweepCoefficient or > WingConstraints.MaxSweepCoefficient) {
				throw new ArgumentOutOfRangeException(
					$"Span must be in range " +
					$"[{WingConstraints.MinSweepCoefficient}; {WingConstraints.MaxSweepCoefficient}]");
			}
			_sweepCoefficient = value;
		}
	}

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the exclusion ratio value is out of the defined range.
	///</exception>
	public double TipExclusionRatio {
		get => _tipExclusionRatio;
		set {
			if (value is < WingConstraints.MinTipExclusionRatio or > WingConstraints.MaxTipExclusionRatio) {
				throw new ArgumentOutOfRangeException(
					$"Tip exclusion ratio must be in range " +
					$"[{WingConstraints.MinTipExclusionRatio}; {WingConstraints.MaxTipExclusionRatio}]");
			}
			_tipExclusionRatio = value;
		}
	}

	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="EllipticalWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="chord">The length of the wing root chord.</param>>
	/// <param name="sweep">The sweep coefficient, defining the curvature of the leading edge.</param>
	/// <param name="tipExclusion">The ratio of the tip exclusion, limiting the rib generation area.</param>
	public EllipticalWing(
		string name, int span, double incidenceAngle, Airfoil rootAirfoil, Airfoil tipAirfoil,
		int chord, double sweep, int tipExclusion
	)
		: base(name, span, incidenceAngle, rootAirfoil, tipAirfoil) {
		Chord = chord;
		SweepCoefficient = sweep;
		TipExclusionRatio = tipExclusion;
	}

	public override double GetArea() {
		return double.Pi * Chord * Span / 4.0;
	}

	public override double GetAspectRatio() {
		return Span * 4.0 / (double.Pi * Chord);
	}
}