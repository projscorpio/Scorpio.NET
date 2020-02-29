using Newtonsoft.Json;
using System.Collections.Generic;

namespace Scorpio.CanOpenEds.DTO
{
    public class CanOpenObjectRootDTO
    {
        [JsonProperty("canOpenObjects")]
        public ICollection<CanOpenObjectDTO> CanOpenObjects { get; set; }
    }
}
