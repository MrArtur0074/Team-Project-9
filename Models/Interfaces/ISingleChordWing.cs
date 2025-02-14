namespace Project_9.Models.Interfaces;

/// <summary>
/// Represents a single-chord wing with a defined chord length.
/// </summary>
public interface ISingleChordWing
{
	/// <summary>
	/// Gets or sets the chord length of the wing in millimeters.
	/// </summary>
	public int Chord { get; set; }
}