namespace NPAutoBooth.UI
{
    partial class FormCreditPaymentMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCreditPaymentMenu));
            this.btnCredTest = new System.Windows.Forms.Button();
            this.tbxTestDIscountValue = new System.Windows.Forms.TextBox();
            this.btnTestDiscount = new System.Windows.Forms.Button();
            this.btnCurrentMoneyTest = new System.Windows.Forms.Button();
            this.txtTestBarcodeDiscount = new System.Windows.Forms.TextBox();
            this.btnBarcodeTestDiscount = new System.Windows.Forms.Button();
            this.timerKisCardPay = new System.Windows.Forms.Timer(this.components);
            this.timerSmartroVCat = new System.Windows.Forms.Timer(this.components);
            this.timer_CardReader2 = new System.Windows.Forms.Timer(this.components);
            this.tmrReadAccount = new System.Windows.Forms.Timer(this.components);
            this.inputTimer = new System.Windows.Forms.Timer(this.components);
            this.MovieTimer = new System.Windows.Forms.Timer(this.components);
            this.btnTest1004Money = new System.Windows.Forms.Button();
            this.txtCardInfo = new System.Windows.Forms.TextBox();
            this.timerBarcode = new System.Windows.Forms.Timer(this.components);
            this.timerKiccTs141State = new System.Windows.Forms.Timer(this.components);
            this.timerKiccSoundPlay = new System.Windows.Forms.Timer(this.components);
            this.timerCardVisible = new System.Windows.Forms.Timer(this.components);
            this.cbGamMyeonItem = new System.Windows.Forms.ComboBox();
            this.btnGamMyunTestDiscount = new System.Windows.Forms.Button();
            this.timerAutoCardReading = new System.Windows.Forms.Timer(this.components);
            this.pic_Wait_MSG_WAIT = new System.Windows.Forms.PictureBox();
            this.groupTest = new System.Windows.Forms.GroupBox();
            this.btnTestBarcodeJehan = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnTestJson = new System.Windows.Forms.Button();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Wait_MSG_WAIT)).BeginInit();
            this.groupTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCredTest
            // 
            this.btnCredTest.Location = new System.Drawing.Point(505, 23);
            this.btnCredTest.Name = "btnCredTest";
            this.btnCredTest.Size = new System.Drawing.Size(77, 40);
            this.btnCredTest.TabIndex = 228;
            this.btnCredTest.Text = "신용카드";
            this.btnCredTest.UseVisualStyleBackColor = true;
            this.btnCredTest.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbxTestDIscountValue
            // 
            this.tbxTestDIscountValue.Location = new System.Drawing.Point(122, 47);
            this.tbxTestDIscountValue.Name = "tbxTestDIscountValue";
            this.tbxTestDIscountValue.Size = new System.Drawing.Size(211, 21);
            this.tbxTestDIscountValue.TabIndex = 233;
            this.tbxTestDIscountValue.Text = "0001011902041000060000012 ";
            // 
            // btnTestDiscount
            // 
            this.btnTestDiscount.Location = new System.Drawing.Point(363, 21);
            this.btnTestDiscount.Name = "btnTestDiscount";
            this.btnTestDiscount.Size = new System.Drawing.Size(45, 34);
            this.btnTestDiscount.TabIndex = 234;
            this.btnTestDiscount.Text = "마그네틱할인권";
            this.btnTestDiscount.UseVisualStyleBackColor = true;
            this.btnTestDiscount.Click += new System.EventHandler(this.btnTestDiscount_Click);
            // 
            // btnCurrentMoneyTest
            // 
            this.btnCurrentMoneyTest.Location = new System.Drawing.Point(422, 7);
            this.btnCurrentMoneyTest.Name = "btnCurrentMoneyTest";
            this.btnCurrentMoneyTest.Size = new System.Drawing.Size(81, 34);
            this.btnCurrentMoneyTest.TabIndex = 236;
            this.btnCurrentMoneyTest.Text = "현금100";
            this.btnCurrentMoneyTest.UseVisualStyleBackColor = true;
            this.btnCurrentMoneyTest.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // txtTestBarcodeDiscount
            // 
            this.txtTestBarcodeDiscount.Location = new System.Drawing.Point(127, 71);
            this.txtTestBarcodeDiscount.Name = "txtTestBarcodeDiscount";
            this.txtTestBarcodeDiscount.Size = new System.Drawing.Size(211, 21);
            this.txtTestBarcodeDiscount.TabIndex = 244;
            this.txtTestBarcodeDiscount.Text = "99160601510000";
            // 
            // btnBarcodeTestDiscount
            // 
            this.btnBarcodeTestDiscount.Location = new System.Drawing.Point(344, 59);
            this.btnBarcodeTestDiscount.Name = "btnBarcodeTestDiscount";
            this.btnBarcodeTestDiscount.Size = new System.Drawing.Size(84, 30);
            this.btnBarcodeTestDiscount.TabIndex = 245;
            this.btnBarcodeTestDiscount.Text = "바코드할인권";
            this.btnBarcodeTestDiscount.UseVisualStyleBackColor = true;
            this.btnBarcodeTestDiscount.Click += new System.EventHandler(this.btnBarcodeTestDiscount_Click);
            // 
            // timerKisCardPay
            // 
            this.timerKisCardPay.Interval = 1000;
            this.timerKisCardPay.Tick += new System.EventHandler(this.timerCardPay_Tick);
            // 
            // timerSmartroVCat
            // 
            this.timerSmartroVCat.Interval = 1000;
            this.timerSmartroVCat.Tick += new System.EventHandler(this.timerSmartroVCat_Tick);
            // 
            // timer_CardReader2
            // 
            this.timer_CardReader2.Interval = 1000;
            this.timer_CardReader2.Tick += new System.EventHandler(this.timer_CardReader2_Tick);
            // 
            // tmrReadAccount
            // 
            this.tmrReadAccount.Interval = 800;
            this.tmrReadAccount.Tick += new System.EventHandler(this.tmrReadAccount_Tick);
            // 
            // inputTimer
            // 
            this.inputTimer.Interval = 3000;
            this.inputTimer.Tick += new System.EventHandler(this.inputTimer_Tick);
            // 
            // MovieTimer
            // 
            this.MovieTimer.Interval = 800;
            this.MovieTimer.Tick += new System.EventHandler(this.MovieTimer_Tick);
            // 
            // btnTest1004Money
            // 
            this.btnTest1004Money.Location = new System.Drawing.Point(499, 18);
            this.btnTest1004Money.Name = "btnTest1004Money";
            this.btnTest1004Money.Size = new System.Drawing.Size(77, 40);
            this.btnTest1004Money.TabIndex = 255;
            this.btnTest1004Money.Text = "강제요금";
            this.btnTest1004Money.UseVisualStyleBackColor = true;
            this.btnTest1004Money.Click += new System.EventHandler(this.btnTest1004Money_Click);
            // 
            // txtCardInfo
            // 
            this.txtCardInfo.Location = new System.Drawing.Point(16, 73);
            this.txtCardInfo.Name = "txtCardInfo";
            this.txtCardInfo.Size = new System.Drawing.Size(89, 21);
            this.txtCardInfo.TabIndex = 256;
            // 
            // timerBarcode
            // 
            this.timerBarcode.Interval = 700;
            this.timerBarcode.Tick += new System.EventHandler(this.timerBarcode_Tick);
            // 
            // timerKiccTs141State
            // 
            this.timerKiccTs141State.Interval = 1000;
            this.timerKiccTs141State.Tick += new System.EventHandler(this.timerKiccTs141State_Tick);
            // 
            // timerKiccSoundPlay
            // 
            this.timerKiccSoundPlay.Interval = 1000;
            this.timerKiccSoundPlay.Tick += new System.EventHandler(this.timerKiccSoundPlay_Tick);
            // 
            // timerCardVisible
            // 
            this.timerCardVisible.Interval = 1000;
            this.timerCardVisible.Tick += new System.EventHandler(this.timerCardVisible_Tick);
            // 
            // cbGamMyeonItem
            // 
            this.cbGamMyeonItem.FormattingEnabled = true;
            this.cbGamMyeonItem.Items.AddRange(new object[] {
            "보훈",
            "장애(1~3급)",
            "장애(4~6급)"});
            this.cbGamMyeonItem.Location = new System.Drawing.Point(27, 48);
            this.cbGamMyeonItem.Name = "cbGamMyeonItem";
            this.cbGamMyeonItem.Size = new System.Drawing.Size(89, 20);
            this.cbGamMyeonItem.TabIndex = 302;
            // 
            // btnGamMyunTestDiscount
            // 
            this.btnGamMyunTestDiscount.Location = new System.Drawing.Point(517, 22);
            this.btnGamMyunTestDiscount.Name = "btnGamMyunTestDiscount";
            this.btnGamMyunTestDiscount.Size = new System.Drawing.Size(39, 32);
            this.btnGamMyunTestDiscount.TabIndex = 303;
            this.btnGamMyunTestDiscount.Text = "감면";
            this.btnGamMyunTestDiscount.UseVisualStyleBackColor = true;
            this.btnGamMyunTestDiscount.Click += new System.EventHandler(this.btnGamMyunTestDiscount_Click);
            // 
            // timerAutoCardReading
            // 
            this.timerAutoCardReading.Interval = 1000;
            this.timerAutoCardReading.Tick += new System.EventHandler(this.timerAutoCardReading_Tick);
            // 
            // pic_Wait_MSG_WAIT
            // 
            this.pic_Wait_MSG_WAIT.Image = global::NPAutoBooth.Properties.Resources.Type2Wait;
            this.pic_Wait_MSG_WAIT.Location = new System.Drawing.Point(156, 145);
            this.pic_Wait_MSG_WAIT.Name = "pic_Wait_MSG_WAIT";
            this.pic_Wait_MSG_WAIT.Size = new System.Drawing.Size(648, 475);
            this.pic_Wait_MSG_WAIT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Wait_MSG_WAIT.TabIndex = 277;
            this.pic_Wait_MSG_WAIT.TabStop = false;
            this.pic_Wait_MSG_WAIT.Tag = "MSG_WAIT";
            this.pic_Wait_MSG_WAIT.Visible = false;
            // 
            // groupTest
            // 
            this.groupTest.Controls.Add(this.btnTestBarcodeJehan);
            this.groupTest.Controls.Add(this.button1);
            this.groupTest.Controls.Add(this.btnTestJson);
            this.groupTest.Controls.Add(this.btnTest1004Money);
            this.groupTest.Controls.Add(this.btnGamMyunTestDiscount);
            this.groupTest.Controls.Add(this.btnCurrentMoneyTest);
            this.groupTest.Controls.Add(this.btnTestDiscount);
            this.groupTest.Controls.Add(this.btnCredTest);
            this.groupTest.Controls.Add(this.btnBarcodeTestDiscount);
            this.groupTest.Controls.Add(this.txtTestBarcodeDiscount);
            this.groupTest.Controls.Add(this.cbGamMyeonItem);
            this.groupTest.Controls.Add(this.txtCardInfo);
            this.groupTest.Controls.Add(this.tbxTestDIscountValue);
            this.groupTest.Location = new System.Drawing.Point(299, 30);
            this.groupTest.Name = "groupTest";
            this.groupTest.Size = new System.Drawing.Size(582, 100);
            this.groupTest.TabIndex = 335;
            this.groupTest.TabStop = false;
            this.groupTest.Text = "groupBox1";
            this.groupTest.Visible = false;
            // 
            // btnTestBarcodeJehan
            // 
            this.btnTestBarcodeJehan.Location = new System.Drawing.Point(434, 59);
            this.btnTestBarcodeJehan.Name = "btnTestBarcodeJehan";
            this.btnTestBarcodeJehan.Size = new System.Drawing.Size(84, 30);
            this.btnTestBarcodeJehan.TabIndex = 306;
            this.btnTestBarcodeJehan.Text = "바코드제한테스트";
            this.btnTestBarcodeJehan.UseVisualStyleBackColor = true;
            this.btnTestBarcodeJehan.Click += new System.EventHandler(this.btnTestBarcodeJehan_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 30);
            this.button1.TabIndex = 305;
            this.button1.Text = "테스트결제Json";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // btnTestJson
            // 
            this.btnTestJson.Location = new System.Drawing.Point(269, 12);
            this.btnTestJson.Name = "btnTestJson";
            this.btnTestJson.Size = new System.Drawing.Size(45, 30);
            this.btnTestJson.TabIndex = 304;
            this.btnTestJson.Text = "카드";
            this.btnTestJson.UseVisualStyleBackColor = true;
            this.btnTestJson.Click += new System.EventHandler(this.btnTestJson_Click);
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(12, 12);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(0, 0);
            this.axWindowsMediaPlayer1.TabIndex = 222;
            this.axWindowsMediaPlayer1.Visible = false;
            // 
            // FormCreditPaymentMenu
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.groupTest);
            this.Controls.Add(this.pic_Wait_MSG_WAIT);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCreditPaymentMenu";
            this.Text = "NPAutoBooth";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormPaymentMenu_FormClosed);
            this.Load += new System.EventHandler(this.FormPaymentMenu_Load);
            this.Shown += new System.EventHandler(this.FormPaymentMenu_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Wait_MSG_WAIT)).EndInit();
            this.groupTest.ResumeLayout(false);
            this.groupTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCredTest;
        private System.Windows.Forms.TextBox tbxTestDIscountValue;
        private System.Windows.Forms.Button btnTestDiscount;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.Button btnCurrentMoneyTest;
        private System.Windows.Forms.TextBox txtTestBarcodeDiscount;
        private System.Windows.Forms.Button btnBarcodeTestDiscount;
        private System.Windows.Forms.Timer timerKisCardPay;
        private System.Windows.Forms.Timer timerSmartroVCat;
        private System.Windows.Forms.Timer timer_CardReader2;
        private System.Windows.Forms.Timer tmrReadAccount;
        private System.Windows.Forms.Timer inputTimer;
        private System.Windows.Forms.Timer MovieTimer;



        private System.Windows.Forms.Button btnTest1004Money;
        private System.Windows.Forms.TextBox txtCardInfo;
        private System.Windows.Forms.Timer timerBarcode;
        private System.Windows.Forms.Timer timerKiccTs141State;
        private System.Windows.Forms.Timer timerKiccSoundPlay;
        private System.Windows.Forms.Timer timerCardVisible;
        private System.Windows.Forms.PictureBox pic_Wait_MSG_WAIT;
        private System.Windows.Forms.ComboBox cbGamMyeonItem;
        private System.Windows.Forms.Button btnGamMyunTestDiscount;
        private System.Windows.Forms.Timer timerAutoCardReading;
        private System.Windows.Forms.GroupBox groupTest;
        private System.Windows.Forms.Button btnTestJson;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnTestBarcodeJehan;
    }
}