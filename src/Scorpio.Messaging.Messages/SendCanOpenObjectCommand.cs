using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;

namespace Scorpio.Messaging.Messages
{
    public class SendCanOpenObjectCommand : IntegrationEvent
    {
        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("subIndex")]
        public string SubIndex { get; set; }

        [JsonProperty("dataType")]
        public string DataType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
