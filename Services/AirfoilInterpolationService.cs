using System.Collections.Generic;
using System.Linq;
using Aspose.CAD.Primitives;
using Project_9.Models;
using Project_9.Settings;

namespace Project_9.Services;

public static class AirfoilsInterpolationService
{
	private static Airfoil UnifyAirfoil(Airfoil airfoil) {
		var upperSurface = airfoil.Points.Where(point => point.isUpper).Select(point => point.point).ToArray();
		var lowerSurface = airfoil.Points.Where(point => !point.isUpper).Select(point => point.point).ToArray();

		var name = $"{airfoil.Name}-unified";
		var points = new List<(Point2D, bool)>();

		points.AddRange(UnifySurface(upperSurface, true, AirfoilInterpolationSettings.Instance().UpperPointsCount));
		points.AddRange(UnifySurface(lowerSurface, false, AirfoilInterpolationSettings.Instance().LowerPointsCount));

		return new Airfoil(name, points.ToArray());
	}

	private static (Point2D, bool)[] UnifySurface(Point2D[] surfacePoints, bool isUpper, int pointsCount) {
		var points = new (Point2D, bool)[pointsCount];

		double xMin = surfacePoints.Min(point => point.X);
		double xMax = surfacePoints.Max(point => point.X);
		double step = (xMax - xMin) / (pointsCount - 1);

		for (int i = 0; i < pointsCount; ++i) {
			double xNew = xMin + i * step;
			double yNew = InterpolateY(surfacePoints, xNew);
			points[i] = (new(xNew, yNew), isUpper);
		}

		return points;
	}

	private static double InterpolateY(Point2D[] points, double x) {
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
		var rootPoints = rootAirfoil.Points;
		var tipPoints = tipAirfoil.Points;

		var name = $"{rootAirfoil.Name}-{tipAirfoil.Name}-{ratio}";
		var points = new (Point2D, bool)[rootPoints.Length];

		for (int i = 0; i < points.Length; ++i) {
			var rootPoint = rootPoints[i].point;
			var tipPoint = tipPoints[i].point;
			var x = rootPoint.X + (tipPoint.X - rootPoint.X) * ratio;
			var y = rootPoint.Y + (tipPoint.Y - rootPoint.Y) * ratio;
			points[i] = (new(x, y), rootPoints[i].isUpper);
		}

		return new Airfoil(name, points);
	}

	public static Airfoil[] Interpolate(
		Airfoil rootAirfoil, Airfoil tipAirfoil, double span, RibCollection ribCollection
	) {
		var ribs = ribCollection.Ribs;
		int ribsCount = ribs.Count;

		var airfoils = new Airfoil[ribsCount];

		var unifiedRootAirfoil = UnifyAirfoil(rootAirfoil);
		var unifiedTipAirfoil = UnifyAirfoil(tipAirfoil);

		for (int i = 0; i < ribsCount; ++i) {
			var ratio = ribs[i] / span;
			airfoils[i] = InterpolateAirfoil(unifiedRootAirfoil, unifiedTipAirfoil, ratio);
		}

		return airfoils;
	}
}