namespace NPConfig
{
    partial class ConfigMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigMain));
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonSeparator1 = new System.Windows.Forms.RibbonSeparator();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.btnServerSetting = new System.Windows.Forms.RibbonButton();
            this.btnSerialPortSetting = new System.Windows.Forms.RibbonButton();
            this.btnRestfulSetting = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.btnParkingSetting = new System.Windows.Forms.RibbonButton();
            this.btnDeviceSetting = new System.Windows.Forms.RibbonButton();
            this.btnPaymentSetting = new System.Windows.Forms.RibbonButton();
            this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
            this.btnAdminPWSetting = new System.Windows.Forms.RibbonButton();
            this.label1 = new System.Windows.Forms.Label();
            this.oftConfig = new FadeFox.UI.OpenFileTextBox();
            this.SuspendLayout();
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BackColor = System.Drawing.SystemColors.Control;
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonSeparator1);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 75);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2010;
            this.ribbon1.OrbText = "";
            this.ribbon1.OrbVisible = false;
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(1008, 162);
            this.ribbon1.TabIndex = 0;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.TabSpacing = 3;
            this.ribbon1.Text = "ribbon1";
            this.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Blue_2010;
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Panels.Add(this.ribbonPanel2);
            this.ribbonTab1.Panels.Add(this.ribbonPanel3);
            this.ribbonTab1.Text = "기능설정";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Items.Add(this.btnServerSetting);
            this.ribbonPanel1.Items.Add(this.btnSerialPortSetting);
            this.ribbonPanel1.Items.Add(this.btnRestfulSetting);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Text = "연결설정";
            // 
            // btnServerSetting
            // 
            this.btnServerSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnServerSetting.Image")));
            this.btnServerSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnServerSetting.LargeImage")));
            this.btnServerSetting.MaxSizeMode = System.Windows.Forms.RibbonElementSizeMode.Large;
            this.btnServerSetting.Name = "btnServerSetting";
            this.btnServerSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnServerSetting.SmallImage")));
            this.btnServerSetting.Tag = "SVR";
            this.btnServerSetting.Text = "서버    설정";
            this.btnServerSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // btnSerialPortSetting
            // 
            this.btnSerialPortSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSerialPortSetting.Image")));
            this.btnSerialPortSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSerialPortSetting.LargeImage")));
            this.btnSerialPortSetting.Name = "btnSerialPortSetting";
            this.btnSerialPortSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnSerialPortSetting.SmallImage")));
            this.btnSerialPortSetting.Tag = "SRI";
            this.btnSerialPortSetting.Text = "시리얼포트 설정";
            this.btnSerialPortSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // btnRestfulSetting
            // 
            this.btnRestfulSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnRestfulSetting.Image")));
            this.btnRestfulSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRestfulSetting.LargeImage")));
            this.btnRestfulSetting.Name = "btnRestfulSetting";
            this.btnRestfulSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnRestfulSetting.SmallImage")));
            this.btnRestfulSetting.Tag = "RES";
            this.btnRestfulSetting.Text = "레스트풀 설정";
            this.btnRestfulSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.Items.Add(this.btnParkingSetting);
            this.ribbonPanel2.Items.Add(this.btnDeviceSetting);
            this.ribbonPanel2.Items.Add(this.btnPaymentSetting);
            this.ribbonPanel2.Name = "ribbonPanel2";
            this.ribbonPanel2.Text = "기능설정";
            // 
            // btnParkingSetting
            // 
            this.btnParkingSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.Image")));
            this.btnParkingSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.LargeImage")));
            this.btnParkingSetting.Name = "btnParkingSetting";
            this.btnParkingSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnParkingSetting.SmallImage")));
            this.btnParkingSetting.Tag = "PRK";
            this.btnParkingSetting.Text = "주차관련 설정";
            this.btnParkingSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // btnDeviceSetting
            // 
            this.btnDeviceSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnDeviceSetting.Image")));
            this.btnDeviceSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnDeviceSetting.LargeImage")));
            this.btnDeviceSetting.Name = "btnDeviceSetting";
            this.btnDeviceSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnDeviceSetting.SmallImage")));
            this.btnDeviceSetting.Tag = "DIV";
            this.btnDeviceSetting.Text = "사용기능 설정";
            this.btnDeviceSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // btnPaymentSetting
            // 
            this.btnPaymentSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnPaymentSetting.Image")));
            this.btnPaymentSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnPaymentSetting.LargeImage")));
            this.btnPaymentSetting.Name = "btnPaymentSetting";
            this.btnPaymentSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnPaymentSetting.SmallImage")));
            this.btnPaymentSetting.Tag = "PAY";
            this.btnPaymentSetting.Text = "일반    설정";
            this.btnPaymentSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // ribbonPanel3
            // 
            this.ribbonPanel3.Items.Add(this.btnAdminPWSetting);
            this.ribbonPanel3.Name = "ribbonPanel3";
            this.ribbonPanel3.Text = "관리설정";
            // 
            // btnAdminPWSetting
            // 
            this.btnAdminPWSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnAdminPWSetting.Image")));
            this.btnAdminPWSetting.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnAdminPWSetting.LargeImage")));
            this.btnAdminPWSetting.Name = "btnAdminPWSetting";
            this.btnAdminPWSetting.SmallImage = ((System.Drawing.Image)(resources.GetObject("btnAdminPWSetting.SmallImage")));
            this.btnAdminPWSetting.Tag = "PWD";
            this.btnAdminPWSetting.Text = "관리암호 설정";
            this.btnAdminPWSetting.Click += new System.EventHandler(this.SettingButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(521, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(487, 27);
            this.label1.TabIndex = 13;
            this.label1.Text = "  설정파일 :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // oftConfig
            // 
            this.oftConfig.AllowFileCheck = false;
            this.oftConfig.BackColor = System.Drawing.Color.Transparent;
            this.oftConfig.ClearButtonVisible = true;
            this.oftConfig.Filter = "모든 파일 (*.*)|*.*";
            this.oftConfig.Location = new System.Drawing.Point(596, 8);
            this.oftConfig.Name = "oftConfig";
            this.oftConfig.Size = new System.Drawing.Size(402, 22);
            this.oftConfig.TabIndex = 12;
            this.oftConfig.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // ConfigMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.oftConfig);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ribbon1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "ConfigMain";
            this.Text = "통합 설정 관리자";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigMain_FormClosed);
            this.Load += new System.EventHandler(this.ConfigMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.RibbonButton btnServerSetting;
        private System.Windows.Forms.RibbonButton btnSerialPortSetting;
        private System.Windows.Forms.RibbonButton btnRestfulSetting;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton btnParkingSetting;
        private System.Windows.Forms.RibbonButton btnDeviceSetting;
        private System.Windows.Forms.RibbonButton btnPaymentSetting;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonButton btnAdminPWSetting;
        private System.Windows.Forms.RibbonSeparator ribbonSeparator1;
        private FadeFox.UI.OpenFileTextBox oftConfig;
        private System.Windows.Forms.Label label1;
    }
}