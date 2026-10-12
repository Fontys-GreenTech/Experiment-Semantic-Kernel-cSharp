using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MskCore.IO;

public class CsvReader : ICsvReader
{
    private readonly string _filePath;
    
    public CsvReader()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../data.csv");
        if (!File.Exists(_filePath))
        {
            _filePath = "data.csv"; // TODO: From config/IOptions
        }
    }
    
    public async Task<List<Dictionary<string, string>>> ReadCsvAsync()
    {
        var lines = await File.ReadAllLinesAsync(_filePath);
        if (lines.Length == 0) return [];

        var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
        var rows = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();
            var row = new Dictionary<string, string>();
            for (int j = 0; j < Math.Min(headers.Length, values.Length); j++)
            {
                row[headers[j]] = values[j];
            }
            rows.Add(row);
        }

        return rows;
    }
}