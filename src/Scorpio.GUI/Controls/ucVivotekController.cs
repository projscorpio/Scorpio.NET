using Autofac;
using Scorpio.Instrumentation.Vivotek;
using Scorpio.Instrumentation.Vivotek.DomeCamera;
using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace Scorpio.GUI.Controls
{
    public partial class ucVivotekController : UserControl
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

        public ucVivotekController()
        {
            InitializeComponent();
            InitializeSpeedComboBox(cbPanSpeed);
            InitializeSpeedComboBox(cbTiltSpeed);
            InitializeSpeedComboBox(cbZoomSpeed);
        }

        private void InitializeSpeedComboBox(ComboBox cb)
        {
            var items = Enumerable.Range(-5, 11).ToList(); // range from -5 to 5
            cb.DataSource = items; // bind options
            cb.SelectedIndex = 7; // default 2
            cb.DropDownStyle = ComboBoxStyle.DropDownList; // Disable free-text input

            cb.SelectedIndexChanged += Cb_SelectedIndexChanged;
        }

        private async void Cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            var speed = sbyte.Parse(cb.Text);
            CameraSpeedCommand cmd;

            switch (cb.Name)
            {
                case "cbPanSpeed":
                    cmd = CameraSpeedCommand.PanSpeed;
                    break;

                case "cbTiltSpeed":
                    cmd = CameraSpeedCommand.TiltSpeed;
                    break;

                case "cbZoomSpeed":
                    cmd = CameraSpeedCommand.ZoomSpeed;
                    break;

                default: throw new NotImplementedException();
            }

            await Controller.SetSpeed(cmd, speed);
        }

        // Proper solution: derive from button, add CameraCommand there and use common event handler or at least do switch as above
        private async void btnCenter_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Home);

        private async void btnLeft_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Left);

        private async void btnRight_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Right);

        private async void btnDown_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Down);

        private async void btnUp_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Up);

        private async void btnFocusFar_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.FocusFar);

        private async void btnFocusNear_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.FocusNear);

        private async void btnFocusAuto_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.Autofocus);

        private async void btnZoomWide_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.ZoomOut);

        private async void btnZoomTele_Click(object sender, EventArgs e) => await Controller.Control(CameraCommand.ZoomIn);
    }
}
