using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scorpio.Gamepad.IO;
using Scorpio.Gamepad.IO.Args;
using Scorpio.Gamepad.Processors;
using Scorpio.Gamepad.Processors.Mixing;
using Scorpio.GUI.Utils;
using Scorpio.Messaging.Abstractions;
using Scorpio.Messaging.Messages;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scorpio.Gamepad.Models;
using Scorpio.Instrumentation.Vivotek;
using Scorpio.Instrumentation.Vivotek.DomeCamera;

namespace Scorpio.GUI.Controls
{
    public partial class ucRoverGamepad : UserControl
    {
        public event EventHandler<bool> StateChanged;

        private ILifetimeScope _autofac;
        public ILifetimeScope Autofac
        {
            get
            {
                if (_autofac is null && !DesignMode)
                    throw new ArgumentNullException("Autofac was not initialized in this control.");
                return _autofac;
            }
            set => _autofac = value;
        }

        private GamepadPoller _poller;
        private IGamepadProcessor<RoverMixer, RoverProcessorResult> _gamepadProcessor;
        private int _pollerThreadSleepTime = 40; // default 40
        private ILogger<ucRoverGamepad> _logger;
        private int _gamepadIndex;
        private RoverProcessorResult _latestResult;
        private GamepadModel _lastGamepadModel;
        private CyclicTimer _timer;
        private IEventBus _eventBus;
        private bool _enableSending;
        private int _accelerationLimit = 200;

        #region Vivotek section
        private string _vivotekId;
        public string VivotekId
        {
            get
            {
                if (_vivotekId is null && !DesignMode)
                    throw new ArgumentNullException("VivotekId was not provided.");
                return _vivotekId;
            }
            set => _vivotekId = value;
        }

        private VivotekModel _camConfig;
        private VivotekModel Config => _camConfig ?? (_camConfig = _autofac.Resolve<CameraConfigModel>().GetVivotekById(VivotekId));

        private VivotekDomeCameraController _controller;
        private VivotekDomeCameraController Controller
        {
            get
            {
                if (_controller != null)
                    return _controller;

                _controller = _autofac.Resolve<VivotekDomeCameraController>();
                _controller.Credentials = new NetworkCredential(Config.Username, Config.Password);
                _controller.ApiUrl = Config.BaseApiUrl;

                return _controller;
            }
        }
        #endregion

        public ucRoverGamepad()
        {
            InitializeComponent();
            SetStateStopped();
            InitializeIndexComboBox();
        }

        private void InitializeIndexComboBox()
        {
            cbGamepadIndex.DropDownStyle = ComboBoxStyle.DropDownList; // Disable free-text input
            cbGamepadIndex.DataSource = Enumerable.Range(0, 4).ToList();
            cbGamepadIndex.SelectedIndex = 0;
            cbGamepadIndex.SelectedIndexChanged += CbGamepadIndex_SelectedIndexChanged;
        }

        private void CbGamepadIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            _gamepadIndex = int.Parse(((ComboBox)sender).Text);
            btnStop_Click(this, EventArgs.Empty);
            btnStart_Click(this, EventArgs.Empty);
        }

        public void Setup(ILifetimeScope autofac)
        {
            Autofac = autofac;
            _pollerThreadSleepTime = Autofac.Resolve<IConfiguration>().GetValue<int>("gamepadUpdateFrequency");
            _gamepadProcessor = Autofac.Resolve<IGamepadProcessor<RoverMixer, RoverProcessorResult>>();
            _logger = Autofac.Resolve<ILogger<ucRoverGamepad>>();
            _timer = Autofac.Resolve<CyclicTimer>();
            _eventBus = Autofac.Resolve<IEventBus>();
            _timer.ElapsedAction = TimerElapsedAction;
            lblAcc.Text = string.Empty;
            lblDir.Text = string.Empty;
            pbAcc.Minimum = 0;
            pbAcc.Maximum = 1600;
            pbDir.Minimum = 0;
            pbDir.Maximum = 200;

            SetupGamepadPoller();
        }

        private void _poller_GamepadStateChanged(object sender, GamepadEventArgs e)
        {
            if (e.Gamepad.RightThumbStick.Vertical >= short.MaxValue-500 
                && _lastGamepadModel.RightThumbStick.Vertical < e.Gamepad.RightThumbStick.Vertical)
            {
                Task.Factory.StartNew(async () => await Controller.Control(CameraCommand.ZoomIn));
            }

            if (e.Gamepad.RightThumbStick.Vertical <= -(short.MaxValue - 500)
                && _lastGamepadModel.RightThumbStick.Vertical > e.Gamepad.RightThumbStick.Vertical)
            {
                Task.Factory.StartNew(async () => await Controller.Control(CameraCommand.ZoomOut));
            }

            _latestResult = _gamepadProcessor.Process(e.Gamepad);
            _latestResult.Acceleration = ScalingUtils.SymmetricalConstrain(_latestResult.Acceleration, _accelerationLimit);
            _lastGamepadModel = e.Gamepad;
            UpdateResultWidgets(_latestResult);
        }

        private void UpdateResultWidgets(RoverProcessorResult result) =>
            this.Invoke(() =>
            {
                // -200 200 rot hack (progress bar would crash over -100:100 range)
                // so limit it to -100:100
                var limitedRot = ScalingUtils.SymmetricalConstrain((int)(result.Direction * 1000), 100);
                var acceleration = (int)result.Acceleration + 800;

                lblAcc.Text = result.Acceleration.ToString("0.##");
                lblDir.Text = result.Direction.ToString("0.##");
                pbAcc.SetProgressNoAnimation(acceleration); // Progress bar has range 0-1600, shift + 800
                pbDir.SetProgressNoAnimation((int)limitedRot + 100); // Progress bar has range 0-200, shift + 100
            });

        private void btnStart_Click(object sender, EventArgs ev)
        {
            if (_enableSending)
            {
                _logger.LogWarning("Already armed!");
                return;
            }

            // Start publishing messages
            _timer.Start(20); // send message every x ms
            SetStateStarted();
            _enableSending = true;
            _logger.LogInformation($"Rover gamepad started with index: {_gamepadIndex}");
            StateChanged?.Invoke(this, true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Already started
            if (!_enableSending)
            {
                _logger.LogDebug("Already disarmed!");
                return;
            }

            _timer.Stop();
            _enableSending = false;
            SetStateStopped();
            StateChanged?.Invoke(this, false);
        }

        private void SetupGamepadPoller()
        {
            // Start gamepad poller
            _poller = new GamepadPoller(_gamepadIndex, _pollerThreadSleepTime);
            _poller.GamepadStateChanged += _poller_GamepadStateChanged;
            _poller.DPadLeftChanged += async (_, isPressed) => { if (isPressed) await Controller.Control(CameraCommand.Left); };
            _poller.DPadRightChanged += async (_, isPressed) => { if (isPressed) await Controller.Control(CameraCommand.Right); };
            _poller.DPadUpChanged += async (_, isPressed) => { if (isPressed) await Controller.Control(CameraCommand.Up); };
            _poller.DPadDownChanged += async (_, isPressed) => { if (isPressed) await Controller.Control(CameraCommand.Down); };
            _poller.RightThumbStickPressedChanged += async (_, isPressed) => {  if(isPressed) await Controller.Control(CameraCommand.Home); };
        }

        private void TimerElapsedAction()
        {
            if (!_enableSending || _latestResult is null) return;

            var msg = new RoverControlCommand(_latestResult.Direction, _latestResult.Acceleration);
            _eventBus?.Publish(msg);
        }

        private void SetStateStarted() =>
            this.Invoke(() =>
            {
                lblState.Text = "Started";
                lblState.ForeColor = Color.Green;
            });

        private void SetStateStopped() => 
            this.Invoke(() =>
                {
                    lblState.Text = "Stopped";
                    lblState.ForeColor = Color.Red;

                    lblAcc.Text = string.Empty;
                    lblDir.Text = string.Empty;
                    pbAcc.SetProgressNoAnimation(0);
                    pbDir.SetProgressNoAnimation(0);
                });
        

        private void Invoke(Action action)
        {
            if (IsHandleCreated)
                base.BeginInvoke(action);
        }

        private void tbAccLimit_Scroll(object sender, EventArgs e)
        {
            var value = ((TrackBar) sender).Value;
            lblLimit.Text = value.ToString();
            _accelerationLimit = value;
        }
    }
}
