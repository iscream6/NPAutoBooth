namespace NPAutoBooth.UI.BoothUC
{
    partial class Ctl4by3Information
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
            this.lbl_Msg1 = new System.Windows.Forms.Label();
            this.lbl_Msg2 = new System.Windows.Forms.Label();
            this.btnPreForm = new FadeFox.UI.NPButton();
            this.SuspendLayout();
            // 
            // lbl_Msg1
            // 
            this.lbl_Msg1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg1.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msg1.Location = new System.Drawing.Point(31, 239);
            this.lbl_Msg1.Name = "lbl_Msg1";
            this.lbl_Msg1.Size = new System.Drawing.Size(968, 83);
            this.lbl_Msg1.TabIndex = 322;
            this.lbl_Msg1.Tag = "";
            this.lbl_Msg1.Text = "죄송합니다. 거르름돈이 부족히야 보관증이 출력됩니다";
            this.lbl_Msg1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Msg2
            // 
            this.lbl_Msg2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Msg2.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Msg2.Location = new System.Drawing.Point(31, 341);
            this.lbl_Msg2.Name = "lbl_Msg2";
            this.lbl_Msg2.Size = new System.Drawing.Size(968, 92);
            this.lbl_Msg2.TabIndex = 323;
            this.lbl_Msg2.Tag = "";
            this.lbl_Msg2.Text = "관리실로 가져오시면 거스름돈을 받으실수 있습니다.";
            this.lbl_Msg2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPreForm
            // 
            this.btnPreForm.Font = new System.Drawing.Font("옥션고딕 B", 32F);
            this.btnPreForm.Location = new System.Drawing.Point(420, 644);
            this.btnPreForm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPreForm.Name = "btnPreForm";
            this.btnPreForm.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnPreForm.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnPreForm.NPDisableBackColor = System.Drawing.Color.White;
            this.btnPreForm.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnPreForm.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnPreForm.NPDownForeColor = System.Drawing.Color.White;
            this.btnPreForm.NPFontSize = 32F;
            this.btnPreForm.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnPreForm.NPHoverForeColor = System.Drawing.Color.White;
            this.btnPreForm.NPLanguageCode = "TXT_BACK";
            this.btnPreForm.NPNormalBackColor = System.Drawing.Color.White;
            this.btnPreForm.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnPreForm.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnPreForm.Size = new System.Drawing.Size(197, 65);
            this.btnPreForm.TabIndex = 0;
            this.btnPreForm.Text = "이전화면";
            this.btnPreForm.UseVisualStyleBackColor = false;
            this.btnPreForm.Click += new System.EventHandler(this.btnPreForm_Click);
            // 
            // Ctl4by3Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NPAutoBooth.Properties.Resources.Type2BackGround;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnPreForm);
            this.Controls.Add(this.lbl_Msg2);
            this.Controls.Add(this.lbl_Msg1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Ctl4by3Information";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);

        }

        #endregion
        private FadeFox.UI.NPButton btnPreForm;
        private System.Windows.Forms.Label lbl_Msg1;
        private System.Windows.Forms.Label lbl_Msg2;
    }
}
