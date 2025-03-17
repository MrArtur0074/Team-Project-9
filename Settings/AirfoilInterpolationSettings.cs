namespace Project_9.Settings;

public sealed class AirfoilInterpolationSettings
{
	private static AirfoilInterpolationSettings _instance;
	
	public int UpperPointsCount { get; set; } = 64;
	public int LowerPointsCount { get; set; } = 64;

	private AirfoilInterpolationSettings() { }

	public static AirfoilInterpolationSettings Instance() {
		_instance = _instance ?? new AirfoilInterpolationSettings();
		return _instance;
	}
}