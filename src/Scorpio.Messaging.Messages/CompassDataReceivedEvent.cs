using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;

namespace Scorpio.Messaging.Messages
{
    public class CompassDataReceivedEvent : IntegrationEvent
    {
        public override string KeyOverride => "compass";

        [JsonProperty("angle")]
        public double Angle { get; set; }
    }
}
