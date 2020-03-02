namespace NPAutoBooth.UI
{
    partial class FormMessageBox
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
            this.PicBackGroud = new System.Windows.Forms.PictureBox();
            this.lbl_message = new System.Windows.Forms.Label();
            this.btn_cancle = new System.Windows.Forms.PictureBox();
            this.btn_OK = new System.Windows.Forms.PictureBox();
            this.lbl_message2 = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PicBackGroud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            this.SuspendLayout();
            // 
            // PicBackGroud
            // 
            this.PicBackGroud.BackgroundImage = global::NPAutoBooth.Properties.Resources.OLD_배경_환경설정;
            this.PicBackGroud.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PicBackGroud.Location = new System.Drawing.Point(0, 0);
            this.PicBackGroud.Name = "PicBackGroud";
            this.PicBackGroud.Size = new System.Drawing.Size(993, 733);
            this.PicBackGroud.TabIndex = 0;
            this.PicBackGroud.TabStop = false;
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Transparent;
            this.lbl_message.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_message.Location = new System.Drawing.Point(91, 198);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(799, 95);
            this.lbl_message.TabIndex = 1;
            this.lbl_message.Text = "현재차량은 무료차량이며 결제가\r\n가능합니다. 결제시 10분안에\r\n";
            // 
            // btn_cancle
            // 
            this.btn_cancle.BackColor = System.Drawing.Color.Transparent;
            this.btn_cancle.BackgroundImage = global::NPAutoBooth.Properties.Resources.OLD_버튼_질문멘트_취소_파랑;
            this.btn_cancle.Location = new System.Drawing.Point(515, 451);
            this.btn_cancle.Margin = new System.Windows.Forms.Padding(0);
            this.btn_cancle.Name = "btn_cancle";
            this.btn_cancle.Size = new System.Drawing.Size(134, 68);
            this.btn_cancle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_cancle.TabIndex = 104;
            this.btn_cancle.TabStop = false;
            this.btn_cancle.Click += new System.EventHandler(this.btn_cancle_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.Transparent;
            this.btn_OK.BackgroundImage = global::NPAutoBooth.Properties.Resources.OLD_버튼_질문멘트_예_청색;
            this.btn_OK.Location = new System.Drawing.Point(296, 451);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(134, 68);
            this.btn_OK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_OK.TabIndex = 103;
            this.btn_OK.TabStop = false;
            this.btn_OK.UseWaitCursor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // lbl_message2
            // 
            this.lbl_message2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_message2.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_message2.Location = new System.Drawing.Point(91, 306);
            this.lbl_message2.Name = "lbl_message2";
            this.lbl_message2.Size = new System.Drawing.Size(799, 117);
            this.lbl_message2.TabIndex = 105;
            this.lbl_message2.Text = "나가셔야  요금부과가 안됩니다.\r\n요금결제를 하시겠습니까?\r\n";
            // 
            // lbltitle
            // 
            this.lbltitle.BackColor = System.Drawing.Color.Transparent;
            this.lbltitle.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold);
            this.lbltitle.Location = new System.Drawing.Point(95, 102);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(211, 33);
            this.lbltitle.TabIndex = 106;
            this.lbltitle.Text = "관리자메뉴";
            this.lbltitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(993, 733);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.lbl_message2);
            this.Controls.Add(this.btn_cancle);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.PicBackGroud);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMessageBox";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMessageBox_FormClosed);
            this.Load += new System.EventHandler(this.FormMessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PicBackGroud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PicBackGroud;
        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.PictureBox btn_cancle;
        private System.Windows.Forms.PictureBox btn_OK;
        private System.Windows.Forms.Label lbl_message2;
        private System.Windows.Forms.Label lbltitle;
    }
}