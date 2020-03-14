using Microsoft.AspNetCore.Mvc;
using Scorpio.Reporting;
using Scorpio.Reporting.Pdf;
using System;
using System.Data;

namespace Scorpio.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReportController : Controller
    {
        private readonly PdfCreator _pdfCreator;
        private readonly IReportBuilder _reportBuilder;

        public ReportController(IReportBuilder reportBuilder, PdfCreator pdfCreator)
        {
            _pdfCreator = pdfCreator;
            _reportBuilder = reportBuilder;
        }

        [HttpGet]
        public IActionResult Create()
        {
            // tests
            var table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("data", typeof(string));

            table.Rows.Add(1, "test");
            table.Rows.Add(12, "aaaa");
            table.Rows.Add(13, "test");
            table.Rows.Add(12333, "dadadad");

            var report = _reportBuilder.WithTitle("Scorpio Science Report")
                .AddTextSection(options =>
                {
                    options.Title = "Introduction";
                    options.Text = "This is team Scorpio Science report.";
                })
                .AddTableSection(options =>
                {
                    options.Title = "Example table";
                    options.Table = table;
                })
                .AddTextSection(options =>
                {
                    options.Title = "Something";
                    options.Text = "This is team Scorpio Science report.";
                })
                .AddTableSection(options =>
                {
                    options.Title = "Example table";
                    options.Table = table;
                })
                .Build();

            var stream = _pdfCreator.ConvertReportToStream(report);
            string fileName = $"report_{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}.pdf";
            return File(stream, "application/pdf", fileName);
        }
    }
}