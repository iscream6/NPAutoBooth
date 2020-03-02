namespace NPAutoBooth.UI
{
    partial class FormMessageShortBox
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
            this.lbl_message = new System.Windows.Forms.Label();
            this.timerInput = new System.Windows.Forms.Timer(this.components);
            this.btn_Yes = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Yes)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Transparent;
            this.lbl_message.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.lbl_message.ForeColor = System.Drawing.Color.White;
            this.lbl_message.Location = new System.Drawing.Point(12, 35);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(653, 122);
            this.lbl_message.TabIndex = 1;
            this.lbl_message.Text = "現在の設定を適用しますか？";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerInput
            // 
            this.timerInput.Interval = 1000;
            this.timerInput.Tick += new System.EventHandler(this.timerInput_Tick);
            // 
            // btn_Yes
            // 
            this.btn_Yes.BackColor = System.Drawing.Color.Transparent;
            this.btn_Yes.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Yes.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_Yes.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Yes.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_Yes.DownTextColor = System.Drawing.Color.White;
            this.btn_Yes.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Yes.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.btn_Yes.ForeColor = System.Drawing.Color.Black;
            this.btn_Yes.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_Yes.Location = new System.Drawing.Point(226, 190);
            this.btn_Yes.Name = "btn_Yes";
            this.btn_Yes.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_Yes.Size = new System.Drawing.Size(215, 79);
            this.btn_Yes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Yes.TabIndex = 346;
            this.btn_Yes.TabStop = false;
            this.btn_Yes.Tag = "";
            this.btn_Yes.Text = "はい";
            this.btn_Yes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Yes.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Yes.TextTopMargin = 2;
            this.btn_Yes.UsingTextAlignInImage = false;
            this.btn_Yes.UsingTextShadow = false;
            this.btn_Yes.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // FormMessageShortBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(677, 323);
            this.Controls.Add(this.btn_Yes);
            this.Controls.Add(this.lbl_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMessageShortBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessageBox";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMessageShortBox_FormClosed);
            this.Load += new System.EventHandler(this.FormMessageShortBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Yes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_message;
        private FadeFox.UI.ImageButton btn_Yes;
        private System.Windows.Forms.Timer timerInput;
    }
}