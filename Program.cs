using System;
using System.IO;
using System.Linq;
using netDxf.Header;
using Project_9.Services;

namespace Project_9;

public static class Program
{
	public static void Main(string[] args) {
		if (args.Length == 0) {
			Console.WriteLine("Provide path to JSON configuration file");
			return;
		}

		try {
			var wing = new WingJsonParserService().ParseWingFromJson(args[0]);
			var blocks = new RibsInterpolationService().Interpolate(wing).ToList();

			var result = DxfExportService.Export(
				Path.ChangeExtension(args[0], ".dxf"),
				blocks,
				DxfVersion.AutoCad2018,
				true);

			Console.WriteLine(!result.IsSuccess
				? $"Error: {result.ErrorMessage}"
				: "Wing successfully generated!"
			);
		}
		catch (Exception ex) {
			Console.WriteLine($"Error: {ex.Message}");
		}
	}
}