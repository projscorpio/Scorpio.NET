using System;
using System.Linq;

namespace Scorpio.Reporting
{
    public class ReportBuilder : IReportBuilder
    {
        private readonly Report _report;

        public ReportBuilder()
        {
            _report = new Report();
        }

        public IReportBuilder WithTitle(string title)
        {
            _report.Title = title;
            return this;
        }

        public IReportBuilder AddTextSection(Action<TextSection> options)
        {
            var section = new TextSection();
            options.Invoke(section);
            _report.Sections.Add(section);
            return this;
        }


        public IReportBuilder AddTableSection(Action<TableSection> options)
        {
            var section = new TableSection();
            options.Invoke(section);
            _report.Sections.Add(section);
            return this;
        }

        public Report Build()
        {
            _report.Sections = _report.Sections
                .OrderBy(x => x.Order)
                .ToList();

            return _report;
        }
    }
}