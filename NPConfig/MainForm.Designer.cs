namespace NPConfig
{
	partial class MainForm
	{
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 디자이너에서 생성한 코드

		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.oftConfig = new FadeFox.UI.OpenFileTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSettingSerialPort = new FadeFox.UI.ImageButton();
            this.btnSettingServer = new FadeFox.UI.ImageButton();
            this.btnNPPaymentInfo = new FadeFox.UI.ImageButton();
            this.btnSettingAdminPassword = new FadeFox.UI.ImageButton();
            this.btnUsingSetting = new FadeFox.UI.ImageButton();
            this.btnParkingSetting = new FadeFox.UI.ImageButton();
            this.btn_BackGroundSetting = new FadeFox.UI.ImageButton();
            this.btnHttpSetting = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingSerialPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNPPaymentInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingAdminPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUsingSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnParkingSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_BackGroundSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHttpSetting)).BeginInit();
            this.SuspendLayout();
            // 
            // oftConfig
            // 
            this.oftConfig.AllowFileCheck = false;
            this.oftConfig.BackColor = System.Drawing.Color.Transparent;
            this.oftConfig.ClearButtonVisible = true;
            this.oftConfig.Filter = "모든 파일 (*.*)|*.*";
            this.oftConfig.Location = new System.Drawing.Point(76, 16);
            this.oftConfig.Name = "oftConfig";
            this.oftConfig.Size = new System.Drawing.Size(391, 22);
            this.oftConfig.TabIndex = 11;
            this.oftConfig.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "설정파일:";
            // 
            // btnSettingSerialPort
            // 
            this.btnSettingSerialPort.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSettingSerialPort.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnSettingSerialPort.DisabledImage")));
            this.btnSettingSerialPort.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnSettingSerialPort.DownImage = ((System.Drawing.Image)(resources.GetObject("btnSettingSerialPort.DownImage")));
            this.btnSettingSerialPort.DownTextColor = System.Drawing.Color.White;
            this.btnSettingSerialPort.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingSerialPort.ForeColor = System.Drawing.Color.White;
            this.btnSettingSerialPort.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnSettingSerialPort.HoverImage")));
            this.btnSettingSerialPort.Location = new System.Drawing.Point(76, 83);
            this.btnSettingSerialPort.Name = "btnSettingSerialPort";
            this.btnSettingSerialPort.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSettingSerialPort.NormalImage")));
            this.btnSettingSerialPort.Size = new System.Drawing.Size(138, 31);
            this.btnSettingSerialPort.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnSettingSerialPort.TabIndex = 16;
            this.btnSettingSerialPort.TabStop = false;
            this.btnSettingSerialPort.Text = "SerialPort 설정";
            this.btnSettingSerialPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnSettingSerialPort.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingSerialPort.TextTopMargin = 2;
            this.btnSettingSerialPort.UsingTextAlignInImage = true;
            this.btnSettingSerialPort.UsingTextShadow = true;
            this.btnSettingSerialPort.Click += new System.EventHandler(this.btnSettingSerialPort_Click);
            // 
            // btnSettingServer
            // 
            this.btnSettingServer.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSettingServer.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnSettingServer.DisabledImage")));
            this.btnSettingServer.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnSettingServer.DownImage = ((System.Drawing.Image)(resources.GetObject("btnSettingServer.DownImage")));
            this.btnSettingServer.DownTextColor = System.Drawing.Color.White;
            this.btnSettingServer.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingServer.ForeColor = System.Drawing.Color.White;
            this.btnSettingServer.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnSettingServer.HoverImage")));
            this.btnSettingServer.Location = new System.Drawing.Point(76, 46);
            this.btnSettingServer.Name = "btnSettingServer";
            this.btnSettingServer.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSettingServer.NormalImage")));
            this.btnSettingServer.Size = new System.Drawing.Size(138, 31);
            this.btnSettingServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnSettingServer.TabIndex = 17;
            this.btnSettingServer.TabStop = false;
            this.btnSettingServer.Text = "Server 설정";
            this.btnSettingServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnSettingServer.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingServer.TextTopMargin = 2;
            this.btnSettingServer.UsingTextAlignInImage = true;
            this.btnSettingServer.UsingTextShadow = true;
            this.btnSettingServer.Click += new System.EventHandler(this.btnSettingServer_Click);
            // 
            // btnNPPaymentInfo
            // 
            this.btnNPPaymentInfo.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnNPPaymentInfo.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnNPPaymentInfo.DisabledImage")));
            this.btnNPPaymentInfo.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnNPPaymentInfo.DownImage = ((System.Drawing.Image)(resources.GetObject("btnNPPaymentInfo.DownImage")));
            this.btnNPPaymentInfo.DownTextColor = System.Drawing.Color.White;
            this.btnNPPaymentInfo.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnNPPaymentInfo.ForeColor = System.Drawing.Color.White;
            this.btnNPPaymentInfo.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnNPPaymentInfo.HoverImage")));
            this.btnNPPaymentInfo.Location = new System.Drawing.Point(76, 120);
            this.btnNPPaymentInfo.Name = "btnNPPaymentInfo";
            this.btnNPPaymentInfo.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnNPPaymentInfo.NormalImage")));
            this.btnNPPaymentInfo.Size = new System.Drawing.Size(138, 31);
            this.btnNPPaymentInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnNPPaymentInfo.TabIndex = 18;
            this.btnNPPaymentInfo.TabStop = false;
            this.btnNPPaymentInfo.Text = "일반설정";
            this.btnNPPaymentInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnNPPaymentInfo.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnNPPaymentInfo.TextTopMargin = 2;
            this.btnNPPaymentInfo.UsingTextAlignInImage = true;
            this.btnNPPaymentInfo.UsingTextShadow = true;
            this.btnNPPaymentInfo.Click += new System.EventHandler(this.btnNPPaymentInfo_Click);
            // 
            // btnSettingAdminPassword
            // 
            this.btnSettingAdminPassword.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSettingAdminPassword.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnSettingAdminPassword.DisabledImage")));
            this.btnSettingAdminPassword.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnSettingAdminPassword.DownImage = ((System.Drawing.Image)(resources.GetObject("btnSettingAdminPassword.DownImage")));
            this.btnSettingAdminPassword.DownTextColor = System.Drawing.Color.White;
            this.btnSettingAdminPassword.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingAdminPassword.ForeColor = System.Drawing.Color.White;
            this.btnSettingAdminPassword.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnSettingAdminPassword.HoverImage")));
            this.btnSettingAdminPassword.Location = new System.Drawing.Point(76, 194);
            this.btnSettingAdminPassword.Name = "btnSettingAdminPassword";
            this.btnSettingAdminPassword.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnSettingAdminPassword.NormalImage")));
            this.btnSettingAdminPassword.Size = new System.Drawing.Size(138, 31);
            this.btnSettingAdminPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnSettingAdminPassword.TabIndex = 19;
            this.btnSettingAdminPassword.TabStop = false;
            this.btnSettingAdminPassword.Text = "관리자 암호 설정";
            this.btnSettingAdminPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnSettingAdminPassword.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnSettingAdminPassword.TextTopMargin = 2;
            this.btnSettingAdminPassword.UsingTextAlignInImage = true;
            this.btnSettingAdminPassword.UsingTextShadow = true;
            this.btnSettingAdminPassword.Click += new System.EventHandler(this.btnSettingAdminPassword_Click);
            // 
            // btnUsingSetting
            // 
            this.btnUsingSetting.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnUsingSetting.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnUsingSetting.DisabledImage")));
            this.btnUsingSetting.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnUsingSetting.DownImage = ((System.Drawing.Image)(resources.GetObject("btnUsingSetting.DownImage")));
            this.btnUsingSetting.DownTextColor = System.Drawing.Color.White;
            this.btnUsingSetting.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnUsingSetting.ForeColor = System.Drawing.Color.White;
            this.btnUsingSetting.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnUsingSetting.HoverImage")));
            this.btnUsingSetting.Location = new System.Drawing.Point(76, 157);
            this.btnUsingSetting.Name = "btnUsingSetting";
            this.btnUsingSetting.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnUsingSetting.NormalImage")));
            this.btnUsingSetting.Size = new System.Drawing.Size(138, 31);
            this.btnUsingSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnUsingSetting.TabIndex = 21;
            this.btnUsingSetting.TabStop = false;
            this.btnUsingSetting.Text = "사용기능 설정";
            this.btnUsingSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnUsingSetting.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnUsingSetting.TextTopMargin = 2;
            this.btnUsingSetting.UsingTextAlignInImage = true;
            this.btnUsingSetting.UsingTextShadow = true;
            this.btnUsingSetting.Click += new System.EventHandler(this.btnUsingSetting_Click);
            // 
            // btnParkingSetting
            // 
            this.btnParkingSetting.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnParkingSetting.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.DisabledImage")));
            this.btnParkingSetting.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnParkingSetting.DownImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.DownImage")));
            this.btnParkingSetting.DownTextColor = System.Drawing.Color.White;
            this.btnParkingSetting.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnParkingSetting.ForeColor = System.Drawing.Color.White;
            this.btnParkingSetting.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.HoverImage")));
            this.btnParkingSetting.Location = new System.Drawing.Point(316, 46);
            this.btnParkingSetting.Name = "btnParkingSetting";
            this.btnParkingSetting.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.NormalImage")));
            this.btnParkingSetting.Size = new System.Drawing.Size(138, 31);
            this.btnParkingSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnParkingSetting.TabIndex = 25;
            this.btnParkingSetting.TabStop = false;
            this.btnParkingSetting.Text = "주차관련설정";
            this.btnParkingSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnParkingSetting.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnParkingSetting.TextTopMargin = 2;
            this.btnParkingSetting.UsingTextAlignInImage = true;
            this.btnParkingSetting.UsingTextShadow = true;
            this.btnParkingSetting.Click += new System.EventHandler(this.btnParkingSetting_Click);
            // 
            // btn_BackGroundSetting
            // 
            this.btn_BackGroundSetting.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_BackGroundSetting.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btn_BackGroundSetting.DisabledImage")));
            this.btn_BackGroundSetting.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_BackGroundSetting.DownImage = ((System.Drawing.Image)(resources.GetObject("btn_BackGroundSetting.DownImage")));
            this.btn_BackGroundSetting.DownTextColor = System.Drawing.Color.White;
            this.btn_BackGroundSetting.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_BackGroundSetting.ForeColor = System.Drawing.Color.White;
            this.btn_BackGroundSetting.HoverImage = ((System.Drawing.Image)(resources.GetObject("btn_BackGroundSetting.HoverImage")));
            this.btn_BackGroundSetting.Location = new System.Drawing.Point(316, 167);
            this.btn_BackGroundSetting.Name = "btn_BackGroundSetting";
            this.btn_BackGroundSetting.NormalImage = ((System.Drawing.Image)(resources.GetObject("btn_BackGroundSetting.NormalImage")));
            this.btn_BackGroundSetting.Size = new System.Drawing.Size(138, 31);
            this.btn_BackGroundSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btn_BackGroundSetting.TabIndex = 28;
            this.btn_BackGroundSetting.TabStop = false;
            this.btn_BackGroundSetting.Text = "배경이미지설정";
            this.btn_BackGroundSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_BackGroundSetting.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_BackGroundSetting.TextTopMargin = 2;
            this.btn_BackGroundSetting.UsingTextAlignInImage = true;
            this.btn_BackGroundSetting.UsingTextShadow = true;
            this.btn_BackGroundSetting.Visible = false;
            this.btn_BackGroundSetting.Click += new System.EventHandler(this.btn_BackGroundSetting_Click);
            // 
            // btnHttpSetting
            // 
            this.btnHttpSetting.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnHttpSetting.DisabledImage = ((System.Drawing.Image)(resources.GetObject("btnHttpSetting.DisabledImage")));
            this.btnHttpSetting.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnHttpSetting.DownImage = ((System.Drawing.Image)(resources.GetObject("btnHttpSetting.DownImage")));
            this.btnHttpSetting.DownTextColor = System.Drawing.Color.White;
            this.btnHttpSetting.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnHttpSetting.ForeColor = System.Drawing.Color.White;
            this.btnHttpSetting.HoverImage = ((System.Drawing.Image)(resources.GetObject("btnHttpSetting.HoverImage")));
            this.btnHttpSetting.Location = new System.Drawing.Point(316, 83);
            this.btnHttpSetting.Name = "btnHttpSetting";
            this.btnHttpSetting.NormalImage = ((System.Drawing.Image)(resources.GetObject("btnHttpSetting.NormalImage")));
            this.btnHttpSetting.Size = new System.Drawing.Size(138, 31);
            this.btnHttpSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnHttpSetting.TabIndex = 29;
            this.btnHttpSetting.TabStop = false;
            this.btnHttpSetting.Text = "레스트풀설정";
            this.btnHttpSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnHttpSetting.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnHttpSetting.TextTopMargin = 2;
            this.btnHttpSetting.UsingTextAlignInImage = true;
            this.btnHttpSetting.UsingTextShadow = true;
            this.btnHttpSetting.Click += new System.EventHandler(this.btnHttpSetting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(533, 239);
            this.Controls.Add(this.btnHttpSetting);
            this.Controls.Add(this.btn_BackGroundSetting);
            this.Controls.Add(this.btnParkingSetting);
            this.Controls.Add(this.btnUsingSetting);
            this.Controls.Add(this.btnSettingAdminPassword);
            this.Controls.Add(this.btnNPPaymentInfo);
            this.Controls.Add(this.btnSettingServer);
            this.Controls.Add(this.btnSettingSerialPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.oftConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "설정";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingSerialPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnNPPaymentInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSettingAdminPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUsingSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnParkingSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_BackGroundSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHttpSetting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private FadeFox.UI.OpenFileTextBox oftConfig;
		private System.Windows.Forms.Label label1;
		private FadeFox.UI.ImageButton btnSettingSerialPort;
		private FadeFox.UI.ImageButton btnSettingServer;
		private FadeFox.UI.ImageButton btnNPPaymentInfo;
        private FadeFox.UI.ImageButton btnSettingAdminPassword;
        private FadeFox.UI.ImageButton btnUsingSetting;
        private FadeFox.UI.ImageButton btnParkingSetting;
        private FadeFox.UI.ImageButton btn_BackGroundSetting;
        private FadeFox.UI.ImageButton btnHttpSetting;
    }
}

