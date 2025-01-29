namespace Project_9.Models;

/// <summary>
/// Represents an airfoil as a collection of points defining its shape.
/// </summary>
public class Airfoil
{
	/// <summary>
	/// Gets or sets the name of the airfoil.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the points that define the shape of the airfoil.
	/// </summary>
	public Point2[] Points { get; set; }
}