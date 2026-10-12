using MskCore.IO;

namespace MskBenchmarks.Mocks;

public class MockCsvReader : ICsvReader
{
    private static readonly List<Dictionary<string, string>> Rows =
    [
        new()
        {
            ["transaction_id"]   = "TXN-1001",
            ["date"]             = "2026-09-01",
            ["customer_name"]    = "Alice Smith",
            ["product_category"] = "Electronics",
            ["item_description"] = "Wireless Mouse",
            ["quantity"]         = "2",
            ["unit_price"]       = "25.00",
            ["total_revenue"]    = "50.00",
            ["cost_of_goods"]    = "20.00",
            ["net_earnings"]     = "30.00"
        },
        new()
        {
            ["transaction_id"]   = "TXN-1002",
            ["date"]             = "2026-09-01",
            ["customer_name"]    = "Bob Jones",
            ["product_category"] = "Office Supplies",
            ["item_description"] = "Ergonomic Chair",
            ["quantity"]         = "1",
            ["unit_price"]       = "180.00",
            ["total_revenue"]    = "180.00",
            ["cost_of_goods"]    = "90.00",
            ["net_earnings"]     = "90.00"
        }
    ];
    
    public async Task<List<Dictionary<string, string>>> ReadCsvAsync()
    {
        return Rows;
    }
}