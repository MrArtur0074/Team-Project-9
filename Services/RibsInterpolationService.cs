using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Collada.FileParser.Elements;
using Aspose.CAD.Primitives;
using Avalonia.Controls.Documents;
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
		List<Cad3DPoint> transformedPoints = airfoil.Points
			.Select(p => TransformPoint(p.point, CalculateChordLength(id)))
			.Select(p => new Cad3DPoint(p.X, p.Y))
			.ToList();

		var ribSpline = new CadSpline {
			ControlPoints = transformedPoints,
			Closed = 1
		};

		var ribGeometry = new CadBlockEntity() {
			Name = airfoil.Name + id,
			Entities = [ribSpline]
		};
		
		return ribGeometry;
	}

	// Applies scale and rotation to specific point of an airfoil
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
		switch (_wing) {
			case StraightWing wing:
				return wing.RootChord;
			case TaperedWing wing:
				double tipChord = wing.RootChord / wing.TaperRatio;
				return tipChord + (wing.RootChord - tipChord) / wing.Span * 2.0 * wing.Ribs[ribIndex];
			case EllipticalWing wing:
				double x = wing.Ribs[ribIndex];
				return wing.RootChord * double.Sqrt(1.0 - (x * x) / (wing.Span * wing.Span / 4.0));
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