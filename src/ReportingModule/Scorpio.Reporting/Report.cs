using System.Collections.Generic;

namespace Scorpio.Reporting
{
    public class Report
    {
        public string Title { get; set; }
        public ICollection<Section> Sections { get; set; }

        public Report()
        {
            Sections = new List<Section>();
        }
    }
}