namespace Project_9.Models;

/// <summary>
/// Provides predefined wing parameter limits for aerodynamic models.
/// </summary>
public static class WingParameters
{
	/// <summary>
	/// The minimum allowable wingspan of a wing, measured in millimeters.
	/// </summary>
	public const int MinWingSpan = 500;

	/// <summary>
	/// The maximum allowable wingspan of a wing, measured in millimeters.
	/// </summary>
	public const int MaxWingSpan = 5000;

	/// <summary>
	/// The minimum allowable root chord length, measured in millimeters.
	/// </summary>
	public const int MinRootChord = 100;

	/// <summary>
	/// The maximum allowable root chord length, measured in millimeters.
	/// </summary>
	public const int MaxRootChord = 1000;

	/// <summary>
	/// The minimum allowable taper ratio, which is the ratio of the tip chord 
	/// length to the root chord length.
	/// </summary>
	public const float MinTaperRatio = 0.1f;

	/// <summary>
	/// The maximum allowable taper ratio, which is the ratio of the tip chord 
	/// length to the root chord length.
	/// </summary>
	public const float MaxTaperRatio = 1f;

	/// <summary>
	/// The minimum allowable sweep coefficient, defining the curvature of
	/// the elliptical wing.
	/// </summary>
	public const float MinSweepCoefficient = -1f;

	/// <summary>
	/// The maximum allowable sweep coefficient, defining the curvature of 
	/// the elliptical wing.
	/// </summary>
	public const float MaxSweepCoefficient = 1f;

	/// <summary>
	/// The minimum allowable incidence angle of the wing, measured in degrees.
	/// </summary>
	public const float MinIncidenceAngle = -5f;

	/// <summary>
	/// The maximum allowable incidence angle of the wing, measured in degrees.
	/// </summary>
	public const float MaxIncidenceAngle = 15f;

	/// <summary>
	/// The minimum allowable sweep angle of the wing, measured in degrees.
	/// </summary>
	public const float MinSweepAngle = 0f;

	/// <summary>
	/// The maximum allowable sweep angle of the wing, measured in degrees.
	/// </summary>
	public const float MaxSweepAngle = 45f;
}