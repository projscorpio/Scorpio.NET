namespace Scorpio.GUI.Controls
{
    partial class ucRoverGamepad
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblState = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.pbAcc = new System.Windows.Forms.ProgressBar();
            this.pbDir = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDir = new System.Windows.Forms.Label();
            this.lblAcc = new System.Windows.Forms.Label();
            this.cbGamepadIndex = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbAccLimit = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLimit = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbAccLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.BackColor = System.Drawing.SystemColors.Control;
            this.lblState.Font = new System.Drawing.Font("Arial", 9F);
            this.lblState.Location = new System.Drawing.Point(300, 32);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(13, 17);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "-";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.GreenYellow;
            this.btnStart.ForeColor = System.Drawing.Color.Black;
            this.btnStart.Location = new System.Drawing.Point(17, 9);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 33);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Arm";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.PaleVioletRed;
            this.btnStop.Location = new System.Drawing.Point(105, 9);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(69, 33);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Disarm";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // pbAcc
            // 
            this.pbAcc.Location = new System.Drawing.Point(60, 60);
            this.pbAcc.Name = "pbAcc";
            this.pbAcc.Size = new System.Drawing.Size(77, 23);
            this.pbAcc.TabIndex = 2;
            // 
            // pbDir
            // 
            this.pbDir.Location = new System.Drawing.Point(60, 95);
            this.pbDir.Name = "pbDir";
            this.pbDir.Size = new System.Drawing.Size(77, 23);
            this.pbDir.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(14, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Acc:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F);
            this.label4.Location = new System.Drawing.Point(14, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Dir:";
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Font = new System.Drawing.Font("Arial", 9F);
            this.lblDir.Location = new System.Drawing.Point(143, 101);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(13, 17);
            this.lblDir.TabIndex = 0;
            this.lblDir.Text = "-";
            // 
            // lblAcc
            // 
            this.lblAcc.AutoSize = true;
            this.lblAcc.Font = new System.Drawing.Font("Arial", 9F);
            this.lblAcc.Location = new System.Drawing.Point(143, 60);
            this.lblAcc.Name = "lblAcc";
            this.lblAcc.Size = new System.Drawing.Size(13, 17);
            this.lblAcc.TabIndex = 0;
            this.lblAcc.Text = "-";
            // 
            // cbGamepadIndex
            // 
            this.cbGamepadIndex.Location = new System.Drawing.Point(195, 29);
            this.cbGamepadIndex.Name = "cbGamepadIndex";
            this.cbGamepadIndex.Size = new System.Drawing.Size(70, 25);
            this.cbGamepadIndex.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F);
            this.label5.Location = new System.Drawing.Point(192, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Index:";
            // 
            // tbAccLimit
            // 
            this.tbAccLimit.LargeChange = 10;
            this.tbAccLimit.Location = new System.Drawing.Point(177, 62);
            this.tbAccLimit.Maximum = 800;
            this.tbAccLimit.Minimum = 20;
            this.tbAccLimit.Name = "tbAccLimit";
            this.tbAccLimit.Size = new System.Drawing.Size(191, 56);
            this.tbAccLimit.TabIndex = 4;
            this.tbAccLimit.Value = 200;
            this.tbAccLimit.Scroll += new System.EventHandler(this.tbAccLimit_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.Location = new System.Drawing.Point(192, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Limit:";
            // 
            // lblLimit
            // 
            this.lblLimit.AutoSize = true;
            this.lblLimit.Font = new System.Drawing.Font("Arial", 9F);
            this.lblLimit.Location = new System.Drawing.Point(241, 121);
            this.lblLimit.Name = "lblLimit";
            this.lblLimit.Size = new System.Drawing.Size(32, 17);
            this.lblLimit.TabIndex = 0;
            this.lblLimit.Text = "200";
            // 
            // ucRoverGamepad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbAccLimit);
            this.Controls.Add(this.cbGamepadIndex);
            this.Controls.Add(this.pbDir);
            this.Controls.Add(this.pbAcc);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblLimit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAcc);
            this.Controls.Add(this.lblDir);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "ucRoverGamepad";
            this.Size = new System.Drawing.Size(430, 143);
            ((System.ComponentModel.ISupportInitialize)(this.tbAccLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ProgressBar pbAcc;
        private System.Windows.Forms.ProgressBar pbDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label lblAcc;
        private System.Windows.Forms.ComboBox cbGamepadIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar tbAccLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLimit;
    }
}
