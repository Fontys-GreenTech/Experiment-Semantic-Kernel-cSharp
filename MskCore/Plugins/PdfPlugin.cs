using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using MskCore.IO;

namespace MskCore.Plugins;

public class PdfPlugin (IPdfGenerator pdfGenerator)
{
    [KernelFunction("generate_fiscal_report_pdf")]
    [Description("Generates a professional fiscal report PDF containing overview and advice, saves it to disk, and returns the file path.")]
    public async Task<string> GenerateFiscalReportPdf(string reportTitle, string overviewText, string adviceText)
    {
        return await pdfGenerator.GeneratePdfAsync(reportTitle, overviewText, adviceText);
    }
}
