using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD.Primitives;

namespace Project_9.Models;

public static class AirfoilParserService
{
	/// <summary>
	/// Parses an airfoil DAT file and returns an Airfoil object.
	/// </summary>
	/// <param name="filePath">The path to the DAT file.</param>
	/// <returns>An Airfoil object containing the parsed data.</returns>
	public static Airfoil Parse(string filePath) {
		var lines = File.ReadAllLines(filePath)
			.Where(line => !string.IsNullOrWhiteSpace(line))
			.ToList();

		if (lines.Count == 0)
			throw new InvalidDataException("The file is empty or contains no valid data.");

		string name = lines[0].Trim();
		var dataLines = lines.Skip(1).ToList();

		bool isLednicerFormat = IsLednicerFormat(dataLines);

		List<(Point2D point, bool isUpper)> points = isLednicerFormat
			? ParseLednicerFormat(dataLines)
			: ParseSeligFormat(dataLines);

		return new Airfoil(name, points.ToArray());
	}

	/// <summary>
	/// Checks if the file is in Lednicer format.
	/// </summary>
	private static bool IsLednicerFormat(List<string> dataLines) {
		var firstLineValues = dataLines[0].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

		if (firstLineValues.Length == 2
		    && double.TryParse(firstLineValues[0], out double x)
		    && double.TryParse(firstLineValues[1], out double y)) {
			return x > 1.01 || y > 1.01;
		}

		return false;
	}

	/// <summary>
	/// Parses the data lines in Selig format.
	/// </summary>
	private static List<(Point2D point, bool isUpper)> ParseSeligFormat(List<string> dataLines) {
		var points = new List<(Point2D point, bool isUpper)>();
		bool isUpperSurface = true;

		foreach (var line in dataLines) {
			var values = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
			if (values.Length != 2)
				throw new InvalidDataException("Invalid data format: Expected 2 numeric values per line.");

			if (!double.TryParse(values[0], out double x) || !double.TryParse(values[1], out double y))
				throw new InvalidDataException("Invalid data format: Non-numeric values encountered.");

			ValidateCoordinates(x, y);
			points.Add((new Point2D(x, y), isUpperSurface));

			if (points.Count > 1 && x > points[^2].point.X)
				isUpperSurface = false;
		}

		return points;
	}

	/// <summary>
	/// Parses the data lines in Lednicer format.
	/// </summary>
	private static List<(Point2D point, bool isUpper)> ParseLednicerFormat(List<string> dataLines) {
		var points = new List<(Point2D point, bool isUpper)>();
		var firstLineValues = dataLines[0].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

		if (!int.TryParse(firstLineValues[0], out int topCount) ||
		    !int.TryParse(firstLineValues[1], out int bottomCount)) {
			throw new InvalidDataException(
				"Invalid Lednicer format: Expected integer counts for top and bottom points.");
		}
		var dataPoints = dataLines.Skip(1).ToList();

		if (dataPoints.Count != topCount + bottomCount) {
			throw new InvalidDataException("Invalid Lednicer format: Mismatch in the number of points.");
		}

		for (int i = 0; i < topCount; i++) {
			var values = dataPoints[i].Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);
			if (values.Length != 2) {
				throw new InvalidDataException("Invalid data format: Expected 2 numeric values per line.");
			}

			if (!double.TryParse(values[0], out double x) || !double.TryParse(values[1], out double y)) {
				throw new InvalidDataException("Invalid data format: Non-numeric values encountered.");
			}

			ValidateCoordinates(x, y);
			points.Add((new Point2D(x, y), true));
		}

		for (int i = topCount; i < topCount + bottomCount; i++) {
			var values = dataPoints[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			if (values.Length != 2)
				throw new InvalidDataException("Invalid data format: Expected 2 numeric values per line.");

			if (!double.TryParse(values[0], out double x) || !double.TryParse(values[1], out double y))
				throw new InvalidDataException("Invalid data format: Non-numeric values encountered.");

			ValidateCoordinates(x, y);
			points.Add((new Point2D(x, y), false));
		}

		var orderedPoints = points.Take(topCount).Reverse()
			.Concat(points.Skip(topCount))
			.ToList();

		return orderedPoints;
	}

	/// <summary>
	/// Validates the X and Y coordinates.
	/// </summary>
	private static void ValidateCoordinates(double x, double y) {
		if (x < -0.01 || x > 1.01)
			Console.WriteLine($"Warning: X coordinate {x} is outside the expected range [0.0, 1.0].");

		if (y < -1.0 || y > 1.0)
			throw new InvalidDataException($"Invalid Y coordinate {y}: Must be in the range [-1.0, 1.0].");
	}
}