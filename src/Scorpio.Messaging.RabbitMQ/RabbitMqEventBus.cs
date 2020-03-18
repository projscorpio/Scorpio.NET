using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Scorpio.Messaging.Abstractions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scorpio.Messaging.RabbitMQ
{
    public class RabbitConfig
    {
        public string MyQueueName { get; set; }
        public string ExchangeName { get; set; }
        public string MessageTimeToLive { get; set; }
    }

    public class RabbitMqEventBus : IEventBus, IDisposable
    {
        private readonly string _exchangeName;
        private readonly IEventBusSubscriptionManager _subsManager;
        private readonly ILogger<RabbitMqEventBus> _logger;
        private readonly ILifetimeScope _autofac;
        private readonly RabbitConfig _config;
        private readonly IRabbitMqConnection _persistentConnection;
        private readonly string _queueName;
        private IModel _consumerChannel;
        protected IModel ConsumerChannel
        {
            get => _consumerChannel ?? (_consumerChannel = CreateConsumerChannel());
            private set => _consumerChannel = value;
        }

        public RabbitMqEventBus(IRabbitMqConnection persistentConnection, ILogger<RabbitMqEventBus> logger,
            ILifetimeScope autofac, IEventBusSubscriptionManager subsManager, RabbitConfig config)
        {
            _queueName = config.MyQueueName ?? throw new ArgumentNullException(nameof(_queueName));
            _exchangeName = config.ExchangeName ?? throw new ArgumentNullException(nameof(_exchangeName));
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _config = config;
            _autofac = autofac;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new GenericEventBusSubscriptionManager();
        }

        public void Publish(IntegrationEvent @event)
        {
            var routingKey = @event.GetType().Name;

            if (!_persistentConnection.IsConnected)
            {
                _logger.LogError("Trying to send message, but disconnected, please connect first");
                return;
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                var props = ConfigureChannel(channel);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: props, body: body);
            }
        }

        private IBasicProperties ConfigureChannel(IModel channel)
        {
            var expiration = _config.MessageTimeToLive ?? throw new ArgumentException("RabbitMq:messageTTL");
            var props = channel.CreateBasicProperties();
            props.DeliveryMode = 1; // non persistent
            props.Expiration = expiration; // ms TTL
            props.ContentType = "application/json";
            props.ContentEncoding = "UTF8";
            return props;
        }

        public void Subscribe<TEvent, THandler>(string keyOverride = null)
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventName = string.IsNullOrWhiteSpace(keyOverride) ? _subsManager.GetEventKey<TEvent>() : keyOverride;

            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                ConsumerChannel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: eventName);
                
                var log = $"RabbitMQ bound routingKey: {eventName} to queue: {_queueName} using exchange {_exchangeName}";
                _logger.LogInformation(log);
            }
            _subsManager.AddSubscription<TEvent, THandler>(eventName);
        }

        public void Unsubscribe<TEvent, THandler>(string keyOverride = null)
            where TEvent : IIntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            _subsManager.RemoveSubscription<TEvent, THandler>();
        }

        public void Dispose()
        {
            ConsumerChannel?.Dispose();
            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(_queueName, false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                await ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(_queueName, false, consumer);

            channel.CallbackException += (sender, ea) =>
            {
                ConsumerChannel.Dispose();
                ConsumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            using (var scope = _autofac.BeginLifetimeScope(_exchangeName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ResolveOptional(subscription.HandlerType);
                    if (handler is null) continue;

                    var eventType = _subsManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType
                        .GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.Handle))
                        ?.Invoke(handler, new[] { integrationEvent });
                }
            }
        }
    }
}
