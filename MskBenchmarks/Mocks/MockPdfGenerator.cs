using MskCore.IO;

namespace MskBenchmarks.Mocks;

public class MockPdfGenerator : IPdfGenerator
{
    public Task<string> GeneratePdfAsync(string reportTitle, string overviewText, string adviceText)
    {
        return Task.FromResult("generated!");
    }
}