using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Windows.Forms;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using Scorpio.Messaging.Sockets;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

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
            cbLogLevel.SelectedIndex = 0;
            ucRoverGamepad1.StateChanged += (_, e) =>toolStripStatusLabel2.Text = $"Rover gamepad: {(e ? "Started!" : "Stopped")}";
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
            SocketClient.Connected += (_, __) =>
            {;
                _logger.LogInformation("Socket client: connected!");
                toolStripStatusLabel1.Text = "Socket client: connected!";
            };
            SocketClient.Disconnected += (_, __) =>
            {
                _logger.LogWarning("Socket disconnected");
                toolStripStatusLabel1.Text = "Socket client: disconnected!";
            };
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
                _cts?.Cancel(true);
                _socketClient?.Disconnect();
                _cts?.Dispose();
                _logger.LogInformation("Successfully disconnected!");
                _cts = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private void btnClearLogs_Click(object sender, EventArgs e) => logbox.Clear();

        private void cbLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            var level = ((ComboBox)sender).Text;
            var rule = LogManager.Configuration.LoggingRules.FirstOrDefault();
            rule.DisableLoggingForLevels(NLog.LogLevel.Trace, NLog.LogLevel.Fatal);
            rule.EnableLoggingForLevels(NLog.LogLevel.FromString(level), NLog.LogLevel.Fatal);
            LogManager.ReconfigExistingLoggers();
        }
    }
}
