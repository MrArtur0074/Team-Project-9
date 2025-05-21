using System;
using System.IO;
using System.Text.Json;
using Coswalt.Models;
using Coswalt.Parsers;

namespace Coswalt.Services;

public class WingJsonParserService
{
	public Wing ParseWingFromJson(string jsonFilePath) {
		if (!File.Exists(jsonFilePath)) {
			throw new FileNotFoundException("JSON configuration file not found", jsonFilePath);
		}

		string jsonContent = File.ReadAllText(jsonFilePath);
		var config = JsonSerializer.Deserialize<WingConfig>(jsonContent);

		var rootAirfoil = AirfoilParserService.Parse(config.RootAirfoilPath);
		var tipAirfoil = AirfoilParserService.Parse(config.TipAirfoilPath);

		Wing wing = config.Type.ToLower() switch {
			"straight" => new StraightWing(
				config.Name,
				config.RootChord,
				config.Span,
				config.IncidenceAngle,
				rootAirfoil,
				tipAirfoil),

			"tapered" => new TaperedWing(
				config.Name,
				config.RootChord,
				config.Span,
				config.IncidenceAngle,
				rootAirfoil,
				tipAirfoil,
				config.TaperRatio),

			"elliptical" => new EllipticalWing(
				config.Name,
				config.RootChord,
				config.Span,
				config.IncidenceAngle,
				rootAirfoil,
				tipAirfoil,
				config.Sweep,
				config.TipExclusion),

			_ => throw new ArgumentException($"Unknown wing type: {config.Type}")
		};

		wing.Ribs.ResetWithRibsNumber(config.RibCount);

		foreach (var sparConfig in config.Spars) {
			Spar spar = sparConfig.Type.ToLower() switch {
				"circle" => new CircleSpar(
					config.RibCount,
					sparConfig.StartRib,
					sparConfig.EndRib,
					sparConfig.StartChordOffset,
					sparConfig.EndChordOffset,
					Enum.Parse<Spar.AlignmentType>(sparConfig.Alignment),
					sparConfig.YOffset,
					sparConfig.Radius),

				"rect" => new RectSpar(
					config.RibCount,
					sparConfig.StartRib,
					sparConfig.EndRib,
					sparConfig.StartChordOffset,
					sparConfig.EndChordOffset,
					Enum.Parse<Spar.AlignmentType>(sparConfig.Alignment),
					sparConfig.YOffset,
					sparConfig.Height,
					sparConfig.Width) {
					ProfileAlignment = Enum.Parse<RectSpar.ProfileAlignmentType>(sparConfig.ProfileAlignment)
				},

				_ => throw new ArgumentException($"Unknown spar type: {sparConfig.Type}")
			};

			wing.Spars.Add(spar);
		}

		return wing;
	}
}