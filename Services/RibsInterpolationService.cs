using System;
using System.Collections.Generic;
using System.Linq;
using netDxf.Blocks;
using netDxf.Entities;
using netDxf.GTE;
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
					AddRectSparToRib(rib, rectSpar, chordOffset);
					break;
				case CircleSpar circleSpar:
					AddCircleSparToRib(rib, circleSpar, chordOffset);
					break;
			}
		}
	}

	private void AddCircleSparToRib(Block rib, CircleSpar spar, double chordOffset) {
		var circle = new Circle(new Vector2(chordOffset, 0.0), spar.Radius) {
			Layer = _sparsLayer
		};
		rib.Entities.Add(circle);
	}

	private void AddRectSparToRib(Block rib, RectSpar spar, double chordOffset) {
		double x = spar.Width / 2.0 + chordOffset;
		double y = spar.Height / 2.0;

		var rect = new Polyline2D([
			new Vector2(x, y),
			new Vector2(x + spar.Width, y),
			new Vector2(x + spar.Width, y + spar.Height),
			new Vector2(x, y + spar.Height)
		], true) {
			Layer = _sparsLayer
		};


		rib.Entities.Add(rect);
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