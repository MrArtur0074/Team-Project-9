using System;
using Project_9.Models.Interfaces;

namespace Project_9.Models;

/// <summary>
/// Represents a tapered wing with properties and methods for defining its
/// geometric characteristics and calculating aerodynamic parameters
/// </summary>
public class TaperedWing : Wing, ITaperedWing
{
	private int   _rootChord;
	private float _taperRatio;

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the root chord length is out of the defined range.
	/// </exception>
	public int RootChord {
		get => _rootChord;
		set {
			if (value is < WingParameters.MinRootChord or > WingParameters.MaxRootChord) {
				throw new ArgumentOutOfRangeException(
					$"Root chord length must be in range " +
					$"[{WingParameters.MinRootChord}; {WingParameters.MaxRootChord}]");
			}
			_rootChord = value;
		}
	}

	/// <inheritdoc />
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the taper ratio is out of the defined range.
	/// </exception>
	public float TaperRatio {
		get => _taperRatio;
		set {
			if (value is < WingParameters.MinTaperRatio or > WingParameters.MaxTaperRatio) {
				throw new ArgumentOutOfRangeException(
					$"Taper ratio must be in range [{WingParameters.MinTaperRatio}; {WingParameters.MaxTaperRatio}]");
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
		int span, 
		float incidenceAngle, 
		Airfoil rootAirfoil, 
		Airfoil tipAirfoil, 
		int rootChord, 
		float taperRatio
	) : base(span, incidenceAngle, rootAirfoil, tipAirfoil) {
		RootChord = rootChord;
		TaperRatio = taperRatio;
	}

	public override float GetArea() {
		return (1f + _taperRatio) * _rootChord * Span;
	}

	public override float GetAspectRatio() {
		return Span / (1f + _taperRatio) * _rootChord;
	}
}