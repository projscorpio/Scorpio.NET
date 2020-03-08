using System;

namespace Scorpio.Reporting
{
    public interface IReportBuilder
    {
        IReportBuilder WithTitle(string title);
        IReportBuilder AddTextSection(Action<TextSection> options);
        IReportBuilder AddTableSection(Action<TableSection> options);
        Report Build();
    }
}
