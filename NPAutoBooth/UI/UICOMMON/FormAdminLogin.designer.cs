namespace NPAutoBooth.UI
{
    partial class FormAdminLogin
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
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.timeSerialKeyFormAction = new System.Windows.Forms.Timer(this.components);
            this.lblTXT_ADMIN_MENU = new System.Windows.Forms.Label();
            this.lblMSG_ADMIN_LOGIN = new System.Windows.Forms.Label();
            this.labelPwd = new FadeFox.UI.SimpleLabel();
            this.npPad = new NPAutoBooth.UI.NumberPad();
            this.panelAdminLogin = new System.Windows.Forms.Panel();
            this.btn_TXT_BACK = new FadeFox.UI.ImageButton();
            this.btnOk_TXT_YES = new FadeFox.UI.ImageButton();
            this.panelAdminLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk_TXT_YES)).BeginInit();
            this.SuspendLayout();
            // 
            // timerClose
            // 
            this.timerClose.Interval = 1000;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // lblTXT_ADMIN_MENU
            // 
            this.lblTXT_ADMIN_MENU.BackColor = System.Drawing.Color.Transparent;
            this.lblTXT_ADMIN_MENU.Font = new System.Drawing.Font("옥션고딕 B", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTXT_ADMIN_MENU.ForeColor = System.Drawing.Color.White;
            this.lblTXT_ADMIN_MENU.Location = new System.Drawing.Point(73, 19);
            this.lblTXT_ADMIN_MENU.Name = "lblTXT_ADMIN_MENU";
            this.lblTXT_ADMIN_MENU.Size = new System.Drawing.Size(803, 63);
            this.lblTXT_ADMIN_MENU.TabIndex = 0;
            this.lblTXT_ADMIN_MENU.Tag = "MSG_ADMIN_PWD";
            this.lblTXT_ADMIN_MENU.Text = "管理者パスワード";
            this.lblTXT_ADMIN_MENU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMSG_ADMIN_LOGIN
            // 
            this.lblMSG_ADMIN_LOGIN.BackColor = System.Drawing.Color.Transparent;
            this.lblMSG_ADMIN_LOGIN.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMSG_ADMIN_LOGIN.ForeColor = System.Drawing.Color.White;
            this.lblMSG_ADMIN_LOGIN.Location = new System.Drawing.Point(146, 274);
            this.lblMSG_ADMIN_LOGIN.Name = "lblMSG_ADMIN_LOGIN";
            this.lblMSG_ADMIN_LOGIN.Size = new System.Drawing.Size(360, 33);
            this.lblMSG_ADMIN_LOGIN.TabIndex = 1;
            this.lblMSG_ADMIN_LOGIN.Tag = "MSG_ADMIN_LOGIN";
            this.lblMSG_ADMIN_LOGIN.Text = "管理者パスワードをご入力ください";
            this.lblMSG_ADMIN_LOGIN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPwd
            // 
            this.labelPwd.BackColor = System.Drawing.Color.White;
            this.labelPwd.BorderColor = System.Drawing.Color.Transparent;
            this.labelPwd.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPwd.Location = new System.Drawing.Point(150, 326);
            this.labelPwd.Name = "labelPwd";
            this.labelPwd.PasswordChar = "*";
            this.labelPwd.Size = new System.Drawing.Size(356, 38);
            this.labelPwd.TabIndex = 2;
            this.labelPwd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPwd.TextChanged += new System.EventHandler(this.labelPwd_TextChanged);
            this.labelPwd.Click += new System.EventHandler(this.Label_Click);
            // 
            // npPad
            // 
            this.npPad.BackColor = System.Drawing.Color.Transparent;
            this.npPad.IsNumber = true;
            this.npPad.LimitLength = 10;
            this.npPad.LinkedSimpleLabel = null;
            this.npPad.Location = new System.Drawing.Point(692, 100);
            this.npPad.Name = "npPad";
            this.npPad.Size = new System.Drawing.Size(258, 342);
            this.npPad.TabIndex = 193;
            this.npPad.Value = "";
            // 
            // panelAdminLogin
            // 
            this.panelAdminLogin.BackColor = System.Drawing.Color.Black;
            this.panelAdminLogin.Controls.Add(this.btn_TXT_BACK);
            this.panelAdminLogin.Controls.Add(this.btnOk_TXT_YES);
            this.panelAdminLogin.Controls.Add(this.npPad);
            this.panelAdminLogin.Controls.Add(this.labelPwd);
            this.panelAdminLogin.Controls.Add(this.lblMSG_ADMIN_LOGIN);
            this.panelAdminLogin.Controls.Add(this.lblTXT_ADMIN_MENU);
            this.panelAdminLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAdminLogin.Location = new System.Drawing.Point(0, 0);
            this.panelAdminLogin.Name = "panelAdminLogin";
            this.panelAdminLogin.Size = new System.Drawing.Size(1001, 600);
            this.panelAdminLogin.TabIndex = 0;
            // 
            // btn_TXT_BACK
            // 
            this.btn_TXT_BACK.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_BACK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_BACK.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXT_BACK.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_BACK.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXT_BACK.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.Font = new System.Drawing.Font("옥션고딕 B", 22F);
            this.btn_TXT_BACK.ForeColor = System.Drawing.Color.Yellow;
            this.btn_TXT_BACK.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXT_BACK.Location = new System.Drawing.Point(368, 393);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(138, 43);
            this.btn_TXT_BACK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_BACK.TabIndex = 273;
            this.btn_TXT_BACK.TabStop = false;
            this.btn_TXT_BACK.Tag = "TXT_BACK";
            this.btn_TXT_BACK.Text = "이전화면";
            this.btn_TXT_BACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_BACK.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.TextTopMargin = 2;
            this.btn_TXT_BACK.UsingTextAlignInImage = false;
            this.btn_TXT_BACK.UsingTextShadow = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btnOk_TXT_YES
            // 
            this.btnOk_TXT_YES.BackColor = System.Drawing.Color.Transparent;
            this.btnOk_TXT_YES.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnOk_TXT_YES.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btnOk_TXT_YES.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnOk_TXT_YES.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btnOk_TXT_YES.DownTextColor = System.Drawing.Color.White;
            this.btnOk_TXT_YES.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk_TXT_YES.Font = new System.Drawing.Font("옥션고딕 B", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk_TXT_YES.ForeColor = System.Drawing.Color.Yellow;
            this.btnOk_TXT_YES.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btnOk_TXT_YES.Location = new System.Drawing.Point(150, 393);
            this.btnOk_TXT_YES.Name = "btnOk_TXT_YES";
            this.btnOk_TXT_YES.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btnOk_TXT_YES.Size = new System.Drawing.Size(138, 43);
            this.btnOk_TXT_YES.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnOk_TXT_YES.TabIndex = 272;
            this.btnOk_TXT_YES.TabStop = false;
            this.btnOk_TXT_YES.Tag = "TXT_ENTER";
            this.btnOk_TXT_YES.Text = "確定";
            this.btnOk_TXT_YES.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnOk_TXT_YES.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk_TXT_YES.TextTopMargin = 2;
            this.btnOk_TXT_YES.UsingTextAlignInImage = false;
            this.btnOk_TXT_YES.UsingTextShadow = false;
            this.btnOk_TXT_YES.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // FormAdminLogin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1001, 600);
            this.Controls.Add(this.panelAdminLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAdminLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPAutoBooth";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAdminLogin_FormClosed);
            this.Load += new System.EventHandler(this.FormAdminLogin_Load);
            this.Shown += new System.EventHandler(this.FormAdminLogin_Shown);
            this.panelAdminLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk_TXT_YES)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timerClose;
        private System.Windows.Forms.Timer timeSerialKeyFormAction;
        private System.Windows.Forms.Label lblTXT_ADMIN_MENU;
        private System.Windows.Forms.Label lblMSG_ADMIN_LOGIN;
        private FadeFox.UI.SimpleLabel labelPwd;
        private NumberPad npPad;
        private System.Windows.Forms.Panel panelAdminLogin;
        private FadeFox.UI.ImageButton btn_TXT_BACK;
        private FadeFox.UI.ImageButton btnOk_TXT_YES;
    }
}