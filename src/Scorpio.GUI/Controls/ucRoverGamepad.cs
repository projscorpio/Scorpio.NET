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
using System.Windows.Forms;

namespace Scorpio.GUI.Controls
{
    public partial class ucRoverGamepad : UserControl
    {
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

        private bool _isStarted;
        private GamepadPoller _poller;
        private IGamepadProcessor<RoverMixer, RoverProcessorResult> _gamepadProcessor;
        private int _pollerThreadSleepTime = 40; // default 40
        private ILogger<ucRoverGamepad> _logger;
        private int _gamepadIndex;
        private RoverProcessorResult _latestResult;
        private CyclicTimer _timer;
        private IEventBus _eventBus;

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
            pbAcc.Maximum = 200;
            pbDir.Minimum = 0;
            pbDir.Maximum = 200;
        }

        private void _poller_GamepadStateChanged(object sender, GamepadEventArgs e)
        {
            _latestResult = _gamepadProcessor.Process(e.Gamepad);
            UpdateResultWidgets(_latestResult);
        }

        private void UpdateResultWidgets(RoverProcessorResult result)
        {
            Invoke(new Action(() =>
            {
                // -200 200 rot hack (progress bar would crash over -100:100 range)
                // so limit it to -100:100
                var limitedRot = ScalingUtils.SymmetricalConstrain((int)(result.Direction * 100), 100);

                lblAcc.Text = result.Acceleration.ToString("0.##");
                lblDir.Text = result.Direction.ToString("0.##");
                //pbAcc.SetProgressNoAnimation((int)result.Acceleration + 100); // Progress bar has range 0-200, shift + 100
                pbDir.SetProgressNoAnimation((int)limitedRot + 100); // Progress bar has range 0-200, shift + 100
            }));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_isStarted)
            {
                _logger.LogWarning("Already started!");
                return;
            }

            // Start gamepad poller
            _poller = new GamepadPoller(_gamepadIndex, _pollerThreadSleepTime);
            _poller.GamepadStateChanged += _poller_GamepadStateChanged;
            _poller.StartPolling();

            // Start publishing messages
            _timer.Start(20); // send message every 50 ms

            _logger.LogInformation($"Rover gamepad started with index: {_gamepadIndex}");
            SetStateStarted();
        }

        private void TimerElapsedAction()
        {
            if (_latestResult is null) _latestResult = new RoverProcessorResult();

            var msg = new RoverControlCommand(_latestResult.Direction, _latestResult.Acceleration * 8.0f);
            _eventBus?.Publish(msg);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_isStarted == false) return;

            // Stop publishing to rabbit mq
            _timer.Stop();

            // Stop gamepad polling
            _poller.StopPolling();
            _poller.GamepadStateChanged -= _poller_GamepadStateChanged;

            _logger.LogInformation("Rover gamepad stopped");
            SetStateStopped();
        }

        private void SetStateStarted()
        {
            lblState.Text = "Started";
            lblState.ForeColor = Color.Green;

            _isStarted = true;
        }

        private void SetStateStopped()
        {
            lblState.Text = "Stopped";
            lblState.ForeColor = Color.Red;

            lblAcc.Text = string.Empty;
            lblDir.Text = string.Empty;
            pbAcc.SetProgressNoAnimation(0);
            pbDir.SetProgressNoAnimation(0);

            _isStarted = false;
        }
    }
}
