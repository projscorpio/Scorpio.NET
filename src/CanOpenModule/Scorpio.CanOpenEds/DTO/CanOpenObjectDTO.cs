using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scorpio.CanOpenEds.DTO
{
    public class CanOpenObjectDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("base64Desc")]
        public string Description { get; set; }

        [JsonProperty("CANopenSubObject")]
        public ICollection<CanOpenSubObjectDTO> SubObjects { get; set; }
    }
}