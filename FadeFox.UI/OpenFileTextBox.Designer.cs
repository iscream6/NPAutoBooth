namespace FadeFox.UI
{
	partial class OpenFileTextBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenFileTextBox));
			this.txtBody = new System.Windows.Forms.TextBox();
			this.txtOpenFileText = new System.Windows.Forms.TextBox();
			this.ofOpen = new System.Windows.Forms.OpenFileDialog();
			this.btnClear = new FadeFox.UI.FlatButton();
			this.btnSearch = new FadeFox.UI.FlatButton();
			this.SuspendLayout();
			// 
			// txtBody
			// 
			this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtBody.BackColor = System.Drawing.Color.White;
			this.txtBody.Enabled = false;
			this.txtBody.Location = new System.Drawing.Point(0, 0);
			this.txtBody.Name = "txtBody";
			this.txtBody.ReadOnly = true;
			this.txtBody.Size = new System.Drawing.Size(352, 21);
			this.txtBody.TabIndex = 0;
			this.txtBody.TabStop = false;
			// 
			// txtOpenFileText
			// 
			this.txtOpenFileText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOpenFileText.BackColor = System.Drawing.Color.White;
			this.txtOpenFileText.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtOpenFileText.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.txtOpenFileText.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtOpenFileText.Location = new System.Drawing.Point(4, 4);
			this.txtOpenFileText.MinimumSize = new System.Drawing.Size(50, 0);
			this.txtOpenFileText.Name = "txtOpenFileText";
			this.txtOpenFileText.ReadOnly = true;
			this.txtOpenFileText.Size = new System.Drawing.Size(306, 14);
			this.txtOpenFileText.TabIndex = 2;
			this.txtOpenFileText.TabStop = false;
			this.txtOpenFileText.Text = "OpenFileText";
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.ButtonFaceColor = System.Drawing.SystemColors.Window;
			this.btnClear.DisplayFocus = true;
			this.btnClear.FlatButtonBorderColor = System.Drawing.Color.DimGray;
			this.btnClear.FlatButtonBorderHotColor = System.Drawing.Color.DimGray;
			this.btnClear.FlatButtonHotColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(214)))));
			this.btnClear.FlatButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(218)))), ((int)(((byte)(232)))));
			this.btnClear.FlatButtonStyle = FadeFox.UI.FlatButtonStyle.Flat;
			this.btnClear.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
			this.btnClear.Location = new System.Drawing.Point(314, 3);
			this.btnClear.Name = "btnClear";
			this.btnClear.PopupButtonHighlightColor = System.Drawing.SystemColors.ButtonHighlight;
			this.btnClear.PopupButtonPressedColor = System.Drawing.Color.White;
			this.btnClear.PopupButtonShadowColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnClear.Size = new System.Drawing.Size(17, 15);
			this.btnClear.TabIndex = 6;
			this.btnClear.TabStop = false;
			this.btnClear.TextAlign = System.Drawing.StringAlignment.Center;
			this.btnClear.TextColor = System.Drawing.Color.Black;
			this.btnClear.TextHotColor = System.Drawing.Color.Black;
			this.btnClear.TextShadow = false;
			this.btnClear.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.ButtonFaceColor = System.Drawing.SystemColors.Window;
			this.btnSearch.DisplayFocus = true;
			this.btnSearch.FlatButtonBorderColor = System.Drawing.Color.DimGray;
			this.btnSearch.FlatButtonBorderHotColor = System.Drawing.Color.DimGray;
			this.btnSearch.FlatButtonHotColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(214)))));
			this.btnSearch.FlatButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(218)))), ((int)(((byte)(232)))));
			this.btnSearch.FlatButtonStyle = FadeFox.UI.FlatButtonStyle.Flat;
			this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSearch.Location = new System.Drawing.Point(332, 3);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.PopupButtonHighlightColor = System.Drawing.SystemColors.ButtonHighlight;
			this.btnSearch.PopupButtonPressedColor = System.Drawing.Color.White;
			this.btnSearch.PopupButtonShadowColor = System.Drawing.SystemColors.ButtonShadow;
			this.btnSearch.Size = new System.Drawing.Size(17, 15);
			this.btnSearch.TabIndex = 7;
			this.btnSearch.TabStop = false;
			this.btnSearch.Text = "...";
			this.btnSearch.TextAlign = System.Drawing.StringAlignment.Center;
			this.btnSearch.TextColor = System.Drawing.Color.Black;
			this.btnSearch.TextHotColor = System.Drawing.Color.Black;
			this.btnSearch.TextShadow = false;
			this.btnSearch.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// OpenFileTextBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.txtOpenFileText);
			this.Controls.Add(this.txtBody);
			this.Name = "OpenFileTextBox";
			this.Size = new System.Drawing.Size(352, 21);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.TextBox txtOpenFileText;
		private System.Windows.Forms.OpenFileDialog ofOpen;
		private FlatButton btnClear;
		private FlatButton btnSearch;
	}
}
