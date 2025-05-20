using System.IO;
using Coswalt.Models;

namespace Coswalt.Services;

public class CsvAirfoilService : IAirfoilService
{
    private readonly string _csvPath;
    private readonly string _airfoilsDir;
    private readonly Dictionary<string, string> _airfoilFileMap;

    public CsvAirfoilService(string csvPath, string airfoilsDir) {
        _csvPath        = csvPath;
        _airfoilsDir    = airfoilsDir;
        _airfoilFileMap = LoadMapping();
    }

    private Dictionary<string, string> LoadMapping() {
        var map = new Dictionary<string, string>();
        foreach (var line in File.ReadLines(_csvPath).Skip(1)) {
            var parts = line.Split(',');
            if (parts.Length >= 2) {
                var filename = parts[0].Trim();
                var name = parts[1].Trim();
                map[name] = filename;
            }
        }

        return map;
    }

    public Task<IEnumerable<string>> GetAvailableAirfoilsAsync() {
        return Task.FromResult<IEnumerable<string>>(_airfoilFileMap.Keys);
    }

    public Task<Airfoil> LoadAirfoilAsync(string name) {
        if (!_airfoilFileMap.TryGetValue(name, out string? file))
            throw new ArgumentException($"Airfoil '{name}' not found");

        var fullPath = Path.Combine(_airfoilsDir, file);
        var airfoil = AirfoilParserService.Parse(fullPath);
        airfoil.Name = name;
        return Task.FromResult(airfoil);
    }
}