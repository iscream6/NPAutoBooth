namespace FadeFox.UI
{
	partial class MsgBoxForm
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
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.txtMessage = new System.Windows.Forms.RichTextBox();
			this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
			this.btnOk = new FadeFox.UI.GlassButton();
			this.btnNo = new FadeFox.UI.GlassButton();
			this.btnYes = new FadeFox.UI.GlassButton();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.pnlContent.SuspendLayout();
			this.pnlCondition.SuspendLayout();
			this.SuspendLayout();
			// 
			// picIcon
			// 
			this.picIcon.Image = global::FadeFox.UI.Properties.Resources.MsgBoxError;
			this.picIcon.Location = new System.Drawing.Point(18, 16);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(48, 48);
			this.picIcon.TabIndex = 1;
			this.picIcon.TabStop = false;
			// 
			// pnlContent
			// 
			this.pnlContent.BackColor = System.Drawing.Color.White;
			this.pnlContent.Controls.Add(this.txtMessage);
			this.pnlContent.Controls.Add(this.picIcon);
			this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlContent.Location = new System.Drawing.Point(0, 0);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(466, 130);
			this.pnlContent.TabIndex = 31;
			// 
			// txtMessage
			// 
			this.txtMessage.BackColor = System.Drawing.Color.White;
			this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtMessage.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtMessage.Location = new System.Drawing.Point(78, 20);
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.Size = new System.Drawing.Size(372, 94);
			this.txtMessage.TabIndex = 2;
			this.txtMessage.Text = "";
			// 
			// pnlCondition
			// 
			this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
			this.pnlCondition.Controls.Add(this.btnOk);
			this.pnlCondition.Controls.Add(this.btnNo);
			this.pnlCondition.Controls.Add(this.btnYes);
			this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlCondition.Location = new System.Drawing.Point(5, 140);
			this.pnlCondition.Name = "pnlCondition";
			this.pnlCondition.Size = new System.Drawing.Size(454, 26);
			this.pnlCondition.TabIndex = 67;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnOk.BackColor = System.Drawing.Color.DarkGray;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.EnableFadeInOut = true;
			this.btnOk.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOk.ForeColor = System.Drawing.Color.Black;
			this.btnOk.GlowColor = System.Drawing.Color.White;
			this.btnOk.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnOk.Location = new System.Drawing.Point(366, 0);
			this.btnOk.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(86, 24);
			this.btnOk.TabIndex = 29;
			this.btnOk.Text = "확인(&O)";
			// 
			// btnNo
			// 
			this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnNo.BackColor = System.Drawing.Color.DarkGray;
			this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.btnNo.EnableFadeInOut = true;
			this.btnNo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnNo.ForeColor = System.Drawing.Color.Black;
			this.btnNo.GlowColor = System.Drawing.Color.White;
			this.btnNo.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnNo.Location = new System.Drawing.Point(278, 0);
			this.btnNo.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnNo.Name = "btnNo";
			this.btnNo.Size = new System.Drawing.Size(86, 24);
			this.btnNo.TabIndex = 30;
			this.btnNo.Text = "아니오(&N)";
			// 
			// btnYes
			// 
			this.btnYes.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnYes.BackColor = System.Drawing.Color.DarkGray;
			this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnYes.EnableFadeInOut = true;
			this.btnYes.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnYes.ForeColor = System.Drawing.Color.Black;
			this.btnYes.GlowColor = System.Drawing.Color.White;
			this.btnYes.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnYes.Location = new System.Drawing.Point(190, 0);
			this.btnYes.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnYes.Name = "btnYes";
			this.btnYes.Size = new System.Drawing.Size(86, 24);
			this.btnYes.TabIndex = 42;
			this.btnYes.Text = "예(Y)";
			// 
			// MsgBoxForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(466, 172);
			this.ControlBox = false;
			this.Controls.Add(this.pnlCondition);
			this.Controls.Add(this.pnlContent);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MsgBoxForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "메시지";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.MsgBoxForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.pnlContent.ResumeLayout(false);
			this.pnlCondition.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Panel pnlContent;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private GlassButton btnOk;
		private GlassButton btnNo;
		private GlassButton btnYes;
		private System.Windows.Forms.RichTextBox txtMessage;
	}
}