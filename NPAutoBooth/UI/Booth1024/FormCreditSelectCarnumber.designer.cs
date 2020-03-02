namespace NPAutoBooth.UI
{
    partial class FormCreditSelectCarnumber
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditSelectCarnumber));
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.inputTimer = new System.Windows.Forms.Timer(this.components);
            this.MovieTimer = new System.Windows.Forms.Timer(this.components);
            this.TimerTmapWait = new System.Windows.Forms.Timer(this.components);
            this.picWait = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(-1, 170);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(39, 41);
            this.axWindowsMediaPlayer1.TabIndex = 212;
            this.axWindowsMediaPlayer1.Visible = false;
            // 
            // inputTimer
            // 
            this.inputTimer.Interval = 3000;
            this.inputTimer.Tick += new System.EventHandler(this.inputTimer_Tick);
            // 
            // MovieTimer
            // 
            this.MovieTimer.Interval = 1000;
            this.MovieTimer.Tick += new System.EventHandler(this.MovieTimer_Tick);
            // 
            // TimerTmapWait
            // 
            this.TimerTmapWait.Interval = 1000;
            this.TimerTmapWait.Tick += new System.EventHandler(this.TimerTmpaWait_Tick);
            // 
            // picWait
            // 
            this.picWait.Image = global::NPAutoBooth.Properties.Resources.Type2Wait;
            this.picWait.Location = new System.Drawing.Point(250, 181);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(574, 379);
            this.picWait.TabIndex = 363;
            this.picWait.TabStop = false;
            this.picWait.Tag = "MSG_WAIT";
            this.picWait.Visible = false;
            // 
            // FormCreditSelectCarnumber
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCreditSelectCarnumber";
            this.Text = "NPAutoBooth";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSelectCarnumber_FormClosed);
            this.Load += new System.EventHandler(this.FormSelectCarnumber_Load);
            this.Shown += new System.EventHandler(this.FormSelectCarnumber_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Timer inputTimer;
        private System.Windows.Forms.Timer MovieTimer;
        private System.Windows.Forms.Timer TimerTmapWait;
        private System.Windows.Forms.PictureBox picWait;
    }
}