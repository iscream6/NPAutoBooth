namespace NPAutoBooth.UI
{
    partial class FormMessageShortBoxYESNO
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
            this.btn_Yes = new FadeFox.UI.ImageButton();
            this.timerInput = new System.Windows.Forms.Timer(this.components);
            this.Btn_No = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Yes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_No)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Transparent;
            this.lbl_message.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.lbl_message.ForeColor = System.Drawing.Color.White;
            this.lbl_message.Location = new System.Drawing.Point(12, 9);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(653, 188);
            this.lbl_message.TabIndex = 1;
            this.lbl_message.Text = "現在の設定を適用しますか？";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btn_Yes.Location = new System.Drawing.Point(39, 200);
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
            // timerInput
            // 
            this.timerInput.Interval = 1000;
            this.timerInput.Tick += new System.EventHandler(this.timerInput_Tick);
            // 
            // Btn_No
            // 
            this.Btn_No.BackColor = System.Drawing.Color.Transparent;
            this.Btn_No.DialogResult = System.Windows.Forms.DialogResult.None;
            this.Btn_No.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.Btn_No.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.Btn_No.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.Btn_No.DownTextColor = System.Drawing.Color.White;
            this.Btn_No.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.Btn_No.Font = new System.Drawing.Font("MS Gothic", 28F, System.Drawing.FontStyle.Bold);
            this.Btn_No.ForeColor = System.Drawing.Color.Black;
            this.Btn_No.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.Btn_No.Location = new System.Drawing.Point(378, 200);
            this.Btn_No.Name = "Btn_No";
            this.Btn_No.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.Btn_No.Size = new System.Drawing.Size(215, 79);
            this.Btn_No.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Btn_No.TabIndex = 347;
            this.Btn_No.TabStop = false;
            this.Btn_No.Tag = "";
            this.Btn_No.Text = "いいえ";
            this.Btn_No.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Btn_No.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.Btn_No.TextTopMargin = 2;
            this.Btn_No.UsingTextAlignInImage = false;
            this.Btn_No.UsingTextShadow = false;
            this.Btn_No.Click += new System.EventHandler(this.Btn_No_Click);
            // 
            // FormMessageShortBoxYESNO
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(677, 323);
            this.Controls.Add(this.Btn_No);
            this.Controls.Add(this.btn_Yes);
            this.Controls.Add(this.lbl_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMessageShortBoxYESNO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessageBox";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMessageShortBox_FormClosed);
            this.Load += new System.EventHandler(this.FormMessageShortBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Yes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_No)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbl_message;
        private FadeFox.UI.ImageButton btn_Yes;
        private System.Windows.Forms.Timer timerInput;
        private FadeFox.UI.ImageButton Btn_No;
    }
}