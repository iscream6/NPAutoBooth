namespace NPAutoBooth
{
    partial class UserControl_SearchCar
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.pic_SelectArrow = new System.Windows.Forms.PictureBox();
            this.lblFourCarNumber = new System.Windows.Forms.Label();
            this.lblThreeCarNumber = new System.Windows.Forms.Label();
            this.lblTwoCarNumber = new System.Windows.Forms.Label();
            this.lblOneCarNumber = new System.Windows.Forms.Label();
            this.lblFiveCarNumber = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_SelectArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_SelectArrow
            // 
            this.pic_SelectArrow.BackColor = System.Drawing.Color.Transparent;
            this.pic_SelectArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pic_SelectArrow.Image = global::NPAutoBooth.Properties.Resources.Type2_Image_CarSearchTextBar;
            this.pic_SelectArrow.Location = new System.Drawing.Point(0, 0);
            this.pic_SelectArrow.Name = "pic_SelectArrow";
            this.pic_SelectArrow.Size = new System.Drawing.Size(763, 151);
            this.pic_SelectArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_SelectArrow.TabIndex = 237;
            this.pic_SelectArrow.TabStop = false;
            // 
            // lblFourCarNumber
            // 
            this.lblFourCarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblFourCarNumber.Font = new System.Drawing.Font("옥션고딕 B", 110F);
            this.lblFourCarNumber.Location = new System.Drawing.Point(470, 9);
            this.lblFourCarNumber.Name = "lblFourCarNumber";
            this.lblFourCarNumber.Size = new System.Drawing.Size(118, 134);
            this.lblFourCarNumber.TabIndex = 241;
            this.lblFourCarNumber.Text = "7";
            this.lblFourCarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblThreeCarNumber
            // 
            this.lblThreeCarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblThreeCarNumber.Font = new System.Drawing.Font("옥션고딕 B", 110F);
            this.lblThreeCarNumber.Location = new System.Drawing.Point(320, 9);
            this.lblThreeCarNumber.Name = "lblThreeCarNumber";
            this.lblThreeCarNumber.Size = new System.Drawing.Size(118, 134);
            this.lblThreeCarNumber.TabIndex = 240;
            this.lblThreeCarNumber.Text = "8";
            this.lblThreeCarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTwoCarNumber
            // 
            this.lblTwoCarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblTwoCarNumber.Font = new System.Drawing.Font("옥션고딕 B", 110F);
            this.lblTwoCarNumber.Location = new System.Drawing.Point(170, 9);
            this.lblTwoCarNumber.Name = "lblTwoCarNumber";
            this.lblTwoCarNumber.Size = new System.Drawing.Size(118, 134);
            this.lblTwoCarNumber.TabIndex = 239;
            this.lblTwoCarNumber.Text = "9";
            this.lblTwoCarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOneCarNumber
            // 
            this.lblOneCarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblOneCarNumber.Font = new System.Drawing.Font("옥션고딕 B", 110F);
            this.lblOneCarNumber.Location = new System.Drawing.Point(20, 9);
            this.lblOneCarNumber.Name = "lblOneCarNumber";
            this.lblOneCarNumber.Size = new System.Drawing.Size(118, 134);
            this.lblOneCarNumber.TabIndex = 238;
            this.lblOneCarNumber.Text = "9";
            this.lblOneCarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFiveCarNumber
            // 
            this.lblFiveCarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblFiveCarNumber.Font = new System.Drawing.Font("옥션고딕 B", 110F);
            this.lblFiveCarNumber.Location = new System.Drawing.Point(620, 9);
            this.lblFiveCarNumber.Name = "lblFiveCarNumber";
            this.lblFiveCarNumber.Size = new System.Drawing.Size(118, 134);
            this.lblFiveCarNumber.TabIndex = 242;
            this.lblFiveCarNumber.Text = "7";
            this.lblFiveCarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserControl_SearchCar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblFiveCarNumber);
            this.Controls.Add(this.lblFourCarNumber);
            this.Controls.Add(this.lblThreeCarNumber);
            this.Controls.Add(this.lblTwoCarNumber);
            this.Controls.Add(this.lblOneCarNumber);
            this.Controls.Add(this.pic_SelectArrow);
            this.Name = "UserControl_SearchCar";
            this.Size = new System.Drawing.Size(763, 151);
            this.Load += new System.EventHandler(this.UserControl_SearchCar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_SelectArrow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_SelectArrow;
        private System.Windows.Forms.Label lblFourCarNumber;
        private System.Windows.Forms.Label lblThreeCarNumber;
        private System.Windows.Forms.Label lblTwoCarNumber;
        private System.Windows.Forms.Label lblOneCarNumber;
        private System.Windows.Forms.Label lblFiveCarNumber;
    }
}
