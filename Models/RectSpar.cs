using Aspose.CAD;

namespace Oswalt.Models;

/// <summary>
/// Represents a spar in an aircraft wing with a rectangular profile.
/// </summary>
public class RectSpar : Spar
{
	/// <inheritdoc />>
	/// <summary>
	/// Initializes a new instance of the <see cref="RectSpar"/> class with the specified parameters.
	/// </summary>
	/// <param name="rect">The rectangle describing the spar's profile.</param>
	public RectSpar(
		int ribCount,
		int startRib,
		int endRib,
		double startChordOffset,
		double endChordOffset,
		AlignmentType alignment,
		Rectangle rect
	) : base(ribCount, startRib, endRib, startChordOffset, endChordOffset, alignment) {
		Rect = rect;
	}

	/// <summary>
	/// Defines the alignment type of spar's profile.
	/// </summary>
	public enum ProfileAlignmentType
	{
		Upper,
		Lower,
		Custom
	}

	/// <summary>
	/// Gets or sets the type of spar's profile alignment.
	/// </summary>
	public ProfileAlignmentType ProfileAlignment { get; set; }

	/// <summary>
	/// Gets or sets the rectangle defining the spar's profile.
	/// </summary>
	public RectangleF Rect { get; set; }
}