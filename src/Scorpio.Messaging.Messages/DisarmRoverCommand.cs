using Scorpio.Messaging.Abstractions;

namespace Scorpio.Messaging.Messages
{
    public class DisarmRoverCommand : IntegrationEvent
    {
        public override string KeyOverride => "disarm";
    }
}
