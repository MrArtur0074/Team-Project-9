using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents an elliptical wing, a specialized type of wing with 
/// an elliptical planform for optimized aerodynamic performance.
/// </summary>
public class EllipticalWing : Wing, IEllipticalWing
{
	private float _sweepCoefficient;
	private float _tipExclusionRatio;
	
	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the sweep coefficient is out of the defined range.
	///</exception>
	public float SweepCoefficient {
		get => _sweepCoefficient;
		set {
			if (value is < WingParameters.MinSweepCoefficient or > WingParameters.MaxSweepCoefficient) {
				throw new ArgumentOutOfRangeException(
					$"Span must be in range " +
					$"[{WingParameters.MinSweepCoefficient}; {WingParameters.MaxSweepCoefficient}]");
			}
			_sweepCoefficient = value;
		} 
	}

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the exclusion ratio value is out of the defined range.
	///</exception>
	public float TipExclusionRatio {
		get => _tipExclusionRatio;
		set {
			if (value is < WingParameters.MinTipExclusionRatio or > WingParameters.MaxTipExclusionRatio) {
				throw new ArgumentOutOfRangeException(
					$"Tip exclusion ratio must be in range " +
					$"[{WingParameters.MinTipExclusionRatio}; {WingParameters.MaxTipExclusionRatio}]");
			}
			_tipExclusionRatio = value;
		}
	}
	
	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="EllipticalWing"/> class with the specified parameters.
	/// </summary>
	/// <param name="sweep">The sweep coefficient, defining the curvature of the leading edge.</param>
	/// <param name="tipExclusion">The ratio of the tip exclusion, limiting the rib generation area.</param>
	public EllipticalWing(int span, float incidenceAngle, Airfoil rootAirfoil, Airfoil tipAirfoil, 
		float sweep, int tipExclusion) 
		: base(span, incidenceAngle, rootAirfoil, tipAirfoil) {
		SweepCoefficient = sweep;
		TipExclusionRatio = tipExclusion;
	}

	public override float GetArea() {
		throw new System.NotImplementedException();
	}

	public override float GetAspectRatio() {
		throw new System.NotImplementedException();
	}
}