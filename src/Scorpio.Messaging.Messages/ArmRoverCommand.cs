using Scorpio.Messaging.Abstractions;

namespace Scorpio.Messaging.Messages
{
    public class ArmRoverCommand : IntegrationEvent
    {
        public override string KeyOverride => "arm";
    }
}
