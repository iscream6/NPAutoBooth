namespace NPAutoBooth.UI
{
    partial class FormCreditMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditMain));
            this.TimerFormLauncher = new System.Windows.Forms.Timer(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.lbl_status_CardRead = new System.Windows.Forms.Label();
            this.lbl_status_ReciptPrint = new System.Windows.Forms.Label();
            this.timerDeviceStatus = new System.Windows.Forms.Timer(this.components);
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.lblTcpIpName = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lbl_status_Tmoney = new System.Windows.Forms.Label();
            this.lbl_status_CardRead2 = new System.Windows.Forms.Label();
            this.timerCardreaderFrontEject = new System.Windows.Forms.Timer(this.components);
            this.timeSerialKeyFormAction = new System.Windows.Forms.Timer(this.components);
            this.lbl_status_CoinCharge500 = new System.Windows.Forms.Label();
            this.lbl_status_CoinCharge100 = new System.Windows.Forms.Label();
            this.lbl_status_CoinCharge50 = new System.Windows.Forms.Label();
            this.lbl_status_CoinInsert = new System.Windows.Forms.Label();
            this.lbl_status_BillCharge = new System.Windows.Forms.Label();
            this.lbl_status_MoneyInsert = new System.Windows.Forms.Label();
            this.timerPlayer = new System.Windows.Forms.Timer(this.components);
            this.lbl_status_SinbunReader = new System.Windows.Forms.Label();
            this.lbl_status_CoinCharge10 = new System.Windows.Forms.Label();
            this.grupTest = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnTime = new System.Windows.Forms.Button();
            this.btnTestMagam = new System.Windows.Forms.Button();
            this.btnTestPayInfo = new System.Windows.Forms.Button();
            this.btnTestSearchCar = new System.Windows.Forms.Button();
            this.btnTestParkInfo = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.axSmtSndRcvVCAT = new AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT();
            this.axSmartroEvcat = new AxDreampos_Ocx.AxDP_Certification_Ocx();
            this.axKisPosAgent = new AxKisPosAgentLib.AxKisPosAgent();
            this.grupTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSmtSndRcvVCAT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSmartroEvcat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKisPosAgent)).BeginInit();
            this.SuspendLayout();
            // 
            // TimerFormLauncher
            // 
            this.TimerFormLauncher.Interval = 500;
            this.TimerFormLauncher.Tick += new System.EventHandler(this.TimerFormLauncher_Tick);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblVersion.ForeColor = System.Drawing.Color.DarkGray;
            this.lblVersion.Location = new System.Drawing.Point(12, 704);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(62, 15);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "Version";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_status_CardRead
            // 
            this.lbl_status_CardRead.AutoSize = true;
            this.lbl_status_CardRead.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CardRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CardRead.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CardRead.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CardRead.Location = new System.Drawing.Point(202, 9);
            this.lbl_status_CardRead.Name = "lbl_status_CardRead";
            this.lbl_status_CardRead.Size = new System.Drawing.Size(150, 24);
            this.lbl_status_CardRead.TabIndex = 17;
            this.lbl_status_CardRead.Tag = "TXT_CARDREADER1";
            this.lbl_status_CardRead.Text = "CARDREADER1";
            this.lbl_status_CardRead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CardRead.Visible = false;
            // 
            // lbl_status_ReciptPrint
            // 
            this.lbl_status_ReciptPrint.AutoSize = true;
            this.lbl_status_ReciptPrint.BackColor = System.Drawing.Color.Red;
            this.lbl_status_ReciptPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_ReciptPrint.ForeColor = System.Drawing.Color.White;
            this.lbl_status_ReciptPrint.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_ReciptPrint.Location = new System.Drawing.Point(622, 41);
            this.lbl_status_ReciptPrint.Name = "lbl_status_ReciptPrint";
            this.lbl_status_ReciptPrint.Size = new System.Drawing.Size(171, 24);
            this.lbl_status_ReciptPrint.TabIndex = 21;
            this.lbl_status_ReciptPrint.Tag = "TXT_RECEIPTPRINTER";
            this.lbl_status_ReciptPrint.Text = "RECEIPTPRINTER";
            this.lbl_status_ReciptPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_ReciptPrint.Visible = false;
            // 
            // timerDeviceStatus
            // 
            this.timerDeviceStatus.Interval = 3000;
            this.timerDeviceStatus.Tick += new System.EventHandler(this.timerDeviceStatus_Tick);
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.BackColor = System.Drawing.Color.Transparent;
            this.lblDeviceName.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDeviceName.ForeColor = System.Drawing.Color.DarkGray;
            this.lblDeviceName.Location = new System.Drawing.Point(13, 677);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(39, 11);
            this.lblDeviceName.TabIndex = 42;
            this.lblDeviceName.Text = "Name";
            this.lblDeviceName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTcpIpName
            // 
            this.lblTcpIpName.AutoSize = true;
            this.lblTcpIpName.BackColor = System.Drawing.Color.Transparent;
            this.lblTcpIpName.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTcpIpName.ForeColor = System.Drawing.Color.DarkGray;
            this.lblTcpIpName.Location = new System.Drawing.Point(1695, 1000);
            this.lblTcpIpName.Name = "lblTcpIpName";
            this.lblTcpIpName.Size = new System.Drawing.Size(53, 11);
            this.lblTcpIpName.TabIndex = 41;
            this.lblTcpIpName.Text = "Tcp_IP:";
            this.lblTcpIpName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStartDate.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStartDate.ForeColor = System.Drawing.Color.DarkGray;
            this.lblStartDate.Location = new System.Drawing.Point(11, 731);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(115, 15);
            this.lblStartDate.TabIndex = 45;
            this.lblStartDate.Text = "STARTDATE : ";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_status_Tmoney
            // 
            this.lbl_status_Tmoney.AutoSize = true;
            this.lbl_status_Tmoney.BackColor = System.Drawing.Color.Red;
            this.lbl_status_Tmoney.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_Tmoney.ForeColor = System.Drawing.Color.White;
            this.lbl_status_Tmoney.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_Tmoney.Location = new System.Drawing.Point(622, 115);
            this.lbl_status_Tmoney.Name = "lbl_status_Tmoney";
            this.lbl_status_Tmoney.Size = new System.Drawing.Size(70, 24);
            this.lbl_status_Tmoney.TabIndex = 46;
            this.lbl_status_Tmoney.Text = "교통카드";
            this.lbl_status_Tmoney.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_Tmoney.Visible = false;
            // 
            // lbl_status_CardRead2
            // 
            this.lbl_status_CardRead2.AutoSize = true;
            this.lbl_status_CardRead2.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CardRead2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CardRead2.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CardRead2.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CardRead2.Location = new System.Drawing.Point(408, 9);
            this.lbl_status_CardRead2.Name = "lbl_status_CardRead2";
            this.lbl_status_CardRead2.Size = new System.Drawing.Size(150, 24);
            this.lbl_status_CardRead2.TabIndex = 56;
            this.lbl_status_CardRead2.Tag = "TXT_CARDREADER2";
            this.lbl_status_CardRead2.Text = "CARDREADER2";
            this.lbl_status_CardRead2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CardRead2.Visible = false;
            // 
            // timerCardreaderFrontEject
            // 
            this.timerCardreaderFrontEject.Interval = 20000;
            this.timerCardreaderFrontEject.Tick += new System.EventHandler(this.timerCardreaderFrontEject_Tick);
            // 
            // lbl_status_CoinCharge500
            // 
            this.lbl_status_CoinCharge500.AutoSize = true;
            this.lbl_status_CoinCharge500.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CoinCharge500.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CoinCharge500.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CoinCharge500.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CoinCharge500.Location = new System.Drawing.Point(202, 112);
            this.lbl_status_CoinCharge500.Name = "lbl_status_CoinCharge500";
            this.lbl_status_CoinCharge500.Size = new System.Drawing.Size(220, 24);
            this.lbl_status_CoinCharge500.TabIndex = 221;
            this.lbl_status_CoinCharge500.Tag = "TXT_COINDISPENSER4";
            this.lbl_status_CoinCharge500.Text = "TXT_COINDISPENSER4";
            this.lbl_status_CoinCharge500.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CoinCharge500.Visible = false;
            // 
            // lbl_status_CoinCharge100
            // 
            this.lbl_status_CoinCharge100.AutoSize = true;
            this.lbl_status_CoinCharge100.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CoinCharge100.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CoinCharge100.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CoinCharge100.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CoinCharge100.Location = new System.Drawing.Point(622, 78);
            this.lbl_status_CoinCharge100.Name = "lbl_status_CoinCharge100";
            this.lbl_status_CoinCharge100.Size = new System.Drawing.Size(220, 24);
            this.lbl_status_CoinCharge100.TabIndex = 220;
            this.lbl_status_CoinCharge100.Tag = "TXT_COINDISPENSER3";
            this.lbl_status_CoinCharge100.Text = "TXT_COINDISPENSER3";
            this.lbl_status_CoinCharge100.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CoinCharge100.Visible = false;
            // 
            // lbl_status_CoinCharge50
            // 
            this.lbl_status_CoinCharge50.AutoSize = true;
            this.lbl_status_CoinCharge50.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CoinCharge50.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CoinCharge50.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CoinCharge50.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CoinCharge50.Location = new System.Drawing.Point(408, 78);
            this.lbl_status_CoinCharge50.Name = "lbl_status_CoinCharge50";
            this.lbl_status_CoinCharge50.Size = new System.Drawing.Size(220, 24);
            this.lbl_status_CoinCharge50.TabIndex = 219;
            this.lbl_status_CoinCharge50.Tag = "TXT_COINDISPENSER2";
            this.lbl_status_CoinCharge50.Text = "TXT_COINDISPENSER2";
            this.lbl_status_CoinCharge50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CoinCharge50.Visible = false;
            // 
            // lbl_status_CoinInsert
            // 
            this.lbl_status_CoinInsert.AutoSize = true;
            this.lbl_status_CoinInsert.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CoinInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CoinInsert.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CoinInsert.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CoinInsert.Location = new System.Drawing.Point(622, 9);
            this.lbl_status_CoinInsert.Name = "lbl_status_CoinInsert";
            this.lbl_status_CoinInsert.Size = new System.Drawing.Size(134, 24);
            this.lbl_status_CoinInsert.TabIndex = 218;
            this.lbl_status_CoinInsert.Tag = "TXT_CIONREADER";
            this.lbl_status_CoinInsert.Text = "CIONREADER";
            this.lbl_status_CoinInsert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CoinInsert.Visible = false;
            // 
            // lbl_status_BillCharge
            // 
            this.lbl_status_BillCharge.AutoSize = true;
            this.lbl_status_BillCharge.BackColor = System.Drawing.Color.Red;
            this.lbl_status_BillCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_BillCharge.ForeColor = System.Drawing.Color.White;
            this.lbl_status_BillCharge.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_BillCharge.Location = new System.Drawing.Point(408, 41);
            this.lbl_status_BillCharge.Name = "lbl_status_BillCharge";
            this.lbl_status_BillCharge.Size = new System.Drawing.Size(152, 24);
            this.lbl_status_BillCharge.TabIndex = 217;
            this.lbl_status_BillCharge.Tag = "TXT_BILLDISPENSER";
            this.lbl_status_BillCharge.Text = "BILLDISPENSER";
            this.lbl_status_BillCharge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_BillCharge.Visible = false;
            // 
            // lbl_status_MoneyInsert
            // 
            this.lbl_status_MoneyInsert.AutoSize = true;
            this.lbl_status_MoneyInsert.BackColor = System.Drawing.Color.Red;
            this.lbl_status_MoneyInsert.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_MoneyInsert.ForeColor = System.Drawing.Color.White;
            this.lbl_status_MoneyInsert.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_MoneyInsert.Location = new System.Drawing.Point(202, 41);
            this.lbl_status_MoneyInsert.Name = "lbl_status_MoneyInsert";
            this.lbl_status_MoneyInsert.Size = new System.Drawing.Size(124, 24);
            this.lbl_status_MoneyInsert.TabIndex = 216;
            this.lbl_status_MoneyInsert.Tag = "TXT_BILLREADER";
            this.lbl_status_MoneyInsert.Text = "BILLREADER";
            this.lbl_status_MoneyInsert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_MoneyInsert.Visible = false;
            // 
            // timerPlayer
            // 
            this.timerPlayer.Interval = 2000;
            // 
            // lbl_status_SinbunReader
            // 
            this.lbl_status_SinbunReader.AutoSize = true;
            this.lbl_status_SinbunReader.BackColor = System.Drawing.Color.Red;
            this.lbl_status_SinbunReader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_SinbunReader.ForeColor = System.Drawing.Color.White;
            this.lbl_status_SinbunReader.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_SinbunReader.Location = new System.Drawing.Point(408, 115);
            this.lbl_status_SinbunReader.Name = "lbl_status_SinbunReader";
            this.lbl_status_SinbunReader.Size = new System.Drawing.Size(153, 24);
            this.lbl_status_SinbunReader.TabIndex = 229;
            this.lbl_status_SinbunReader.Tag = "TXT_IDREADER";
            this.lbl_status_SinbunReader.Text = "TXT_IDREADER";
            this.lbl_status_SinbunReader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_SinbunReader.Visible = false;
            // 
            // lbl_status_CoinCharge10
            // 
            this.lbl_status_CoinCharge10.AutoSize = true;
            this.lbl_status_CoinCharge10.BackColor = System.Drawing.Color.Red;
            this.lbl_status_CoinCharge10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_status_CoinCharge10.ForeColor = System.Drawing.Color.White;
            this.lbl_status_CoinCharge10.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbl_status_CoinCharge10.Location = new System.Drawing.Point(202, 78);
            this.lbl_status_CoinCharge10.Name = "lbl_status_CoinCharge10";
            this.lbl_status_CoinCharge10.Size = new System.Drawing.Size(90, 24);
            this.lbl_status_CoinCharge10.TabIndex = 324;
            this.lbl_status_CoinCharge10.Tag = "TXT_COINDISPENSER1";
            this.lbl_status_CoinCharge10.Text = "동전방출10";
            this.lbl_status_CoinCharge10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_status_CoinCharge10.Visible = false;
            // 
            // grupTest
            // 
            this.grupTest.Controls.Add(this.button2);
            this.grupTest.Controls.Add(this.button1);
            this.grupTest.Controls.Add(this.btnTime);
            this.grupTest.Controls.Add(this.btnTestMagam);
            this.grupTest.Controls.Add(this.btnTestPayInfo);
            this.grupTest.Controls.Add(this.btnTestSearchCar);
            this.grupTest.Controls.Add(this.btnTestParkInfo);
            this.grupTest.Location = new System.Drawing.Point(59, 338);
            this.grupTest.Name = "grupTest";
            this.grupTest.Size = new System.Drawing.Size(280, 178);
            this.grupTest.TabIndex = 325;
            this.grupTest.TabStop = false;
            this.grupTest.Text = "groupBox1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(199, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 328;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(122, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 327;
            this.button1.Text = "checkALarm";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnTime
            // 
            this.btnTime.Location = new System.Drawing.Point(25, 121);
            this.btnTime.Name = "btnTime";
            this.btnTime.Size = new System.Drawing.Size(75, 23);
            this.btnTime.TabIndex = 4;
            this.btnTime.Text = "button2";
            this.btnTime.UseVisualStyleBackColor = true;
            this.btnTime.Click += new System.EventHandler(this.btnTime_Click);
            // 
            // btnTestMagam
            // 
            this.btnTestMagam.Location = new System.Drawing.Point(122, 78);
            this.btnTestMagam.Name = "btnTestMagam";
            this.btnTestMagam.Size = new System.Drawing.Size(75, 23);
            this.btnTestMagam.TabIndex = 3;
            this.btnTestMagam.Text = "btnTestMagam";
            this.btnTestMagam.UseVisualStyleBackColor = true;
            this.btnTestMagam.Click += new System.EventHandler(this.btnTestMagam_Click);
            // 
            // btnTestPayInfo
            // 
            this.btnTestPayInfo.Location = new System.Drawing.Point(15, 78);
            this.btnTestPayInfo.Name = "btnTestPayInfo";
            this.btnTestPayInfo.Size = new System.Drawing.Size(75, 23);
            this.btnTestPayInfo.TabIndex = 2;
            this.btnTestPayInfo.Text = "테스트요금조회";
            this.btnTestPayInfo.UseVisualStyleBackColor = true;
            this.btnTestPayInfo.Click += new System.EventHandler(this.btnTestPayInfo_Click);
            // 
            // btnTestSearchCar
            // 
            this.btnTestSearchCar.Location = new System.Drawing.Point(122, 38);
            this.btnTestSearchCar.Name = "btnTestSearchCar";
            this.btnTestSearchCar.Size = new System.Drawing.Size(75, 23);
            this.btnTestSearchCar.TabIndex = 1;
            this.btnTestSearchCar.Text = "btnTestSearchCar";
            this.btnTestSearchCar.UseVisualStyleBackColor = true;
            this.btnTestSearchCar.Click += new System.EventHandler(this.btnTestSearchCar_Click);
            // 
            // btnTestParkInfo
            // 
            this.btnTestParkInfo.Location = new System.Drawing.Point(25, 38);
            this.btnTestParkInfo.Name = "btnTestParkInfo";
            this.btnTestParkInfo.Size = new System.Drawing.Size(75, 23);
            this.btnTestParkInfo.TabIndex = 0;
            this.btnTestParkInfo.Text = "btnParkinfo";
            this.btnTestParkInfo.UseVisualStyleBackColor = true;
            this.btnTestParkInfo.Click += new System.EventHandler(this.btnTestParkInfo_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(920, 526);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(33, 19);
            this.axWindowsMediaPlayer1.TabIndex = 227;
            // 
            // axSmtSndRcvVCAT
            // 
            this.axSmtSndRcvVCAT.Enabled = true;
            this.axSmtSndRcvVCAT.Location = new System.Drawing.Point(719, 550);
            this.axSmtSndRcvVCAT.Name = "axSmtSndRcvVCAT";
            this.axSmtSndRcvVCAT.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSmtSndRcvVCAT.OcxState")));
            this.axSmtSndRcvVCAT.Size = new System.Drawing.Size(100, 50);
            this.axSmtSndRcvVCAT.TabIndex = 215;
            // 
            // axSmartroEvcat
            // 
            this.axSmartroEvcat.Enabled = true;
            this.axSmartroEvcat.Location = new System.Drawing.Point(868, 393);
            this.axSmartroEvcat.Name = "axSmartroEvcat";
            this.axSmartroEvcat.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axSmartroEvcat.OcxState")));
            this.axSmartroEvcat.Size = new System.Drawing.Size(109, 30);
            this.axSmartroEvcat.TabIndex = 327;
            // 
            // axKisPosAgent
            // 
            this.axKisPosAgent.Enabled = true;
            this.axKisPosAgent.Location = new System.Drawing.Point(853, 317);
            this.axKisPosAgent.Name = "axKisPosAgent";
            this.axKisPosAgent.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axKisPosAgent.OcxState")));
            this.axKisPosAgent.Size = new System.Drawing.Size(100, 50);
            this.axKisPosAgent.TabIndex = 335;
            // 
            // FormCreditMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.axKisPosAgent);
            this.Controls.Add(this.axSmartroEvcat);
            this.Controls.Add(this.grupTest);
            this.Controls.Add(this.lbl_status_CoinCharge10);
            this.Controls.Add(this.lbl_status_SinbunReader);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Controls.Add(this.lbl_status_CoinCharge500);
            this.Controls.Add(this.lbl_status_CoinCharge100);
            this.Controls.Add(this.lbl_status_CoinCharge50);
            this.Controls.Add(this.lbl_status_CoinInsert);
            this.Controls.Add(this.lbl_status_BillCharge);
            this.Controls.Add(this.lbl_status_MoneyInsert);
            this.Controls.Add(this.axSmtSndRcvVCAT);
            this.Controls.Add(this.lbl_status_CardRead2);
            this.Controls.Add(this.lbl_status_Tmoney);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.lblDeviceName);
            this.Controls.Add(this.lblTcpIpName);
            this.Controls.Add(this.lbl_status_ReciptPrint);
            this.Controls.Add(this.lbl_status_CardRead);
            this.Controls.Add(this.lblVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCreditMain";
            this.Text = "NPAutoBooth";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.grupTest.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSmtSndRcvVCAT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axSmartroEvcat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axKisPosAgent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer TimerFormLauncher;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lbl_status_CardRead;
        private System.Windows.Forms.Label lbl_status_ReciptPrint;
        private System.Windows.Forms.Timer timerDeviceStatus;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.Label lblTcpIpName;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lbl_status_Tmoney;
        private System.Windows.Forms.Label lbl_status_CardRead2;
        private System.Windows.Forms.Timer timerCardreaderFrontEject;
        private AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT axSmtSndRcvVCAT;
        private System.Windows.Forms.Timer timeSerialKeyFormAction;
        private System.Windows.Forms.Label lbl_status_CoinCharge500;
        private System.Windows.Forms.Label lbl_status_CoinCharge100;
        private System.Windows.Forms.Label lbl_status_CoinCharge50;
        private System.Windows.Forms.Label lbl_status_CoinInsert;
        private System.Windows.Forms.Label lbl_status_BillCharge;
        private System.Windows.Forms.Label lbl_status_MoneyInsert;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Timer timerPlayer;
        private System.Windows.Forms.Label lbl_status_SinbunReader;
        private System.Windows.Forms.Label lbl_status_CoinCharge10;
        private System.Windows.Forms.GroupBox grupTest;
        private System.Windows.Forms.Button btnTestParkInfo;
        private System.Windows.Forms.Button btnTestSearchCar;
        private System.Windows.Forms.Button btnTestPayInfo;
        private System.Windows.Forms.Button btnTestMagam;
        private System.Windows.Forms.Button btnTime;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private AxDreampos_Ocx.AxDP_Certification_Ocx axSmartroEvcat;
        private AxKisPosAgentLib.AxKisPosAgent axKisPosAgent;
    }
}