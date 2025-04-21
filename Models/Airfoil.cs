using Aspose.CAD.Primitives;

namespace Oswalt.Models;

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
	public (Point2D point, bool isUpper)[] Points { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Airfoil"/> class with a specified name and set of points.
	/// </summary>
	/// <param name="name">The name of the airfoil.</param>
	/// <param name="points">
	/// The array of <see cref="Point2"/> representing the points defining the shape of the airfoil.
	/// </param>
	public Airfoil(string name, (Point2D, bool)[] points) {
		Name = name;
		Points = points;
	}
}