namespace Project_9.Models.Interfaces;

/// <summary>
/// Represents an elliptical wing model with properties to define its aerodynamic shape.
/// </summary>
public interface IEllipticalWing
{
	/// <summary>
	/// Gets or sets the sweep coefficient. Defines the curvature of the elliptical wing.
	/// </summary>
	public float SweepCoefficient { get; set; }
	
	/// <summary>
	/// Gets or sets the length to exclude at the tip of the wing in millimeters.
	/// </summary>
	public int TipExclusionLength { get; set; }
}