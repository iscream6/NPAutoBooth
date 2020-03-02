namespace NPAutoBooth.UI.BoothUC
{
    partial class Ctl9by16SearchCar
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_ConfigClick1 = new System.Windows.Forms.Panel();
            this.panel_ConfigClick2 = new System.Windows.Forms.Panel();
            this.lbl_MSG_SEARCH_4NUMBER = new System.Windows.Forms.Label();
            this.btn_TXT_BACK = new FadeFox.UI.ImageButton();
            this.btnBackNumber = new FadeFox.UI.ImageButton();
            this.btnOk = new FadeFox.UI.ImageButton();
            this.btn_FIve = new FadeFox.UI.ImageButton();
            this.btn_Nine = new FadeFox.UI.ImageButton();
            this.btn_Eight = new FadeFox.UI.ImageButton();
            this.btn_Seven = new FadeFox.UI.ImageButton();
            this.btn_Six = new FadeFox.UI.ImageButton();
            this.btn_Four = new FadeFox.UI.ImageButton();
            this.btn_Three = new FadeFox.UI.ImageButton();
            this.btn_Two = new FadeFox.UI.ImageButton();
            this.btn_One = new FadeFox.UI.ImageButton();
            this.btn_Zero = new FadeFox.UI.ImageButton();
            this.pic_Dot1 = new System.Windows.Forms.PictureBox();
            this.btnEnglish = new FadeFox.UI.ImageButton();
            this.btnJapan = new FadeFox.UI.ImageButton();
            this.btn_TXT_HOME = new FadeFox.UI.ImageButton();
            this.uc_SearchCarTextBar = new NPAutoBooth.UserControl_SearchCar();
            this.lblHomeTextMsg = new System.Windows.Forms.Label();
            this.lblPremoveTextMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBackNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_FIve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Nine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Eight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Seven)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Six)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Four)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Three)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Two)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_One)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Zero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEnglish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnJapan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_HOME)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_ConfigClick1
            // 
            this.panel_ConfigClick1.BackColor = System.Drawing.Color.Transparent;
            this.panel_ConfigClick1.Location = new System.Drawing.Point(0, 0);
            this.panel_ConfigClick1.Name = "panel_ConfigClick1";
            this.panel_ConfigClick1.Size = new System.Drawing.Size(92, 97);
            this.panel_ConfigClick1.TabIndex = 266;
            this.panel_ConfigClick1.Click += new System.EventHandler(this.panel_ConfigClick1_Click);
            // 
            // panel_ConfigClick2
            // 
            this.panel_ConfigClick2.BackColor = System.Drawing.Color.Transparent;
            this.panel_ConfigClick2.Location = new System.Drawing.Point(979, 0);
            this.panel_ConfigClick2.Name = "panel_ConfigClick2";
            this.panel_ConfigClick2.Size = new System.Drawing.Size(100, 97);
            this.panel_ConfigClick2.TabIndex = 267;
            this.panel_ConfigClick2.Click += new System.EventHandler(this.panel_ConfigClick2_Click);
            // 
            // lbl_MSG_SEARCH_4NUMBER
            // 
            this.lbl_MSG_SEARCH_4NUMBER.BackColor = System.Drawing.Color.Transparent;
            this.lbl_MSG_SEARCH_4NUMBER.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_MSG_SEARCH_4NUMBER.Location = new System.Drawing.Point(63, 195);
            this.lbl_MSG_SEARCH_4NUMBER.Name = "lbl_MSG_SEARCH_4NUMBER";
            this.lbl_MSG_SEARCH_4NUMBER.Size = new System.Drawing.Size(953, 94);
            this.lbl_MSG_SEARCH_4NUMBER.TabIndex = 269;
            this.lbl_MSG_SEARCH_4NUMBER.Tag = "MSG_SEARCH_4NUMBER";
            this.lbl_MSG_SEARCH_4NUMBER.Text = "4桁の車両番号を入力してください。\r\nよろしければ、「確認」ボタンを押してください。";
            this.lbl_MSG_SEARCH_4NUMBER.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_TXT_BACK
            // 
            this.btn_TXT_BACK.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_BACK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_BACK.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_off;
            this.btn_TXT_BACK.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_BACK.DownImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_on;
            this.btn_TXT_BACK.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btn_TXT_BACK.ForeColor = System.Drawing.Color.Green;
            this.btn_TXT_BACK.HoverImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_on;
            this.btn_TXT_BACK.Location = new System.Drawing.Point(676, 1672);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NormalImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Premover_off;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(176, 127);
            this.btn_TXT_BACK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_BACK.TabIndex = 270;
            this.btn_TXT_BACK.TabStop = false;
            this.btn_TXT_BACK.Tag = "";
            this.btn_TXT_BACK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_BACK.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_BACK.TextTopMargin = 2;
            this.btn_TXT_BACK.UsingTextAlignInImage = false;
            this.btn_TXT_BACK.UsingTextShadow = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btn_TXT_HOME_Click);
            // 
            // btnBackNumber
            // 
            this.btnBackNumber.BackColor = System.Drawing.Color.Transparent;
            this.btnBackNumber.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnBackNumber.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnBackNumber.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnBackNumber.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnBackNumber.DownTextColor = System.Drawing.Color.White;
            this.btnBackNumber.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnBackNumber.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBackNumber.ForeColor = System.Drawing.Color.Green;
            this.btnBackNumber.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnBackNumber.Location = new System.Drawing.Point(146, 1397);
            this.btnBackNumber.Name = "btnBackNumber";
            this.btnBackNumber.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnBackNumber.Size = new System.Drawing.Size(258, 143);
            this.btnBackNumber.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnBackNumber.TabIndex = 222;
            this.btnBackNumber.TabStop = false;
            this.btnBackNumber.Text = "<-";
            this.btnBackNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnBackNumber.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnBackNumber.TextTopMargin = 2;
            this.btnBackNumber.UsingTextAlignInImage = false;
            this.btnBackNumber.UsingTextShadow = false;
            this.btnBackNumber.Click += new System.EventHandler(this.btnBackNumber_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnOk.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnOk.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnOk.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnOk.DownTextColor = System.Drawing.Color.White;
            this.btnOk.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.Color.Green;
            this.btnOk.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnOk.Location = new System.Drawing.Point(676, 1397);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnOk.Size = new System.Drawing.Size(258, 143);
            this.btnOk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnOk.TabIndex = 220;
            this.btnOk.TabStop = false;
            this.btnOk.Tag = "TXT_ENTER";
            this.btnOk.Text = "確認";
            this.btnOk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnOk.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnOk.TextTopMargin = 2;
            this.btnOk.UsingTextAlignInImage = false;
            this.btnOk.UsingTextShadow = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btn_FIve
            // 
            this.btn_FIve.BackColor = System.Drawing.Color.Transparent;
            this.btn_FIve.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_FIve.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_FIve.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_FIve.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_FIve.DownTextColor = System.Drawing.Color.White;
            this.btn_FIve.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_FIve.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_FIve.ForeColor = System.Drawing.Color.Green;
            this.btn_FIve.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_FIve.Location = new System.Drawing.Point(410, 1071);
            this.btn_FIve.Name = "btn_FIve";
            this.btn_FIve.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_FIve.Size = new System.Drawing.Size(258, 143);
            this.btn_FIve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_FIve.TabIndex = 219;
            this.btn_FIve.TabStop = false;
            this.btn_FIve.Text = "5";
            this.btn_FIve.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_FIve.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_FIve.TextTopMargin = 2;
            this.btn_FIve.UsingTextAlignInImage = false;
            this.btn_FIve.UsingTextShadow = false;
            this.btn_FIve.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Nine
            // 
            this.btn_Nine.BackColor = System.Drawing.Color.Transparent;
            this.btn_Nine.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Nine.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Nine.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Nine.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Nine.DownTextColor = System.Drawing.Color.White;
            this.btn_Nine.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Nine.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Nine.ForeColor = System.Drawing.Color.Green;
            this.btn_Nine.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Nine.Location = new System.Drawing.Point(675, 1234);
            this.btn_Nine.Name = "btn_Nine";
            this.btn_Nine.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Nine.Size = new System.Drawing.Size(258, 143);
            this.btn_Nine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Nine.TabIndex = 218;
            this.btn_Nine.TabStop = false;
            this.btn_Nine.Text = "9";
            this.btn_Nine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Nine.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Nine.TextTopMargin = 2;
            this.btn_Nine.UsingTextAlignInImage = false;
            this.btn_Nine.UsingTextShadow = false;
            this.btn_Nine.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Eight
            // 
            this.btn_Eight.BackColor = System.Drawing.Color.Transparent;
            this.btn_Eight.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Eight.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Eight.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Eight.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Eight.DownTextColor = System.Drawing.Color.White;
            this.btn_Eight.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Eight.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Eight.ForeColor = System.Drawing.Color.Green;
            this.btn_Eight.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Eight.Location = new System.Drawing.Point(408, 1234);
            this.btn_Eight.Name = "btn_Eight";
            this.btn_Eight.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Eight.Size = new System.Drawing.Size(258, 143);
            this.btn_Eight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Eight.TabIndex = 217;
            this.btn_Eight.TabStop = false;
            this.btn_Eight.Text = "8";
            this.btn_Eight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Eight.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Eight.TextTopMargin = 2;
            this.btn_Eight.UsingTextAlignInImage = false;
            this.btn_Eight.UsingTextShadow = false;
            this.btn_Eight.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Seven
            // 
            this.btn_Seven.BackColor = System.Drawing.Color.Transparent;
            this.btn_Seven.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Seven.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Seven.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Seven.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Seven.DownTextColor = System.Drawing.Color.White;
            this.btn_Seven.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Seven.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Seven.ForeColor = System.Drawing.Color.Green;
            this.btn_Seven.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Seven.Location = new System.Drawing.Point(146, 1234);
            this.btn_Seven.Name = "btn_Seven";
            this.btn_Seven.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Seven.Size = new System.Drawing.Size(258, 143);
            this.btn_Seven.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Seven.TabIndex = 216;
            this.btn_Seven.TabStop = false;
            this.btn_Seven.Text = "7";
            this.btn_Seven.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Seven.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Seven.TextTopMargin = 2;
            this.btn_Seven.UsingTextAlignInImage = false;
            this.btn_Seven.UsingTextShadow = false;
            this.btn_Seven.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Six
            // 
            this.btn_Six.BackColor = System.Drawing.Color.Transparent;
            this.btn_Six.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Six.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Six.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Six.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Six.DownTextColor = System.Drawing.Color.White;
            this.btn_Six.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Six.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Six.ForeColor = System.Drawing.Color.Green;
            this.btn_Six.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Six.Location = new System.Drawing.Point(677, 1071);
            this.btn_Six.Name = "btn_Six";
            this.btn_Six.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Six.Size = new System.Drawing.Size(258, 143);
            this.btn_Six.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Six.TabIndex = 215;
            this.btn_Six.TabStop = false;
            this.btn_Six.Text = "6";
            this.btn_Six.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Six.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Six.TextTopMargin = 2;
            this.btn_Six.UsingTextAlignInImage = false;
            this.btn_Six.UsingTextShadow = false;
            this.btn_Six.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Four
            // 
            this.btn_Four.BackColor = System.Drawing.Color.Transparent;
            this.btn_Four.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Four.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Four.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Four.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Four.DownTextColor = System.Drawing.Color.White;
            this.btn_Four.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Four.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Four.ForeColor = System.Drawing.Color.Green;
            this.btn_Four.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Four.Location = new System.Drawing.Point(146, 1071);
            this.btn_Four.Name = "btn_Four";
            this.btn_Four.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Four.Size = new System.Drawing.Size(258, 143);
            this.btn_Four.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Four.TabIndex = 213;
            this.btn_Four.TabStop = false;
            this.btn_Four.Text = "4";
            this.btn_Four.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Four.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Four.TextTopMargin = 2;
            this.btn_Four.UsingTextAlignInImage = false;
            this.btn_Four.UsingTextShadow = false;
            this.btn_Four.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Three
            // 
            this.btn_Three.BackColor = System.Drawing.Color.Transparent;
            this.btn_Three.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Three.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Three.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Three.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Three.DownTextColor = System.Drawing.Color.White;
            this.btn_Three.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Three.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Three.ForeColor = System.Drawing.Color.Green;
            this.btn_Three.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Three.Location = new System.Drawing.Point(678, 908);
            this.btn_Three.Name = "btn_Three";
            this.btn_Three.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Three.Size = new System.Drawing.Size(258, 143);
            this.btn_Three.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Three.TabIndex = 212;
            this.btn_Three.TabStop = false;
            this.btn_Three.Text = "3";
            this.btn_Three.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Three.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Three.TextTopMargin = 2;
            this.btn_Three.UsingTextAlignInImage = false;
            this.btn_Three.UsingTextShadow = false;
            this.btn_Three.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Two
            // 
            this.btn_Two.BackColor = System.Drawing.Color.Transparent;
            this.btn_Two.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Two.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Two.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Two.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Two.DownTextColor = System.Drawing.Color.White;
            this.btn_Two.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Two.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Two.ForeColor = System.Drawing.Color.Green;
            this.btn_Two.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Two.Location = new System.Drawing.Point(412, 908);
            this.btn_Two.Name = "btn_Two";
            this.btn_Two.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Two.Size = new System.Drawing.Size(258, 143);
            this.btn_Two.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Two.TabIndex = 211;
            this.btn_Two.TabStop = false;
            this.btn_Two.Text = "2";
            this.btn_Two.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Two.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Two.TextTopMargin = 2;
            this.btn_Two.UsingTextAlignInImage = false;
            this.btn_Two.UsingTextShadow = false;
            this.btn_Two.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_One
            // 
            this.btn_One.BackColor = System.Drawing.Color.Transparent;
            this.btn_One.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_One.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_One.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_One.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_One.DownTextColor = System.Drawing.Color.White;
            this.btn_One.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_One.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_One.ForeColor = System.Drawing.Color.Green;
            this.btn_One.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_One.Location = new System.Drawing.Point(146, 908);
            this.btn_One.Name = "btn_One";
            this.btn_One.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_One.Size = new System.Drawing.Size(258, 143);
            this.btn_One.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_One.TabIndex = 210;
            this.btn_One.TabStop = false;
            this.btn_One.Text = "1";
            this.btn_One.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_One.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_One.TextTopMargin = 2;
            this.btn_One.UsingTextAlignInImage = false;
            this.btn_One.UsingTextShadow = false;
            this.btn_One.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // btn_Zero
            // 
            this.btn_Zero.BackColor = System.Drawing.Color.Transparent;
            this.btn_Zero.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Zero.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Zero.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_Zero.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Zero.DownTextColor = System.Drawing.Color.White;
            this.btn_Zero.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Zero.Font = new System.Drawing.Font("옥션고딕 B", 50.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Zero.ForeColor = System.Drawing.Color.Green;
            this.btn_Zero.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btn_Zero.Location = new System.Drawing.Point(411, 1397);
            this.btn_Zero.Name = "btn_Zero";
            this.btn_Zero.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btn_Zero.Size = new System.Drawing.Size(258, 143);
            this.btn_Zero.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Zero.TabIndex = 209;
            this.btn_Zero.TabStop = false;
            this.btn_Zero.Text = "0";
            this.btn_Zero.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_Zero.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_Zero.TextTopMargin = 2;
            this.btn_Zero.UsingTextAlignInImage = false;
            this.btn_Zero.UsingTextShadow = false;
            this.btn_Zero.Click += new System.EventHandler(this.btnNumber_Click);
            // 
            // pic_Dot1
            // 
            this.pic_Dot1.BackColor = System.Drawing.Color.Transparent;
            this.pic_Dot1.Image = global::NPAutoBooth.Properties.Resources.Type2_Dot;
            this.pic_Dot1.Location = new System.Drawing.Point(63, 302);
            this.pic_Dot1.Name = "pic_Dot1";
            this.pic_Dot1.Size = new System.Drawing.Size(958, 21);
            this.pic_Dot1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Dot1.TabIndex = 324;
            this.pic_Dot1.TabStop = false;
            // 
            // btnEnglish
            // 
            this.btnEnglish.BackColor = System.Drawing.Color.Transparent;
            this.btnEnglish.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnEnglish.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnEnglish.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnEnglish.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnEnglish.DownTextColor = System.Drawing.Color.White;
            this.btnEnglish.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnEnglish.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btnEnglish.ForeColor = System.Drawing.Color.Green;
            this.btnEnglish.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnEnglish.Location = new System.Drawing.Point(778, 1583);
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnEnglish.Size = new System.Drawing.Size(243, 83);
            this.btnEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnEnglish.TabIndex = 330;
            this.btnEnglish.TabStop = false;
            this.btnEnglish.Tag = "";
            this.btnEnglish.Text = "English";
            this.btnEnglish.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnEnglish.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnEnglish.TextTopMargin = 2;
            this.btnEnglish.UsingTextAlignInImage = false;
            this.btnEnglish.UsingTextShadow = false;
            this.btnEnglish.Click += new System.EventHandler(this.btnEnglish_Click);
            // 
            // btnJapan
            // 
            this.btnJapan.BackColor = System.Drawing.Color.Transparent;
            this.btnJapan.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnJapan.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnJapan.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnJapan.DownImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnJapan.DownTextColor = System.Drawing.Color.White;
            this.btnJapan.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnJapan.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btnJapan.ForeColor = System.Drawing.Color.Green;
            this.btnJapan.HoverImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOn;
            this.btnJapan.Location = new System.Drawing.Point(529, 1583);
            this.btnJapan.Name = "btnJapan";
            this.btnJapan.NormalImage = global::NPAutoBooth.Properties.Resources.Type2ButtonOff;
            this.btnJapan.Size = new System.Drawing.Size(243, 83);
            this.btnJapan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnJapan.TabIndex = 329;
            this.btnJapan.TabStop = false;
            this.btnJapan.Tag = "";
            this.btnJapan.Text = "日本語";
            this.btnJapan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnJapan.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnJapan.TextTopMargin = 2;
            this.btnJapan.UsingTextAlignInImage = false;
            this.btnJapan.UsingTextShadow = false;
            this.btnJapan.Click += new System.EventHandler(this.btnJapan_Click);
            // 
            // btn_TXT_HOME
            // 
            this.btn_TXT_HOME.BackColor = System.Drawing.Color.Transparent;
            this.btn_TXT_HOME.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TXT_HOME.DisabledImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Home_off;
            this.btn_TXT_HOME.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_HOME.DownImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Home_on;
            this.btn_TXT_HOME.DownTextColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.DownTextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_HOME.Font = new System.Drawing.Font("굴림", 40F, System.Drawing.FontStyle.Bold);
            this.btn_TXT_HOME.ForeColor = System.Drawing.Color.Green;
            this.btn_TXT_HOME.HoverImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Home_on;
            this.btn_TXT_HOME.Location = new System.Drawing.Point(228, 1672);
            this.btn_TXT_HOME.Name = "btn_TXT_HOME";
            this.btn_TXT_HOME.NormalImage = global::NPAutoBooth.Properties.Resources.Type2_Button_Home_off;
            this.btn_TXT_HOME.Size = new System.Drawing.Size(176, 127);
            this.btn_TXT_HOME.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_TXT_HOME.TabIndex = 332;
            this.btn_TXT_HOME.TabStop = false;
            this.btn_TXT_HOME.Tag = "";
            this.btn_TXT_HOME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btn_TXT_HOME.TextShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btn_TXT_HOME.TextTopMargin = 2;
            this.btn_TXT_HOME.UsingTextAlignInImage = false;
            this.btn_TXT_HOME.UsingTextShadow = false;
            this.btn_TXT_HOME.Click += new System.EventHandler(this.btn_TXT_HOME_Click);
            // 
            // uc_SearchCarTextBar
            // 
            this.uc_SearchCarTextBar.Location = new System.Drawing.Point(160, 598);
            this.uc_SearchCarTextBar.Name = "uc_SearchCarTextBar";
            this.uc_SearchCarTextBar.NumberFont = new System.Drawing.Font("옥션고딕 B", 110F);
            this.uc_SearchCarTextBar.SetFiveText = "7";
            this.uc_SearchCarTextBar.SetFourText = "7";
            this.uc_SearchCarTextBar.SetOneText = "9";
            this.uc_SearchCarTextBar.SetThreeText = "8";
            this.uc_SearchCarTextBar.SetTwoText = "9";
            this.uc_SearchCarTextBar.Size = new System.Drawing.Size(763, 151);
            this.uc_SearchCarTextBar.TabIndex = 333;
            // 
            // lblHomeTextMsg
            // 
            this.lblHomeTextMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblHomeTextMsg.Font = new System.Drawing.Font("옥션고딕 B", 22F);
            this.lblHomeTextMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.lblHomeTextMsg.Location = new System.Drawing.Point(228, 1813);
            this.lblHomeTextMsg.Name = "lblHomeTextMsg";
            this.lblHomeTextMsg.Size = new System.Drawing.Size(176, 46);
            this.lblHomeTextMsg.TabIndex = 334;
            this.lblHomeTextMsg.Tag = "TXT_HOME";
            this.lblHomeTextMsg.Text = "처음화면";
            this.lblHomeTextMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPremoveTextMsg
            // 
            this.lblPremoveTextMsg.BackColor = System.Drawing.Color.Transparent;
            this.lblPremoveTextMsg.Font = new System.Drawing.Font("옥션고딕 B", 22F);
            this.lblPremoveTextMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.lblPremoveTextMsg.Location = new System.Drawing.Point(676, 1813);
            this.lblPremoveTextMsg.Name = "lblPremoveTextMsg";
            this.lblPremoveTextMsg.Size = new System.Drawing.Size(176, 46);
            this.lblPremoveTextMsg.TabIndex = 335;
            this.lblPremoveTextMsg.Tag = "TXT_BACK";
            this.lblPremoveTextMsg.Text = "이전화면";
            this.lblPremoveTextMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Ctl9by16SearchCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NPAutoBooth.Properties.Resources.Type2BackGround1080;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblPremoveTextMsg);
            this.Controls.Add(this.lblHomeTextMsg);
            this.Controls.Add(this.uc_SearchCarTextBar);
            this.Controls.Add(this.btn_TXT_HOME);
            this.Controls.Add(this.btnEnglish);
            this.Controls.Add(this.btnJapan);
            this.Controls.Add(this.pic_Dot1);
            this.Controls.Add(this.btn_TXT_BACK);
            this.Controls.Add(this.lbl_MSG_SEARCH_4NUMBER);
            this.Controls.Add(this.panel_ConfigClick2);
            this.Controls.Add(this.panel_ConfigClick1);
            this.Controls.Add(this.btnBackNumber);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btn_FIve);
            this.Controls.Add(this.btn_Nine);
            this.Controls.Add(this.btn_Eight);
            this.Controls.Add(this.btn_Seven);
            this.Controls.Add(this.btn_Six);
            this.Controls.Add(this.btn_Four);
            this.Controls.Add(this.btn_Three);
            this.Controls.Add(this.btn_Two);
            this.Controls.Add(this.btn_One);
            this.Controls.Add(this.btn_Zero);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Ctl9by16SearchCar";
            this.Size = new System.Drawing.Size(1080, 1920);
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_BACK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnBackNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_FIve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Nine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Eight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Seven)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Six)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Four)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Three)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Two)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_One)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Zero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEnglish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnJapan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_TXT_HOME)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FadeFox.UI.ImageButton btn_Zero;
        private FadeFox.UI.ImageButton btn_One;
        private FadeFox.UI.ImageButton btn_Two;
        private FadeFox.UI.ImageButton btn_Three;
        private FadeFox.UI.ImageButton btn_Four;
        private FadeFox.UI.ImageButton btnOk;
        private FadeFox.UI.ImageButton btn_FIve;
        private FadeFox.UI.ImageButton btn_Nine;
        private FadeFox.UI.ImageButton btn_Eight;
        private FadeFox.UI.ImageButton btn_Seven;
        private FadeFox.UI.ImageButton btn_Six;
        private FadeFox.UI.ImageButton btnBackNumber;
        private System.Windows.Forms.Panel panel_ConfigClick1;
        private System.Windows.Forms.Panel panel_ConfigClick2;
        private System.Windows.Forms.Label lbl_MSG_SEARCH_4NUMBER;
        private FadeFox.UI.ImageButton btn_TXT_BACK;
        private System.Windows.Forms.PictureBox pic_Dot1;
        private FadeFox.UI.ImageButton btnEnglish;
        private FadeFox.UI.ImageButton btnJapan;
        private FadeFox.UI.ImageButton btn_TXT_HOME;
        private NPAutoBooth.UserControl_SearchCar uc_SearchCarTextBar;
        private System.Windows.Forms.Label lblHomeTextMsg;
        private System.Windows.Forms.Label lblPremoveTextMsg;
    }
}
