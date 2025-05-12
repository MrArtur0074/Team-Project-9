using Vector2 = netDxf.Vector2;

namespace Project_9.Models;

/// <summary>
/// Represents a spar in an aircraft wing with a circular profile.
/// </summary>
public class CircleSpar : Spar
{
	/// <inheritdoc />
	/// <summary>
	/// Initializes a new instance of the <see cref="CircleSpar"/> class with the specified parameters.
	/// </summary>
	/// <param name="center">The center of the spar's profile.</param>
	/// <param name="radius">The radius of the spar's profile.</param>
	public CircleSpar(
		int ribCount,
		int startRib,
		int endRib,
		double startChordOffset,
		double endChordOffset,
		AlignmentType alignment,
		Vector2 center,
		double radius
	) : base(ribCount, startRib, endRib, startChordOffset, endChordOffset, alignment) {
		Center = center;
		Radius = radius;
	}

	/// <summary>
	/// Gets or sets the center of the spar's profile.
	/// </summary>
	public Vector2 Center { get; set; }

	/// <summary>
	/// Gets or sets the radius of the spar's profile.
	/// </summary>
	public double Radius { get; set; }
}