namespace Project_9.Models.Interfaces;

/// <summary>
/// Represents a tapered wing model with a root chord and taper ratio.
/// </summary>
public interface ITaperedWing
{
	/// <summary>
	/// Gets or sets the root chord length of the tapered wing in millimeters.
	/// </summary>
	public int RootChord { get; set; }
	
	/// <summary>
	/// Gets or sets the taper ratio of the wing, which is
	/// the ratio of the tip chord length to the root chord length.
	/// </summary>
	public double TaperRatio { get; set; }
}