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
	private const double BlocksMargin     = 10f;
	private const string DefaultLayerName = "Geometry";

	public static ExportResult Export(
		string filePath,
		List<Block> blocks,
		DxfVersion version = DxfVersion.AutoCad2018,
		bool arrangeBlocks = true
	) {
		try {
			if (string.IsNullOrWhiteSpace(filePath))
				return ExportResult.Failure("File path required");

			if (blocks.Count == 0)
				return ExportResult.Failure("No blocks to export");

			var doc = new DxfDocument(version) {
				DrawingVariables = {
					InsUnits = netDxf.Units.DrawingUnits.Millimeters,
					InsBase = Vector3.Zero
				}
			};

			CreateDocumentLayers(doc, blocks);

			if (arrangeBlocks) {
				ArrangeBlocksInLayout(blocks, doc);
			} else {
				AddBlocksWithoutArrangement(doc, blocks);
			}

			doc.Save(filePath);
			return ExportResult.Success(filePath);
		}
		catch (Exception ex) {
			return ExportResult.Failure($"Export failed: {ex.Message}");
		}
	}

	private static void CreateDocumentLayers(DxfDocument doc, IEnumerable<Block> blocks) {
		doc.Layers.Add(new Layer(DefaultLayerName) { Color = AciColor.Cyan });

		var layers = blocks
			.SelectMany(b => b.Entities)
			.Select(e => e.Layer)
			.Where(l => l != null)
			.GroupBy(l => l.Name)
			.Select(g => g.First());

		foreach (Layer layer in layers) {
			if (!doc.Layers.Contains(layer.Name)) {
				doc.Layers.Add(layer);
			}
		}
	}

	private static void ArrangeBlocksInLayout(IList<Block> blocks, DxfDocument doc) {
		double currentY = 0f;

		foreach (var block in blocks) {
			block.Name = SanitizeBlockName(block.Name);
			doc.Blocks.Add(block);

			BoundingRectangle bounds = GetBlockBoundingBox(block);
			double blockHeight = bounds.Maximum.Y - bounds.Minimum.Y;

			var insert = new Insert(block) {
				Position = new Vector3(
					-bounds.Minimum.X,
					currentY + bounds.Minimum.Y,
					0),
				Layer = block.Entities.FirstOrDefault()?.Layer ?? doc.Layers[DefaultLayerName],
				Color = AciColor.ByLayer
			};

			doc.Entities.Add(insert);
			currentY -= blockHeight + BlocksMargin;
		}
	}

	private static BoundingRectangle GetBlockBoundingBox(Block block) {
		var bounds = new BoundingRectangle();

		foreach (var entity in block.Entities) {
			switch (entity) {
				case Line line:
					bounds.ExpandToInclude(line.StartPoint);
					bounds.ExpandToInclude(line.EndPoint);
					break;
				case Circle circle:
					bounds.ExpandToInclude(new Vector2(
						circle.Center.X - circle.Radius,
						circle.Center.Y - circle.Radius));
					bounds.ExpandToInclude(new Vector2(
						circle.Center.X + circle.Radius,
						circle.Center.Y + circle.Radius));
					break;
				case Polyline2D polyline:
					foreach (var vertex in polyline.Vertexes) {
						bounds.ExpandToInclude(new Vector2(vertex.Position.X, vertex.Position.Y));
					}
					break;
				case Spline spline:
					foreach (var point in spline.ControlPoints) {
						bounds.ExpandToInclude(new Vector2(point.X, point.Y));
					}
					break;
			}
		}

		return bounds;
	}

	private static void AddBlocksWithoutArrangement(DxfDocument doc, IEnumerable<Block> blocks) {
		foreach (var block in blocks) {
			block.Name = SanitizeBlockName(block.Name);
			doc.Blocks.Add(block);

			var insert = new Insert(block) {
				Layer = block.Entities.FirstOrDefault()?.Layer ?? doc.Layers[DefaultLayerName],
				Color = AciColor.ByLayer
			};
			doc.Entities.Add(insert);
		}
	}

	private static string SanitizeBlockName(string name) {
		if (string.IsNullOrEmpty(name))
			return $"Block_{Guid.NewGuid():N8}";

		return new string(name
			.Where(c => char.IsLetterOrDigit(c) || c == '_' || c == '-')
			.ToArray());
	}
}

public class BoundingRectangle
{
	public Vector2 Minimum { get; private set; } = new(double.MaxValue, double.MaxValue);
	public Vector2 Maximum { get; private set; } = new(double.MinValue, double.MinValue);

	public void ExpandToInclude(Vector2 point) {
		Minimum = new Vector2(
			Math.Min(Minimum.X, point.X),
			Math.Min(Minimum.Y, point.Y));

		Maximum = new Vector2(
			Math.Max(Maximum.X, point.X),
			Math.Max(Maximum.Y, point.Y));
	}

	public void ExpandToInclude(Vector3 point) =>
		ExpandToInclude(new Vector2(point.X, point.Y));
}