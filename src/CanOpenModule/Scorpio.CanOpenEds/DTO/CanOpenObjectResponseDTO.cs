using Newtonsoft.Json;

namespace Scorpio.CanOpenEds.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CanOpenObjectResponseDTO
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
        public string Name { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Include)]
        public string Label { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Include)]
        public string Index { get; set; }

        [JsonProperty("subIndex", NullValueHandling = NullValueHandling.Include)]
        public string SubIndex { get; set; }

        [JsonProperty("base64Desc", NullValueHandling = NullValueHandling.Include)]
        public string Description { get; set; }

        [JsonProperty("objectType", NullValueHandling = NullValueHandling.Include)]
        public string ObjectType { get; set; }

        [JsonProperty("dataType", NullValueHandling = NullValueHandling.Include)]
        public string DataType { get; set; }

        [JsonProperty("accessType", NullValueHandling = NullValueHandling.Include)]
        public string AccessType { get; set; }

        [JsonProperty("PDOmapping", NullValueHandling = NullValueHandling.Include)]
        public string PDOmapping { get; set; }

        [JsonProperty("defaultValue", NullValueHandling = NullValueHandling.Include)]
        public string DefaultValue { get; set; }

        [JsonProperty("highValue", NullValueHandling = NullValueHandling.Include)]
        public string HighValue { get; set; }

        [JsonProperty("lowValue", NullValueHandling = NullValueHandling.Include)]
        public string LowValue { get; set; }

        [JsonProperty("TPDOdetectCOS", NullValueHandling = NullValueHandling.Include)]
        public string TPDOdetectCOS { get; set; }
    }
}