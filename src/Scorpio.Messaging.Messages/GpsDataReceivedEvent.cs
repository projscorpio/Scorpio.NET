using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;

namespace Scorpio.Messaging.Messages
{
    public class GpsDataReceivedEvent : IntegrationEvent
    {
        public override string KeyOverride => "gps";

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
