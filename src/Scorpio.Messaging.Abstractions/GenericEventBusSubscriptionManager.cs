using System;
using System.Collections.Generic;
using System.Linq;

namespace Scorpio.Messaging.Abstractions
{
    public class GenericEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;

        public GenericEventBusSubscriptionManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();

        public void AddSubscription<T, TH>(string key = null) where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = string.IsNullOrWhiteSpace(key) ? GetEventKey<T>() : key;

            DoAddSubscription(typeof(TH), typeof(T), eventName);
        }

        private void DoAddSubscription(Type handlerType, Type eventType, string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(new SubscriptionInfo(eventType, handlerType));
        }

        public void RemoveSubscription<TEvent, THandler>() where THandler : IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
        {
            var handlerToRemove = FindSubscriptionToRemove<TEvent, THandler>();
            var eventName = GetEventKey<TEvent>();
            DoRemoveHandler(eventName, handlerToRemove);
        }

        private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove is null) return;
            _handlers[eventName].Remove(subsToRemove);

            if (_handlers[eventName].Any()) return;
            _handlers.Remove(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<TEvent>() where TEvent : IIntegrationEvent
        {
            var key = GetEventKey<TEvent>();
            return GetHandlersForEvent(key);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        private SubscriptionInfo FindSubscriptionToRemove<TEvent, THandler>() where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            return DoFindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public bool HasSubscriptionsForEvent<TEvent>() where TEvent : IIntegrationEvent
        {
            var key = GetEventKey<TEvent>();
            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public Type GetEventTypeByName(string eventName) => _handlers[eventName].FirstOrDefault()?.EventType;

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }
    }
}
