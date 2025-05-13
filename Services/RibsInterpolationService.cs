using System;
using System.Collections.Generic;
using System.Linq;
using netDxf.Blocks;
using netDxf.Entities;
using netDxf.Tables;
using Project_9.Models;
using Vector2 = netDxf.Vector2;
using Vector3 = netDxf.Vector3;

namespace Project_9.Services;

public class RibsInterpolationService
{
	private Wing        _wing;
	private Airfoil[]   _interpolatedAirfoils;
	private List<Block> _ribEntities;

	private double _incidenceAngleSin;
	private double _incidenceAngleCos;

	private Layer _ribsLayer  = new Layer("Ribs");
	private Layer _sparsLayer = new Layer("Spars");

	public Block[] Interpolate(Wing wing) {
		if (wing is null || wing.Ribs is null) {
			throw new ArgumentException("Invalid input data.");
		}

		_wing = wing;

		double radians = double.DegreesToRadians(_wing.IncidenceAngle);
		_incidenceAngleSin = Double.Sin(radians);
		_incidenceAngleCos = Double.Cos(radians);

		_interpolatedAirfoils = _wing.RootAirfoil == _wing.TipAirfoil
			? Enumerable.Repeat(_wing.RootAirfoil, _wing.Ribs.Count).ToArray()
			: AirfoilsInterpolationService.Interpolate(_wing.RootAirfoil, _wing.TipAirfoil, _wing.Ribs);

		_ribEntities = [.._interpolatedAirfoils.Select(CreateRibGeometry)];

		foreach (Spar spar in _wing.Spars) {
			IntegrateSpar(_ribEntities, spar);
		}

		return _ribEntities.ToArray();
	}

	private Block CreateRibGeometry(Airfoil airfoil, int id) {
		double chordLength = CalculateChordLength(id);
		var transformedPoints = airfoil.GetPointsSelig()
			.Select(p => TransformPoint(p, chordLength))
			.Select(p => new Vector3(p.X, p.Y, 0.0))
			.ToArray();

		var weights = Enumerable.Repeat(1.0, transformedPoints.Length).ToList();
		var ribSpline = new Spline(transformedPoints, weights, 3, false);

		return new Block(airfoil.Name + id) {
			Layer = _ribsLayer,
			Entities = { ribSpline }
		};
	}

	private Vector2 TransformPoint(Vector2 point, double scaleFactor) {
		double x = point.X * scaleFactor;
		double y = point.Y * scaleFactor;
		double rotatedX = x * _incidenceAngleCos - y * _incidenceAngleSin;
		double rotatedY = x * _incidenceAngleSin + y * _incidenceAngleCos;
		return new Vector2(rotatedX, rotatedY);
	}

	private double CalculateChordLength(int ribIndex) {
		double chord;

		switch (_wing) {
			case StraightWing wing:
				chord = wing.RootChord;
				break;
			case TaperedWing wing:
				double tipChord = wing.RootChord / wing.TaperRatio;
				chord = tipChord + (wing.RootChord - tipChord) / wing.Span * wing.Ribs[ribIndex] * wing.Span;
				break;
			case EllipticalWing wing:
				double xLimit = 1 - wing.TipExclusionRatio;
				double x = wing.Ribs[ribIndex] * xLimit;
				chord = wing.RootChord * double.Sqrt(1.0 - x * x);
				break;
			default:
				chord = 1.0;
				break;
		}
		return chord;
	}

	private void IntegrateSpar(List<Block> ribEntities, Spar spar) {
		for (int i = spar.StartRib; i <= spar.EndRib; ++i) {
			var rib = ribEntities[i];
			double chordOffset = CalculateSparChordOffset(spar, i);

			switch (spar) {
				case RectSpar rectSpar:
					AddRectSparToRib(rib, rectSpar, chordOffset, i);
					break;
				case CircleSpar circleSpar:
					AddCircleSparToRib(rib, circleSpar, chordOffset, i);
					break;
			}
		}
	}

	private void AddCircleSparToRib(Block rib, CircleSpar spar, double chordOffset, int ribIndex) {
		Airfoil airfoil = _interpolatedAirfoils[ribIndex];
		double chordLength = CalculateChordLength(ribIndex);
		var transformedPoints = airfoil.GetPointsSelig()
			.Select(p => TransformPoint(p, chordLength))
			.ToList();

		var (upperY, lowerY) = GetAirfoilYBounds(transformedPoints, chordOffset);

		if (spar.Radius > (upperY - lowerY) / 2.0) {
			throw new ArgumentOutOfRangeException(nameof(spar.Radius), "Radius exceeds airfoil thickness.");
		}
		
		if (spar.YOffset - spar.Radius < lowerY || spar.YOffset + spar.Radius > upperY) {
			throw new ArgumentException("Circle spar exceeds airfoil boundaries.");
		}

		var circle = new Circle(new Vector2(chordOffset, spar.YOffset), spar.Radius) { Layer = _sparsLayer };
		rib.Entities.Add(circle);
	}

	private void AddRectSparToRib(Block rib, RectSpar spar, double chordOffset, int ribIndex) {
		Airfoil airfoil = _interpolatedAirfoils[ribIndex];
		double chordLength = CalculateChordLength(ribIndex);
		var transformedPoints = airfoil.GetPointsSelig()
			.Select(p => TransformPoint(p, chordLength))
			.ToList();

		var (upperY, lowerY) = GetAirfoilYBounds(transformedPoints, chordOffset);

		double xStart = chordOffset - spar.Width / 2;
		double yPosition = CalculateRectYPosition(spar, upperY, lowerY);

		if (xStart < 0 || xStart + spar.Width > chordLength)
			throw new ArgumentException("Rectangular spar exceeds chord boundaries.");
		
		if (yPosition < lowerY || yPosition + spar.Height > upperY)
			throw new ArgumentException("Rectangular spar exceeds airfoil boundaries.");

		var rect = new Polyline2D(new[] {
			new Vector2(xStart, yPosition),
			new Vector2(xStart + spar.Width, yPosition),
			new Vector2(xStart + spar.Width, yPosition + spar.Height),
			new Vector2(xStart, yPosition + spar.Height)
		}, true) { Layer = _sparsLayer };

		rib.Entities.Add(rect);
	}

	private double CalculateRectYPosition(RectSpar spar, double upperY, double lowerY) {
		return spar.ProfileAlignment switch {
			RectSpar.ProfileAlignmentType.Upper => upperY + spar.YOffset - spar.Height,
			RectSpar.ProfileAlignmentType.Lower => lowerY + spar.YOffset,
			RectSpar.ProfileAlignmentType.Custom => spar.YOffset - spar.Height / 2,
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private (double upperY, double lowerY) GetAirfoilYBounds(List<Vector2> points, double x) {
		var upper = new List<Vector2>();
		var lower = new List<Vector2>();
		int mid = points.Count / 2;

		for (int i = 0; i < mid; i++)
			upper.Add(points[i]);
		for (int i = mid; i < points.Count; i++)
			lower.Add(points[i]);

		double upperY = InterpolateYAtX(upper.OrderBy(p => p.X).ToList(), x);
		double lowerY = InterpolateYAtX(lower.OrderBy(p => p.X).ToList(), x);
		return (upperY, lowerY);
	}

	private double InterpolateYAtX(List<Vector2> points, double x) {
		for (int i = 0; i < points.Count - 1; i++) {
			if (points[i].X <= x && points[i + 1].X >= x) {
				double t = (x - points[i].X) / (points[i + 1].X - points[i].X);
				return points[i].Y + t * (points[i + 1].Y - points[i].Y);
			}
		}
		return points.OrderBy(p => Math.Abs(p.X - x)).First().Y;
	}

	private double CalculateSparChordOffset(Spar spar, int ribIndex) {
		switch (spar.Alignment) {
			case Spar.AlignmentType.Linear:
				double startPos = _wing.Ribs[spar.StartRib];
				double endPos = _wing.Ribs[spar.EndRib];
				double ribPos = _wing.Ribs[ribIndex];
				double t = (ribPos - startPos) / (endPos - startPos);
				double interpolated = spar.StartChordOffset + t * (spar.EndChordOffset - spar.StartChordOffset);
				double chordLength = CalculateChordLength(ribIndex);
				double offset = interpolated * chordLength;

				if (offset < 0 || offset > chordLength)
					throw new ArgumentOutOfRangeException(nameof(offset), "Offset exceeds chord length.");

				return offset;
			case Spar.AlignmentType.Interpolated:
				throw new NotImplementedException("Interpolated alignment is not implemented.");
			default:
				return 0.0;
		}
	}
}