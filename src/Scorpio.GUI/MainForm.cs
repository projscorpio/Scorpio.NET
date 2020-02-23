using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Windows.Forms;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using Scorpio.Messaging.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scorpio.GUI
{
    public partial class MainForm : Form
    {
        private readonly ILifetimeScope _autofac;
        private readonly ILogger<MainForm> _logger;
        private readonly IEventBus _eventBus;

        private ISocketClient _socketClient;
        private ISocketClient SocketClient => _socketClient ?? (_socketClient = _autofac.Resolve<ISocketClient>());

        public MainForm(ILifetimeScope iocFactory, ILogger<MainForm> logger)
        {
            InitializeComponent();

            _autofac = iocFactory;
            _eventBus = _autofac.Resolve<IEventBus>();
            _socketClient = _autofac.Resolve<ISocketClient>();
            _logger = logger;

            SetupStreamControl();
            SetupGamepadControls();
            SetupMessaging();

            this.AutoScaleMode = AutoScaleMode.Dpi;
            base.Load += (_, __) => RichTextBoxTarget.ReInitializeAllTextboxes(this); // Refresh NLog RichTextBox
        }

        private void SetupStreamControl()
        {
            // Maybe build this dynamically basing on config?
            ucStreamControl1.Autofac = _autofac.Resolve<ILifetimeScope>();
            ucStreamControl2.Autofac = _autofac.Resolve<ILifetimeScope>();
            ucStreamControl3.Autofac = _autofac.Resolve<ILifetimeScope>();
            ucStreamControl4.Autofac = _autofac.Resolve<ILifetimeScope>();
            ucStreamControl1.CameraId = "cam1";
            ucStreamControl2.CameraId = "cam2";
            ucStreamControl3.CameraId = "cam3";
            ucStreamControl4.CameraId = "cam4";

            ucVivotekController1.Autofac = _autofac.Resolve<ILifetimeScope>();
            ucVivotekController1.VivotekId = "vivotek1";
            ucRoverGamepad1.VivotekId = "vivotek1";
        }

        private void SetupGamepadControls()
        {
            ucRoverGamepad1.Setup(_autofac.Resolve<ILifetimeScope>());
        }

        private void SetupMessaging()
        { 
            SocketClient.Connected += (_, __) => _logger.LogInformation("Socket connected!");
            SocketClient.Disconnected += (_, __) => _logger.LogWarning("Socket disconnected");

            _eventBus.Subscribe<RoverControlCommand, RoverControlCommandEventHandler>("override");
        }

        private CancellationTokenSource _cts;
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // we are not connected
                if (_cts is null)
                {
                    _logger.LogInformation("New connection was requested...");
                    _cts = new CancellationTokenSource();
                    Task.Factory.StartNew(() => _socketClient.TryConnect(_cts.Token), _cts.Token);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (_socketClient.IsConnected)
                {
                    _socketClient.Disconnect();
                    _logger.LogInformation("Successfully disconnected!");
                }
                else // Still connecting - cancel the task
                {
                    if (_cts is null)
                    {
                        _logger.LogWarning("You are not connected!");
                        return;
                    }
                    _cts.Cancel(true);
                }

                _cts = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }

    class RoverControlCommandEventHandler : IIntegrationEventHandler<RoverControlCommand>
    {
        private readonly ILogger<RoverControlCommandEventHandler> _logger;

        public RoverControlCommandEventHandler(ILogger<RoverControlCommandEventHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(RoverControlCommand @event)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(@event));
            return Task.FromResult(0);
        }
    }
}
