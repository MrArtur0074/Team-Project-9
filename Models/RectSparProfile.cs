using Avalonia;

namespace Project_9.Models;

/// <summary>
/// Represents a rectangular profile of wing spar.
/// </summary>
public class RectSparProfile : SparProfile
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RectSparProfile"/> class with specified parameters.
	/// </summary>
	/// <param name="rectangle">The rectangle of the profile.</param>
	public RectSparProfile(Rect rectangle) {
		Rectangle = rectangle;
	}

	public Rect Rectangle { get; set; }
}