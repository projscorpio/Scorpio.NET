using Autofac;
using Microsoft.Extensions.Hosting;
using Scorpio.Api.EventHandlers;
using Scorpio.Api.Events;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using Scorpio.Messaging.RabbitMQ;
using System;
using System.Threading;
using System.Threading.Tasks;
using Scorpio.Messaging.Sockets;

namespace Scorpio.Api.HostedServices
{
    public class EventBusHostedService : IHostedService, IDisposable
    {
        private readonly ILifetimeScope _autofac;
        private IEventBus _eventBus;
        private ISocketClient _socketClient;

        public EventBusHostedService(ILifetimeScope autofac)
        {
            _autofac = autofac;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // RabbitMQ connection requires connecting - this can be refactored later
            var rabbitMqConnection = _autofac.ResolveOptional<IRabbitMqConnection>();
            if (rabbitMqConnection != null) // rabbit is enabled
                rabbitMqConnection.TryConnect();

            try
            {
                _eventBus = _autofac.Resolve<IEventBus>();

                _eventBus.Subscribe<SaveSensorDataEvent, SaveSensorDataEventHandler>();
                _eventBus.Subscribe<SaveManySensorDataEvent, SaveManySensorDataEventHandler>();
                _eventBus.Subscribe<UbiquitiDataReceivedEvent, UbiquitiDataReceivedEventHandler>();
                _eventBus.Subscribe<GpsDataReceivedEvent, GpsDataReceivedEventHandler>("gps");

                _socketClient = _autofac.Resolve<ISocketClient>();
                _socketClient.TryConnect(cancellationToken);
            }
            catch (OperationCanceledException) { }

            return Task.FromResult(0);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _socketClient?.Disconnect();
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            _socketClient?.Disconnect();
            _socketClient?.Dispose();
        }
    }
}
