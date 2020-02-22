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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblSender = new System.Windows.Forms.Label();
            this.pbAcc = new System.Windows.Forms.ProgressBar();
            this.pbDir = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDir = new System.Windows.Forms.Label();
            this.lblAcc = new System.Windows.Forms.Label();
            this.cbGamepadIndex = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rover Gamepad";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(247, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "State:";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Font = new System.Drawing.Font("Arial", 9F);
            this.lblState.Location = new System.Drawing.Point(247, 81);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(40, 17);
            this.lblState.TabIndex = 0;
            this.lblState.Text = "state";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(89, 65);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 33);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(163, 65);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 33);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblSender
            // 
            this.lblSender.AutoSize = true;
            this.lblSender.Font = new System.Drawing.Font("Arial", 9F);
            this.lblSender.Location = new System.Drawing.Point(14, 73);
            this.lblSender.Name = "lblSender";
            this.lblSender.Size = new System.Drawing.Size(59, 17);
            this.lblSender.TabIndex = 0;
            this.lblSender.Text = "Sender:";
            // 
            // pbAcc
            // 
            this.pbAcc.Location = new System.Drawing.Point(106, 127);
            this.pbAcc.Name = "pbAcc";
            this.pbAcc.Size = new System.Drawing.Size(165, 23);
            this.pbAcc.TabIndex = 2;
            // 
            // pbDir
            // 
            this.pbDir.Location = new System.Drawing.Point(106, 156);
            this.pbDir.Name = "pbDir";
            this.pbDir.Size = new System.Drawing.Size(165, 23);
            this.pbDir.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(14, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Acc:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F);
            this.label4.Location = new System.Drawing.Point(14, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Dir:";
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Font = new System.Drawing.Font("Arial", 9F);
            this.lblDir.Location = new System.Drawing.Point(283, 156);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(37, 17);
            this.lblDir.TabIndex = 0;
            this.lblDir.Text = "Acc:";
            // 
            // lblAcc
            // 
            this.lblAcc.AutoSize = true;
            this.lblAcc.Font = new System.Drawing.Font("Arial", 9F);
            this.lblAcc.Location = new System.Drawing.Point(283, 127);
            this.lblAcc.Name = "lblAcc";
            this.lblAcc.Size = new System.Drawing.Size(37, 17);
            this.lblAcc.TabIndex = 0;
            this.lblAcc.Text = "Acc:";
            // 
            // cbGamepadIndex
            // 
            this.cbGamepadIndex.Location = new System.Drawing.Point(250, 15);
            this.cbGamepadIndex.Name = "cbGamepadIndex";
            this.cbGamepadIndex.Size = new System.Drawing.Size(70, 25);
            this.cbGamepadIndex.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F);
            this.label5.Location = new System.Drawing.Point(198, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Index:";
            // 
            // ucRoverGamepad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.cbGamepadIndex);
            this.Controls.Add(this.pbDir);
            this.Controls.Add(this.pbAcc);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAcc);
            this.Controls.Add(this.lblDir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSender);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "ucRoverGamepad";
            this.Size = new System.Drawing.Size(339, 192);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblSender;
        private System.Windows.Forms.ProgressBar pbAcc;
        private System.Windows.Forms.ProgressBar pbDir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label lblAcc;
        private System.Windows.Forms.ComboBox cbGamepadIndex;
        private System.Windows.Forms.Label label5;
    }
}
