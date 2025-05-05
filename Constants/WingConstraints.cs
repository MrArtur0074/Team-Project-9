namespace Project_9.Models;

/// <summary>
/// Provides predefined wing parameter limits for aerodynamic models.
/// </summary>
public static class WingConstraints
{
	/// <summary>
	/// The minimum allowable wingspan of a wing, measured in millimeters.
	/// </summary>
	public const double MinWingSpan = 500;

	/// <summary>
	/// The maximum allowable wingspan of a wing, measured in millimeters.
	/// </summary>
	public const double MaxWingSpan = 5000;

	/// <summary>
	/// The minimum allowable root chord length, measured in millimeters.
	/// </summary>
	public const double MinRootChord = 100;

	/// <summary>
	/// The maximum allowable root chord length, measured in millimeters.
	/// </summary>
	public const double MaxRootChord = 1000;

	/// <summary>
	/// The minimum allowable taper ratio, which is the ratio of the tip chord 
	/// length to the root chord length.
	/// </summary>
	public const double MinTaperRatio = 0.1;

	/// <summary>
	/// The maximum allowable taper ratio, which is the ratio of the tip chord 
	/// length to the root chord length.
	/// </summary>
	public const double MaxTaperRatio = 1.0;

	/// <summary>
	/// The minimum allowable sweep coefficient, defining the curvature of
	/// the elliptical wing.
	/// </summary>
	public const double MinSweepCoefficient = -1.0;

	/// <summary>
	/// The maximum allowable sweep coefficient, defining the curvature of 
	/// the elliptical wing.
	/// </summary>
	public const double MaxSweepCoefficient = 1.0;

	/// <summary>
	/// The minimum allowable elliptical wingtip exclusion ratio
	/// relative to the wingspan.
	/// </summary>
	public const double MinTipExclusionRatio = 0.1;
	
	/// <summary>
	/// The maximum allowable elliptical wingtip exclusion ratio
	/// relative to the wingspan.
	/// </summary>
	public const double MaxTipExclusionRatio = 0.5;

	/// <summary>
	/// The minimum allowable incidence angle of the wing, measured in degrees.
	/// </summary>
	public const double MinIncidenceAngle = -5.0;

	/// <summary>
	/// The maximum allowable incidence angle of the wing, measured in degrees.
	/// </summary>
	public const double MaxIncidenceAngle = 15.0;

	/// <summary>
	/// The minimum allowable sweep angle of the wing, measured in degrees.
	/// </summary>
	public const double MinSweepAngle = 0.0;

	/// <summary>
	/// The maximum allowable sweep angle of the wing, measured in degrees.
	/// </summary>
	public const double MaxSweepAngle = 45.0;

	/// <summary>
	/// The minimum allowable spar-to-chord offset ratio, relative to the 
	/// chord length of the wing.
	/// </summary>
	public const double MinSparChordOffset = 0.1;
	
	/// <summary>
	/// The maximum allowable spar-to-chord offset ratio, relative to the 
	/// chord length of the wing.
	/// </summary>
	public const double MaxSparChordOffset = 0.9;
	
	/// <summary>
	/// The minimum allowable space between two ribs in millimeters.
	/// </summary>
	public const double MinInterRibSpace = 1.0;
}