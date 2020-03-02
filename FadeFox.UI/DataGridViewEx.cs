/*
 * ==============================================================================
 *   Program ID     :
 *   Program Name   :
 * ------------------------------------------------------------------------------
 *   Description
 * ------------------------------------------------------------------------------
 *   Company Name   : sayou.net
 *   Developer      : Hyosik-Bae
 *   Create Date    : 2009-08-24
 * ------------------------------------------------------------------------------
 *   Update History
 *		2010-11-08 : Excel에서 불러오기 가능 추가.
 *		2010-12-08 : BorderVisiable 기능 추가.
 *		2010-12-08 : 포커스를 잃었을 시 활성화된 행에 대한 색상 적용 기능 추가
 *		             (주, 만약 이 기능을 사용할 때에는 DefaultCellStyle에서 시작
 *		              색상을 포커스를 잃었을 때의 색으로 지정해야 보기 좋다.)
 *      2011-01-20 : 코드 리펙토링 및 데이터 읽기 델리게이트 추가
 *      2011-02-18 : 셀 마우스 더블클릭시 해당 내용 클립보드로 복사하는 기능 추가
 *                   해더 마우스 오른쪽 버튼 클릭시 소팅 모드로 변경되는 버그 수정
 *      2011-04-08 : MoveRow에 startIndex 파라메터 추가
 *      2011-05-26 : Delegate 함수 명 변경
 *                   ProgressBar 표시 형식 변경
 * ------------------------------------------------------------------------------
 *   Reference
 * ==============================================================================
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FadeFox.Utility;
using FadeFox.Document;
using FadeFox.Document.MyXls;
using FadeFox.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using FadeFox.Database;

namespace FadeFox.UI
{
	public enum ExportTypes
	{
		None,
		Excel,
		CSV
	}

	public enum ColumnType
	{
		None,
		Button,
		CheckBox,
		ComboBox,
		DatePicker,
		Image,
		Link,
		NumericUpDown,
		TextBox
	}

	/// <summary>
	/// 화면제어 관련 함수 연결 델리게이트
	/// </summary>
	public delegate void DataGridViewExDelegate();

	public delegate void DataGridViewExBoolDelegate(bool pValue);

	public delegate void DataGridViewExDataTableDelegate(DataTable pValue);

	public delegate void DataGridViewExReturnDataTableDelegate(out DataTable pValue);

	[ToolboxBitmap(typeof(System.Windows.Forms.DataGridView))]
	public class DataGridViewEx : DataGridView, ICloseEnabled
	{
		private enum DataImportMethod
		{
			None,
			Database,
			Datatable,
			Delegate
		}

		private Color mDefaultSelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
		public Color DefaultSelectionBackColor
		{
			get { return mDefaultSelectionBackColor; }
			set { mDefaultSelectionBackColor = value; }
		}

		public Color DefaultSelectionForeColor
		{
			get { return mDefaultSelectionForeColor; }
			set { mDefaultSelectionForeColor = value; }
		}

		private Color mDefaultSelectionForeColor = System.Drawing.Color.Black;
		


		private Color mStoredRowForeColor = Color.Red;
		public Color StoredRowForeColor
		{
			get { return mStoredRowForeColor; }
			set { mStoredRowForeColor = value; }
		}

		/// <summary>
		/// 데이터를 그리드에 넣기 바로 직전 단계
		/// </summary>
		private DataGridViewExDataTableDelegate mRowsPreAdding = null;
		[Browsable(false)]
		public DataGridViewExDataTableDelegate RowsPreAdding
		{
			get { return mRowsPreAdding; }
			set { mRowsPreAdding = value; }
		}

		/// <summary>
		/// 데이터를 그리드에 넣기전 데이터를 읽기위한 단계
		/// </summary>
		private DataGridViewExReturnDataTableDelegate mRowsDataRead = null;
		[Browsable(false)]
		public DataGridViewExReturnDataTableDelegate RowsDataRead
		{
			get { return mRowsDataRead; }
			set { mRowsDataRead = value; }
		}

		/// <summary>
		/// 그리드에서 행을 선택했을 시 발생
		/// </summary>
		private DataGridViewExDelegate mRowSelected = null;
		[Browsable(false)]
		public DataGridViewExDelegate RowSelected
		{
			get { return mRowSelected; }
			set { mRowSelected = value; }
		}

		/// <summary>
		/// 그리드에서 행을 옴겼을시 발생
		/// </summary>
		private DataGridViewExDelegate mRowMoved = null;
		[Browsable(false)]
		public DataGridViewExDelegate RowMoved
		{
			get { return mRowMoved; }
			set { mRowMoved = value; }
		}

		// Import 시작과 종료시 실행될 델리게이트 인수는 (true, false로 넘어옴)
		private DataGridViewExBoolDelegate mRowsAdding = null;
		[Browsable(false)]
		public DataGridViewExBoolDelegate RowsAdding
		{
			get { return mRowsAdding; }
			set { mRowsAdding = value; }
		}

		private bool mClipboardCopyWithHeaderText = false;
		public bool ClipboardCopyWithHeaderText
		{
			get
			{
				return mClipboardCopyWithHeaderText;
			}
			set
			{
				mClipboardCopyWithHeaderText = value;
			}
		}

		private bool mClipboardCopyWithHideColumn = false;
		public bool ClipboardCopyWithHideColumn
		{
			get
			{
				return mClipboardCopyWithHideColumn;
			}
			set
			{
				mClipboardCopyWithHideColumn = value;
			}
		}

		// DataGridViewEx 테두리
		private SimpleBox mBorderRight;
		private SimpleBox mBorderLeft;
		private SimpleBox mBorderTop;
		private SimpleBox mBorderBottom;

		private TableLayoutPanel mProgressBackPanel;
		private Panel mProgressPanel;
		private SimpleProgressBar mProgressBar;
		private FlatButton mProgressCancelButton;
		private ScrollBars mScrollBars;

		private BackgroundWorker mExportWorker = new BackgroundWorker();
		private BackgroundWorker mImportWorker = new BackgroundWorker();

		private ExportTypes mExportType = ExportTypes.CSV;

		private ContextMenuStrip mMenu = new ContextMenuStrip();

		private bool mRunAfterExport = false;

		private IDatabase mDB = null;
		private DataTable mDataTable = null;
		private bool mClearBeforeImportT = true;
		private string mSQL = string.Empty;
		private DataImportMethod mImportMethod = DataImportMethod.None;


		// 그리드의 셀을 마우스 더블클릭 할 시 해당 값을 클립보드로 자동 복사할 지 여부 
		private bool mUsingCellMouseDoubleClickCopyClipboard = true;
		public bool UsingCellMouseDoubleClickCopyClipboard
		{
			get { return mUsingCellMouseDoubleClickCopyClipboard; }
			set { mUsingCellMouseDoubleClickCopyClipboard = value; }
		}


		// 그리드의 셀을 수정하는데 수정 되는 즉시 ValueChanged를 보내도록 적용시킴. 원래는 변경이 끝나고 셀을 벗어날때 발생됨.
		private bool mUsingCurrentCellDirtyAutoCommit = true;
		public bool UsingCurrentCellDirtyAutoCommit
		{
			get { return mUsingCurrentCellDirtyAutoCommit; }
			set { mUsingCurrentCellDirtyAutoCommit = value; }
		}

		private bool mAllowCheckBoxExport = false;
		public bool AllowCheckBoxExport
		{
			get { return mAllowCheckBoxExport; }
			set { mAllowCheckBoxExport = value; }
		}

		private bool mUsingSort = false;
		public bool UsingSort
		{
			get { return mUsingSort; }
			set { mUsingSort = value; }
		}

		private bool mBorderVisible = true;
		public bool BorderVisible
		{
			get { return mBorderVisible; }
			set 
			{
				mBorderVisible = value;
				mBorderLeft.Visible = mBorderVisible;
				mBorderRight.Visible = mBorderVisible;
				mBorderTop.Visible = mBorderVisible;
				mBorderBottom.Visible = mBorderVisible;
			}
		}

		private Color mBorderColor = Color.FromArgb(165, 172, 181);
		public Color BorderColor
		{
			get { return mBorderColor; }
			set
			{
				if (mBorderColor != value)
				{
					mBorderColor = value;
					mBorderLeft.BackColor = mBorderColor;
					mBorderRight.BackColor = mBorderColor;
					mBorderTop.BackColor = mBorderColor;
					mBorderBottom.BackColor = mBorderColor;
				}
			}
		}

		private Dictionary<string, string> mStoredRowData = new Dictionary<string, string>();
		public Dictionary<string, string> StoredRowData
		{
			get 
			{ 
				return mStoredRowData; 
			}
		}

		private Dictionary<string, string> mSelectedRowData = new Dictionary<string, string>();
		private Dictionary<string, string> SelectedRowData
		{
			get
			{
				return mSelectedRowData;
			}
		}

		private bool mIsBusy = false;
		[Browsable(false)]
		public bool IsBusy
		{
			get { return mIsBusy; }
		}

		ExportWorkerData mExportWorkData = new ExportWorkerData();

		private bool mCloseEnabled = true;
		[Browsable(false)]
		public bool CloseEnabled
		{
			get { return mCloseEnabled; }
		}

		public ExportTypes ExportType
		{
			get { return mExportType; }
			set { mExportType = value; }
		}

		/// <summary>
		/// 내보내기 후 자동으로 연결된 프로그램을 실행할 지 여부
		/// </summary>
		public bool RunAfterExport
		{
			get { return mRunAfterExport; }
			set { mRunAfterExport = value; }
		}

		/// <summary>
		/// 프로그래스 바를 보이게 할 지 안할지 설정
		/// </summary>
		private bool ProgressVisible
		{
			get { return mProgressBackPanel.Visible; }
			set
			{
				if (value == true)
				{
					this.ScrollBars = ScrollBars.None;
					mProgressBackPanel.Visible = true;
				}
				else
				{
					mProgressBackPanel.Visible = false;
					this.ScrollBars = mScrollBars;
				}
			}
		}

		/// <summary>
		/// 프로그래스바의 최소값
		/// </summary>
		public int ProgressBarMinimum
		{
			get { return mProgressBar.Minimum; }
			set { mProgressBar.Minimum = value; }
		}

		/// <summary>
		/// 프로그래스바의 최대값
		/// </summary>
		public int ProgressBarMaximum
		{
			get { return mProgressBar.Maximum; }
			set { mProgressBar.Maximum = value; }
		}

		/// <summary>
		/// 프로그래스의 현재 값 설정
		/// </summary>
		public int ProgressBarValue
		{
			get { return mProgressBar.Value; }
			set 
			{ 
				if (mProgressBar.Maximum >= value)
					mProgressBar.Value = value; 
			}
		}

		/// <summary>
		/// 프로그래스 바안에 텍스트 설정
		/// </summary>
		public string ProgressBarText
		{
			get { return mProgressBar.Text; }
			set { mProgressBar.Text = value; }
		}


		/// <summary>
		/// 프로그래스 취소버튼 활성화 비활성화 설정
		/// </summary>
		public bool ProgressCancelButtonEnabled
		{
			get { return mProgressCancelButton.Enabled; }
			set { mProgressCancelButton.Enabled = value; }
		}


		public new ScrollBars ScrollBars
		{
			get { return base.ScrollBars; }
			set
			{
				if (mProgressBackPanel.Visible)
				{
					base.ScrollBars = ScrollBars.None;
				}
				else
				{
					mScrollBars = base.ScrollBars;
					base.ScrollBars = value;
				}
			}
		}

		/// <summary>
		/// 프로그래스 백그라운드 컬러.
		/// </summary>
		public Color ProgressBackColor
		{
			get { return mProgressBackPanel.BackColor; }
			set 
			{ 
				mProgressBackPanel.BackColor = value;
				mProgressPanel.BackColor = value;
			}
		}

		public int RowCountAll
		{
			get
			{
				return this.Rows.Count;
			}
		}

		public int RowCountShown
		{
			get
			{
				int count = 0;

				for (int i = 0; i < this.Rows.Count; i++)
				{
					if (this.Rows[i].Visible == true)
						count++;
				}

				return count;
			}
		}

		public int RowCountHidden
		{
			get
			{
				int count = 0;

				for (int i = 0; i < this.Rows.Count; i++)
				{
					if (this.Rows[i].Visible == false)
						count++;
				}

				return count;
			}
		}

		private bool mDisplayMenuCopyColumnWidth = false;
		public bool DisplayMenuCopyColumnWidth
		{
			get { return mDisplayMenuCopyColumnWidth; }
			set 
			{ 
				mDisplayMenuCopyColumnWidth = value;

				foreach (ToolStripMenuItem item in mMenu.Items)
				{
					if (item.Text == COPY_COLUMN_WIDTH)
						item.Visible = value;
				}
			}
		}

		public DataGridViewEx() : base()
		{
			Initialize();
			InitializeProgress();
		}

		private bool mUsingLostFocusSelectionColor = false;
		public bool UsingLostFocusSelectionColor
		{
			get { return mUsingLostFocusSelectionColor; }
			set { mUsingLostFocusSelectionColor = value; }
		}

		private Color mLostFocusSelectionBackColor = Color.FromKnownColor(KnownColor.Control);
		public Color LostFocusSelectionBackColor
		{
			get { return mLostFocusSelectionBackColor;  }
			set { mLostFocusSelectionBackColor = value; }
		}

		private Color mLostFocusSelectionForeColor = Color.FromKnownColor(KnownColor.ControlText);
		public Color LostFocusSelectionForeColor
		{
			get { return mLostFocusSelectionForeColor; }
			set { mLostFocusSelectionForeColor = value; }
		}

		/// <summary>
		/// 최초 초기화 수행
		/// </summary>
		private void Initialize()
		{
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			//this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.UpdateStyles();

			this.DoubleBuffered = true;

			base.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			mScrollBars = base.ScrollBars;

			mExportWorker.WorkerReportsProgress = true;
			mExportWorker.WorkerSupportsCancellation = true;

			mImportWorker.WorkerReportsProgress = true;
			mImportWorker.WorkerSupportsCancellation = true;

			#region 스타일

			this.BackgroundColor = System.Drawing.Color.White;
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;

			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();

			this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.ColumnHeadersHeight = 22;

			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = mDefaultSelectionBackColor;
			dataGridViewCellStyle2.SelectionForeColor = mDefaultSelectionForeColor;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.DefaultCellStyle = dataGridViewCellStyle2;
			this.EnableHeadersVisualStyles = false;
			this.MultiSelect = false;
			this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.RowHeadersWidth = 22;
			this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.RowTemplate.Height = 21;
			this.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

			#endregion

			#region DataGridView Border 박수~~~
			this.mBorderRight = new SimpleBox();
			this.mBorderLeft = new SimpleBox();
			this.mBorderTop = new SimpleBox();
			this.mBorderBottom = new SimpleBox();

			mBorderRight.BackColor = mBorderColor;
			mBorderRight.Width = 1;

			mBorderLeft.BackColor = mBorderColor;
			mBorderLeft.Width = 1;

			mBorderTop.BackColor = mBorderColor;
			mBorderTop.Height = 1;

			mBorderBottom.BackColor = mBorderColor;
			mBorderBottom.Height = 1;

			this.Controls.Add(mBorderRight);
			this.Controls.Add(mBorderLeft);
			this.Controls.Add(mBorderTop);
			this.Controls.Add(mBorderBottom);

			#endregion

			#region 프로그래스
			// 생성
			this.mProgressBackPanel = new TableLayoutPanel();
			this.mProgressPanel = new Panel();
			this.mProgressBar = new SimpleProgressBar();
			this.mProgressCancelButton = new FlatButton();

			this.mProgressBackPanel.SuspendLayout();
			this.mProgressPanel.SuspendLayout();

			// 
			// mReadingPanel
			// 
			mProgressBackPanel.BackColor = System.Drawing.Color.White;
			mProgressBackPanel.ColumnCount = 1;
			mProgressBackPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			mProgressBackPanel.Controls.Add(this.mProgressPanel, 0, 0);
			mProgressBackPanel.Location = new System.Drawing.Point(10, 10);
			mProgressBackPanel.RowCount = 1;
			mProgressBackPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			mProgressBackPanel.Size = new System.Drawing.Size(386, 62);
			mProgressBackPanel.Dock = DockStyle.Fill;
			mProgressBackPanel.Visible = false;
			// 
			// mReadingProgressPanel
			// 
			mProgressPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
			mProgressPanel.BackColor = System.Drawing.Color.White;
			mProgressPanel.Controls.Add(this.mProgressBar);
			mProgressPanel.Controls.Add(this.mProgressCancelButton);
			mProgressPanel.Location = new System.Drawing.Point(6, 6);
			mProgressPanel.Size = new System.Drawing.Size(186, 30);
			// 
			// mReadingProgressBar
			// 
			mProgressBar.TextShadow = false;
			mProgressBar.ColorText = System.Drawing.Color.Black;
			mProgressBar.GradientHighColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(240)))), ((int)(((byte)(170)))));
			mProgressBar.GradientLowColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(150)))), ((int)(((byte)(10)))));
			mProgressBar.Location = new System.Drawing.Point(4, 8);
			mProgressBar.Maximum = 100;
			mProgressBar.Minimum = 0;
			mProgressBar.Size = new System.Drawing.Size(150, 19);
			mProgressBar.Value = 10;
			mProgressBar.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			// 
			// mReadingCancelButton
			// 
			mProgressCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
			mProgressCancelButton.ButtonFaceColor = System.Drawing.SystemColors.ButtonFace;
			mProgressCancelButton.DisplayFocus = true;

			//mProgressCancelButton.FlatButtonBorderColor = System.Drawing.SystemColors.ControlDark;
			//mProgressCancelButton.FlatButtonBorderHotColor = System.Drawing.SystemColors.ControlDark;

			mProgressCancelButton.FlatButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));
			mProgressCancelButton.FlatButtonBorderHotColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(104)))), ((int)(((byte)(104)))));

			mProgressCancelButton.FlatButtonHotColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
			mProgressCancelButton.FlatButtonPressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(134)))));
			mProgressCancelButton.FlatButtonStyle = FadeFox.UI.FlatButtonStyle.Flat;
			mProgressCancelButton.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			mProgressCancelButton.Location = new System.Drawing.Point(152, 8);
			mProgressCancelButton.Name = "btnDataGirdViewExCancelButton";
			mProgressCancelButton.PopupButtonHighlightColor = System.Drawing.SystemColors.ButtonHighlight;
			mProgressCancelButton.PopupButtonPressedColor = System.Drawing.Color.White;
			mProgressCancelButton.PopupButtonShadowColor = System.Drawing.SystemColors.ButtonShadow;
			mProgressCancelButton.Size = new System.Drawing.Size(34, 19);
			mProgressCancelButton.TabIndex = 76;
			mProgressCancelButton.Text = "취소";
			mProgressCancelButton.TextAlign = System.Drawing.StringAlignment.Center;
			mProgressCancelButton.TextColor = System.Drawing.Color.Black;
			mProgressCancelButton.TextHotColor = System.Drawing.Color.Black;
			mProgressCancelButton.TextShadow = false;
			mProgressCancelButton.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			mProgressCancelButton.UseVisualStyleBackColor = true;



			this.Controls.Add(mProgressBackPanel);

			this.mProgressBackPanel.ResumeLayout(false);
			this.mProgressPanel.ResumeLayout(false);
			this.mProgressPanel.PerformLayout();

			#endregion


			mProgressCancelButton.Click += new EventHandler(mProgressCancelButton_Click);

			mExportWorker.DoWork += new DoWorkEventHandler(mExportingWorker_DoWork);
			mExportWorker.ProgressChanged += new ProgressChangedEventHandler(mExportingWorker_ProgressChanged);
			mExportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mExportingWorker_RunWorkerCompleted);

			mImportWorker.DoWork += new DoWorkEventHandler(mImportWorker_DoWork);
			mImportWorker.ProgressChanged += new ProgressChangedEventHandler(mImportWorker_ProgressChanged);
			mImportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mImportWorker_RunWorkerCompleted);

			mMenu.ItemClicked += new ToolStripItemClickedEventHandler(mMenu_ItemClicked);

			if (Excel2003.IsInstalled)
				mMenu.Items.Add(new ToolStripMenuItem(EXPORT_TO_EXCEL));

			mMenu.Items.Add(new ToolStripMenuItem(EXPORT_TO_CSV));

			mMenu.Items.Add(new ToolStripMenuItem(COPY_COLUMN_WIDTH));

			foreach (ToolStripMenuItem item in mMenu.Items)
			{
				if (item.Text == COPY_COLUMN_WIDTH)
					item.Visible = false;
			}

			ClearSortMode();
		}

		protected override void OnCurrentCellDirtyStateChanged(EventArgs e)
		{
			if (mUsingCurrentCellDirtyAutoCommit)
			{
				if (this.IsCurrentCellDirty)
				{
					this.CommitEdit(DataGridViewDataErrorContexts.Commit);
				}
			}

			base.OnCurrentCellDirtyStateChanged(e);
		}

		bool isCursorTypeofSizeWE;
		bool needDrawSplitter;
		Point currentMousePos;

		protected override void OnCursorChanged(EventArgs e)
		{
			if (this.Cursor == Cursors.SizeWE)
			{
				isCursorTypeofSizeWE = true;
			}
			else
			{
				isCursorTypeofSizeWE = false;
			}

			if (this.Cursor == Cursors.WaitCursor || this.Cursor == Cursors.AppStarting)
				this.Cursor = Cursors.Arrow;

			base.OnCursorChanged(e);
		}
		
		public void CopyClipBoard(int pIndex)
		{
			CopyClipBoard(pIndex, mClipboardCopyWithHeaderText, mClipboardCopyWithHideColumn);
		}

		public void CopyClipBoard(int pIndex, bool pClipboardCopyWithHeaderText, bool pClipboardCopyWithHideColumn)
		{
			if (this.Rows.Count < 1 ||
				pIndex >= this.Rows.Count ||
				this.Columns.Count < 1)
			{
			//	Clipboard.SetText("");
				return;
			}

			StringBuilder str = new StringBuilder();

			bool firstColumn = true;

			if (pClipboardCopyWithHeaderText)
			{
				// Copy HeaderText
				for (int i = 0; i < this.Columns.Count; i++)
				{
					if (pClipboardCopyWithHideColumn)
					{
						if (firstColumn)
						{
							firstColumn = false;
						}
						else
						{
							str.Append("\t");
						}

						str.Append(this.Columns[i].HeaderText);
					}
					else
					{
						if (this.Columns[i].Visible)
						{
							if (firstColumn)
							{
								firstColumn = false;
							}
							else
							{
								str.Append("\t");
							}

							str.Append(this.Columns[i].HeaderText);
						}
					}
				}

				str.Append("\r\n");
			}

			firstColumn = true;

			// Copy Content
			for (int i = 0; i < this.Columns.Count; i++)
			{
				if (pClipboardCopyWithHideColumn)
				{
					if (firstColumn)
					{
						firstColumn = false;
					}
					else
					{
						str.Append("\t");
					}

					str.Append(this.Rows[pIndex].Cells[i].Value.ToString());
				}
				else
				{
					if (this.Columns[i].Visible)
					{
						if (firstColumn)
						{
							firstColumn = false;
						}
						else
						{
							str.Append("\t");
						}

						str.Append(this.Rows[pIndex].Cells[i].Value.ToString());
					}
				}
			}

			if (str.Length > 0)
				Clipboard.SetText(str.ToString());
		}
		
		/*
		private bool mFirstDefaultCellStyleChanged = true;

		protected override void OnDefaultCellStyleChanged(EventArgs e)
		{
			if (mUsingLostFocusSelectionColor)
			{
				if (mFirstDefaultCellStyleChanged)
				{
					this.DefaultCellStyle.SelectionBackColor = mLostFocusSelectionBackColor;
					this.DefaultCellStyle.SelectionForeColor = mLostFocusSelectionForeColor;

					mFirstDefaultCellStyleChanged = false;
				}
			
			}

			base.OnDefaultCellStyleChanged(e);
		}
		*/

		protected override void OnGotFocus(EventArgs e)
		{
			if (mUsingLostFocusSelectionColor)
			{
				this.DefaultCellStyle.SelectionBackColor = mDefaultSelectionBackColor;
				this.DefaultCellStyle.SelectionForeColor = mDefaultSelectionForeColor;
			}
			
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (mUsingLostFocusSelectionColor)
			{
				this.DefaultCellStyle.SelectionForeColor = mLostFocusSelectionForeColor;
				this.DefaultCellStyle.SelectionBackColor = mLostFocusSelectionBackColor;
			}
			
			base.OnLostFocus(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
			{
				if (this.Rows.Count > 0)
				{
					if (this.Columns.Count > 0)
					{
						if (this.SelectedRows != null)
						{
							if (this.SelectedRows[0].Index >= 0)
							{
								CopyClipBoard(this.SelectedRows[0].Index);
							}
						}
					}
				}
			}

			base.OnKeyDown(e);
		}

		[Browsable(false)]
		new public DataGridViewClipboardCopyMode ClipboardCopyMode
		{
			get
			{
				return DataGridViewClipboardCopyMode.Disable;
			}
			set
			{
				;
			}
		}
		

		/// <summary>
		/// 행 해더를 보일지 안보일지 설정
		/// </summary>
		public new bool RowHeadersVisible
		{
			get { return base.RowHeadersVisible; }
			set
			{
				base.RowHeadersVisible = value;
			}
		}

		/// <summary>
		/// 컬럼 해더를 보일지 안보일지 설정
		/// </summary>
		public new bool ColumnHeadersVisible
		{
			get { return base.ColumnHeadersVisible; }
			set
			{
				base.ColumnHeadersVisible = value;
			}
		}

		/*
		protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseClick(e);

			if (mIsBusy)
				return;

			if (e.ColumnIndex > 0 && e.RowIndex > 0)
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Right)
				{
					if (mUsingExport)
						mMenu.Show(this, e.X, e.Y);
				}
			}
		}
		*/

		
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);

			if (mIsBusy)
				return;

			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (mUsingExport)
					mMenu.Show(this, e.X, e.Y);
			}
		}
		

		private const string EXPORT_TO_EXCEL = "Excel로 내보내기";
		private const string EXPORT_TO_CSV = "CSV로 내보내기";
		private const string COPY_COLUMN_WIDTH = "Copy Column Width";

		private void mMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripMenuItem menuItem = e.ClickedItem as ToolStripMenuItem;

			if (menuItem == null)
				return;

			mMenu.Hide();

			switch (menuItem.Text)
			{
				case EXPORT_TO_EXCEL:
					this.ExportType = ExportTypes.Excel;
					ExportStart();
					break;
				case EXPORT_TO_CSV:
					this.ExportType = ExportTypes.CSV;
					ExportStart();
					break;
				case COPY_COLUMN_WIDTH:
					CopyClipboardColumnWidth();
					break;
			}
		}

		/// <summary>
		/// Progresss에 대한 초기화 수행
		/// </summary>
		private void InitializeProgress()
		{
			mProgressBar.Minimum = 0;
			mProgressBar.Maximum = 100;
			mProgressBar.Value = 0;
			mProgressBar.Text = "";
			mProgressCancelButton.Enabled = true;
			mTotalRowCount = 0;
		}

		/// <summary>
		/// 프로그래스 시작
		/// </summary>
		public bool BeginProgress()
		{
			if (mIsBusy)
				return false;
			else
			{
				InitializeProgress();
				this.ProgressVisible = true;
				return true;
			}
		}

		/// <summary>
		/// 프로그래스 종료
		/// </summary>
		public void EndProgress()
		{
			this.ProgressVisible = false;
		}

		#region ######## Invoke 함수

		public bool InvokeProgressVisible
		{
			set
			{
				if (this.InvokeRequired)
				{
					mProgressBackPanel.Invoke(new MethodInvoker(delegate()
					{
						this.ProgressVisible = value;
					}));
				}
				else
				{
					this.ProgressVisible = value;
				}
			}
		}

		public int InvokeProgressBarValue
		{
			set
			{
				if (this.InvokeRequired)
				{
					mProgressBar.Invoke(new MethodInvoker(delegate()
					{
						this.ProgressBarValue = value;
					}));
				}
				else
				{
					this.ProgressBarValue = value;
				}
			}
		}

		public string InvokeProgressBarText
		{
			set
			{
				if (this.InvokeRequired)
				{
					mProgressBar.Invoke(new MethodInvoker(delegate()
					{
						this.ProgressBarText = value;
					}));
				}
				else
				{
					this.ProgressBarText = value;
				}
			}
		}


		/// <summary>
		/// Invoke를 사용하여 프로그래스 취소버튼 활성화 비활성화 설정
		/// </summary>
		public bool InvokeProgressCancelButtonEnabled
		{
			set
			{
				try
				{
					if (this.InvokeRequired)
					{
						mProgressCancelButton.Invoke(new MethodInvoker(delegate()
						{
							this.ProgressCancelButtonEnabled = value;
						}));
					}
					else
					{
						this.ProgressCancelButtonEnabled = value;
					}
				}
				catch
				{
					return;
				}
			}
		}

		/// <summary>
		/// Invoke를 사용하여 행 초기화
		/// </summary>
		public void InvokeRowsClear()
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Rows.Clear();
						ClearSortMode();
					}));
				}
				else
				{
					this.Rows.Clear();
					ClearSortMode();
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Invoke를 사용하여 열 초기화
		/// </summary>
		public void InvokeColumnsClear()
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns.Clear();
					}));
				}
				else
				{
					this.Columns.Clear();
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Invoke를 사용해여 행 추가
		/// </summary>
		/// <param name="pRow">행에 추가할 내용</param>
		public void InvokeRowsAdd(string[] pRow)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Rows.Add(pRow);
					}));
				}
				else
				{
					this.Rows.Add(pRow);
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Invoke를 사용해여 행 추가
		/// </summary>
		/// <param name="pRow">행에 추가할 내용</param>
		public void InvokeRowsAdd(object[] pRow)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Rows.Add(pRow);
					}));
				}
				else
				{
					this.Rows.Add(pRow);
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Invoke를 사용해여 행 추가
		/// </summary>
		/// <param name="pRow">행에 추가할 내용</param>
		public void InvokeRowsAdd()
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Rows.Add();
					}));
				}
				else
				{
					this.Rows.Add();
				}
			}
			catch
			{
				return;
			}
		}

		public void InvokeSeFillWeight(int pIndex, int pWidth)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns[pIndex].FillWeight = pWidth;
					}));
				}
				else
				{
					this.Columns[pIndex].FillWeight = pWidth;
				}
			}
			catch
			{
				return;
			}
		}

		public void InvokeSetWidth(int pIndex, int pWidth)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns[pIndex].Width = pWidth;
					}));
				}
				else
				{
					this.Columns[pIndex].Width = pWidth;
				}
			}
			catch
			{
				return;
			}
		}

		public void InvokeSetAutoSizeMode(int pIndex, DataGridViewAutoSizeColumnMode pMode)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns[pIndex].AutoSizeMode = pMode;
					}));
				}
				else
				{
					this.Columns[pIndex].AutoSizeMode = pMode;
				}
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Invoke를 사용하여 열 추가
		/// </summary>
		/// <param name="pColumn"></param>
		public void InvokeColumnAdd(DataGridViewColumn pColumn)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns.Add(pColumn);
					}));
				}
				else
				{
					this.Columns.Add(pColumn);
				}
			}
			catch
			{
				return;
			}
		}

		public void InvokeColumnAdd(string pColumnName, string pHeaderText)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Columns.Add(pColumnName, pHeaderText);
					}));
				}
				else
				{
					this.Columns.Add(pColumnName, pHeaderText);
				}
			}
			catch
			{
				return;
			}
		}

		public void InvokeSetValue(int pRowIndex, int pColumnIndex, object pValue)
		{
			try
			{
				if (this.InvokeRequired)
				{
					this.Invoke(new MethodInvoker(delegate()
					{
						this.Rows[pRowIndex].Cells[pColumnIndex].Value = pValue;
					}));
				}
				else
				{
					this.Rows[pRowIndex].Cells[pColumnIndex].Value = pValue;
				}
			}
			catch
			{
				return;
			}
		}

		#endregion

		/// <summary>
		/// 취소 버튼 클릭 이벤트
		/// </summary>
		public event EventHandler ProgressCancelButtonClick
		{
			add 
			{
				mProgressCancelButton.Click += value;
			}
			remove 
			{
				mProgressCancelButton.Click -= value;
			}
		}

		/// <summary>
		/// LTButton을 Export 버튼으로 바로 사용.
		/// </summary>
		private bool mUsingExport = true;
		public bool UsingExport
		{
			get { return mUsingExport; }
			set { mUsingExport = value; }
		}


		/// <summary>
		/// 컨트롤 Disposing 시
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (mExportWorker.IsBusy)
			{
				mExportWorker.CancelAsync();
			}

			if (mImportWorker.IsBusy)
			{
				mImportWorker.CancelAsync();
			}

			if (disposing && mExportWorker.IsBusy)
			{
				if (mExportWorkData.Writer != null)
				{
					mExportWorkData.Writer.Close();
					mExportWorkData.Writer.Dispose();
					mExportWorkData.Writer = null;
				}
			}

			base.Dispose(disposing);
		}

		internal class ExportWorkerData
		{
			private string mFileName;
			public string FileName
			{
				get { return mFileName; }
				set { mFileName = value; }
			}

			private XlsDocument _XlsDocument = new XlsDocument();
			private Worksheet _Worksheet = null;

			public Worksheet WorkSheet
			{
				get
				{
					if (_Worksheet == null)
						_Worksheet = _XlsDocument.Workbook.Worksheets.Add("Sheet1");

					return _Worksheet;
				}
			}

			private TextWriter mWriter = null;
			public TextWriter Writer
			{
				get { return mWriter; }
				set { mWriter = value; }
			}

			public void SaveXls()
			{
				ColumnInfo columnInfo = new ColumnInfo(_XlsDocument, _Worksheet); 
//				columnInfo.ColumnIndexStart = 0;
//				columnInfo.ColumnIndexEnd = 4;
				//columnInfo.Width = 10;
//				_Worksheet.AddColumnInfo(columnInfo);
				
				_XlsDocument.FileName = mFileName;
				_XlsDocument.Save(true);
			}
		}

		public event EventHandler ExportStarted = null;
		public event EventHandler ExportFinished = null;
		public event EventHandler ExportCanceled = null;

		protected virtual void OnExportStarted(object sender, EventArgs e)
		{
			if (ExportStarted != null)
				ExportStarted(sender, e);
		}

		protected virtual void OnExportFinished(object sender, EventArgs e)
		{
			if (ExportFinished != null)
				ExportFinished(sender, e);
		}

		protected virtual void OnExportCanceled(object sender, EventArgs e)
		{
			if (ExportCanceled != null)
				ExportCanceled(sender, e);
		}

		/// <summary>
		/// 내보내기 시작
		/// </summary>
		/// 

		public void ExportStart()
		{
			if (mIsBusy)
			{
				MsgBox.Show("처리중인 작업이 있습니다.", MsgType.Warning);
				return;
			}

			if (mExportWorker.IsBusy)
			{
				MsgBox.Show("아직 내보내기 중입니다.", MsgType.Warning);
				return;
			}

			SaveFileDialog dialog = new SaveFileDialog();
			dialog.ValidateNames = true;

			switch (mExportType)
			{
				case ExportTypes.CSV:
					dialog.Filter = "CSV (쉼표로 분리) (*.csv)|*.csv";
					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						mExportWorkData.FileName = dialog.FileName;
						mCloseEnabled = false;
						mExportWorkData.Writer = new StreamWriter(mExportWorkData.FileName, false, Encoding.Default);
					}
					else
					{
						mCloseEnabled = true;
						return;
					}
					break;
				case ExportTypes.Excel:
					dialog.Filter = "Excel 97 - 2003 통합 문서 (*.xls)|*.xls";

					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						mExportWorkData.FileName = dialog.FileName;
						mCloseEnabled = false;
					}
					else
					{
						mCloseEnabled = true;
						return;
					}
					break;
				default:
					return;
			}

			InitializeProgress();

			OnExportStarted(this, new EventArgs());

			this.ProgressVisible = true;

			mIsBusy = true;
			this.mExportWorker.RunWorkerAsync();
		}

		/// <summary>
		/// 프로그래스 취소 버튼 클릭시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mProgressCancelButton_Click(object sender, EventArgs e)
		{
			CancelWork();
		}

		/// <summary>
		/// 진행중인 작업 취소
		/// </summary>
		public void CancelWork()
		{
			if (mExportWorker.IsBusy)
			{
				mProgressCancelButton.Enabled = false;
				mExportWorker.CancelAsync();
			}

			if (mImportWorker.IsBusy)
			{
				mProgressCancelButton.Enabled = false;
				mImportWorker.CancelAsync();
			}
		}

		private void mExportingWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bw = sender as BackgroundWorker;

			try
			{
				switch (mExportType)
				{
					case ExportTypes.CSV:
						ExportCSV(bw);
						break;

					case ExportTypes.Excel:
						ExportExcel(bw);
						break;
				}
			}
			catch (Exception ex)
			{
				MsgBox.Show(ex);
				e.Cancel = true;
			}

			if (bw.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		private void mExportingWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.ProgressBarValue = e.ProgressPercentage;
			this.ProgressBarText = TextCore.ToCommaString(mTotalRowCount) + "건 내보내기중. " + e.ProgressPercentage.ToString() + "%";
		}

		private void mExportingWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			mCloseEnabled = true;
			mIsBusy = false;

			if (e.Cancelled)
			{
				if (mExportWorkData.Writer != null)
				{
					//Trace.WriteLine("TextExportCanceled");
					mExportWorkData.Writer.Close();
					mExportWorkData.Writer.Dispose();
					mExportWorkData.Writer = null;
				}
				OnExportCanceled(this, new EventArgs());
				//Trace.WriteLine("ExportCanceled");
			}
			else if (e.Error != null)
			{
				MsgBox.Show(e.Error.Message, MsgType.Error);
			}
			else
			{
				OnExportFinished(this, new EventArgs());
			}

			this.ProgressVisible = false;
			InitializeProgress();
		}

		#region Export

		/// <summary>
		/// 내보내기시 보이는 컬럼만 할지 전부다 할지 여부
		/// </summary>
		private bool mExportVisibleColumnOnly = true;
		public bool ExportVisibleColumnOnly
		{
			get { return mExportVisibleColumnOnly; }
			set { mExportVisibleColumnOnly = value; }
		}

		/// <summary>
		/// 내보내기시 해더와 같이 내보낼지 안할지 여부
		/// </summary>
		private bool mExportWithHeader = true;
		public bool ExportWithHeader
		{
			get { return mExportWithHeader; }
			set { mExportWithHeader = value; }
		}

		private bool IsExportableColumn(int pColumnIndex)
		{
			return IsExportableColumn(this.Columns[pColumnIndex]);
		}

		private bool IsExportableColumn(DataGridViewColumn pColumn)
		{
			if (pColumn is DataGridViewButtonColumn ||
				pColumn is DataGridViewImageColumn)
				return false;

			if (pColumn is DataGridViewCheckBoxColumn)
				if (!mAllowCheckBoxExport)
					return false;

			if (mExportVisibleColumnOnly)
			{
				if (pColumn.Visible)
					return true;
				else
					return false;
			}
			else
			{
				return true;
			}
		}

		private object CellValue(DataGridViewCell pCell)
		{
			if (pCell is DataGridViewCheckBoxCell)
			{
				if (pCell.Value is DBNull)
					return "";

				if (pCell.FormattedValue is bool)
					return ((bool)pCell.FormattedValue) ? "Y" : "N";
				else
				{
					object falseValue = ((DataGridViewCheckBoxColumn)pCell.DataGridView.Columns[pCell.ColumnIndex]).FalseValue;
					if (falseValue != null)
						return falseValue.Equals(pCell.Value.ToString()) ? "N" : "Y";
				}
			}
			else if (TextCore.IsNumeric(pCell.Value) || pCell.Value is DateTime)
				return pCell.Value;

			return pCell.FormattedValue;
		}

		#region CSV

		private void ExportCSV(BackgroundWorker pBw)
		{
			if (mExportWithHeader)
			{
				string line = "";
				for (int i = 0; i < this.ColumnCount; i++)
				{
					if (pBw.CancellationPending)
						return;

					if (IsExportableColumn(i))
					{
						if (pBw.CancellationPending)
							return;

						line += "," + this.Columns[i].HeaderText;
					}
				}

				mExportWorkData.Writer.WriteLine(line.Substring(1));
			}

			mTotalRowCount = this.RowCount;
			int prevPercent = 0;
			int percent = 0;

			for (int row = 0; row < this.RowCount; row++)
			{
				string line = "";
				for (int column = 0; column < this.ColumnCount; column++)
				{
					if (pBw.CancellationPending)
						return;

					if (IsExportableColumn(column))
					{
						if (pBw.CancellationPending)
							return;

						line += "," + CellValue(this[column, row]).ToString();
					}
				}

				mExportWorkData.Writer.WriteLine(line.Substring(1));

				percent = (int)((float)row / (float)mTotalRowCount * 100);

				if (percent > prevPercent)
				{
					mExportWorker.ReportProgress(percent);
					prevPercent = percent;
				}

				if (pBw.CancellationPending)
					return;
			}

			if (mExportWorkData.Writer != null)
			{
				mExportWorkData.Writer.Close();
				mExportWorkData.Writer.Dispose();
				mExportWorkData.Writer = null;
			}

			if (mRunAfterExport)
				System.Diagnostics.Process.Start(mExportWorkData.FileName);
		}

		#endregion CSV

		#region Excel

		private void ExportExcel(BackgroundWorker pBw)
		{
			// WorksheetName Setting
			if (this.Parent != null)
			{
				string name = this.Parent.Text;

				foreach (char c in ":\\/?*[]")
					name = name.Replace(c.ToString(), "");

				if (name.Length > 31)
					name = name.Substring(0, 28) + "...";

				if (name != "")
					mExportWorkData.WorkSheet.Name = name;
			}

			int startRow = 1;

			// Export Headers
			if (mExportWithHeader == true)
			{
				for (int column = 0, worksheetColumn = 1; column < this.ColumnCount; column++)
				{
					if (pBw.CancellationPending)
						return;

					if (IsExportableColumn(column))
					{
						if (pBw.CancellationPending)
							return;

						Cell cell = mExportWorkData.WorkSheet.Cells.Add(1, worksheetColumn++, this.Columns[column].HeaderText);
						cell.Font.Bold = true;

						if (pBw.CancellationPending)
							return;
					}
				}

				startRow++;
			}

			// Export Cell
			mTotalRowCount = this.RowCount;
			int prevPercent = 0;
			int percent = 0;

			for (int row = 0; row < this.RowCount; row++)
			{
				for (int column = 0, worksheetColumn = 1; column < this.ColumnCount; column++)
				{
					if (pBw.CancellationPending)
						return;

					if (IsExportableColumn(column))
					{
						if (pBw.CancellationPending)
							return;

						Cell cell = mExportWorkData.WorkSheet.Cells.Add(row + startRow, worksheetColumn, CellValue(this[column, row]));


						worksheetColumn++;
					}
				}

				percent = (int)((float)row / (float)mTotalRowCount * 100);

				if (percent > prevPercent)
				{
					mExportWorker.ReportProgress(percent);
					prevPercent = percent;
				}

				if (pBw.CancellationPending)
					return;
			}

			mExportWorkData.SaveXls();

			if (mRunAfterExport)
				System.Diagnostics.Process.Start(mExportWorkData.FileName);

			return;
		}

		#endregion Excel

		#endregion Export

		#region DataTable Import

		public event EventHandler ImportStarted = null;
		public event EventHandler ImportFinished = null;
		public event EventHandler ImportCanceled = null;

		protected virtual void OnImportStarted(object sender, EventArgs e)
		{
			if (ImportStarted != null)
				ImportStarted(sender, e);
		}

		protected virtual void OnImportFinished(object sender, EventArgs e)
		{
			if (ImportFinished != null)
				ImportFinished(sender, e);
		}

		protected virtual void OnImportCanceled(object sender, EventArgs e)
		{
			if (ImportCanceled != null)
				ImportCanceled(sender, e);
		}


		private void mImportWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker bw = sender as BackgroundWorker;

			e.Result = ReadData(bw);

			if (bw.CancellationPending)
			{
				e.Cancel = true;
			}
		}

		private void mImportWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			this.ProgressBarValue = e.ProgressPercentage;
			this.ProgressBarText = TextCore.ToCommaString(mTotalRowCount) + "건 읽는중. " + e.ProgressPercentage.ToString() + "%";
		}

		private void mImportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			mCloseEnabled = true;
			mIsBusy = false;

			if (e.Cancelled)
			{
				OnImportCanceled(this, new EventArgs());
			}
			else if (e.Error != null)
			{
				MsgBox.Show(e.Error.Message, MsgType.Error);
			}
			else
			{
				OnImportFinished(this, new EventArgs());
			}

			// Import 작업 종료후의 컨트롤
			if (mRowsAdding != null)
			{
				mRowsAdding(false);
			}

			this.ProgressVisible = false;
			InitializeProgress();
		}

		private void ImportStart()
		{
			if (mIsBusy)
			{
				MsgBox.Show("아직 처리중인 작업이 있습니다.", MsgType.Warning);
				return;
			}

			if (mImportWorker.IsBusy)
			{
				MsgBox.Show("아직 불러오기 중입니다.", MsgType.Warning);
				return;
			}

			InitializeProgress();

			OnImportStarted(this, new EventArgs());

			// Import 작업 시작후의 엑션.
			if (mRowsAdding != null)
			{
				mRowsAdding(true);
			}

			this.ProgressVisible = true;
			//this.ProgressMessage = "총 " + TextCore.ToCommaString(this.RowCount) + "건 불러오는중.";

			mIsBusy = true;
			this.mImportWorker.RunWorkerAsync();
		}


		public int ImportR(int pIndex, params object[] values)
		{
			if (pIndex < 0)
			{
				return this.Rows.Add(values);
			}
			else
			{
				if (pIndex >= this.Rows.Count)
					return -1;

				this.Rows[pIndex].SetValues(values);
			}

			return pIndex;
		}

		public int ImportR(IDatabase pDB, string pSql, int pIndex)
		{
			DataTable dt = pDB.SelectT(pSql);

			return ImportR(dt, pIndex);
		}

		public int ImportR(DataTable pDT, int pIndex)
		{
			if (mRowsPreAdding != null)
				mRowsPreAdding(pDT);

			if (pDT == null)
			{
				return -1;
			}

			if (pDT.Rows.Count != 1)
			{
				return -1;
			}

			if (this.Columns.Count < 1)
			{
				return -1;
			}

			int[] matchColumnIndex = new int[this.Columns.Count];
			string[] columnDefaultValue = new string[this.Columns.Count];

			if (MatchColumnInfo(pDT, out matchColumnIndex, out columnDefaultValue) == false)
				return -1;

			int applyIndex = -1;

			for (int i = 0; i < pDT.Rows.Count; i++)
			{
				List<object> list = new List<object>();

				for (int j = 0; j < matchColumnIndex.Length; j++)
				{
					if (matchColumnIndex[j] >= 0)
					{
						list.Add(pDT.Rows[i][matchColumnIndex[j]]);
					}
					else
					{
						list.Add(columnDefaultValue[j]);
					}
				}

				if (this.Rows.Count < 1)
				{
					applyIndex = this.Rows.Add(list.ToArray());
				}
				else
				{
					if (pIndex >= 0 && pIndex < this.Rows.Count)
					{
						this.Rows[pIndex].SetValues(list.ToArray());
						applyIndex = pIndex;
					}
					else
					{
						applyIndex = this.Rows.Add(list.ToArray());
					}
				}
			}

			return applyIndex;
		}

		public void ImportT(IDatabase pDB, string pSql)
		{
			ImportT(pDB, pSql, true);
		}

		public void ImportT(IDatabase pDB, string pSql, bool pClearBeforeImportT)
		{
			mDB = pDB;
			mSQL = pSql;
			mImportMethod = DataImportMethod.Database;
			mClearBeforeImportT = pClearBeforeImportT;

			mStoredRow = null;
			mStoredRowData.Clear();

			ImportStart();
		}

		public void ImportT(DataTable pDataTable)
		{
			ImportT(pDataTable, true);
		}

		public void ImportT(DataTable pDataTable, bool pClearBeforeImportT)
		{
			mDataTable = pDataTable;
			mImportMethod = DataImportMethod.Datatable;
			mClearBeforeImportT = pClearBeforeImportT;

			mStoredRow = null;
			mStoredRowData.Clear();

			ImportStart();
		}

		public void ImportT()
		{
			ImportT(true);
		}

		public void ImportT(bool pClearBeforeImportT)
		{
			mImportMethod = DataImportMethod.Delegate;
			mClearBeforeImportT = pClearBeforeImportT;

			mStoredRow = null;
			mStoredRowData.Clear();

			ImportStart();
		}

		private int mTotalRowCount = 0;

		private int ReadData(BackgroundWorker pBw)
		{
			if (mClearBeforeImportT)
				this.InvokeRowsClear();

			mTotalRowCount = this.Rows.Count;

			this.InvokeProgressBarText = "읽는중.";

			this.InvokeProgressCancelButtonEnabled = false;

			DataTable dt = null;

			switch (mImportMethod)
			{
				case DataImportMethod.Database:
					dt = mDB.SelectT(mSQL);
					break;
				case DataImportMethod.Datatable:
					dt = mDataTable;
					break;
				case DataImportMethod.Delegate:
					if (mRowsDataRead != null)
						mRowsDataRead(out dt);

					if (dt == null)
						return -1;

					break;
				default:
					return -1;
			}

			if (mRowsPreAdding != null)
				mRowsPreAdding(dt);

			this.InvokeProgressCancelButtonEnabled = true;

			if (dt == null)
			{
				return -1;
			}

			if (dt.Rows.Count < 1)
			{
				return -1;
			}

			if (this.Columns.Count < 1)
			{
				return -1;
			}

			int[] matchColumnIndex = new int[this.Columns.Count];
			string[] columnDefaultValue = new string[this.Columns.Count];

			if (MatchColumnInfo(dt, out matchColumnIndex, out columnDefaultValue) == false)
				return -1;

			this.InvokeProgressCancelButtonEnabled = true;

			mTotalRowCount += dt.Rows.Count;


			int rowCount = 0;
			int prevPercent = 0;
			int percent = 0;

			for (int i = 0; i < dt.Rows.Count; i++)
			{
				List<object> list = new List<object>();

				for (int j = 0; j < matchColumnIndex.Length; j++)
				{
					if (matchColumnIndex[j] >= 0)
					{
						list.Add(dt.Rows[i][matchColumnIndex[j]]);
					}
					else
					{
						list.Add(columnDefaultValue[j]);
					}
				}

				this.InvokeRowsAdd(list.ToArray());

				rowCount++;

				percent = (int)((float)rowCount / (float)mTotalRowCount * 100);

				if (percent > prevPercent)
				{
					mImportWorker.ReportProgress(percent);
					prevPercent = percent;
				}

				if (pBw.CancellationPending)
					break;
			}

			dt.Dispose();

			return rowCount;
		}

		private bool MatchColumnInfo(DataTable pDT, out int[] pMatchColumnIndex, out string[] pColumnDefaultValue)
		{
			int[] matchColumnIndex = new int[this.Columns.Count];
			string[] columnDefaultValue = new string[this.Columns.Count];
			bool isMatched = false;

			for (int i = 0; i < matchColumnIndex.Length; i++)
			{
				matchColumnIndex[i] = -1;
				columnDefaultValue[i] = string.Empty;
			}

			for (int i = 0; i < this.Columns.Count; i++)
			{
				// Default Value 얻기
				ColumnType ctype = GetColumnType(i);

				if (ctype == ColumnType.CheckBox)
				{
					DataGridViewCheckBoxColumn s = this.Columns[i] as DataGridViewCheckBoxColumn;

					if (s != null)
					{
						if (s.FalseValue != null)
						{
							columnDefaultValue[i] = s.FalseValue.ToString();
						}
					}
				}

				string gridColumnName = this.Columns[i].Name.ToUpper();

				for (int j = 0; j < pDT.Columns.Count; j++)
				{
					if (gridColumnName == pDT.Columns[j].ColumnName.ToUpper())
					{
						matchColumnIndex[i] = j;
						isMatched = true;
						break;
					}
				}
			}

			pMatchColumnIndex = matchColumnIndex;
			pColumnDefaultValue = columnDefaultValue;

			return isMatched;
		}

		#endregion

		protected override void OnSelectionChanged(EventArgs e)
		{
			if (!this.mIsBusy)
			{
				if (this.SelectedRows.Count > 0 && this.RowCountShown > 0)
				{
					if (mRowSelected != null)
					{
						mRowSelected();
					}
				}
			}

			base.OnSelectionChanged(e);
		}

		DataGridViewRow mStoredRow = null;

		[Browsable(false)]
		public int StoredRowIndex
		{
			get 
			{
				if (mStoredRow != null)
					return mStoredRow.Index;
				else
					return -1;
			}
		}

		[Browsable(false)]
		public DataGridViewRow StoredRow
		{
			get { return mStoredRow; }
		}

		public bool StoreRowData(DataTable pDT)
		{
			mStoredRow = null;
			mStoredRowData.Clear();

			if (pDT == null)
				return false;

			if (pDT.Rows.Count < 1)
				return false;

			for (int i = 0; i < pDT.Columns.Count; i++)
			{
				mStoredRowData.Add(pDT.Columns[i].ColumnName.ToUpper(), pDT.Rows[0][i].ToString());
			}

			return true;
		}

		public bool StoreRowData(int pIndex)
		{
			if (mStoredRow != null)
			{
				for (int i = 0; i < this.Columns.Count; i++)
				{
					this.mStoredRow.Cells[i].Style.ForeColor = Color.Black;
				}
			}

			mStoredRow = null;
			mStoredRowData.Clear();

//			if (this.mIsBusy == true)
//				return false;

			if (this.Rows.Count < 1)
				return false;

			if (pIndex >= this.Rows.Count)
				return false;

			if (this.Columns.Count < 1)
				return false;

			for (int i = 0; i < this.Columns.Count; i++)
			{
				if (this.Rows[pIndex].Cells[i].Value != null)
					mStoredRowData.Add(this.Columns[i].Name, this.Rows[pIndex].Cells[i].Value.ToString());
				else
					mStoredRowData.Add(this.Columns[i].Name, string.Empty);

				this.Rows[pIndex].Cells[i].Style.ForeColor = mStoredRowForeColor;
			}

			mStoredRow = this.Rows[pIndex];
			return true;
		}

		public Dictionary<string, string> GetRowData(int pIndex)
		{
			Dictionary<string, string> rowData = new Dictionary<string, string>();

			if (this.Rows.Count < 1)
				return null;

			if (pIndex >= this.Rows.Count)
				return null;

			if (this.Columns.Count < 1)
				return null;

			for (int i = 0; i < this.Columns.Count; i++)
			{
				if (this.Rows[pIndex].Cells[i].Value != null)
					rowData.Add(this.Columns[i].Name, this.Rows[pIndex].Cells[i].Value.ToString());
				else
					rowData.Add(this.Columns[i].Name, string.Empty);
			}

			return rowData;
		}

		private void StoreSelectedRowData()
		{
			mSelectedRowData.Clear();

			if (this.Rows.Count < 1)
				return;

			if (this.SelectedRows == null)
				return;

			if (this.SelectedRows[0].Index < 0)
				return;

			for (int i = 0; i < this.Columns.Count; i++)
			{
				if (this.SelectedRows[0].Cells[i].Value != null)
				{
					mSelectedRowData.Add(this.Columns[i].Name, this.SelectedRows[0].Cells[i].Value.ToString());
				}
				else
				{
					mSelectedRowData.Add(this.Columns[i].Name, string.Empty);
				}
			}
		}

		public int SelectCell(string pColumnName, string pValue)
		{
			int result = -1;

			if (this.mIsBusy)
				return result;

			if (this.Rows.Count < 1)
				return result;

			foreach (DataGridViewRow r in this.Rows)
			{
				if (r.Cells[pColumnName].Value.ToString() == pValue)
				{
					result = r.Index;
					SelectRow(result);
					break;
				}
			}

			return result;
		}

		public int RemoveR(int pIndex)
		{
			int result = -1;

			if (this.mIsBusy == true)
				return result;

			if (this.RowCountShown < 1)
				return result;

			if (pIndex >= this.Rows.Count)
				return result;

			if (this.Rows[pIndex].Visible == false)
				return result;

			this.Rows.RemoveAt(pIndex);

			for (int i = (pIndex - 1); i >= 0; i--)
			{
				if (this.Rows[i].Visible)
				{
					result = i;
					SelectRow(i);
					break;
				}
			}

			if (result == -1)
			{
				for (int i = (pIndex + 1); i < this.Rows.Count; i++)
				{
					if (this.Rows[i].Visible)
					{
						result = i;
						SelectRow(i);
						break;
					}
				}
			}

			//Debug.WriteLine("DataGridViewEx->RemoveR:" + result);

			return result;
		}

		public int HideR(int pIndex)
		{
			int result = -1;

			if (this.mIsBusy == true)
				return result;

			if (this.RowCountShown < 1)
				return result;

			if (pIndex >= this.Rows.Count)
				return result;

			if (this.Rows[pIndex].Visible == false)
				return result;

			this.Rows[pIndex].Visible = false;

			for (int i = (pIndex - 1); i >= 0; i--)
			{
				if (this.Rows[i].Visible)
				{
					result = i;
					SelectRow(i);
					break;
				}
			}

			if (result == -1)
			{
				for (int i = (pIndex + 1); i < this.Rows.Count; i++)
				{
					if (this.Rows[i].Visible)
					{
						result = i;
						SelectRow(i);
						break;
					}
				}
			}

			//Debug.WriteLine("DataGridViewEx->HideR:" + result);

			return result;
		}
		
		public int SelectRowFirst()
		{
			for (int i = 0; i < this.Rows.Count; i++)
			{
				if (this.Rows[i].Visible == true)
				{
					SelectRow(i);
					return i;
				}
			}

			return -1;
		}
		
		//
		// 기능 : 입력된 인덱스에 해당하는 행을 선택하고, 해당 행에 대한 정보를 표시
		// 리턴 : 선택되기 전의 Row 인덱스
		//
		public int SelectRow(int pIndex)
		{
			int result = -1;

			if (pIndex < 0)
				return -1;

			if (this.mIsBusy == true)
				return result;

			if (this.RowCountShown < 1)
				return result;

			if (pIndex >= this.Rows.Count)
				return result;

			if (this.Rows[pIndex].Visible == false)
				return result;

			//if (this.SelectedRows.Count < 1)
			//	return result;

			//if (this.SelectedRows[0].Index < 0)
			//	return result;

			result = this.SelectedRows[0].Index;

			if (result != pIndex)
			{
				foreach (DataGridViewCell cell in this.Rows[pIndex].Cells)
				{
					if (cell.Visible)
					{
						cell.Selected = true;
						break;
					}
				}
			}
			else
			{
				if (mRowSelected != null)
					mRowSelected();
			}

			//StoreSelectedRowData();

			return result;
		}


		public event EventHandler MoveRowFinished = null;

		protected virtual void OnMoveRowFinished(object sender, EventArgs e)
		{
			if (MoveRowFinished != null)
				MoveRowFinished(sender, e);
		}

		//
		// 기능 : 행을 이동시킴. 
		// 내용 : pIndex 이동시킬 행의 인덱스
		//        pOffset 이동 간격, 만약 음수면 위쪽으로, 양수이면 아래쪽으로 이동.
		//
		public void MoveRow(int pIndex, int pOffset)
		{
			MoveRow(pIndex, pOffset, 0);
		}

		public void MoveRow(int pIndex, int pOffset, int startIndex)
		{
			int lastIndex = this.Rows.Count - 1;
			int currentIndex = pIndex;
			int offsetIndex = -1;
			int cellCount = this.CurrentRow.Cells.Count;

			offsetIndex = pIndex + pOffset;

			if (offsetIndex >= 0 && offsetIndex <= lastIndex)
			{
				object[] currentRow = new object[cellCount];
				object[] offsetRow = new object[cellCount];
				Color[] currentRowForecolor = new Color[cellCount];
				Color[] currentRowBackcolor = new Color[cellCount];
				Color[] offsetRowForecolor = new Color[cellCount];
				Color[] offsetRowBackcolor = new Color[cellCount];

				for (int i = 0; i < cellCount; i++)
				{
					currentRow[i] = this.Rows[pIndex].Cells[i].Value;
					currentRowForecolor[i] = this.Rows[pIndex].Cells[i].Style.ForeColor;
					currentRowBackcolor[i] = this.Rows[pIndex].Cells[i].Style.BackColor;

					offsetRow[i] = this.Rows[offsetIndex].Cells[i].Value;
					offsetRowForecolor[i] = this.Rows[pIndex].Cells[i].Style.ForeColor;
					offsetRowBackcolor[i] = this.Rows[pIndex].Cells[i].Style.BackColor;

					if (i >= startIndex)
						this.Rows[offsetIndex].Cells[i].Value = currentRow[i];

					this.Rows[offsetIndex].Cells[i].Style.ForeColor = currentRowForecolor[i];
					this.Rows[offsetIndex].Cells[i].Style.BackColor = currentRowBackcolor[i];

					if (i >= startIndex)
						this.Rows[currentIndex].Cells[i].Value = offsetRow[i];

					this.Rows[currentIndex].Cells[i].Style.ForeColor = offsetRowForecolor[i];
					this.Rows[currentIndex].Cells[i].Style.BackColor = offsetRowBackcolor[i]; 
				}

				SelectRow(offsetIndex);

				if (mRowMoved != null)
					mRowMoved();

				OnMoveRowFinished(this, new EventArgs());
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			mBorderRight.Height = this.Height;
			mBorderRight.Location = new Point(this.Width - 1, 0);

			mBorderLeft.Height = this.Height;
			mBorderLeft.Location = new Point(0, 0);

			mBorderTop.Width = this.Width;
			mBorderTop.Location = new Point(0, 0);

			mBorderBottom.Width = this.Width;
			mBorderBottom.Location = new Point(0, this.Height - 1);

			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (needDrawSplitter)
			{
				Rectangle r = new Rectangle(currentMousePos.X, this.ColumnHeadersHeight, 1, this.Height - this.ColumnHeadersHeight - 1);
				using (Pen pen = new Pen(Color.FromArgb(165, 172, 181)))
				{
					e.Graphics.DrawRectangle(pen, r);
				}
			}
		}

		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex == -1 && e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				
				if (mUsingSort)
				{
					if (this.SortedColumn != this.Columns[e.ColumnIndex] && this.Cursor != Cursors.SizeWE)
					{
						if (this.SortedColumn != null)
							this.SortedColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

						this.Columns[e.ColumnIndex].SortMode = DataGridViewColumnSortMode.Automatic;
					}
				}
			}

			base.OnCellMouseDown(e);
		}

		protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
			if (mUsingCellMouseDoubleClickCopyClipboard)
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.Button == System.Windows.Forms.MouseButtons.Left)
				{
					string text = "";

					if (this.Rows[e.RowIndex].Cells[e.ColumnIndex] != null)
						text = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

					if (text.Length > 0)
						Clipboard.SetText(text);
				}
			}

			base.OnCellMouseDoubleClick(e);
		}

		private void ClearSortMode()
		{
			foreach (DataGridViewColumn c in this.Columns)
			{
				if (c.SortMode != DataGridViewColumnSortMode.NotSortable)
					c.SortMode = DataGridViewColumnSortMode.NotSortable;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left && isCursorTypeofSizeWE)
			{
				needDrawSplitter = true;
				currentMousePos = new Point(e.X, e.Y);
			
				this.Invalidate();
			}
			else
			{
				needDrawSplitter = false;
				this.Invalidate();
			}

			mBorderBottom.Invalidate();
		}

		public ColumnType GetColumnType(int pIndex)
		{
			if (pIndex >= this.Columns.Count)
				return ColumnType.None;

			switch (this.Columns[pIndex].CellType.Name)
			{
				case "DataGridViewButtonCell":
					return ColumnType.Button;
				case "DataGridViewCheckBoxCell":
					return ColumnType.CheckBox;
				case "DataGridViewComboBoxCell":
					return ColumnType.ComboBox;
				case "DataGridViewDatePickerCell":
					return ColumnType.DatePicker;
				case "DataGridViewImageCell":
					return ColumnType.Image;
				case "DataGridViewLinkCell":
					return ColumnType.Link;
				case "DataGridViewNumericUpDownCell":
					return ColumnType.NumericUpDown;
				case "DataGridViewTextBoxCell":
					return ColumnType.TextBox;
				default:
					return ColumnType.None;
			}
		}

		public string CopyClipboardColumnWidth()
		{
			StringBuilder sb = new StringBuilder();

			foreach (DataGridViewColumn c in this.Columns)
			{
				sb.AppendLine(c.HeaderText + "\t" + c.Name + "\t" + c.Width.ToString());
			}

			Clipboard.SetText(sb.ToString());

			return sb.ToString();
		}

		private class SetValueClass
		{
			private object mValue = null;
			private int mDelayTime = 100;
			private DataGridViewEx mGrid = null;
			private int mRowIndex = -1;
			private int mColumnIndex = -1;

			public SetValueClass(DataGridViewEx pGrid, int pDelayTime, int pRowIndex, int pColumnIndex, Object pValue)
			{
				mGrid = pGrid;
				mRowIndex = pRowIndex;
				mColumnIndex = pColumnIndex;
				mValue = pValue;
				mDelayTime = pDelayTime;
			}

			public void Worker()
			{
				Thread.Sleep(mDelayTime);
				mGrid.InvokeSetValue(mRowIndex, mColumnIndex, mValue);
			}
		}

		public void SetValueAfterDelay(int pDelayTime, int pRowIndex, int pColumnIndex, object pValue)
		{
			SetValueClass wc = new SetValueClass(this, pDelayTime, pRowIndex, pColumnIndex, pValue);

			Thread t = new Thread(new ThreadStart(wc.Worker));
			t.Start();
		}
	}
}
