using FadeFox.Text;
using FadeFox.UI;
using NPAutoBooth.Common;
using NPAutoBooth.UI.BoothUC;
using NPCommon;
using NPCommon.DEVICE;
using NPCommon.DTO;
using NPCommon.REST;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    public partial class FormCreditRecipt : Form, ISubForm
    {

        private int inputtime = NPSYS.SettingReceiptTimeValue;
        public NormalCarInfo mCurrentNormalCarInfo = null;
        public bool isPlayerOkStatus = true;
        private HttpProcess mHttpProcess = new HttpProcess();


        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Receipt;

        public delegate void mExitReceiptForm(NPSYS.FormType pFormType);
        ///// <summary>
        ///// 고객이 처음으로 등을 눌러 종료시
        ///// </summary>
        //public event mExitReceiptForm EventExitReceiptForm;
        /// <summary>
        /// 고객이 처음으로 등을 눌러 종료시
        /// </summary>
        public event ChangeView EventExitReceiptForm;
        public event EventHandlerAddCtrl OnAddCtrl;

        private FormCreditInfomation mFormCreditInfomation = new FormCreditInfomation();
        private ReciptUC boothRecipt;

        #region 폼생성 관련
        public FormCreditRecipt()
        {
            InitializeComponent();

            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
                boothRecipt = FormFactory.GetInstance().GetDesignControl<ReciptUC>(BoothCommonLib.ClientAreaRate._4vs3);
            }
            //1080해상도 적용
            else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                this.ClientSize = new System.Drawing.Size(1080, 1920);
                boothRecipt = FormFactory.GetInstance().GetDesignControl<ReciptUC>(BoothCommonLib.ClientAreaRate._9vs16);
            }

            this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);

        }

        private void FormRecipt_Load(object sender, EventArgs e)
        {
            try
            {
                Invoke(OnAddCtrl); //컨트롤 추가
                axWindowsMediaPlayer1.SendToBack();
                axWindowsMediaPlayer1.Visible = false;
                //1080해상도 적용 완료
                this.Location = new Point(0, 0);

                SetControl();
                SetLanguage(NPSYS.CurrentLanguageType);
                SettingEnableEvent();
                TextCore.INFO(TextCore.INFOS.MEMORY, "FormRecipt|FormRecipt_Load", "영수증 화면 로드|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormRecipt|FormRecipt_Load", ex.ToString());
            }

        }

        void AddCtrl()
        {
            this.Controls.Add((Control)boothRecipt);
            boothRecipt.Dock = DockStyle.Fill;
            boothRecipt.BringToFront();
            boothRecipt.Initialize();
        }

        private void FormRecipt_Shown(object sender, EventArgs e)
        {

        }
        private void SetControl()
        {
            boothRecipt.Recipt_Click += this.btn_Recipt_Click;
            boothRecipt.Cancle_Click += this.btn_Cancle_Click;

            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                //pic_titleInfomationPrint.Visible = true;  //뉴타입주석
                axWindowsMediaPlayer1.Visible = false;
                axWindowsMediaPlayer1.SendToBack();
            }
            else
            {
                //pic_titleInfomationPrint.Visible = false;  //뉴타입주석
            }
        }

        #endregion


        #region 폼활성화 / 종료시 이벤트 
        private void SettingEnableEvent()
        {
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
            {

                NPSYS.Device.DoSensors.DosensorSignalEvent += new GoodTechContorlBoard.SignalEvent(DoSensors_DosensorSignalEvent);
            }
            axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent += new EventHandler(player_ErrorEvent);

        }

        private void SettingDisableEvent()
        {
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
            {

                NPSYS.Device.DoSensors.DosensorSignalEvent -= new GoodTechContorlBoard.SignalEvent(DoSensors_DosensorSignalEvent);
            }
            axWindowsMediaPlayer1.MediaError -= new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent -= new EventHandler(player_ErrorEvent);

        }
        #endregion

        #region 동영상 관련 이벤트처리

        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|Player_MediaError", "플레이어오류:" + e.ToString());
                isPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|Player_MediaError", ex.ToString());
            }
        }

        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {
                // Get the description of the first error. 
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|player_ErrorEvent", "에러내용" + errDesc);
                isPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|player_ErrorEvent", ex.ToString());
            }
        }

        #endregion

        #region 언어변경
        /// <summary>
        /// 언어변경
        /// </summary>
        public void SetLanguage(NPCommon.ConfigID.LanguageType pLanguageType)
        {




            Control[] currentControl = GetAllControlsUsingRecursive(this);
            foreach (Control controlItem in currentControl)
            {

                switch (controlItem)
                {
                    case Label labelType:
                        if (labelType.Tag != null && labelType.Tag.ToString().Trim().Length > 0)
                        {

                            labelType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, labelType.Tag.ToString());
                        }
                        break;
                    case ImageButton imageButtonType:
                        if (imageButtonType.Tag != null && imageButtonType.Tag.ToString().Trim().Length > 0)
                        {
                            imageButtonType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, imageButtonType.Tag.ToString());
                        }
                        break;
                    case TextBox textBoxType:
                        if (textBoxType.Tag != null && textBoxType.Tag.ToString().Trim().Length > 0)
                        {
                            textBoxType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, textBoxType.Tag.ToString());
                        }
                        break;
                    case Button buttoType:
                        if (buttoType.Tag != null && buttoType.Tag.ToString().Trim().Length > 0)
                        {
                            buttoType.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.transaction, buttoType.Tag.ToString());
                        }
                        break;


                }

            }
            SetLanuageDynamic(pLanguageType);



        }

        private void SetLanuageDynamic(NPCommon.ConfigID.LanguageType pLanguageType)
        {
            //if (NPSYS.gIsAutoBooth)
            //{
            //    lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME1.ToString());
            //}
            //else
            //{
            //    lbl_TITLE_MAINNAME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_MACHINNAME2.ToString());
            //}
        }

        private Control[] GetAllControlsUsingRecursive(Control containerControl)

        {

            List<Control> allControls = new List<Control>();



            foreach (Control control in containerControl.Controls)
            {
                //자식 컨트롤을 컬렉션에 추가한다

                allControls.Add(control);

                //만일 자식 컨트롤이 또 다른 자식 컨트롤을 가지고 있다면…

                if (control.Controls.Count > 0)

                {

                    //자신을 재귀적으로 호출한다

                    allControls.AddRange(GetAllControlsUsingRecursive(control));

                }

            }

            //모든 컨트롤을 반환한다

            return allControls.ToArray();

        }

        #endregion


        private void btn_Cancle_Click(object sender, EventArgs e)
        {
            try
            {

                NPSYS.buttonSoundDingDong();

                CancleAction();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|btn_Cancle_Click", "예외사항:" + ex.ToString());
            }

        }
        private void CancleAction()
        {
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|btn_Cancle_Click", "처음으로 버튼누름");
            EventExitReceiptForm(mCurrentFormType);
            return;
        }


        private void Payment()
        {

            try
            {
                InputTimer.Enabled = true;
                reciptPrintyn = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|Payment", ex.ToString());
            }

        }


        /// <summary>
        /// 잔돈 방출를 끝낸후에 동작
        /// </summary>
        /// <param name="db"></param>
        /// <param name="currentDate"></param>
        private void JobBeforePayment(DateTime currentDate)
        {
            try
            {
                //if (mCurrentNormalCarInfo.isBillUse())
                //{
                    
                //    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditRecipt | JobBeforePayment", "결제처리 전송시작");
                //    DateTime paydate = DateTime.Now;
                //    ParkingReceiveData.payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                //    if (currentCar.status.Success == false)
                //    {

                //        // 재전송 처리
                //        return;
                //    }

                //    NPSYS.NoCheckCargeMoneyOut();

                //}
                if (mCurrentNormalCarInfo.Current_Money > 0)
                {
                    int l_Confirmoney = mCurrentNormalCarInfo.Current_Money - mCurrentNormalCarInfo.GetChargeMoney;

                }
                string p_IO_TYPE = "";
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCurrentNormalCarInfo.GetNotDisChargeMoney == 0)
                {

                    p_IO_TYPE = "CASH";

                }
                else if (mCurrentNormalCarInfo.Current_Money > 0 && mCurrentNormalCarInfo.GetNotDisChargeMoney > 0)
                {

                    p_IO_TYPE = "CASH";

                }
                else if (mCurrentNormalCarInfo.TMoneyPay > 0)
                {
                    p_IO_TYPE = MoneyType.TmoneyCard.ToString();
                }
                else if (mCurrentNormalCarInfo.VanAmt > 0)
                {
                    p_IO_TYPE = MoneyType.CreditCard.ToString();
                }
                else
                {
                    p_IO_TYPE = "CASH";

                }

                LPRDbSelect.Car_Log_INsert(mCurrentNormalCarInfo.OutCarNo1, mCurrentNormalCarInfo.ReceiveMoney, Car_Type.NORMAL, p_IO_TYPE);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|JobBeforePayment", ex.ToString());
            }
        }


        /// <summary>
        /// 거스름돈에 대한 배출 가능 검사 // 거스름돈 0일떄 필요없고
        /// </summary>
        /// <returns></returns>
        public bool CheckChargeMoneyOut()
        {
            try
            {
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
                int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
                int cash50MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MinQty));
                int cash100MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MinQty));
                int cash500MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MinQty));
                int cash1000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MinQty));
                int cash5000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MinQty));

                TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|CheckChargeMoneyOut", "거스름검사시작 여유액: 5000원수량:" + cash5000SettingQty.ToString() + "  1000원수량:" + cash1000SettingQty.ToString() + "  500원수량:" + cash500SettingQty.ToString() + "  100원수량:" + cash100SettingQty.ToString() + "  50원수량:" + cash50SettingQty.ToString());
                // 동전에 대한 거스름돈 이 없으면 무조건 거스름돈 모지람으로 리턴시킴.
                if ((cash5000SettingQty >= mCurrentNormalCarInfo.Charge5000Qty) &&
                    (cash1000SettingQty >= mCurrentNormalCarInfo.Charge1000Qty) &&
                    (cash500SettingQty >= mCurrentNormalCarInfo.Charge500Qty) &&
                    (cash100SettingQty >= mCurrentNormalCarInfo.Charge100Qty) &&
                    (cash50SettingQty >= mCurrentNormalCarInfo.Charge50Qty))
                {
                    return true;
                }

                if (cash5000SettingQty < mCurrentNormalCarInfo.Charge5000Qty)  // 5000원권이 부죽한 경우 1000원권으로 넘김
                {

                    int lack5000Qty = mCurrentNormalCarInfo.Charge5000Qty - cash5000SettingQty;  // 5000원권의 부족한 수량

                    mCurrentNormalCarInfo.Charge5000Qty = cash5000SettingQty; // 현재 남아있는 5000원권 수량을 거스름돈으로 대체
                    mCurrentNormalCarInfo.Charge1000Qty = mCurrentNormalCarInfo.Charge1000Qty + (lack5000Qty * 5); // 부족한 5000원 수량을 1000원권 수량에 더함
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|CheckChargeMoneyOut", "거스름돈 반환시 지폐부족:5000원권 부족으로 1000원권으로 지폐확인-현보유5000수량:" + cash5000SettingQty.ToString() + " 1000원수량:" + mCurrentNormalCarInfo.Charge1000Qty.ToString());
                }

                if (cash1000SettingQty < mCurrentNormalCarInfo.Charge1000Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                {

                    int lack1000Qty = mCurrentNormalCarInfo.Charge1000Qty - cash1000SettingQty; // 1000원권의 부족한 수량

                    mCurrentNormalCarInfo.Charge1000Qty = cash1000SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                    mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (lack1000Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|CheckChargeMoneyOut", "거스름돈 반환시 지폐부족:1000원권 부족으로 500원권으로 지폐확인-현보유1000수량:" + cash1000SettingQty.ToString() + " 500원수량:" + mCurrentNormalCarInfo.Charge500Qty.ToString());
                }

                if (cash500SettingQty < mCurrentNormalCarInfo.Charge500Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                {

                    int lack500Qty = mCurrentNormalCarInfo.Charge500Qty - cash500SettingQty; // 1000원권의 부족한 수량

                    mCurrentNormalCarInfo.Charge500Qty = cash500SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                    mCurrentNormalCarInfo.Charge100Qty = mCurrentNormalCarInfo.Charge100Qty + (lack500Qty * 5); // 부족한 1000원 수량을 500원 수량에 더함
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|CheckChargeMoneyOut", "거스름돈 반환시 지폐부족:500원권 부족으로 100원권으로 지폐확인-현보유500수량:" + cash500SettingQty.ToString() + " 100원수량:" + mCurrentNormalCarInfo.Charge100Qty.ToString());

                }

                if (cash100SettingQty < mCurrentNormalCarInfo.Charge100Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                {
                    int lack100Qty = mCurrentNormalCarInfo.Charge100Qty - cash100SettingQty; // 1000원권의 부족한 수량

                    mCurrentNormalCarInfo.Charge100Qty = cash100SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                    mCurrentNormalCarInfo.Charge50Qty = mCurrentNormalCarInfo.Charge100Qty + (lack100Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|CheckChargeMoneyOut", "거스름돈 반환시 지폐부족:100원권 부족으로 50원권으로 동전교환-현보유100수량:" + cash100SettingQty.ToString() + " 50원수량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());

                }


                return true;



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|CheckChargeMoneyOut", ex.ToString());
                return false;
            }
        }


        private void CheckSettingCashQty()
        {
            try
            {
                string temp = "";

                // 보유 수량
                temp = NPSYS.Config.GetValue(ConfigID.Cash50SettingQty);
                int S50 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash100SettingQty);
                int S100 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash500SettingQty);
                int S500 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty);
                int S1000 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty);
                int S5000 = (temp != "" ? Convert.ToInt32(temp) : 0);


                // 최소 수량
                temp = NPSYS.Config.GetValue(ConfigID.Cash50MinQty);
                int M50 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash100MinQty);
                int M100 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash500MinQty);
                int M500 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash1000MinQty);
                int M1000 = (temp != "" ? Convert.ToInt32(temp) : 0);

                temp = NPSYS.Config.GetValue(ConfigID.Cash5000MinQty);
                int M5000 = (temp != "" ? Convert.ToInt32(temp) : 0);

                if (S50 < M50)
                {
                    //    //NPSYS.SendMessageServerErrorCode(this.Name, "A002");
                }

                if (S100 < M100)
                {
                    //   //NPSYS.SendMessageServerErrorCode(this.Name, "A003");
                }

                if (S500 < M500)
                {
                    //   //NPSYS.SendMessageServerErrorCode(this.Name, "A004");
                }

                if (S1000 < M1000)
                {
                    //   //NPSYS.SendMessageServerErrorCode(this.Name, "A005");
                }

                if (S5000 < M5000)
                {
                    //   //NPSYS.SendMessageServerErrorCode(this.Name, "A006");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "Formrecipt|CheckSettingCashQty", ex.ToString());
            }
        }


        enum PaymentResult
        {
            Success = 0,
            Fail = 1,
            ChargeSmall = 2
        }


        private void ReceiptErrorActions()
        {
            try
            {

                LPRDbSelect.LogMoney(PaymentType.CashTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mCurrentNormalCarInfo, MoneyType.CashTicket, 0, mCurrentNormalCarInfo.GetNotDisChargeMoney, "보관증");
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|ReceiptErrorActions", "보관증발행액:" + mCurrentNormalCarInfo.GetNotDisChargeMoney);
                Print.CashTicketNotCoinPrint(mCurrentNormalCarInfo, true, this.Name);
                if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
                {
                    mFormCreditInfomation.SetCurrentStatus(InfoStatus.NotEnoghfMoney);
                    mFormCreditInfomation.ShowDialog();
                }
                System.Threading.Thread.Sleep(500);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormRecipt|ReceiptErrorActions", "보관증발행후 메인으로 이돈");
                EventExitReceiptForm(mCurrentFormType);
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|ReceiptErrorActions", "예외사항:" + ex.ToString());
            }

        }




        private void PlayVideos()
        {
            try
            {

                // axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Movie\동영상-영수증선택.avi";
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_ReceitPrintInfo.ToString());

                axWindowsMediaPlayer1.uiMode = "none";
                if (isPlayerOkStatus == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|PlayVideos", ex.ToString());
            }


        }

        private void PlayVideosCharge()
        {
            try
            {

                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Movie\동영상-거스름돈.avi";
                axWindowsMediaPlayer1.uiMode = "none";
                if (isPlayerOkStatus == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|PlayVideos", ex.ToString());
            }


        }


        private void btn_Recipt_Click(object sender, EventArgs e)
        {
            if (reciptPrintyn == false)
            {
                return;
            }
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|btn_Recipt_Click", "화면상 영수증 출력버튼누름");
            ReceiptPrintAction();
        }



        private void FormRecipt_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
  
                //try
                //{
                    //SettingDisableEvent();
                    //axWindowsMediaPlayer1.Ctlcontrols.stop();
                    //axWindowsMediaPlayer1.close();
       
                //}
                //catch (Exception ex)
                //{
                //    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|FormRecipt_FormClosed", ex.ToString());
                //}
            

        
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormRecipt|FormRecipt_FormClosed", "작업종료");
                //this.Close();
                //if (this != null)
                //{
                //    this.Dispose();
                //}



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|FormRecipt_FormClosed", ex.ToString());
            }

        }

        private void timerInput_Tick(object sender, EventArgs e)
        {
            try
            {
                if (inputtime < 0)
                {
                   TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|timerInput_Tick", "시간초과로 메인화면으로 이동");
                    EventExitReceiptForm(mCurrentFormType);
                    return;

                }
                inputtime -= 1000;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|timerInput_Tick", "예외사항:" + ex.ToString());
            }

        }


        bool reciptPrintyn = false;
        private void ReceiptPrintAction()
        {
            try
            {
                if (reciptPrintyn == false)
                {
                    return;
                }
                NPSYS.buttonSoundDingDong();

                NPSYS.RecipePrint(mCurrentNormalCarInfo, this.Name);
                EventExitReceiptForm(mCurrentFormType);
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|ReceiptPrintAction", "예외사항:" + ex.ToString());
            }
        }

        /// <summary>
        /// 이벤트방식 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void DoSensors_DosensorSignalEvent(object sender, GoodTechContorlBoard.SignalType p_SignalType)
        {
            try
            {
                if (this.Visible == false)
                {
                    return;
                }
                if (p_SignalType == GoodTechContorlBoard.SignalType.ReciptSignal)
                {
                    //if (btn_Recipt.Visible == false)
                    //{
                    //    TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|ReceiptButton_Siganl_Event", "영수증 장비버튼 눌렀지만 아직 영수증출력 차례가 아니라서 무시");
                    //    return;
                    //}
                    if (reciptPrintyn == false)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|ReceiptButton_Siganl_Event", "영수증 장비버튼 눌렀지만 아직 영수증출력 차례가 아니라서 무시");
                        return;
                    }
                    
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|ReceiptButton_Siganl_Event", "영수증 장비버튼 누름");
                    
                    TextCore.ACTION(TextCore.ACTIONS.RECIPT, "FormRecipt|ReceiptButton_Siganl_Event", "영수증출력");

                    NPSYS.buttonSoundDingDong();

                    if (NPSYS.gUsePreFreeCarNoRecipt && mCurrentNormalCarInfo.VanAmt == 0 && mCurrentNormalCarInfo.Current_Money == 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormRecipt|ReceiptButton_Siganl_Event", "영수증 장비버튼 눌렀지만 무료차량이라서 영수증출력 안함");
                        return;
                    }
                    else
                    {
                        NPSYS.RecipePrint(mCurrentNormalCarInfo, this.Name);
                    }

                    EventExitReceiptForm(mCurrentFormType);
                    return;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|DoSensors_DosensorSignalEvent", "예외사항:" + ex.ToString());
            }

        }


        private void btn_CancleReceipt_Click(object sender, EventArgs e)
        {
            try
            {

                NPSYS.buttonSoundDingDong();

                CancleAction();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|btn_Cancle_Click", "예외사항:" + ex.ToString());
            }
        }

        public void OpenView<T>(NPSYS.FormType pFormType, T param)
        {
            if (this.Visible == false)
            {
                NormalCarInfo normalCarInfo = param.To<NormalCarInfo>();
                inputtime = NPSYS.SettingReceiptTimeValue;
                ;
                NPSYS.CurrentFormType = mCurrentFormType;
                mPreFomrType = pFormType;
                PlayVideos();
                int i = 0;
                while (true)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    i++;
                    if (i > 2)
                    {
                        break;
                    }

                }
                this.Show();
                this.Activate();
                mCurrentNormalCarInfo = CommonFuction.Clone<NormalCarInfo>(normalCarInfo);
                Payment();

                TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditRecipt | OpenView", "차량선택화면로드됨|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }

        public void CloseView()
        {
            if (this.Visible)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                InputTimer.Enabled = false;

                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditRecipt | CloseView", "영수증 화면종료됨");
            }
        }

        public void CloseViewBeforeInfo()
        {
            //Do nothing.
        }

        public void OpenViewBeforeInfo(NPSYS.FormType pFormType)
        {
            //Do nothing.
        }

    }
}
