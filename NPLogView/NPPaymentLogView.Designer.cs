namespace NPLogView
{
	partial class NPPaymentLogView
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.label1 = new System.Windows.Forms.Label();
			this.oftConfig = new FadeFox.UI.OpenFileTextBox();
			this.btnSearch = new FadeFox.UI.GlassButton();
			this.pnlLayoutTop = new FadeFox.UI.GradientPanelEx();
			this.pnlContent = new FadeFox.UI.BorderPanelEx();
			this.grdList = new FadeFox.UI.DataGridViewEx();
			this.dtpFrom = new System.Windows.Forms.DateTimePicker();
			this.dtpTo = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.로그일 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.차량번호 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.입차일 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.정산일 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.정산방법 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.총주차시간_분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.기본주차시간_분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.기본주차요금_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.단위주차시간_분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.단위주차요금_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.주차요금_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.투입금액_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.거스름돈_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.할인_분 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.할인_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.지불처리결과 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.추가정산여부 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.이전결제금액_원 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.비고 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pnlLayoutTop.SuspendLayout();
			this.pnlContent.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 12);
			this.label1.TabIndex = 14;
			this.label1.Text = "파일:";
			// 
			// oftConfig
			// 
			this.oftConfig.AllowFileCheck = false;
			this.oftConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.oftConfig.BackColor = System.Drawing.Color.Transparent;
			this.oftConfig.ClearButtonVisible = true;
			this.oftConfig.Filter = "모든 파일 (*.*)|*.*";
			this.oftConfig.Location = new System.Drawing.Point(48, 10);
			this.oftConfig.Name = "oftConfig";
			this.oftConfig.Size = new System.Drawing.Size(424, 22);
			this.oftConfig.TabIndex = 13;
			this.oftConfig.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.BackColor = System.Drawing.Color.DarkGray;
			this.btnSearch.EnableFadeInOut = true;
			this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnSearch.ForeColor = System.Drawing.Color.Black;
			this.btnSearch.GlowColor = System.Drawing.Color.White;
			this.btnSearch.InnerBorderColor = System.Drawing.Color.DimGray;
			this.btnSearch.Location = new System.Drawing.Point(702, 8);
			this.btnSearch.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(86, 24);
			this.btnSearch.TabIndex = 31;
			this.btnSearch.Text = "검색(&S)";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// pnlLayoutTop
			// 
			this.pnlLayoutTop.BackColor = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlLayoutTop.Controls.Add(this.label2);
			this.pnlLayoutTop.Controls.Add(this.dtpTo);
			this.pnlLayoutTop.Controls.Add(this.dtpFrom);
			this.pnlLayoutTop.Controls.Add(this.btnSearch);
			this.pnlLayoutTop.Controls.Add(this.label1);
			this.pnlLayoutTop.Controls.Add(this.oftConfig);
			this.pnlLayoutTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLayoutTop.GradientDirection = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.pnlLayoutTop.GradientEnd = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.GradientStart = System.Drawing.SystemColors.Window;
			this.pnlLayoutTop.Location = new System.Drawing.Point(0, 0);
			this.pnlLayoutTop.Name = "pnlLayoutTop";
			this.pnlLayoutTop.Size = new System.Drawing.Size(796, 46);
			this.pnlLayoutTop.TabIndex = 32;
			// 
			// pnlContent
			// 
			this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlContent.BackColor = System.Drawing.Color.Transparent;
			this.pnlContent.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(172)))), ((int)(((byte)(181)))));
			this.pnlContent.Controls.Add(this.grdList);
			this.pnlContent.Location = new System.Drawing.Point(0, 46);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(796, 356);
			this.pnlContent.TabIndex = 33;
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
            this.로그일,
            this.차량번호,
            this.입차일,
            this.정산일,
            this.정산방법,
            this.총주차시간_분,
            this.기본주차시간_분,
            this.기본주차요금_원,
            this.단위주차시간_분,
            this.단위주차요금_원,
            this.주차요금_원,
            this.투입금액_원,
            this.거스름돈_원,
            this.할인_분,
            this.할인_원,
            this.지불처리결과,
            this.추가정산여부,
            this.이전결제금액_원,
            this.비고});
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.DefaultCellStyle = dataGridViewCellStyle9;
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
			dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.grdList.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
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
			this.grdList.Size = new System.Drawing.Size(786, 346);
			this.grdList.StoredRowForeColor = System.Drawing.Color.Red;
			this.grdList.TabIndex = 22;
			this.grdList.UsingCellMouseDoubleClickCopyClipboard = true;
			this.grdList.UsingCurrentCellDirtyAutoCommit = true;
			this.grdList.UsingExport = true;
			this.grdList.UsingLostFocusSelectionColor = false;
			this.grdList.UsingSort = false;
			// 
			// dtpFrom
			// 
			this.dtpFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpFrom.CustomFormat = "yyyy-MM-dd";
			this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFrom.Location = new System.Drawing.Point(476, 10);
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.Size = new System.Drawing.Size(102, 21);
			this.dtpFrom.TabIndex = 32;
			// 
			// dtpTo
			// 
			this.dtpTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dtpTo.CustomFormat = "yyyy-MM-dd";
			this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpTo.Location = new System.Drawing.Point(600, 10);
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.Size = new System.Drawing.Size(98, 21);
			this.dtpTo.TabIndex = 33;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(582, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 12);
			this.label2.TabIndex = 34;
			this.label2.Text = "~";
			// 
			// 로그일
			// 
			this.로그일.FillWeight = 130F;
			this.로그일.HeaderText = "로그일";
			this.로그일.Name = "로그일";
			this.로그일.ReadOnly = true;
			this.로그일.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.로그일.Width = 130;
			// 
			// 차량번호
			// 
			this.차량번호.HeaderText = "차량번호";
			this.차량번호.Name = "차량번호";
			this.차량번호.ReadOnly = true;
			this.차량번호.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 입차일
			// 
			this.입차일.FillWeight = 130F;
			this.입차일.HeaderText = "입차일";
			this.입차일.Name = "입차일";
			this.입차일.ReadOnly = true;
			this.입차일.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.입차일.Width = 130;
			// 
			// 정산일
			// 
			this.정산일.FillWeight = 130F;
			this.정산일.HeaderText = "정산일";
			this.정산일.Name = "정산일";
			this.정산일.ReadOnly = true;
			this.정산일.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.정산일.Width = 130;
			// 
			// 정산방법
			// 
			this.정산방법.HeaderText = "정산방법";
			this.정산방법.Name = "정산방법";
			this.정산방법.ReadOnly = true;
			this.정산방법.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 총주차시간_분
			// 
			this.총주차시간_분.FillWeight = 130F;
			this.총주차시간_분.HeaderText = "총주차시간(분)";
			this.총주차시간_분.Name = "총주차시간_분";
			this.총주차시간_분.ReadOnly = true;
			this.총주차시간_분.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.총주차시간_분.Width = 130;
			// 
			// 기본주차시간_분
			// 
			this.기본주차시간_분.FillWeight = 130F;
			this.기본주차시간_분.HeaderText = "기본주차시간(분)";
			this.기본주차시간_분.Name = "기본주차시간_분";
			this.기본주차시간_분.ReadOnly = true;
			this.기본주차시간_분.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.기본주차시간_분.Width = 130;
			// 
			// 기본주차요금_원
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "N0";
			this.기본주차요금_원.DefaultCellStyle = dataGridViewCellStyle2;
			this.기본주차요금_원.FillWeight = 130F;
			this.기본주차요금_원.HeaderText = "기본주차요금(원)";
			this.기본주차요금_원.Name = "기본주차요금_원";
			this.기본주차요금_원.ReadOnly = true;
			this.기본주차요금_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.기본주차요금_원.Width = 130;
			// 
			// 단위주차시간_분
			// 
			this.단위주차시간_분.FillWeight = 130F;
			this.단위주차시간_분.HeaderText = "단위주차시간(분)";
			this.단위주차시간_분.Name = "단위주차시간_분";
			this.단위주차시간_분.ReadOnly = true;
			this.단위주차시간_분.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.단위주차시간_분.Width = 130;
			// 
			// 단위주차요금_원
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle3.Format = "N0";
			this.단위주차요금_원.DefaultCellStyle = dataGridViewCellStyle3;
			this.단위주차요금_원.FillWeight = 130F;
			this.단위주차요금_원.HeaderText = "단위주차요금(원)";
			this.단위주차요금_원.Name = "단위주차요금_원";
			this.단위주차요금_원.ReadOnly = true;
			this.단위주차요금_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.단위주차요금_원.Width = 130;
			// 
			// 주차요금_원
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle4.Format = "N0";
			this.주차요금_원.DefaultCellStyle = dataGridViewCellStyle4;
			this.주차요금_원.HeaderText = "주차요금(원)";
			this.주차요금_원.Name = "주차요금_원";
			this.주차요금_원.ReadOnly = true;
			this.주차요금_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 투입금액_원
			// 
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle5.Format = "N0";
			this.투입금액_원.DefaultCellStyle = dataGridViewCellStyle5;
			this.투입금액_원.HeaderText = "투입금액(원)";
			this.투입금액_원.Name = "투입금액_원";
			this.투입금액_원.ReadOnly = true;
			this.투입금액_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 거스름돈_원
			// 
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle6.Format = "N0";
			this.거스름돈_원.DefaultCellStyle = dataGridViewCellStyle6;
			this.거스름돈_원.HeaderText = "거스름돈(원)";
			this.거스름돈_원.Name = "거스름돈_원";
			this.거스름돈_원.ReadOnly = true;
			this.거스름돈_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 할인_분
			// 
			this.할인_분.HeaderText = "할인(분)";
			this.할인_분.Name = "할인_분";
			this.할인_분.ReadOnly = true;
			this.할인_분.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 할인_원
			// 
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle7.Format = "N0";
			this.할인_원.DefaultCellStyle = dataGridViewCellStyle7;
			this.할인_원.HeaderText = "할인(원)";
			this.할인_원.Name = "할인_원";
			this.할인_원.ReadOnly = true;
			this.할인_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// 지불처리결과
			// 
			this.지불처리결과.FillWeight = 180F;
			this.지불처리결과.HeaderText = "지불처리결과";
			this.지불처리결과.Name = "지불처리결과";
			this.지불처리결과.ReadOnly = true;
			this.지불처리결과.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.지불처리결과.Width = 180;
			// 
			// 추가정산여부
			// 
			this.추가정산여부.FillWeight = 130F;
			this.추가정산여부.HeaderText = "추가정산여부";
			this.추가정산여부.Name = "추가정산여부";
			this.추가정산여부.ReadOnly = true;
			this.추가정산여부.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.추가정산여부.Width = 130;
			// 
			// 이전결제금액_원
			// 
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle8.Format = "N0";
			this.이전결제금액_원.DefaultCellStyle = dataGridViewCellStyle8;
			this.이전결제금액_원.FillWeight = 130F;
			this.이전결제금액_원.HeaderText = "이전결제금액(원)";
			this.이전결제금액_원.Name = "이전결제금액_원";
			this.이전결제금액_원.ReadOnly = true;
			this.이전결제금액_원.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.이전결제금액_원.Width = 130;
			// 
			// 비고
			// 
			this.비고.FillWeight = 300F;
			this.비고.HeaderText = "비고";
			this.비고.Name = "비고";
			this.비고.ReadOnly = true;
			this.비고.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.비고.Width = 300;
			// 
			// NPPaymentLogView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(796, 402);
			this.Controls.Add(this.pnlContent);
			this.Controls.Add(this.pnlLayoutTop);
			this.Name = "NPPaymentLogView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "LogView";
			this.pnlLayoutTop.ResumeLayout(false);
			this.pnlLayoutTop.PerformLayout();
			this.pnlContent.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private FadeFox.UI.OpenFileTextBox oftConfig;
		private FadeFox.UI.GlassButton btnSearch;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private FadeFox.UI.BorderPanelEx pnlContent;
		private FadeFox.UI.DataGridViewEx grdList;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DateTimePicker dtpTo;
		private System.Windows.Forms.DateTimePicker dtpFrom;
		private System.Windows.Forms.DataGridViewTextBoxColumn 로그일;
		private System.Windows.Forms.DataGridViewTextBoxColumn 차량번호;
		private System.Windows.Forms.DataGridViewTextBoxColumn 입차일;
		private System.Windows.Forms.DataGridViewTextBoxColumn 정산일;
		private System.Windows.Forms.DataGridViewTextBoxColumn 정산방법;
		private System.Windows.Forms.DataGridViewTextBoxColumn 총주차시간_분;
		private System.Windows.Forms.DataGridViewTextBoxColumn 기본주차시간_분;
		private System.Windows.Forms.DataGridViewTextBoxColumn 기본주차요금_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 단위주차시간_분;
		private System.Windows.Forms.DataGridViewTextBoxColumn 단위주차요금_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 주차요금_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 투입금액_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 거스름돈_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 할인_분;
		private System.Windows.Forms.DataGridViewTextBoxColumn 할인_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 지불처리결과;
		private System.Windows.Forms.DataGridViewTextBoxColumn 추가정산여부;
		private System.Windows.Forms.DataGridViewTextBoxColumn 이전결제금액_원;
		private System.Windows.Forms.DataGridViewTextBoxColumn 비고;

	}
}

