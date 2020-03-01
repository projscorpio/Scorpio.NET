namespace Scorpio.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.logbox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbLogLevel = new System.Windows.Forms.ComboBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ucRoverGamepad1 = new Scorpio.GUI.Controls.ucRoverGamepad();
            this.ucVivotekController1 = new Scorpio.GUI.Controls.ucVivotekController();
            this.ucStreamControl4 = new Scorpio.GUI.Controls.ucStreamControl();
            this.ucStreamControl3 = new Scorpio.GUI.Controls.ucStreamControl();
            this.ucStreamControl2 = new Scorpio.GUI.Controls.ucStreamControl();
            this.ucStreamControl1 = new Scorpio.GUI.Controls.ucStreamControl();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logbox
            // 
            this.logbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logbox.Location = new System.Drawing.Point(3, 23);
            this.logbox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.logbox.Name = "logbox";
            this.logbox.ReadOnly = true;
            this.logbox.Size = new System.Drawing.Size(1153, 174);
            this.logbox.TabIndex = 0;
            this.logbox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbLogLevel);
            this.groupBox1.Controls.Add(this.btnClearLogs);
            this.groupBox1.Controls.Add(this.logbox);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F);
            this.groupBox1.Location = new System.Drawing.Point(0, 298);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(1159, 199);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Events";
            // 
            // cbLogLevel
            // 
            this.cbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogLevel.FormattingEnabled = true;
            this.cbLogLevel.Items.AddRange(new object[] {
            "Debug",
            "Info",
            "Warn",
            "Error",
            "Fatal"});
            this.cbLogLevel.Location = new System.Drawing.Point(984, 25);
            this.cbLogLevel.Margin = new System.Windows.Forms.Padding(4);
            this.cbLogLevel.Name = "cbLogLevel";
            this.cbLogLevel.Size = new System.Drawing.Size(85, 25);
            this.cbLogLevel.TabIndex = 12;
            this.cbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cbLogLevel_SelectedIndexChanged);
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Location = new System.Drawing.Point(1080, 26);
            this.btnClearLogs.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(61, 28);
            this.btnClearLogs.TabIndex = 11;
            this.btnClearLogs.Text = "Clear";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.GreenYellow;
            this.btnConnect.Location = new System.Drawing.Point(37, 48);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(177, 53);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.BackColor = System.Drawing.Color.PaleVioletRed;
            this.btnDisconnect.Location = new System.Drawing.Point(37, 148);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(177, 53);
            this.btnDisconnect.TabIndex = 10;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1159, 300);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1151, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connection & control";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1143, 263);
            this.panel1.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.ucRoverGamepad1);
            this.groupBox3.Location = new System.Drawing.Point(281, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(607, 256);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rover control";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.btnDisconnect);
            this.groupBox2.Location = new System.Drawing.Point(7, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(267, 256);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connection";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1151, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Camera movement";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ucVivotekController1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1143, 263);
            this.panel2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1151, 271);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "GStreamer";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ucStreamControl4);
            this.panel3.Controls.Add(this.ucStreamControl3);
            this.panel3.Controls.Add(this.ucStreamControl2);
            this.panel3.Controls.Add(this.ucStreamControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1151, 271);
            this.panel3.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 501);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1159, 26);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(173, 20);
            this.toolStripStatusLabel1.Text = "Socket client: connected!";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(179, 20);
            this.toolStripStatusLabel2.Text = "Rover gamepad: Stopped";
            // 
            // ucRoverGamepad1
            // 
            this.ucRoverGamepad1.Autofac = null;
            this.ucRoverGamepad1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucRoverGamepad1.Font = new System.Drawing.Font("Arial", 9F);
            this.ucRoverGamepad1.Location = new System.Drawing.Point(7, 22);
            this.ucRoverGamepad1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucRoverGamepad1.Name = "ucRoverGamepad1";
            this.ucRoverGamepad1.Size = new System.Drawing.Size(593, 214);
            this.ucRoverGamepad1.TabIndex = 9;
            this.ucRoverGamepad1.VivotekId = null;
            // 
            // ucVivotekController1
            // 
            this.ucVivotekController1.Autofac = null;
            this.ucVivotekController1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucVivotekController1.Font = new System.Drawing.Font("Arial", 9F);
            this.ucVivotekController1.Location = new System.Drawing.Point(3, 2);
            this.ucVivotekController1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucVivotekController1.Name = "ucVivotekController1";
            this.ucVivotekController1.Size = new System.Drawing.Size(1134, 256);
            this.ucVivotekController1.TabIndex = 8;
            this.ucVivotekController1.VivotekId = null;
            // 
            // ucStreamControl4
            // 
            this.ucStreamControl4.Autofac = null;
            this.ucStreamControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucStreamControl4.CameraId = null;
            this.ucStreamControl4.Location = new System.Drawing.Point(839, 18);
            this.ucStreamControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucStreamControl4.Name = "ucStreamControl4";
            this.ucStreamControl4.Size = new System.Drawing.Size(271, 126);
            this.ucStreamControl4.TabIndex = 4;
            // 
            // ucStreamControl3
            // 
            this.ucStreamControl3.Autofac = null;
            this.ucStreamControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucStreamControl3.CameraId = null;
            this.ucStreamControl3.Location = new System.Drawing.Point(561, 18);
            this.ucStreamControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucStreamControl3.Name = "ucStreamControl3";
            this.ucStreamControl3.Size = new System.Drawing.Size(271, 126);
            this.ucStreamControl3.TabIndex = 4;
            // 
            // ucStreamControl2
            // 
            this.ucStreamControl2.Autofac = null;
            this.ucStreamControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucStreamControl2.CameraId = null;
            this.ucStreamControl2.Location = new System.Drawing.Point(284, 18);
            this.ucStreamControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucStreamControl2.Name = "ucStreamControl2";
            this.ucStreamControl2.Size = new System.Drawing.Size(271, 126);
            this.ucStreamControl2.TabIndex = 4;
            // 
            // ucStreamControl1
            // 
            this.ucStreamControl1.Autofac = null;
            this.ucStreamControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucStreamControl1.CameraId = null;
            this.ucStreamControl1.Location = new System.Drawing.Point(7, 18);
            this.ucStreamControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucStreamControl1.Name = "ucStreamControl1";
            this.ucStreamControl1.Size = new System.Drawing.Size(271, 126);
            this.ucStreamControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 527);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Rysiu Player 2.0";
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox logbox;
        private System.Windows.Forms.GroupBox groupBox1;
        private Controls.ucStreamControl ucStreamControl1;
        private Controls.ucStreamControl ucStreamControl2;
        private Controls.ucStreamControl ucStreamControl3;
        private Controls.ucStreamControl ucStreamControl4;
        private Controls.ucVivotekController ucVivotekController1;
        private Controls.ucRoverGamepad ucRoverGamepad1;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbLogLevel;
    }
}

