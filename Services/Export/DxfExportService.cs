using System;
using System.Collections.Generic;
using System.Linq;
using netDxf;
using netDxf.Blocks;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;

public static class DxfExportService
{
	public static ExportResult Export(
		string filePath, List<Block> blocks, DxfVersion version = DxfVersion.AutoCad2018
	) {
		try {
			if (string.IsNullOrWhiteSpace(filePath))
				return ExportResult.Failure("File path required");

			var doc = new DxfDocument(version) {
				DrawingVariables = {
					InsUnits = netDxf.Units.DrawingUnits.Millimeters,
					InsBase = Vector3.Zero
				}
			};

			var layers = blocks
				.SelectMany(b => b.Entities)
				.Select(e => e.Layer)
				.Where(l => l != null)
				.GroupBy(l => l.Name)
				.Select(g => g.First());

			foreach (Layer layer in layers) {
				doc.Layers.Add(layer);
			}

			foreach (Block block in blocks) {
				block.Name = SanitizeBlockName(block.Name);
				doc.Blocks.Add(block);

				var insert = new Insert(block) {
					Layer = block.Entities.FirstOrDefault()?.Layer,
					Color = AciColor.ByLayer
				};
				doc.Entities.Add(insert);
			}

			doc.Save(filePath);
			return ExportResult.Success(filePath);
		}
		catch (Exception ex) {
			return ExportResult.Failure($"Export failed: {ex.Message}");
		}
	}
	
	private static string SanitizeBlockName(string name) {
		if (string.IsNullOrEmpty(name))
			return "Unnamed_Block_" + Guid.NewGuid().ToString("N")[..8];

		return new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
	}
}