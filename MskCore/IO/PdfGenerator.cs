using System;
using System.IO;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MskCore.IO;

public class PdfGenerator : IPdfGenerator
{
    static PdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }
    
    public async Task<string> GeneratePdfAsync(string reportTitle, string overviewText, string adviceText)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fiscal_report.pdf");

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Lato").FontColor(Colors.Grey.Darken3));

                page.Header()
                    .Text(reportTitle)
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Spacing(15);

                        col.Item().Text("1. Financieel Overzicht").Bold().FontSize(14).FontColor(Colors.Grey.Darken4);
                        col.Item().Text(overviewText);

                        col.Item().PaddingTop(10).Text("2. Fiscaal Advies").Bold().FontSize(14).FontColor(Colors.Grey.Darken4);
                        col.Item().Text(adviceText);
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Fiscaal Rapport - Pagina ");
                        x.CurrentPageNumber();
                        x.Span(" van ");
                        x.TotalPages();
                    });
            });
        })
        .GeneratePdf(filePath);

        return await Task.FromResult($"PDF-rapport succesvol gegenereerd en opgeslagen op: {filePath}");
    }
}