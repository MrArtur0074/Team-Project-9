using System;
using System.Collections.Generic;

namespace Project_9.Models;

/// <summary>
/// Represents a wing with properties and methods for defining its
/// geometric characteristics and calculating aerodynamic parameters
/// </summary>
public abstract class Wing
{
	private string _name;
	private double _rootChord;
	private double _span;
	private double _incidenceAngle;

	private Airfoil _rootAirfoil;
	private Airfoil _tipAirfoil;

	/// <summary>
	/// Gets or sets the name of the wing.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the name is set to null.</exception>
	public string Name {
		get => _name;
		set => _name = value ?? throw new ArgumentNullException($"Name cannot be null!");
	}

	/// <summary>
	/// Gets or sets the root chord length of the wing.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the root chord length is out of the defined range.
	/// </exception>
	public double RootChord {
		get => _rootChord;
		set {
			if (value is < WingConstraints.MinRootChord or > WingConstraints.MaxRootChord) {
				throw new ArgumentOutOfRangeException(
					$"Root chord length must be in range " +
					$"[{WingConstraints.MinRootChord}; {WingConstraints.MaxRootChord}]");
			}
			_rootChord = value;
		}
	}

	/// <summary>
	/// Gets or sets the span of the wing in millimeters.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the span is out of the defined range.
	/// </exception>
	public double Span {
		get => _span;
		set {
			if (value is < WingConstraints.MinWingSpan or > WingConstraints.MaxWingSpan) {
				throw new ArgumentOutOfRangeException(
					$"Span must be in range " +
					$"[{WingConstraints.MinWingSpan}; {WingConstraints.MaxWingSpan}]");
			}
			_span = value;
		}
	}

	/// <summary>
	/// Gets or sets the incidence angle of the wing in degrees.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the incidence angle is out of the defined range.
	/// </exception>
	public double IncidenceAngle {
		get => _incidenceAngle;
		set {
			if (value is < WingConstraints.MinIncidenceAngle or > WingConstraints.MaxIncidenceAngle) {
				throw new ArgumentOutOfRangeException(
					$"Incidence angle must be in range " +
					$"[{WingConstraints.MinIncidenceAngle}; {WingConstraints.MaxIncidenceAngle}]");
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
		set => _rootAirfoil = value ?? throw new ArgumentNullException($"Root airfoil cannot be null!");
	}

	/// <summary>
	/// Gets or sets the tip airfoil of the wing.
	/// </summary>
	/// <exception cref="ArgumentNullException">Thrown when the tip airfoil is set to null.</exception>
	public Airfoil TipAirfoil {
		get => _tipAirfoil;
		set => _tipAirfoil = value ?? throw new ArgumentNullException($"Tip airfoil cannot be null!");
	}

	/// <summary>
	/// Get the collection of ribs of the wing.
	/// </summary>
	public RibCollection Ribs { get; private set; }

	/// <summary>
	/// Gets the collection of spars of the wing.
	/// </summary>
	public List<Spar> Spars { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="Wing"/> class with the specified parameters.
	/// </summary>
	/// <param name="name">The name of the wing.</param>>
	/// <param name="rootChord">The root chord length of the wing in millimeters.</param>
	/// <param name="span">The span of the wing in millimeters.</param>
	/// <param name="incidenceAngle">The incidence angle of the wing in degrees.</param>
	/// <exception cref="ArgumentNullException">
	/// Thrown when the name or root or tip airfoils are null.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the span or incidence angle are out of range.
	/// </exception>
	protected Wing(
		string name,
		double rootChord,
		double span,
		double incidenceAngle
	) {
		Name = name;
		RootChord = rootChord;
		Span = span;
		IncidenceAngle = incidenceAngle;
		Ribs = new RibCollection(span);
		Spars = new List<Spar>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Wing"/> class with the specified parameters.
	/// </summary>
	/// <param name="name">The name of the wing.</param>>
	/// <param name="rootChord">The root chord length of the wing in millimeters.</param>
	/// <param name="span">The span of the wing in millimeters.</param>
	/// <param name="incidenceAngle">The incidence angle of the wing in degrees.</param>
	/// <param name="rootAirfoil">The root airfoil of the wing.</param>
	/// <param name="tipAirfoil">The tip airfoil of the wing.</param>
	/// <exception cref="ArgumentNullException">
	/// Thrown when the name or root or tip airfoils are null.
	/// </exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the span or incidence angle are out of range.
	/// </exception>
	protected Wing(
		string name,
		double rootChord,
		double span,
		double incidenceAngle,
		Airfoil rootAirfoil,
		Airfoil tipAirfoil
	) : this(name, rootChord, span, incidenceAngle) {
		RootAirfoil = rootAirfoil;
		TipAirfoil = tipAirfoil;
	}

	/// <summary>
	/// Calculates the area of the wing.
	/// </summary>
	/// <returns>The area of the wing in square millimeters.</returns>
	public abstract double GetArea();

	/// <summary>
	/// Calculates the aspect ratio of the wing.
	/// </summary>
	/// <returns>The aspect ratio of the wing.</returns>
	public abstract double GetAspectRatio();
}