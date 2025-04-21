using System.Threading.Tasks;
using Oswalt.Models;

namespace Oswalt.Services;

public interface IDwgExportService
{
	Task ExportAsync(Wing wing, string filePath);
}