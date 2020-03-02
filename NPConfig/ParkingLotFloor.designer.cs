namespace NPConfig
{
	partial class ParkingLotFloor
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.grdList = new FadeFox.UI.DataGridViewEx();
			this.lblRowCount = new System.Windows.Forms.Label();
			this.btnInsert = new FadeFox.UI.GlassButton();
			this.btnUpdate = new FadeFox.UI.GlassButton();
			this.btnDelete = new FadeFox.UI.GlassButton();
			this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
			this.btnClose = new FadeFox.UI.GlassButton();
			this.btnSearch = new FadeFox.UI.GlassButton();
			this.cboParkingLot = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnDown = new FadeFox.UI.GlassButton();
			this.btnUp = new FadeFox.UI.GlassButton();
			this.txtMapCode = new System.Windows.Forms.TextBox();
			this.txtFloorName = new System.Windows.Forms.TextBox();
			this.txtFloorCode = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnCancel = new FadeFox.UI.GlassButton();
			this.btnSave = new FadeFox.UI.GlassButton();
			this.pnlLayoutButtom = new FadeFox.UI.SimplePanel();
			this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
			this.chkContinueInsertMode = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlContent = new FadeFox.UI.BorderPanelEx();
			this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
			this.lblSubject = new FadeFox.UI.Text3DLabel();
			this.pnlInput = new FadeFox.UI.SimplePanel();
			this.PARKINGLOT_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FLOOR_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FLOOR_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MAP_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.COMMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.UPDATE_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
			this.pnlCondition.SuspendLayout();
			this.pnlLayoutButtom.SuspendLayout();
			this.pnlControl.SuspendLayout();
			this.pnlContent.SuspendLayout();
			this.pnlLayoutTop.SuspendLayout();
			this.pnlInput.SuspendLayout();
			this.SuspendLayout();
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
            this.PARKINGLOT_NO,
            this.FLOOR_CODE,
            this.FLOOR_NAME,
            this.MAP_CODE,
            this.COMMENT,
            this.UPDATE_DATE});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.DefaultCellStyle = dataGridViewCellStyle2;
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
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
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
			this.grdList.Size = new System.Drawing.Size(982, 414);
			this.grdList.StoredRowForeColor = System.Drawing.Color.Red;
			this.grdList.TabIndex = 22;
			this.grdList.UsingCellMouseDoubleClickCopyClipboard = true;
			this.grdList.UsingCurrentCellDirtyAutoCommit = true;
			this.grdList.UsingExport = true;
			this.grdList.UsingLostFocusSelectionColor = false;
			this.grdList.UsingSort = false;
			// 
			// lblRowCount
			// 
			this.lblRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblRowCount.AutoSize = true;
			this.lblRowCount.Location = new System.Drawing.Point(38, 14);
			this.lblRowCount.Name = "lblRowCount";
			this.lblRowCount.Size = new System.Drawing.Size(11, 12);
			this.lblRowCount.TabIndex = 35;
			this.lblRowCount.Text = "0";
			// 
			// btnInsert
			// 
			this.btnInsert.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnInsert.BackColor = System.Drawing.Color.DarkGray;
			this.btnInsert.EnableFadeInOut = true;
			this.btnInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnInsert.ForeColor = System.Drawing.Color.Black;
			this.btnInsert.GlowColor = System.Drawing.Color.White;
			this.btnInsert.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnInsert.Location = new System.Drawing.Point(442, 3);
			this.btnInsert.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnInsert.Name = "btnInsert";
			this.btnInsert.Size = new System.Drawing.Size(86, 24);
			this.btnInsert.TabIndex = 33;
			this.btnInsert.Text = "추가(&I)";
			this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnUpdate.BackColor = System.Drawing.Color.DarkGray;
			this.btnUpdate.EnableFadeInOut = true;
			this.btnUpdate.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnUpdate.ForeColor = System.Drawing.Color.Black;
			this.btnUpdate.GlowColor = System.Drawing.Color.White;
			this.btnUpdate.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnUpdate.Location = new System.Drawing.Point(530, 3);
			this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(86, 24);
			this.btnUpdate.TabIndex = 32;
			this.btnUpdate.Text = "수정(&E)";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnDelete.BackColor = System.Drawing.Color.DarkGray;
			this.btnDelete.EnableFadeInOut = true;
			this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDelete.ForeColor = System.Drawing.Color.Black;
			this.btnDelete.GlowColor = System.Drawing.Color.White;
			this.btnDelete.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnDelete.Location = new System.Drawing.Point(618, 3);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(86, 24);
			this.btnDelete.TabIndex = 31;
			this.btnDelete.Text = "삭제(&D)";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// pnlCondition
			// 
			this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
			this.pnlCondition.Controls.Add(this.btnClose);
			this.pnlCondition.Controls.Add(this.btnSearch);
			this.pnlCondition.Controls.Add(this.cboParkingLot);
			this.pnlCondition.Controls.Add(this.label3);
			this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlCondition.Location = new System.Drawing.Point(528, 8);
			this.pnlCondition.Name = "pnlCondition";
			this.pnlCondition.Size = new System.Drawing.Size(460, 26);
			this.pnlCondition.TabIndex = 66;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnClose.BackColor = System.Drawing.Color.DarkGray;
			this.btnClose.EnableFadeInOut = true;
			this.btnClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnClose.ForeColor = System.Drawing.Color.Black;
			this.btnClose.GlowColor = System.Drawing.Color.White;
			this.btnClose.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnClose.Location = new System.Drawing.Point(372, 0);
			this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(86, 24);
			this.btnClose.TabIndex = 29;
			this.btnClose.Text = "닫기(&C)";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnSearch.BackColor = System.Drawing.Color.DarkGray;
			this.btnSearch.EnableFadeInOut = true;
			this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnSearch.ForeColor = System.Drawing.Color.Black;
			this.btnSearch.GlowColor = System.Drawing.Color.White;
			this.btnSearch.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnSearch.Location = new System.Drawing.Point(284, 0);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(86, 24);
			this.btnSearch.TabIndex = 30;
			this.btnSearch.Text = "검색(&S)";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// cboParkingLot
			// 
			this.cboParkingLot.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.cboParkingLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboParkingLot.FormattingEnabled = true;
			this.cboParkingLot.Location = new System.Drawing.Point(58, 3);
			this.cboParkingLot.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
			this.cboParkingLot.MaxDropDownItems = 20;
			this.cboParkingLot.Name = "cboParkingLot";
			this.cboParkingLot.Size = new System.Drawing.Size(224, 20);
			this.cboParkingLot.TabIndex = 38;
			this.cboParkingLot.SelectedIndexChanged += new System.EventHandler(this.cboKind_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(7, 7);
			this.label3.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 12);
			this.label3.TabIndex = 37;
			this.label3.Text = "주차장 :";
			// 
			// btnDown
			// 
			this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnDown.BackColor = System.Drawing.Color.DarkGray;
			this.btnDown.EnableFadeInOut = true;
			this.btnDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDown.ForeColor = System.Drawing.Color.Black;
			this.btnDown.GlowColor = System.Drawing.Color.White;
			this.btnDown.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnDown.Location = new System.Drawing.Point(354, 3);
			this.btnDown.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnDown.Name = "btnDown";
			this.btnDown.Size = new System.Drawing.Size(86, 24);
			this.btnDown.TabIndex = 44;
			this.btnDown.Text = "아래로(&N)";
			this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
			// 
			// btnUp
			// 
			this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnUp.BackColor = System.Drawing.Color.DarkGray;
			this.btnUp.EnableFadeInOut = true;
			this.btnUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnUp.ForeColor = System.Drawing.Color.Black;
			this.btnUp.GlowColor = System.Drawing.Color.White;
			this.btnUp.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnUp.Location = new System.Drawing.Point(266, 3);
			this.btnUp.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnUp.Name = "btnUp";
			this.btnUp.Size = new System.Drawing.Size(86, 24);
			this.btnUp.TabIndex = 43;
			this.btnUp.Text = "위로(&U)";
			this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
			// 
			// txtMapCode
			// 
			this.txtMapCode.Location = new System.Drawing.Point(68, 58);
			this.txtMapCode.MaxLength = 4;
			this.txtMapCode.Name = "txtMapCode";
			this.txtMapCode.Size = new System.Drawing.Size(74, 21);
			this.txtMapCode.TabIndex = 66;
			// 
			// txtFloorName
			// 
			this.txtFloorName.Location = new System.Drawing.Point(68, 34);
			this.txtFloorName.Name = "txtFloorName";
			this.txtFloorName.Size = new System.Drawing.Size(168, 21);
			this.txtFloorName.TabIndex = 65;
			// 
			// txtFloorCode
			// 
			this.txtFloorCode.Location = new System.Drawing.Point(68, 10);
			this.txtFloorCode.MaxLength = 4;
			this.txtFloorCode.Name = "txtFloorCode";
			this.txtFloorCode.Size = new System.Drawing.Size(76, 21);
			this.txtFloorCode.TabIndex = 64;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label7.Location = new System.Drawing.Point(12, 16);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 69;
			this.label7.Text = "층 코드:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label6.Location = new System.Drawing.Point(12, 39);
			this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 12);
			this.label6.TabIndex = 70;
			this.label6.Text = "층 명칭:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.label5.Location = new System.Drawing.Point(12, 62);
			this.label5.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 12);
			this.label5.TabIndex = 71;
			this.label5.Text = "맵 코드:";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
			this.btnCancel.EnableFadeInOut = true;
			this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnCancel.ForeColor = System.Drawing.Color.Black;
			this.btnCancel.GlowColor = System.Drawing.Color.White;
			this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnCancel.Location = new System.Drawing.Point(178, 3);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(86, 24);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "취소";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnSave.BackColor = System.Drawing.Color.DarkGray;
			this.btnSave.EnableFadeInOut = true;
			this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnSave.ForeColor = System.Drawing.Color.Black;
			this.btnSave.GlowColor = System.Drawing.Color.White;
			this.btnSave.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnSave.Location = new System.Drawing.Point(90, 3);
			this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(86, 24);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "저장";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlLayoutButtom
			// 
			this.pnlLayoutButtom.BackColor = System.Drawing.SystemColors.Control;
			this.pnlLayoutButtom.Border1ColorBottom = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border1ColorLeft = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border1ColorRight = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border1ColorTop = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlLayoutButtom.Border1WidthBottom = 0;
			this.pnlLayoutButtom.Border1WidthLeft = 0;
			this.pnlLayoutButtom.Border1WidthRight = 0;
			this.pnlLayoutButtom.Border1WidthTop = 1;
			this.pnlLayoutButtom.Border2ColorBottom = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border2ColorLeft = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border2ColorRight = System.Drawing.Color.Gray;
			this.pnlLayoutButtom.Border2ColorTop = System.Drawing.Color.White;
			this.pnlLayoutButtom.Border2WidthBottom = 0;
			this.pnlLayoutButtom.Border2WidthLeft = 0;
			this.pnlLayoutButtom.Border2WidthRight = 0;
			this.pnlLayoutButtom.Border2WidthTop = 0;
			this.pnlLayoutButtom.Controls.Add(this.pnlControl);
			this.pnlLayoutButtom.Controls.Add(this.label1);
			this.pnlLayoutButtom.Controls.Add(this.lblRowCount);
			this.pnlLayoutButtom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlLayoutButtom.Location = new System.Drawing.Point(0, 583);
			this.pnlLayoutButtom.Name = "pnlLayoutButtom";
			this.pnlLayoutButtom.Size = new System.Drawing.Size(992, 37);
			this.pnlLayoutButtom.TabIndex = 47;
			// 
			// pnlControl
			// 
			this.pnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlControl.Controls.Add(this.btnDelete);
			this.pnlControl.Controls.Add(this.btnUpdate);
			this.pnlControl.Controls.Add(this.btnInsert);
			this.pnlControl.Controls.Add(this.btnDown);
			this.pnlControl.Controls.Add(this.btnUp);
			this.pnlControl.Controls.Add(this.btnCancel);
			this.pnlControl.Controls.Add(this.btnSave);
			this.pnlControl.Controls.Add(this.chkContinueInsertMode);
			this.pnlControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.pnlControl.Location = new System.Drawing.Point(282, 4);
			this.pnlControl.Name = "pnlControl";
			this.pnlControl.Size = new System.Drawing.Size(706, 30);
			this.pnlControl.TabIndex = 0;
			// 
			// chkContinueInsertMode
			// 
			this.chkContinueInsertMode.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.chkContinueInsertMode.AutoSize = true;
			this.chkContinueInsertMode.Location = new System.Drawing.Point(16, 7);
			this.chkContinueInsertMode.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
			this.chkContinueInsertMode.Name = "chkContinueInsertMode";
			this.chkContinueInsertMode.Size = new System.Drawing.Size(72, 16);
			this.chkContinueInsertMode.TabIndex = 45;
			this.chkContinueInsertMode.Text = "계속추가";
			this.chkContinueInsertMode.UseVisualStyleBackColor = true;
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
			this.pnlContent.Size = new System.Drawing.Size(992, 424);
			this.pnlContent.TabIndex = 23;
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
			this.pnlLayoutTop.Size = new System.Drawing.Size(992, 46);
			this.pnlLayoutTop.TabIndex = 23;
			// 
			// lblSubject
			// 
			this.lblSubject.BackColor = System.Drawing.Color.Transparent;
			this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblSubject.Location = new System.Drawing.Point(10, 10);
			this.lblSubject.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lblSubject.Name = "lblSubject";
			this.lblSubject.Size = new System.Drawing.Size(220, 24);
			this.lblSubject.TabIndex = 69;
			this.lblSubject.Text = "주차장별 층 정보";
			this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
			this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			this.lblSubject.TextColor = System.Drawing.Color.Black;
			this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
			this.lblSubject.TextTopMargin = 2;
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
			this.pnlInput.Controls.Add(this.txtFloorCode);
			this.pnlInput.Controls.Add(this.txtFloorName);
			this.pnlInput.Controls.Add(this.label6);
			this.pnlInput.Controls.Add(this.txtMapCode);
			this.pnlInput.Controls.Add(this.label5);
			this.pnlInput.Controls.Add(this.label7);
			this.pnlInput.Location = new System.Drawing.Point(0, 470);
			this.pnlInput.Name = "pnlInput";
			this.pnlInput.Size = new System.Drawing.Size(992, 112);
			this.pnlInput.TabIndex = 72;
			// 
			// PARKINGLOT_NO
			// 
			this.PARKINGLOT_NO.HeaderText = "주차장 번호";
			this.PARKINGLOT_NO.Name = "PARKINGLOT_NO";
			this.PARKINGLOT_NO.ReadOnly = true;
			this.PARKINGLOT_NO.Visible = false;
			// 
			// FLOOR_CODE
			// 
			this.FLOOR_CODE.FillWeight = 200F;
			this.FLOOR_CODE.HeaderText = "층 코드";
			this.FLOOR_CODE.MaxInputLength = 128;
			this.FLOOR_CODE.Name = "FLOOR_CODE";
			this.FLOOR_CODE.ReadOnly = true;
			this.FLOOR_CODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.FLOOR_CODE.Width = 200;
			// 
			// FLOOR_NAME
			// 
			this.FLOOR_NAME.FillWeight = 200F;
			this.FLOOR_NAME.HeaderText = "층 명칭";
			this.FLOOR_NAME.MaxInputLength = 128;
			this.FLOOR_NAME.Name = "FLOOR_NAME";
			this.FLOOR_NAME.ReadOnly = true;
			this.FLOOR_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.FLOOR_NAME.Width = 200;
			// 
			// MAP_CODE
			// 
			this.MAP_CODE.FillWeight = 200F;
			this.MAP_CODE.HeaderText = "멥 코드";
			this.MAP_CODE.Name = "MAP_CODE";
			this.MAP_CODE.ReadOnly = true;
			this.MAP_CODE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.MAP_CODE.Width = 200;
			// 
			// COMMENT
			// 
			this.COMMENT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.COMMENT.FillWeight = 400F;
			this.COMMENT.HeaderText = "";
			this.COMMENT.Name = "COMMENT";
			this.COMMENT.ReadOnly = true;
			this.COMMENT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// UPDATE_DATE
			// 
			this.UPDATE_DATE.HeaderText = "UPDATE_DATE";
			this.UPDATE_DATE.Name = "UPDATE_DATE";
			this.UPDATE_DATE.ReadOnly = true;
			this.UPDATE_DATE.Visible = false;
			// 
			// ParkingLotFloor
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(992, 620);
			this.Controls.Add(this.pnlInput);
			this.Controls.Add(this.pnlLayoutTop);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlLayoutButtom);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ParkingLotFloor";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "현장 정보";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ParkingLotFloor_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ParkingLotFloor_FormClosed);
			this.Load += new System.EventHandler(this.ParkingLotFloor_Load);
			this.Shown += new System.EventHandler(this.ParkingLotFloor_Shown);
			((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
			this.pnlCondition.ResumeLayout(false);
			this.pnlCondition.PerformLayout();
			this.pnlLayoutButtom.ResumeLayout(false);
			this.pnlLayoutButtom.PerformLayout();
			this.pnlControl.ResumeLayout(false);
			this.pnlControl.PerformLayout();
			this.pnlContent.ResumeLayout(false);
			this.pnlLayoutTop.ResumeLayout(false);
			this.pnlInput.ResumeLayout(false);
			this.pnlInput.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private FadeFox.UI.GlassButton btnClose;
		private FadeFox.UI.DataGridViewEx grdList;
		private FadeFox.UI.GlassButton btnSearch;
		private FadeFox.UI.GlassButton btnDelete;
		private FadeFox.UI.GlassButton btnUpdate;
		private FadeFox.UI.GlassButton btnInsert;
		private System.Windows.Forms.Label lblRowCount;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cboParkingLot;
		private FadeFox.UI.GlassButton btnDown;
		private FadeFox.UI.GlassButton btnUp;
		private FadeFox.UI.SimplePanel pnlLayoutButtom;
		private System.Windows.Forms.FlowLayoutPanel pnlControl;
		private FadeFox.UI.GlassButton btnCancel;
		private FadeFox.UI.GlassButton btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtMapCode;
		private System.Windows.Forms.TextBox txtFloorName;
		private System.Windows.Forms.TextBox txtFloorCode;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private System.Windows.Forms.CheckBox chkContinueInsertMode;
		private FadeFox.UI.BorderPanelEx pnlContent;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private FadeFox.UI.SimplePanel pnlInput;
		private FadeFox.UI.Text3DLabel lblSubject;
		private System.Windows.Forms.DataGridViewTextBoxColumn PARKINGLOT_NO;
		private System.Windows.Forms.DataGridViewTextBoxColumn FLOOR_CODE;
		private System.Windows.Forms.DataGridViewTextBoxColumn FLOOR_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn MAP_CODE;
		private System.Windows.Forms.DataGridViewTextBoxColumn COMMENT;
		private System.Windows.Forms.DataGridViewTextBoxColumn UPDATE_DATE;
	}
}