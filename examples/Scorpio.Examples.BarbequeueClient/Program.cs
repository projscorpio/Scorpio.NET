using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using Scorpio.Messaging.Sockets;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace Scorpio.Examples.BarbequeueClient
{
    public class Program
    {
        private IContainer _container;
        private ILogger<Program> _logger;
        private ILogger<Program> Logger => _logger ??= _container.Resolve<ILogger<Program>>();

        static void Main(string[] args) => new Program().Run(args);

        private void Run(string[] args)
        {
            BuildContainer();
            var eventBus = _container.Resolve<IEventBus>();
            //eventBus.Subscribe<RoverControlCommand, RoverControlCommandHandler>();

            var socketClient = _container.Resolve<ISocketClient>();
            socketClient.TryConnect();

            double angle = 0;

            while (true)
            {
                angle += 5;
                if (angle > 350) angle = 0;
                //var msg = new RoverControlCommand(-14423.1f, 313.11312f);
                var msg = new GpsDataReceivedEvent {Latitude = 51.107883, Longitude = 17.1};
                var msg2 = new CompassDataReceivedEvent {Angle = angle};
                Logger.LogInformation("Publishing...");
                eventBus.Publish(msg);
                eventBus.Publish(msg2);
                Thread.Sleep(3000);
            }
        }

        private void BuildContainer()
        {
            var socketConf = new SocketConfiguration
            {
                //Host = "192.168.43.166",
                Host = "80.68.231.116",
                //Host = "127.0.0.1",
                Port = 2138
            };

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console();

            var builder = new ContainerBuilder()
                .RegisterSerilog(loggerConfig)
                .AddSocketClientConnection(socketConf)
                .AddSocketClientEventBus();
            builder.RegisterType<RoverControlCommandHandler>().InstancePerDependency();
            _container = builder.Build();
        }
    }

    public class RoverControlCommandHandler : IIntegrationEventHandler<RoverControlCommand>
    {
        private readonly ILogger<RoverControlCommand> _logger;

        public RoverControlCommandHandler(ILogger<RoverControlCommand> logger)
        {
            _logger = logger;
        }

        public Task Handle(RoverControlCommand @event)
        {
            _logger.LogInformation("Received:");
            _logger.LogInformation(JsonConvert.SerializeObject(@event));
            return Task.FromResult(0);
        }
    }
}
