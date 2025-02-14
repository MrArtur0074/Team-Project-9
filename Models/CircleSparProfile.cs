namespace Project_9.Models;

/// <summary>
/// Represent a circle profile of wing spar.
/// </summary>
public class CircleSparProfile : SparProfile
{
	
	/// <summary>
	/// Initializes a new instance of the <see cref="CircleSparProfile"/> class with radius
	/// </summary>
	/// <param name="radius">The radius of the circle profile.</param>
	public CircleSparProfile(double radius) {
		Radius = radius;
	}

	/// <summary>
	/// Gets of sets the radius.
	/// </summary>
	public double Radius { get; set; }
}