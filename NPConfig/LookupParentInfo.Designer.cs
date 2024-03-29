﻿namespace NPConfig
{
	partial class LookupParentInfo
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.btnClose = new FadeFox.UI.GlassButton();
			this.pnlInput = new FadeFox.UI.SimplePanel();
			this.txtLookupComment = new System.Windows.Forms.TextBox();
			this.txtLookupExtra = new System.Windows.Forms.TextBox();
			this.txtLookupName = new System.Windows.Forms.TextBox();
			this.txtLookupID = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.btnCancel = new FadeFox.UI.GlassButton();
			this.btnSave = new FadeFox.UI.GlassButton();
			this.grdList = new FadeFox.UI.DataGridViewEx();
			this.LOOKUP_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LOOKUP_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LOOKUP_EXTRA = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LOOKUP_COMMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UPDATE_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
			this.btnDelete = new FadeFox.UI.GlassButton();
			this.btnUpdate = new FadeFox.UI.GlassButton();
			this.btnInsert = new FadeFox.UI.GlassButton();
			this.btnDown = new FadeFox.UI.GlassButton();
			this.btnUp = new FadeFox.UI.GlassButton();
			this.chkContinueInsertMode = new System.Windows.Forms.CheckBox();
			this.pnlLayoutBottom = new FadeFox.UI.SimplePanel();
			this.label1 = new System.Windows.Forms.Label();
			this.lblRowCount = new System.Windows.Forms.Label();
			this.pnlContent = new FadeFox.UI.BorderPanelEx();
			this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
			this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
			this.lblSubject = new FadeFox.UI.Text3DLabel();
			this.pnlInput.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
			this.pnlControl.SuspendLayout();
			this.pnlLayoutBottom.SuspendLayout();
			this.pnlContent.SuspendLayout();
			this.pnlLayoutTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.DarkGray;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.EnableFadeInOut = true;
			this.btnClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnClose.ForeColor = System.Drawing.Color.Black;
			this.btnClose.GlowColor = System.Drawing.Color.White;
			this.btnClose.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnClose.Location = new System.Drawing.Point(701, 3);
			this.btnClose.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(86, 24);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "닫기";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// pnlInput
			// 
			this.pnlInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlInput.BackColor = System.Drawing.SystemColors.Control;
			this.pnlInput.Border1ColorBottom = System.Drawing.Color.Gray;
			this.pnlInput.Border1ColorLeft = System.Drawing.Color.Gray;
			this.pnlInput.Border1ColorRight = System.Drawing.Color.Gray;
			this.pnlInput.Border1ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlInput.Border1WidthBottom = 0;
			this.pnlInput.Border1WidthLeft = 0;
			this.pnlInput.Border1WidthRight = 0;
			this.pnlInput.Border1WidthTop = 1;
			this.pnlInput.Border2ColorBottom = System.Drawing.Color.Gray;
			this.pnlInput.Border2ColorLeft = System.Drawing.Color.Gray;
			this.pnlInput.Border2ColorRight = System.Drawing.Color.Gray;
			this.pnlInput.Border2ColorTop = System.Drawing.Color.Gray;
			this.pnlInput.Border2WidthBottom = 0;
			this.pnlInput.Border2WidthLeft = 0;
			this.pnlInput.Border2WidthRight = 0;
			this.pnlInput.Border2WidthTop = 0;
			this.pnlInput.Controls.Add(this.txtLookupComment);
			this.pnlInput.Controls.Add(this.txtLookupExtra);
			this.pnlInput.Controls.Add(this.txtLookupName);
			this.pnlInput.Controls.Add(this.txtLookupID);
			this.pnlInput.Controls.Add(this.label7);
			this.pnlInput.Controls.Add(this.label6);
			this.pnlInput.Controls.Add(this.label5);
			this.pnlInput.Controls.Add(this.label8);
			this.pnlInput.Location = new System.Drawing.Point(0, 374);
			this.pnlInput.Name = "pnlInput";
			this.pnlInput.Size = new System.Drawing.Size(886, 109);
			this.pnlInput.TabIndex = 0;
			// 
			// txtLookupComment
			// 
			this.txtLookupComment.Location = new System.Drawing.Point(44, 80);
			this.txtLookupComment.Name = "txtLookupComment";
			this.txtLookupComment.Size = new System.Drawing.Size(584, 21);
			this.txtLookupComment.TabIndex = 3;
			// 
			// txtLookupExtra
			// 
			this.txtLookupExtra.Location = new System.Drawing.Point(44, 56);
			this.txtLookupExtra.Name = "txtLookupExtra";
			this.txtLookupExtra.Size = new System.Drawing.Size(584, 21);
			this.txtLookupExtra.TabIndex = 2;
			// 
			// txtLookupName
			// 
			this.txtLookupName.Location = new System.Drawing.Point(44, 32);
			this.txtLookupName.Name = "txtLookupName";
			this.txtLookupName.Size = new System.Drawing.Size(584, 21);
			this.txtLookupName.TabIndex = 1;
			// 
			// txtLookupID
			// 
			this.txtLookupID.Location = new System.Drawing.Point(44, 8);
			this.txtLookupID.Name = "txtLookupID";
			this.txtLookupID.Size = new System.Drawing.Size(584, 21);
			this.txtLookupID.TabIndex = 0;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label7.Location = new System.Drawing.Point(8, 14);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(35, 12);
			this.label7.TabIndex = 45;
			this.label7.Text = "코드:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label6.Location = new System.Drawing.Point(8, 37);
			this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(35, 12);
			this.label6.TabIndex = 46;
			this.label6.Text = "이름:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label5.Location = new System.Drawing.Point(8, 60);
			this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 12);
			this.label5.TabIndex = 47;
			this.label5.Text = "기타:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label8.Location = new System.Drawing.Point(8, 83);
			this.label8.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(35, 12);
			this.label8.TabIndex = 44;
			this.label8.Text = "설명:";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
			this.btnCancel.EnableFadeInOut = true;
			this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnCancel.ForeColor = System.Drawing.Color.Black;
			this.btnCancel.GlowColor = System.Drawing.Color.White;
			this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnCancel.Location = new System.Drawing.Point(173, 3);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(86, 24);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "취소";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.DarkGray;
			this.btnSave.EnableFadeInOut = true;
			this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnSave.ForeColor = System.Drawing.Color.Black;
			this.btnSave.GlowColor = System.Drawing.Color.White;
			this.btnSave.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnSave.Location = new System.Drawing.Point(85, 3);
			this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(86, 24);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "저장";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grdList
			// 
			this.grdList.AllowCheckBoxExport = false;
			this.grdList.AllowUserToAddRows = false;
			this.grdList.AllowUserToDeleteRows = false;
			this.grdList.AllowUserToResizeRows = false;
			this.grdList.BackgroundColor = System.Drawing.Color.White;
			this.grdList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.grdList.BorderVisible = true;
			this.grdList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
			this.grdList.ClipboardCopyWithHeaderText = false;
			this.grdList.ClipboardCopyWithHideColumn = false;
			this.grdList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.grdList.ColumnHeadersHeight = 22;
			this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.grdList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LOOKUP_ID,
            this.LOOKUP_NAME,
            this.LOOKUP_EXTRA,
            this.LOOKUP_COMMENT,
            this.UPDATE_DATE});
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.DefaultCellStyle = dataGridViewCellStyle3;
			this.grdList.DefaultSelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			this.grdList.DefaultSelectionForeColor = System.Drawing.Color.Black;
			this.grdList.DisplayMenuCopyColumnWidth = false;
			this.grdList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.grdList.EnableHeadersVisualStyles = false;
			this.grdList.ExportType = FadeFox.UI.ExportTypes.CSV;
			this.grdList.ExportVisibleColumnOnly = true;
			this.grdList.ExportWithHeader = true;
			this.grdList.Location = new System.Drawing.Point(5, 5);
			this.grdList.LostFocusSelectionBackColor = System.Drawing.SystemColors.Control;
			this.grdList.LostFocusSelectionForeColor = System.Drawing.SystemColors.ControlText;
			this.grdList.MultiSelect = false;
			this.grdList.Name = "grdList";
			this.grdList.ProgressBackColor = System.Drawing.Color.White;
			this.grdList.ProgressBarMaximum = 100;
			this.grdList.ProgressBarMinimum = 0;
			this.grdList.ProgressBarText = "";
			this.grdList.ProgressBarValue = 0;
			this.grdList.ProgressCancelButtonEnabled = true;
			this.grdList.ReadOnly = true;
			this.grdList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.grdList.RowHeadersVisible = false;
			this.grdList.RowHeadersWidth = 22;
			this.grdList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.grdList.RowMoved = null;
			this.grdList.RowsAdding = null;
			this.grdList.RowsDataRead = null;
			this.grdList.RowSelected = null;
			this.grdList.RowsPreAdding = null;
			this.grdList.RowTemplate.Height = 21;
			this.grdList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.RunAfterExport = false;
			this.grdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdList.Size = new System.Drawing.Size(876, 318);
			this.grdList.StoredRowForeColor = System.Drawing.Color.Red;
			this.grdList.TabIndex = 0;
			this.grdList.UsingCellMouseDoubleClickCopyClipboard = true;
			this.grdList.UsingCurrentCellDirtyAutoCommit = true;
			this.grdList.UsingExport = true;
			this.grdList.UsingLostFocusSelectionColor = false;
			this.grdList.UsingSort = false;
			// 
			// LOOKUP_ID
			// 
			this.LOOKUP_ID.FillWeight = 130F;
			this.LOOKUP_ID.HeaderText = "코드";
			this.LOOKUP_ID.MaxInputLength = 128;
			this.LOOKUP_ID.Name = "LOOKUP_ID";
			this.LOOKUP_ID.ReadOnly = true;
			this.LOOKUP_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOOKUP_ID.Width = 130;
			// 
			// LOOKUP_NAME
			// 
			dataGridViewCellStyle2.NullValue = null;
			this.LOOKUP_NAME.DefaultCellStyle = dataGridViewCellStyle2;
			this.LOOKUP_NAME.FillWeight = 195F;
			this.LOOKUP_NAME.HeaderText = "이름";
			this.LOOKUP_NAME.MaxInputLength = 128;
			this.LOOKUP_NAME.Name = "LOOKUP_NAME";
			this.LOOKUP_NAME.ReadOnly = true;
			this.LOOKUP_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOOKUP_NAME.Width = 195;
			// 
			// LOOKUP_EXTRA
			// 
			this.LOOKUP_EXTRA.FillWeight = 180F;
			this.LOOKUP_EXTRA.HeaderText = "기타";
			this.LOOKUP_EXTRA.Name = "LOOKUP_EXTRA";
			this.LOOKUP_EXTRA.ReadOnly = true;
			this.LOOKUP_EXTRA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOOKUP_EXTRA.Width = 180;
			// 
			// LOOKUP_COMMENT
			// 
			this.LOOKUP_COMMENT.FillWeight = 330F;
			this.LOOKUP_COMMENT.HeaderText = "설명";
			this.LOOKUP_COMMENT.Name = "LOOKUP_COMMENT";
			this.LOOKUP_COMMENT.ReadOnly = true;
			this.LOOKUP_COMMENT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOOKUP_COMMENT.Width = 330;
			// 
			// UPDATE_DATE
			// 
			this.UPDATE_DATE.HeaderText = "수정일";
			this.UPDATE_DATE.Name = "UPDATE_DATE";
			this.UPDATE_DATE.ReadOnly = true;
			this.UPDATE_DATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.UPDATE_DATE.Visible = false;
			// 
			// pnlControl
			// 
			this.pnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlControl.Controls.Add(this.btnClose);
			this.pnlControl.Controls.Add(this.btnDelete);
			this.pnlControl.Controls.Add(this.btnUpdate);
			this.pnlControl.Controls.Add(this.btnInsert);
			this.pnlControl.Controls.Add(this.btnDown);
			this.pnlControl.Controls.Add(this.btnUp);
			this.pnlControl.Controls.Add(this.btnCancel);
			this.pnlControl.Controls.Add(this.btnSave);
			this.pnlControl.Controls.Add(this.chkContinueInsertMode);
			this.pnlControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlControl.Location = new System.Drawing.Point(94, 4);
			this.pnlControl.Name = "pnlControl";
			this.pnlControl.Size = new System.Drawing.Size(789, 30);
			this.pnlControl.TabIndex = 0;
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.DarkGray;
			this.btnDelete.EnableFadeInOut = true;
			this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDelete.ForeColor = System.Drawing.Color.Black;
			this.btnDelete.GlowColor = System.Drawing.Color.White;
			this.btnDelete.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnDelete.Location = new System.Drawing.Point(613, 3);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(86, 24);
			this.btnDelete.TabIndex = 2;
			this.btnDelete.Text = "삭제";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.BackColor = System.Drawing.Color.DarkGray;
			this.btnUpdate.EnableFadeInOut = true;
			this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnUpdate.ForeColor = System.Drawing.Color.Black;
			this.btnUpdate.GlowColor = System.Drawing.Color.White;
			this.btnUpdate.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnUpdate.Location = new System.Drawing.Point(525, 3);
			this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(86, 24);
			this.btnUpdate.TabIndex = 1;
			this.btnUpdate.Text = "수정";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// btnInsert
			// 
			this.btnInsert.BackColor = System.Drawing.Color.DarkGray;
			this.btnInsert.EnableFadeInOut = true;
			this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnInsert.ForeColor = System.Drawing.Color.Black;
			this.btnInsert.GlowColor = System.Drawing.Color.White;
			this.btnInsert.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnInsert.Location = new System.Drawing.Point(437, 3);
			this.btnInsert.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnInsert.Name = "btnInsert";
			this.btnInsert.Size = new System.Drawing.Size(86, 24);
			this.btnInsert.TabIndex = 0;
			this.btnInsert.Text = "추가";
			this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
			// 
			// btnDown
			// 
			this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDown.BackColor = System.Drawing.Color.DarkGray;
			this.btnDown.EnableFadeInOut = true;
			this.btnDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDown.ForeColor = System.Drawing.Color.Black;
			this.btnDown.GlowColor = System.Drawing.Color.White;
			this.btnDown.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnDown.Location = new System.Drawing.Point(349, 3);
			this.btnDown.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(86, 24);
			this.btnDown.TabIndex = 46;
			this.btnDown.Text = "아래로(&N)";
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUp.BackColor = System.Drawing.Color.DarkGray;
			this.btnUp.EnableFadeInOut = true;
			this.btnUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnUp.ForeColor = System.Drawing.Color.Black;
			this.btnUp.GlowColor = System.Drawing.Color.White;
			this.btnUp.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnUp.Location = new System.Drawing.Point(261, 3);
			this.btnUp.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(86, 24);
			this.btnUp.TabIndex = 45;
			this.btnUp.Text = "위로(&U)";
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// chkContinueInsertMode
			// 
			this.chkContinueInsertMode.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.chkContinueInsertMode.AutoSize = true;
			this.chkContinueInsertMode.Location = new System.Drawing.Point(11, 7);
			this.chkContinueInsertMode.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.chkContinueInsertMode.Name = "chkContinueInsertMode";
			this.chkContinueInsertMode.Size = new System.Drawing.Size(72, 16);
			this.chkContinueInsertMode.TabIndex = 47;
			this.chkContinueInsertMode.Text = "계속추가";
			this.chkContinueInsertMode.UseVisualStyleBackColor = true;
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
			this.pnlLayoutBottom.Controls.Add(this.label1);
			this.pnlLayoutBottom.Controls.Add(this.lblRowCount);
			this.pnlLayoutBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLayoutBottom.Location = new System.Drawing.Point(0, 483);
			this.pnlLayoutBottom.Name = "pnlLayoutBottom";
			this.pnlLayoutBottom.Size = new System.Drawing.Size(886, 37);
			this.pnlLayoutBottom.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 12);
			this.label1.TabIndex = 36;
			this.label1.Text = "건수:";
			// 
			// lblRowCount
			// 
			this.lblRowCount.AutoSize = true;
			this.lblRowCount.Location = new System.Drawing.Point(38, 14);
			this.lblRowCount.Name = "lblRowCount";
			this.lblRowCount.Size = new System.Drawing.Size(11, 12);
			this.lblRowCount.TabIndex = 37;
			this.lblRowCount.Text = "0";
			// 
			// pnlContent
			// 
			this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlContent.BackColor = System.Drawing.Color.Transparent;
			this.pnlContent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlContent.Controls.Add(this.grdList);
			this.pnlContent.Location = new System.Drawing.Point(0, 44);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(886, 328);
			this.pnlContent.TabIndex = 24;
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
			this.pnlLayoutTop.Size = new System.Drawing.Size(886, 46);
			this.pnlLayoutTop.TabIndex = 25;
			// 
			// pnlCondition
			// 
			this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
			this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlCondition.Location = new System.Drawing.Point(534, 8);
			this.pnlCondition.Name = "pnlCondition";
			this.pnlCondition.Size = new System.Drawing.Size(348, 26);
			this.pnlCondition.TabIndex = 66;
			// 
			// lblSubject
			// 
			this.lblSubject.BackColor = System.Drawing.Color.Transparent;
			this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblSubject.Location = new System.Drawing.Point(10, 10);
			this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(220, 24);
			this.lblSubject.TabIndex = 70;
			this.lblSubject.Text = "구분관리";
			this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
			this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.lblSubject.TextColor = System.Drawing.Color.Black;
			this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
			this.lblSubject.TextTopMargin = 2;
			// 
			// LookupParentInfo
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(886, 520);
			this.Controls.Add(this.pnlLayoutTop);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlLayoutBottom);
			this.Controls.Add(this.pnlInput);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LookupParentInfo";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LookupParentInfo_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LookupParentInfo_FormClosed);
			this.Load += new System.EventHandler(this.LookupParentInfo_Load);
			this.Shown += new System.EventHandler(this.LookupParentInfo_Shown);
			this.pnlInput.ResumeLayout(false);
			this.pnlInput.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
			this.pnlControl.ResumeLayout(false);
			this.pnlControl.PerformLayout();
			this.pnlLayoutBottom.ResumeLayout(false);
			this.pnlLayoutBottom.PerformLayout();
			this.pnlContent.ResumeLayout(false);
			this.pnlLayoutTop.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private FadeFox.UI.GlassButton btnClose;
		private FadeFox.UI.SimplePanel pnlInput;
		private FadeFox.UI.DataGridViewEx grdList;
		private System.Windows.Forms.FlowLayoutPanel pnlControl;
		private FadeFox.UI.GlassButton btnCancel;
		private FadeFox.UI.GlassButton btnDelete;
		private FadeFox.UI.GlassButton btnUpdate;
		private FadeFox.UI.GlassButton btnInsert;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private FadeFox.UI.SimplePanel pnlLayoutBottom;
		private FadeFox.UI.GlassButton btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRowCount;
		private System.Windows.Forms.TextBox txtLookupComment;
		private System.Windows.Forms.TextBox txtLookupExtra;
		private System.Windows.Forms.TextBox txtLookupName;
		private System.Windows.Forms.TextBox txtLookupID;
		private FadeFox.UI.GlassButton btnDown;
		private FadeFox.UI.GlassButton btnUp;
		private System.Windows.Forms.CheckBox chkContinueInsertMode;
		private FadeFox.UI.BorderPanelEx pnlContent;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private System.Windows.Forms.DataGridViewTextBoxColumn LOOKUP_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn LOOKUP_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn LOOKUP_EXTRA;
		private System.Windows.Forms.DataGridViewTextBoxColumn LOOKUP_COMMENT;
		private System.Windows.Forms.DataGridViewTextBoxColumn UPDATE_DATE;
		private FadeFox.UI.Text3DLabel lblSubject;
	}
}