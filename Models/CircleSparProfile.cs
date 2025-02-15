using Avalonia;

namespace Project_9.Models;

/// <summary>
/// Represent a circle profile of wing spar.
/// </summary>
public class CircleSparProfile : SparProfile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CircleSparProfile"/> class with radius
	/// </summary>
	/// <param name="position">The position of the circle profile.</param>
	/// <param name="radius">The radius of the circle profile.</param>
	public CircleSparProfile(Point position, double radius) {
		Position = position;
		Radius = radius;
	}

	/// <summary>
	/// Gets or sets the position of the spar.
	/// </summary>
	public Point Position { get; set; }

	/// <summary>
	/// Gets of sets the radius.
	/// </summary>
	public double Radius { get; set; }
}