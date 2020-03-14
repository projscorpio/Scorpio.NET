using IronPdf;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Scorpio.Reporting.Pdf
{
    public class PdfCreator
    {
        private readonly HtmlToPdf _renderer;

        public PdfCreator()
        {
            _renderer = new HtmlToPdf();
        }

        public Stream ConvertReportToStream(Report report)
        {
            if (report is null)
                throw new ArgumentNullException(nameof(report), "Report cannot be null");

            SetupRenderer();

            var builder = new HtmlBuilder();
            var html = builder.BuildFromReport(report);

            var document = _renderer.RenderHtmlAsPdf(html);

            return document.Stream;
        }

        private void SetupRenderer()
        {
            // Iron-PDF automatically install its required deps on linux
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                IronPdf.Installation.TempFolderPath = "/home/jetson/temp";
            }

            _renderer.PrintOptions.Header = new SimpleHeaderFooter
            {
                LeftText = "Scorpio Report",
                RightText = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
                DrawDividerLine = true,
                FontSize = 14
            };

            _renderer.PrintOptions.Footer = new SimpleHeaderFooter
            {
                RightText = "{page} / {total-pages}",
                DrawDividerLine = true,
                FontSize = 12
            };

            _renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
            _renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            _renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Portrait;
        }
    }
}
