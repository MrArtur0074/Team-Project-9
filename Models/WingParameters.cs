namespace Project_9.Models;

public static class WingParameters
{
	public const int MinWingSpan = 500; // mm
	public const int MaxWingSpan = 5000; // mm

	public const int MinRootChord = 100; // mm
	public const int MaxRootChord = 1000; // mm

	public const float MinTaperRatio = 0.1f;
	public const float MaxTaperRatio = 1f;
	
	public const float MinIncidenceAngle = -5f; // degree
	public const float MaxIncidenceAngle = 15f; // degree

	public const float MinSweepAngle = 0f; // degree
	public const float MaxSweepAngle = 45f; // degree
}