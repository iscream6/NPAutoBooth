namespace NPAutoBooth.UI
{
    partial class FormMessagePrePay
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
            this.inputTimer = new System.Windows.Forms.Timer(this.components);
            this.PicBackGroud = new System.Windows.Forms.PictureBox();
            this.btnTXT_BACK_TOMENU = new FadeFox.UI.ImageButton();
            this.btn_TXT_YES = new FadeFox.UI.ImageButton();
            this.lbl_Msg2 = new System.Windows.Forms.Label();
            this.lbl_Msg1 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicBackGroud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_BACK_TOMENU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_YES)).BeginInit();
            this.SuspendLayout();
            // 
            // inputTimer
            // 
            this.inputTimer.Interval = 1000;
            this.inputTimer.Tick += new System.EventHandler(this.inputTimer_Tick);
            // 
            // PicBackGroud
            // 
            this.PicBackGroud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicBackGroud.Image = global::NPAutoBooth.Properties.Resources.NEW_팝업_요금결제진행여부;
            this.PicBackGroud.Location = new System.Drawing.Point(0, 0);
            this.PicBackGroud.Name = "PicBackGroud";
            this.PicBackGroud.Size = new System.Drawing.Size(993, 733);
            this.PicBackGroud.TabIndex = 0;
            this.PicBackGroud.TabStop = false;
            // 
            // btnTXT_BACK_TOMENU
            // 
            this.btnTXT_BACK_TOMENU.BackColor = System.Drawing.Color.Transparent;
            this.btnTXT_BACK_TOMENU.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnTXT_BACK_TOMENU.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnTXT_BACK_TOMENU.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnTXT_BACK_TOMENU.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnTXT_BACK_TOMENU.DownTextColor = System.Drawing.Color.White;
            this.btnTXT_BACK_TOMENU.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnTXT_BACK_TOMENU.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btnTXT_BACK_TOMENU.ForeColor = System.Drawing.Color.Green;
            this.btnTXT_BACK_TOMENU.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnTXT_BACK_TOMENU.Location = new System.Drawing.Point(526, 556);
            this.btnTXT_BACK_TOMENU.Name = "btnTXT_BACK_TOMENU";
            this.btnTXT_BACK_TOMENU.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnTXT_BACK_TOMENU.Size = new System.Drawing.Size(314, 86);
            this.btnTXT_BACK_TOMENU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnTXT_BACK_TOMENU.TabIndex = 275;
            this.btnTXT_BACK_TOMENU.TabStop = false;
            this.btnTXT_BACK_TOMENU.Tag = "TXT_BACK";
            this.btnTXT_BACK_TOMENU.Text = "취소";
            this.btnTXT_BACK_TOMENU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnTXT_BACK_TOMENU.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnTXT_BACK_TOMENU.TextTopMargin = 2;
            this.btnTXT_BACK_TOMENU.UsingTextAlignInImage = false;
            this.btnTXT_BACK_TOMENU.UsingTextShadow = false;
            this.btnTXT_BACK_TOMENU.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btn_TXT_YES
            // 
            this.btn_TXT_YES.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_YES.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_YES.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_TXT_YES.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_YES.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_TXT_YES.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_YES.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_YES.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btn_TXT_YES.ForeColor = System.Drawing.Color.Green;
            this.btn_TXT_YES.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_TXT_YES.Location = new System.Drawing.Point(115, 556);
            this.btn_TXT_YES.Name = "btn_TXT_YES";
            this.btn_TXT_YES.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_TXT_YES.Size = new System.Drawing.Size(314, 86);
            this.btn_TXT_YES.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_YES.TabIndex = 274;
            this.btn_TXT_YES.TabStop = false;
            this.btn_TXT_YES.Tag = "TXT_YES";
            this.btn_TXT_YES.Text = "예";
            this.btn_TXT_YES.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_YES.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_YES.TextTopMargin = 2;
            this.btn_TXT_YES.UsingTextAlignInImage = false;
            this.btn_TXT_YES.UsingTextShadow = false;
            this.btn_TXT_YES.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lbl_Msg2
            // 
            this.lbl_Msg2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg2.Font = new System.Drawing.Font("굴림", 30F, System.Drawing.FontStyle.Bold);
            this.lbl_Msg2.Location = new System.Drawing.Point(43, 332);
            this.lbl_Msg2.Name = "lbl_Msg2";
            this.lbl_Msg2.Size = new System.Drawing.Size(875, 83);
            this.lbl_Msg2.TabIndex = 324;
            this.lbl_Msg2.Tag = "";
            this.lbl_Msg2.Text = "관리실로 가져오시면 거스름돈을 받으실수 있습니다.";
            this.lbl_Msg2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Msg1
            // 
            this.lbl_Msg1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg1.Font = new System.Drawing.Font("굴림", 30F, System.Drawing.FontStyle.Bold);
            this.lbl_Msg1.Location = new System.Drawing.Point(43, 227);
            this.lbl_Msg1.Name = "lbl_Msg1";
            this.lbl_Msg1.Size = new System.Drawing.Size(875, 83);
            this.lbl_Msg1.TabIndex = 323;
            this.lbl_Msg1.Tag = "";
            this.lbl_Msg1.Text = "죄송합니다. 거르름돈이 부족히야 보관증이 출력됩니다";
            this.lbl_Msg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 30F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(33, 32);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(875, 83);
            this.lblTitle.TabIndex = 325;
            this.lblTitle.Tag = "";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMessagePrePay
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(993, 733);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lbl_Msg2);
            this.Controls.Add(this.lbl_Msg1);
            this.Controls.Add(this.btnTXT_BACK_TOMENU);
            this.Controls.Add(this.btn_TXT_YES);
            this.Controls.Add(this.PicBackGroud);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMessagePrePay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessageBox";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMessageBox_FormClosed);
            this.Load += new System.EventHandler(this.FormMessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicBackGroud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_BACK_TOMENU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_YES)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PicBackGroud;
        private System.Windows.Forms.Timer inputTimer;
        private FadeFox.UI.ImageButton btnTXT_BACK_TOMENU;
        private FadeFox.UI.ImageButton btn_TXT_YES;
        private System.Windows.Forms.Label lbl_Msg2;
        private System.Windows.Forms.Label lbl_Msg1;
        private System.Windows.Forms.Label lblTitle;
    }
}