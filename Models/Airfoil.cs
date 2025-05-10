using System;
using System.Linq;
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
	/// Gets or sets the points that define the upper surface of the airfoil.
	/// </summary>
	public Point2D[] UpperPoints { get; set; }

	/// <summary>
	/// Gets or sets the points that define the lower surface of the airfoil.
	/// </summary>
	public Point2D[] LowerPoints { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Airfoil"/> class with a specified name and set of points.
	/// </summary>
	/// <param name="name">The name of the airfoil.</param>
	/// <param name="upperPoints">The array of points defining the upper surface of the airfoil.</param>
	/// <param name="lowerPoints">The array of points defining the lower surface of the airfoil.</param>
	public Airfoil(string name, Point2D[] upperPoints, Point2D[] lowerPoints) {
		Name = name;
		UpperPoints = upperPoints;
		LowerPoints = lowerPoints;
	}

	public Point2D[] GetPointsSelig() {
		var upperSurface = UpperPoints.OrderByDescending(p => p.X).ToArray();
		var lowerSurface = LowerPoints.Skip(1).OrderBy(p => p.X).ToArray();
		return upperSurface.Concat(lowerSurface).ToArray();
	}
}