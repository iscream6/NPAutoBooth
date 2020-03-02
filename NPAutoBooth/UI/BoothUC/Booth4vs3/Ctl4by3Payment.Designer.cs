namespace NPAutoBooth.UI.BoothUC
{
    partial class Ctl4by3Payment
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
            this.pic_Dot1 = new System.Windows.Forms.PictureBox();
            this.lbl_MSG_PAYINFO = new System.Windows.Forms.Label();
            this.lbl_MSG_DISCOUNTINFO = new System.Windows.Forms.Label();
            this.panel_ConfigClick2 = new System.Windows.Forms.Panel();
            this.panel_ConfigClick1 = new System.Windows.Forms.Panel();
            this.lbl_UNIT_CASH3 = new System.Windows.Forms.Label();
            this.lbl_TXT_PREFEE = new System.Windows.Forms.Label();
            this.lbl_RecvMoney = new System.Windows.Forms.Label();
            this.pic_Dot2 = new System.Windows.Forms.PictureBox();
            this.lbl_TXT_PARKINGFEE = new System.Windows.Forms.Label();
            this.lbl_TXT_DISCOUNTFEE = new System.Windows.Forms.Label();
            this.lbl_TXT_AMOUNTFEE = new System.Windows.Forms.Label();
            this.lbl_UNIT_CASH4 = new System.Windows.Forms.Label();
            this.lbl_UNIT_CASH2 = new System.Windows.Forms.Label();
            this.lbl_UNIT_CASH1 = new System.Windows.Forms.Label();
            this.lbl_TXT_ELAPSEDTIME = new System.Windows.Forms.Label();
            this.lbl_TXT_INDATE = new System.Windows.Forms.Label();
            this.lbl_TXT_CARNO = new System.Windows.Forms.Label();
            this.lbl_DIscountMoney = new System.Windows.Forms.Label();
            this.lbl_CarType = new System.Windows.Forms.Label();
            this.lblCarnumber2 = new System.Windows.Forms.Label();
            this.pic_carimage = new System.Windows.Forms.PictureBox();
            this.lbl_Payment = new System.Windows.Forms.Label();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.lblParkingFee = new System.Windows.Forms.Label();
            this.lblIndate = new System.Windows.Forms.Label();
            this.lbl_CarNumber = new System.Windows.Forms.Label();
            this.lblDiscountCountName = new System.Windows.Forms.Label();
            this.lblDiscountInputCount = new System.Windows.Forms.Label();
            this.lblRegExpireInfo = new System.Windows.Forms.Label();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.btnSixMonthAdd = new FadeFox.UI.NPButton();
            this.btnFiveMonthAdd = new FadeFox.UI.NPButton();
            this.btnFourMonthAdd = new FadeFox.UI.NPButton();
            this.btnThreeMonthAdd = new FadeFox.UI.NPButton();
            this.btnTwoMonthAdd = new FadeFox.UI.NPButton();
            this.btnOneMonthAdd = new FadeFox.UI.NPButton();
            this.btn_TXT_CANCEL = new FadeFox.UI.NPButton();
            this.btnCardApproval = new FadeFox.UI.NPButton();
            this.btn_TXT_BACK = new FadeFox.UI.NPButton();
            this.btn_TXT_HOME = new FadeFox.UI.NPButton();
            this.btnEnglish = new FadeFox.UI.NPButton();
            this.btnJapan = new FadeFox.UI.NPButton();
            this.btnSamsunPay = new FadeFox.UI.NPButton();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_carimage)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Dot1
            // 
            this.pic_Dot1.BackColor = System.Drawing.Color.Transparent;
            this.pic_Dot1.Image = global::NPAutoBooth.Properties.Resources.Type2_Dot;
            this.pic_Dot1.Location = new System.Drawing.Point(29, 142);
            this.pic_Dot1.Name = "pic_Dot1";
            this.pic_Dot1.Size = new System.Drawing.Size(971, 21);
            this.pic_Dot1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Dot1.TabIndex = 328;
            this.pic_Dot1.TabStop = false;
            // 
            // lbl_MSG_PAYINFO
            // 
            this.lbl_MSG_PAYINFO.BackColor = System.Drawing.Color.Transparent;
            this.lbl_MSG_PAYINFO.Font = new System.Drawing.Font("옥션고딕 M", 30F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MSG_PAYINFO.Location = new System.Drawing.Point(54, 93);
            this.lbl_MSG_PAYINFO.Name = "lbl_MSG_PAYINFO";
            this.lbl_MSG_PAYINFO.Size = new System.Drawing.Size(915, 42);
            this.lbl_MSG_PAYINFO.TabIndex = 327;
            this.lbl_MSG_PAYINFO.Tag = "";
            this.lbl_MSG_PAYINFO.Text = "현금또는 신용카드로 결제해주세요";
            this.lbl_MSG_PAYINFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_MSG_DISCOUNTINFO
            // 
            this.lbl_MSG_DISCOUNTINFO.BackColor = System.Drawing.Color.Transparent;
            this.lbl_MSG_DISCOUNTINFO.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_MSG_DISCOUNTINFO.Location = new System.Drawing.Point(54, 45);
            this.lbl_MSG_DISCOUNTINFO.Name = "lbl_MSG_DISCOUNTINFO";
            this.lbl_MSG_DISCOUNTINFO.Size = new System.Drawing.Size(915, 42);
            this.lbl_MSG_DISCOUNTINFO.TabIndex = 326;
            this.lbl_MSG_DISCOUNTINFO.Tag = "";
            this.lbl_MSG_DISCOUNTINFO.Text = "할인권이 있다면 스캔해 주시고";
            this.lbl_MSG_DISCOUNTINFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_ConfigClick2
            // 
            this.panel_ConfigClick2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_ConfigClick2.BackColor = System.Drawing.Color.Transparent;
            this.panel_ConfigClick2.Location = new System.Drawing.Point(975, 1);
            this.panel_ConfigClick2.Name = "panel_ConfigClick2";
            this.panel_ConfigClick2.Size = new System.Drawing.Size(48, 108);
            this.panel_ConfigClick2.TabIndex = 325;
            this.panel_ConfigClick2.Click += new System.EventHandler(this.panel_ConfigClick2_Click);
            // 
            // panel_ConfigClick1
            // 
            this.panel_ConfigClick1.BackColor = System.Drawing.Color.Transparent;
            this.panel_ConfigClick1.Location = new System.Drawing.Point(0, 1);
            this.panel_ConfigClick1.Name = "panel_ConfigClick1";
            this.panel_ConfigClick1.Size = new System.Drawing.Size(48, 108);
            this.panel_ConfigClick1.TabIndex = 324;
            this.panel_ConfigClick1.Click += new System.EventHandler(this.panel_ConfigClick1_Click);
            // 
            // lbl_UNIT_CASH3
            // 
            this.lbl_UNIT_CASH3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UNIT_CASH3.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UNIT_CASH3.Font = new System.Drawing.Font("옥션고딕 L", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UNIT_CASH3.ForeColor = System.Drawing.Color.Black;
            this.lbl_UNIT_CASH3.Location = new System.Drawing.Point(562, 411);
            this.lbl_UNIT_CASH3.Name = "lbl_UNIT_CASH3";
            this.lbl_UNIT_CASH3.Size = new System.Drawing.Size(58, 41);
            this.lbl_UNIT_CASH3.TabIndex = 366;
            this.lbl_UNIT_CASH3.Tag = "UNIT_CASH";
            this.lbl_UNIT_CASH3.Text = "円";
            this.lbl_UNIT_CASH3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_PREFEE
            // 
            this.lbl_TXT_PREFEE.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_PREFEE.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_PREFEE.Location = new System.Drawing.Point(30, 414);
            this.lbl_TXT_PREFEE.Name = "lbl_TXT_PREFEE";
            this.lbl_TXT_PREFEE.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_PREFEE.TabIndex = 365;
            this.lbl_TXT_PREFEE.Tag = "TXT_PREFEE";
            this.lbl_TXT_PREFEE.Text = "사전정산금액";
            this.lbl_TXT_PREFEE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_RecvMoney
            // 
            this.lbl_RecvMoney.BackColor = System.Drawing.Color.Transparent;
            this.lbl_RecvMoney.Font = new System.Drawing.Font("옥션고딕 B", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RecvMoney.ForeColor = System.Drawing.Color.Olive;
            this.lbl_RecvMoney.Location = new System.Drawing.Point(296, 414);
            this.lbl_RecvMoney.Name = "lbl_RecvMoney";
            this.lbl_RecvMoney.Size = new System.Drawing.Size(261, 34);
            this.lbl_RecvMoney.TabIndex = 364;
            this.lbl_RecvMoney.Text = "3,000";
            this.lbl_RecvMoney.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pic_Dot2
            // 
            this.pic_Dot2.BackColor = System.Drawing.Color.Transparent;
            this.pic_Dot2.Image = global::NPAutoBooth.Properties.Resources.Type2_Dot;
            this.pic_Dot2.Location = new System.Drawing.Point(29, 512);
            this.pic_Dot2.Name = "pic_Dot2";
            this.pic_Dot2.Size = new System.Drawing.Size(971, 21);
            this.pic_Dot2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Dot2.TabIndex = 363;
            this.pic_Dot2.TabStop = false;
            // 
            // lbl_TXT_PARKINGFEE
            // 
            this.lbl_TXT_PARKINGFEE.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_PARKINGFEE.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_PARKINGFEE.Location = new System.Drawing.Point(30, 318);
            this.lbl_TXT_PARKINGFEE.Name = "lbl_TXT_PARKINGFEE";
            this.lbl_TXT_PARKINGFEE.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_PARKINGFEE.TabIndex = 362;
            this.lbl_TXT_PARKINGFEE.Tag = "";
            this.lbl_TXT_PARKINGFEE.Text = "주차요금";
            this.lbl_TXT_PARKINGFEE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_DISCOUNTFEE
            // 
            this.lbl_TXT_DISCOUNTFEE.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_DISCOUNTFEE.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_DISCOUNTFEE.Location = new System.Drawing.Point(30, 366);
            this.lbl_TXT_DISCOUNTFEE.Name = "lbl_TXT_DISCOUNTFEE";
            this.lbl_TXT_DISCOUNTFEE.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_DISCOUNTFEE.TabIndex = 361;
            this.lbl_TXT_DISCOUNTFEE.Tag = "TXT_DISCOUNTFEE";
            this.lbl_TXT_DISCOUNTFEE.Text = "할인요금";
            this.lbl_TXT_DISCOUNTFEE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_AMOUNTFEE
            // 
            this.lbl_TXT_AMOUNTFEE.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_AMOUNTFEE.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_AMOUNTFEE.Location = new System.Drawing.Point(30, 462);
            this.lbl_TXT_AMOUNTFEE.Name = "lbl_TXT_AMOUNTFEE";
            this.lbl_TXT_AMOUNTFEE.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_AMOUNTFEE.TabIndex = 360;
            this.lbl_TXT_AMOUNTFEE.Tag = "";
            this.lbl_TXT_AMOUNTFEE.Text = "결제요금";
            this.lbl_TXT_AMOUNTFEE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_UNIT_CASH4
            // 
            this.lbl_UNIT_CASH4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UNIT_CASH4.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UNIT_CASH4.Font = new System.Drawing.Font("옥션고딕 L", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UNIT_CASH4.ForeColor = System.Drawing.Color.Black;
            this.lbl_UNIT_CASH4.Location = new System.Drawing.Point(562, 459);
            this.lbl_UNIT_CASH4.Name = "lbl_UNIT_CASH4";
            this.lbl_UNIT_CASH4.Size = new System.Drawing.Size(58, 41);
            this.lbl_UNIT_CASH4.TabIndex = 359;
            this.lbl_UNIT_CASH4.Tag = "UNIT_CASH";
            this.lbl_UNIT_CASH4.Text = "円";
            this.lbl_UNIT_CASH4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_UNIT_CASH2
            // 
            this.lbl_UNIT_CASH2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UNIT_CASH2.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UNIT_CASH2.Font = new System.Drawing.Font("옥션고딕 L", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UNIT_CASH2.ForeColor = System.Drawing.Color.Black;
            this.lbl_UNIT_CASH2.Location = new System.Drawing.Point(562, 363);
            this.lbl_UNIT_CASH2.Name = "lbl_UNIT_CASH2";
            this.lbl_UNIT_CASH2.Size = new System.Drawing.Size(58, 41);
            this.lbl_UNIT_CASH2.TabIndex = 358;
            this.lbl_UNIT_CASH2.Tag = "UNIT_CASH";
            this.lbl_UNIT_CASH2.Text = "円";
            this.lbl_UNIT_CASH2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_UNIT_CASH1
            // 
            this.lbl_UNIT_CASH1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_UNIT_CASH1.BackColor = System.Drawing.Color.Transparent;
            this.lbl_UNIT_CASH1.Font = new System.Drawing.Font("옥션고딕 B", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_UNIT_CASH1.ForeColor = System.Drawing.Color.Black;
            this.lbl_UNIT_CASH1.Location = new System.Drawing.Point(562, 315);
            this.lbl_UNIT_CASH1.Name = "lbl_UNIT_CASH1";
            this.lbl_UNIT_CASH1.Size = new System.Drawing.Size(58, 41);
            this.lbl_UNIT_CASH1.TabIndex = 357;
            this.lbl_UNIT_CASH1.Tag = "UNIT_CASH";
            this.lbl_UNIT_CASH1.Text = "원";
            this.lbl_UNIT_CASH1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_ELAPSEDTIME
            // 
            this.lbl_TXT_ELAPSEDTIME.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_ELAPSEDTIME.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_ELAPSEDTIME.Location = new System.Drawing.Point(30, 270);
            this.lbl_TXT_ELAPSEDTIME.Name = "lbl_TXT_ELAPSEDTIME";
            this.lbl_TXT_ELAPSEDTIME.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_ELAPSEDTIME.TabIndex = 356;
            this.lbl_TXT_ELAPSEDTIME.Tag = "";
            this.lbl_TXT_ELAPSEDTIME.Text = "경과시간";
            this.lbl_TXT_ELAPSEDTIME.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_INDATE
            // 
            this.lbl_TXT_INDATE.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_INDATE.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_INDATE.Location = new System.Drawing.Point(30, 222);
            this.lbl_TXT_INDATE.Name = "lbl_TXT_INDATE";
            this.lbl_TXT_INDATE.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_INDATE.TabIndex = 355;
            this.lbl_TXT_INDATE.Tag = "";
            this.lbl_TXT_INDATE.Text = "입차시간";
            this.lbl_TXT_INDATE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_TXT_CARNO
            // 
            this.lbl_TXT_CARNO.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TXT_CARNO.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TXT_CARNO.Location = new System.Drawing.Point(30, 174);
            this.lbl_TXT_CARNO.Name = "lbl_TXT_CARNO";
            this.lbl_TXT_CARNO.Size = new System.Drawing.Size(249, 35);
            this.lbl_TXT_CARNO.TabIndex = 354;
            this.lbl_TXT_CARNO.Tag = "TXT_CARNO";
            this.lbl_TXT_CARNO.Text = "차량번호";
            this.lbl_TXT_CARNO.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_DIscountMoney
            // 
            this.lbl_DIscountMoney.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DIscountMoney.Font = new System.Drawing.Font("옥션고딕 B", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DIscountMoney.ForeColor = System.Drawing.Color.Red;
            this.lbl_DIscountMoney.Location = new System.Drawing.Point(296, 366);
            this.lbl_DIscountMoney.Name = "lbl_DIscountMoney";
            this.lbl_DIscountMoney.Size = new System.Drawing.Size(261, 34);
            this.lbl_DIscountMoney.TabIndex = 353;
            this.lbl_DIscountMoney.Text = "3,000";
            this.lbl_DIscountMoney.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CarType
            // 
            this.lbl_CarType.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CarType.Font = new System.Drawing.Font("옥션고딕 B", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CarType.ForeColor = System.Drawing.Color.Red;
            this.lbl_CarType.Location = new System.Drawing.Point(639, 459);
            this.lbl_CarType.Name = "lbl_CarType";
            this.lbl_CarType.Size = new System.Drawing.Size(350, 46);
            this.lbl_CarType.TabIndex = 352;
            this.lbl_CarType.Text = "일반요금";
            this.lbl_CarType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_CarType.Visible = false;
            // 
            // lblCarnumber2
            // 
            this.lblCarnumber2.BackColor = System.Drawing.Color.Transparent;
            this.lblCarnumber2.Font = new System.Drawing.Font("옥션고딕 B", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarnumber2.ForeColor = System.Drawing.Color.Black;
            this.lblCarnumber2.Location = new System.Drawing.Point(637, 410);
            this.lblCarnumber2.Name = "lblCarnumber2";
            this.lblCarnumber2.Size = new System.Drawing.Size(350, 46);
            this.lblCarnumber2.TabIndex = 351;
            this.lblCarnumber2.Text = "가가58너.79911-";
            this.lblCarnumber2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pic_carimage
            // 
            this.pic_carimage.Location = new System.Drawing.Point(638, 174);
            this.pic_carimage.Name = "pic_carimage";
            this.pic_carimage.Size = new System.Drawing.Size(350, 230);
            this.pic_carimage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_carimage.TabIndex = 350;
            this.pic_carimage.TabStop = false;
            // 
            // lbl_Payment
            // 
            this.lbl_Payment.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Payment.Font = new System.Drawing.Font("옥션고딕 B", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Payment.ForeColor = System.Drawing.Color.Red;
            this.lbl_Payment.Location = new System.Drawing.Point(296, 459);
            this.lbl_Payment.Name = "lbl_Payment";
            this.lbl_Payment.Size = new System.Drawing.Size(261, 40);
            this.lbl_Payment.TabIndex = 349;
            this.lbl_Payment.Text = "999,999";
            this.lbl_Payment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.BackColor = System.Drawing.Color.Transparent;
            this.lblElapsedTime.Font = new System.Drawing.Font("옥션고딕 L", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElapsedTime.ForeColor = System.Drawing.Color.Black;
            this.lblElapsedTime.Location = new System.Drawing.Point(296, 268);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(324, 38);
            this.lblElapsedTime.TabIndex = 348;
            this.lblElapsedTime.Text = "1Day";
            this.lblElapsedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblParkingFee
            // 
            this.lblParkingFee.BackColor = System.Drawing.Color.Transparent;
            this.lblParkingFee.Font = new System.Drawing.Font("옥션고딕 B", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParkingFee.ForeColor = System.Drawing.Color.Olive;
            this.lblParkingFee.Location = new System.Drawing.Point(296, 317);
            this.lblParkingFee.Name = "lblParkingFee";
            this.lblParkingFee.Size = new System.Drawing.Size(261, 37);
            this.lblParkingFee.TabIndex = 347;
            this.lblParkingFee.Text = "999,999";
            this.lblParkingFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIndate
            // 
            this.lblIndate.BackColor = System.Drawing.Color.Transparent;
            this.lblIndate.Font = new System.Drawing.Font("옥션고딕 L", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIndate.ForeColor = System.Drawing.Color.Black;
            this.lblIndate.Location = new System.Drawing.Point(296, 224);
            this.lblIndate.Name = "lblIndate";
            this.lblIndate.Size = new System.Drawing.Size(324, 30);
            this.lblIndate.TabIndex = 346;
            this.lblIndate.Text = "2013-11-11 11:10";
            this.lblIndate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CarNumber
            // 
            this.lbl_CarNumber.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CarNumber.Font = new System.Drawing.Font("옥션고딕 L", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CarNumber.ForeColor = System.Drawing.Color.Black;
            this.lbl_CarNumber.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_CarNumber.Location = new System.Drawing.Point(296, 170);
            this.lbl_CarNumber.Name = "lbl_CarNumber";
            this.lbl_CarNumber.Size = new System.Drawing.Size(324, 43);
            this.lbl_CarNumber.TabIndex = 345;
            this.lbl_CarNumber.Text = "가가58너.79911-";
            this.lbl_CarNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDiscountCountName
            // 
            this.lblDiscountCountName.AutoSize = true;
            this.lblDiscountCountName.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscountCountName.Font = new System.Drawing.Font("옥션고딕 B", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountCountName.ForeColor = System.Drawing.Color.Red;
            this.lblDiscountCountName.Location = new System.Drawing.Point(641, 678);
            this.lblDiscountCountName.Name = "lblDiscountCountName";
            this.lblDiscountCountName.Size = new System.Drawing.Size(291, 35);
            this.lblDiscountCountName.TabIndex = 368;
            this.lblDiscountCountName.Text = "입수된할인권수량:";
            this.lblDiscountCountName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDiscountCountName.Visible = false;
            // 
            // lblDiscountInputCount
            // 
            this.lblDiscountInputCount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDiscountInputCount.BackColor = System.Drawing.Color.Transparent;
            this.lblDiscountInputCount.Font = new System.Drawing.Font("옥션고딕 B", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscountInputCount.ForeColor = System.Drawing.Color.Red;
            this.lblDiscountInputCount.Location = new System.Drawing.Point(919, 668);
            this.lblDiscountInputCount.Name = "lblDiscountInputCount";
            this.lblDiscountInputCount.Size = new System.Drawing.Size(81, 53);
            this.lblDiscountInputCount.TabIndex = 369;
            this.lblDiscountInputCount.Text = "0";
            this.lblDiscountInputCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDiscountInputCount.Visible = false;
            // 
            // lblRegExpireInfo
            // 
            this.lblRegExpireInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblRegExpireInfo.Font = new System.Drawing.Font("옥션고딕 B", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegExpireInfo.ForeColor = System.Drawing.Color.Red;
            this.lblRegExpireInfo.Location = new System.Drawing.Point(31, 540);
            this.lblRegExpireInfo.Name = "lblRegExpireInfo";
            this.lblRegExpireInfo.Size = new System.Drawing.Size(444, 30);
            this.lblRegExpireInfo.TabIndex = 370;
            this.lblRegExpireInfo.Tag = "TXT_REGEXTANSION";
            this.lblRegExpireInfo.Text = "정기권 연장기간(할부선택 아님)";
            this.lblRegExpireInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRegExpireInfo.Visible = false;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblErrorMessage.Font = new System.Drawing.Font("옥션고딕 B", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(34, 638);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(954, 30);
            this.lblErrorMessage.TabIndex = 367;
            // 
            // btnSixMonthAdd
            // 
            this.btnSixMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnSixMonthAdd.Location = new System.Drawing.Point(576, 585);
            this.btnSixMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSixMonthAdd.Name = "btnSixMonthAdd";
            this.btnSixMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnSixMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnSixMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnSixMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnSixMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnSixMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnSixMonthAdd.NPFontSize = 20F;
            this.btnSixMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnSixMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnSixMonthAdd.NPLanguageCode = "UNIT_SIXMONTHADD";
            this.btnSixMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnSixMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnSixMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnSixMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnSixMonthAdd.TabIndex = 376;
            this.btnSixMonthAdd.Text = "6개월";
            this.btnSixMonthAdd.UseVisualStyleBackColor = false;
            this.btnSixMonthAdd.Click += new System.EventHandler(this.btnSixMonthAdd_Click);
            // 
            // btnFiveMonthAdd
            // 
            this.btnFiveMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnFiveMonthAdd.Location = new System.Drawing.Point(468, 585);
            this.btnFiveMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFiveMonthAdd.Name = "btnFiveMonthAdd";
            this.btnFiveMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnFiveMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnFiveMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnFiveMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnFiveMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnFiveMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnFiveMonthAdd.NPFontSize = 20F;
            this.btnFiveMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnFiveMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnFiveMonthAdd.NPLanguageCode = "UNIT_FIVEMONTHADD";
            this.btnFiveMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnFiveMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnFiveMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnFiveMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnFiveMonthAdd.TabIndex = 375;
            this.btnFiveMonthAdd.Text = "5개월";
            this.btnFiveMonthAdd.UseVisualStyleBackColor = false;
            this.btnFiveMonthAdd.Click += new System.EventHandler(this.btnFiveMonthAdd_Click);
            // 
            // btnFourMonthAdd
            // 
            this.btnFourMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnFourMonthAdd.Location = new System.Drawing.Point(360, 585);
            this.btnFourMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFourMonthAdd.Name = "btnFourMonthAdd";
            this.btnFourMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnFourMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnFourMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnFourMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnFourMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnFourMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnFourMonthAdd.NPFontSize = 20F;
            this.btnFourMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnFourMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnFourMonthAdd.NPLanguageCode = "UNIT_FOURMONTHADD";
            this.btnFourMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnFourMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnFourMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnFourMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnFourMonthAdd.TabIndex = 374;
            this.btnFourMonthAdd.Text = "4개월";
            this.btnFourMonthAdd.UseVisualStyleBackColor = false;
            this.btnFourMonthAdd.Click += new System.EventHandler(this.btnFourMonthAdd_Click);
            // 
            // btnThreeMonthAdd
            // 
            this.btnThreeMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnThreeMonthAdd.Location = new System.Drawing.Point(252, 585);
            this.btnThreeMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThreeMonthAdd.Name = "btnThreeMonthAdd";
            this.btnThreeMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnThreeMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnThreeMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnThreeMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnThreeMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnThreeMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnThreeMonthAdd.NPFontSize = 20F;
            this.btnThreeMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnThreeMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnThreeMonthAdd.NPLanguageCode = "UNIT_THREEMONTHADD";
            this.btnThreeMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnThreeMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnThreeMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnThreeMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnThreeMonthAdd.TabIndex = 373;
            this.btnThreeMonthAdd.Text = "3개월";
            this.btnThreeMonthAdd.UseVisualStyleBackColor = false;
            this.btnThreeMonthAdd.Click += new System.EventHandler(this.btnThreeMonthAdd_Click);
            // 
            // btnTwoMonthAdd
            // 
            this.btnTwoMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnTwoMonthAdd.Location = new System.Drawing.Point(144, 585);
            this.btnTwoMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTwoMonthAdd.Name = "btnTwoMonthAdd";
            this.btnTwoMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnTwoMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnTwoMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnTwoMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnTwoMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnTwoMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnTwoMonthAdd.NPFontSize = 20F;
            this.btnTwoMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnTwoMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnTwoMonthAdd.NPLanguageCode = "UNIT_TWOMONTHADD";
            this.btnTwoMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnTwoMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnTwoMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnTwoMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnTwoMonthAdd.TabIndex = 372;
            this.btnTwoMonthAdd.Text = "2개월";
            this.btnTwoMonthAdd.UseVisualStyleBackColor = false;
            this.btnTwoMonthAdd.Click += new System.EventHandler(this.btnTwoMonthAdd_Click);
            // 
            // btnOneMonthAdd
            // 
            this.btnOneMonthAdd.Font = new System.Drawing.Font("옥션고딕 B", 20F);
            this.btnOneMonthAdd.Location = new System.Drawing.Point(36, 585);
            this.btnOneMonthAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOneMonthAdd.Name = "btnOneMonthAdd";
            this.btnOneMonthAdd.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnOneMonthAdd.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnOneMonthAdd.NPDisableBackColor = System.Drawing.Color.White;
            this.btnOneMonthAdd.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnOneMonthAdd.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnOneMonthAdd.NPDownForeColor = System.Drawing.Color.White;
            this.btnOneMonthAdd.NPFontSize = 20F;
            this.btnOneMonthAdd.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnOneMonthAdd.NPHoverForeColor = System.Drawing.Color.White;
            this.btnOneMonthAdd.NPLanguageCode = "UNIT_ONEMONTHADD";
            this.btnOneMonthAdd.NPNormalBackColor = System.Drawing.Color.White;
            this.btnOneMonthAdd.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnOneMonthAdd.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnOneMonthAdd.Size = new System.Drawing.Size(99, 49);
            this.btnOneMonthAdd.TabIndex = 371;
            this.btnOneMonthAdd.Text = "1개월";
            this.btnOneMonthAdd.UseVisualStyleBackColor = false;
            this.btnOneMonthAdd.Click += new System.EventHandler(this.btnOneMonthAdd_Click);
            // 
            // btn_TXT_CANCEL
            // 
            this.btn_TXT_CANCEL.Font = new System.Drawing.Font("옥션고딕 B", 35F);
            this.btn_TXT_CANCEL.Location = new System.Drawing.Point(406, 541);
            this.btn_TXT_CANCEL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_TXT_CANCEL.Name = "btn_TXT_CANCEL";
            this.btn_TXT_CANCEL.NPDefaultBackColor = System.Drawing.Color.White;
            this.btn_TXT_CANCEL.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btn_TXT_CANCEL.NPDisableBackColor = System.Drawing.Color.White;
            this.btn_TXT_CANCEL.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_CANCEL.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_CANCEL.NPDownForeColor = System.Drawing.Color.White;
            this.btn_TXT_CANCEL.NPFontSize = 35F;
            this.btn_TXT_CANCEL.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_CANCEL.NPHoverForeColor = System.Drawing.Color.White;
            this.btn_TXT_CANCEL.NPLanguageCode = "TXT_CASHCANCLE";
            this.btn_TXT_CANCEL.NPNormalBackColor = System.Drawing.Color.White;
            this.btn_TXT_CANCEL.NPNormalForeColor = System.Drawing.Color.Green;
            this.btn_TXT_CANCEL.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btn_TXT_CANCEL.Size = new System.Drawing.Size(231, 63);
            this.btn_TXT_CANCEL.TabIndex = 377;
            this.btn_TXT_CANCEL.Text = "현금취소";
            this.btn_TXT_CANCEL.UseVisualStyleBackColor = false;
            this.btn_TXT_CANCEL.Click += new System.EventHandler(this.btn_TXT_CANCEL_Click);
            // 
            // btnCardApproval
            // 
            this.btnCardApproval.Font = new System.Drawing.Font("옥션고딕 B", 35F);
            this.btnCardApproval.Location = new System.Drawing.Point(643, 571);
            this.btnCardApproval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCardApproval.Name = "btnCardApproval";
            this.btnCardApproval.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnCardApproval.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnCardApproval.NPDisableBackColor = System.Drawing.Color.White;
            this.btnCardApproval.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnCardApproval.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnCardApproval.NPDownForeColor = System.Drawing.Color.White;
            this.btnCardApproval.NPFontSize = 35F;
            this.btnCardApproval.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnCardApproval.NPHoverForeColor = System.Drawing.Color.White;
            this.btnCardApproval.NPLanguageCode = "TXT_NONE";
            this.btnCardApproval.NPNormalBackColor = System.Drawing.Color.White;
            this.btnCardApproval.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnCardApproval.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnCardApproval.Size = new System.Drawing.Size(338, 63);
            this.btnCardApproval.TabIndex = 378;
            this.btnCardApproval.Text = "신용카드결제";
            this.btnCardApproval.UseVisualStyleBackColor = false;
            this.btnCardApproval.Visible = false;
            // 
            // btn_TXT_BACK
            // 
            this.btn_TXT_BACK.Font = new System.Drawing.Font("옥션고딕 B", 24F);
            this.btn_TXT_BACK.Location = new System.Drawing.Point(34, 673);
            this.btn_TXT_BACK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_TXT_BACK.Name = "btn_TXT_BACK";
            this.btn_TXT_BACK.NPDefaultBackColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btn_TXT_BACK.NPDisableBackColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_BACK.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_BACK.NPDownForeColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.NPFontSize = 24F;
            this.btn_TXT_BACK.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_BACK.NPHoverForeColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.NPLanguageCode = "TXT_BACK";
            this.btn_TXT_BACK.NPNormalBackColor = System.Drawing.Color.White;
            this.btn_TXT_BACK.NPNormalForeColor = System.Drawing.Color.Green;
            this.btn_TXT_BACK.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btn_TXT_BACK.Size = new System.Drawing.Size(151, 61);
            this.btn_TXT_BACK.TabIndex = 379;
            this.btn_TXT_BACK.Text = "이전화면";
            this.btn_TXT_BACK.UseVisualStyleBackColor = false;
            this.btn_TXT_BACK.Click += new System.EventHandler(this.btn_TXT_BACK_Click);
            // 
            // btn_TXT_HOME
            // 
            this.btn_TXT_HOME.Font = new System.Drawing.Font("옥션고딕 B", 24F);
            this.btn_TXT_HOME.Location = new System.Drawing.Point(191, 673);
            this.btn_TXT_HOME.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_TXT_HOME.Name = "btn_TXT_HOME";
            this.btn_TXT_HOME.NPDefaultBackColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btn_TXT_HOME.NPDisableBackColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btn_TXT_HOME.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_HOME.NPDownForeColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.NPFontSize = 24F;
            this.btn_TXT_HOME.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btn_TXT_HOME.NPHoverForeColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.NPLanguageCode = "TXT_HOME";
            this.btn_TXT_HOME.NPNormalBackColor = System.Drawing.Color.White;
            this.btn_TXT_HOME.NPNormalForeColor = System.Drawing.Color.Green;
            this.btn_TXT_HOME.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btn_TXT_HOME.Size = new System.Drawing.Size(151, 61);
            this.btn_TXT_HOME.TabIndex = 380;
            this.btn_TXT_HOME.Text = "처음화면";
            this.btn_TXT_HOME.UseVisualStyleBackColor = false;
            this.btn_TXT_HOME.Click += new System.EventHandler(this.btn_TXT_HOME_Click);
            // 
            // btnEnglish
            // 
            this.btnEnglish.Font = new System.Drawing.Font("옥션고딕 M", 17F);
            this.btnEnglish.Location = new System.Drawing.Point(711, 682);
            this.btnEnglish.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEnglish.Name = "btnEnglish";
            this.btnEnglish.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnEnglish.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnEnglish.NPDisableBackColor = System.Drawing.Color.White;
            this.btnEnglish.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnEnglish.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnEnglish.NPDownForeColor = System.Drawing.Color.White;
            this.btnEnglish.NPFontSize = 17F;
            this.btnEnglish.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnEnglish.NPHoverForeColor = System.Drawing.Color.White;
            this.btnEnglish.NPLanguageCode = "";
            this.btnEnglish.NPNormalBackColor = System.Drawing.Color.White;
            this.btnEnglish.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnEnglish.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.M_FONT;
            this.btnEnglish.Size = new System.Drawing.Size(125, 38);
            this.btnEnglish.TabIndex = 381;
            this.btnEnglish.Text = "English";
            this.btnEnglish.UseVisualStyleBackColor = false;
            this.btnEnglish.Click += new System.EventHandler(this.btnEnglish_Click);
            // 
            // btnJapan
            // 
            this.btnJapan.Font = new System.Drawing.Font("옥션고딕 M", 20F);
            this.btnJapan.Location = new System.Drawing.Point(849, 682);
            this.btnJapan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnJapan.Name = "btnJapan";
            this.btnJapan.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnJapan.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnJapan.NPDisableBackColor = System.Drawing.Color.White;
            this.btnJapan.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnJapan.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnJapan.NPDownForeColor = System.Drawing.Color.White;
            this.btnJapan.NPFontSize = 20F;
            this.btnJapan.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnJapan.NPHoverForeColor = System.Drawing.Color.White;
            this.btnJapan.NPLanguageCode = "";
            this.btnJapan.NPNormalBackColor = System.Drawing.Color.White;
            this.btnJapan.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnJapan.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.M_FONT;
            this.btnJapan.Size = new System.Drawing.Size(125, 38);
            this.btnJapan.TabIndex = 382;
            this.btnJapan.Text = "日本語";
            this.btnJapan.UseVisualStyleBackColor = false;
            this.btnJapan.Click += new System.EventHandler(this.btnJapan_Click);
            // 
            // btnSamsunPay
            // 
            this.btnSamsunPay.Font = new System.Drawing.Font("옥션고딕 B", 35F);
            this.btnSamsunPay.Location = new System.Drawing.Point(711, 541);
            this.btnSamsunPay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSamsunPay.Name = "btnSamsunPay";
            this.btnSamsunPay.NPDefaultBackColor = System.Drawing.Color.White;
            this.btnSamsunPay.NPDefaultForeColor = System.Drawing.Color.Green;
            this.btnSamsunPay.NPDisableBackColor = System.Drawing.Color.White;
            this.btnSamsunPay.NPDisableForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(179)))), ((int)(((byte)(179)))));
            this.btnSamsunPay.NPDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnSamsunPay.NPDownForeColor = System.Drawing.Color.White;
            this.btnSamsunPay.NPFontSize = 35F;
            this.btnSamsunPay.NPHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(110)))), ((int)(((byte)(97)))));
            this.btnSamsunPay.NPHoverForeColor = System.Drawing.Color.White;
            this.btnSamsunPay.NPLanguageCode = "TXT_SAMSUNGPAY";
            this.btnSamsunPay.NPNormalBackColor = System.Drawing.Color.White;
            this.btnSamsunPay.NPNormalForeColor = System.Drawing.Color.Green;
            this.btnSamsunPay.NPUseFontStyle = FadeFox.UI.PropertyListBAS.ENUM_FONTSELECT.BOLD;
            this.btnSamsunPay.Size = new System.Drawing.Size(231, 63);
            this.btnSamsunPay.TabIndex = 383;
            this.btnSamsunPay.Text = "삼성페이";
            this.btnSamsunPay.UseVisualStyleBackColor = false;
            this.btnSamsunPay.Visible = false;
            this.btnSamsunPay.Click += new System.EventHandler(this.btnSamsunPay_Click);
            // 
            // Ctl4by3Payment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::NPAutoBooth.Properties.Resources.Type2BackGround;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btnSixMonthAdd);
            this.Controls.Add(this.btnCardApproval);
            this.Controls.Add(this.btnSamsunPay);
            this.Controls.Add(this.btn_TXT_BACK);
            this.Controls.Add(this.btn_TXT_HOME);
            this.Controls.Add(this.btnEnglish);
            this.Controls.Add(this.btnJapan);
            this.Controls.Add(this.btnFiveMonthAdd);
            this.Controls.Add(this.btnFourMonthAdd);
            this.Controls.Add(this.btnThreeMonthAdd);
            this.Controls.Add(this.btnTwoMonthAdd);
            this.Controls.Add(this.btnOneMonthAdd);
            this.Controls.Add(this.btn_TXT_CANCEL);
            this.Controls.Add(this.lblDiscountCountName);
            this.Controls.Add(this.lblDiscountInputCount);
            this.Controls.Add(this.lblRegExpireInfo);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.lbl_UNIT_CASH3);
            this.Controls.Add(this.lbl_TXT_PREFEE);
            this.Controls.Add(this.lbl_RecvMoney);
            this.Controls.Add(this.pic_Dot2);
            this.Controls.Add(this.lbl_TXT_PARKINGFEE);
            this.Controls.Add(this.lbl_TXT_DISCOUNTFEE);
            this.Controls.Add(this.lbl_TXT_AMOUNTFEE);
            this.Controls.Add(this.lbl_UNIT_CASH4);
            this.Controls.Add(this.lbl_UNIT_CASH2);
            this.Controls.Add(this.lbl_UNIT_CASH1);
            this.Controls.Add(this.lbl_TXT_ELAPSEDTIME);
            this.Controls.Add(this.lbl_TXT_INDATE);
            this.Controls.Add(this.lbl_TXT_CARNO);
            this.Controls.Add(this.lbl_DIscountMoney);
            this.Controls.Add(this.lbl_CarType);
            this.Controls.Add(this.lblCarnumber2);
            this.Controls.Add(this.pic_carimage);
            this.Controls.Add(this.lbl_Payment);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.lblParkingFee);
            this.Controls.Add(this.lblIndate);
            this.Controls.Add(this.lbl_CarNumber);
            this.Controls.Add(this.pic_Dot1);
            this.Controls.Add(this.lbl_MSG_PAYINFO);
            this.Controls.Add(this.lbl_MSG_DISCOUNTINFO);
            this.Controls.Add(this.panel_ConfigClick2);
            this.Controls.Add(this.panel_ConfigClick1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Ctl4by3Payment";
            this.Size = new System.Drawing.Size(1024, 768);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Dot2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_carimage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Dot1;
        private System.Windows.Forms.Label lbl_MSG_PAYINFO;
        private System.Windows.Forms.Label lbl_MSG_DISCOUNTINFO;
        private System.Windows.Forms.Panel panel_ConfigClick2;
        private System.Windows.Forms.Panel panel_ConfigClick1;
        private System.Windows.Forms.Label lbl_UNIT_CASH3;
        private System.Windows.Forms.Label lbl_TXT_PREFEE;
        private System.Windows.Forms.Label lbl_RecvMoney;
        private System.Windows.Forms.PictureBox pic_Dot2;
        private System.Windows.Forms.Label lbl_TXT_PARKINGFEE;
        private System.Windows.Forms.Label lbl_TXT_DISCOUNTFEE;
        private System.Windows.Forms.Label lbl_TXT_AMOUNTFEE;
        private System.Windows.Forms.Label lbl_UNIT_CASH4;
        private System.Windows.Forms.Label lbl_UNIT_CASH2;
        private System.Windows.Forms.Label lbl_UNIT_CASH1;
        private System.Windows.Forms.Label lbl_TXT_ELAPSEDTIME;
        private System.Windows.Forms.Label lbl_TXT_INDATE;
        private System.Windows.Forms.Label lbl_TXT_CARNO;
        private System.Windows.Forms.Label lbl_DIscountMoney;
        private System.Windows.Forms.Label lbl_CarType;
        private System.Windows.Forms.Label lblCarnumber2;
        private System.Windows.Forms.PictureBox pic_carimage;
        private System.Windows.Forms.Label lbl_Payment;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Label lblParkingFee;
        private System.Windows.Forms.Label lblIndate;
        private System.Windows.Forms.Label lbl_CarNumber;
        private System.Windows.Forms.Label lblDiscountCountName;
        private System.Windows.Forms.Label lblDiscountInputCount;
        private System.Windows.Forms.Label lblRegExpireInfo;
        private System.Windows.Forms.Label lblErrorMessage;
        private FadeFox.UI.NPButton btnSixMonthAdd;
        private FadeFox.UI.NPButton btnFiveMonthAdd;
        private FadeFox.UI.NPButton btnFourMonthAdd;
        private FadeFox.UI.NPButton btnThreeMonthAdd;
        private FadeFox.UI.NPButton btnTwoMonthAdd;
        private FadeFox.UI.NPButton btnOneMonthAdd;
        private FadeFox.UI.NPButton btn_TXT_CANCEL;
        private FadeFox.UI.NPButton btnCardApproval;
        private FadeFox.UI.NPButton btn_TXT_BACK;
        private FadeFox.UI.NPButton btn_TXT_HOME;
        private FadeFox.UI.NPButton btnEnglish;
        private FadeFox.UI.NPButton btnJapan;
        private FadeFox.UI.NPButton btnSamsunPay;
    }
}
