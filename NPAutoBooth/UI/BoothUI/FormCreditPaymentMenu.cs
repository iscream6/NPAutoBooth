﻿using FadeFox;
using FadeFox.Text;
using FadeFox.UI;
using NPAutoBooth.Common;
using NPAutoBooth.UI.BoothUC;
using NPCommon;
using NPCommon.DEVICE;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.REST;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using static NPAutoBooth.UI.BoothUC.PaymentUC;

namespace NPAutoBooth.UI
{
    public partial class FormCreditPaymentMenu : Form, ISubForm
    {
        #region Member Fields

        /// <summary>
        /// Design 추가 이벤트 핸들러(필수)
        /// </summary>
        public event EventHandlerAddCtrl OnAddCtrl;
        public bool mIsPlayerOkStatus = true;
        private HttpProcess mHttpProcess = new HttpProcess();
        private PayCardandCash m_PayCardandCash = new PayCardandCash();

        private SmartroVCat mSmartroVCat = new SmartroVCat();  // 스마트로 추가
        private Smatro_TITDIP_EVCAT mSmartro_TITDIP_EVCat = new Smatro_TITDIP_EVCAT();
        private KIS_TITDIPDevice mKIS_TITDIPDevice = new KIS_TITDIPDevice();
        private CardDeviceStatus mCardStatus = new CardDeviceStatus();

        private ManualResetEvent mCreditCardThreadLock = new ManualResetEvent(true);

        private string mCurrentMovieName = string.Empty; // 2016-03-17 카드관련 동영상 떄문에 추가
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Payment;

        //바코드할인 리스트로변경
        private List<string> mListBarcodeData = new List<string>();
        //바코드할인 리스트로변경 주석처리

        //바코드모터드리블 사용
        private List<BarcodeMoter.BarcodeMotorResult> mListBarcodeMotorData = new List<BarcodeMoter.BarcodeMotorResult>();
        //바코드모터드리블 사용완료

        private PaymentUC paymentControl;

        private int paymentInputTimer = (NPSYS.PaymentInsertInfinite == true ? 12000000 : NPSYS.SettingInputTimeValue);
        private int MovieStopPlay = -1000;
        private bool IsNextFormPlaying = false;

        private System.Windows.Forms.Timer timerSmatro_TITDIP_Evcat = new System.Windows.Forms.Timer();

        public NormalCarInfo CurrentNormalCarInfo { get; set; } = new NormalCarInfo();

        #endregion

        #region 폼이동 이벤트

        /// <summary>
        /// 고객이 처음으로 등을 눌러 종료시
        /// </summary>
        public event ChangeView EventExitPayForm;

        /// <summary>
        /// 고객이 차량을 찾아 다음 요금으로 넘어가야할시 이벤트
        /// </summary>
        public event ChangeView<NormalCarInfo> EventExitPayForm_NextReceiptForm;

        /// <summary>
        /// 정보 메시지 창으로 넘어가는 이벤트
        /// </summary>
        public event ChangeView<InfoStatus> EventExitPayForm_NextInfo;

        #endregion

        #region 생성자

        public FormCreditPaymentMenu()
        {
            InitializeComponent();

            if (paymentControl == null)
            {
                if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
                {
                    this.ClientSize = new System.Drawing.Size(1024, 768);
                    paymentControl = FormFactory.GetInstance().GetDesignControl<PaymentUC>(BoothCommonLib.ClientAreaRate._4vs3);
                }
                else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
                {
                    this.ClientSize = new System.Drawing.Size(1080, 1920);
                    paymentControl = FormFactory.GetInstance().GetDesignControl<PaymentUC>(BoothCommonLib.ClientAreaRate._9vs16);
                }

                this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);
            }
        }

        #endregion

        #region 폼로딩시

        private void FormPaymentMenu_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            Invoke(OnAddCtrl); //컨트롤 추가
            InitializeControl();
            SetLanguage(NPSYS.CurrentLanguageType);
            axWindowsMediaPlayer1.SendToBack();

            SettingEnableEvent();
        }

        /// <summary>
        /// Event를 설정한다.
        /// </summary>
        private void SettingEnableEvent()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingDisableEvent", "[장비이벤트 활성화]");
            try
            {
                paymentControl.ConfigCall += Close_Callback;
                paymentControl.PreForm_Click += btn_PrePage_Click;
                paymentControl.Home_Click += btn_home_Click;
                paymentControl.SeasonCarAddMonth += SetNextRegExipire;
                paymentControl.CashCancel_Click += btn_CashCancle_Click;
                paymentControl.SamsungPay_Click += btnSamSungPay_Click;

                axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
                axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
                axWindowsMediaPlayer1.ErrorEvent += new EventHandler(player_ErrorEvent);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|FormPaymentMenu", ex.ToString());
            }

            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeSerials.EventBarcode += new BarcodeSerial.BarcodeEvent(BarcodeSerials_EventBarcode);
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeMoter.EventAutoRedingData += new BarcodeMoter.GetAutoRedingData(BarcodeMotorSerials_EventBarcode);
            }

            if (NPSYS.Device.gIsUseSinbunReader)
            {
                NPSYS.Device.SinbunReader.readEvent += SinbunProcess; // 이벤트
            }

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {
                mSmartroVCat.VCatIp = NPSYS.gVanIp;
                mSmartroVCat.VCatPort = Convert.ToInt32(NPSYS.gVanPort);
                NPSYS.Device.SmtSndRcv.OnRcvState += new AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEventHandler(SmtSndRcv_OnRcvState);
                NPSYS.Device.SmtSndRcv.OnTermComplete += new EventHandler(SmtSndRcv_OnTermComplete);
                NPSYS.Device.SmtSndRcv.OnTermExit += new EventHandler(SmtSndRcv_OnTermExit);
            }
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;
            }
        }

        /// <summary>
        /// Form Design 컨트롤을 생성한다.
        /// </summary>
        void AddCtrl()
        {
            this.Controls.Add((Control)paymentControl);
            paymentControl.Dock = DockStyle.Fill;
            paymentControl.BringToFront();
        }

        /// <summary>
        /// 화면 컨트롤을 초기화 한다.
        /// </summary>
        private void InitializeControl()
        {
            paymentControl.Initialize();

            if (!NPSYS.isBoothRealMode) groupTest.Visible = true;

            if (NPSYS.gUseMultiLanguage) paymentControl.ForeignLanguageVisible(true);
            else paymentControl.ForeignLanguageVisible(false);

            //picWait 화면을 부모폼 중앙에 위치시켜야 겠다.
            //picWait의 Location 위치를 잡자. 잡는 방법은
            //부모폼의 Location은 0,0 이므로 부모폼의 Width 에서 picWait의 Width를 뺀 값의 반이 X좌표
            //Y좌표 또한 X좌표 구하는 방법과 동일하다.
            pic_Wait_MSG_WAIT.Location = new Point
            {
                X = (this.Width - pic_Wait_MSG_WAIT.Width) / 2,
                Y = (this.Height - pic_Wait_MSG_WAIT.Height) / 2,
            };
        }

        /// <summary>
        /// 카드만 사용해야 하는 상황인지 확인
        /// </summary>
        /// <returns></returns>
        public bool isOnlyCard()
        {
            switch (CurrentNormalCarInfo.CurrentCarPayStatus)
            {
                case NormalCarInfo.CarPayStatus.Reg_Outcar_Season:
                    return true;
                case NormalCarInfo.CarPayStatus.Reg_Precar_Season:
                    return true;
                case NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar:
                    return true;
                case NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar:
                    return true;
                default:
                    return false;

            }
        }

        /// <summary>
        /// 획득한 차량정보 및 출차시간을 가지고 요금 등을 계산하고 화면표시
        /// </summary>
        /// <param name="pPrevNormalCarInfo"></param>
        public void SetCarInfo(NormalCarInfo pPrevNormalCarInfo)
        {
            CurrentNormalCarInfo = pPrevNormalCarInfo;
            paymentControl.SetCarInfo(CurrentNormalCarInfo);
        }

        #endregion

        #region 폼 컨트롤 이벤트 처리

        /// <summary>
        /// 처음화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_home_Click(object sender, EventArgs e)
        {
            try
            {
                NPSYS.buttonSoundDingDong();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | btn_home_Click", "고객이 홈버튼 누름");
                if (CurrentNormalCarInfo.ListDcDetail.Count > 0)
                {
                    foreach (DcDetail detailDcDetail in CurrentNormalCarInfo.ListDcDetail)
                    {
                        if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                        {
                            mHttpProcess.DiscountCancle(CurrentNormalCarInfo, detailDcDetail);
                            detailDcDetail.UseYn = false;
                        }
                    }
                }
                EventExitPayForm(mCurrentFormType);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|btn_home_Click", ex.ToString());
            }
        }

        /// <summary>
        /// 이전화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PrePage_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | btn_PrePage_Click", "이전 버튼 클릭");
            if (CurrentNormalCarInfo.Current_Money > 0)
            {
                //CashCancleAction();
            }
            if (CurrentNormalCarInfo.ListDcDetail.Count > 0)
            {
                foreach (DcDetail detailDcDetail in CurrentNormalCarInfo.ListDcDetail)
                {
                    if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                    {
                        mHttpProcess.DiscountCancle(CurrentNormalCarInfo, detailDcDetail);
                        detailDcDetail.UseYn = false;
                    }
                }
            }
            CurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
            EventExitPayForm(mCurrentFormType, NPSYS.FormType.Select);
        }

        private void Close_Callback()
        {
            NPSYS.buttonSoundDingDong();
            EventExitPayForm(mCurrentFormType);
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");
        }

        // 결제종료시 이벤트를 종료한다
        private void SettingDisableEvent()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingDisableEvent", "[장비이벤트 삭제]");
            try
            {
                axWindowsMediaPlayer1.PlayStateChange -= new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
                axWindowsMediaPlayer1.MediaError -= new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
                axWindowsMediaPlayer1.ErrorEvent -= new EventHandler(player_ErrorEvent);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|FormPaymentMenu", ex.ToString());
            }

            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeSerials.EventBarcode -= new BarcodeSerial.BarcodeEvent(BarcodeSerials_EventBarcode);
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeMoter.EventAutoRedingData -= new BarcodeMoter.GetAutoRedingData(BarcodeMotorSerials_EventBarcode);
            }

            if (NPSYS.Device.gIsUseSinbunReader)
            {
                NPSYS.Device.SinbunReader.readEvent -= SinbunProcess; // 이벤트
            }
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {
                NPSYS.Device.SmtSndRcv.OnRcvState -= new AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEventHandler(SmtSndRcv_OnRcvState);
                NPSYS.Device.SmtSndRcv.OnTermComplete -= new EventHandler(SmtSndRcv_OnTermComplete);
                NPSYS.Device.SmtSndRcv.OnTermExit -= new EventHandler(SmtSndRcv_OnTermExit);
            }
        }

        /// <summary>
        /// 정기권 연장
        /// </summary>
        /// <param name="pMonth"></param>
        private void SetNextRegExipire(int pMonth)
        {
            paymentControl.ButtonEnable(ButtonEnableType.AddMonthStart);
            int pPrePayMoney = CurrentNormalCarInfo.PaymentMoney;
            CurrentNormalCarInfo.SetRegCurrMonthSetting(pMonth);
            paymentControl.ElapsedTime = NPSYS.ConvetYears_Dash(CurrentNormalCarInfo.NextExpiredYmd);
            paymentControl.ParkingFee = TextCore.ToCommaString(CurrentNormalCarInfo.TotFee);
            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SetNextRegExipire", "정기권연장일선택 개월수:" + pMonth.ToString() + " 연장가능일:" + NPSYS.ConvetYears_Dash(CurrentNormalCarInfo.NextExpiredYmd));

            if (pPrePayMoney > CurrentNormalCarInfo.PaymentMoney) // 현재 할인되서 금액이 할인됬다면
            {
                BeforeChangePayValueAsCardReader();
                ChangePayValueAsCardReader();
            }

            paymentControl.ButtonEnable(ButtonEnableType.AddMonthEnd);
        }

        /// <summary>
        /// 삼성페이
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSamSungPay_Click(object sender, EventArgs e)
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | btnSamSungPay_Click", "삼성페이 결제버튼 누름");

            if (CurrentNormalCarInfo.PaymentMoney > 0 && CurrentNormalCarInfo.Current_Money == 0
                && (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_DIP_IFM || NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_TCM || NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_PAYMGATE))
            {

                if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_DIP_IFM)
                {
                    try
                    {
                        paymentControl.ButtonEnable(ButtonEnableType.SamsumPayStart);
                        SettingDisableDevice();
                        if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED || NPSYS.Device.KICC_TIT.GetCardInsert())
                        {
                            NPSYS.Device.KICC_TIT.CardEject();
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        }
                        Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, CurrentNormalCarInfo);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                        {
                            NPSYS.CashCreditCount += 1;
                            NPSYS.CashCreditMoney += CurrentNormalCarInfo.VanAmt;
                            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제됨");

                            if (CurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                                //카드실패전송완료
                                PaymentComplete();

                                return;
                            }
                        }
                        else // 잘못된 티켓
                        {
                            TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제안됨 사유 : " + _CardpaySuccess.Message);
                            paymentControl.ErrorMessage = _CardpaySuccess.Message;
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                            EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | btnSamSungPay_Click", "KICC 결제중 예외 발생 : 사유 : " + ex.ToString());
                    }
                    finally
                    {
                        if (CurrentNormalCarInfo.PaymentMoney > 0)
                        {
                            SettingEnableDevice();
                            paymentControl.ButtonEnable(ButtonEnableType.SamsunPayEnd);
                        }
                    }
                }
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_TCM || NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_PAYMGATE)
                {
                    try
                    {
                        paymentControl.ButtonEnable(ButtonEnableType.SamsumPayStart);
                        SettingDisableDevice();
                        if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED || KocesTcmMotor.CardState() == 2)
                        {
                            KocesTcmMotor.CardEject();
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        }

                        Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, CurrentNormalCarInfo);
                        if (_CardpaySuccess.Success)
                        {
                            NPSYS.CashCreditCount += 1;
                            NPSYS.CashCreditMoney += CurrentNormalCarInfo.VanAmt;
                            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제됨");

                            if (CurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                                //카드실패전송완료
                                PaymentComplete();

                                return;
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제안됨");
                            paymentControl.ErrorMessage = _CardpaySuccess.Message;
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                            EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | btnSamSungPay_Click", "KOCES 결제중 예외 발생 : 사유 : " + ex.ToString());
                    }
                    finally
                    {
                        if (CurrentNormalCarInfo.PaymentMoney > 0)
                        {
                            SettingEnableDevice();
                            paymentControl.ButtonEnable(ButtonEnableType.SamsunPayEnd);
                        }
                    }
                }
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | btnSamSungPay_Click", "삼성페이 결제되지 않는장비임");
            }
        }

        private void btn_CashCancle_Click(object sender, EventArgs e)
        {
            if (CurrentNormalCarInfo.PaymentMoney == 0 || CurrentNormalCarInfo.Current_Money == 0)
            {
                return;
            }
            inputtime = NPSYS.SettingInputTimeValue;
            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu|btn_CashCancle_Click", "현금 취소버튼누름");
            CashCancleAction();
        }

        /// <summary>
        /// 취소시 할인금액을 제외한 주차요금 관련 변수 초기화 시킴 
        /// </summary>
        private void CashCancleAction()
        {
            try
            {
                if (CurrentNormalCarInfo.Current_Money > 0)  // 투입된 금액이 있다면
                {
                    paymentControl.ButtonEnable(ButtonEnableType.CashCancle);
                    SettingDisableDevice();

                    PaymentResult result = CancleRefundMoneyAction();

                    if (result == PaymentResult.Success) // 지폐나 동전불출할수 있으면
                    {
                        CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "취소시 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        //카드실패전송
                        CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료
                        Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }

                        CurrentNormalCarInfo.CanCleClear();
                        NormalCarInfo lCanclecarInfo = new NormalCarInfo();
                        lCanclecarInfo.CurrentPayment = CommonFuction.Clone<Payment>(currentCar);
                        lCanclecarInfo.SetCurrentPayment(NormalCarInfo.PaymentSetType.NormalPaymnet);
                        SetCarInfo(lCanclecarInfo);
                        paymentControl.CancelButtonVisible = false;


                    }
                    else
                    {
                        paymentControl.CancelButtonVisible = false;
                        //카드실패전송
                        CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료
                        CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelNotOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "취소시 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }
                        ReceiptCancleErrorActions();
                        CurrentNormalCarInfo.CanCleClear();
                        NormalCarInfo lCanclecarInfo = new NormalCarInfo();
                        lCanclecarInfo.CurrentPayment = CommonFuction.Clone<Payment>(currentCar);
                        lCanclecarInfo.SetCurrentPayment(NormalCarInfo.PaymentSetType.NormalPaymnet);
                        SetCarInfo(lCanclecarInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|btn_CashCancle_Click", ex.ToString());
            }
            finally
            {
                paymentControl.ButtonEnable(ButtonEnableType.CashCancleStop);
                SettingEnableDevice();
            }
        }

        private void FormPaymentMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //SettingDisableEvent();
                //axWindowsMediaPlayer1.Ctlcontrols.stop();
                //axWindowsMediaPlayer1.close();

                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu|FormPaymentMenu_FormClosed", "요금화면 처리작업 종료");
                //this.Close();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|FormPaymentMenu_FormClosed", ex.ToString());
            }
        }

        #endregion 폼 컨트롤 이벤트 처리

        #region 폼 활성화 시작/폼 활성화 종료시 호출함수

        /// <summary>
        /// View Open
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pFormType"></param>
        /// <param name="param">NormalCarInfo Type Only</param>
        public void OpenView<T>(NPSYS.FormType pFormType, T param)
        {
            try
            {
                if (this.Visible == false)
                {
                    NormalCarInfo normalCarInfo = param.To<NormalCarInfo>();
                    NPSYS.CurrentFormType = mCurrentFormType;
                    mPreFomrType = pFormType;
                    ClearView();

                    inputtime = NPSYS.SettingInputTimeValue;
                    MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                    //화면 Display(mCurrentNormalCarInfo를 아래 함수에서 셋팅함.)
                    SetCarInfo(normalCarInfo);

                    pic_Wait_MSG_WAIT.SendToBack();
                    pic_Wait_MSG_WAIT.Visible = false;
                    paymentControl.CancelButtonVisible = false; //현금 취소 버튼 비활성화

                    if (normalCarInfo.PaymentMoney == 0)
                    {
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu|FormPaymentMenu_Load", "요금화면 로드바로영수증으로");
                        //카드실패전송
                        normalCarInfo.PaymentMethod = PaymentType.Free;
                        //카드실패전송완료

                        PaymentComplete();
                        return;
                    }

                    GetImage();

                    inputTimer.Enabled = true;
                    MovieTimer.Enabled = true;

                    //SettingEnableEvent();
                    SettingEnableDevice(); //결제장비 동작 시작
                    paymentControl.ButtonEnable(ButtonEnableType.PayFormStart);
                    SetLanuageDynamic(NPSYS.CurrentLanguageType);

                    this.TopMost = true;
                    this.Show();
                    this.Activate();
                    TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | OpenView", "요금화면 로드|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | OpenView", ex.ToString());
            }
            finally
            {

            }
        }

        /// <summary>
        /// 변수초기화 및 장비초기화
        /// </summary>
        private void ClearView()
        {
            paymentControl.Clear();
            mListBarcodeData = new List<string>();
            CurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
            BillReader.g_billValue = string.Empty;
            if (NPSYS.Device.CoinReader != null)
            {
                NPSYS.Device.CoinReader.mLIstQty = new List<string>();
            }
            mListBarcodeMotorData = new List<BarcodeMoter.BarcodeMotorResult>();
            NPSYS.CurrentBusyType = NPSYS.BusyType.None;
        }

        private void GetImage()
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지 불러오기 작업시작");
                if (NPSYS.gIsAutoBooth)
                {
                    if (CurrentNormalCarInfo.OutImage1 != null && CurrentNormalCarInfo.OutImage1.Trim().Length > 0)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지경로" + CurrentNormalCarInfo.OutImage1);
                        paymentControl.CarImage = NPSYS.WebImageView(CurrentNormalCarInfo.OutImage1);
                    }
                    else if (CurrentNormalCarInfo.InImage1 != null && CurrentNormalCarInfo.InImage1.Trim().Length > 0)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지경로" + CurrentNormalCarInfo.InImage1);
                        paymentControl.CarImage = NPSYS.WebImageView(CurrentNormalCarInfo.InImage1);
                    }
                }
                else
                {
                    paymentControl.CarImage = NPSYS.WebImageView(CurrentNormalCarInfo.InImage1);
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지 불러오기 작업종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | GetImage", ex.ToString());
            }
        }

        public void CloseView()
        {
            if (this.Visible)
            {
                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CloseView", "요금화면 화면종료시작됨");
                paymentControl.ButtonEnable(ButtonEnableType.PayFormEnd);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                paymentControl.CarImage = null;

                MovieTimer.Enabled = false;

                stopVideo();
                if (CurrentNormalCarInfo.PaymentMoney != 0) // 주차요금이 있는상태에서 폼이종료된다면
                {
                    SettingDisableDevice();
                    CashCancleFormCloseAction(false);
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CloseView", "요금화면 화면종료됨");
            }
        }

        public void CloseViewBeforeInfo()
        {
            if (this.Visible)
            {
                paymentControl.ButtonEnable(ButtonEnableType.PayFormEnd);
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                paymentControl.CarImage = null;

                MovieTimer.Enabled = false;

                stopVideo();
                SettingDisableDevice();
                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CloseViewBeforeInfo", "요금화면 인포메이션 화면으로 인하여 화면종료됨");
            }
        }

        public void OpenViewBeforeInfo(NPSYS.FormType pFormType)
        {
            try
            {
                if (this.Visible == false)
                {
                    NPSYS.CurrentFormType = mCurrentFormType;
                    mPreFomrType = pFormType;
                    inputtime = NPSYS.SettingInputTimeValue;
                    MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                    pic_Wait_MSG_WAIT.SendToBack();
                    pic_Wait_MSG_WAIT.Visible = false;
                    paymentControl.CancelButtonVisible = false;
                    inputTimer.Enabled = true;
                    MovieTimer.Enabled = true;

                    SettingEnableDevice();
                    paymentControl.ButtonEnable(ButtonEnableType.PayFormStart);
                    SetLanuageDynamic(NPSYS.CurrentLanguageType);
                    this.TopMost = true;
                    this.Activate();
                    this.Show();
                    TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | OpenViewBeforeInfo", "요금화면 다시로드|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | OpenView", ex.ToString());
            }
            finally
            {

            }
        }

        #endregion 폼 활성화 시작/폼 활성화 종료시 호출함수

        #region 결제 동작 처리

        /// <summary>
        /// 결제 시작시 장비 동작처리
        /// </summary>
        private void SettingEnableDevice()
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingEnableDevice", "[장비동작시킴]");

            //마그네틱 카드리더기 설정
            StartTicketCardRead();

            //현금 리더기 설정
            if (NPSYS.Device.isUseDeviceBillReaderDevice || NPSYS.Device.isUseDeviceCoinReaderDevice) StartMoneyInsert();
            else StopMoneyInsert();

            //바코드 설정
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                timerBarcode.Enabled = true;
                timerBarcode.Start();
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeMoter.SetEnable();
                timerBarcode.Enabled = true;
                timerBarcode.Start();
            }

            //신분증 리더기 설정
            if (NPSYS.Device.UsingSettingSinbunReader && NPSYS.Device.gIsUseSinbunReader) NPSYS.Device.SinbunReader.ThreadStart();

            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

            //VAN 설정
            switch (NPSYS.Device.GetCurrentUseDeviceCard())
            {
                case ConfigID.CardReaderType.SMATRO_TIT_DIP:
                    //   NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;
                    timerKisCardPay.Enabled = true;
                    if (CurrentNormalCarInfo.PaymentMoney > 0)
                    {
                        timerSmatro_TITDIP_Evcat.Interval = 500;
                        timerSmatro_TITDIP_Evcat.Tick += timerSmatro_TITDIP_Evcat_Tick;
                        paymentControl.ErrorMessage = string.Empty;
                        UnsetSmatro_DIPTIT_Evcat();

                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditPaymentMenu | SettingEnableDevice", "[Smatro_TITDIP_Evcat 신용카드요금결제시작]true");
                        this.timerSmatro_TITDIP_Evcat.Enabled = true;
                        this.timerSmatro_TITDIP_Evcat.Start();

                    }
                    break;
                case ConfigID.CardReaderType.SmartroVCat:
                    this.timerSmartroVCat.Enabled = true;
                    this.timerSmartroVCat.Start();
                    timerKisCardPay.Enabled = true;
                    break;
                case ConfigID.CardReaderType.FIRSTDATA_DIP:
                case ConfigID.CardReaderType.KOCES_PAYMGATE:
                case ConfigID.CardReaderType.KICC_DIP_IFM:
                    timerAutoCardReading.Enabled = true;
                    timerAutoCardReading.Start();
                    break;
                case ConfigID.CardReaderType.KOCES_TCM:
                    bool isSuccessInsertedReady = KocesTcmMotor.CardAccept();

                    if (isSuccessInsertedReady) mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    else mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                    timerAutoCardReading.Enabled = true;
                    timerAutoCardReading.Start();
                    break;
                case ConfigID.CardReaderType.KICC_TS141:
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    timerCardVisible.Enabled = true;
                    timerCardVisible.Start();
                    break;
                case ConfigID.CardReaderType.KIS_TIT_DIP_IFM:
                    timerKisCardPay.Enabled = true;
                    if (CurrentNormalCarInfo.PaymentMoney > 0)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        SetKisDipIFM();
                    }
                    break;
                case ConfigID.CardReaderType.SMATRO_TL3500S:
                    if (NPSYS.Device.TmoneySmartro3500 == null) NPSYS.Device.TmoneySmartro3500 = new Smartro_TL3500S();

                    NPSYS.Device.TmoneySmartro3500.EventTMoneyData += TmoneySmartro3500_EventTMoneyData;

                    if (!NPSYS.Device.TmoneySmartro3500.IsConnect)
                    {
                        //다시 연결 시도
                        NPSYS.Device.TmoneySmartro3500.Connect();
                        if (NPSYS.Device.TmoneySmartro3500.IsConnect)
                        {
                            //장치체크
                            NPSYS.Device.TmoneySmartro3500.RequestDeviceCheck();
                            System.Threading.Thread.Sleep(1000); //잠시 대기
                            //장치설정
                            NPSYS.Device.TmoneySmartro3500.DeviceType = NPSYS.Config.GetValue(ConfigID.TmoneyDeviceType);
                            NPSYS.Device.TmoneySmartro3500.EthernetIP = NPSYS.Config.GetValue(ConfigID.TmoneyDevIP);
                            NPSYS.Device.TmoneySmartro3500.EthernetPort = NPSYS.Config.GetValue(ConfigID.TmoneyDevPort);
                            NPSYS.Device.TmoneySmartro3500.MID = NPSYS.Config.GetValue(ConfigID.TmoneyCatID);
                            NPSYS.Device.TmoneySmartro3500.VanIP = NPSYS.Config.GetValue(ConfigID.TmoneyVanIp);
                            NPSYS.Device.TmoneySmartro3500.VanPort = NPSYS.Config.GetValue(ConfigID.TmoneyVanPort);
                            NPSYS.Device.TmoneySmartro3500.DeviceIPType = NPSYS.Config.GetValue(ConfigID.TmoneyEntDhcp);
                            NPSYS.Device.TmoneySmartro3500.DeviceIP = NPSYS.Config.GetValue(ConfigID.TmoneyEntDeviceIp);
                            NPSYS.Device.TmoneySmartro3500.DeviceSubNet = NPSYS.Config.GetValue(ConfigID.TmoneyEntSubnet);
                            NPSYS.Device.TmoneySmartro3500.DeviceGateWay = NPSYS.Config.GetValue(ConfigID.TmoneyEntGateway);
                            NPSYS.Device.TmoneySmartro3500.RequestInitSetting();
                            System.Threading.Thread.Sleep(1000); //잠시 대기
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditPaymentMenu | SettingEnableDevice", "[TmoneySmatro 연결실패]");
                        }
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "결제하여 주십시오.";
                    }

                    //else
                    //{
                    //    Thread.Sleep(100); //잠시 대기
                    //    //결제 요청 전문 송신
                    //    //0원 결제일 경우를 제외시켜야 함.
                    //    if (CurrentNormalCarInfo.PaymentMoney != 0)
                    //    {
                    //        if (CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar ||
                    //            CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    //        {
                    //            //취소금액
                    //            string cancelPaymentMoney = CurrentNormalCarInfo.PaymentMoney.ToString();
                    //            //취소 승인일시
                    //            string cancelDateTime = CurrentNormalCarInfo.VanDate_Cancle.Replace("-", "") + CurrentNormalCarInfo.VanTime_Cancle.Replace(":", "");
                    //            //취소할 승인번호
                    //            string cancelNumber = CurrentNormalCarInfo.VanRegNo_Cancle;
                                
                    //            //결제 취소 요청
                    //            NPSYS.Device.TmoneySmartro3500.RequestApprovalCancle(cancelPaymentMoney, cancelDateTime, cancelNumber);
                    //        }
                    //        else
                    //        {
                    //            //결제 승인 요청
                    //            NPSYS.Device.TmoneySmartro3500.RequestApproval(CurrentNormalCarInfo.PaymentMoney.ToString());
                    //            //NPSYS.Device.TmoneySmartro3500.RequestApprovalCancle("2500", "20200317124737", "31298213"); //결제취소 테스트
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //0원결제
                    //    }
                    //}

                    break;
            }
        }

        // 결제종료시 동작처리
        private void SettingDisableDevice()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingEnableDevice", "[장비종료시킴]");
            StopTicketCardRead();
            StopMoneyInsert();
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                timerBarcode.Enabled = false;
                timerBarcode.Stop();
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeMoter.SetDIsable();
                timerBarcode.Enabled = false;
                timerBarcode.Stop();
            }
            if (NPSYS.Device.UsingSettingSinbunReader)
            {
                if (NPSYS.Device.gIsUseSinbunReader)
                {
                    NPSYS.Device.SinbunReader.ThreadEnd();
                }
            }

            switch (NPSYS.Device.GetCurrentUseDeviceCard())
            {
                case ConfigID.CardReaderType.SmartroVCat:
                    this.timerSmartroVCat.Enabled = false;
                    this.timerSmartroVCat.Stop();
                    timerKisCardPay.Enabled = false;
                    break;
                case ConfigID.CardReaderType.FIRSTDATA_DIP:
                case ConfigID.CardReaderType.KOCES_PAYMGATE:
                case ConfigID.CardReaderType.KICC_DIP_IFM:
                    timerAutoCardReading.Enabled = false;
                    timerAutoCardReading.Stop();
                    break;
                case ConfigID.CardReaderType.KOCES_TCM:
                    timerAutoCardReading.Enabled = false;
                    timerAutoCardReading.Stop();
                    KocesTcmMotor.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    break;
                case ConfigID.CardReaderType.KICC_TS141:
                    timerCardVisible.Enabled = false;
                    timerCardVisible.Stop();
                    //btnCardApproval.Visible = true; //뉴타입주석
                    break;
                case ConfigID.CardReaderType.KIS_TIT_DIP_IFM:
                    timerKisCardPay.Enabled = false;
                    timerKisCardPay.Stop();
                    UnSetKisDipIFM();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | Kis_TIT_DIpDeveinCancle", "[KIS_TIT_DIP거래초기화 요청시작 종료]");
                    break;
                case ConfigID.CardReaderType.SMATRO_TIT_DIP:
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    timerKisCardPay.Enabled = false;
                    timerKisCardPay.Stop();
                    timerSmatro_TITDIP_Evcat.Tick -= timerSmatro_TITDIP_Evcat_Tick;
                    this.timerSmatro_TITDIP_Evcat.Enabled = false;
                    this.timerSmatro_TITDIP_Evcat.Stop();
                    paymentControl.ErrorMessage = string.Empty;
                    UnsetSmatro_DIPTIT_Evcat();
                    break;
                case ConfigID.CardReaderType.SMATRO_TL3500S:
                    //결제 금액이 떠 있을 수 있으므로 결제 대기 상태로 전문을 전송한다.
                    NPSYS.Device.TmoneySmartro3500.RequestApprovalWait(); //음성 : 취소되었습니다.
                    NPSYS.Device.TmoneySmartro3500.EventTMoneyData -= TmoneySmartro3500_EventTMoneyData;
                    break;
            }

        }

        /// <summary>
        /// 주차요금을 모두 고객이 지불했을시 실행되며 영수증 폼으로 이동
        /// </summary>
        private void PaymentComplete()
        {
            try
            {

                PaymentEndAction();
                CashRecipt();
                if (CurrentNormalCarInfo.GetInComeMoney > 0)
                {
                    if (CompletOutChargeAction() != PaymentResult.Success)
                    {
                        //시제설정누락처리
                        CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessNotOut; //시제설정누락처리
                        ReceiptChargeErrorActions();
                        //카드실패전송
                        CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료

                    }
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "결제처리 전송시작");
                DateTime paydate = DateTime.Now;
                Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                if (currentCar.status.Success == false)
                {
                    // DB에 저장하고 재전송처리해야함
                    return;
                }
                NormalCarInfo SendcarInfo = new NormalCarInfo();
                SendcarInfo = CommonFuction.Clone<NormalCarInfo>(CurrentNormalCarInfo);
                EventExitPayForm_NextReceiptForm(mCurrentFormType, NPSYS.FormType.Receipt, SendcarInfo);
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Payment", ex.ToString());
                paymentControl.ErrorMessage = "Payment():" + ex.ToString();
            }
        }
        /// <summary>
        /// 현금영수증 처리
        /// </summary>
        /// <returns></returns>
        private bool CashRecipt()
        {
            if (NPSYS.Device.UsingUsingSettingCashReceipt && CurrentNormalCarInfo.Current_Money > 0) // 현금영수증 사용이라면
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CashRecipt", "[현금영수증 처리시도]");
                Result cashResult = m_PayCardandCash.CashRecipt(CurrentNormalCarInfo);
                return cashResult.Success;
            }
            return false;
        }

        private void PaymentEndAction()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | PaymentEndAction", "[PaymentEndAction]");
            paymentControl.ButtonEnable(ButtonEnableType.PayFormEnd);
            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
            SettingDisableDevice();
        }

        #endregion 결제 동작 처리

        #region 타이머 Tick Event Handler

        private int inputtime = NPSYS.SettingInputTimeValue;

        /// <summary>
        /// 일정시간 화면 닫기 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputTimer_Tick(object sender, EventArgs e)
        {
            if (inputtime < 0)
            {
                inputtime = 50000000;
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | inputTimer_Tick", "시간이지나서 처음으로 돌아감");
                CashCancleFormCloseAction(false);
                if (CurrentNormalCarInfo.ListDcDetail.Count > 0)
                {
                    foreach (DcDetail detailDcDetail in CurrentNormalCarInfo.ListDcDetail)
                    {
                        if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                        {
                            mHttpProcess.DiscountCancle(CurrentNormalCarInfo, detailDcDetail);
                            detailDcDetail.UseYn = false;
                        }
                    }
                }
                CurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
                EventExitPayForm(mCurrentFormType);
                return;

            }
            inputtime = inputtime - 3000;
        }

        /// <summary>
        /// 신용카드 리더기 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerAutoCardReading_Tick(object sender, EventArgs e)
        {
            timerAutoCardReading.Stop();
            try
            {
                switch (NPSYS.Device.GetCurrentUseDeviceCard())
                {
                    case ConfigID.CardReaderType.FIRSTDATA_DIP:
                        CardActionFirstDataDip();
                        break;
                    case ConfigID.CardReaderType.KICC_DIP_IFM:
                        CardActionKiccDip();
                        break;
                    case ConfigID.CardReaderType.KOCES_PAYMGATE:
                        CardActionKocesPayMGate();
                        break;
                    case ConfigID.CardReaderType.KOCES_TCM:
                        CardActionKocesTcm();
                        break;
                }
            }
            catch
            {
            }
            finally
            {
                if (CurrentNormalCarInfo.PaymentMoney != 0)
                {
                    timerAutoCardReading.Start();
                }
            }
        }

        /// <summary>
        /// 동전/지폐 리더기 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrReadAccount_Tick(object sender, EventArgs e)
        {
            tmrReadAccount.Stop();

            if (!NPSYS.Device.isUseDeviceBillReaderDevice && !NPSYS.Device.isUseDeviceCoinReaderDevice)  // 지폐 및 동전 리더기 둘다 동시 작동이 안될때
            {
                tmrReadAccount.Enabled = false;
                return;
            }
            try
            {
                tmrReadAccount.Stop();

                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (NPSYS.Device.isUseDeviceCoinReaderDevice)
                {
                    //동전연속투입관련 변경
                    if (NPSYS.Device.CoinReader.mLIstQty.Count > 0)
                    {
                        string coinmessage = NPSYS.Device.CoinReader.mLIstQty[0].ToString();
                        NPSYS.Device.CoinReader.mLIstQty.RemoveAt(0);
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu|tmrReadAccount_Tick", "동전 넣음");
                        TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|tmrReadAccount_Tick", "동전 들어옴");
                        InsertMoney(coinmessage);
                    }
                }

                //동전연속투입관련 변경
                if (BillReader.g_billValue.Trim() != "")
                {
                    string billValue = BillReader.g_billValue;
                    BillReader.g_billValue = "";
                    if (billValue.ToUpper().Trim() == "REJECT")
                    {
                        NPSYS.Device.BillReader.BillReject();
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐불량으로 리젝트");
                    }
                    else
                    {
                        BillReader.BillRederStatusType currentInsertStatus = BillReader.BillRederStatusType.OK;
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐 넣음:" + billValue);
                        if (NPSYS.SettingUse50000QtyBill == true)
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillAccept();
                            if (currentInsertStatus == BillReader.BillRederStatusType.OK)
                            {
                                InsertMoney(billValue);
                            }
                            else
                            {
                                currentInsertStatus = NPSYS.Device.BillReader.BillReject();
                                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "지폐불량으로 리젝트:" + currentInsertStatus.ToString());
                            }
                        }
                        else if (billValue.ToUpper() == "50000QTY")
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillReject(); ;
                            TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "5만원권 사용불가처리로 리젝트:" + currentInsertStatus.ToString());
                        }
                        else
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillAccept();
                            if (currentInsertStatus == BillReader.BillRederStatusType.OK)
                            {
                                InsertMoney(billValue);
                            }
                            else
                            {
                                currentInsertStatus = NPSYS.Device.BillReader.BillReject();
                                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "지폐불량으로 리젝트:" + currentInsertStatus.ToString());
                            }
                        }
                    }
                }

                if (CurrentNormalCarInfo.Current_Money > 0)
                {
                    paymentControl.CancelButtonVisible = true;
                }
                else
                {
                    paymentControl.CancelButtonVisible = false;
                }

                if (CurrentNormalCarInfo.Current_Money == 0)
                {
                    tmrReadAccount.Start();
                    return;
                }

                if (CurrentNormalCarInfo.PaymentMoney == 0)
                {
                    //카드실패전송
                    CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                    //카드실패전송완료
                    PaymentComplete();

                    return;
                }

                tmrReadAccount.Start();
                return;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|tmrReadAccount_Tick", ex.ToString());
                tmrReadAccount.Start();
            }
            finally
            {

            }
        }

        /// <summary>
        /// 마그네틱 할인권 리더기 타이머
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_CardReader2_Tick(object sender, EventArgs e)
        {
            int lTicketActionResult = 0;

            if (!NPSYS.Device.gIsUseMagneticReaderDevice)
            {
                timer_CardReader2.Enabled = false;
                return;
            }
            try
            {
                // 2016.10.27 KIS_DIP 추가
                if (CurrentNormalCarInfo.Current_Money > 0
                    || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess
                    || CurrentNormalCarInfo.VanAmt > 0)
                {
                    return;
                }
                // 2016.10.27 KIS_DIP 추가종료

                Result _TIcketStatus = NPSYS.Device.CardDevice2.GetStatus();
                if (_TIcketStatus.Success == false)  // 티켓장비가 정상이 아니면
                {
                    if (NPSYS.Device.CardDevice2.IsCreditCardSuccessStatus(_TIcketStatus.ReultIntMessage))
                    {
                        if (_TIcketStatus.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.Read_and_Verify_Fail)
                        {
                            NPSYS.Device.CardDevice2.TIcketFrontEject();
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "신용카드상태체크에러:" + "잘못들어갔을때 발생:" + _TIcketStatus.Message);
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:");
                            Thread.Sleep(5000);
                            return;
                        }
                        TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "신용카드상태체크에러:" + "잘못들어갔거나 비었을때 발생:" + _TIcketStatus.Message);
                        return;
                    }
                    NPSYS.Device.CardDevice2.TIcketFrontEject(); //카드 앞으로 배출

                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드장비 소프트리셋:" + _TIcketStatus.Message);
                    _TIcketStatus = NPSYS.Device.CardDevice2.SoftResetCreditDevice();
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드장비 소프트리셋 결과:" + _TIcketStatus.Message);
                    Thread.Sleep(3000);
                    _TIcketStatus = NPSYS.Device.CardDevice2.GetStatus();
                    if (_TIcketStatus.Success == false)  // 티켓장비가 정상이 아니면
                    {

                        if (NPSYS.Device.CardDevice2.IsCreditCardSuccessStatus(_TIcketStatus.ReultIntMessage))
                        {
                            if (_TIcketStatus.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.Read_and_Verify_Fail)
                            {
                                NPSYS.Device.CardDevice2.TIcketFrontEject();
                                TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "신용카드상태체크에러:" + "잘못들어갔을때 발생:" + _TIcketStatus.Message);
                                TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:");
                                TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드장비 소프트리셋");
                                _TIcketStatus = NPSYS.Device.CardDevice2.SoftResetCreditDevice();
                                TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드장비 소프트리셋 결과:" + _TIcketStatus.Message);
                                Thread.Sleep(3000);
                                NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketCardDevice.TicketAndCardResult.OK, true);

                                return;
                            }
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "신용카드상태체크에러:" + "잘못들어갔거나 비었을때 발생:" + _TIcketStatus.Message);
                            NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketCardDevice.TicketAndCardResult.OK, true);
                            return;
                        }


                        NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus((TicketCardDevice.TicketAndCardResult)_TIcketStatus.ReultIntMessage, false);

                        NPSYS.Device.CreditCardDeviceErrorMessage2 = _TIcketStatus.Message;
                        SetLanuageDynamic(NPSYS.CurrentLanguageType);
                        NPSYS.LedLight();
                        StartTicketCardRead();
                        TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", _TIcketStatus.Message);
                        // CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA2, _TIcketStatus.ReultIntMessage); // 희주주석

                        return;
                    }
                }

                Result _result = NPSYS.Device.CardDevice2.readingTicketCardStart(); //카드 읽기
                Result l_ReadingTrackdata = NPSYS.Device.CardDevice2.readingTicketCardEnd();

                timer_CardReader2.Stop(); //Timer 잠시 멈춤
                paymentControl.ErrorMessage = string.Empty;

                //카드 읽기 결과에 문제가 없고 정기권 차량일 경우
                if (l_ReadingTrackdata.ReultIntMessage != (int)TicketCardDevice.TicketAndCardResult.No_Return_Sensor_Ticket
                    && l_ReadingTrackdata.ReultIntMessage != (int)TicketCardDevice.TicketAndCardResult.Empy
                    && (
                        CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                        || CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season
                        )
                    )
                {
                    lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject(); //카드 앞으로 배출
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:" + lTicketActionResult.ToString());
                    System.Threading.Thread.Sleep(200);
                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoRegExtensDiscount);
                    return;
                }

                if (l_ReadingTrackdata.Success)    // 카드 또는 티켓이 삽입된 상태이고 정보값을 읽었다면
                {
                    inputtime = paymentInputTimer;

                    if (l_ReadingTrackdata.CurrentReadingType == Result.ReadingTypes.DiscountTIcket)  // 티켓정보라면
                    {
                        if (!NPSYS.Device.UsingSettingDiscountCard)   // 할인권 설정에서 사용안함일때 VCat사용시
                        {
                            lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject();
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:" + lTicketActionResult.ToString());
                            System.Threading.Thread.Sleep(200);
                            EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoDiscountTicket);
                            return;
                        }

                        Payment paymentAfterDisocunt = mHttpProcess.Discount(CurrentNormalCarInfo, DcDetail.DIscountTicketType.MI, l_ReadingTrackdata.Message);

                        //=========== 할인권 처리 ==============
                        if (paymentAfterDisocunt.status.Success) // 정상적인 티켓이라면
                        {
                            int prepayment = CurrentNormalCarInfo.PaymentMoney;

                            CurrentNormalCarInfo.ParkingMin = paymentAfterDisocunt.parkingMin;
                            CurrentNormalCarInfo.TotFee = Convert.ToInt32(paymentAfterDisocunt.totFee);
                            CurrentNormalCarInfo.TotDc = Convert.ToInt32(paymentAfterDisocunt.totDc);
                            CurrentNormalCarInfo.Change = Convert.ToInt32(paymentAfterDisocunt.change); //시제설정누락처리
                            CurrentNormalCarInfo.RecvAmt = Convert.ToInt32(paymentAfterDisocunt.recvAmt); //시제설정누락처리
                            CurrentNormalCarInfo.DcCnt = paymentAfterDisocunt.dcCnt;
                            CurrentNormalCarInfo.RealFee = Convert.ToInt32(paymentAfterDisocunt.realFee);
                            //할인권 입수수량 표출
                            paymentControl.DiscountInputCount = (Convert.ToInt32(paymentControl.DiscountInputCount) + 1).ToString();
                            //할인권 입수수량 표출 주석완료

                            //요금할인권처리
                            paymentControl.ParkingFee = TextCore.ToCommaString(CurrentNormalCarInfo.TotFee.ToString());
                            paymentControl.RecvMoney = TextCore.ToCommaString((CurrentNormalCarInfo.RecvAmt - CurrentNormalCarInfo.Change).ToString()); //시제설정누락처리

                            TextCore.INFO(TextCore.INFOS.DISCOUNTTICEKT_SUCCESS, "FormPaymentMenu|timer_CardReader2_Tick", "결제성공");
                            lTicketActionResult = NPSYS.Device.CardDevice2.TIcketBackEject();
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 뒤로배출:" + lTicketActionResult.ToString());
                            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);

                            if (prepayment == CurrentNormalCarInfo.PaymentMoney)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|timer_CardReader1_Tick", "할인에 성공했지만 할인금액이 없음");
                                return;
                            }

                            //할인 요금 적용 전 카드결제 동작 중지
                            BeforeChangePayValueAsCardReader();

                            if (CurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                CurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountCard;
                                //카드실패전송완료
                                PaymentComplete();

                                // db에 저장
                                // 영수증 출력화면
                                return;
                            }
                            else
                            {
                                //할인 요금 적용 된 상태로 카드결제 동작 시작
                                ChangePayValueAsCardReader();
                            }
                        }
                        else // 잘못된 티켓
                        {
                            lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject();
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:" + lTicketActionResult.ToString());
                            System.Threading.Thread.Sleep(200);

                            if (paymentAfterDisocunt.status.currentStatus == Status.BodyStatus.DiscountMod_NotAdd)
                            {
                                //할인권 수량 제한
                                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoAddDiscountTIcket);
                                return;
                            }
                            else if (paymentAfterDisocunt.status.currentStatus == Status.BodyStatus.Discount_PreUsed)
                            {
                                //동일 할인권 사용 제한
                                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.DuplicateDiscountTicket);
                                return;
                            }
                            else
                            {
                                //잘못된 할인권
                                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoDiscountTicket);
                                return;
                            }
                        }
                    }
                    else if (l_ReadingTrackdata.CurrentReadingType == Result.ReadingTypes.CreditCard)   // 카드정보라면
                    {
                        lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject();
                        TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드 앞으로 배출:" + lTicketActionResult.ToString());
                        System.Threading.Thread.Sleep(500);
                        inputtime = paymentInputTimer;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                        return;
                    }
                    else
                    {
                        lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject();
                        TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "카드 앞으로 배출:" + lTicketActionResult.ToString());
                        System.Threading.Thread.Sleep(500);
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu|timer_CardReader2_Tick", l_ReadingTrackdata.CurrentReadingType.ToString());
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                        return;
                    }
                }
                else if (l_ReadingTrackdata.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.No_TICEKT)
                {
                    paymentControl.ErrorMessage = "투입방향 오류.투입방향을 확인하세요";
                    TextCore.INFO(TextCore.INFOS.DISCOUNTTICEKT_ERRPR, "FormPaymentMenu|timer_CardReader2_Tick", "투입방향 오류.투입방향을 확인하세요");
                    NPSYS.Device.CardDevice2.TIcketFrontEject();
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:" + lTicketActionResult.ToString());
                    System.Threading.Thread.Sleep(500);
                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.CorrectCard);
                    return;

                }
                else if (l_ReadingTrackdata.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.Read_and_Verify_Fail)
                {
                    paymentControl.ErrorMessage = "투입방향 오류.투입방향을 확인하세요";
                    TextCore.INFO(TextCore.INFOS.DISCOUNTTICEKT_ERRPR, "FormPaymentMenu|timer_CardReader2_Tick", "투입방향 오류.투입방향을 확인하세요");
                    NPSYS.Device.CardDevice2.TIcketFrontEject();
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:" + lTicketActionResult.ToString());
                    System.Threading.Thread.Sleep(500);

                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                    return;
                }

                else if (l_ReadingTrackdata.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.No_Return_Sensor_Ticket || l_ReadingTrackdata.ReultIntMessage == (int)TicketCardDevice.TicketAndCardResult.Empy)
                {
                    // TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "else if:" + l_ReadingTrackdata.ReultIntMessage);
                }
                else
                {
                    paymentControl.ErrorMessage = l_ReadingTrackdata.Message;
                    TextCore.INFO(TextCore.INFOS.DISCOUNTTICEKT_ERRPR, "FormPaymentMenu|timer_CardReader2_Tick", l_ReadingTrackdata.Message);
                    NPSYS.Device.CardDevice2.TIcketFrontEject();
                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 앞으로 배출:");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|timer_CardReader2_Tick", ex.ToString());
                paymentControl.ErrorMessage = "timer_CardReader2_Tick():" + ex.ToString();
            }
            finally
            {
                if (NPSYS.Device.gIsUseMagneticReaderDevice && CurrentNormalCarInfo.PaymentMoney > 0)
                {
                    timer_CardReader2.Start();
                }
            }
        }

        /// <summary>
        /// 바코드할인 타이커(리스트로변경)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerBarcode_Tick(object sender, EventArgs e)
        {
            if (NPSYS.CurrentFormType != mCurrentFormType)
            {
                return;
            }
            if (CurrentNormalCarInfo.Current_Money > 0)
            {
                return;
            }
            if (CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }
            //KIS 할인처리시 처리문제

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM && mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardReady)
            {
                return;
            }
            //KIS 할인처리시 처리문제주석완료
            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode)
            {
                if (mListBarcodeData.Count > 0)
                {
                    timerBarcode.Stop();
                    string barcodeData = mListBarcodeData[0];
                    mListBarcodeData.RemoveAt(0);
                    BarcodeAction(barcodeData);

                    if (CurrentNormalCarInfo.PaymentMoney != 0)
                    {
                        timerBarcode.Start();
                    }

                }
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                if (mListBarcodeMotorData.Count > 0)
                {
                    timerBarcode.Stop();
                    if (mListBarcodeMotorData[0].ResultStatus == BarcodeMotorErrorCode.Ok)
                    {
                        string barcodeMotroData = mListBarcodeMotorData[0].Data;
                        //barcodeMotroData = "SF1E";//test
                        mListBarcodeMotorData.RemoveAt(0);
                        BarcodeAction(barcodeMotroData);
                    }
                    else
                    {
                        paymentControl.ErrorMessage = string.Empty;
                        switch (mListBarcodeMotorData[0].ResultStatus)
                        {
                            case BarcodeMotorErrorCode.TicketJamError:
                                paymentControl.ErrorMessage = "용지가 안에 걸렸습니다";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerBarcode_Tick", paymentControl.ErrorMessage);
                                Application.DoEvents();
                                break;
                            case BarcodeMotorErrorCode.TIcketReadError:
                                paymentControl.ErrorMessage = "바코드를 읽지 못하였습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerBarcode_Tick", paymentControl.ErrorMessage);
                                // 자동으로 방출된다
                                break;
                            default:
                                paymentControl.ErrorMessage = mListBarcodeMotorData[0].ResultStatus.ToString();
                                break;
                        }
                        mListBarcodeMotorData.RemoveAt(0);
                    }

                    if (CurrentNormalCarInfo.PaymentMoney != 0)
                    {
                        timerBarcode.Start();
                    }

                }
            }
            //바코드모터드리블 사용완료
        }

        ////포시즌 카드누를시 화면숨김
        int mCardVisible = 0;
        private void timerCardVisible_Tick(object sender, EventArgs e)
        {
            if (mCardVisible <= 0) // mCardVisible이 0보다적으면
            {
                //if (btnCardApproval.Visible == false) //뉴타입주석
                //{ //뉴타입주석
                //    btnCardApproval.Visible = true; //뉴타입주석
                //} //뉴타입주석
            }
            mCardVisible -= 1;
        }
        //포시즌 카드누를시 화면숨김 주석완료

        private void timerKiccTs141State_Tick(object sender, EventArgs e)
        {
            try
            {
                timerKiccTs141State.Stop();

                GetKiccState();
                if (CurrentNormalCarInfo.PaymentMoney == 0 && CurrentNormalCarInfo.VanAmt > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    //카드실패전송
                    CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                    //카드실패전송완료
                    PaymentComplete();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | timerKiccTs141State_Tick", ex.ToString());
            }
            finally
            {
                if (CurrentNormalCarInfo.PaymentMoney != 0)
                {
                    timerKiccTs141State.Start();
                }
            }

        }
        private void timerKiccSoundPlay_Tick(object sender, EventArgs e)
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay)
            {
                if (KiccTs141.StartSoundTick > 0)
                {
                    KiccTs141.StartSoundTick -= 1;
                    if (KiccTs141.StartSoundTick == 0)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void timerCardPay_Tick(object sender, EventArgs e)
        {
            if (CurrentNormalCarInfo.VanAmt > 0)
            {
                timerKisCardPay.Enabled = false;
                timerKisCardPay.Stop();
                //카드실패전송
                CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                //카드실패전송완료
                PaymentComplete();
            }
        }

        /// <summary>
        /// 스마트로 DIP 타입결제기 카드결제요청이 안되있을시 카드결제요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSmatro_TITDIP_Evcat_Tick(object sender, EventArgs e)
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay)
            {
                if (mSmartro_TITDIP_EVCat.StartSoundTick > 0)
                {
                    mSmartro_TITDIP_EVCat.StartSoundTick -= 1;
                    if (mSmartro_TITDIP_EVCat.StartSoundTick == 0)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            timerSmatro_TITDIP_Evcat.Stop();
            SmatroEVCAT_CardApprovalAction();
            timerSmatro_TITDIP_Evcat.Start();

        }

        private void timerSmartroVCat_Tick(object sender, EventArgs e)
        {

            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay)
            {
                if (mSmartroVCat.StartSoundTick > 0)
                {
                    mSmartroVCat.StartSoundTick -= 1;
                    if (mSmartroVCat.StartSoundTick == 0)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    }
                }
            }
            timerSmartroVCat.Stop();
            SmartroCardApprovalAction();
            timerSmartroVCat.Start();

        }

        private void MovieTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsNextFormPlaying == false)
                {
                    MovieStopPlay -= 1000;
                }
                if (MovieStopPlay == 0 && IsNextFormPlaying == false)
                {
                    //if (mIsPlayerOkStatus == true)
                    //{
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    //}
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|MovieTimer_Tick", "예외사항:" + ex.ToString());
            }
        }

        #endregion 타이머 Tick Event Handler

        #region 카드리더기 동작 처리

        /// <summary>
        /// 결제요금이 변경되기전 동작멈춤 중간에 결제가 되지않게 현재 카드리더기 기기에 요금 취소등의 동작
        /// </summary>
        private void BeforeChangePayValueAsCardReader()
        {
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {
                timerSmartroVCat.Enabled = false;
                timerSmartroVCat.Stop();

                SmatroDeveinCancle();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | timer_CardReader1_Tick", "[스마트로 VCat거래초기화 요청]");
                // 스마트로추가종료
            }
            // 2016.10.27  KIS_DIP 추가종료
            //KICCTS141적용
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
            {
                timerKiccTs141State.Enabled = false;
                timerKiccTs141State.Stop();
                KiccTs141.Initilize();
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                Thread.Sleep(1000);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | timer_CardReader2_Tick", "[KICC 거래초기화 요청]");
            }
            //else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TL3500S)
            //{
            //    //결제 대기 상태로 전환한다. 이상태는 결제 불가함.
            //    NPSYS.Device.TmoneySmartro3500.RequestApprovalWait();
            //}
        }

        /// <summary>
        /// 결제요금이 변경된후 카드리더기 동작처리
        /// </summary>
        private void ChangePayValueAsCardReader()
        {
            // 2016.10.27 KIS_DIP 추가
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                timerSmartroVCat.Enabled = true;
                timerSmartroVCat.Start();
            }

            //스마트로 TIT_DIP EV-CAT 적용
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                mSmartro_TITDIP_EVCat.ChangePayMoney(NPSYS.Device.Smartro_TITDIP_Evcat, CurrentNormalCarInfo.PaymentMoney.ToString());
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;

                Thread.Sleep(1000);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | ChangePayValueAsCardReader", "[스마트로 TIT_DIP EV-CAT 결제요금변경 요청]");
            }
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            }
            //else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TL3500S)
            //{
            //    //할인 된 금액으로 다시 재결제 요청
            //    NPSYS.Device.TmoneySmartro3500.RequestApproval(CurrentNormalCarInfo.PaymentMoney.ToString());
            //}
        }

        // 2016.10.27 KIS_DIP 추가
        #region KIS_TIT_DIP_IFM

        //KIS 할인처리시 처리문제

        private void btnCardAction_Click(object sender, EventArgs e)
        {
            if (CurrentNormalCarInfo.Current_Money > 0 || CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }
            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnCardAction_Click", "카드결제버튼 누름");

        }


        #endregion
        // 2016.10.27  KIS_DIP 추가종료


        /// <summary>
        /// 스마트로 추가로 이함수 모두 변경
        /// </summary>
        private void StartTicketCardRead()
        {
            try
            {
                TextCore.ACTION(TextCore.ACTIONS.CARDREADER, "FormPaymentMenu|StartTicketRead", "티켓/ 카드 받을 준비 시작");
                timer_CardReader2.Enabled = false;

                if ((NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.TItMagnetincDiscount)
                    && NPSYS.Device.gIsUseMagneticReaderDevice)
                {
                    timer_CardReader2.Enabled = true;
                    timer_CardReader2.Start();
                }

                if (NPSYS.Device.UsingSettingCardReadType != ConfigID.CardReaderType.None
                 && NPSYS.Device.UsingSettingMagneticReadType != ConfigID.CardReaderType.None
                 && NPSYS.Device.gIsUseCreditCardDevice == false && NPSYS.Device.gIsUseMagneticReaderDevice == false)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StopTicketCardRead", "카드러더기 장비 두곳다 에러");
                }

                TextCore.ACTION(TextCore.ACTIONS.CARDREADER, "FormPaymentMenu|StartTicketRead", "티켓/ 카드 받을 준비 종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StartTicketRead", ex.ToString());
            }
        }

        /// <summary>
        /// 스마트로 추가로 이함수 모두 변경
        /// </summary>
        private void StopTicketCardRead()
        {
            try
            {
                TextCore.ACTION(TextCore.ACTIONS.CARDREADER, "FormPaymentMenu|StopTicketCardRead", "티켓/ 카드 받을 준비 중지 작업 시작");

                if (NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.TItMagnetincDiscount
                    && NPSYS.Device.gIsUseMagneticReaderDevice)
                {
                    timer_CardReader2.Enabled = false;
                    timer_CardReader2.Stop();
                    NPSYS.Device.CardDevice2.TIcketFrontEject();
                }

                if (NPSYS.Device.UsingSettingCardReadType != ConfigID.CardReaderType.None
                 && NPSYS.Device.UsingSettingMagneticReadType != ConfigID.CardReaderType.None
                 && NPSYS.Device.gIsUseCreditCardDevice == false && NPSYS.Device.gIsUseMagneticReaderDevice == false)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StopTicketCardRead", "카드러더기 장비 두곳다 에러");
                }
                TextCore.ACTION(TextCore.ACTIONS.CARDREADER, "FormPaymentMenu|StopTicketCardRead", "티켓/ 카드 받을 준비 중지 종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StopTicketCardRead", ex.ToString());
            }
        }

        #endregion 카드리더기 동작 처리

        #region 취소버튼 처리

        private void CashCancleFormCloseAction(bool pIsSetDisable = false)
        {
            try
            {
                if (CurrentNormalCarInfo.Current_Money > 0)  // 투입된 금액이 있다면
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|CashCancleFormCloseAction", "시간초과로 입수된 돈 방출처리 방출금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString());

                    paymentControl.ButtonEnable(ButtonEnableType.CashCancle);
                    if (pIsSetDisable)
                    {
                        SettingDisableDevice();
                    }
                    //시제설정누락처리
                    if (NPSYS.gIsAutoBooth == false)
                    {
                        PaymentResult result = CancleRefundMoneyAction();

                        if (result == PaymentResult.Success) // 지폐나 동전불출할수 있으면
                        {
                            CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelOut; //시제설정누락처리
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 결제처리 전송시작");
                            DateTime paydate = DateTime.Now;
                            //카드실패전송
                            CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                            //카드실패전송완료

                            Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                            if (currentCar.status.Success == false)
                            {
                                // DB에 저장하고 재전송처리해야함
                                return;
                            }
                            paymentControl.CancelButtonVisible = false;
                        }
                        else
                        {
                            CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelNotOut; //시제설정누락처리
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 결제처리 전송시작");
                            DateTime paydate = DateTime.Now;
                            //카드실패전송
                            CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                            //카드실패전송완료

                            Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                            if (currentCar.status.Success == false)
                            {
                                // DB에 저장하고 재전송처리해야함
                                return;
                            }
                            paymentControl.CancelButtonVisible = false;
                            ReceiptCancleErrorActions(true);
                            CurrentNormalCarInfo.CanCleClear();
                        }
                    }
                    else
                    {
                        CurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CommandOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 강제입금 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }
                        CurrentNormalCarInfo.CanCleClear();
                    }
                    //시제설정누락처리 완료
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|btn_CashCancle_Click", ex.ToString());
            }
        }

        #endregion 취소버튼 처리

        #region 보관증관련

        //시제설정누락처리 수정
        /// <summary>
        /// 동전 및 지폐장비 이상일때 현금 취소시 보관증 출력
        /// </summary>
        private void ReceiptCancleErrorActions(bool isFormClose = false)   // 보관증출력
        {

            LPRDbSelect.LogMoney(PaymentType.CashTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentNormalCarInfo, MoneyType.CashTicket, 0, CurrentNormalCarInfo.GetNotDisChargeMoney, "보관증");
            TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|ReceiptErrorActions", "보관증발행액" + CurrentNormalCarInfo.GetNotDisChargeMoney);
            Print.CashTicketNotCoinPrint(CurrentNormalCarInfo, false, this.Name);
            if (isFormClose == false)
            {
                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotEnoghfMoney);
            }

            SetLanuageDynamic(NPSYS.CurrentLanguageType);


        }
        //시제설정누락처리 완료

        private void ReceiptChargeErrorActions()
        {
            try
            {

                LPRDbSelect.LogMoney(PaymentType.CashTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CurrentNormalCarInfo, MoneyType.CashTicket, 0, CurrentNormalCarInfo.GetNotDisChargeMoney, "보관증");
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|ReceiptErrorActions", "보관증발행액:" + CurrentNormalCarInfo.GetNotDisChargeMoney);
                Print.CashTicketNotCoinPrint(CurrentNormalCarInfo, true, this.Name);

                System.Threading.Thread.Sleep(500);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormRecipt|ReceiptErrorActions", "보관증발행");
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|ReceiptErrorActions", "예외사항:" + ex.ToString());
            }

        }
        #endregion 보관증관련

        #region 거스름돈 / 취소시 방출관련

        /// <summary>
        /// Success 결제성공, Fail 장비이상 , ChargeSmall 거스름돈부족
        /// </summary>
        enum PaymentResult
        {
            Success = 0,
            Fail = 1,
            ChargeSmall = 2
        }

        /// <summary>
        /// 0이면 결제성공 1이면 장비이상 2이면 거스름돈부족
        /// </summary>
        /// <returns></returns>
        ///
        private PaymentResult CancleRefundMoneyAction()
        {
            try
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "취소명령시 입금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCancelMoney();

                    CurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할금액: 5000원수량:" + CurrentNormalCarInfo.Cancle5000Qty.ToString() + "  1000원수량:" + CurrentNormalCarInfo.Cancle1000Qty.ToString() + "  500원수량:" + CurrentNormalCarInfo.Cancle500Qty.ToString() + "  100원수량:" + CurrentNormalCarInfo.Cancle100Qty.ToString() + "  50원수량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (CurrentNormalCarInfo.Cancle5000Qty > 0 || CurrentNormalCarInfo.Cancle1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (CurrentNormalCarInfo.Cancle1000Qty * 2) + (CurrentNormalCarInfo.Cancle5000Qty * 10);
                            CurrentNormalCarInfo.Cancle1000Qty = 0;
                            CurrentNormalCarInfo.Cancle5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "[지폐대신 동전방출로 변경] 방출할500원 수량: " + CurrentNormalCarInfo.Cancle500Qty.ToString());

                        }
                        else
                        {
                            if (CurrentNormalCarInfo.Cancle5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "5000원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle5000Qty.ToString() + "개");
                                int out5000Qty = CurrentNormalCarInfo.Cancle5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillCancleDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (CurrentNormalCarInfo.Cancle1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1000원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle1000Qty.ToString() + "개");
                                int out1000Qty = CurrentNormalCarInfo.Cancle1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillCancleDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (CurrentNormalCarInfo.Cancle1000Qty * 2) + (CurrentNormalCarInfo.Cancle5000Qty * 10);
                        CurrentNormalCarInfo.Cancle1000Qty = 0;
                        CurrentNormalCarInfo.Cancle5000Qty = 0;

                    }
                    if (CurrentNormalCarInfo.CurrentCancleCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할 동전이 없으므로 성공:" + CurrentNormalCarInfo.CurrentCancleCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        CurrentNormalCarInfo.NotdisChargeMoney500Qty = CurrentNormalCarInfo.Cancle500Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney100Qty = CurrentNormalCarInfo.Cancle100Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty;
                        CurrentNormalCarInfo.Cancle50Qty = 0;
                        CurrentNormalCarInfo.Cancle100Qty = 0;
                        CurrentNormalCarInfo.Cancle500Qty = 0;

                        return PaymentResult.Fail;
                    }

                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (CurrentNormalCarInfo.Cancle500Qty > 0) // 500원 방출수량이 있다면
                        {
                            CurrentNormalCarInfo.Cancle100Qty = CurrentNormalCarInfo.Cancle100Qty + (CurrentNormalCarInfo.Cancle500Qty * 5); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "500원 장비 미사용 또는 장비고장으로 500원 100원으로 교체:" + CurrentNormalCarInfo.Cancle100Qty.ToString());
                            CurrentNormalCarInfo.Cancle500Qty = 0;
                        }
                    }

                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < CurrentNormalCarInfo.Cancle500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();
                    }

                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (CurrentNormalCarInfo.Cancle500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "500원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle500Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);
                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (CurrentNormalCarInfo.Cancle100Qty > 0)
                        {
                            CurrentNormalCarInfo.Cancle50Qty = CurrentNormalCarInfo.Cancle50Qty + (CurrentNormalCarInfo.Cancle100Qty * 2); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "100원 장비 미사용 또는 장비고장으로 100원 50원으로 교체:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                            CurrentNormalCarInfo.Cancle100Qty = 0;
                        }
                    }

                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));

                    if (cash100SettingQty < CurrentNormalCarInfo.Cancle100Qty)
                    {
                        CheckCurrentOutCancelMoney();
                    }

                    if (CurrentNormalCarInfo.Cancle100Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "100원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle100Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);
                    }

                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (CurrentNormalCarInfo.Cancle50Qty > 0)
                        {
                            CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty;
                            CurrentNormalCarInfo.Cancle50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.
                    if (cash50SettingQty < CurrentNormalCarInfo.Cancle50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "동전 50원 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty - cash50SettingQty;
                        CurrentNormalCarInfo.Cancle50Qty = cash50SettingQty;
                    }

                    if (CurrentNormalCarInfo.Cancle50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "50원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle50Qty.ToString() + "개");
                        l_resultPayment = CoinOutCancleMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }

                    if (CurrentNormalCarInfo.GetNotDisChargeMoney > 0)
                    {
                        return PaymentResult.Fail;
                    }
                    else
                    {
                        return l_resultPayment;
                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "취소명령시 입금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCancelMoney();

                    CurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할금액: 50PHP수량:" + CurrentNormalCarInfo.Cancle5000Qty.ToString() + "  20pPHP원수량:" + CurrentNormalCarInfo.Cancle1000Qty.ToString() + "  10PHP원수량:" + CurrentNormalCarInfo.Cancle500Qty.ToString() + "  5PHP수량:" + CurrentNormalCarInfo.Cancle100Qty.ToString() + "  1PHP수량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (CurrentNormalCarInfo.Cancle5000Qty > 0 || CurrentNormalCarInfo.Cancle1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (CurrentNormalCarInfo.Cancle1000Qty * 2) + (CurrentNormalCarInfo.Cancle5000Qty * 5);
                            CurrentNormalCarInfo.Cancle1000Qty = 0;
                            CurrentNormalCarInfo.Cancle5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "[지폐대신 동전방출로 변경] 방출할10PHP 수량: " + CurrentNormalCarInfo.Cancle500Qty.ToString());

                        }
                        else
                        {
                            if (CurrentNormalCarInfo.Cancle5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "50PHP권 방출명령 내림:" + CurrentNormalCarInfo.Cancle5000Qty.ToString() + "개");
                                int out5000Qty = CurrentNormalCarInfo.Cancle5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillCancleDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (CurrentNormalCarInfo.Cancle1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "20PHP원권 방출명령 내림:" + CurrentNormalCarInfo.Cancle1000Qty.ToString() + "개");
                                int out1000Qty = CurrentNormalCarInfo.Cancle1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillCancleDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (CurrentNormalCarInfo.Cancle1000Qty * 2) + (CurrentNormalCarInfo.Cancle5000Qty * 5);
                        CurrentNormalCarInfo.Cancle1000Qty = 0;
                        CurrentNormalCarInfo.Cancle5000Qty = 0;
                    }

                    if (CurrentNormalCarInfo.CurrentCancleCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할 동전이 없으므로 성공:" + CurrentNormalCarInfo.CurrentCancleCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        CurrentNormalCarInfo.NotdisChargeMoney500Qty = CurrentNormalCarInfo.Cancle500Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney100Qty = CurrentNormalCarInfo.Cancle100Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty;
                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.

                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 10PHP 방출을 할수없을때
                    {
                        if (CurrentNormalCarInfo.Cancle500Qty > 0) // 10PHP 방출수량이 있다면
                        {
                            CurrentNormalCarInfo.Cancle100Qty = CurrentNormalCarInfo.Cancle100Qty + (CurrentNormalCarInfo.Cancle500Qty * 2); // 부족한 10PHP원 수량을 5PHP원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "10PHP 장비 미사용 또는 장비고장으로 10PHP 5PHP으로 교체:" + CurrentNormalCarInfo.Cancle100Qty.ToString());
                            CurrentNormalCarInfo.Cancle500Qty = 0;
                        }
                    }

                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));

                    if (cash500SettingQty < CurrentNormalCarInfo.Cancle500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();
                    }

                    PaymentResult l_resultPayment = PaymentResult.Success;

                    if (CurrentNormalCarInfo.Cancle500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "10PHP권 방출명령 내림:" + CurrentNormalCarInfo.Cancle500Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);
                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (CurrentNormalCarInfo.Cancle100Qty > 0)
                        {
                            CurrentNormalCarInfo.Cancle50Qty = CurrentNormalCarInfo.Cancle50Qty + (CurrentNormalCarInfo.Cancle100Qty * 5); // 부족한 5PHP 수량을 1PHP권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "5PHP 장비 미사용 또는 장비고장으로 5PHP 1PHP으로 교체:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                            CurrentNormalCarInfo.Cancle100Qty = 0;
                        }
                    }

                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));

                    if (cash100SettingQty < CurrentNormalCarInfo.Cancle100Qty)
                    {
                        CheckCurrentOutCancelMoney();
                    }

                    if (CurrentNormalCarInfo.Cancle100Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "5PHP 방출명령 내림:" + CurrentNormalCarInfo.Cancle100Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);
                    }

                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (CurrentNormalCarInfo.Cancle50Qty > 0)
                        {
                            CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty;
                            CurrentNormalCarInfo.Cancle50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }

                    if (cash50SettingQty < CurrentNormalCarInfo.Cancle50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1PHP 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Cancle50Qty - cash50SettingQty;
                        CurrentNormalCarInfo.Cancle50Qty = cash50SettingQty;
                    }

                    if (CurrentNormalCarInfo.Cancle50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1PHP 방출명령 내림:" + CurrentNormalCarInfo.Cancle50Qty.ToString() + "개");
                        l_resultPayment = CoinOutCancleMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }

                    if (CurrentNormalCarInfo.GetNotDisChargeMoney > 0)
                    {
                        return PaymentResult.Fail;
                    }
                    else
                    {
                        return l_resultPayment;
                    }
                }
                else
                {
                    return PaymentResult.Fail;
                }


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymenu | CancleRefundMoneyAction", ex.ToString());
                CurrentNormalCarInfo.LastErrorMessage = "FormPaymenu | CancleRefundMoneyAction|Exception:" + ex.ToString();
                return PaymentResult.Fail;
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymenu | CancleRefundMoneyAction", "[방출한금액] 5000원수량:" + CurrentNormalCarInfo.OutCome5000Qty.ToString() + "  1000원수량:" + CurrentNormalCarInfo.OutCome1000Qty.ToString() + "  500원수량:" + CurrentNormalCarInfo.OutCome500Qty.ToString() + "  100원수량:" + CurrentNormalCarInfo.OutCome100Qty.ToString() + "  50원수량:" + CurrentNormalCarInfo.OutCome50Qty.ToString());
                NPSYS.NoCheckCargeMoneyOut();
            }
        }


        private PaymentResult CompletOutChargeAction()
        {
            try
            {
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "결제완료 명령시 입금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCompleteMoney();

                    CurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할금액: 5000원수량:" + CurrentNormalCarInfo.Charge5000Qty.ToString() + "  1000원수량:" + CurrentNormalCarInfo.Charge1000Qty.ToString() + "  500원수량:" + CurrentNormalCarInfo.Charge500Qty.ToString() + "  100원수량:" + CurrentNormalCarInfo.Charge100Qty.ToString() + "  50원수량:" + CurrentNormalCarInfo.Charge50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (CurrentNormalCarInfo.Charge5000Qty > 0 || CurrentNormalCarInfo.Charge1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (CurrentNormalCarInfo.Charge1000Qty * 2) + (CurrentNormalCarInfo.Charge5000Qty * 10);
                            CurrentNormalCarInfo.Charge1000Qty = 0;
                            CurrentNormalCarInfo.Charge5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "[지폐대신 동전방출로 변경] 방출할500원 수량: " + CurrentNormalCarInfo.Charge500Qty.ToString());

                        }
                        else
                        {
                            if (CurrentNormalCarInfo.Charge5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "5000원권 방출명령 내림:" + CurrentNormalCarInfo.Charge5000Qty.ToString() + "개");
                                int out5000Qty = CurrentNormalCarInfo.Charge5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillPaymentCompleteDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (CurrentNormalCarInfo.Charge1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "1000원권 방출명령 내림:" + CurrentNormalCarInfo.Charge1000Qty.ToString() + "개");
                                int out1000Qty = CurrentNormalCarInfo.Charge1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillPaymentCompleteDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (CurrentNormalCarInfo.Charge1000Qty * 2) + (CurrentNormalCarInfo.Charge5000Qty * 10);
                        CurrentNormalCarInfo.Charge1000Qty = 0;
                        CurrentNormalCarInfo.Charge5000Qty = 0;

                    }
                    if (CurrentNormalCarInfo.ChargeCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할 동전이 없으므로 성공:" + CurrentNormalCarInfo.ChargeCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        CurrentNormalCarInfo.NotdisChargeMoney500Qty = CurrentNormalCarInfo.Charge500Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney100Qty = CurrentNormalCarInfo.Charge100Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty;
                        CurrentNormalCarInfo.Charge50Qty = 0;
                        CurrentNormalCarInfo.Charge100Qty = 0;
                        CurrentNormalCarInfo.Charge500Qty = 0;

                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.



                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (CurrentNormalCarInfo.Charge500Qty > 0) // 500원 방출수량이 있다면
                        {
                            CurrentNormalCarInfo.Charge100Qty = CurrentNormalCarInfo.Charge100Qty + (CurrentNormalCarInfo.Charge500Qty * 5); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "500원 장비 미사용 또는 장비고장으로 500원 100원으로 교체:" + CurrentNormalCarInfo.Charge100Qty.ToString());
                            CurrentNormalCarInfo.Charge500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < CurrentNormalCarInfo.Charge500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (CurrentNormalCarInfo.Charge500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CompletOutChargeAction", "500원권 방출명령 내림:" + CurrentNormalCarInfo.Charge500Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (CurrentNormalCarInfo.Charge100Qty > 0)
                        {
                            CurrentNormalCarInfo.Charge50Qty = CurrentNormalCarInfo.Charge50Qty + (CurrentNormalCarInfo.Charge100Qty * 2); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "100원 장비 미사용 또는 장비고장으로 100원 50원으로 교체:" + CurrentNormalCarInfo.Charge100Qty.ToString());
                            CurrentNormalCarInfo.Charge100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < CurrentNormalCarInfo.Charge100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (CurrentNormalCarInfo.Charge100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CompletOutChargeAction", "100원권 방출명령 내림:" + CurrentNormalCarInfo.Charge100Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (CurrentNormalCarInfo.Charge50Qty > 0)
                        {
                            CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty;
                            CurrentNormalCarInfo.Charge50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }


                    if (cash50SettingQty < CurrentNormalCarInfo.Charge50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "동전 50원 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + CurrentNormalCarInfo.Charge50Qty.ToString());
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty - cash50SettingQty;
                        CurrentNormalCarInfo.Charge50Qty = cash50SettingQty;
                    }


                    if (CurrentNormalCarInfo.Charge50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "50원권 방출명령 내림:" + CurrentNormalCarInfo.Charge50Qty.ToString() + "개");
                        l_resultPayment = CoinOutChargeMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (CurrentNormalCarInfo.GetNotDisChargeMoney > 0)
                    {
                        return PaymentResult.Fail;
                    }
                    else
                    {
                        return l_resultPayment;
                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "결제완료 명령시 입금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCompleteMoney();

                    CurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할금액: 50PHP수량:" + CurrentNormalCarInfo.Charge5000Qty.ToString() + "  20PHP수량:" + CurrentNormalCarInfo.Charge1000Qty.ToString() + "  10PHP수량:" + CurrentNormalCarInfo.Charge500Qty.ToString() + "  5PHP수량:" + CurrentNormalCarInfo.Charge100Qty.ToString() + "  1PHP수량:" + CurrentNormalCarInfo.Charge50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (CurrentNormalCarInfo.Charge5000Qty > 0 || CurrentNormalCarInfo.Charge1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (CurrentNormalCarInfo.Charge1000Qty * 2) + (CurrentNormalCarInfo.Charge5000Qty * 5);
                            CurrentNormalCarInfo.Charge1000Qty = 0;
                            CurrentNormalCarInfo.Charge5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "[지폐대신 동전방출로 변경] 방출할1PHP 수량: " + CurrentNormalCarInfo.Charge500Qty.ToString());

                        }
                        else
                        {
                            if (CurrentNormalCarInfo.Charge5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "50PHP 방출명령 내림:" + CurrentNormalCarInfo.Charge5000Qty.ToString() + "개");
                                int out5000Qty = CurrentNormalCarInfo.Charge5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillPaymentCompleteDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (CurrentNormalCarInfo.Charge1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "20PHP 방출명령 내림:" + CurrentNormalCarInfo.Charge1000Qty.ToString() + "개");
                                int out1000Qty = CurrentNormalCarInfo.Charge1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillPaymentCompleteDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (CurrentNormalCarInfo.Charge1000Qty * 2) + (CurrentNormalCarInfo.Charge5000Qty * 5);
                        CurrentNormalCarInfo.Charge1000Qty = 0;
                        CurrentNormalCarInfo.Charge5000Qty = 0;

                    }
                    if (CurrentNormalCarInfo.ChargeCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할 동전이 없으므로 성공:" + CurrentNormalCarInfo.ChargeCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        CurrentNormalCarInfo.NotdisChargeMoney500Qty = CurrentNormalCarInfo.Charge500Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney100Qty = CurrentNormalCarInfo.Charge100Qty;
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty;
                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.



                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (CurrentNormalCarInfo.Charge500Qty > 0) // 500원 방출수량이 있다면
                        {
                            CurrentNormalCarInfo.Charge100Qty = CurrentNormalCarInfo.Charge100Qty + (CurrentNormalCarInfo.Charge500Qty * 2); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "10PHP 장비 미사용 또는 장비고장으로 10PHP 5PHP으로 교체:" + CurrentNormalCarInfo.Charge100Qty.ToString());
                            CurrentNormalCarInfo.Charge500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < CurrentNormalCarInfo.Charge500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (CurrentNormalCarInfo.Charge500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CompletOutChargeAction", "10PHP 방출명령 내림:" + CurrentNormalCarInfo.Charge500Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (CurrentNormalCarInfo.Charge100Qty > 0)
                        {
                            CurrentNormalCarInfo.Charge50Qty = CurrentNormalCarInfo.Charge50Qty + (CurrentNormalCarInfo.Charge100Qty * 5); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "5PHP 장비 미사용 또는 장비고장으로 5PHP 1PHP원으로 교체:" + CurrentNormalCarInfo.Charge50Qty.ToString());
                            CurrentNormalCarInfo.Charge100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < CurrentNormalCarInfo.Charge100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (CurrentNormalCarInfo.Charge100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CompletOutChargeAction", "5PHP 방출명령 내림:" + CurrentNormalCarInfo.Charge100Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (CurrentNormalCarInfo.Charge50Qty > 0)
                        {
                            CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty;
                            CurrentNormalCarInfo.Charge50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }


                    if (cash50SettingQty < CurrentNormalCarInfo.Charge50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "1PHP 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + CurrentNormalCarInfo.Charge50Qty.ToString());
                        CurrentNormalCarInfo.NotdisChargeMoney50Qty = CurrentNormalCarInfo.Charge50Qty - cash50SettingQty;
                        CurrentNormalCarInfo.Charge50Qty = cash50SettingQty;
                    }


                    if (CurrentNormalCarInfo.Charge50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "1PHP 방출명령 내림:" + CurrentNormalCarInfo.Charge50Qty.ToString() + "개");
                        l_resultPayment = CoinOutChargeMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (CurrentNormalCarInfo.GetNotDisChargeMoney > 0)
                    {
                        return PaymentResult.Fail;
                    }
                    else
                    {
                        return l_resultPayment;
                    }
                }
                else
                {
                    return PaymentResult.Fail;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymenu | CompletOutChargeAction", ex.ToString());
                CurrentNormalCarInfo.LastErrorMessage = "FormPaymenu | CompletOutChargeAction | Exception:" + ex.ToString();
                return PaymentResult.Fail;
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymenu | CompletOutChargeAction", "[방출한금액] 5000원수량:" + CurrentNormalCarInfo.OutCome5000Qty.ToString() + "  1000원수량:" + CurrentNormalCarInfo.OutCome1000Qty.ToString() + "  500원수량:" + CurrentNormalCarInfo.OutCome500Qty.ToString() + "  100원수량:" + CurrentNormalCarInfo.OutCome100Qty.ToString() + "  50원수량:" + CurrentNormalCarInfo.OutCome50Qty.ToString());
                NPSYS.NoCheckCargeMoneyOut();
            }
        }

        /// <summary>
        /// 결제완료후 거스름돈 처리 동전방출이 실패한다면 현재 방출하지 않은 동전을 다음 동전으로 넘긴다
        /// </summary>
        /// <param name="pMoneyType"></param>
        /// <param name="logDate"></param>
        /// <param name="pCoinSettingQty"></param>
        /// <param name="p_payresult"></param>
        /// <returns></returns>
        private PaymentResult CoinOutChargeMoneyDischarge(MoneyType pMoneyType, string logDate, int pCoinSettingQty, PaymentResult p_payresult)
        {
            int output_RequesetCointQty = 0; // 방출요청수량
            int output_ResponeCointQty = 0;  // 방출된 수량

            CoinDispensor currentCoinDispenser = null;
            switch (pMoneyType)
            {
                case MoneyType.Coin500:
                    output_RequesetCointQty = CurrentNormalCarInfo.Charge500Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor500;
                    break;
                case MoneyType.Coin100:
                    output_RequesetCointQty = CurrentNormalCarInfo.Charge100Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor100;
                    break;
                case MoneyType.Coin50:
                    output_RequesetCointQty = CurrentNormalCarInfo.Charge50Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor50;
                    break;

            }

            CoinDispensor.CoinDispensorStatusType CoinDispensortResult = NPSYS.OutChargeCoin(currentCoinDispenser, output_RequesetCointQty, ref output_ResponeCointQty);
            if (CoinDispensortResult != CoinDispensor.CoinDispensorStatusType.OK)
            {


                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormPaymentMenu | CoinOutChargeMoneyDischarge", CoinDispensortResult.ToString());
                        break;
                    case MoneyType.Coin100:
                        TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormPaymentMenu | CoinOutChargeMoneyDischarge", CoinDispensortResult.ToString());
                        break;
                    case MoneyType.Coin50:
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CoinOutChargeMoneyDischarge", CoinDispensortResult.ToString());
                        break;

                }



            }
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 500, "");
                        CurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 500원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 500원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "500원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        CurrentNormalCarInfo.Charge100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 100원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "500원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름500원권100원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 100원권 방출해야할 수량:" + (CurrentNormalCarInfo.Charge100Qty).ToString());
                        CurrentNormalCarInfo.Charge500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 100, "");
                        CurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 100원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "100원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        CurrentNormalCarInfo.Charge50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 50원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름100원권50원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 50원권 방출해야할 수량:" + (CurrentNormalCarInfo.Charge50Qty).ToString());
                        CurrentNormalCarInfo.Charge100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 50, "");
                        CurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "50원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        CurrentNormalCarInfo.Charge50Qty = 0;

                        CurrentNormalCarInfo.NotdisChargeMoney50Qty += (output_RequesetCointQty - output_ResponeCointQty); // //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "50원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (CurrentNormalCarInfo.NotdisChargeMoney50Qty * 50).ToString());

                        break;
                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 10, "");
                        CurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 10PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 10PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Charge100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 5PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "10PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름10PHP권5PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 5PHP권 방출해야할 수량:" + (CurrentNormalCarInfo.Charge100Qty).ToString());
                        CurrentNormalCarInfo.Charge500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 5, "");
                        CurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 5PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Charge50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 1PHP원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "5PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름5PHP권 1PHP으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 1PHP 방출해야할 수량:" + (CurrentNormalCarInfo.Charge50Qty).ToString());
                        CurrentNormalCarInfo.Charge100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 1, "");
                        CurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Charge50Qty = 0;

                        CurrentNormalCarInfo.NotdisChargeMoney50Qty += CurrentNormalCarInfo.NotdisChargeMoney50Qty + (output_RequesetCointQty - output_ResponeCointQty);
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "1PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (CurrentNormalCarInfo.NotdisChargeMoney50Qty * 1).ToString());

                        break;
                }
            }

            return (output_ResponeCointQty == output_RequesetCointQty ? PaymentResult.Success : PaymentResult.Fail); // 요청금액이랑 방출금액이 다르면 실패
        }

        /// <summary>
        /// 취소시 동전방출 처리 동전방출이 실패한다면 현재 방출하지 않은 동전을 다음 동전으로 넘긴다
        /// </summary>
        /// <param name="pMoneyType"></param>
        /// <param name="logDate"></param>
        /// <param name="pCoinSettingQty"></param>
        /// <param name="p_payresult"></param>
        /// <returns></returns>
        private PaymentResult CoinOutCancleMoneyDischarge(MoneyType pMoneyType, string logDate, int pCoinSettingQty, PaymentResult p_payresult)
        {
            int output_RequesetCointQty = 0; // 방출요청수량
            int output_ResponeCointQty = 0;  // 방출된 수량

            CoinDispensor currentCoinDispenser = null;
            switch (pMoneyType)
            {
                case MoneyType.Coin500:
                    output_RequesetCointQty = CurrentNormalCarInfo.Cancle500Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor500;
                    break;
                case MoneyType.Coin100:
                    output_RequesetCointQty = CurrentNormalCarInfo.Cancle100Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor100;
                    break;
                case MoneyType.Coin50:
                    output_RequesetCointQty = CurrentNormalCarInfo.Cancle50Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor50;
                    break;

            }

            CoinDispensor.CoinDispensorStatusType CoinDispensortResult = NPSYS.OutChargeCoin(currentCoinDispenser, output_RequesetCointQty, ref output_ResponeCointQty);
            if (CoinDispensortResult != CoinDispensor.CoinDispensorStatusType.OK)
            {

                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormPaymentMenu | CoinOutCancleMoneyDischarge", CoinDispensortResult.ToString());
                        break;
                    case MoneyType.Coin100:
                        TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormPaymentMenu | CoinOutCancleMoneyDischarge", CoinDispensortResult.ToString());
                        break;
                    case MoneyType.Coin50:
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CoinOutCancleMoneyDischarge", CoinDispensortResult.ToString());
                        break;

                }

                //}

            }
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 500, "");
                        CurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 500원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 500원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "500원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());

                        CurrentNormalCarInfo.Cancle100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 100원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "500원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름500원권100원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 100원권 방출해야할 수량:" + (CurrentNormalCarInfo.Cancle100Qty).ToString());
                        CurrentNormalCarInfo.Cancle500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 100, "");
                        CurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 100원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "100원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        CurrentNormalCarInfo.Cancle50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 50원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름100원권50원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 50원권 방출해야할 수량:" + (CurrentNormalCarInfo.Cancle50Qty).ToString());
                        CurrentNormalCarInfo.Cancle100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 50, "");
                        CurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "50원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        CurrentNormalCarInfo.Cancle50Qty = 0;

                        CurrentNormalCarInfo.NotdisChargeMoney50Qty += (output_RequesetCointQty - output_ResponeCointQty); // //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "50원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (CurrentNormalCarInfo.NotdisChargeMoney50Qty * 50).ToString());

                        break;
                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 10, "");
                        CurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 10PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 10PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Cancle100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 5PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "10PHP권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름10PHP권5PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 5PHP 방출해야할 수량:" + (CurrentNormalCarInfo.Cancle100Qty).ToString());
                        CurrentNormalCarInfo.Cancle500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 5, "");
                        CurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 5PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); //5PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Cancle50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 1PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름5PHP권1PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 1PHP 방출해야할 수량:" + (CurrentNormalCarInfo.Cancle50Qty).ToString());
                        CurrentNormalCarInfo.Cancle100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 1, "");
                        CurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; //1PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 1PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        CurrentNormalCarInfo.Cancle50Qty = 0;

                        CurrentNormalCarInfo.NotdisChargeMoney50Qty += CurrentNormalCarInfo.NotdisChargeMoney50Qty + (output_RequesetCointQty - output_ResponeCointQty);
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "1PHP권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (CurrentNormalCarInfo.NotdisChargeMoney50Qty * 1).ToString());

                        break;
                }
            }



            return (output_ResponeCointQty == output_RequesetCointQty ? PaymentResult.Success : PaymentResult.Fail); // 요청금액이랑 방출금액이 다르면 실패
        }

        /// <summary>
        /// 5000원 취소금액방출
        /// </summary>
        /// <param name="logDate"></param>
        /// <param name="pNotDischarge5000Qty"></param>
        /// <param name="_result"></param>
        private void BillCancleDischarge5000(string logDate, int pNotDischarge5000Qty, Result _result)
        {
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                int outSuccess5000Qty = CurrentNormalCarInfo.Cancle5000Qty - pNotDischarge5000Qty; //5000원권 배출 성공한수량을 저장
                CurrentNormalCarInfo.Cancle5000Qty = 0;
                CurrentNormalCarInfo.OutCome5000Qty = CurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 5000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillCancleDischarge5000", "5000원 보유시제변경:" + (cash5000SettingQty - outSuccess5000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 5000, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge5000", "5000원권 불출안됨으로 1000원권으로수량변경:" + (pNotDischarge5000Qty * 5).ToString());
                    CurrentNormalCarInfo.Cancle1000Qty = CurrentNormalCarInfo.Cancle1000Qty + pNotDischarge5000Qty * 5;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess5000Qty = CurrentNormalCarInfo.Cancle5000Qty - pNotDischarge5000Qty; //50PHP 배출 성공한수량을 저장
                CurrentNormalCarInfo.Cancle5000Qty = 0;
                CurrentNormalCarInfo.OutCome5000Qty = CurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 50PHP 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 50, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    int php50 = pNotDischarge5000Qty;
                    int php50Fee = php50 * 50;
                    int php20 = php50Fee / 20;
                    int php10 = (php50Fee - (php20 * 20)) / 10;
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge5000", "50PHP 불출안됨으로 20PHP으로수량변경:" + (php20).ToString() + "10PHP으로수량변경:" + php10.ToString());
                    CurrentNormalCarInfo.Cancle1000Qty = CurrentNormalCarInfo.Cancle1000Qty + php20;
                    CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + php10;

                }

            }
        }

        /// <summary>
        /// 5000원 거스름돈 방출
        /// </summary>
        /// <param name="logDate"></param>
        /// <param name="pNotDischarge5000Qty"></param>
        /// <param name="_result"></param>
        private void BillPaymentCompleteDischarge5000(string logDate, int pNotDischarge5000Qty, Result _result)
        {
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                int outSuccess5000Qty = CurrentNormalCarInfo.Charge5000Qty - pNotDischarge5000Qty; //5000원권 배출 성공한수량을 저장
                CurrentNormalCarInfo.Charge5000Qty = 0;
                CurrentNormalCarInfo.OutCome5000Qty = CurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 5000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillPaymentCompleteDischarge5000", "5000원 보유시제변경:" + (cash5000SettingQty - outSuccess5000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 5000, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge5000", "5000원권 불출안됨으로 1000원권으로수량변경:" + (pNotDischarge5000Qty * 5).ToString());
                    CurrentNormalCarInfo.Charge1000Qty = CurrentNormalCarInfo.Charge1000Qty + pNotDischarge5000Qty * 5;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess5000Qty = CurrentNormalCarInfo.Charge5000Qty - pNotDischarge5000Qty; //50PHP 배출 성공한수량을 저장
                CurrentNormalCarInfo.Charge5000Qty = 0;
                CurrentNormalCarInfo.OutCome5000Qty = CurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 50PHP 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 50, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    int php50 = pNotDischarge5000Qty;
                    int php50Fee = php50 * 50;
                    int php20 = php50Fee / 20;
                    int php10 = (php50Fee - (php20 * 20)) / 10;
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge5000", "50PHP 불출안됨으로 20PHP권으로수량변경:" + php20.ToString() + "10PHP권으로수량변경:" + php10.ToString());
                    CurrentNormalCarInfo.Charge1000Qty = CurrentNormalCarInfo.Charge1000Qty + php20;
                    CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + php10;

                }
            }
        }


        /// <summary>
        /// 1000원 취소금액방출
        /// </summary>
        /// <param name="logDate"></param>
        /// <param name="pNotDischarge1000Qty"></param>
        /// <param name="_result"></param>
        private void BillCancleDischarge1000(string logDate, int pNotDischarge1000Qty, Result _result)
        {
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                int outSuccess1000Qty = CurrentNormalCarInfo.Cancle1000Qty - pNotDischarge1000Qty; //1000원권 배출 성공한수량을 저장
                CurrentNormalCarInfo.Cancle1000Qty = 0;
                CurrentNormalCarInfo.OutCome1000Qty = CurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 1000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillCancleDischarge1000", "1000원 보유시제변경:" + (cash1000SettingQty - outSuccess1000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 1000, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge1000", "1000원권 불출안됨으로 500원권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (pNotDischarge1000Qty * 2);

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess1000Qty = CurrentNormalCarInfo.Cancle1000Qty - pNotDischarge1000Qty; //20PHP 배출 성공한수량을 저장
                CurrentNormalCarInfo.Cancle1000Qty = 0;
                CurrentNormalCarInfo.OutCome1000Qty = CurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 20PHP권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 20, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge1000", "20PHP 불출안됨으로 10PHP권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (pNotDischarge1000Qty * 2);

                }
            }
        }
        /// <summary>
        /// 1000원 거스름돈 방출
        /// </summary>
        /// <param name="logDate"></param>
        /// <param name="pNotDischarge1000Qty"></param>
        /// <param name="_result"></param>
        private void BillPaymentCompleteDischarge1000(string logDate, int pNotDischarge1000Qty, Result _result)
        {
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                int outSuccess1000Qty = CurrentNormalCarInfo.Charge1000Qty - pNotDischarge1000Qty; //1000원권 배출 성공한수량을 저장
                CurrentNormalCarInfo.Charge1000Qty = 0;
                CurrentNormalCarInfo.OutCome1000Qty = CurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 1000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillPaymentCompleteDischarge1000", "1000원 보유시제변경:" + (cash1000SettingQty - outSuccess1000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 1000, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge1000", "1000원권 불출안됨으로 500원권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + pNotDischarge1000Qty * 2;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess1000Qty = CurrentNormalCarInfo.Charge1000Qty - pNotDischarge1000Qty; //20PHP 배출 성공한수량을 저장
                CurrentNormalCarInfo.Charge1000Qty = 0;
                CurrentNormalCarInfo.OutCome1000Qty = CurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 20PHP권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, CurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 20, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge1000", "20PHP 불출안됨으로 10PHP권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + pNotDischarge1000Qty * 2;

                }

            }
        }



        /// <summary>
        /// 현재 금액에 대한 거스름돈 반환 가능 검사
        /// </summary>
        /// <returns></returns>
        public bool CheckCurrentOutCompleteMoney()
        {
            try
            {
                ////////////////
                // 거스름돈 검사
                ////////////////
                int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
                int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));
                int cash50MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MinQty));
                int cash100MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MinQty));
                int cash500MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MinQty));
                int cash1000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MinQty));
                int cash5000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MinQty));

                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름검사시작 여유액: 5000원수량:" + cash5000SettingQty.ToString() + "  1000원수량:" + cash1000SettingQty.ToString() + "  500원수량:" + cash500SettingQty.ToString() + "  100원수량:" + cash100SettingQty.ToString() + "  50원수량:" + cash50SettingQty.ToString());
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름검사시작 여유액: 50PHP수량:" + cash5000SettingQty.ToString() + "  20PHP수량:" + cash1000SettingQty.ToString() + "  10PHP:" + cash500SettingQty.ToString() + "  5PHP수량:" + cash100SettingQty.ToString() + "  1PHP:" + cash50SettingQty.ToString());
                }

                if ((cash5000SettingQty >= CurrentNormalCarInfo.Charge5000Qty) &&
                    (cash1000SettingQty >= CurrentNormalCarInfo.Charge1000Qty) &&
                    (cash500SettingQty >= CurrentNormalCarInfo.Charge500Qty) &&
                    (cash100SettingQty >= CurrentNormalCarInfo.Charge100Qty) &&
                    (cash50SettingQty >= CurrentNormalCarInfo.Charge50Qty))
                {
                    return true;
                }
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    if (cash5000SettingQty < CurrentNormalCarInfo.Charge5000Qty)  // 5000원권이 부죽한 경우 1000원권으로 넘김
                    {
                        int lack5000Qty = CurrentNormalCarInfo.Charge5000Qty - cash5000SettingQty;  // 5000원권의 부족한 수량

                        CurrentNormalCarInfo.Charge5000Qty = cash5000SettingQty; // 현재 남아있는 5000원권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Charge1000Qty = CurrentNormalCarInfo.Charge1000Qty + (lack5000Qty * 5); // 부족한 5000원 수량을 1000원권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:5000원권 부족으로 1000원권으로 동전교환-현보유1000수량:" + cash5000SettingQty.ToString() + " 1000원수량:" + CurrentNormalCarInfo.Charge1000Qty.ToString());

                    }

                    if (cash1000SettingQty < CurrentNormalCarInfo.Charge1000Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                    {
                        int lack1000Qty = CurrentNormalCarInfo.Charge1000Qty - cash1000SettingQty; // 1000원권의 부족한 수량

                        CurrentNormalCarInfo.Charge1000Qty = cash1000SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (lack1000Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:1000원권 부족으로 500원권으로 동전교환-현보유1000수량:" + cash1000SettingQty.ToString() + " 500원수량:" + CurrentNormalCarInfo.Charge500Qty.ToString());

                    }

                    if (cash500SettingQty < CurrentNormalCarInfo.Charge500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = CurrentNormalCarInfo.Charge500Qty - cash500SettingQty; // 500원권의 부족한 수량
                        CurrentNormalCarInfo.Charge500Qty = cash500SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Charge100Qty = CurrentNormalCarInfo.Charge100Qty + (lack500Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:500원권 부족으로 100원권으로 동전교환-현보유500수량:" + cash500SettingQty.ToString() + " 100원수량:" + CurrentNormalCarInfo.Charge100Qty.ToString());
                    }

                    if (cash100SettingQty < CurrentNormalCarInfo.Charge100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = CurrentNormalCarInfo.Charge100Qty - cash100SettingQty; // 1000원권의 부족한 수량
                        CurrentNormalCarInfo.Charge100Qty = cash100SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Charge50Qty = CurrentNormalCarInfo.Charge50Qty + (lack100Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:100원권 부족으로 50원권으로 동전교환-현보유100수량:" + cash100SettingQty.ToString() + " 50원수량:" + CurrentNormalCarInfo.Charge50Qty.ToString());

                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    if (cash5000SettingQty < CurrentNormalCarInfo.Charge5000Qty)  // 50PHP권이 부죽한 경우 20PHP원권으로 넘김
                    {
                        int lack5000Qty = CurrentNormalCarInfo.Charge5000Qty - cash5000SettingQty;  // 50PHP권의 부족한 수량
                        int php50 = lack5000Qty;
                        int php50Fee = php50 * 50;
                        int php20 = php50Fee / 20;
                        int php10 = (php50Fee - (php20 * 20)) / 10;
                        CurrentNormalCarInfo.Charge5000Qty = cash5000SettingQty; // 현재 남아있는 50PHP 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Charge1000Qty = CurrentNormalCarInfo.Charge1000Qty + (php20); // 부족한 50PHP 수량을 20PHP 수량에 더함
                        CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (php10); // 부족한 50PHP원 수량을 10PHP권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:50PHP 부족으로 20PHP권으로 동전교환-현보유50PHP수량:" + cash5000SettingQty.ToString() + " 20PHP수량:" + CurrentNormalCarInfo.Charge1000Qty.ToString());

                    }

                    if (cash1000SettingQty < CurrentNormalCarInfo.Charge1000Qty) // 20PHP원권이 부족한 경우 10PHP원 으로 넘김
                    {
                        int lack1000Qty = CurrentNormalCarInfo.Charge1000Qty - cash1000SettingQty; // 20PHP원권의 부족한 수량

                        CurrentNormalCarInfo.Charge1000Qty = cash1000SettingQty; // 현재 남아있는 20PHP권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Charge500Qty = CurrentNormalCarInfo.Charge500Qty + (lack1000Qty * 2); // 부족한 20PHP 수량을 10PHP원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:20PHP원권 부족으로 10PHP권으로 동전교환-현보유20PHP수량:" + cash1000SettingQty.ToString() + " 10PHP수량:" + CurrentNormalCarInfo.Charge500Qty.ToString());

                    }

                    if (cash500SettingQty < CurrentNormalCarInfo.Charge500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = CurrentNormalCarInfo.Charge500Qty - cash500SettingQty; // 10PHP 부족한 수량
                        CurrentNormalCarInfo.Charge500Qty = cash500SettingQty; // 현재 남아있는 10PHP 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Charge100Qty = CurrentNormalCarInfo.Charge100Qty + (lack500Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 동전부족:10PHP 부족으로 5PHP권으로 동전교환-현보유10PHP수량:" + cash500SettingQty.ToString() + " 5PHP수량:" + CurrentNormalCarInfo.Charge100Qty.ToString());
                    }

                    if (cash100SettingQty < CurrentNormalCarInfo.Charge100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = CurrentNormalCarInfo.Charge100Qty - cash100SettingQty; // 5PHP원권의 부족한 수량
                        CurrentNormalCarInfo.Charge100Qty = cash100SettingQty; // 현재 남아있는 5PHP권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Charge50Qty = CurrentNormalCarInfo.Charge50Qty + (lack100Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 동전부족:5PHP권 부족으로 1PHP원권으로 동전교환-현보유5PHP수량:" + cash100SettingQty.ToString() + " 1PHP수량:" + CurrentNormalCarInfo.Charge50Qty.ToString());

                    }
                }
                NPSYS.NoCheckCargeMoneyOut();



                return true;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|CheckCurrentMoneyOut", ex.ToString());
                return false;
            }
        }


        /// <summary>
        /// 현재 금액에 대한 취소 반환 가능 검사
        /// </summary>
        /// <returns></returns>
        public bool CheckCurrentOutCancelMoney()
        {
            try
            {
                ////////////////
                // 거스름돈 검사
                ////////////////
                int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
                int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));
                int cash50MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MinQty));
                int cash100MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MinQty));
                int cash500MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MinQty));
                int cash1000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MinQty));
                int cash5000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MinQty));

                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름검사시작 여유액: 5000원수량:" + cash5000SettingQty.ToString() + "  1000원수량:" + cash1000SettingQty.ToString() + "  500원수량:" + cash500SettingQty.ToString() + "  100원수량:" + cash100SettingQty.ToString() + "  50원수량:" + cash50SettingQty.ToString());
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름검사시작 여유액: 50PHP수량:" + cash5000SettingQty.ToString() + "  20PHP수량:" + cash1000SettingQty.ToString() + "  10PHP수량:" + cash500SettingQty.ToString() + "  5PHP수량:" + cash100SettingQty.ToString() + "  1PHP수량:" + cash50SettingQty.ToString());
                }

                if ((cash5000SettingQty >= CurrentNormalCarInfo.Cancle5000Qty) &&
                    (cash1000SettingQty >= CurrentNormalCarInfo.Cancle1000Qty) &&
                    (cash500SettingQty >= CurrentNormalCarInfo.Cancle500Qty) &&
                    (cash100SettingQty >= CurrentNormalCarInfo.Cancle100Qty) &&
                    (cash50SettingQty >= CurrentNormalCarInfo.Cancle50Qty))
                {
                    return true;
                }
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    if (cash5000SettingQty < CurrentNormalCarInfo.Cancle5000Qty)  // 5000원권이 부죽한 경우 1000원권으로 넘김
                    {
                        int lack5000Qty = CurrentNormalCarInfo.Cancle5000Qty - cash5000SettingQty;  // 5000원권의 부족한 수량

                        CurrentNormalCarInfo.Cancle5000Qty = cash5000SettingQty; // 현재 남아있는 5000원권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Cancle1000Qty = CurrentNormalCarInfo.Cancle1000Qty + (lack5000Qty * 5); // 부족한 5000원 수량을 1000원권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:5000원권 부족으로 1000원권으로 동전교환-현보유1000수량:" + cash5000SettingQty.ToString() + " 1000원수량:" + CurrentNormalCarInfo.Cancle1000Qty.ToString());

                    }

                    if (cash1000SettingQty < CurrentNormalCarInfo.Cancle1000Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                    {
                        int lack1000Qty = CurrentNormalCarInfo.Cancle1000Qty - cash1000SettingQty; // 1000원권의 부족한 수량

                        CurrentNormalCarInfo.Cancle1000Qty = cash1000SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (lack1000Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:1000원권 부족으로 500원권으로 동전교환-현보유1000수량:" + cash1000SettingQty.ToString() + " 500원수량:" + CurrentNormalCarInfo.Cancle500Qty.ToString());

                    }

                    if (cash500SettingQty < CurrentNormalCarInfo.Cancle500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = CurrentNormalCarInfo.Cancle500Qty - cash500SettingQty; // 500원권의 부족한 수량
                        CurrentNormalCarInfo.Cancle500Qty = cash500SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Cancle100Qty = CurrentNormalCarInfo.Cancle100Qty + (lack500Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:500원권 부족으로 100원권으로 동전교환-현보유500수량:" + cash500SettingQty.ToString() + " 100원수량:" + CurrentNormalCarInfo.Cancle100Qty.ToString());
                    }

                    if (cash100SettingQty < CurrentNormalCarInfo.Cancle100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = CurrentNormalCarInfo.Cancle100Qty - cash100SettingQty; // 1000원권의 부족한 수량
                        CurrentNormalCarInfo.Cancle100Qty = cash100SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Cancle50Qty = CurrentNormalCarInfo.Cancle50Qty + (lack100Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:100원권 부족으로 50원권으로 동전교환-현보유100수량:" + cash100SettingQty.ToString() + " 50원수량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());

                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    if (cash5000SettingQty < CurrentNormalCarInfo.Cancle5000Qty)  // 50PHP권이 부죽한 경우 20PHP으로 넘김
                    {
                        int lack5000Qty = CurrentNormalCarInfo.Cancle5000Qty - cash5000SettingQty;  // 50PHP권의 부족한 수량
                        int php50 = lack5000Qty;
                        int php50Fee = php50 * 50;
                        int php20 = php50Fee / 20;
                        int php10 = (php50Fee - (php20 * 20)) / 10;
                        CurrentNormalCarInfo.Cancle5000Qty = cash5000SettingQty; // 현재 남아있는 50PHP권 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Cancle1000Qty = CurrentNormalCarInfo.Cancle1000Qty + (php20); // 부족한 5000원 수량을 1000원권 수량에 더함
                        CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + php10;
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:50PHP 부족으로 20PHP로 교환 현보유50PHP수량:" + cash5000SettingQty.ToString() + " 20PHP수량:" + CurrentNormalCarInfo.Cancle1000Qty.ToString());

                    }

                    if (cash1000SettingQty < CurrentNormalCarInfo.Cancle1000Qty) // 20PHP권이 부족한 경우 10PHP원 으로 넘김
                    {
                        int lack1000Qty = CurrentNormalCarInfo.Cancle1000Qty - cash1000SettingQty; // 20PHP의 부족한 수량

                        CurrentNormalCarInfo.Cancle1000Qty = cash1000SettingQty; // 현재 남아있는 20PHP 수량을 거스름돈으로 대체
                        CurrentNormalCarInfo.Cancle500Qty = CurrentNormalCarInfo.Cancle500Qty + (lack1000Qty * 2); // 부족한 20PHP 수량을 10PHP원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:20PHP원권 부족으로 10PHP원권으로 동전교환-현보유20PHP수량:" + cash1000SettingQty.ToString() + " 10PHP원수량:" + CurrentNormalCarInfo.Cancle500Qty.ToString());

                    }

                    if (cash500SettingQty < CurrentNormalCarInfo.Cancle500Qty) // 10PHP원 권에 대해서만 처리, 
                    {
                        int lack500Qty = CurrentNormalCarInfo.Cancle500Qty - cash500SettingQty; // 10PHP원권의 부족한 수량
                        CurrentNormalCarInfo.Cancle500Qty = cash500SettingQty; // 현재 남아있는 10PHP권 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Cancle100Qty = CurrentNormalCarInfo.Cancle100Qty + (lack500Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 동전부족:10PHP 부족으로 5PHP 동전교환-현보유10PHP:" + cash500SettingQty.ToString() + " 5PHP:" + CurrentNormalCarInfo.Cancle100Qty.ToString());
                    }

                    if (cash100SettingQty < CurrentNormalCarInfo.Cancle100Qty) // 5PHP원 권에 대해서만 처리
                    {
                        int lack100Qty = CurrentNormalCarInfo.Cancle100Qty - cash100SettingQty; // 5PHP원권의 부족한 수량
                        CurrentNormalCarInfo.Cancle100Qty = cash100SettingQty; // 현재 남아있는 5PHP 수량을 거스름돈으로 대체                        
                        CurrentNormalCarInfo.Cancle50Qty = CurrentNormalCarInfo.Cancle50Qty + (lack100Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:5PHP원권 부족으로 1PHP원권으로 동전교환-현보유5PHP:" + cash100SettingQty.ToString() + " 5PHP수량:" + CurrentNormalCarInfo.Cancle50Qty.ToString());

                    }
                }
                NPSYS.NoCheckCargeMoneyOut();



                return true;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | CheckCurrentOutCancelMoney", ex.ToString());
                return false;
            }
        }

        #endregion 거스름돈 / 취소시 방출관련

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
            paymentControl.SetLanguage(pLanguageType, CurrentNormalCarInfo);

            //m_DiscountAndPayAndCreditAndTMoneyMovie = "할인권_현금_신용카드_교통카드.avi";
            m_DiscountAndPayAndCreditAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Cash_Card_Tmoney.ToString());

            //m_PayAndCreditAndTMoneyMovie = "현금_신용카드_교통카드.avi";
            m_PayAndCreditAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Cash_Card_Tmoney.ToString());
            //m_DiscountAndCreditAndTMoneyMovie = "할인권_신용카드_교통카드.avi";
            m_DiscountAndCreditAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Card_Tmoney.ToString());

            //m_CreditAndTmoneyMovie = "신용카드_교통카드.avi";
            m_CreditAndTmoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Card_Tmoney.ToString());

            // m_DiscountAndPayAndCreditMovie = "할인권_현금_신용카드.avi";
            m_DiscountAndPayAndCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Cash_Card.ToString());

            //m_PayAndCreditMovie = "현금_신용카드.avi";
            m_PayAndCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Cash_Card.ToString());

            //m_DiscountAndPayMovie = "할인권_현금.avi";
            m_DiscountAndPayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Cash.ToString());

            // m_PayMovie = "현금.avi";
            m_PayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Cash.ToString());

            //m_CreditMovie = "신용카드.avi";
            m_CreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Card.ToString());

            //m_DiscountAndCreditMovie = "할인권_신용카드.avi";
            m_DiscountAndCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Card.ToString());

            //m_DiscountAndTmoneyMovie = "할인권_교통카드.avi";
            m_DiscountAndTmoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount_Tmoney.ToString());

            //m_TmoneyMovie = "교통카드.avi";
            m_TmoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Tmoney.ToString());


            //m_DiscountMovie = "할인권.avi";
            m_DiscountMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Discount.ToString());

            //m_JuminDIscountMovie = "감면혜택.wav";
            m_JuminDIscountMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DiscountReduce.ToString());

            // m_Junggi = "정기권연장.wav";
            m_Junggi = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_CommuterExtendedPeriod.ToString());

            //m_CancleCard = "카드취소.wav";
            m_CancleCard = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_CardCancle.ToString());

            //m_DiscountBarcodeCreditMovie = "할인권바코드_신용카드.wav";
            m_DiscountBarcodeCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DiscountBarcode_Card.ToString());

            //m_BarAndDiscountAndCreditAndPayAndTMoneyMovie = "바코드_할인권_신용카드_현금_교통카드.wav";
            m_BarAndDiscountAndCreditAndPayAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Card_Cash_Tmoney.ToString());

            //m_BarAndCreditAndPayAndTMoneyMovie = "바코드_신용카드_현금_교통카드.wav";
            m_BarAndCreditAndPayAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Card_Cash_Tmoney.ToString());

            //m_BarAndDiscountAndPayAndTMoneyMovie = "바코드_할인권_현금_교통카드.wav";
            m_BarAndDiscountAndPayAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Cash_Tmoney.ToString());

            //m_BarAndPayAndTMoneyMovie = "바코드_현금_교통카드.wav";
            m_BarAndPayAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Cash_Tmoney.ToString());

            //m_BarAndDiscountAndCreditAndPayMovie = "바코드_할인권_신용카드_현금.wav";
            m_BarAndDiscountAndCreditAndPayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Card_Cash.ToString());

            //m_BarAndCreditAndPayMovie = "바코드_신용카드_현금.wav";
            m_BarAndCreditAndPayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Card_Cash.ToString());

            //m_BarAndDiscountAndPayMovie = "바코드_할인권_현금.wav";
            m_BarAndDiscountAndPayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Cash.ToString());

            // m_BarAndPayMovie = "바코드_현금.wav";
            m_BarAndPayMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Cash.ToString());

            //m_BarAndDiscountAndCreditAndTMoneyMovie = "바코드_할인권_신용카드_교통카드.wav";
            m_BarAndDiscountAndCreditAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Card_Tmoney.ToString());

            //m_BarAndCreditAndTMoneyMovie = "바코드_신용카드_교통카드.wav";
            m_BarAndCreditAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Card_Tmoney.ToString());

            //m_BarAndDiscountAndTMoneyMovie = "바코드_할인권_교통카드.wav";
            m_BarAndDiscountAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Tmoney.ToString());

            //m_BarAndDiscountAndCreditMovie = "바코드_할인권_신용카드.wav";
            m_BarAndDiscountAndCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Discount_Card.ToString());

            //m_BarAndCreditMovie = "바코드_신용카드.wav";
            m_BarAndCreditMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Card.ToString());

            //m_BarAndTMoneyMovie = "바코드_교통카드.wav";
            m_BarAndTMoneyMovie = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DBarcode_Tmoney.ToString());

            Action_DeviceEnableMovie();
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

        #endregion 언어변경

        #region 정기권처리
        //정기권관련기능(만료요금부과/연장관련)
        private void btnRegExtension_Click(object sender, EventArgs e)
        {
            RegExtension();
        }
        // 정기권관련기능(만료요금부과/연장관련)
        /// <summary>
        /// 일반차량 정기권 연장시 사용
        /// </summary>
        private void RegExtension()
        {
            //TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | RegExtension", "[정기권연장버튼누름]");
            //if (mNormalCarInfo.CurrentMoney > 0)
            //{
            //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | RegExtension", "현금취소 후 가능합니다.");
            //    paymentControl.ErrorMessage = "현금취소 후 가능합니다";
            //}
            //if (mNormalCarInfo.CarNormalRegType == NormalCarInfo.CarNormalRegTypes.REG_EXTENSION)
            //{
            //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | RegExtension", "[정기권 연장요금제에서 일반요금제로 변경]");
            //    mNormalCarInfo.CarNormalRegType = mNormalCarInfo.PreCarNormalRegTypes;
            //    mNormalCarInfo.TkNO = mNormalCarInfo.NormalTkNo;
            //    mNormalCarInfo.ParkMoney = mNormalCarInfo.PreParkMoney;
            //    mNormalCarInfo.DiscountMoney = mNormalCarInfo.PreDiscountMoney;
            //    SetCarInfo(mNormalCarInfo);

            //    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            //    {
            //        timerSmartroVCat.Enabled = false;
            //        timerSmartroVCat.Stop();

            //        SmatroDeveinCancle();
            //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | RegExtension", "[스마트로 VCat거래초기화 요청]");
            //        // 스마트로추가종료
            //    }
            //    else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            //    {
            //        Kis_TIT_DIpDeveinCancle();
            //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | RegExtension", "[KIS_TIT_DIP 거래초기화 요청]");

            //    }

            //    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            //    {
            //        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            //        timerSmartroVCat.Enabled = true;
            //        timerSmartroVCat.Start();
            //    }
            //    else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            //    {
            //        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            //    }
            //    return;
            //}
            //if (mNormalCarInfo.getCarTypeIndenty(mNormalCarInfo.OutCarNumber) == NormalCarInfo.CarNormalRegTypes.REG_OK) // 일반정기권
            //{
            //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | RegExtension", "[정기권선택됨]");
            //}
            //else
            //{
            //    if (mNormalCarInfo.ExpireYmd == string.Empty) // 만료기간이 없다면 
            //    {
            //        paymentControl.ErrorMessage = "정기권 연장이 불가한 차량입니다.";
            //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | RegExtension", "정기권 연장이 불가한 차량입니다.");

            //    }
            //    else
            //    {
            //        //만료차량 정기권요금제에서 일반요금제 변경기능
            //        if (!NPSYS.gUseRegPayToNormalPay) // 정기에서 일반요금으로 변경이 불가하다면
            //        {
            //            this.btnRegExtension.Visible = false;
            //        }
            //        //만료차량 정기권요금제에서 일반요금제 변경기능주석완료
            //        mNormalCarInfo.PreCarNormalRegTypes = mNormalCarInfo.CarNormalRegType;
            //        mNormalCarInfo.NormalTkNo = mNormalCarInfo.TkNO;
            //        mNormalCarInfo.PreParkMoney = mNormalCarInfo.ParkMoney;
            //        mNormalCarInfo.PreDiscountMoney = mNormalCarInfo.DiscountMoney;
            //        mNormalCarInfo.SetRegExpireExtensionCarPay();
            //        if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            //        {
            //            timerSmartroVCat.Enabled = false;
            //            timerSmartroVCat.Stop();

            //            SmatroDeveinCancle();
            //            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | RegExtension", "[스마트로 VCat거래초기화 요청]");
            //            // 스마트로추가종료
            //        }
            //        else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            //        {
            //            Kis_TIT_DIpDeveinCancle();
            //            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | RegExtension", "[KIS_TIT_DIP 거래초기화 요청]");

            //        }

            //        if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            //        {
            //            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            //            timerSmartroVCat.Enabled = true;
            //            timerSmartroVCat.Start();
            //        }
            //        else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            //        {
            //            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            //        }
            //        string tkno = LPRDbSelect.GetRegMasterCar_ExpireTkNo(NPSYS.m_MSSQL, mNormalCarInfo.OutCarNumber);
            //        mNormalCarInfo.TkNO = tkno;
            //        mNormalCarInfo.RegTkNo = tkno;
            //        lblIndate.Text = "정기권연장";
            //        lblElapsedTime.Text = string.Empty;
            //        paymentControl.ParkingFee = TextCore.ToCommaString(mNormalCarInfo.ParkMoney.ToString()) ;
            //        paymentControl.Payment = TextCore.ToCommaString(mNormalCarInfo.PaymentMoney) ;
            //        paymentControl.DiscountMoney = TextCore.ToCommaString(mNormalCarInfo.DiscountMoney) ;
            //        mNormalCarInfo.CurrentMoney = 0;
            //        TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu | SetCarInfo", "[정기권연장] 연장요금:" + mNormalCarInfo.ParkMoney.ToString() + " 결제요금:" + mNormalCarInfo.PaymentMoney.ToString() + " " + "주차시간:" + lblElapsedTime.Text + "-출차시간:" + NPSYS.datestringParser(mNormalCarInfo.OutYmd + mNormalCarInfo.OutHms)
            //                                  + " 차량번호:" + mNormalCarInfo.OutCarNumber);

            //        lbl_CarType.Visible = true;
            //        lbl_CarType.Text = "정기권연장";
            //        JungangPangDisplay();
            //    }
            //}
        }
        //정기권관련기능(만료요금부과/연장관련)주석완료
        private void btnTest1004Money_Click(object sender, EventArgs e)
        {
            CurrentNormalCarInfo.TotFee = 1004;
            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
        }
        //나이스개월연장기능


        /// <summary>
        /// 정기권연장버튼 사용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddMonth(object sender, EventArgs e)
        {
            try
            {

                //FadeFox.UI.ImageButton btnAddMonth = sender as FadeFox.UI.ImageButton;
                //TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | BtnAddMonth", "[연장 개월수 누름] " + btnAddMonth.Tag.ToString() + "개월 연장버튼 누름");
                //if (mCurrentNormalCarInfo.CurrMonth == Convert.ToInt32(btnAddMonth.Tag.ToString()))
                //{
                //    TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | BtnAddMonth", "[현재 기본 연장개월수랑 동일해서 개월변경안함]");
                //    return;
                //}
                ////정기권 연장개월수정리
                //btnOneMonthAdd.Visible = false;
                //btnTwoMonthAdd.Visible = false;
                //btnThreeMonthAdd.Visible = false;
                //btnFourMonthAdd.Visible = false;
                //btnFiveMonthAdd.Visible = false;
                //btnSixMonthAdd.Visible = false;
                ////정기권 연장개월수정리 주석완료

                //// mNormalCarInfo.SetRegCurrMonthSetting(Convert.ToInt32(btnAddMonth.Tag.ToString()));
                ////통합관련 처리어떻게 할지 결정해야함
                //paymentControl.ParkingFee = TextCore.ToCommaString(mCurrentNormalCarInfo.TotFee.ToString());
                //paymentControl.RecvMoney = TextCore.ToCommaString((mCurrentNormalCarInfo.RecvAmt - mCurrentNormalCarInfo.Change).ToString()); //시제설정누락처리
                //paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                //paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                //TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu | SetCarInfo", "[정기권연장] 연장요금:" + mCurrentNormalCarInfo.TotFee.ToString() + " 결제요금:" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " " + "주차시간:" + lblElapsedTime.Text + "-출차시간:" + NPSYS.datestringParser(mCurrentNormalCarInfo.OutYmd + mCurrentNormalCarInfo.OutHms)
                //                          + " 차량번호:" + mCurrentNormalCarInfo.OutCarNo1);

                //if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
                //{
                //    timerSmartroVCat.Enabled = false;
                //    timerSmartroVCat.Stop();
                //    SmatroDeveinCancle();
                //}


                //if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
                //{
                //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                //    timerSmartroVCat.Enabled = true;
                //    timerSmartroVCat.Start();

                //}
                //else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                //{
                //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                //}




            }
            finally
            {
                //if (mNormalCarInfo.PaymentMoney != 0)
                //{
                //    gbMonthAdd.Visible = true;
                //}
            }
        }

        //나이스개월연장기능 주석완료
        #endregion

        private void playVideo(string p_MovieName)
        {
            try
            {
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + p_MovieName;
                mCurrentMovieName = p_MovieName; // 2016-03-17 카드관련 동영상 떄문에 추가
                axWindowsMediaPlayer1.uiMode = "none";
                //if (mIsPlayerOkStatus)
                //{
                axWindowsMediaPlayer1.Ctlcontrols.play();
                //}
                MovieTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|playVideo", ex.ToString());
            }
        }

        public void SetRemoteDiscount(int pPrePayMoney, Payment pRemotePayment)
        {
            try
            {
                inputtime = NPSYS.SettingInputTimeValue;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SetRemoteDiscount", "원격할인 받은정보"
                                                                                                     + " 기존결제금액:" + pPrePayMoney.ToString()
                                                                                                     + " 현재남은금액:" + CurrentNormalCarInfo.PaymentMoney.ToString()
                                                                                                     + " 기존정보 주차시간:" + CurrentNormalCarInfo.ParkingMin.ToString()
                                                                                                     + " 기존정보 전체주차요금:" + CurrentNormalCarInfo.TotFee.ToString()
                                                                                                     + " 기존정보 전체할인요금:" + CurrentNormalCarInfo.TotDc.ToString()
                                                                                                     + " 기존정보 사정전산요금:" + CurrentNormalCarInfo.RecvAmt.ToString()
                                                                                                     + " 기존정보 사정전산거스름돈:" + CurrentNormalCarInfo.Change.ToString()
                                                                                                     + " 받은정보 주차시간:" + pRemotePayment.parkingMin.ToString()
                                                                                                     + " 받은정보 전체주차요금:" + pRemotePayment.totFee.ToString()
                                                                                                     + " 받은정보 전체할인요금:" + pRemotePayment.totDc.ToString()
                                                                                                     + " 받은정보 사정전산요금:" + pRemotePayment.recvAmt.ToString());

                CurrentNormalCarInfo.ParkingMin = pRemotePayment.parkingMin;
                CurrentNormalCarInfo.TotFee = Convert.ToInt32(pRemotePayment.totFee);
                CurrentNormalCarInfo.TotDc = Convert.ToInt32(pRemotePayment.totDc);
                CurrentNormalCarInfo.RecvAmt = Convert.ToInt32(pRemotePayment.recvAmt);
                CurrentNormalCarInfo.Change = Convert.ToInt32(pRemotePayment.change);
                CurrentNormalCarInfo.DcCnt = pRemotePayment.dcCnt;
                CurrentNormalCarInfo.RealFee = Convert.ToInt32(pRemotePayment.realFee);

                paymentControl.ParkingFee = TextCore.ToCommaString(CurrentNormalCarInfo.TotFee.ToString());
                paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                paymentControl.RecvMoney = TextCore.ToCommaString(CurrentNormalCarInfo.RecvAmt - CurrentNormalCarInfo.Change); //시제설정누락처리
                paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);

                if (pPrePayMoney == CurrentNormalCarInfo.PaymentMoney)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SetRemoteDiscount", "원격할인에 대한 추가 할인이없음");
                    return;
                }
                else
                {
                    timerAutoCardReading.Stop();
                    BeforeChangePayValueAsCardReader();
                }

                // 통합처리 할인이 되고난후 무언가 보내야하나
                if (CurrentNormalCarInfo.PaymentMoney == 0)
                {
                    //카드실패전송
                    CurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountRemote;
                    //카드실패전송완료
                    PaymentComplete();

                    // db에 저장
                    // 영수증 출력화면
                    return;
                }
                else
                {
                    timerAutoCardReading.Start();
                    ChangePayValueAsCardReader();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|SetRemoteDiscount", ex.ToString());
                paymentControl.ErrorMessage = "SetRemoteDiscount:" + ex.ToString();
            }
        }

        #region 테스트

        /// <summary>
        /// btnCurrentMoneyTest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            InsertMoney("100QTY");
            if (CurrentNormalCarInfo.PaymentMoney == 0)
            {
                //카드실패전송
                CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                //카드실패전송완료
                PaymentComplete();
            }
        }

        /// <summary>
        /// btnCredTest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            m_PayCardandCash.CreditCardPayResult(txtCardInfo.Text, CurrentNormalCarInfo);
            if (CurrentNormalCarInfo.PaymentMoney == 0)
            {
                //카드실패전송
                CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                //카드실패전송완료
                PaymentComplete();
            }
        }

        private void btnBarcodeTestDiscount_Click(object sender, EventArgs e)
        {
            BarcodeAction(txtTestBarcodeDiscount.Text);
        }

        private void btnTestBarcodeJehan_Click(object sender, EventArgs e)
        {
            DcDetail dcDetail = new DcDetail();
            dcDetail.currentDiscountTicketType = DcDetail.DIscountTicketType.BR;
            dcDetail.DcTkno = txtTestBarcodeDiscount.Text;
            dcDetail.UseYn = true;
            CurrentNormalCarInfo.ListDcDetail.Add(dcDetail);
        }

        /// <summary>
        /// 테스트 결제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void btnTestJson_Click(object sender, EventArgs e)
        {
            CurrentNormalCarInfo.VanCheck = 1;
            CurrentNormalCarInfo.VanCardNumber = "1234";
            CurrentNormalCarInfo.VanRegNo = "1234567890";
            CurrentNormalCarInfo.VanDate = DateTime.Now.ToString("yyyyMMdd");
            CurrentNormalCarInfo.VanRescode = "0000";
            CurrentNormalCarInfo.VanResMsg = "성공";
            CurrentNormalCarInfo.VanCardName = "하나SK카드";
            CurrentNormalCarInfo.VanBeforeCardPay = CurrentNormalCarInfo.PaymentMoney;
            string senddate = DateTime.Now.ToString("yyyy-MM-dd");
            string sendtime = DateTime.Now.ToString("HH:mm:ss");
            CurrentNormalCarInfo.VanCardApproveYmd = senddate;
            CurrentNormalCarInfo.VanCardApproveHms = sendtime;
            CurrentNormalCarInfo.VanCardApprovalYmd = senddate;
            CurrentNormalCarInfo.VanCardApprovalHms = sendtime;
            CurrentNormalCarInfo.VanIssueCode = "01";
            CurrentNormalCarInfo.VanIssueName = "하나은행";
            CurrentNormalCarInfo.VanCardAcquirerCode = "11";
            CurrentNormalCarInfo.VanCardAcquirerName = "성공";
            CurrentNormalCarInfo.VanAmt = CurrentNormalCarInfo.PaymentMoney;
            LPRDbSelect.Creditcard_Log_INsert(CurrentNormalCarInfo);
            PaymentComplete(); // 희주test
            return;
            //if (paymentData != null) 
            //{
            //    Payment();
            //}
            //else
            //{
            //    //재전송데이터 전송
            //}

        }

        private void btnTestDiscount_Click(object sender, EventArgs e)
        {
            //ParkingReceiveData.payment paymentAfterDisocunt = mHttpProcess.Discount(mCurrentNormalCarInfo, tbxTestDIscountValue.Text);
            //if (paymentAfterDisocunt.status.Success)
            //{
            //    mCurrentNormalCarInfo.ParkingMin = paymentAfterDisocunt.parkingMin;
            //    mCurrentNormalCarInfo.TotFee = Convert.ToInt32(paymentAfterDisocunt.totFee);
            //    mCurrentNormalCarInfo.TotDc = Convert.ToInt32(paymentAfterDisocunt.totDc);
            //    mCurrentNormalCarInfo.RecvAmt = Convert.ToInt32(paymentAfterDisocunt.recvAmt);
            //    mCurrentNormalCarInfo.RealFee = Convert.ToInt32(paymentAfterDisocunt.realFee);
            //    mCurrentNormalCarInfo.Change = Convert.ToInt32(paymentAfterDisocunt.change);
            //    mCurrentNormalCarInfo.DcCnt = paymentAfterDisocunt.dcCnt;
            //    //요금할인권처리 주석완료
            //    paymentControl.ParkingFee = TextCore.ToCommaString(mCurrentNormalCarInfo.TotFee);
            //    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
            //    paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);

            //    if (mCurrentNormalCarInfo.PaymentMoney == 0)
            //    {
            //        Payment();
            //    }
            //}



        }

        #endregion 테스트
 
    }
}


