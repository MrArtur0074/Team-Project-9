using System;
using Avalonia.Controls.Documents;

namespace Project_9.Models;

public abstract class Wing
{
	private int   _span; // mm
	private float _incidenceAngle; // degree

	private Airfoil _rootAirfoil;
	private Airfoil _tipAirfoil;

	public int Span {
		get => _span;
		set {
			if (value is < WingParameters.MinWingSpan or > WingParameters.MaxWingSpan) {
				throw new ArgumentException(
					$"Span must be in range " +
					$"[{WingParameters.MinWingSpan}; {WingParameters.MaxWingSpan}]");
			}
			_span = value;
		}
	}
	
	public float IncidenceAngle {
		get => _incidenceAngle;
		set {
			if (value is < WingParameters.MinIncidenceAngle or > WingParameters.MaxIncidenceAngle) {
				throw new ArgumentException(
					$"Incidence angle must be in range " +
					$"[{WingParameters.MinIncidenceAngle}; {WingParameters.MaxIncidenceAngle}]");
			}
			_incidenceAngle = value;
		} 
	}

	public Airfoil RootAirfoil {
		get => _rootAirfoil;
		set => _rootAirfoil = value ?? throw new ArgumentNullException("Root airfoil cannot be null!");
	}
	
	public Airfoil TipAirfoil {
		get => _tipAirfoil;
		set => _tipAirfoil = value ?? throw new ArgumentNullException("Tip airfoil cannot be null!");
	}
	
	protected Wing(int span, float incidenceAngle, Airfoil rootAirfoil, Airfoil tipAirfoil) {
		Span = span;
		IncidenceAngle = incidenceAngle;
		RootAirfoil = rootAirfoil;
		TipAirfoil = tipAirfoil;
	}
	
	public abstract float GetArea();
	public abstract float GetAspectRatio();
}
