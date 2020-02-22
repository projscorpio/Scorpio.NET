using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scorpio.Messaging.Sockets
{
    public class SocketEventBus : IEventBus
    {
        private readonly ISocketClient _socketClient;
        private readonly ILogger<SocketEventBus> _logger;
        private readonly IEventBusSubscriptionManager _busSubscriptionManager;
        private readonly ILifetimeScope _autofac;
        private readonly IList<string> _subscriptionKeys;

        public SocketEventBus(ISocketClient socketClient,
            ILogger<SocketEventBus> logger,
            IEventBusSubscriptionManager busSubscriptionManager,
            ILifetimeScope autofac)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _busSubscriptionManager = busSubscriptionManager ?? throw new ArgumentNullException(nameof(busSubscriptionManager));
            _autofac = autofac ?? throw new ArgumentNullException(nameof(autofac));
            _subscriptionKeys = new List<string>();

            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _socketClient.MessageReceived += async (s, e) => await ProcessEvent(e?.Envelope);
            _socketClient.Connected += (s, e) => SendUpdatedSubscriptionList();
        }
        
        public void Publish(IntegrationEvent @event)
        {
            try
            {
                _socketClient.Enqueue(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lost connection while writing message: " + ex.Message, ex);
            }
        }

        private async Task ProcessEvent(Envelope envelope)
        {
            if (envelope is null)
                return;
            
            if (!_busSubscriptionManager.HasSubscriptionsForEvent(envelope.Key))
                return;

            using (var scope = _autofac.BeginLifetimeScope())
            {
                var subscriptions = _busSubscriptionManager.GetHandlersForEvent(envelope.Key);
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler is null) continue;

                    var eventType = _busSubscriptionManager.GetEventTypeByName(envelope.Key);
                    var integrationEvent = JsonConvert.DeserializeObject(envelope.Data?.ToString(), eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    
                    // ReSharper disable once PossibleNullReferenceException
                    await (Task)concreteType
                        .GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.Handle))
                        ?.Invoke(handler, new[] { integrationEvent });
                }
            }
        }

        public void Subscribe<TEvent, THandler>(string keyOverride = null)
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = string.IsNullOrWhiteSpace(keyOverride) ? _busSubscriptionManager.GetEventKey<TEvent>() : keyOverride;
            _logger.LogInformation($"Socket subscription manager: subscribed to: {eventName}");
            _busSubscriptionManager.AddSubscription<TEvent, THandler>();
            _subscriptionKeys.Add(eventName);
        }

        public void Unsubscribe<TEvent, THandler>(string keyOverride = null)
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {

            var eventName = string.IsNullOrWhiteSpace(keyOverride) ? _busSubscriptionManager.GetEventKey<TEvent>() : keyOverride;
            _logger.LogInformation($"Socket subscription manager: unsubscribed from: {eventName}");
            _busSubscriptionManager.RemoveSubscription<TEvent, THandler>();
            _subscriptionKeys.Remove(eventName);
        }

        private void SendUpdatedSubscriptionList()
        {
            if (_subscriptionKeys.Any())
            {
                var advertiseCommand = new AdvertiseCommand(_subscriptionKeys);
                Publish(advertiseCommand);
            }
        }
    }
}
