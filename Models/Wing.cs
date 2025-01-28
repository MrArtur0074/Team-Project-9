using System;
using Avalonia.Controls.Documents;

namespace Project_9.Models;

/// <summary>
/// Represents a wing with properties and methods for defining its
/// geometric characteristics and calculating aerodynamic parameters
/// </summary>
public abstract class Wing
{
	private int   _span; // mm
	private float _incidenceAngle; // degree

	private Airfoil _rootAirfoil;
	private Airfoil _tipAirfoil;

	/// <summary>
	/// Gets or sets the span of the wing in millimeters.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when the span is out of the defined range.</exception>
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
	
	/// <summary>
	/// Gets or sets the incidence angle of the wing in degrees.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when the incidence angle is out of the defined range.</exception>
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

	/// <summary>
	/// Gets or sets the root airfoil of the wing.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the root airfoil is set to null.</exception>
	public Airfoil RootAirfoil {
		get => _rootAirfoil;
		set => _rootAirfoil = value ?? throw new ArgumentNullException("Root airfoil cannot be null!");
	}
	
	/// <summary>
	/// Gets or sets the tip airfoil of the wing.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the tip airfoil is set to null.</exception>
	public Airfoil TipAirfoil {
		get => _tipAirfoil;
		set => _tipAirfoil = value ?? throw new ArgumentNullException("Tip airfoil cannot be null!");
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Wing"/> class with the specified parameters.
	/// </summary>
	/// <param name="span">The span of the wing in millimeters.</param>
	/// <param name="incidenceAngle">The incidence angle of the wing in degrees.</param>
	/// <param name="rootAirfoil">The root airfoil of the wing.</param>
	/// <param name="tipAirfoil">The tip airfoil of the wing.</param>
	protected Wing(int span, float incidenceAngle, Airfoil rootAirfoil, Airfoil tipAirfoil) {
		Span = span;
		IncidenceAngle = incidenceAngle;
		RootAirfoil = rootAirfoil;
		TipAirfoil = tipAirfoil;
	}
	
	/// <summary>
	/// Calculates the area of the wing.
	/// </summary>
	/// <returns>The area of the wing in square millimeters.</returns>
	public abstract float GetArea();
	
	/// <summary>
	/// Calculates the aspect ratio of the wing.
	/// </summary>
	/// <returns>The aspect ratio of the wing.</returns>
	public abstract float GetAspectRatio();
}
