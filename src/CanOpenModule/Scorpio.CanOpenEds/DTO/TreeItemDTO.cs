using System.Collections.Generic;

namespace Scorpio.CanOpenEds.DTO
{
    public class TreeItemDTO
    {
        public string Name { get; set; }
        public string Index { get; set; }
        public ICollection<TreeSubItemDTO> SubItems { get; set; }
    }

    public class TreeSubItemDTO
    {
        public string Name { get; set; }
        public string Index { get; set; }
    }
}
