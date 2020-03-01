using Newtonsoft.Json;

namespace Scorpio.CanOpenEds.DTO
{
    public class CanOpenSubObjectDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subIndex")]
        public string SubIndex { get; set; }

        [JsonProperty("base64Desc")]
        public string Description { get; set; }

        [JsonProperty("objectType")]
        public string ObjectType { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("accessType")]
        public string AccessType { get; set; }

        [JsonProperty("PDOmapping")]
        public string PDOmapping { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [JsonProperty("highValue")]
        public string HighValue { get; set; }

        [JsonProperty("lowValue")]
        public string LowValue { get; set; }

        [JsonProperty("TPDOdetectCOS")]
        public string TPDOdetectCOS { get; set; }
    }
}