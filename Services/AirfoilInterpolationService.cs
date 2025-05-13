using System;
using System.Linq;
using netDxf;
using Project_9.Models;
using Project_9.Settings;

namespace Project_9.Services;

public static class AirfoilsInterpolationService
{
	private static Airfoil UnifyAirfoil(Airfoil airfoil) {
		var settings = AirfoilInterpolationSettings.Instance();
		var upperPoints = UnifySurface(airfoil.UpperPoints, settings.UpperPointsCount);
		var lowerPoints = UnifySurface(airfoil.LowerPoints, settings.LowerPointsCount);

		return new Airfoil($"{airfoil.Name} UNIFIED", upperPoints, lowerPoints);
	}

	private static Vector2[] UnifySurface(Vector2[] surfacePoints, int pointsCount) {
		var sorted = surfacePoints.OrderBy(p => p.X).ToArray();
		double xMin = sorted.First().X;
		double xMax = sorted.Last().X;
		double step = (xMax - xMin) / (pointsCount - 1);

		var points = new Vector2[pointsCount];
		for (int i = 0; i < pointsCount; ++i) {
			double xNew = xMin + i * step;
			double yNew = InterpolateY(sorted, xNew);
			points[i] = new Vector2(xNew, yNew);
		}

		return points;
	}

	private static double InterpolateY(Vector2[] points, double x) {
		if (x < points[0].X || x > points[^1].X)
			throw new ArgumentOutOfRangeException(nameof(x),
				$"x={x} is outside interpolation range [{points[0].X}, {points[^1].X}]");

		for (int i = 0; i < points.Length - 1; i++) {
			if (points[i].X <= x && x <= points[i + 1].X) {
				double x1 = points[i].X, y1 = points[i].Y;
				double x2 = points[i + 1].X, y2 = points[i + 1].Y;
				return y1 + (y2 - y1) * (x - x1) / (x2 - x1);
			}
		}
		return 0.0;
	}

	private static Airfoil InterpolateAirfoil(Airfoil rootAirfoil, Airfoil tipAirfoil, double ratio) {
		var name = $"{rootAirfoil.Name} - {tipAirfoil.Name} {ratio:F4}";
		var upperPoints = InterpolateSurface(rootAirfoil.UpperPoints, tipAirfoil.UpperPoints, ratio);
		var lowerPoints = InterpolateSurface(rootAirfoil.LowerPoints, tipAirfoil.LowerPoints, ratio);

		return new Airfoil(name, upperPoints, lowerPoints);
	}

	private static Vector2[] InterpolateSurface(Vector2[] rootPoints, Vector2[] tipPoints, double ratio) {
		if (rootPoints.Length != tipPoints.Length)
			throw new ArgumentException("Root and tip surface points must be the same!");

		var points = new Vector2[rootPoints.Length];
		for (int i = 0; i < rootPoints.Length; ++i) {
			Vector2 rootPoint = rootPoints[i];
			Vector2 tipPoint = tipPoints[i];
			var x = rootPoint.X + (tipPoint.X - rootPoint.X) * ratio;
			var y = rootPoint.Y + (tipPoint.Y - rootPoint.Y) * ratio;
			points[i] = new Vector2(x, y);
		}

		return points;
	}

	public static Airfoil[] Interpolate(
		Airfoil rootAirfoil, Airfoil tipAirfoil, RibCollection ribCollection
	) {
		int ribsCount = ribCollection.Count;
		var airfoils = new Airfoil[ribsCount];

		Airfoil unifiedRoot = UnifyAirfoil(rootAirfoil);
		Airfoil unifiedTip = UnifyAirfoil(tipAirfoil);

		for (int i = 0; i < ribsCount; ++i) {
			var ratio = ribCollection[i];
			airfoils[i] = InterpolateAirfoil(unifiedRoot, unifiedTip, ratio);
		}

		return airfoils;
	}
}