namespace Coswalt.Models;

/// <summary>
/// Represents a straight wing, a specific type of wing characterized by
/// a non-tapered, linear geometry.
/// </summary>
public class StraightWing : Wing
{
	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="StraightWing"/> class with the specified parameters.
	/// </summary>
	public StraightWing(
		string name,
		double rootChord,
		double span,
		double incidenceAngle
	) : base(name, rootChord, span, incidenceAngle) {
	}
	
	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="StraightWing"/> class with the specified parameters.
	/// </summary>
	public StraightWing(
		string name,
		double rootChord,
		double span,
		double incidenceAngle,
		Airfoil rootAirfoil,
		Airfoil tipAirfoil
	) : base(name, rootChord, span, incidenceAngle, rootAirfoil, tipAirfoil) {
	}

	public override double GetArea() {
		return Span * RootChord;
	}

	public override double GetAspectRatio() {
		return Span / RootChord;
	}
}