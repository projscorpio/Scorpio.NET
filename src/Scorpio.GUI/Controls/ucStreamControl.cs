using System;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using Scorpio.GUI.Streaming;

namespace Scorpio.GUI.Controls
{
    public partial class ucStreamControl : UserControl
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

        private string _cameraId;
        public string CameraId
        {
            get
            {
                if (_cameraId is null && !DesignMode) 
                    throw new ArgumentNullException("CameraId was not provided.");
                return _cameraId;
            }
            set => _cameraId = value;
        }

        private const string STARTED = "Started!";
        private const string STOPPED = "Stopped";

        private GStreamerLauncher _gStreamer;
        private GStreamerLauncher GStreamer => _gStreamer ?? (_gStreamer = _autofac.Resolve<GStreamerLauncher>());

        private CameraConfigModel _camConfig;
        private CameraConfigModel CamConfig => _camConfig ?? (_camConfig = _autofac.Resolve<CameraConfigModel>());

        public ucStreamControl()
        {
            InitializeComponent();
            lblState.Text = STOPPED;
            lblState.BackColor = Color.Red;
            this.Load += (_, __) => lblStreamId.Text = CameraId;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            var config = CamConfig.GetStreamById(CameraId);
            if (await GStreamer.Launch(config.GstreamerArg))
            {
                lblState.Text = STARTED;
                lblState.BackColor = Color.Green;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            var config = CamConfig.GetStreamById(CameraId);
            GStreamer.Stop(config.GstreamerArg);

            lblState.Text = STOPPED;
            lblState.BackColor = Color.Red;
        }
    }
}
