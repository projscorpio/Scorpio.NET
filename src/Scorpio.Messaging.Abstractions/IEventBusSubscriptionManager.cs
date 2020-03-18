using System;
using System.Collections.Generic;

namespace Scorpio.Messaging.Abstractions
{
    public interface IEventBusSubscriptionManager
    {
        void AddSubscription<TEvent, THandler>(string key) where TEvent : IIntegrationEvent where THandler : IIntegrationEventHandler<TEvent>;
        void RemoveSubscription<TEvent, THandler>() where THandler : IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent;
        bool HasSubscriptionsForEvent<TEvent>() where TEvent : IIntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : IIntegrationEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<TEvent>();
    }
}
