using System;
using System.IO;
using System.Linq;
using netDxf;
using Coswalt.Models;

namespace Coswalt.Services;

public static class AirfoilParserService
{
	public static Airfoil Parse(string filePath) {
		var lines = File.ReadAllLines(filePath)
			.Where(line => !string.IsNullOrWhiteSpace(line))
			.Select(line => line.Trim())
			.ToArray();

		if (lines.Length < 2)
			throw new InvalidDataException("File must contain at least two non-empty lines.");

		string name = lines[0];
		var dataLines = lines.Skip(1).ToArray();

		return TryParseLednicerHeader(dataLines[0], out int upperCount, out int lowerCount)
			? ParseLednicerFormat(name, dataLines, upperCount, lowerCount)
			: ParseSeligFormat(name, dataLines);
	}

	private static bool TryParseLednicerHeader(string line, out int upperCount, out int lowerCount) {
		upperCount = lowerCount = 0;
		var parts = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length != 2) return false;
		if (!double.TryParse(parts[0], out double first) || !double.TryParse(parts[1], out double second))
			return false;

		if (first > 1.1) {
			upperCount = (int)first;
			lowerCount = (int)second;
			return true;
		}

		return false;
	}

	private static Airfoil ParseLednicerFormat(string name, string[] lines, int upperCount, int lowerCount) {
		var points = ParsePoints(lines.Skip(1).Take(upperCount + lowerCount).ToArray());

		if (points.Length != upperCount + lowerCount)
			throw new InvalidDataException("Number of points doesn't match header counts.");

		return new Airfoil(
			name,
			points.Take(upperCount).ToArray(),
			points.Skip(upperCount).Take(lowerCount).ToArray()
		);
	}

	private static Airfoil ParseSeligFormat(string name, string[] lines) {
		var points = ParsePoints(lines);
		var trailingEdgeIndex = FindTrailingEdgeIndex(points);

		var upperPoints = points.Take(trailingEdgeIndex + 1).ToArray();
		var lowerPoints = points.Skip(trailingEdgeIndex).Reverse().ToArray();

		return new Airfoil(name, upperPoints, lowerPoints);
	}

	private static Vector2[] ParsePoints(string[] lines) {
		var points = new Vector2[lines.Length];

		for (int i = 0; i < lines.Length; i++) {
			var parts = lines[i].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != 2) {
				throw new InvalidDataException($"Line {i + 2} must contain exactly two numbers.");
			}
			if (!double.TryParse(parts[0], out double x) || !double.TryParse(parts[1], out double y)) {
				throw new InvalidDataException($"Line {i + 2} contains invalid numbers.");
			}
			if (x is < -0.01 or > 1.01) {
				throw new InvalidDataException(
					$"X coordinate {x} on line {i + 2} is out of valid range [-0.01, 1.01].");
			}
			if (y is < -1.0 or > 1.0) {
				throw new InvalidDataException(
					$"Y coordinate {y} on line {i + 2} is out of valid range [-1.0, 1.0].");
			}
			if (x is < 0.0 or > 1.0) {
				Console.WriteLine(
					$"Warning: X coordinate {x} on line {i + 2} is outside standard [0.0, 1.0] range.");
			}

			points[i] = new Vector2(x, y);
		}

		return points;
	}

	private static int FindTrailingEdgeIndex(Vector2[] points) {
		for (int i = 0; i < points.Length - 1; i++) {
			if (points[i].X < points[i + 1].X) {
				return i;
			}
		}

		return points.Length - 1;
	}
}