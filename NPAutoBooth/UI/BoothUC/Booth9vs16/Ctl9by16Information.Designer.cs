namespace NPAutoBooth.UI.BoothUC
{
    partial class Ctl9by16Information
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
            this.lbl_Msg2 = new System.Windows.Forms.Label();
            this.lbl_Msg1 = new System.Windows.Forms.Label();
            this.lblPremoveTextMsg = new System.Windows.Forms.Label();
            this.btn_TXT_BACK = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Msg2
            // 
            this.lbl_Msg2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg2.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msg2.Location = new System.Drawing.Point(59, 973);
            this.lbl_Msg2.Name = "lbl_Msg2";
            this.lbl_Msg2.Size = new System.Drawing.Size(962, 92);
            this.lbl_Msg2.TabIndex = 324;
            this.lbl_Msg2.Tag = "";
            this.lbl_Msg2.Text = "관리실로 가져오시면 거스름돈을 받으실수 있습니다.";
            this.lbl_Msg2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Msg1
            // 
            this.lbl_Msg1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg1.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msg1.Location = new System.Drawing.Point(59, 856);
            this.lbl_Msg1.Name = "lbl_Msg1";
            this.lbl_Msg1.Size = new System.Drawing.Size(962, 83);
            this.lbl_Msg1.TabIndex = 323;
            this.lbl_Msg1.Tag = "";
            this.lbl_Msg1.Text = "죄송합니다. 거르름돈이 부족히야 보관증이 출력됩니다";
            this.lbl_Msg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPremoveTextMsg
            // 
            this.lblPremoveTextMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblPremoveTextMsg.Font = new System.Drawing.Font("옥션고딕 B", 22F);
            this.lblPremoveTextMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.lblPremoveTextMsg.Location = new System.Drawing.Point(457, 1813);
            this.lblPremoveTextMsg.Name = "lblPremoveTextMsg";
            this.lblPremoveTextMsg.Size = new System.Drawing.Size(176, 46);
            this.lblPremoveTextMsg.TabIndex = 341;
            this.lblPremoveTextMsg.Tag = "TXT_BACK";
            this.lblPremoveTextMsg.Text = "이전화면";
            this.lblPremoveTextMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPremoveTextMsg.Click += new System.EventHandler(this.btnPreForm_Click);
            // 
            // btn_TXT_BACK
            // 
            this.btn_TXT_BACK.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_BACK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_BACK.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_off;
            this.btn_TXT_BACK.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_BACK.DownImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_on;
            this.btn_TXT_BACK.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btn_TXT_BACK.ForeColor = System.Drawing.Color.Green;
            this.btn_TXT_BACK.HoverImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_on;
            this.btn_TXT_BACK.Location = new System.Drawing.Point(457, 1672);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NormalImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_off;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(176, 127);
            this.btn_TXT_BACK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_BACK.TabIndex = 340;
            this.btn_TXT_BACK.TabStop = false;
            this.btn_TXT_BACK.Tag = "";
            this.btn_TXT_BACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_BACK.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.TextTopMargin = 2;
            this.btn_TXT_BACK.UsingTextAlignInImage = false;
            this.btn_TXT_BACK.UsingTextShadow = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btnPreForm_Click);
            // 
            // Ctl9by16Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NPAutoBooth.Properties.Resources.Type2BackGround1080;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblPremoveTextMsg);
            this.Controls.Add(this.btn_TXT_BACK);
            this.Controls.Add(this.lbl_Msg2);
            this.Controls.Add(this.lbl_Msg1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Ctl9by16Information";
            this.Size = new System.Drawing.Size(1080, 1920);
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Msg2;
        private System.Windows.Forms.Label lbl_Msg1;
        private System.Windows.Forms.Label lblPremoveTextMsg;
        private FadeFox.UI.ImageButton btn_TXT_BACK;
    }
}
