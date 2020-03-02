namespace NPConfig
{
	partial class SerialPortConnectionSetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new FadeFox.UI.GlassButton();
            this.pnlInput = new FadeFox.UI.SimplePanel();
            this.txtPortName = new FadeFox.UI.CodeSearchTextBox();
            this.cboHandshake = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.cboDataBits = new System.Windows.Forms.ComboBox();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.chkUseYN = new System.Windows.Forms.CheckBox();
            this.txtSerialPortComment = new System.Windows.Forms.TextBox();
            this.txtSerialPortName = new System.Windows.Forms.TextBox();
            this.txtSerialPortID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblUseYN = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCancel = new FadeFox.UI.GlassButton();
            this.btnSave = new FadeFox.UI.GlassButton();
            this.grdList = new FadeFox.UI.DataGridViewEx();
            this.USE_YN = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.SERIALPORT_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERIALPORT_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PORTNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BAUDRATE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATABITS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PARITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STOPBITS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HANDSHAKE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SERIALPORT_COMMENT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlControl = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDelete = new FadeFox.UI.GlassButton();
            this.btnUpdate = new FadeFox.UI.GlassButton();
            this.btnInsert = new FadeFox.UI.GlassButton();
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
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.BackColor = System.Drawing.Color.DarkGray;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.EnableFadeInOut = true;
            this.btnClose.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.GlowColor = System.Drawing.Color.White;
            this.btnClose.InnerBorderColor = System.Drawing.Color.DimGray;
            this.btnClose.Location = new System.Drawing.Point(527, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 24);
            this.btnClose.TabIndex = 6;
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
            this.pnlInput.Controls.Add(this.txtPortName);
            this.pnlInput.Controls.Add(this.cboHandshake);
            this.pnlInput.Controls.Add(this.label10);
            this.pnlInput.Controls.Add(this.label11);
            this.pnlInput.Controls.Add(this.label12);
            this.pnlInput.Controls.Add(this.label13);
            this.pnlInput.Controls.Add(this.label14);
            this.pnlInput.Controls.Add(this.cboStopBits);
            this.pnlInput.Controls.Add(this.cboDataBits);
            this.pnlInput.Controls.Add(this.cboParity);
            this.pnlInput.Controls.Add(this.cboBaudRate);
            this.pnlInput.Controls.Add(this.chkUseYN);
            this.pnlInput.Controls.Add(this.txtSerialPortComment);
            this.pnlInput.Controls.Add(this.txtSerialPortName);
            this.pnlInput.Controls.Add(this.txtSerialPortID);
            this.pnlInput.Controls.Add(this.label9);
            this.pnlInput.Controls.Add(this.lblUseYN);
            this.pnlInput.Controls.Add(this.label7);
            this.pnlInput.Controls.Add(this.label6);
            this.pnlInput.Controls.Add(this.label8);
            this.pnlInput.Location = new System.Drawing.Point(0, 380);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(898, 64);
            this.pnlInput.TabIndex = 0;
            // 
            // txtPortName
            // 
            this.txtPortName.AllowInputCode = false;
            this.txtPortName.AutoCodeName = false;
            this.txtPortName.BackColor = System.Drawing.SystemColors.Control;
            this.txtPortName.ClearButtonVisible = true;
            this.txtPortName.Code = "";
            this.txtPortName.CodeAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPortName.CodeMaxLength = 4;
            this.txtPortName.CodeName = "";
            this.txtPortName.CodeNameVisible = false;
            this.txtPortName.CodeWidth = 55;
            this.txtPortName.Location = new System.Drawing.Point(62, 34);
            this.txtPortName.Name = "txtPortName";
            this.txtPortName.OpenSearchFormAction = null;
            this.txtPortName.Size = new System.Drawing.Size(102, 22);
            this.txtPortName.TabIndex = 3;
            // 
            // cboHandshake
            // 
            this.cboHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHandshake.FormattingEnabled = true;
            this.cboHandshake.Items.AddRange(new object[] {
            "None",
            "Xon/Xoff",
            "Rts",
            "Both"});
            this.cboHandshake.Location = new System.Drawing.Point(790, 34);
            this.cboHandshake.Name = "cboHandshake";
            this.cboHandshake.Size = new System.Drawing.Size(98, 20);
            this.cboHandshake.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(728, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 71;
            this.label10.Text = "흐름제어:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(600, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 70;
            this.label11.Text = "중단비트:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(458, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 12);
            this.label12.TabIndex = 69;
            this.label12.Text = "데이터비트:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(328, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 12);
            this.label13.TabIndex = 68;
            this.label13.Text = "패리티:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(180, 38);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 67;
            this.label14.Text = "전송속도:";
            // 
            // cboStopBits
            // 
            this.cboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Items.AddRange(new object[] {
            "1",
            "2"});
            this.cboStopBits.Location = new System.Drawing.Point(662, 34);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(58, 20);
            this.cboStopBits.TabIndex = 7;
            // 
            // cboDataBits
            // 
            this.cboDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataBits.FormattingEnabled = true;
            this.cboDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6",
            "5"});
            this.cboDataBits.Location = new System.Drawing.Point(530, 34);
            this.cboDataBits.Name = "cboDataBits";
            this.cboDataBits.Size = new System.Drawing.Size(60, 20);
            this.cboDataBits.TabIndex = 6;
            // 
            // cboParity
            // 
            this.cboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.cboParity.Location = new System.Drawing.Point(376, 34);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(76, 20);
            this.cboParity.TabIndex = 5;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Items.AddRange(new object[] {
            "115200",
            "57600",
            "38400",
            "19200",
            "9600",
            "4800",
            "2400"});
            this.cboBaudRate.Location = new System.Drawing.Point(240, 34);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(78, 20);
            this.cboBaudRate.TabIndex = 4;
            // 
            // chkUseYN
            // 
            this.chkUseYN.AutoSize = true;
            this.chkUseYN.Location = new System.Drawing.Point(790, 14);
            this.chkUseYN.Name = "chkUseYN";
            this.chkUseYN.Size = new System.Drawing.Size(15, 14);
            this.chkUseYN.TabIndex = 9;
            this.chkUseYN.TabStop = false;
            this.chkUseYN.UseVisualStyleBackColor = true;
            // 
            // txtSerialPortComment
            // 
            this.txtSerialPortComment.Location = new System.Drawing.Point(530, 8);
            this.txtSerialPortComment.Name = "txtSerialPortComment";
            this.txtSerialPortComment.Size = new System.Drawing.Size(190, 21);
            this.txtSerialPortComment.TabIndex = 2;
            // 
            // txtSerialPortName
            // 
            this.txtSerialPortName.Location = new System.Drawing.Point(364, 8);
            this.txtSerialPortName.Name = "txtSerialPortName";
            this.txtSerialPortName.Size = new System.Drawing.Size(124, 21);
            this.txtSerialPortName.TabIndex = 1;
            // 
            // txtSerialPortID
            // 
            this.txtSerialPortID.Location = new System.Drawing.Point(124, 8);
            this.txtSerialPortID.Name = "txtSerialPortID";
            this.txtSerialPortID.Size = new System.Drawing.Size(146, 21);
            this.txtSerialPortID.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 12);
            this.label9.TabIndex = 59;
            this.label9.Text = "포트 명:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUseYN
            // 
            this.lblUseYN.AutoSize = true;
            this.lblUseYN.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseYN.Location = new System.Drawing.Point(728, 14);
            this.lblUseYN.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.lblUseYN.Name = "lblUseYN";
            this.lblUseYN.Size = new System.Drawing.Size(59, 12);
            this.lblUseYN.TabIndex = 54;
            this.lblUseYN.Text = "사용여부:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(10, 14);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 12);
            this.label7.TabIndex = 45;
            this.label7.Text = "시리얼포트 아이디:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(274, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 46;
            this.label6.Text = "시리얼포트 명:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(492, 12);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 5, 0, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 44;
            this.label8.Text = "설명:";
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
            this.btnCancel.Location = new System.Drawing.Point(175, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 24);
            this.btnCancel.TabIndex = 2;
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
            this.btnSave.Location = new System.Drawing.Point(87, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 24);
            this.btnSave.TabIndex = 1;
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
            this.USE_YN,
            this.SERIALPORT_ID,
            this.SERIALPORT_NAME,
            this.PORTNAME,
            this.BAUDRATE,
            this.DATABITS,
            this.PARITY,
            this.STOPBITS,
            this.HANDSHAKE,
            this.SERIALPORT_COMMENT});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("돋움체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
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
            this.grdList.Size = new System.Drawing.Size(888, 326);
            this.grdList.StoredRowForeColor = System.Drawing.Color.Red;
            this.grdList.TabIndex = 0;
            this.grdList.UsingCellMouseDoubleClickCopyClipboard = true;
            this.grdList.UsingCurrentCellDirtyAutoCommit = true;
            this.grdList.UsingExport = true;
            this.grdList.UsingLostFocusSelectionColor = false;
            this.grdList.UsingSort = false;
            // 
            // USE_YN
            // 
            this.USE_YN.FalseValue = "N";
            this.USE_YN.FillWeight = 50F;
            this.USE_YN.HeaderText = "사용";
            this.USE_YN.Name = "USE_YN";
            this.USE_YN.ReadOnly = true;
            this.USE_YN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.USE_YN.TrueValue = "Y";
            this.USE_YN.Width = 50;
            // 
            // SERIALPORT_ID
            // 
            this.SERIALPORT_ID.FillWeight = 130F;
            this.SERIALPORT_ID.HeaderText = "시리얼포트 아이디";
            this.SERIALPORT_ID.MaxInputLength = 128;
            this.SERIALPORT_ID.Name = "SERIALPORT_ID";
            this.SERIALPORT_ID.ReadOnly = true;
            this.SERIALPORT_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERIALPORT_ID.Width = 130;
            // 
            // SERIALPORT_NAME
            // 
            dataGridViewCellStyle2.NullValue = null;
            this.SERIALPORT_NAME.DefaultCellStyle = dataGridViewCellStyle2;
            this.SERIALPORT_NAME.FillWeight = 130F;
            this.SERIALPORT_NAME.HeaderText = "시리얼포트 명";
            this.SERIALPORT_NAME.MaxInputLength = 128;
            this.SERIALPORT_NAME.Name = "SERIALPORT_NAME";
            this.SERIALPORT_NAME.ReadOnly = true;
            this.SERIALPORT_NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERIALPORT_NAME.Width = 130;
            // 
            // PORTNAME
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PORTNAME.DefaultCellStyle = dataGridViewCellStyle3;
            this.PORTNAME.FillWeight = 75F;
            this.PORTNAME.HeaderText = "포트 명";
            this.PORTNAME.Name = "PORTNAME";
            this.PORTNAME.ReadOnly = true;
            this.PORTNAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PORTNAME.Width = 75;
            // 
            // BAUDRATE
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.BAUDRATE.DefaultCellStyle = dataGridViewCellStyle4;
            this.BAUDRATE.FillWeight = 80F;
            this.BAUDRATE.HeaderText = "전송속도";
            this.BAUDRATE.Name = "BAUDRATE";
            this.BAUDRATE.ReadOnly = true;
            this.BAUDRATE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.BAUDRATE.Width = 80;
            // 
            // DATABITS
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.DATABITS.DefaultCellStyle = dataGridViewCellStyle5;
            this.DATABITS.FillWeight = 90F;
            this.DATABITS.HeaderText = "데이터비트";
            this.DATABITS.Name = "DATABITS";
            this.DATABITS.ReadOnly = true;
            this.DATABITS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DATABITS.Width = 90;
            // 
            // PARITY
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.PARITY.DefaultCellStyle = dataGridViewCellStyle6;
            this.PARITY.FillWeight = 60F;
            this.PARITY.HeaderText = "패리티";
            this.PARITY.Name = "PARITY";
            this.PARITY.ReadOnly = true;
            this.PARITY.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PARITY.Width = 60;
            // 
            // STOPBITS
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.STOPBITS.DefaultCellStyle = dataGridViewCellStyle7;
            this.STOPBITS.FillWeight = 70F;
            this.STOPBITS.HeaderText = "중단비트";
            this.STOPBITS.Name = "STOPBITS";
            this.STOPBITS.ReadOnly = true;
            this.STOPBITS.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.STOPBITS.Width = 70;
            // 
            // HANDSHAKE
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.HANDSHAKE.DefaultCellStyle = dataGridViewCellStyle8;
            this.HANDSHAKE.FillWeight = 80F;
            this.HANDSHAKE.HeaderText = "흐름제어";
            this.HANDSHAKE.Name = "HANDSHAKE";
            this.HANDSHAKE.ReadOnly = true;
            this.HANDSHAKE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.HANDSHAKE.Width = 80;
            // 
            // SERIALPORT_COMMENT
            // 
            this.SERIALPORT_COMMENT.FillWeight = 300F;
            this.SERIALPORT_COMMENT.HeaderText = "시리얼포트 설명";
            this.SERIALPORT_COMMENT.Name = "SERIALPORT_COMMENT";
            this.SERIALPORT_COMMENT.ReadOnly = true;
            this.SERIALPORT_COMMENT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SERIALPORT_COMMENT.Width = 300;
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
            this.pnlControl.Location = new System.Drawing.Point(278, 4);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(615, 30);
            this.pnlControl.TabIndex = 0;
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
            this.btnDelete.Location = new System.Drawing.Point(439, 3);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 24);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "삭제";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.btnUpdate.Location = new System.Drawing.Point(351, 3);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(86, 24);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "수정";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            this.btnInsert.Location = new System.Drawing.Point(263, 3);
            this.btnInsert.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(86, 24);
            this.btnInsert.TabIndex = 3;
            this.btnInsert.Text = "추가";
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // chkContinueInsertMode
            // 
            this.chkContinueInsertMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkContinueInsertMode.AutoSize = true;
            this.chkContinueInsertMode.Location = new System.Drawing.Point(13, 7);
            this.chkContinueInsertMode.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            this.chkContinueInsertMode.Name = "chkContinueInsertMode";
            this.chkContinueInsertMode.Size = new System.Drawing.Size(72, 16);
            this.chkContinueInsertMode.TabIndex = 0;
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
            this.pnlLayoutBottom.Location = new System.Drawing.Point(0, 444);
            this.pnlLayoutBottom.Name = "pnlLayoutBottom";
            this.pnlLayoutBottom.Size = new System.Drawing.Size(898, 37);
            this.pnlLayoutBottom.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "건수:";
            // 
            // lblRowCount
            // 
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(44, 14);
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
            this.pnlContent.Size = new System.Drawing.Size(898, 336);
            this.pnlContent.TabIndex = 71;
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
            this.pnlLayoutTop.TabIndex = 73;
            // 
            // pnlCondition
            // 
            this.pnlCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCondition.BackColor = System.Drawing.Color.Transparent;
            this.pnlCondition.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlCondition.Location = new System.Drawing.Point(424, 8);
            this.pnlCondition.Name = "pnlCondition";
            this.pnlCondition.Size = new System.Drawing.Size(470, 26);
            this.pnlCondition.TabIndex = 66;
            // 
            // lblSubject
            // 
            this.lblSubject.BackColor = System.Drawing.Color.Transparent;
            this.lblSubject.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSubject.Location = new System.Drawing.Point(10, 10);
            this.lblSubject.Margin = new System.Windows.Forms.Padding(4);
            this.lblSubject.Name = "lblSubject";
            this.lblSubject.Size = new System.Drawing.Size(228, 24);
            this.lblSubject.TabIndex = 70;
            this.lblSubject.Text = "시리얼포트 연결관리";
            this.lblSubject.Text3DColor = System.Drawing.Color.Gainsboro;
            this.lblSubject.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.lblSubject.TextColor = System.Drawing.Color.Black;
            this.lblSubject.TextShadowColor = System.Drawing.Color.Transparent;
            this.lblSubject.TextTopMargin = 2;
            // 
            // SerialPortConnectionSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(898, 481);
            this.Controls.Add(this.pnlLayoutTop);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlLayoutBottom);
            this.Controls.Add(this.pnlInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialPortConnectionSetting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialPortConnectionSetting_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SerialPortConnectionSetting_FormClosed);
            this.Load += new System.EventHandler(this.SerialPortConnectionSetting_Load);
            this.Shown += new System.EventHandler(this.SerialPortConnectionSetting_Shown);
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
		private System.Windows.Forms.Label label8;
		private FadeFox.UI.SimplePanel pnlLayoutBottom;
		private FadeFox.UI.GlassButton btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRowCount;
		private System.Windows.Forms.Label lblUseYN;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtSerialPortName;
		private System.Windows.Forms.TextBox txtSerialPortID;
		private System.Windows.Forms.CheckBox chkUseYN;
		private System.Windows.Forms.CheckBox chkContinueInsertMode;
		private FadeFox.UI.BorderPanelEx pnlContent;
		private FadeFox.UI.GradientPanelEx pnlLayoutTop;
		private System.Windows.Forms.FlowLayoutPanel pnlCondition;
		private FadeFox.UI.Text3DLabel lblSubject;
		internal System.Windows.Forms.ComboBox cboHandshake;
		internal System.Windows.Forms.Label label10;
		internal System.Windows.Forms.Label label11;
		internal System.Windows.Forms.Label label12;
		internal System.Windows.Forms.Label label13;
		internal System.Windows.Forms.Label label14;
		internal System.Windows.Forms.ComboBox cboStopBits;
		internal System.Windows.Forms.ComboBox cboDataBits;
		internal System.Windows.Forms.ComboBox cboParity;
		internal System.Windows.Forms.ComboBox cboBaudRate;
		private System.Windows.Forms.TextBox txtSerialPortComment;
		private FadeFox.UI.CodeSearchTextBox txtPortName;
		private System.Windows.Forms.DataGridViewCheckBoxColumn USE_YN;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERIALPORT_ID;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERIALPORT_NAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn PORTNAME;
		private System.Windows.Forms.DataGridViewTextBoxColumn BAUDRATE;
		private System.Windows.Forms.DataGridViewTextBoxColumn DATABITS;
		private System.Windows.Forms.DataGridViewTextBoxColumn PARITY;
		private System.Windows.Forms.DataGridViewTextBoxColumn STOPBITS;
		private System.Windows.Forms.DataGridViewTextBoxColumn HANDSHAKE;
		private System.Windows.Forms.DataGridViewTextBoxColumn SERIALPORT_COMMENT;
	}
}