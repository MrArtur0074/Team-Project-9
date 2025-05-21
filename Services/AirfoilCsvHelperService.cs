using System.IO;

public static class AirfoilCsvHelperService
{
    public static string? GetPathByName(string csvFilePath, string nameToFind) {
        if (!File.Exists(csvFilePath))
            throw new FileNotFoundException("CSV file not found.", csvFilePath);

        var airfoilsDirectory = Path.Combine(Path.GetDirectoryName(csvFilePath) ?? "", "airfoils");

        using var reader = new StreamReader(csvFilePath);
        string? line;
        bool headerSkipped = false;

        while ((line = reader.ReadLine()) != null) {
            if (!headerSkipped) {
                headerSkipped = true;
                continue;
            }

            var parts = line.Split(',');
            if (parts.Length < 2)
                continue;

            string relativePath = parts[0].Trim();
            string name = parts[1].Trim();

            if (name.Equals(nameToFind, StringComparison.OrdinalIgnoreCase)) {
                var fullPath = Path.Combine(airfoilsDirectory, relativePath);
                return Path.GetFullPath(fullPath);
            }
        }

        return null;
    }
}