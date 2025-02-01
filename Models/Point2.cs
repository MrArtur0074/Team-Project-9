namespace Project_9.Models;

/// <summary>
/// Represents a two-dimensional point.
/// </summary>
public struct Point2
{
	/// <summary>
	/// The X coordinate of the point.
	/// </summary>
	public double X;

	/// <summary>
	/// The Y coordinate of the point.
	/// </summary>
	public double Y;

	/// <summary>
	/// Initializes a new instance of the <see cref="Point2"/> struct with default values (0, 0).
	/// </summary>
	public Point2() {
		X = 0d;
		Y = 0d;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Point2"/> struct with specified X and Y coordinates.
	/// </summary>
	/// <param name="x">The X coordinate of the point.</param>
	/// <param name="y">The Y coordinate of the point.</param>
	public Point2(double x, double y) {
		X = x;
		Y = y;
	}
}