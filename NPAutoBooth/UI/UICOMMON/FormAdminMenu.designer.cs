namespace NPAutoBooth.UI
{
    partial class FormAdminMenu
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
            this.panelAdminLogin = new System.Windows.Forms.Panel();
            this.btn_RePassWord = new System.Windows.Forms.Button();
            this.btn_DeviceTest = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btn_home = new System.Windows.Forms.Button();
            this.btn_FeeSetup = new System.Windows.Forms.Button();
            this.lbltitle = new System.Windows.Forms.Label();
            this.timeSerialKeyFormAction = new System.Windows.Forms.Timer(this.components);
            this.timerExit = new System.Windows.Forms.Timer(this.components);
            this.panelAdminLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelAdminLogin
            // 
            this.panelAdminLogin.BackColor = System.Drawing.Color.Black;
            this.panelAdminLogin.Controls.Add(this.btn_RePassWord);
            this.panelAdminLogin.Controls.Add(this.btn_DeviceTest);
            this.panelAdminLogin.Controls.Add(this.btnExit);
            this.panelAdminLogin.Controls.Add(this.btn_home);
            this.panelAdminLogin.Controls.Add(this.btn_FeeSetup);
            this.panelAdminLogin.Controls.Add(this.lbltitle);
            this.panelAdminLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdminLogin.Location = new System.Drawing.Point(0, 0);
            this.panelAdminLogin.Name = "panelAdminLogin";
            this.panelAdminLogin.Size = new System.Drawing.Size(1001, 600);
            this.panelAdminLogin.TabIndex = 1;
            // 
            // btn_RePassWord
            // 
            this.btn_RePassWord.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_RePassWord.Location = new System.Drawing.Point(282, 196);
            this.btn_RePassWord.Name = "btn_RePassWord";
            this.btn_RePassWord.Size = new System.Drawing.Size(452, 55);
            this.btn_RePassWord.TabIndex = 12;
            this.btn_RePassWord.Tag = "TXT_RESET_PWD";
            this.btn_RePassWord.Text = "パスワード再設定";
            this.btn_RePassWord.UseVisualStyleBackColor = true;
            this.btn_RePassWord.Click += new System.EventHandler(this.btn_RePassWord_Click);
            // 
            // btn_DeviceTest
            // 
            this.btn_DeviceTest.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_DeviceTest.Location = new System.Drawing.Point(282, 266);
            this.btn_DeviceTest.Name = "btn_DeviceTest";
            this.btn_DeviceTest.Size = new System.Drawing.Size(452, 55);
            this.btn_DeviceTest.TabIndex = 11;
            this.btn_DeviceTest.Tag = "TXT_DEVICETEST";
            this.btn_DeviceTest.Text = "機器テスト";
            this.btn_DeviceTest.UseVisualStyleBackColor = true;
            this.btn_DeviceTest.Click += new System.EventHandler(this.btn_DeviceTest_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(282, 461);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(452, 55);
            this.btnExit.TabIndex = 7;
            this.btnExit.Tag = "TXT_PROGRAM_EXIT";
            this.btnExit.Text = "プログラム終了  ";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btn_Application_Exit);
            // 
            // btn_home
            // 
            this.btn_home.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_home.Location = new System.Drawing.Point(282, 326);
            this.btn_home.Name = "btn_home";
            this.btn_home.Size = new System.Drawing.Size(452, 55);
            this.btn_home.TabIndex = 4;
            this.btn_home.Tag = "TXT_BACK_TOMENU";
            this.btn_home.Text = "メニューへ";
            this.btn_home.UseVisualStyleBackColor = true;
            this.btn_home.Click += new System.EventHandler(this.btn_home_Click);
            // 
            // btn_FeeSetup
            // 
            this.btn_FeeSetup.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_FeeSetup.Location = new System.Drawing.Point(282, 135);
            this.btn_FeeSetup.Name = "btn_FeeSetup";
            this.btn_FeeSetup.Size = new System.Drawing.Size(452, 55);
            this.btn_FeeSetup.TabIndex = 1;
            this.btn_FeeSetup.Tag = "TXT_CLOSESETTION";
            this.btn_FeeSetup.Text = "保有現金と締め設定)";
            this.btn_FeeSetup.UseVisualStyleBackColor = true;
            this.btn_FeeSetup.Click += new System.EventHandler(this.btn_FeeSetup_Click);
            // 
            // lbltitle
            // 
            this.lbltitle.BackColor = System.Drawing.Color.Transparent;
            this.lbltitle.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbltitle.ForeColor = System.Drawing.Color.White;
            this.lbltitle.Location = new System.Drawing.Point(374, 69);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(211, 33);
            this.lbltitle.TabIndex = 0;
            this.lbltitle.Tag = "TXT_ADMIN_MENU";
            this.lbltitle.Text = "管理者メニュー";
            this.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timerExit
            // 
            this.timerExit.Interval = 600;
            this.timerExit.Tick += new System.EventHandler(this.timerExit_Tick);
            // 
            // FormAdminMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1001, 600);
            this.Controls.Add(this.panelAdminLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAdminMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPAutoBooth";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAdminMenu_FormClosed);
            this.Load += new System.EventHandler(this.FormAdminMenu_Load);
            this.panelAdminLogin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelAdminLogin;
        private System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Button btn_FeeSetup;
        private System.Windows.Forms.Button btn_home;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btn_DeviceTest;
        private System.Windows.Forms.Button btn_RePassWord;
        private System.Windows.Forms.Timer timeSerialKeyFormAction;
        private System.Windows.Forms.Timer timerExit;
    }
}