using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.Primitives;
using Project_9.Models;

namespace Project_9.Services;

public static class RibInterpolationService
{
	private static int           _rootChord;
	private static double        _incidenceAngleSin;
	private static double        _incidenceAngleCos;
	private static RibCollection _ribs;

	public static CadBlockEntity[] Interpolate(Wing wing, RibCollection ribs) {
		if (wing is null || ribs is null || ribs.Ribs is null) {
			throw new ArgumentException("Invalid input data.");
		}

		_rootChord = wing.RootChord;
		double radians = double.DegreesToRadians(wing.IncidenceAngle);
		_incidenceAngleSin = Double.Sin(radians);
		_incidenceAngleCos = Double.Cos(radians);
		_ribs = ribs;

		Airfoil[] interpolatedAirfoils = wing.RootAirfoil == wing.TipAirfoil
			? Enumerable.Repeat(wing.RootAirfoil, ribs.Ribs.Count).ToArray()
			: AirfoilsInterpolationService.Interpolate(wing.RootAirfoil, wing.TipAirfoil, wing.Span, ribs);

		List<CadBlockEntity> ribEntities = [];
		foreach (var airfoil in interpolatedAirfoils) {
			ribEntities.Add(CreateRibGeometry(airfoil));
		}

		foreach (Spar spar in wing.Spars) {
			IntegrateSpar(ribEntities, spar, wing);
		}

		return ribEntities.ToArray();
	}

	private static CadBlockEntity CreateRibGeometry(Airfoil airfoil) {
		List<Cad3DPoint> transformedPoints = airfoil.Points
			.Select(p => TransformPoint(p.point))
			.Select(p => new Cad3DPoint(p.X, p.Y))
			.ToList();

		CadSpline ribSpline = new CadSpline {
			ControlPoints = transformedPoints,
			Closed = 1
		};

		var ribGeometry = new CadBlockEntity();
		ribGeometry.AddEntity(ribSpline);

		return ribGeometry;
	}

	private static Point2D TransformPoint(Point2D point) {
		double x = point.X * _rootChord;
		double y = point.Y * _rootChord;
		double rotatedX = x * _incidenceAngleCos - y * _incidenceAngleSin;
		double rotatedY = x * _incidenceAngleSin + y * _incidenceAngleCos;
		return new Point2D(rotatedX, rotatedY);
	}

	private static void IntegrateSpar(List<CadBlockEntity> ribEntities, Spar spar, Wing wing) {
		for (int i = spar.StartRib; i <= spar.EndRib; ++i) {
			var rib = ribEntities[i];
			double chordOffset = CalculateChordOffset(spar, i);

			switch (spar) {
				case RectSpar rectSpar:
					AddRectSparToRib(rib, rectSpar, chordOffset);
					break;
				case CircleSpar circleSpar:
					AddCircleSparToRib(rib, circleSpar, chordOffset);
					break;
			}
		}
	}

	private static void AddRectSparToRib(CadBlockEntity rib, RectSpar spar, double chordOffset) {
		double x = spar.Rectangle.Width / 2 + chordOffset;
		double y = spar.Rectangle.Height / 2;
		var rect = new CadLwPolyline {
			PointCount = 4,
			Coordinates = [
				new Cad2DPoint(x, y),
				new Cad2DPoint(x + spar.Rectangle.Width, y),
				new Cad2DPoint(x + spar.Rectangle.Width, y + spar.Rectangle.Height),
				new Cad2DPoint(x, y + spar.Rectangle.Height)
			]
		};
		rib.AddEntity(rect);
	}

	private static void AddCircleSparToRib(CadBlockEntity rib, CircleSpar spar, double chordOffset) {
		CadCircle circle = new CadCircle {
			CenterPoint = new Cad3DPoint(chordOffset, 0.0),
			Radius = spar.Radius
		};
		rib.AddEntity(circle);
	}

	private static double CalculateChordOffset(Spar spar, int ribIndex) {
		switch (spar.Alignment) {
			case Spar.AlignmentType.Linear:
				double x = (_ribs.Ribs[ribIndex] - _ribs.Ribs[spar.StartRib]) /
				           (_ribs.Ribs[spar.EndRib] - _ribs.Ribs[spar.StartRib]);
				return spar.StartChordOffset + x * (spar.EndChordOffset - spar.StartChordOffset);
			case Spar.AlignmentType.Interpolated:
				throw new NotImplementedException("Interpolated alignment is not implemented.");
		}
		return 0.0;
	}
}