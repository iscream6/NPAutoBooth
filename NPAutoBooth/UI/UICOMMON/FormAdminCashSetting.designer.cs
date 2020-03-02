namespace NPAutoBooth.UI
{
    partial class FormAdminCashSetting
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
            this.components = new System.ComponentModel.Container();
            this.lblMoney_2TypeName = new System.Windows.Forms.Label();
            this.lbl_TXT_CLOSESETTION = new System.Windows.Forms.Label();
            this.lbl_TXT_CUR_AMOUNT = new System.Windows.Forms.Label();
            this.lbl_TXT_MIN_AMOUNT = new System.Windows.Forms.Label();
            this.lblMoney_3TypeName = new System.Windows.Forms.Label();
            this.lblMoney_4TypeName = new System.Windows.Forms.Label();
            this.lblMoney_5TypeName = new System.Windows.Forms.Label();
            this.lblMoney_6TypeName = new System.Windows.Forms.Label();
            this.btn_out50 = new System.Windows.Forms.Button();
            this.btn_out100 = new System.Windows.Forms.Button();
            this.btn_out500 = new System.Windows.Forms.Button();
            this.btn_out1000 = new System.Windows.Forms.Button();
            this.btn_out5000 = new System.Windows.Forms.Button();
            this.tmrHome = new System.Windows.Forms.Timer(this.components);
            this.lbl50MinQty = new FadeFox.UI.SimpleLabel();
            this.lbl50SettingQty = new FadeFox.UI.SimpleLabel();
            this.lbl5000MinQty = new FadeFox.UI.SimpleLabel();
            this.lbl5000SettingQty = new FadeFox.UI.SimpleLabel();
            this.lbl1000MinQty = new FadeFox.UI.SimpleLabel();
            this.lbl1000SettingQty = new FadeFox.UI.SimpleLabel();
            this.lbl500MinQty = new FadeFox.UI.SimpleLabel();
            this.lbl500SettingQty = new FadeFox.UI.SimpleLabel();
            this.lbl100MinQty = new FadeFox.UI.SimpleLabel();
            this.lbl100SettingQty = new FadeFox.UI.SimpleLabel();
            this.lbl_TXT_CLOSING = new System.Windows.Forms.Label();
            this.lbl_TXT_CLOSEINFO = new System.Windows.Forms.Label();
            this.picMagamSave = new System.Windows.Forms.PictureBox();
            this.picMagamDataInfo = new System.Windows.Forms.PictureBox();
            this.lbMoney_2Type = new System.Windows.Forms.Label();
            this.lbMoney_3Type = new System.Windows.Forms.Label();
            this.lbMoney_4Type = new System.Windows.Forms.Label();
            this.lbMoney_5Type = new System.Windows.Forms.Label();
            this.lbMoney_6Type = new System.Windows.Forms.Label();
            this.lbl50MaxQty = new FadeFox.UI.SimpleLabel();
            this.lbl5000MaxQty = new FadeFox.UI.SimpleLabel();
            this.lbl1000MaxQty = new FadeFox.UI.SimpleLabel();
            this.lbl500MaxQty = new FadeFox.UI.SimpleLabel();
            this.lbl100MaxQty = new FadeFox.UI.SimpleLabel();
            this.lbl_TXT_MAX_AMOUNT = new System.Windows.Forms.Label();
            this.npPad = new NPAutoBooth.UI.NumberPad();
            this.btnOk_TXT_YES = new FadeFox.UI.ImageButton();
            this.btn_TXT_BACK = new FadeFox.UI.ImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.picMagamSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMagamDataInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk_TXT_YES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMoney_2TypeName
            // 
            this.lblMoney_2TypeName.BackColor = System.Drawing.Color.Transparent;
            this.lblMoney_2TypeName.ForeColor = System.Drawing.Color.White;
            this.lblMoney_2TypeName.Location = new System.Drawing.Point(83, 118);
            this.lblMoney_2TypeName.Name = "lblMoney_2TypeName";
            this.lblMoney_2TypeName.Size = new System.Drawing.Size(42, 12);
            this.lblMoney_2TypeName.TabIndex = 0;
            this.lblMoney_2TypeName.Tag = "";
            this.lblMoney_2TypeName.Text = "50";
            this.lblMoney_2TypeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_CLOSESETTION
            // 
            this.lbl_TXT_CLOSESETTION.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_CLOSESETTION.Font = new System.Drawing.Font("굴림", 27F, System.Drawing.FontStyle.Bold);
            this.lbl_TXT_CLOSESETTION.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_CLOSESETTION.Location = new System.Drawing.Point(40, 10);
            this.lbl_TXT_CLOSESETTION.Name = "lbl_TXT_CLOSESETTION";
            this.lbl_TXT_CLOSESETTION.Size = new System.Drawing.Size(803, 66);
            this.lbl_TXT_CLOSESETTION.TabIndex = 2;
            this.lbl_TXT_CLOSESETTION.Tag = "TXT_CLOSESETTION";
            this.lbl_TXT_CLOSESETTION.Text = "保有現金と締め設定";
            this.lbl_TXT_CLOSESETTION.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_TXT_CUR_AMOUNT
            // 
            this.lbl_TXT_CUR_AMOUNT.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_CUR_AMOUNT.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_CUR_AMOUNT.Location = new System.Drawing.Point(177, 83);
            this.lbl_TXT_CUR_AMOUNT.Name = "lbl_TXT_CUR_AMOUNT";
            this.lbl_TXT_CUR_AMOUNT.Size = new System.Drawing.Size(121, 12);
            this.lbl_TXT_CUR_AMOUNT.TabIndex = 3;
            this.lbl_TXT_CUR_AMOUNT.Tag = "TXT_CUR_AMOUNT";
            this.lbl_TXT_CUR_AMOUNT.Text = "現在数量";
            this.lbl_TXT_CUR_AMOUNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_TXT_MIN_AMOUNT
            // 
            this.lbl_TXT_MIN_AMOUNT.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_MIN_AMOUNT.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_MIN_AMOUNT.Location = new System.Drawing.Point(300, 83);
            this.lbl_TXT_MIN_AMOUNT.Name = "lbl_TXT_MIN_AMOUNT";
            this.lbl_TXT_MIN_AMOUNT.Size = new System.Drawing.Size(121, 12);
            this.lbl_TXT_MIN_AMOUNT.TabIndex = 4;
            this.lbl_TXT_MIN_AMOUNT.Tag = "TXT_MIN_AMOUNT";
            this.lbl_TXT_MIN_AMOUNT.Text = "最小数量";
            this.lbl_TXT_MIN_AMOUNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMoney_3TypeName
            // 
            this.lblMoney_3TypeName.BackColor = System.Drawing.Color.Transparent;
            this.lblMoney_3TypeName.ForeColor = System.Drawing.Color.White;
            this.lblMoney_3TypeName.Location = new System.Drawing.Point(83, 180);
            this.lblMoney_3TypeName.Name = "lblMoney_3TypeName";
            this.lblMoney_3TypeName.Size = new System.Drawing.Size(42, 12);
            this.lblMoney_3TypeName.TabIndex = 6;
            this.lblMoney_3TypeName.Tag = "";
            this.lblMoney_3TypeName.Text = "100";
            this.lblMoney_3TypeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMoney_4TypeName
            // 
            this.lblMoney_4TypeName.BackColor = System.Drawing.Color.Transparent;
            this.lblMoney_4TypeName.ForeColor = System.Drawing.Color.White;
            this.lblMoney_4TypeName.Location = new System.Drawing.Point(83, 234);
            this.lblMoney_4TypeName.Name = "lblMoney_4TypeName";
            this.lblMoney_4TypeName.Size = new System.Drawing.Size(42, 12);
            this.lblMoney_4TypeName.TabIndex = 9;
            this.lblMoney_4TypeName.Tag = "";
            this.lblMoney_4TypeName.Text = "500";
            this.lblMoney_4TypeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMoney_5TypeName
            // 
            this.lblMoney_5TypeName.BackColor = System.Drawing.Color.Transparent;
            this.lblMoney_5TypeName.ForeColor = System.Drawing.Color.White;
            this.lblMoney_5TypeName.Location = new System.Drawing.Point(83, 290);
            this.lblMoney_5TypeName.Name = "lblMoney_5TypeName";
            this.lblMoney_5TypeName.Size = new System.Drawing.Size(42, 12);
            this.lblMoney_5TypeName.TabIndex = 12;
            this.lblMoney_5TypeName.Tag = "";
            this.lblMoney_5TypeName.Text = "1000";
            this.lblMoney_5TypeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMoney_6TypeName
            // 
            this.lblMoney_6TypeName.BackColor = System.Drawing.Color.Transparent;
            this.lblMoney_6TypeName.ForeColor = System.Drawing.Color.White;
            this.lblMoney_6TypeName.Location = new System.Drawing.Point(83, 348);
            this.lblMoney_6TypeName.Name = "lblMoney_6TypeName";
            this.lblMoney_6TypeName.Size = new System.Drawing.Size(42, 12);
            this.lblMoney_6TypeName.TabIndex = 15;
            this.lblMoney_6TypeName.Tag = "";
            this.lblMoney_6TypeName.Text = "5000";
            this.lblMoney_6TypeName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_out50
            // 
            this.btn_out50.Location = new System.Drawing.Point(525, 100);
            this.btn_out50.Name = "btn_out50";
            this.btn_out50.Size = new System.Drawing.Size(91, 48);
            this.btn_out50.TabIndex = 22;
            this.btn_out50.Tag = "TXT_EJECT";
            this.btn_out50.Text = "放出";
            this.btn_out50.UseVisualStyleBackColor = true;
            this.btn_out50.Click += new System.EventHandler(this.btnOut50_Click);
            // 
            // btn_out100
            // 
            this.btn_out100.Location = new System.Drawing.Point(525, 154);
            this.btn_out100.Name = "btn_out100";
            this.btn_out100.Size = new System.Drawing.Size(91, 48);
            this.btn_out100.TabIndex = 23;
            this.btn_out100.Tag = "TXT_EJECT";
            this.btn_out100.Text = "放出";
            this.btn_out100.UseVisualStyleBackColor = true;
            this.btn_out100.Click += new System.EventHandler(this.btnOut100_Click);
            // 
            // btn_out500
            // 
            this.btn_out500.Location = new System.Drawing.Point(525, 208);
            this.btn_out500.Name = "btn_out500";
            this.btn_out500.Size = new System.Drawing.Size(91, 50);
            this.btn_out500.TabIndex = 24;
            this.btn_out500.Tag = "TXT_EJECT";
            this.btn_out500.Text = "放出";
            this.btn_out500.UseVisualStyleBackColor = true;
            this.btn_out500.Click += new System.EventHandler(this.btnOut500_Click);
            // 
            // btn_out1000
            // 
            this.btn_out1000.Location = new System.Drawing.Point(525, 266);
            this.btn_out1000.Name = "btn_out1000";
            this.btn_out1000.Size = new System.Drawing.Size(91, 48);
            this.btn_out1000.TabIndex = 25;
            this.btn_out1000.Tag = "TXT_EJECT";
            this.btn_out1000.Text = "放出";
            this.btn_out1000.UseVisualStyleBackColor = true;
            this.btn_out1000.Click += new System.EventHandler(this.btnOutTest1000_Click);
            // 
            // btn_out5000
            // 
            this.btn_out5000.Location = new System.Drawing.Point(525, 322);
            this.btn_out5000.Name = "btn_out5000";
            this.btn_out5000.Size = new System.Drawing.Size(91, 48);
            this.btn_out5000.TabIndex = 26;
            this.btn_out5000.Tag = "TXT_EJECT";
            this.btn_out5000.Text = "放出";
            this.btn_out5000.UseVisualStyleBackColor = true;
            this.btn_out5000.Click += new System.EventHandler(this.btnOutTest5000_Click);
            // 
            // tmrHome
            // 
            this.tmrHome.Tick += new System.EventHandler(this.tmrHome_Tick);
            // 
            // lbl50MinQty
            // 
            this.lbl50MinQty.BackColor = System.Drawing.Color.White;
            this.lbl50MinQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl50MinQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl50MinQty.Location = new System.Drawing.Point(306, 101);
            this.lbl50MinQty.Name = "lbl50MinQty";
            this.lbl50MinQty.PasswordChar = "";
            this.lbl50MinQty.Size = new System.Drawing.Size(100, 48);
            this.lbl50MinQty.TabIndex = 151;
            this.lbl50MinQty.Text = "0";
            this.lbl50MinQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl50MinQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl50MinQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl50SettingQty
            // 
            this.lbl50SettingQty.BackColor = System.Drawing.SystemColors.Control;
            this.lbl50SettingQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl50SettingQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl50SettingQty.Location = new System.Drawing.Point(190, 101);
            this.lbl50SettingQty.Name = "lbl50SettingQty";
            this.lbl50SettingQty.PasswordChar = "";
            this.lbl50SettingQty.Size = new System.Drawing.Size(100, 48);
            this.lbl50SettingQty.TabIndex = 150;
            this.lbl50SettingQty.Text = "0";
            this.lbl50SettingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl50SettingQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl50SettingQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl5000MinQty
            // 
            this.lbl5000MinQty.BackColor = System.Drawing.Color.White;
            this.lbl5000MinQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl5000MinQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl5000MinQty.Location = new System.Drawing.Point(306, 323);
            this.lbl5000MinQty.Name = "lbl5000MinQty";
            this.lbl5000MinQty.PasswordChar = "";
            this.lbl5000MinQty.Size = new System.Drawing.Size(100, 48);
            this.lbl5000MinQty.TabIndex = 148;
            this.lbl5000MinQty.Text = "0";
            this.lbl5000MinQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl5000MinQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl5000MinQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl5000SettingQty
            // 
            this.lbl5000SettingQty.BackColor = System.Drawing.Color.White;
            this.lbl5000SettingQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl5000SettingQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl5000SettingQty.Location = new System.Drawing.Point(190, 323);
            this.lbl5000SettingQty.Name = "lbl5000SettingQty";
            this.lbl5000SettingQty.PasswordChar = "";
            this.lbl5000SettingQty.Size = new System.Drawing.Size(100, 48);
            this.lbl5000SettingQty.TabIndex = 147;
            this.lbl5000SettingQty.Text = "0";
            this.lbl5000SettingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl5000SettingQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl5000SettingQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl1000MinQty
            // 
            this.lbl1000MinQty.BackColor = System.Drawing.Color.White;
            this.lbl1000MinQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl1000MinQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl1000MinQty.Location = new System.Drawing.Point(306, 267);
            this.lbl1000MinQty.Name = "lbl1000MinQty";
            this.lbl1000MinQty.PasswordChar = "";
            this.lbl1000MinQty.Size = new System.Drawing.Size(100, 48);
            this.lbl1000MinQty.TabIndex = 146;
            this.lbl1000MinQty.Text = "0";
            this.lbl1000MinQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl1000MinQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl1000MinQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl1000SettingQty
            // 
            this.lbl1000SettingQty.BackColor = System.Drawing.Color.White;
            this.lbl1000SettingQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl1000SettingQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl1000SettingQty.Location = new System.Drawing.Point(190, 267);
            this.lbl1000SettingQty.Name = "lbl1000SettingQty";
            this.lbl1000SettingQty.PasswordChar = "";
            this.lbl1000SettingQty.Size = new System.Drawing.Size(100, 48);
            this.lbl1000SettingQty.TabIndex = 145;
            this.lbl1000SettingQty.Text = "0";
            this.lbl1000SettingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl1000SettingQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl1000SettingQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl500MinQty
            // 
            this.lbl500MinQty.BackColor = System.Drawing.Color.White;
            this.lbl500MinQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl500MinQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl500MinQty.Location = new System.Drawing.Point(306, 211);
            this.lbl500MinQty.Name = "lbl500MinQty";
            this.lbl500MinQty.PasswordChar = "";
            this.lbl500MinQty.Size = new System.Drawing.Size(100, 48);
            this.lbl500MinQty.TabIndex = 144;
            this.lbl500MinQty.Text = "0";
            this.lbl500MinQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl500MinQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl500MinQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl500SettingQty
            // 
            this.lbl500SettingQty.BackColor = System.Drawing.Color.White;
            this.lbl500SettingQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl500SettingQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl500SettingQty.Location = new System.Drawing.Point(190, 211);
            this.lbl500SettingQty.Name = "lbl500SettingQty";
            this.lbl500SettingQty.PasswordChar = "";
            this.lbl500SettingQty.Size = new System.Drawing.Size(100, 48);
            this.lbl500SettingQty.TabIndex = 143;
            this.lbl500SettingQty.Text = "0";
            this.lbl500SettingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl500SettingQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl500SettingQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl100MinQty
            // 
            this.lbl100MinQty.BackColor = System.Drawing.Color.White;
            this.lbl100MinQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl100MinQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl100MinQty.Location = new System.Drawing.Point(306, 155);
            this.lbl100MinQty.Name = "lbl100MinQty";
            this.lbl100MinQty.PasswordChar = "";
            this.lbl100MinQty.Size = new System.Drawing.Size(100, 48);
            this.lbl100MinQty.TabIndex = 142;
            this.lbl100MinQty.Text = "0";
            this.lbl100MinQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl100MinQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl100MinQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl100SettingQty
            // 
            this.lbl100SettingQty.BackColor = System.Drawing.Color.White;
            this.lbl100SettingQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl100SettingQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl100SettingQty.Location = new System.Drawing.Point(190, 155);
            this.lbl100SettingQty.Name = "lbl100SettingQty";
            this.lbl100SettingQty.PasswordChar = "";
            this.lbl100SettingQty.Size = new System.Drawing.Size(100, 48);
            this.lbl100SettingQty.TabIndex = 141;
            this.lbl100SettingQty.Text = "0";
            this.lbl100SettingQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl100SettingQty.TextChanged += new System.EventHandler(this.TextChangedEvent);
            this.lbl100SettingQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl_TXT_CLOSING
            // 
            this.lbl_TXT_CLOSING.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_CLOSING.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_TXT_CLOSING.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_CLOSING.Location = new System.Drawing.Point(367, 574);
            this.lbl_TXT_CLOSING.Name = "lbl_TXT_CLOSING";
            this.lbl_TXT_CLOSING.Size = new System.Drawing.Size(161, 19);
            this.lbl_TXT_CLOSING.TabIndex = 184;
            this.lbl_TXT_CLOSING.Tag = "TXT_CLOSING";
            this.lbl_TXT_CLOSING.Text = "精算";
            this.lbl_TXT_CLOSING.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_TXT_CLOSEINFO
            // 
            this.lbl_TXT_CLOSEINFO.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_CLOSEINFO.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_TXT_CLOSEINFO.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_CLOSEINFO.Location = new System.Drawing.Point(173, 574);
            this.lbl_TXT_CLOSEINFO.Name = "lbl_TXT_CLOSEINFO";
            this.lbl_TXT_CLOSEINFO.Size = new System.Drawing.Size(161, 19);
            this.lbl_TXT_CLOSEINFO.TabIndex = 183;
            this.lbl_TXT_CLOSEINFO.Tag = "TXT_CLOSEINFO";
            this.lbl_TXT_CLOSEINFO.Text = "精算内容の確認";
            this.lbl_TXT_CLOSEINFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picMagamSave
            // 
            this.picMagamSave.BackColor = System.Drawing.Color.Transparent;
            this.picMagamSave.Image = global::NPAutoBooth.Properties.Resources.OLD_이미지_프린터;
            this.picMagamSave.Location = new System.Drawing.Point(385, 442);
            this.picMagamSave.Name = "picMagamSave";
            this.picMagamSave.Size = new System.Drawing.Size(131, 124);
            this.picMagamSave.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMagamSave.TabIndex = 182;
            this.picMagamSave.TabStop = false;
            this.picMagamSave.Click += new System.EventHandler(this.picPrint2_Click);
            // 
            // picMagamDataInfo
            // 
            this.picMagamDataInfo.BackColor = System.Drawing.Color.Transparent;
            this.picMagamDataInfo.Image = global::NPAutoBooth.Properties.Resources.OLD_이미지_프린터;
            this.picMagamDataInfo.Location = new System.Drawing.Point(196, 442);
            this.picMagamDataInfo.Name = "picMagamDataInfo";
            this.picMagamDataInfo.Size = new System.Drawing.Size(131, 124);
            this.picMagamDataInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMagamDataInfo.TabIndex = 181;
            this.picMagamDataInfo.TabStop = false;
            this.picMagamDataInfo.Click += new System.EventHandler(this.picPrint_Click);
            // 
            // lbMoney_2Type
            // 
            this.lbMoney_2Type.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney_2Type.ForeColor = System.Drawing.Color.White;
            this.lbMoney_2Type.Location = new System.Drawing.Point(130, 119);
            this.lbMoney_2Type.Name = "lbMoney_2Type";
            this.lbMoney_2Type.Size = new System.Drawing.Size(53, 12);
            this.lbMoney_2Type.TabIndex = 281;
            this.lbMoney_2Type.Tag = "UNIT_CASH";
            this.lbMoney_2Type.Text = "円";
            // 
            // lbMoney_3Type
            // 
            this.lbMoney_3Type.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney_3Type.ForeColor = System.Drawing.Color.White;
            this.lbMoney_3Type.Location = new System.Drawing.Point(130, 182);
            this.lbMoney_3Type.Name = "lbMoney_3Type";
            this.lbMoney_3Type.Size = new System.Drawing.Size(53, 12);
            this.lbMoney_3Type.TabIndex = 282;
            this.lbMoney_3Type.Tag = "UNIT_CASH";
            this.lbMoney_3Type.Text = "円";
            // 
            // lbMoney_4Type
            // 
            this.lbMoney_4Type.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney_4Type.ForeColor = System.Drawing.Color.White;
            this.lbMoney_4Type.Location = new System.Drawing.Point(130, 234);
            this.lbMoney_4Type.Name = "lbMoney_4Type";
            this.lbMoney_4Type.Size = new System.Drawing.Size(53, 12);
            this.lbMoney_4Type.TabIndex = 283;
            this.lbMoney_4Type.Tag = "UNIT_CASH";
            this.lbMoney_4Type.Text = "円";
            // 
            // lbMoney_5Type
            // 
            this.lbMoney_5Type.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney_5Type.ForeColor = System.Drawing.Color.White;
            this.lbMoney_5Type.Location = new System.Drawing.Point(130, 290);
            this.lbMoney_5Type.Name = "lbMoney_5Type";
            this.lbMoney_5Type.Size = new System.Drawing.Size(53, 12);
            this.lbMoney_5Type.TabIndex = 284;
            this.lbMoney_5Type.Tag = "UNIT_CASH";
            this.lbMoney_5Type.Text = "円";
            // 
            // lbMoney_6Type
            // 
            this.lbMoney_6Type.BackColor = System.Drawing.Color.Transparent;
            this.lbMoney_6Type.ForeColor = System.Drawing.Color.White;
            this.lbMoney_6Type.Location = new System.Drawing.Point(130, 348);
            this.lbMoney_6Type.Name = "lbMoney_6Type";
            this.lbMoney_6Type.Size = new System.Drawing.Size(53, 12);
            this.lbMoney_6Type.TabIndex = 285;
            this.lbMoney_6Type.Tag = "UNIT_CASH";
            this.lbMoney_6Type.Text = "円";
            // 
            // lbl50MaxQty
            // 
            this.lbl50MaxQty.BackColor = System.Drawing.Color.White;
            this.lbl50MaxQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl50MaxQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl50MaxQty.Location = new System.Drawing.Point(417, 100);
            this.lbl50MaxQty.Name = "lbl50MaxQty";
            this.lbl50MaxQty.PasswordChar = "";
            this.lbl50MaxQty.Size = new System.Drawing.Size(100, 48);
            this.lbl50MaxQty.TabIndex = 290;
            this.lbl50MaxQty.Text = "0";
            this.lbl50MaxQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl50MaxQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl5000MaxQty
            // 
            this.lbl5000MaxQty.BackColor = System.Drawing.Color.White;
            this.lbl5000MaxQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl5000MaxQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl5000MaxQty.Location = new System.Drawing.Point(417, 322);
            this.lbl5000MaxQty.Name = "lbl5000MaxQty";
            this.lbl5000MaxQty.PasswordChar = "";
            this.lbl5000MaxQty.Size = new System.Drawing.Size(100, 48);
            this.lbl5000MaxQty.TabIndex = 289;
            this.lbl5000MaxQty.Text = "0";
            this.lbl5000MaxQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl5000MaxQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl1000MaxQty
            // 
            this.lbl1000MaxQty.BackColor = System.Drawing.Color.White;
            this.lbl1000MaxQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl1000MaxQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl1000MaxQty.Location = new System.Drawing.Point(417, 266);
            this.lbl1000MaxQty.Name = "lbl1000MaxQty";
            this.lbl1000MaxQty.PasswordChar = "";
            this.lbl1000MaxQty.Size = new System.Drawing.Size(100, 48);
            this.lbl1000MaxQty.TabIndex = 288;
            this.lbl1000MaxQty.Text = "0";
            this.lbl1000MaxQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl1000MaxQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl500MaxQty
            // 
            this.lbl500MaxQty.BackColor = System.Drawing.Color.White;
            this.lbl500MaxQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl500MaxQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl500MaxQty.Location = new System.Drawing.Point(417, 210);
            this.lbl500MaxQty.Name = "lbl500MaxQty";
            this.lbl500MaxQty.PasswordChar = "";
            this.lbl500MaxQty.Size = new System.Drawing.Size(100, 48);
            this.lbl500MaxQty.TabIndex = 287;
            this.lbl500MaxQty.Text = "0";
            this.lbl500MaxQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl500MaxQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl100MaxQty
            // 
            this.lbl100MaxQty.BackColor = System.Drawing.Color.White;
            this.lbl100MaxQty.BorderColor = System.Drawing.Color.DimGray;
            this.lbl100MaxQty.Font = new System.Drawing.Font("돋움", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl100MaxQty.Location = new System.Drawing.Point(417, 154);
            this.lbl100MaxQty.Name = "lbl100MaxQty";
            this.lbl100MaxQty.PasswordChar = "";
            this.lbl100MaxQty.Size = new System.Drawing.Size(100, 48);
            this.lbl100MaxQty.TabIndex = 286;
            this.lbl100MaxQty.Text = "0";
            this.lbl100MaxQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl100MaxQty.Click += new System.EventHandler(this.Label_Click);
            // 
            // lbl_TXT_MAX_AMOUNT
            // 
            this.lbl_TXT_MAX_AMOUNT.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_MAX_AMOUNT.ForeColor = System.Drawing.Color.White;
            this.lbl_TXT_MAX_AMOUNT.Location = new System.Drawing.Point(417, 83);
            this.lbl_TXT_MAX_AMOUNT.Name = "lbl_TXT_MAX_AMOUNT";
            this.lbl_TXT_MAX_AMOUNT.Size = new System.Drawing.Size(121, 12);
            this.lbl_TXT_MAX_AMOUNT.TabIndex = 292;
            this.lbl_TXT_MAX_AMOUNT.Tag = "TXT_MAX_AMOUNT";
            this.lbl_TXT_MAX_AMOUNT.Text = "最大数量";
            this.lbl_TXT_MAX_AMOUNT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // npPad
            // 
            this.npPad.BackColor = System.Drawing.Color.Transparent;
            this.npPad.IsNumber = true;
            this.npPad.LimitLength = 10;
            this.npPad.LinkedSimpleLabel = null;
            this.npPad.Location = new System.Drawing.Point(692, 100);
            this.npPad.Name = "npPad";
            this.npPad.Size = new System.Drawing.Size(256, 342);
            this.npPad.TabIndex = 190;
            this.npPad.Value = "";
            // 
            // btnOk_TXT_YES
            // 
            this.btnOk_TXT_YES.BackColor = System.Drawing.Color.Transparent;
            this.btnOk_TXT_YES.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnOk_TXT_YES.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btnOk_TXT_YES.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnOk_TXT_YES.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btnOk_TXT_YES.DownTextColor = System.Drawing.Color.White;
            this.btnOk_TXT_YES.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk_TXT_YES.Font = new System.Drawing.Font("옥션고딕 B", 21.75F);
            this.btnOk_TXT_YES.ForeColor = System.Drawing.Color.Yellow;
            this.btnOk_TXT_YES.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btnOk_TXT_YES.Location = new System.Drawing.Point(652, 517);
            this.btnOk_TXT_YES.Name = "btnOk_TXT_YES";
            this.btnOk_TXT_YES.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btnOk_TXT_YES.Size = new System.Drawing.Size(138, 60);
            this.btnOk_TXT_YES.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnOk_TXT_YES.TabIndex = 294;
            this.btnOk_TXT_YES.TabStop = false;
            this.btnOk_TXT_YES.Tag = "TXT_CLOSESETTING";
            this.btnOk_TXT_YES.Text = "closeset";
            this.btnOk_TXT_YES.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnOk_TXT_YES.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk_TXT_YES.TextTopMargin = 2;
            this.btnOk_TXT_YES.UsingTextAlignInImage = false;
            this.btnOk_TXT_YES.UsingTextShadow = false;
            this.btnOk_TXT_YES.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btn_TXT_BACK
            // 
            this.btn_TXT_BACK.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_BACK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_BACK.DisabledImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXT_BACK.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_BACK.DownImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXT_BACK.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.Font = new System.Drawing.Font("옥션고딕 B", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_TXT_BACK.ForeColor = System.Drawing.Color.Yellow;
            this.btn_TXT_BACK.HoverImage = global::NPAutoBooth.Properties.Resources.btnOkBackon;
            this.btn_TXT_BACK.Location = new System.Drawing.Point(810, 517);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NormalImage = global::NPAutoBooth.Properties.Resources.btnOkBackOff;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(138, 60);
            this.btn_TXT_BACK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_BACK.TabIndex = 295;
            this.btn_TXT_BACK.TabStop = false;
            this.btn_TXT_BACK.Tag = "TXT_BACK";
            this.btn_TXT_BACK.Text = "back";
            this.btn_TXT_BACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_BACK.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.TextTopMargin = 2;
            this.btn_TXT_BACK.UsingTextAlignInImage = false;
            this.btn_TXT_BACK.UsingTextShadow = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormAdminCashSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1001, 600);
            this.Controls.Add(this.btn_TXT_BACK);
            this.Controls.Add(this.btnOk_TXT_YES);
            this.Controls.Add(this.lbl_TXT_MAX_AMOUNT);
            this.Controls.Add(this.lbl50MaxQty);
            this.Controls.Add(this.lbl5000MaxQty);
            this.Controls.Add(this.lbl1000MaxQty);
            this.Controls.Add(this.lbl500MaxQty);
            this.Controls.Add(this.lbl100MaxQty);
            this.Controls.Add(this.lbMoney_6Type);
            this.Controls.Add(this.lbMoney_5Type);
            this.Controls.Add(this.lbMoney_4Type);
            this.Controls.Add(this.lbMoney_3Type);
            this.Controls.Add(this.lbMoney_2Type);
            this.Controls.Add(this.npPad);
            this.Controls.Add(this.lbl_TXT_CLOSING);
            this.Controls.Add(this.lbl_TXT_CLOSEINFO);
            this.Controls.Add(this.picMagamSave);
            this.Controls.Add(this.picMagamDataInfo);
            this.Controls.Add(this.lbl_TXT_CLOSESETTION);
            this.Controls.Add(this.lbl50MinQty);
            this.Controls.Add(this.lbl50SettingQty);
            this.Controls.Add(this.lbl5000MinQty);
            this.Controls.Add(this.lbl5000SettingQty);
            this.Controls.Add(this.lbl1000MinQty);
            this.Controls.Add(this.lbl1000SettingQty);
            this.Controls.Add(this.lbl500MinQty);
            this.Controls.Add(this.lbl500SettingQty);
            this.Controls.Add(this.lbl100MinQty);
            this.Controls.Add(this.lbl100SettingQty);
            this.Controls.Add(this.btn_out5000);
            this.Controls.Add(this.btn_out1000);
            this.Controls.Add(this.btn_out500);
            this.Controls.Add(this.btn_out100);
            this.Controls.Add(this.btn_out50);
            this.Controls.Add(this.lblMoney_6TypeName);
            this.Controls.Add(this.lblMoney_5TypeName);
            this.Controls.Add(this.lblMoney_4TypeName);
            this.Controls.Add(this.lblMoney_3TypeName);
            this.Controls.Add(this.lbl_TXT_MIN_AMOUNT);
            this.Controls.Add(this.lbl_TXT_CUR_AMOUNT);
            this.Controls.Add(this.lblMoney_2TypeName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormAdminCashSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NPAutoBooth";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAdminCashSetting_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormAdminCashSetting_FormClosed);
            this.Load += new System.EventHandler(this.FromAdminCashSetting_load);
            ((System.ComponentModel.ISupportInitialize)(this.picMagamSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMagamDataInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk_TXT_YES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label lblMoney_2TypeName;
        private System.Windows.Forms.Label lbl_TXT_CLOSESETTION;
        private System.Windows.Forms.Label lbl_TXT_CUR_AMOUNT;
		private System.Windows.Forms.Label lbl_TXT_MIN_AMOUNT;
		private System.Windows.Forms.Label lblMoney_3TypeName;
		private System.Windows.Forms.Label lblMoney_4TypeName;
		private System.Windows.Forms.Label lblMoney_5TypeName;
        private System.Windows.Forms.Label lblMoney_6TypeName;
        private System.Windows.Forms.Button btn_out50;
        private System.Windows.Forms.Button btn_out100;
        private System.Windows.Forms.Button btn_out500;
        private System.Windows.Forms.Button btn_out1000;
        private System.Windows.Forms.Button btn_out5000;
        private System.Windows.Forms.Timer tmrHome;
		private FadeFox.UI.SimpleLabel lbl50MinQty;
        private FadeFox.UI.SimpleLabel lbl50SettingQty;
		private FadeFox.UI.SimpleLabel lbl5000MinQty;
		private FadeFox.UI.SimpleLabel lbl5000SettingQty;
		private FadeFox.UI.SimpleLabel lbl1000MinQty;
		private FadeFox.UI.SimpleLabel lbl1000SettingQty;
		private FadeFox.UI.SimpleLabel lbl500MinQty;
		private FadeFox.UI.SimpleLabel lbl500SettingQty;
		private FadeFox.UI.SimpleLabel lbl100MinQty;
		private FadeFox.UI.SimpleLabel lbl100SettingQty;
        private System.Windows.Forms.Label lbl_TXT_CLOSING;
        private System.Windows.Forms.Label lbl_TXT_CLOSEINFO;
        private System.Windows.Forms.PictureBox picMagamSave;
        private System.Windows.Forms.PictureBox picMagamDataInfo;
        private NumberPad npPad;
        private System.Windows.Forms.Label lbMoney_2Type;
        private System.Windows.Forms.Label lbMoney_3Type;
        private System.Windows.Forms.Label lbMoney_4Type;
        private System.Windows.Forms.Label lbMoney_5Type;
        private System.Windows.Forms.Label lbMoney_6Type;
        private FadeFox.UI.SimpleLabel lbl50MaxQty;
        private FadeFox.UI.SimpleLabel lbl5000MaxQty;
        private FadeFox.UI.SimpleLabel lbl1000MaxQty;
        private FadeFox.UI.SimpleLabel lbl500MaxQty;
        private FadeFox.UI.SimpleLabel lbl100MaxQty;
        private System.Windows.Forms.Label lbl_TXT_MAX_AMOUNT;
        private FadeFox.UI.ImageButton btnOk_TXT_YES;
        private FadeFox.UI.ImageButton btn_TXT_BACK;
    }
}

