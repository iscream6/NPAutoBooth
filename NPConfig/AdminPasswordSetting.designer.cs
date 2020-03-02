namespace NPConfig
{
	partial class AdminPasswordSetting
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
            this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
            this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSubject = new FadeFox.UI.Text3DLabel();
            this.txtPassword1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLayoutBottom = new FadeFox.UI.SimplePanel();
            this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new FadeFox.UI.GlassButton();
            this.btnOk = new FadeFox.UI.GlassButton();
            this.txtPassword2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLayoutTop.SuspendLayout();
            this.pnlLayoutBottom.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
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
            this.pnlLayoutTop.TabIndex = 72;
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
            this.lblSubject.Margin = new System.Windows.Forms.Padding(4);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(158, 24);
            this.lblSubject.TabIndex = 70;
            this.lblSubject.Text = "관리자 암호 설정";
            this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
            this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.lblSubject.TextColor = System.Drawing.Color.Black;
            this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
            this.lblSubject.TextTopMargin = 2;
            // 
            // txtPassword1
            // 
            this.txtPassword1.Location = new System.Drawing.Point(116, 58);
            this.txtPassword1.MaxLength = 10;
            this.txtPassword1.Name = "txtPassword1";
            this.txtPassword1.PasswordChar = '*';
            this.txtPassword1.Size = new System.Drawing.Size(100, 21);
            this.txtPassword1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 75;
            this.label1.Text = "암호:";
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
            this.pnlLayoutBottom.Location = new System.Drawing.Point(0, 114);
            this.pnlLayoutBottom.Name = "pnlLayoutBottom";
            this.pnlLayoutBottom.Size = new System.Drawing.Size(319, 37);
            this.pnlLayoutBottom.TabIndex = 2;
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
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.EnableFadeInOut = true;
            this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.GlowColor = System.Drawing.Color.White;
            this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnCancel.Location = new System.Drawing.Point(93, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.DarkGray;
            this.btnOk.EnableFadeInOut = true;
            this.btnOk.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.Color.Black;
            this.btnOk.GlowColor = System.Drawing.Color.White;
            this.btnOk.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnOk.Location = new System.Drawing.Point(5, 3);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 24);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "확인(&O)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPassword2
            // 
            this.txtPassword2.Location = new System.Drawing.Point(116, 82);
            this.txtPassword2.MaxLength = 10;
            this.txtPassword2.Name = "txtPassword2";
            this.txtPassword2.PasswordChar = '*';
            this.txtPassword2.Size = new System.Drawing.Size(100, 21);
            this.txtPassword2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 78;
            this.label2.Text = "다시입력:";
            // 
            // AdminPasswordSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(319, 151);
            this.Controls.Add(this.txtPassword2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlLayoutBottom);
            this.Controls.Add(this.txtPassword1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlLayoutTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdminPasswordSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AdminPasswordSetting";
            this.Load += new System.EventHandler(this.AdminPasswordSetting_Load);
            this.pnlLayoutTop.ResumeLayout(false);
            this.pnlLayoutBottom.ResumeLayout(false);
            this.pnlControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private FadeFox.UI.Text3DLabel lblSubject;
		private System.Windows.Forms.TextBox txtPassword1;
		private System.Windows.Forms.Label label1;
		private FadeFox.UI.SimplePanel pnlLayoutBottom;
		private System.Windows.Forms.FlowLayoutPanel pnlControl;
		private FadeFox.UI.GlassButton btnCancel;
		private FadeFox.UI.GlassButton btnOk;
		private System.Windows.Forms.TextBox txtPassword2;
		private System.Windows.Forms.Label label2;
	}
}