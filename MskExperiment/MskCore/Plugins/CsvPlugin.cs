using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using MskCore.IO;

namespace MskCore.Plugins;

public class CsvPlugin(ICsvReader csvReader)
{
    [KernelFunction("get_csv_rows")]
    [Description("Reads all rows from data.csv file")]
    public async Task<List<Dictionary<string, string>>> GetCsvRows()
    {
        return await csvReader.ReadCsvAsync();
    }

    [KernelFunction("get_financial_summary")]
    [Description("Returns aggregated financial totals and rows summary")]
    public async Task<string> GetFinancialSummary()
    {
        var rows = await GetCsvRows();
        decimal totalRev = rows.Sum(r => decimal.TryParse(r["total_revenue"], out var val) ? val : 0);
        decimal totalCost = rows.Sum(r => decimal.TryParse(r["cost_of_goods"], out var val) ? val : 0);
        decimal totalNet = rows.Sum(r => decimal.TryParse(r["net_earnings"], out var val) ? val : 0);

        var byCategory = rows
            .GroupBy(r => r["product_category"])
            .Select(g => $"{g.Key}: Revenue={g.Sum(x => decimal.Parse(x["total_revenue"])):F2}, Net={g.Sum(x => decimal.Parse(x["net_earnings"])):F2}")
            .ToArray();

        return $"Total Rows: {rows.Count}\nTotal Revenue: {totalRev:F2}\nTotal Cost: {totalCost:F2}\nTotal Net Earnings: {totalNet:F2}\nCategories:\n{string.Join("\n", byCategory)}";
    }
}
