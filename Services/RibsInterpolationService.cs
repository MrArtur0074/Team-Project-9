using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.Primitives;
using Project_9.Models;

namespace Project_9.Services;

public class RibsInterpolationService
{
	private Wing                 _wing;
	private Airfoil[]            _interpolatedAirfoils;
	private List<CadBlockEntity> _ribEntities;

	private double _incidenceAngleSin;
	private double _incidenceAngleCos;
	
	public CadBlockEntity[] Interpolate(Wing wing) {
		if (wing is null || wing.Ribs is null) {
			throw new ArgumentException("Invalid input data.");
		}

		_wing = wing;

		double radians = double.DegreesToRadians(_wing.IncidenceAngle);
		_incidenceAngleSin = Double.Sin(radians);
		_incidenceAngleCos = Double.Cos(radians);

		_interpolatedAirfoils = _wing.RootAirfoil == _wing.TipAirfoil
			? Enumerable.Repeat(_wing.RootAirfoil, _wing.Ribs.Count).ToArray()
			: AirfoilsInterpolationService.Interpolate(_wing.RootAirfoil, _wing.TipAirfoil, _wing.Span, _wing.Ribs);

		_ribEntities = [.._interpolatedAirfoils.Select(CreateRibGeometry)];

		foreach (Spar spar in _wing.Spars) {
			IntegrateSpar(_ribEntities, spar);
		}

		return _ribEntities.ToArray();
	}

	private CadBlockEntity CreateRibGeometry(Airfoil airfoil, int id) {
		double chordLength = CalculateChordLength(id);
		List<Cad3DPoint> transformedPoints = airfoil.Points
			.Select(p => TransformPoint(p.point, chordLength))
			.Select(p => new Cad3DPoint(p.X, p.Y))
			.ToList();

		var ribSpline = new CadSpline {
			ControlPoints = transformedPoints,
			Closed = 1
		};

		return new CadBlockEntity {
			Name = airfoil.Name + id,
			Entities = [ribSpline]
		};
	}

	private Point2D TransformPoint(Point2D point, double scaleFactor) {
		double x = point.X * scaleFactor;
		double y = point.Y * scaleFactor;
		double rotatedX = x * _incidenceAngleCos - y * _incidenceAngleSin;
		double rotatedY = x * _incidenceAngleSin + y * _incidenceAngleCos;
		return new Point2D(rotatedX, rotatedY);
	}

	private static void AddCircleSparToRib(CadBlockEntity rib, CircleSpar spar, double chordOffset) {
		CadCircle circle = new CadCircle {
			CenterPoint = new Cad3DPoint(chordOffset, 0.0),
			Radius = spar.Radius
		};
		rib.AddEntity(circle);
	}

	private double CalculateChordLength(int ribIndex) {
		double chord;
		
		switch (_wing) {
			case StraightWing wing:
				chord = wing.RootChord;
				break;
			case TaperedWing wing:
				double tipChord = wing.RootChord / wing.TaperRatio;
				chord = tipChord + (wing.RootChord - tipChord) / wing.Span * 2.0 * wing.Ribs[ribIndex];
				break;
			case EllipticalWing wing:
				double x = wing.Ribs[ribIndex];
				double spanHalf = wing.Span / 2.0;
				double arg = 1.0 - (x * x) / (spanHalf * spanHalf);
				Console.WriteLine($"[EllipticalWing] ribIndex={ribIndex}, x={x}, spanHalf={spanHalf}, arg={arg}");
				chord = wing.RootChord * double.Sqrt(arg);
				break;
			default:
				chord = 1.0;
				break;
		}
		return 1.0;
	}

	private void IntegrateSpar(List<CadBlockEntity> ribEntities, Spar spar) {
		for (int i = spar.StartRib; i <= spar.EndRib; ++i) {
			var rib = ribEntities[i];
			double chordOffset = CalculateSparChordOffset(spar, i);

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

	private double CalculateSparChordOffset(Spar spar, int ribIndex) {
		switch (spar.Alignment) {
			case Spar.AlignmentType.Linear:
				double x = (_wing.Ribs[ribIndex] - _wing.Ribs[spar.StartRib]) /
				           (_wing.Ribs[spar.EndRib] - _wing.Ribs[spar.StartRib]);
				return spar.StartChordOffset + x * (spar.EndChordOffset - spar.StartChordOffset);
			case Spar.AlignmentType.Interpolated:
				throw new NotImplementedException("Interpolated alignment is not implemented.");
		}
		return 0.0;
	}
}