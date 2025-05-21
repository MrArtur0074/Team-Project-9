namespace Coswalt.Models;

/// <summary>
/// Represents a spar in an aircraft wing, defining its geometric characteristics.
/// </summary>
public abstract class Spar
{
	private int _ribCount = 2;
	private int _startRib = 0;
	private int _endRib   = 1;

	private double _startChordOffset = 0.5;
	private double _endChordOffset   = 0.5;

	/// <summary>
	/// Initializes a new instance of the <see cref="Spar"/> class with the specified parameters.
	/// </summary>
	/// <param name="ribCount">The total number of ribs in the wing.</param>
	/// <param name="startRib">The starting rib index of the spar.</param>
	/// <param name="endRib">The ending rib index of the spar.</param>
	/// <param name="startChordOffset">The offset from the leading edge of the wing at the start rib.</param>
	/// <param name="endChordOffset">The offset from the leading edge of the wing at the end rib.</param>
	/// <param name="yOffset">The vertical offset of spar's center.</param>
	/// <param name="alignment">The alignment type of the spar.</param>
	/// <exception cref="ArgumentException">Thrown when the rib count is less than 2.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the start or end ribs, or the start or end chord offsets, are out of range.
	/// </exception>
	protected Spar(
		int ribCount,
		int startRib,
		int endRib,
		double startChordOffset,
		double endChordOffset,
		double yOffset,
		AlignmentType alignment
	) {
		RibCount = ribCount;
		StartRib = startRib;
		EndRib = endRib;
		StartChordOffset = startChordOffset;
		EndChordOffset = endChordOffset;
		Alignment = alignment;
		YOffset = yOffset;
	}

	/// <summary>
	/// Defines the alignment types of a spar.
	/// </summary>
	public enum AlignmentType
	{
		Linear, // Straight line offset between ribs.
		Interpolated // Follows local chord interpolation.
	}

	/// <summary>
	/// Gets of sets the number of ribs in the wing.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown when the value is less than 2.</exception>
	public int RibCount {
		get => _ribCount;
		set {
			if (value < 2) {
				throw new ArgumentException($"Rib count must be greater than 2");
			}
			_ribCount = value;
		}
	}

	/// <summary>
	/// Gets or sets the starting rib index of the spar.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the value is less than or equal to 0 or greater than the ending rib index.
	/// </exception>
	public int StartRib {
		get => _startRib;
		set {
			if (value < 0 || EndRib <= value) {
				throw new ArgumentOutOfRangeException(
					$"Starting rib must must be greater than 0 and less than ending rib");
			}
			_startRib = value;
		}
	}

	/// <summary>
	/// Gets or sets the ending rib index of the spar.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the value is less than or equal to starting rib index
	/// or greater than the number of ribs in the wing.
	/// </exception>
	public int EndRib {
		get => _endRib;
		set {
			if (value <= _startRib || value >= _ribCount) {
				throw new ArgumentOutOfRangeException(
					$"Ending rib must be greater than starting rib and less than rib count");
			}
			_endRib = value;
		}
	}

	/// <summary>
	/// Gets or sets the offset from the leading edge of the wing at the start rib.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the value is less than or greater than the maximum allowed chord offset.
	/// </exception>
	public double StartChordOffset {
		get => _startChordOffset;
		set {
			if (value is < WingConstraints.MinSparChordOffset or > WingConstraints.MaxSparChordOffset) {
				throw new ArgumentOutOfRangeException(
					$"Chord offset must be between " +
					$"{WingConstraints.MinSparChordOffset} and {WingConstraints.MaxSparChordOffset}");
			}
			_startChordOffset = value;
		}
	}

	/// <summary>
	/// Gets or sets the offset from the leading edge of the wing at the ending rib.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when the value is less than or greater than the maximum allowed chord offset.
	/// </exception>
	public double EndChordOffset {
		get => _endChordOffset;
		set {
			if (value is < WingConstraints.MinSparChordOffset or > WingConstraints.MaxSparChordOffset) {
				throw new ArgumentOutOfRangeException(
					$"Chord offset must be between " +
					$"{WingConstraints.MinSparChordOffset} and {WingConstraints.MaxSparChordOffset}");
			}
			_endChordOffset = value;
		}
	}
	
	public double YOffset { get; set; } = 0.0;

	/// <summary>
	/// Gets or sets the alignment type of the spar.
	/// </summary>
	public AlignmentType Alignment { get; set; }

	/// <inheritdoc />
	public override string ToString() {
		return $"Spar: " +
		       $"RibCount={RibCount}, " +
		       $"StartRib={StartRib}, " +
		       $"EndRib={EndRib}, " +
		       $"StartChordOffset={StartChordOffset}, " +
		       $"EndChordOffset={EndChordOffset}, " +
		       $"Alignment={Alignment}";
	}
}