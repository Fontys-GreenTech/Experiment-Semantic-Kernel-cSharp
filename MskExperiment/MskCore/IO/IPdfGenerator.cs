using System.Threading.Tasks;

namespace MskCore.IO;

public interface IPdfGenerator
{
    public Task<string> GeneratePdfAsync(string reportTitle, string overviewText, string adviceText);
}