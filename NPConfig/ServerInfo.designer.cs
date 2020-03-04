namespace NPConfig
{
	partial class ServerInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ofFindFile = new System.Windows.Forms.OpenFileDialog();
            this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
            this.pnlCondition = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSearch = new FadeFox.UI.GlassButton();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboSearchServerLID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSubject = new FadeFox.UI.Text3DLabel();
            this.pnlContent = new FadeFox.UI.BorderPanelEx();
            this.grdList = new FadeFox.UI.DataGridViewEx();
            this.SERVER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_LID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_LID_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_ADDRESS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATABASE_LID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATABASE_LID_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_DATABASE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_USER_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_USER_PASSWORD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERVER_COMMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WINDOWS_AUTH_YN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPDATE_DATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLayoutBottom = new FadeFox.UI.SimplePanel();
            this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new FadeFox.UI.GlassButton();
            this.btnDelete = new FadeFox.UI.GlassButton();
            this.btnUpdate = new FadeFox.UI.GlassButton();
            this.btnInsert = new FadeFox.UI.GlassButton();
            this.btnCancel = new FadeFox.UI.GlassButton();
            this.btnSave = new FadeFox.UI.GlassButton();
            this.chkContinueInsertMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblRowCount = new System.Windows.Forms.Label();
            this.pnlInput = new FadeFox.UI.SimplePanel();
            this.label12 = new System.Windows.Forms.Label();
            this.cboServerLID = new System.Windows.Forms.ComboBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.txtServerComment = new System.Windows.Forms.TextBox();
            this.txtServerUserPassword = new System.Windows.Forms.TextBox();
            this.txtServerUserID = new System.Windows.Forms.TextBox();
            this.txtServerDatabase = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.txtServerID = new System.Windows.Forms.TextBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.btnFindFile = new FadeFox.UI.FlatButton();
            this.label9 = new System.Windows.Forms.Label();
            this.cboDatabaseLID = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pnlLayoutTop.SuspendLayout();
            this.pnlCondition.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.pnlLayoutBottom.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofFindFile
            // 
            this.ofFindFile.RestoreDirectory = true;
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
            this.pnlLayoutTop.Size = new System.Drawing.Size(898, 46);
            this.pnlLayoutTop.TabIndex = 72;
            // 
            // pnlCondition
            // 
            this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
            this.pnlCondition.Controls.Add(this.btnSearch);
            this.pnlCondition.Controls.Add(this.txtSearchName);
            this.pnlCondition.Controls.Add(this.label3);
            this.pnlCondition.Controls.Add(this.cboSearchServerLID);
            this.pnlCondition.Controls.Add(this.label1);
            this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlCondition.Location = new System.Drawing.Point(418, 8);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(476, 26);
            this.pnlCondition.TabIndex = 66;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSearch.BackColor = System.Drawing.Color.DarkGray;
            this.btnSearch.EnableFadeInOut = true;
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.GlowColor = System.Drawing.Color.White;
            this.btnSearch.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnSearch.Location = new System.Drawing.Point(388, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(86, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "검색(&S)";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchName
            // 
            this.txtSearchName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtSearchName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtSearchName.Location = new System.Drawing.Point(272, 2);
            this.txtSearchName.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(114, 21);
            this.txtSearchName.TabIndex = 1;
            this.txtSearchName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearchName_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 37;
            this.label3.Text = "서버 명:";
            // 
            // cboSearchServerLID
            // 
            this.cboSearchServerLID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cboSearchServerLID.FormattingEnabled = true;
            this.cboSearchServerLID.Location = new System.Drawing.Point(85, 3);
            this.cboSearchServerLID.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.cboSearchServerLID.Name = "cboSearchServerLID";
            this.cboSearchServerLID.Size = new System.Drawing.Size(134, 20);
            this.cboSearchServerLID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 2, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 38;
            this.label1.Text = "서버 종류:";
            // 
            // lblSubject
            // 
            this.lblSubject.BackColor = System.Drawing.Color.Transparent;
            this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSubject.Location = new System.Drawing.Point(10, 10);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(4);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(162, 24);
            this.lblSubject.TabIndex = 70;
            this.lblSubject.Text = "서버 정보";
            this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
            this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.lblSubject.TextColor = System.Drawing.Color.Black;
            this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
            this.lblSubject.TextTopMargin = 2;
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
            this.pnlContent.Size = new System.Drawing.Size(898, 219);
            this.pnlContent.TabIndex = 71;
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
            this.grdList.ClipboardCopyWithHeaderText = true;
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
            this.SERVER_ID,
            this.SERVER_NAME,
            this.SERVER_LID,
            this.SERVER_LID_NAME,
            this.SERVER_ADDRESS,
            this.SERVER_PORT,
            this.DATABASE_LID,
            this.DATABASE_LID_NAME,
            this.SERVER_DATABASE,
            this.SERVER_USER_ID,
            this.SERVER_USER_PASSWORD,
            this.SERVER_COMMENT,
            this.WINDOWS_AUTH_YN,
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
            this.grdList.Size = new System.Drawing.Size(888, 209);
            this.grdList.StoredRowForeColor = System.Drawing.Color.Red;
            this.grdList.TabIndex = 0;
            this.grdList.UsingCellMouseDoubleClickCopyClipboard = true;
            this.grdList.UsingCurrentCellDirtyAutoCommit = true;
            this.grdList.UsingExport = true;
            this.grdList.UsingLostFocusSelectionColor = false;
            this.grdList.UsingSort = false;
            // 
            // SERVER_ID
            // 
            this.SERVER_ID.FillWeight = 130F;
            this.SERVER_ID.HeaderText = "서버 아이디";
            this.SERVER_ID.MaxInputLength = 128;
            this.SERVER_ID.Name = "SERVER_ID";
            this.SERVER_ID.ReadOnly = true;
            this.SERVER_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_ID.Width = 130;
            // 
            // SERVER_NAME
            // 
            dataGridViewCellStyle2.NullValue = null;
            this.SERVER_NAME.DefaultCellStyle = dataGridViewCellStyle2;
            this.SERVER_NAME.FillWeight = 250F;
            this.SERVER_NAME.HeaderText = "서버 명";
            this.SERVER_NAME.MaxInputLength = 128;
            this.SERVER_NAME.Name = "SERVER_NAME";
            this.SERVER_NAME.ReadOnly = true;
            this.SERVER_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_NAME.Width = 250;
            // 
            // SERVER_LID
            // 
            this.SERVER_LID.HeaderText = "서버 종류";
            this.SERVER_LID.Name = "SERVER_LID";
            this.SERVER_LID.ReadOnly = true;
            this.SERVER_LID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_LID.Visible = false;
            // 
            // SERVER_LID_NAME
            // 
            this.SERVER_LID_NAME.HeaderText = "서버 종류 명";
            this.SERVER_LID_NAME.Name = "SERVER_LID_NAME";
            this.SERVER_LID_NAME.ReadOnly = true;
            this.SERVER_LID_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SERVER_ADDRESS
            // 
            this.SERVER_ADDRESS.FillWeight = 200F;
            this.SERVER_ADDRESS.HeaderText = "서버 주소";
            this.SERVER_ADDRESS.Name = "SERVER_ADDRESS";
            this.SERVER_ADDRESS.ReadOnly = true;
            this.SERVER_ADDRESS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_ADDRESS.Width = 200;
            // 
            // SERVER_PORT
            // 
            this.SERVER_PORT.HeaderText = "서버 포트";
            this.SERVER_PORT.Name = "SERVER_PORT";
            this.SERVER_PORT.ReadOnly = true;
            this.SERVER_PORT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DATABASE_LID
            // 
            this.DATABASE_LID.FillWeight = 150F;
            this.DATABASE_LID.HeaderText = "데이터베이스 종류";
            this.DATABASE_LID.Name = "DATABASE_LID";
            this.DATABASE_LID.ReadOnly = true;
            this.DATABASE_LID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DATABASE_LID.Visible = false;
            this.DATABASE_LID.Width = 150;
            // 
            // DATABASE_LID_NAME
            // 
            this.DATABASE_LID_NAME.FillWeight = 150F;
            this.DATABASE_LID_NAME.HeaderText = "데이터베이스 종류 명";
            this.DATABASE_LID_NAME.Name = "DATABASE_LID_NAME";
            this.DATABASE_LID_NAME.ReadOnly = true;
            this.DATABASE_LID_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DATABASE_LID_NAME.Width = 150;
            // 
            // SERVER_DATABASE
            // 
            this.SERVER_DATABASE.HeaderText = "데이터베이스 명";
            this.SERVER_DATABASE.Name = "SERVER_DATABASE";
            this.SERVER_DATABASE.ReadOnly = true;
            this.SERVER_DATABASE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_DATABASE.Width = 250;
            // 
            // SERVER_USER_ID
            // 
            this.SERVER_USER_ID.HeaderText = "사용자 아이디";
            this.SERVER_USER_ID.Name = "SERVER_USER_ID";
            this.SERVER_USER_ID.ReadOnly = true;
            this.SERVER_USER_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_USER_ID.Visible = false;
            // 
            // SERVER_USER_PASSWORD
            // 
            this.SERVER_USER_PASSWORD.HeaderText = "사용자 암호";
            this.SERVER_USER_PASSWORD.Name = "SERVER_USER_PASSWORD";
            this.SERVER_USER_PASSWORD.ReadOnly = true;
            this.SERVER_USER_PASSWORD.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_USER_PASSWORD.Visible = false;
            // 
            // SERVER_COMMENT
            // 
            this.SERVER_COMMENT.FillWeight = 330F;
            this.SERVER_COMMENT.HeaderText = "설명";
            this.SERVER_COMMENT.Name = "SERVER_COMMENT";
            this.SERVER_COMMENT.ReadOnly = true;
            this.SERVER_COMMENT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERVER_COMMENT.Width = 330;
            // 
            // WINDOWS_AUTH_YN
            // 
            this.WINDOWS_AUTH_YN.HeaderText = "Windows 인증";
            this.WINDOWS_AUTH_YN.Name = "WINDOWS_AUTH_YN";
            this.WINDOWS_AUTH_YN.ReadOnly = true;
            this.WINDOWS_AUTH_YN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UPDATE_DATE
            // 
            this.UPDATE_DATE.HeaderText = "수정일";
            this.UPDATE_DATE.Name = "UPDATE_DATE";
            this.UPDATE_DATE.ReadOnly = true;
            this.UPDATE_DATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UPDATE_DATE.Visible = false;
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
            this.pnlLayoutBottom.Controls.Add(this.label5);
            this.pnlLayoutBottom.Controls.Add(this.lblRowCount);
            this.pnlLayoutBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLayoutBottom.Location = new System.Drawing.Point(0, 444);
            this.pnlLayoutBottom.Name = "pnlLayoutBottom";
            this.pnlLayoutBottom.Size = new System.Drawing.Size(898, 37);
            this.pnlLayoutBottom.TabIndex = 2;
            // 
            // pnlControl
            // 
            this.pnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlControl.Controls.Add(this.btnClose);
            this.pnlControl.Controls.Add(this.btnDelete);
            this.pnlControl.Controls.Add(this.btnUpdate);
            this.pnlControl.Controls.Add(this.btnInsert);
            this.pnlControl.Controls.Add(this.btnCancel);
            this.pnlControl.Controls.Add(this.btnSave);
            this.pnlControl.Controls.Add(this.chkContinueInsertMode);
            this.pnlControl.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlControl.Location = new System.Drawing.Point(280, 4);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(614, 30);
            this.pnlControl.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.DarkGray;
            this.btnClose.EnableFadeInOut = true;
            this.btnClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.GlowColor = System.Drawing.Color.White;
            this.btnClose.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnClose.Location = new System.Drawing.Point(526, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.DarkGray;
            this.btnDelete.EnableFadeInOut = true;
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.GlowColor = System.Drawing.Color.White;
            this.btnDelete.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnDelete.Location = new System.Drawing.Point(438, 3);
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
            this.btnUpdate.Location = new System.Drawing.Point(350, 3);
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
            this.btnInsert.Location = new System.Drawing.Point(262, 3);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(86, 24);
            this.btnInsert.TabIndex = 0;
            this.btnInsert.Text = "추가";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.EnableFadeInOut = true;
            this.btnCancel.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.GlowColor = System.Drawing.Color.White;
            this.btnCancel.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnCancel.Location = new System.Drawing.Point(174, 3);
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
            this.btnSave.Location = new System.Drawing.Point(86, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 24);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "저장";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkContinueInsertMode
            // 
            this.chkContinueInsertMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkContinueInsertMode.AutoSize = true;
            this.chkContinueInsertMode.Location = new System.Drawing.Point(12, 7);
            this.chkContinueInsertMode.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.chkContinueInsertMode.Name = "chkContinueInsertMode";
            this.chkContinueInsertMode.Size = new System.Drawing.Size(72, 16);
            this.chkContinueInsertMode.TabIndex = 6;
            this.chkContinueInsertMode.Text = "계속추가";
            this.chkContinueInsertMode.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "건수:";
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
            this.pnlInput.Controls.Add(this.label12);
            this.pnlInput.Controls.Add(this.cboServerLID);
            this.pnlInput.Controls.Add(this.chkWindowsAuth);
            this.pnlInput.Controls.Add(this.txtServerComment);
            this.pnlInput.Controls.Add(this.txtServerUserPassword);
            this.pnlInput.Controls.Add(this.txtServerUserID);
            this.pnlInput.Controls.Add(this.txtServerDatabase);
            this.pnlInput.Controls.Add(this.txtServerName);
            this.pnlInput.Controls.Add(this.txtServerID);
            this.pnlInput.Controls.Add(this.txtServerPort);
            this.pnlInput.Controls.Add(this.txtServerAddress);
            this.pnlInput.Controls.Add(this.btnFindFile);
            this.pnlInput.Controls.Add(this.label9);
            this.pnlInput.Controls.Add(this.cboDatabaseLID);
            this.pnlInput.Controls.Add(this.label2);
            this.pnlInput.Controls.Add(this.label4);
            this.pnlInput.Controls.Add(this.label6);
            this.pnlInput.Controls.Add(this.label7);
            this.pnlInput.Controls.Add(this.label8);
            this.pnlInput.Controls.Add(this.label10);
            this.pnlInput.Controls.Add(this.label11);
            this.pnlInput.Location = new System.Drawing.Point(0, 265);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(898, 178);
            this.pnlInput.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 12);
            this.label12.TabIndex = 63;
            this.label12.Text = "서버 종류:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboServerLID
            // 
            this.cboServerLID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServerLID.FormattingEnabled = true;
            this.cboServerLID.Location = new System.Drawing.Point(122, 32);
            this.cboServerLID.Name = "cboServerLID";
            this.cboServerLID.Size = new System.Drawing.Size(254, 20);
            this.cboServerLID.TabIndex = 2;
            this.cboServerLID.SelectedIndexChanged += new System.EventHandler(this.cboServerLID_SelectedIndexChanged);
            // 
            // chkWindowsAuth
            // 
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Location = new System.Drawing.Point(124, 106);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(103, 16);
            this.chkWindowsAuth.TabIndex = 7;
            this.chkWindowsAuth.TabStop = false;
            this.chkWindowsAuth.Text = "Windows 인증";
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            this.chkWindowsAuth.CheckedChanged += new System.EventHandler(this.chkWindowsAuth_CheckedChanged);
            // 
            // txtServerComment
            // 
            this.txtServerComment.Location = new System.Drawing.Point(122, 150);
            this.txtServerComment.Name = "txtServerComment";
            this.txtServerComment.Size = new System.Drawing.Size(646, 21);
            this.txtServerComment.TabIndex = 10;
            // 
            // txtServerUserPassword
            // 
            this.txtServerUserPassword.Location = new System.Drawing.Point(500, 126);
            this.txtServerUserPassword.Name = "txtServerUserPassword";
            this.txtServerUserPassword.PasswordChar = '*';
            this.txtServerUserPassword.Size = new System.Drawing.Size(268, 21);
            this.txtServerUserPassword.TabIndex = 9;
            // 
            // txtServerUserID
            // 
            this.txtServerUserID.Location = new System.Drawing.Point(122, 126);
            this.txtServerUserID.Name = "txtServerUserID";
            this.txtServerUserID.PasswordChar = '*';
            this.txtServerUserID.Size = new System.Drawing.Size(256, 21);
            this.txtServerUserID.TabIndex = 8;
            // 
            // txtServerDatabase
            // 
            this.txtServerDatabase.Location = new System.Drawing.Point(122, 80);
            this.txtServerDatabase.Name = "txtServerDatabase";
            this.txtServerDatabase.Size = new System.Drawing.Size(622, 21);
            this.txtServerDatabase.TabIndex = 6;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(500, 8);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(268, 21);
            this.txtServerName.TabIndex = 1;
            // 
            // txtServerID
            // 
            this.txtServerID.Location = new System.Drawing.Point(122, 8);
            this.txtServerID.Name = "txtServerID";
            this.txtServerID.Size = new System.Drawing.Size(254, 21);
            this.txtServerID.TabIndex = 0;
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(308, 56);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(68, 21);
            this.txtServerPort.TabIndex = 5;
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(122, 56);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(184, 21);
            this.txtServerAddress.TabIndex = 4;
            // 
            // btnFindFile
            // 
            this.btnFindFile.ButtonFaceColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFindFile.DisplayFocus = true;
            this.btnFindFile.FlatButtonBorderColor = System.Drawing.Color.DarkGray;
            this.btnFindFile.FlatButtonBorderHotColor = System.Drawing.Color.Black;
            this.btnFindFile.FlatButtonHotColor = System.Drawing.Color.FromArgb(((int)(((byte)(182)))), ((int)(((byte)(193)))), ((int)(((byte)(214)))));
            this.btnFindFile.FlatButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(218)))), ((int)(((byte)(232)))));
            this.btnFindFile.FlatButtonStyle = FadeFox.UI.FlatButtonStyle.Flat;
            this.btnFindFile.Location = new System.Drawing.Point(746, 80);
            this.btnFindFile.Name = "btnFindFile";
            this.btnFindFile.PopupButtonHighlightColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnFindFile.PopupButtonPressedColor = System.Drawing.Color.White;
            this.btnFindFile.PopupButtonShadowColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFindFile.Size = new System.Drawing.Size(22, 20);
            this.btnFindFile.TabIndex = 60;
            this.btnFindFile.Text = "...";
            this.btnFindFile.TextAlign = System.Drawing.StringAlignment.Center;
            this.btnFindFile.TextColor = System.Drawing.Color.Black;
            this.btnFindFile.TextHotColor = System.Drawing.Color.Black;
            this.btnFindFile.TextShadow = false;
            this.btnFindFile.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.btnFindFile.UseVisualStyleBackColor = true;
            this.btnFindFile.Click += new System.EventHandler(this.btnFindFile_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(384, 36);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 12);
            this.label9.TabIndex = 59;
            this.label9.Text = "데이터베이스 종류:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cboDatabaseLID
            // 
            this.cboDatabaseLID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabaseLID.FormattingEnabled = true;
            this.cboDatabaseLID.Location = new System.Drawing.Point(500, 32);
            this.cboDatabaseLID.Name = "cboDatabaseLID";
            this.cboDatabaseLID.Size = new System.Drawing.Size(268, 20);
            this.cboDatabaseLID.TabIndex = 3;
            this.cboDatabaseLID.SelectedIndexChanged += new System.EventHandler(this.cboDatabaseKind_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(10, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 54;
            this.label2.Text = "데이터베이스 명:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(388, 130);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 52;
            this.label4.Text = "암호:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(10, 130);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 49;
            this.label6.Text = "사용자 아이디:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(10, 60);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 47;
            this.label7.Text = "서버 주소:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(10, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 45;
            this.label8.Text = "서버 아이디:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(384, 12);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 46;
            this.label10.Text = "서버 명:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(12, 152);
            this.label11.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 44;
            this.label11.Text = "설명:";
            // 
            // ServerInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(898, 481);
            this.Controls.Add(this.pnlLayoutTop);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlLayoutBottom);
            this.Controls.Add(this.pnlInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ServerInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "SVR";
            this.Text = "서버 정보";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerInfo_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerInfo_FormClosed);
            this.Load += new System.EventHandler(this.ServerInfo_Load);
            this.Shown += new System.EventHandler(this.ServerInfo_Shown);
            this.pnlLayoutTop.ResumeLayout(false);
            this.pnlCondition.ResumeLayout(false);
            this.pnlCondition.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.pnlLayoutBottom.ResumeLayout(false);
            this.pnlLayoutBottom.PerformLayout();
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private FadeFox.UI.GlassButton btnClose;
		private FadeFox.UI.GlassButton btnSearch;
		private System.Windows.Forms.TextBox txtSearchName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cboSearchServerLID;
		private System.Windows.Forms.Label label1;
		private FadeFox.UI.DataGridViewEx grdList;
		private FadeFox.UI.SimplePanel pnlInput;
		private System.Windows.Forms.CheckBox chkWindowsAuth;
		private System.Windows.Forms.TextBox txtServerComment;
		private System.Windows.Forms.TextBox txtServerUserPassword;
		private System.Windows.Forms.TextBox txtServerUserID;
		private System.Windows.Forms.TextBox txtServerDatabase;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.TextBox txtServerID;
		private System.Windows.Forms.TextBox txtServerPort;
		private System.Windows.Forms.TextBox txtServerAddress;
		private FadeFox.UI.FlatButton btnFindFile;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cboDatabaseLID;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private FadeFox.UI.SimplePanel pnlLayoutBottom;
		private System.Windows.Forms.FlowLayoutPanel pnlControl;
		private FadeFox.UI.GlassButton btnCancel;
		private FadeFox.UI.GlassButton btnSave;
		private FadeFox.UI.GlassButton btnDelete;
		private FadeFox.UI.GlassButton btnUpdate;
		private FadeFox.UI.GlassButton btnInsert;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblRowCount;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cboServerLID;
		private System.Windows.Forms.OpenFileDialog ofFindFile;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_LID;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_LID_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_ADDRESS;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_PORT;
		private System.Windows.Forms.DataGridViewTextBoxColumn DATABASE_LID;
		private System.Windows.Forms.DataGridViewTextBoxColumn DATABASE_LID_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_DATABASE;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_USER_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_USER_PASSWORD;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERVER_COMMENT;
		private System.Windows.Forms.DataGridViewTextBoxColumn WINDOWS_AUTH_YN;
		private System.Windows.Forms.DataGridViewTextBoxColumn UPDATE_DATE;
		private System.Windows.Forms.CheckBox chkContinueInsertMode;
		private FadeFox.UI.BorderPanelEx pnlContent;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private FadeFox.UI.Text3DLabel lblSubject;
	}
}