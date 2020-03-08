using System.Data;

namespace Scorpio.Reporting.Pdf
{
    public static class DataTableExtensions
    {
        public static string ToHtml(this DataTable dt)
        {
            var html = "<table class=\"table table-striped table-bordered table-hover\">";
            html += "<thead><tr>";
            foreach (var col in dt.Columns)
            {
                html += $"<th scope=\"col\">{col}</th>";
            }
            html += "</tr></thead>";


            html += "<tbody><tr>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j] + "</td>";
                html += "</tr>";
            }
            html += "</tbody></table>";
            return html;
        }
    }
}
