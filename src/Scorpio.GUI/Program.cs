using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Scorpio.Gamepad.Processors;
using Scorpio.GUI.Streaming;
using Scorpio.Instrumentation.Vivotek;
using Scorpio.Messaging.Sockets;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Scorpio.GUI
{
    internal static class Program
    {
        private static ILogger<MainForm> _logger;
        private static IContainer _container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.ThreadException += Application_ThreadException;

                var builder = new ContainerBuilder();
                var services = PopulateServices(builder);
                _container = services.Build();

                #pragma warning disable 618
                SetupLogger();
                #pragma warning restore 618

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                _logger = _container.Resolve<ILogger<MainForm>>();

                Application.Run(_container.Resolve<MainForm>());
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static ContainerBuilder PopulateServices(ContainerBuilder builder)
        {
            builder.RegisterType<MainForm>().SingleInstance();

            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false)
              .Build();

            builder.RegisterInstance(config)
                .As<IConfiguration>()
                .SingleInstance()
                .ExternallyOwned();

            var vivotekConfig = new CameraConfigModel();
            config.GetSection("cameras").Bind(vivotekConfig);

            builder.RegisterInstance(vivotekConfig)
                .As<CameraConfigModel>()
                .SingleInstance()
                .ExternallyOwned();

            var socketsConfig = new SocketConfiguration();
            config.GetSection("socketClient").Bind(socketsConfig);

            builder.RegisterInstance(socketsConfig)
                .As<SocketConfiguration>()
                .SingleInstance()
                .ExternallyOwned();

            builder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

//            SetupRabbitMqConnection(builder, config);
//            SetupRabbitMqEventBus(builder, config);

            builder.AddSocketClientConnection(socketsConfig);
            builder.AddSocketClientEventBus();

            builder.RegisterGeneric(typeof(ExponentialGamepadProcessor<,>))
                .As(typeof(IGamepadProcessor<,>))
                .InstancePerDependency();

            builder.RegisterType<CyclicTimer>()
                .InstancePerDependency();

            builder.RegisterType<GStreamerLauncher>()
                .SingleInstance();

            builder.RegisterType<VivotekDomeCameraController>()
                .InstancePerDependency();

            return builder;
        }


        [Obsolete("YES I KNOW ITS OBSOLETE", false)]
        private static void SetupLogger()
        {
            var loggerFactory = _container.Resolve<ILoggerFactory>();
            loggerFactory.AddNLog(new NLogProviderOptions
            {
                CaptureMessageProperties = true,
                CaptureMessageTemplates = true
            });
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
            => HandleException(e.Exception);

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            => HandleException(e.ExceptionObject as Exception);

        private static void HandleException(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
