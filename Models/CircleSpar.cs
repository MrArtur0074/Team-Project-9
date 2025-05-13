using Vector2 = netDxf.Vector2;

namespace Coswalt.Models;

/// <summary>
/// Represents a spar in an aircraft wing with a circular profile.
/// </summary>
public class CircleSpar : Spar
{
	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="CircleSpar"/> class with the specified parameters.
	/// </summary>
	/// <param name="yOffset">The vertical offset of the spar's profile in millimeters.</param>
	/// <param name="radius">The radius of the spar's profile.</param>
	public CircleSpar(
		int ribCount,
		int startRib,
		int endRib,
		double startChordOffset,
		double endChordOffset,
		AlignmentType alignment,
		double yOffset,
		double radius
	) : base(ribCount, startRib, endRib, startChordOffset, endChordOffset, yOffset, alignment) {
		Radius = radius;
	}

	/// <summary>
	/// Gets or sets the radius of the spar's profile.
	/// </summary>
	public double Radius { get; set; }
}