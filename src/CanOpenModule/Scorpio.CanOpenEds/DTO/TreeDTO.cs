using System.Collections.Generic;

namespace Scorpio.CanOpenEds.DTO
{
    public class TreeDTO
    {
        public ICollection<TreeItemDTO> Items { get; set; }
        public int Count { get; set; }
    }
}
