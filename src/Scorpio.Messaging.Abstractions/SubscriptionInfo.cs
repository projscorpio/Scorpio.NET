using System;

namespace Scorpio.Messaging.Abstractions
{
    public class SubscriptionInfo
    {
        public Type EventType { get; }
        public Type HandlerType { get; }

        public SubscriptionInfo(Type eventType, Type handlerType)
        {
            EventType = eventType;
            HandlerType = handlerType;
        }
    }
}
