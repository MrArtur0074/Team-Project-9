using System;
using System.Collections.Generic;
using System.IO;

namespace Coswalt.Services;

public static class AirfoilCsvHelperService
{
    public static string? GetPathByName(string csvFilePath, string nameToFind) {
        if (!File.Exists(csvFilePath))
            throw new FileNotFoundException("CSV file not found.", csvFilePath);

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

            string name = parts[0].Trim();
            string path = parts[1].Trim();

            if (name.Equals(nameToFind, StringComparison.OrdinalIgnoreCase))
                return path;
        }

        return null;
    }
}