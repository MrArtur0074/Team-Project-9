using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Oswalt.Models;

namespace Project9.Services;

public class DwgExportService : IDwgExportService
{
	public async Task ExportAsync(Wing wing, string filePath) {
		using CadImage cadImage = (CadImage)Image.Load(filePath);

		var ribEntities = new RibsInterpolationService().Interpolate(wing);
		foreach (var ribEntity in ribEntities) {
			cadImage.BlockEntities.Add(ribEntity.Name, ribEntity);
		}

		cadImage.Save(filePath);
	}
}