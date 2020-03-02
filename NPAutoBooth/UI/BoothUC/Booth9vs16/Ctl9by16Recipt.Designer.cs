namespace NPAutoBooth.UI.BoothUC
{
    partial class Ctl9by16Recipt
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_MSG_RECEIPT_PRINT = new System.Windows.Forms.Label();
            this.btnTXT_RECEIPT_YESBUTTON = new FadeFox.UI.ImageButton();
            this.btnTXT_BACK_TOMENU = new FadeFox.UI.ImageButton();
            this.btnEnglish = new FadeFox.UI.ImageButton();
            this.btnJapan = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_RECEIPT_YESBUTTON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_BACK_TOMENU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEnglish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnJapan)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_MSG_RECEIPT_PRINT
            // 
            this.lbl_MSG_RECEIPT_PRINT.BackColor = System.Drawing.Color.Transparent;
            this.lbl_MSG_RECEIPT_PRINT.Font = new System.Drawing.Font("옥션고딕 B", 36.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_MSG_RECEIPT_PRINT.Location = new System.Drawing.Point(70, 769);
            this.lbl_MSG_RECEIPT_PRINT.Name = "lbl_MSG_RECEIPT_PRINT";
            this.lbl_MSG_RECEIPT_PRINT.Size = new System.Drawing.Size(939, 221);
            this.lbl_MSG_RECEIPT_PRINT.TabIndex = 271;
            this.lbl_MSG_RECEIPT_PRINT.Tag = "MSG_RECEIPT_PRINT";
            this.lbl_MSG_RECEIPT_PRINT.Text = "領収書が必要なときは\r\n「領収書」ボタンを押してください。";
            this.lbl_MSG_RECEIPT_PRINT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTXT_RECEIPT_YESBUTTON
            // 
            this.btnTXT_RECEIPT_YESBUTTON.BackColor = System.Drawing.Color.Transparent;
            this.btnTXT_RECEIPT_YESBUTTON.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnTXT_RECEIPT_YESBUTTON.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnTXT_RECEIPT_YESBUTTON.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnTXT_RECEIPT_YESBUTTON.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnTXT_RECEIPT_YESBUTTON.DownTextColor = System.Drawing.Color.White;
            this.btnTXT_RECEIPT_YESBUTTON.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnTXT_RECEIPT_YESBUTTON.Font = new System.Drawing.Font("옥션고딕 B", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTXT_RECEIPT_YESBUTTON.ForeColor = System.Drawing.Color.Green;
            this.btnTXT_RECEIPT_YESBUTTON.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnTXT_RECEIPT_YESBUTTON.Location = new System.Drawing.Point(68, 1339);
            this.btnTXT_RECEIPT_YESBUTTON.Name = "btnTXT_RECEIPT_YESBUTTON";
            this.btnTXT_RECEIPT_YESBUTTON.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnTXT_RECEIPT_YESBUTTON.Size = new System.Drawing.Size(438, 137);
            this.btnTXT_RECEIPT_YESBUTTON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnTXT_RECEIPT_YESBUTTON.TabIndex = 272;
            this.btnTXT_RECEIPT_YESBUTTON.TabStop = false;
            this.btnTXT_RECEIPT_YESBUTTON.Tag = "TXT_RECEIPT_YESBUTTON";
            this.btnTXT_RECEIPT_YESBUTTON.Text = "領収書";
            this.btnTXT_RECEIPT_YESBUTTON.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnTXT_RECEIPT_YESBUTTON.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnTXT_RECEIPT_YESBUTTON.TextTopMargin = 2;
            this.btnTXT_RECEIPT_YESBUTTON.UsingTextAlignInImage = false;
            this.btnTXT_RECEIPT_YESBUTTON.UsingTextShadow = false;
            this.btnTXT_RECEIPT_YESBUTTON.Click += new System.EventHandler(this.btn_Recipt_Click);
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
            this.btnTXT_BACK_TOMENU.Font = new System.Drawing.Font("옥션고딕 B", 39.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTXT_BACK_TOMENU.ForeColor = System.Drawing.Color.Green;
            this.btnTXT_BACK_TOMENU.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnTXT_BACK_TOMENU.Location = new System.Drawing.Point(562, 1339);
            this.btnTXT_BACK_TOMENU.Name = "btnTXT_BACK_TOMENU";
            this.btnTXT_BACK_TOMENU.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnTXT_BACK_TOMENU.Size = new System.Drawing.Size(438, 137);
            this.btnTXT_BACK_TOMENU.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnTXT_BACK_TOMENU.TabIndex = 273;
            this.btnTXT_BACK_TOMENU.TabStop = false;
            this.btnTXT_BACK_TOMENU.Tag = "TXT_BACK_TOMENU";
            this.btnTXT_BACK_TOMENU.Text = "メニューへ";
            this.btnTXT_BACK_TOMENU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnTXT_BACK_TOMENU.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnTXT_BACK_TOMENU.TextTopMargin = 2;
            this.btnTXT_BACK_TOMENU.UsingTextAlignInImage = false;
            this.btnTXT_BACK_TOMENU.UsingTextShadow = false;
            this.btnTXT_BACK_TOMENU.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btnEnglish
            // 
            this.btnEnglish.BackColor = System.Drawing.Color.Transparent;
            this.btnEnglish.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnEnglish.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnEnglish.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnEnglish.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnEnglish.DownTextColor = System.Drawing.Color.White;
            this.btnEnglish.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnEnglish.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold);
            this.btnEnglish.ForeColor = System.Drawing.Color.Green;
            this.btnEnglish.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnEnglish.Location = new System.Drawing.Point(713, 1519);
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnEnglish.Size = new System.Drawing.Size(125, 47);
            this.btnEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnEnglish.TabIndex = 336;
            this.btnEnglish.TabStop = false;
            this.btnEnglish.Tag = "";
            this.btnEnglish.Text = "English";
            this.btnEnglish.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnEnglish.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnEnglish.TextTopMargin = 2;
            this.btnEnglish.UsingTextAlignInImage = false;
            this.btnEnglish.UsingTextShadow = false;
            this.btnEnglish.Click += new System.EventHandler(this.btnEnglish_Click);
            // 
            // btnJapan
            // 
            this.btnJapan.BackColor = System.Drawing.Color.Transparent;
            this.btnJapan.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnJapan.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnJapan.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnJapan.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnJapan.DownTextColor = System.Drawing.Color.White;
            this.btnJapan.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnJapan.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold);
            this.btnJapan.ForeColor = System.Drawing.Color.Green;
            this.btnJapan.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnJapan.Location = new System.Drawing.Point(851, 1519);
            this.btnJapan.Name = "btnJapan";
            this.btnJapan.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnJapan.Size = new System.Drawing.Size(125, 47);
            this.btnJapan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnJapan.TabIndex = 335;
            this.btnJapan.TabStop = false;
            this.btnJapan.Tag = "";
            this.btnJapan.Text = "日本語";
            this.btnJapan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnJapan.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnJapan.TextTopMargin = 2;
            this.btnJapan.UsingTextAlignInImage = false;
            this.btnJapan.UsingTextShadow = false;
            this.btnJapan.Click += new System.EventHandler(this.btnJapan_Click);
            // 
            // Ctl9by16Recipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NPAutoBooth.Properties.Resources.Type2BackGround1080;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnEnglish);
            this.Controls.Add(this.btnJapan);
            this.Controls.Add(this.btnTXT_BACK_TOMENU);
            this.Controls.Add(this.btnTXT_RECEIPT_YESBUTTON);
            this.Controls.Add(this.lbl_MSG_RECEIPT_PRINT);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Ctl9by16Recipt";
            this.Size = new System.Drawing.Size(1080, 1920);
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_RECEIPT_YESBUTTON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTXT_BACK_TOMENU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEnglish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnJapan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_MSG_RECEIPT_PRINT;
        private FadeFox.UI.ImageButton btnTXT_RECEIPT_YESBUTTON;
        private FadeFox.UI.ImageButton btnTXT_BACK_TOMENU;
        private FadeFox.UI.ImageButton btnEnglish;
        private FadeFox.UI.ImageButton btnJapan;
    }
}
