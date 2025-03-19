using System.Threading.Tasks;
using Project_9.Models;

namespace Project_9.Services;

public interface IDwgExportService
{
	Task ExportAsync(Wing wing, string filePath);
}