namespace NPAutoBooth.UI
{
    partial class FormAdminPassword
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
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.lbl_TXT_RESET_PWD = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.lblCurrentPassword = new FadeFox.UI.SimpleLabel();
            this.lblNewPassword1 = new FadeFox.UI.SimpleLabel();
            this.lblNewPassword2 = new FadeFox.UI.SimpleLabel();
            this.npPad = new NPAutoBooth.UI.NumberPad();
            this.btn_TXT_BACK = new FadeFox.UI.ImageButton();
            this.btn_TXTYES = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXTYES)).BeginInit();
            this.SuspendLayout();
            // 
            // picBackground
            // 
            this.picBackground.BackColor = System.Drawing.Color.Black;
            this.picBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBackground.Location = new System.Drawing.Point(0, 0);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(1001, 600);
            this.picBackground.TabIndex = 0;
            this.picBackground.TabStop = false;
            // 
            // lbl_TXT_RESET_PWD
            // 
            this.lbl_TXT_RESET_PWD.BackColor = System.Drawing.Color.Black;
            this.lbl_TXT_RESET_PWD.Font = new System.Drawing.Font("옥션고딕 B", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_TXT_RESET_PWD.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_RESET_PWD.Location = new System.Drawing.Point(83, 19);
            this.lbl_TXT_RESET_PWD.Name = "lbl_TXT_RESET_PWD";
            this.lbl_TXT_RESET_PWD.Size = new System.Drawing.Size(803, 66);
            this.lbl_TXT_RESET_PWD.TabIndex = 4;
            this.lbl_TXT_RESET_PWD.Tag = "TXT_RESET_PWD";
            this.lbl_TXT_RESET_PWD.Text = "암호변경";
            this.lbl_TXT_RESET_PWD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl3
            // 
            this.lbl3.AutoSize = true;
            this.lbl3.BackColor = System.Drawing.Color.Black;
            this.lbl3.Font = new System.Drawing.Font("옥션고딕 B", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl3.ForeColor = System.Drawing.Color.White;
            this.lbl3.Location = new System.Drawing.Point(132, 347);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(231, 24);
            this.lbl3.TabIndex = 168;
            this.lbl3.Tag = "TXT_NEW_REPWD";
            this.lbl3.Text = "변경 비밀번호 재확인";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.BackColor = System.Drawing.Color.Black;
            this.lbl2.Font = new System.Drawing.Font("옥션고딕 B", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl2.ForeColor = System.Drawing.Color.White;
            this.lbl2.Location = new System.Drawing.Point(132, 247);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(208, 24);
            this.lbl2.TabIndex = 167;
            this.lbl2.Tag = "TXT_NEW_PWD";
            this.lbl2.Text = "변경 비밀번호 입력";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.BackColor = System.Drawing.Color.Black;
            this.lbl1.Font = new System.Drawing.Font("옥션고딕 B", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl1.ForeColor = System.Drawing.Color.White;
            this.lbl1.Location = new System.Drawing.Point(132, 149);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(208, 24);
            this.lbl1.TabIndex = 166;
            this.lbl1.Tag = "TXT_CURRENT_PWD";
            this.lbl1.Text = "현재 비밀번호 입력";
            // 
            // lblCurrentPassword
            // 
            this.lblCurrentPassword.BackColor = System.Drawing.Color.White;
            this.lblCurrentPassword.BorderColor = System.Drawing.Color.DimGray;
            this.lblCurrentPassword.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCurrentPassword.Location = new System.Drawing.Point(122, 185);
            this.lblCurrentPassword.Name = "lblCurrentPassword";
            this.lblCurrentPassword.PasswordChar = "*";
            this.lblCurrentPassword.Size = new System.Drawing.Size(437, 48);
            this.lblCurrentPassword.TabIndex = 222;
            this.lblCurrentPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCurrentPassword.TextChanged += new System.EventHandler(this.textChangedEvent);
            this.lblCurrentPassword.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblNewPassword1
            // 
            this.lblNewPassword1.BackColor = System.Drawing.Color.White;
            this.lblNewPassword1.BorderColor = System.Drawing.Color.DimGray;
            this.lblNewPassword1.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNewPassword1.Location = new System.Drawing.Point(122, 283);
            this.lblNewPassword1.Name = "lblNewPassword1";
            this.lblNewPassword1.PasswordChar = "*";
            this.lblNewPassword1.Size = new System.Drawing.Size(437, 48);
            this.lblNewPassword1.TabIndex = 223;
            this.lblNewPassword1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNewPassword1.TextChanged += new System.EventHandler(this.textChangedEvent);
            this.lblNewPassword1.Click += new System.EventHandler(this.Label_Click);
            // 
            // lblNewPassword2
            // 
            this.lblNewPassword2.BackColor = System.Drawing.Color.White;
            this.lblNewPassword2.BorderColor = System.Drawing.Color.DimGray;
            this.lblNewPassword2.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNewPassword2.Location = new System.Drawing.Point(122, 380);
            this.lblNewPassword2.Name = "lblNewPassword2";
            this.lblNewPassword2.PasswordChar = "*";
            this.lblNewPassword2.Size = new System.Drawing.Size(437, 48);
            this.lblNewPassword2.TabIndex = 224;
            this.lblNewPassword2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblNewPassword2.TextChanged += new System.EventHandler(this.textChangedEvent);
            this.lblNewPassword2.Click += new System.EventHandler(this.Label_Click);
            // 
            // npPad
            // 
            this.npPad.BackColor = System.Drawing.Color.Black;
            this.npPad.IsNumber = true;
            this.npPad.LimitLength = 10;
            this.npPad.LinkedSimpleLabel = null;
            this.npPad.Location = new System.Drawing.Point(692, 100);
            this.npPad.Name = "npPad";
            this.npPad.Size = new System.Drawing.Size(258, 342);
            this.npPad.TabIndex = 155;
            this.npPad.Value = "";
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
            this.btn_TXT_BACK.Font = new System.Drawing.Font("옥션고딕 B", 21.75F);
            this.btn_TXT_BACK.ForeColor = System.Drawing.Color.Yellow;
            this.btn_TXT_BACK.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXT_BACK.Location = new System.Drawing.Point(810, 534);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(138, 43);
            this.btn_TXT_BACK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_BACK.TabIndex = 296;
            this.btn_TXT_BACK.TabStop = false;
            this.btn_TXT_BACK.Tag = "TXT_BACK";
            this.btn_TXT_BACK.Text = "이전화면";
            this.btn_TXT_BACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_BACK.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.TextTopMargin = 2;
            this.btn_TXT_BACK.UsingTextAlignInImage = false;
            this.btn_TXT_BACK.UsingTextShadow = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btn_TXTYES
            // 
            this.btn_TXTYES.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXTYES.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXTYES.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXTYES.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXTYES.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXTYES.DownTextColor = System.Drawing.Color.White;
            this.btn_TXTYES.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXTYES.Font = new System.Drawing.Font("옥션고딕 B", 21.75F);
            this.btn_TXTYES.ForeColor = System.Drawing.Color.Yellow;
            this.btn_TXTYES.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXTYES.Location = new System.Drawing.Point(651, 534);
            this.btn_TXTYES.Name = "btn_TXTYES";
            this.btn_TXTYES.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXTYES.Size = new System.Drawing.Size(138, 43);
            this.btn_TXTYES.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXTYES.TabIndex = 297;
            this.btn_TXTYES.TabStop = false;
            this.btn_TXTYES.Tag = "TXT_ENTER";
            this.btn_TXTYES.Text = "확인";
            this.btn_TXTYES.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXTYES.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXTYES.TextTopMargin = 2;
            this.btn_TXTYES.UsingTextAlignInImage = false;
            this.btn_TXTYES.UsingTextShadow = false;
            this.btn_TXTYES.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormAdminPassword
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1001, 600);
            this.Controls.Add(this.btn_TXTYES);
            this.Controls.Add(this.btn_TXT_BACK);
            this.Controls.Add(this.lblNewPassword2);
            this.Controls.Add(this.lblNewPassword1);
            this.Controls.Add(this.lblCurrentPassword);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.npPad);
            this.Controls.Add(this.lbl_TXT_RESET_PWD);
            this.Controls.Add(this.picBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAdminPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormAdminPassword";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAdminPassword_FormClosed);
            this.Load += new System.EventHandler(this.FormAdminPassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXTYES)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picBackground;
        private System.Windows.Forms.Label lbl_TXT_RESET_PWD;
        private NumberPad npPad;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl1;
        private FadeFox.UI.SimpleLabel lblCurrentPassword;
        private FadeFox.UI.SimpleLabel lblNewPassword1;
        private FadeFox.UI.SimpleLabel lblNewPassword2;
        private FadeFox.UI.ImageButton btn_TXT_BACK;
        private FadeFox.UI.ImageButton btn_TXTYES;
    }
}