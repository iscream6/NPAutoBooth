namespace NPConfig
{
	partial class SerialPortSearch
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
			this.btnOk = new FadeFox.UI.GlassButton();
			this.btnCancel = new FadeFox.UI.GlassButton();
			this.pnlLayoutBottom = new FadeFox.UI.SimplePanel();
			this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
			this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
			this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
			this.lblSubject = new FadeFox.UI.Text3DLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.cboPortName = new System.Windows.Forms.ComboBox();
			this.txtPortName = new System.Windows.Forms.TextBox();
			this.pnlLayoutBottom.SuspendLayout();
			this.pnlControl.SuspendLayout();
			this.pnlLayoutTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.Color.DarkGray;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.EnableFadeInOut = true;
			this.btnOk.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOk.ForeColor = System.Drawing.Color.Black;
			this.btnOk.GlowColor = System.Drawing.Color.White;
			this.btnOk.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnOk.Location = new System.Drawing.Point(5, 3);
			this.btnOk.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(86, 24);
			this.btnOk.TabIndex = 31;
			this.btnOk.Text = "확인(&O)";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.EnableFadeInOut = true;
			this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnCancel.ForeColor = System.Drawing.Color.Black;
			this.btnCancel.GlowColor = System.Drawing.Color.White;
			this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnCancel.Location = new System.Drawing.Point(93, 3);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(86, 24);
			this.btnCancel.TabIndex = 29;
			this.btnCancel.Text = "취소(&C)";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// pnlLayoutBottom
			// 
			this.pnlLayoutBottom.BackColor = System.Drawing.SystemColors.Control;
			this.pnlLayoutBottom.Border1ColorBottom = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border1ColorLeft = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border1ColorRight = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border1ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlLayoutBottom.Border1WidthBottom = 0;
			this.pnlLayoutBottom.Border1WidthLeft = 0;
			this.pnlLayoutBottom.Border1WidthRight = 0;
			this.pnlLayoutBottom.Border1WidthTop = 1;
			this.pnlLayoutBottom.Border2ColorBottom = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border2ColorLeft = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border2ColorRight = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border2ColorTop = System.Drawing.Color.Gray;
			this.pnlLayoutBottom.Border2WidthBottom = 0;
			this.pnlLayoutBottom.Border2WidthLeft = 0;
			this.pnlLayoutBottom.Border2WidthRight = 0;
			this.pnlLayoutBottom.Border2WidthTop = 0;
			this.pnlLayoutBottom.Controls.Add(this.pnlControl);
			this.pnlLayoutBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLayoutBottom.Location = new System.Drawing.Point(0, 92);
			this.pnlLayoutBottom.Name = "pnlLayoutBottom";
			this.pnlLayoutBottom.Size = new System.Drawing.Size(319, 37);
			this.pnlLayoutBottom.TabIndex = 48;
			// 
			// pnlControl
			// 
			this.pnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlControl.Controls.Add(this.btnCancel);
			this.pnlControl.Controls.Add(this.btnOk);
			this.pnlControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlControl.Location = new System.Drawing.Point(135, 4);
			this.pnlControl.Name = "pnlControl";
			this.pnlControl.Size = new System.Drawing.Size(181, 30);
			this.pnlControl.TabIndex = 0;
			// 
			// pnlLayoutTop
			// 
			this.pnlLayoutTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlLayoutTop.Controls.Add(this.pnlCondition);
			this.pnlLayoutTop.Controls.Add(this.lblSubject);
			this.pnlLayoutTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLayoutTop.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.pnlLayoutTop.GradientEnd = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.GradientStart = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLayoutTop.Name = "pnlLayoutTop";
			this.pnlLayoutTop.Size = new System.Drawing.Size(319, 46);
			this.pnlLayoutTop.TabIndex = 71;
			// 
			// pnlCondition
			// 
			this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
			this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlCondition.Location = new System.Drawing.Point(216, 8);
			this.pnlCondition.Name = "pnlCondition";
			this.pnlCondition.Size = new System.Drawing.Size(99, 26);
			this.pnlCondition.TabIndex = 66;
			// 
			// lblSubject
			// 
			this.lblSubject.BackColor = System.Drawing.Color.Transparent;
			this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblSubject.Location = new System.Drawing.Point(10, 10);
			this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(158, 24);
			this.lblSubject.TabIndex = 70;
			this.lblSubject.Text = "시리얼포트 선택";
			this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
			this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.lblSubject.TextColor = System.Drawing.Color.Black;
			this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 62);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(73, 12);
			this.label1.TabIndex = 72;
			this.label1.Text = "시리얼 포트:";
			// 
			// cboPortName
			// 
			this.cboPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboPortName.FormattingEnabled = true;
			this.cboPortName.Location = new System.Drawing.Point(94, 58);
			this.cboPortName.Name = "cboPortName";
			this.cboPortName.Size = new System.Drawing.Size(88, 20);
			this.cboPortName.Sorted = true;
			this.cboPortName.TabIndex = 73;
			this.cboPortName.SelectedIndexChanged += new System.EventHandler(this.cboPortName_SelectedIndexChanged);
			// 
			// txtPortName
			// 
			this.txtPortName.Location = new System.Drawing.Point(186, 58);
			this.txtPortName.MaxLength = 5;
			this.txtPortName.Name = "txtPortName";
			this.txtPortName.Size = new System.Drawing.Size(100, 21);
			this.txtPortName.TabIndex = 74;
			// 
			// SerialPortSearch
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(319, 129);
			this.Controls.Add(this.txtPortName);
			this.Controls.Add(this.cboPortName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pnlLayoutTop);
			this.Controls.Add(this.pnlLayoutBottom);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SerialPortSearch";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "선택";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SerialPortSearch_FormClosed);
			this.Load += new System.EventHandler(this.SerialPortSearch_Load);
			this.Shown += new System.EventHandler(this.SerialPortSearch_Shown);
			this.pnlLayoutBottom.ResumeLayout(false);
			this.pnlControl.ResumeLayout(false);
			this.pnlLayoutTop.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private FadeFox.UI.GlassButton btnCancel;
		private FadeFox.UI.GlassButton btnOk;
		private FadeFox.UI.SimplePanel pnlLayoutBottom;
		private System.Windows.Forms.FlowLayoutPanel pnlControl;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private FadeFox.UI.Text3DLabel lblSubject;
		private System.Windows.Forms.Label label1;
		internal System.Windows.Forms.ComboBox cboPortName;
		private System.Windows.Forms.TextBox txtPortName;
	}
}