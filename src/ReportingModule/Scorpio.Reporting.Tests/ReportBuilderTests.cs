using System;
using System.Data;
using System.Linq;
using Xunit;

namespace Scorpio.Reporting.Tests
{
    public class ReportBuilderTests
    {
        [Fact]
        public void ReportBuilder_CanSetName()
        {
            const string title = "title";

            Report report = new ReportBuilder()
                .WithTitle(title)
                .Build();

            Assert.Equal(title, report.Title);
        }

        [Fact]
        public void ReportBuilder_CanAddSectionWithTitle()
        {
            const string sectionTitle = "sectionTitle";
            const string sectionText = "text";
            const string secondSectionTitle = "22222";

            Report report = new ReportBuilder()
                .AddTextSection(options =>
                {
                    options.Title = sectionTitle;
                    options.Text = sectionText;
                    options.Order = 1;
                })
                .AddTextSection(options =>
                {
                    options.Title = secondSectionTitle;
                    options.Text = sectionText;
                    options.Order = 2;
                })
                .Build();

            Assert.NotNull(report.Sections);
            Assert.NotNull(report.Sections.FirstOrDefault());
            Assert.Equal(sectionTitle, report.Sections.First().Title);
            Assert.Equal(secondSectionTitle, report.Sections.Skip(1).First().Title);
        }

        [Fact]
        public void ReportBuilder_CanAddTableSection()
        {
            var table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("data", typeof(string));

            table.Rows.Add(new object[] { 1, "test" });

            Report report = new ReportBuilder()
                .AddTableSection(options =>
                {
                    options.Table = table;
                })
                .Build();

            Assert.NotNull(report.Sections);
            Assert.NotNull(report.Sections.FirstOrDefault());

            var tableSection = report.Sections.First() as TableSection;
            Assert.Equal(table, tableSection?.Table);
        }
    }
}
