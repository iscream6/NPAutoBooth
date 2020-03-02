using FadeFox;
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
        public event EventHandlerAddCtrl OnAddCtrl;

        public NormalCarInfo mCurrentNormalCarInfo = new NormalCarInfo();
        private PayCardandCash m_PayCardandCash = new PayCardandCash();
        private SmartroVCat mSmartroVCat = new SmartroVCat();  // 스마트로 추가

        private ManualResetEvent mCreditCardThreadLock = new ManualResetEvent(true);
        public bool mIsPlayerOkStatus = true;
        private string mCurrentMovieName = string.Empty; // 2016-03-17 카드관련 동영상 떄문에 추가
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Payment;
        // 2016.10.27  KIS_DIP 
        private KIS_TITDIPDevice mKIS_TITDIPDevice = new KIS_TITDIPDevice();
        private CardDeviceStatus mCardStatus = new CardDeviceStatus();
        // 2016.10.27  KIS_DIP 추가종료

        //스마트로 TIT_DIP EV-CAT 적용
        private Smatro_TITDIP_EVCAT mSmartro_TITDIP_EVCat = new Smatro_TITDIP_EVCAT();
        private System.Windows.Forms.Timer timerSmatro_TITDIP_Evcat = new System.Windows.Forms.Timer();
        //스마트로 TIT_DIP EV-CAT 적용완료

        //바코드할인 리스트로변경

        private List<string> mListBarcodeData = new List<string>();
        //바코드할인 리스트로변경 주석처리

        //바코드모터드리블 사용
        private List<BarcodeMoter.BarcodeMotorResult> mListBarcodeMotorData = new List<BarcodeMoter.BarcodeMotorResult>();
        //바코드모터드리블 사용완료
        private HttpProcess mHttpProcess = new HttpProcess();

        private PaymentUC paymentControl;

        #region 폼이동 이벤트

        /// <summary>
        /// 고객이 처음으로 등을 눌러 종료시
        /// </summary>
        public event ChangeView EventExitPayForm;

        /// <summary>
        /// 고객이 차량을 찾아 다음 요금으로 넘어가야할시 이벤트
        /// </summary>
        public event ChangeView<NormalCarInfo> EventExitPayForm_NextReceiptForm;
        public event ChangeView<InfoStatus> EventExitPayForm_NextInfo;

        #endregion

        #region 폼로딩시

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
                //1080해상도 적용 완료

                this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);
            }
        }

        private void FormPaymentMenu_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            Invoke(OnAddCtrl); //컨트롤 추가
            SetControl();
            SetLanguage(NPSYS.CurrentLanguageType);
            axWindowsMediaPlayer1.SendToBack();

            SettingEnableEvent();
        }

        private void FormPaymentMenu_Shown(object sender, EventArgs e)
        {
        }

        void AddCtrl()
        {
            this.Controls.Add((Control)paymentControl);
            paymentControl.Dock = DockStyle.Fill;
            paymentControl.BringToFront();
        }

        private void SetControl()
        {
            paymentControl.ConfigCall += Close_Callback;
            paymentControl.PreForm_Click += btn_PrePage_Click;
            paymentControl.Home_Click += btn_home_Click;
            paymentControl.SeasonCarAddMonth += SetNextRegExipire;
            paymentControl.CashCancel_Click += btn_CashCancle_Click;
            paymentControl.SamsungPay_Click += btnSamSungPay_Click;

            paymentControl.Initialize();

            if (!NPSYS.isBoothRealMode)
            {
                groupTest.Visible = true;
            }

            if (NPSYS.gUseMultiLanguage)
            {
                paymentControl.ForeignLanguageVisible(true);
            }
            else
            {
                paymentControl.ForeignLanguageVisible(false);
            }

            //picWait 화면을 부모폼 중앙에 위치시켜야 겠다.
            //picWait의 Location 위치를 잡자. 잡는 방법은
            //부모폼의 Location은 0,0 이므로 부모폼의 Width 에서 picWait의 Width를 뺀 값의 반이 X좌표
            //Y좌표 또한 X좌표 구하는 방법과 동일하다.
            pic_Wait_MSG_WAIT.Location = new Point
            {
                X = (this.Width - pic_Wait_MSG_WAIT.Width) / 2,
                Y = (this.Height - pic_Wait_MSG_WAIT.Height) / 2,
            };

            if (NPSYS.CurrentBoothType != ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                //없는 컨트롤
                //paymentControl.GetControl<Label>
                //    (PaymentUC.ENUM_Control.lblPremoveTextMsg)).Visible = false;
                //paymentControl.GetControl<Label>
                //    (PaymentUC.ENUM_Control.lblHomeTextMsg)).Visible = false;
            }
        }

        private void Close_Callback()
        {
            NPSYS.buttonSoundDingDong();
            EventExitPayForm(mCurrentFormType);
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");
        }

        /// <summary>
        /// 변수초기화 및 장비초기화
        /// </summary>
        private void ClearView()
        {
            paymentControl.Clear();
            mListBarcodeData = new List<string>();
            mCurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
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
                    if (mCurrentNormalCarInfo.OutImage1 != null && mCurrentNormalCarInfo.OutImage1.Trim().Length > 0)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지경로" + mCurrentNormalCarInfo.OutImage1);
                        paymentControl.CarImage = NPSYS.WebImageView(mCurrentNormalCarInfo.OutImage1);
                    }
                    else if (mCurrentNormalCarInfo.InImage1 != null && mCurrentNormalCarInfo.InImage1.Trim().Length > 0)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지경로" + mCurrentNormalCarInfo.InImage1);
                        paymentControl.CarImage = NPSYS.WebImageView(mCurrentNormalCarInfo.InImage1);
                    }
                }
                else
                {
                    paymentControl.CarImage = NPSYS.WebImageView(mCurrentNormalCarInfo.InImage1);
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|GetImage", "이미지 불러오기 작업종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | GetImage", ex.ToString());
            }
        }


        /// <summary>
        /// 카드만 사용해야 하는 상황인지 확인
        /// </summary>
        /// <returns></returns>
        public bool isOnlyCard()
        {
            switch (mCurrentNormalCarInfo.CurrentCarPayStatus)
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
            mCurrentNormalCarInfo = pPrevNormalCarInfo;
            paymentControl.SetCarInfo(mCurrentNormalCarInfo);
        }

        #endregion

        #region 동영상 관련 이벤트처리

        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_MediaError", "플레이어오류");
            }
            catch (Exception ex)
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_MediaError", ex.ToString());
            }
        }

        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {
                mIsPlayerOkStatus = false;
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|player_ErrorEvent", "에러내용" + errDesc);
            }
            catch (Exception ex)
            {
                mIsPlayerOkStatus = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|player_ErrorEvent", ex.ToString());
            }
        }

        void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|Player_PlayStateChange", ex.ToString());
            }
        }
        #endregion

        #region 폼활성화 / 종료시 이벤트 
        /// <summary>
        /// 결제시작시 이벤트 받아온다
        /// </summary>
        private void SettingEnableEvent()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingDisableEvent", "[장비이벤트 활성화]");
            try
            {
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

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;

            }
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
        #endregion

        #region 폼활성화 / 종료시 장비동작처리 
        // 결제 시작시 장비 동작처리
        private void SettingEnableDevice()
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SettingEnableDevice", "[장비동작시킴]");

            //스마트로 TIT_DIP EV-CAT 적용
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                //   NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;
                timerKisCardPay.Enabled = true;
                if (mCurrentNormalCarInfo.PaymentMoney > 0)
                {
                    timerSmatro_TITDIP_Evcat.Interval = 500;
                    timerSmatro_TITDIP_Evcat.Tick += timerSmatro_TITDIP_Evcat_Tick;
                    paymentControl.ErrorMessage = string.Empty;
                    UnsetSmatro_DIPTIT_Evcat();

                    TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditPaymentMenu | SettingEnableDevice", "[Smatro_TITDIP_Evcat 신용카드요금결제시작]true");
                    this.timerSmatro_TITDIP_Evcat.Enabled = true;
                    this.timerSmatro_TITDIP_Evcat.Start();

                }

            }

            //스마트로 TIT_DIP EV-CAT 적용완료
            StartTIcketCardRead();

            if (NPSYS.Device.isUseDeviceBillReaderDevice || NPSYS.Device.isUseDeviceCoinReaderDevice)
            {
                StartMoneyInsert();
            }
            else
            {
                StopMoneyInsert();
            }
            
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
            if (NPSYS.Device.UsingSettingSinbunReader)
            {
                if (NPSYS.Device.gIsUseSinbunReader)
                {
                    NPSYS.Device.SinbunReader.ThreadStart();
                }
            }

            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;


            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {

                this.timerSmartroVCat.Enabled = true;
                this.timerSmartroVCat.Start();
                timerKisCardPay.Enabled = true;
            }

            switch (NPSYS.Device.GetCurrentUseDeviceCard())
            {
                case ConfigID.CardReaderType.FIRSTDATA_DIP:
                case ConfigID.CardReaderType.KOCES_PAYMGATE:
                case ConfigID.CardReaderType.KICC_DIP_IFM:

                    timerAutoCardReading.Enabled = true;
                    timerAutoCardReading.Start();

                    break;
                case ConfigID.CardReaderType.KOCES_TCM:

                    bool isSuccessInsertedReady = KocesTcmMotor.CardAccept();
                    if (isSuccessInsertedReady)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }
                    else
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    }
                    timerAutoCardReading.Enabled = true;
                    timerAutoCardReading.Start();
                    break;

            }

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                timerCardVisible.Enabled = true;
                timerCardVisible.Start();
            }

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {

                timerKisCardPay.Enabled = true;
                if (mCurrentNormalCarInfo.PaymentMoney > 0)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                    SetKisDipIFM();
                }
            }
        }

        /// <summary>
        /// TMoney 스마트로 Receive Data 처리 이벤트 핸들러
        /// </summary>
        /// <param name="pDTO"></param>
        private void TmoneySmartro3500_EventTMoneyData(SmartroDTO pDTO)
        {
            // =========[수신 받은 JOB 코드 목록]=========
            // a : 장치체크 응답전문
            // b : 거래승인 응답전문
            // c : 거래취소 응답전문
            // d : 카드조회 응답전문
            // e : 결제대기 응답전문
            // f : 카드 UID 읽기 응답전문
            // @ : 이벤트 응답전문
            // g : 부가정보 추가 거래승인 응답전문
            // i : 설정 정보 셋팅 응답전문
            // j : 설정 정보 응답전문
            // K : 설정 정보 메모리 WRITING 응답전문
            // I : 마지막 승인 응답전문
            // v : 버전 체크 응답전문
            // s : 화면&음성 설정 응답전문
            // =========[이벤트 코드 목록]=========
            // M : MS카드 인식
            // R : RF카드 인식
            // I : IC카드 인식
            // O : IC카드 제거
            // F : IC카드 FallBack
            // =========[결제/결제취소 Process]=========
            //결제 : 결제대기 요청 -> 결제대기 응답 -> 카드 삽입 이벤트 응답 -> 거래 승인 요청 -> 거래 승인 응답 -> [카드를 제거해주세요] -> 이벤트 응답
            //결제취소 : 결제대기 요청 -> 결제대기 응답 -> 카드 삽입 이벤트 응답 -> 거래 취소 요청 -> 거래 취소 응답 -> [카드를 제거해주세요] -> 이벤트 응답

            var Test = "1234";

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



            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
            {

                this.timerSmartroVCat.Enabled = false;
                this.timerSmartroVCat.Stop();
                timerKisCardPay.Enabled = false;
            }
            switch (NPSYS.Device.GetCurrentUseDeviceCard())
            {
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

            }


            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
            {
                timerCardVisible.Enabled = false;
                timerCardVisible.Stop();
                //btnCardApproval.Visible = true; //뉴타입주석
            }

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {

                timerKisCardPay.Enabled = false;
                timerKisCardPay.Stop();
                UnSetKisDipIFM();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | Kis_TIT_DIpDeveinCancle", "[KIS_TIT_DIP거래초기화 요청시작 종료]");
            }
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {

                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                timerKisCardPay.Enabled = false;
                timerKisCardPay.Stop();
                timerSmatro_TITDIP_Evcat.Tick -= timerSmatro_TITDIP_Evcat_Tick;
                this.timerSmatro_TITDIP_Evcat.Enabled = false;
                this.timerSmatro_TITDIP_Evcat.Stop();
                paymentControl.ErrorMessage = string.Empty;
                UnsetSmatro_DIPTIT_Evcat();

            }

        }
        #endregion

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

                    //inputTimer.Start();
                    //MovieTimer.Start();

                    GetImage();

                    //SettingEnableEvent();
                    SettingEnableDevice(); //결제장비 동작 시작
                    paymentControl.ButtonEnable(ButtonEnableType.PayFormStart);
                    SetLanuageDynamic(NPSYS.CurrentLanguageType);

                    this.TopMost = true;
                    this.Show();
                    this.Activate();

                    //inputTimer.Enabled = true;
                    MovieTimer.Enabled = true;

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
                if (mCurrentNormalCarInfo.PaymentMoney != 0) // 주차요금이 있는상태에서 폼이종료된다면
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

        #endregion //==========

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
            paymentControl.SetLanguage(pLanguageType, mCurrentNormalCarInfo);

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

        #endregion

        #region 각종 장비들및 결재 방식에 따른 동영상및 문구

        private string m_DiscountAndPayAndCreditAndTMoneyMovie = "할인권_현금_신용카드_교통카드.avi";
        private string m_PayAndCreditAndTMoneyMovie = "현금_신용카드_교통카드.avi";
        private string m_DiscountAndCreditAndTMoneyMovie = "할인권_신용카드_교통카드.avi";
        private string m_CreditAndTmoneyMovie = "신용카드_교통카드.avi";
        private string m_DiscountAndPayAndCreditMovie = "할인권_현금_신용카드.avi";
        private string m_PayAndCreditMovie = "현금_신용카드.avi";
        private string m_DiscountAndPayMovie = "할인권_현금.avi";
        private string m_PayMovie = "현금.avi";
        private string m_CreditMovie = "신용카드.avi";
        private string m_DiscountAndCreditMovie = "할인권_신용카드.avi";
        private string m_DiscountAndTmoneyMovie = "할인권_교통카드.avi";
        private string m_TmoneyMovie = "교통카드.avi";
        private string m_DiscountMovie = "할인권.avi";
        private string m_JuminDIscountMovie = "감면혜택.wav";
        private string m_Junggi = "정기권연장.wav";
        private string m_CancleCard = "카드취소.wav";
        private string m_DiscountBarcodeCreditMovie = "할인권바코드_신용카드.wav";
        //영수증할인문구 적용
        private string m_BarAndDiscountAndCreditAndPayAndTMoneyMovie = "바코드_할인권_신용카드_현금_교통카드.wav";
        private string m_BarAndCreditAndPayAndTMoneyMovie = "바코드_신용카드_현금_교통카드.wav";
        private string m_BarAndDiscountAndPayAndTMoneyMovie = "바코드_할인권_현금_교통카드.wav";
        private string m_BarAndPayAndTMoneyMovie = "바코드_현금_교통카드.wav";
        private string m_BarAndDiscountAndCreditAndPayMovie = "바코드_할인권_신용카드_현금.wav";
        private string m_BarAndCreditAndPayMovie = "바코드_신용카드_현금.wav";
        private string m_BarAndDiscountAndPayMovie = "바코드_할인권_현금.wav";
        private string m_BarAndPayMovie = "바코드_현금.wav";
        private string m_BarAndDiscountAndCreditAndTMoneyMovie = "바코드_할인권_신용카드_교통카드.wav";
        private string m_BarAndCreditAndTMoneyMovie = "바코드_신용카드_교통카드.wav";
        private string m_BarAndDiscountAndTMoneyMovie = "바코드_할인권_교통카드.wav";
        private string m_BarAndDiscountAndCreditMovie = "바코드_할인권_신용카드.wav";
        private string m_BarAndCreditMovie = "바코드_신용카드.wav";
        private string m_BarAndTMoneyMovie = "바코드_교통카드.wav";
        //영수증할인문구 적용완료



        //영수증바코드문구 적용
        public void Action_DeviceEnableMovie()
        {

            if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                playVideo(m_Junggi);
                return;
            }
            else if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                   || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                playVideo(m_CancleCard);
                return;

            }

            if (NPSYS.IsUsedCashPay() == true && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 현금,카드리더기,교통카드 장비 정상
            {
                //635, 419
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)  // 할인권, 현금, 신용카드 , 교통카드
                {

                    playVideo(m_DiscountAndPayAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권, 현금 , 교통카드(동영상없음)
                {

                    playVideo(m_DiscountAndPayMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) //현금, 신용카드 , 교통카드
                {
                    playVideo(m_PayAndCreditAndTMoneyMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금 , 교통카드(동영상 없음)
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 할인권, 신용카드
                {
                    //바코드, 할인권, 신용카드, 현금, 교통카드
                    playVideo(m_BarAndDiscountAndCreditAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 신용카드
                {
                    // 바코드, 신용카드, 현금, 교통카드
                    playVideo(m_BarAndCreditAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드, 할인권
                {
                    // 바코드, 할인권, 현금, 교통카드
                    playVideo(m_BarAndDiscountAndPayAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금, 교통카드], 바코드
                {
                    // 바코드, 현금, 교통카드
                    playVideo(m_BarAndPayAndTMoneyMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == true && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false) // 현금,카드리더기 정상
            {
                //635, 419
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)// 할인권 , 현금, 신용카드 
                {

                    playVideo(m_DiscountAndPayAndCreditMovie);

                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권 ,현금
                {

                    playVideo(m_DiscountAndPayMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금 , 신용카드
                {
                    playVideo(m_PayAndCreditMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 현금
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드, 현금
                    playVideo(m_BarAndDiscountAndCreditAndPayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 신용카드
                {
                    // 바코드, 신용카드, 현금
                    playVideo(m_BarAndCreditAndPayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [현금] 바코드, 할인권
                {
                    // 바코드, 할인권, 현금
                    playVideo(m_BarAndDiscountAndPayMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == false) // 현금 장비 정상
            {
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)
                {
                    playVideo(m_PayMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None)
                {
                    // 바코드, 현금
                    playVideo(m_BarAndPayMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 카드리더기 , 교통카드리더기 정상
            {
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) //  할인권, 신용카드, 교통카드 
                {

                    playVideo(m_DiscountAndCreditAndTMoneyMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 신용카드, 교통카드  
                {
                    playVideo(m_CreditAndTmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권 , 교통카드
                {

                    playVideo(m_DiscountAndTmoneyMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 교통카드
                {
                    playVideo(m_TmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드, 교통카드
                    playVideo(m_BarAndDiscountAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 신용카드
                {
                    // 바코드, 신용카드, 교통카드
                    playVideo(m_BarAndCreditAndTMoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard) // [교통카드] 바코드, 할인권
                {
                    // 바코드, 할인권, 교통카드
                    playVideo(m_BarAndDiscountAndTMoneyMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false) // 카드리더기 정상
            {
                //635, 456
                if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권, 신용카드
                {

                    playVideo(m_DiscountAndCreditMovie);

                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 신용카드
                {
                    playVideo(m_CreditMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 할인권
                {
                    playVideo(m_DiscountMovie);
                }
                else if (!NPSYS.Device.UsingSettingDiscountCard && !NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None) // 암것도 안됨
                {
                    playVideo(m_CreditMovie);
                }
                //할인권 타입설정 추가
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard && NPSYS.Device.UsingSettingBarcodeDCTicket) // 바코드할인권, 신용카드
                {
                    // 바코드할인권, 신용카드
                    playVideo(m_DiscountBarcodeCreditMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // 바코드, 할인권, 신용카드
                {
                    // 바코드, 할인권, 신용카드
                    playVideo(m_BarAndDiscountAndCreditMovie);
                }
                //할인권 타입설정 추가완료
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && !NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingCreditCard) // 바코드, 신용카드
                {
                    // 바코드, 신용카드
                    playVideo(m_BarAndCreditMovie);
                }
            }
            if (NPSYS.IsUsedCashPay() == false && (NPSYS.Device.gIsUseCreditCardDevice || NPSYS.Device.gIsUseMagneticReaderDevice) == false && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true)
            {
                //635, 456
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)
                {
                    playVideo(m_TmoneyMovie);
                }
                else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None) // 바코드, 교통카드
                {
                    // 바코드, 교통카드
                    playVideo(m_BarAndTMoneyMovie);
                }
            }
        }
        //영수증바코드문구 적용완료
        //바코드모터드리블 사용완료
        #endregion

        #region VAN장비

        /// <summary>
        /// 결제요금이 변경되기전 동작멈춤 중간에 결제가 되지않게 현재 카드리더기 기기에 요금  취소등의 동작
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
                mSmartro_TITDIP_EVCat.ChangePayMoney(NPSYS.Device.Smartro_TITDIP_Evcat, mCurrentNormalCarInfo.PaymentMoney.ToString());
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;

                Thread.Sleep(1000);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | ChangePayValueAsCardReader", "[스마트로 TIT_DIP EV-CAT 결제요금변경 요청]");
            }
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            }
        }

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
                if (mCurrentNormalCarInfo.PaymentMoney != 0)
                {
                    timerAutoCardReading.Start();
                }
            }
        }

        #region KICC DIP적용
        private void CardActionKiccDip()
        {

            try
            {
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }

                bool resultCardState = NPSYS.Device.KICC_TIT.GetCardInsert();
                if (resultCardState)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                    KICC_TIT.KICC_TIT_RECV_SUCCESS RECV_SUCCESS = NPSYS.Device.KICC_TIT.GetRecvData();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "KICC_DIP장비상태" + RECV_SUCCESS.MSG);
                }

                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    inputtime = paymentInputTimer;
                    NPSYS.CurrentBusyType = NPSYS.BusyType.Paying;
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    NPSYS.CurrentBusyType = NPSYS.BusyType.None;

                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else // 잘못된 티켓
                    {
                        if (mCurrentNormalCarInfo.VanRescode != KICC_TIT.KICC_USER_CANCLECODE)
                        {
                            //카드실패전송
                            if (NPSYS.gUseCardFailSend)
                            {
                                DateTime paydate = DateTime.Now;
                                //카드실패전송
                                mCurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                                //카드실패전송 완료
                                Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                            }
                            //카드실패전송 완료
                        }
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", ex.ToString());
            }

        }

        #endregion

        #region FistData DIP적용
        //FIRSTDATA처리 
        private void CardActionFirstDataDip()
        {

            try
            {

                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                //if (mNormalCarInfo.PaymentMoney > 0 && mNormalCarInfo.CurrentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.None)
                //{
                //    bool isSuceessAccept = KocesTcmMotor.CardAccept();
                //    if (isSuceessAccept)
                //    {
                //        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                //    }

                //}
                FirstDataDip.readerStatus currentStatus = FirstDataDip.ReadState();
                if (currentStatus == FirstDataDip.readerStatus.ReaderIcIn)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "FISTDATA_DIP장비상태" + CardDeviceStatus.CardReaderStatus.CARDINSERTED);
                }

                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else // 잘못된 티켓
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", ex.ToString());
            }

        }


        //FIRSTDATA처리 주석완료
        #endregion
        // 2016.10.27 KIS_DIP 추가
        #region KIS_TIT_DIP_IFM

        //KIS 할인처리시 처리문제


        private void btnCardAction_Click(object sender, EventArgs e)
        {
            if (mCurrentNormalCarInfo.Current_Money > 0 || mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }
            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnCardAction_Click", "카드결제버튼 누름");

        }



        public bool SetKisDipIFM()
        {

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                timerKisCardPay.Enabled = true;
                if (mCurrentNormalCarInfo.PaymentMoney > 0)
                {
                    NPSYS.Device.KisPosAgent.OnApprovalEnd += new EventHandler(KisPosAgent_OnApprovalEnd);
                    //btnCardAction.Visible = true;
                    paymentControl.ErrorMessage = string.Empty;
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    mKIS_TITDIPDevice.VanIP = NPSYS.gVanIp;
                    mKIS_TITDIPDevice.VanPort = Convert.ToInt16(NPSYS.gVanPort);
                    mKIS_TITDIPDevice.InWCC = "C";
                    mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);

                    TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | SetKisDipIFM", "[KIS_TIT_DIP 신용카드요금결제시작]true");
                }
            }
            return true;

        }

        public bool UnSetKisDipIFM()
        {
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mKIS_TITDIPDevice.CardLockEject(NPSYS.Device.KisPosAgent);
                NPSYS.Device.KisPosAgent.OnApprovalEnd -= new EventHandler(KisPosAgent_OnApprovalEnd);
            }
            return true;
        }


        ////KIS 할인처리시 처리문제주석완료
        private void KisPosAgent_OnApprovalEnd(object sender, EventArgs e)
        {
              if (NPSYS.Device.KisPosAgent.outRtn != 0)
            {
                return;
            }
            else
            {
                DoWork(NPSYS.Device.KisPosAgent);
            }
        }

        private void DoWork(AxKisPosAgentLib.AxKisPosAgent pKisPosAgent)
        {
            System.Threading.Thread.Sleep(200);
            switch (mCardStatus.currentCardReaderStatus)
            {
                case CardDeviceStatus.CardReaderStatus.None:
                    mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[최초 카드상태 체크]");
                    break;
                case CardDeviceStatus.CardReaderStatus.CardReady:
                    if (string.IsNullOrEmpty(pKisPosAgent.outAgentData))
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[outAgentData 데이터 없음]");
                        break;
                    }
                    if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "1")
                    {
                        paymentControl.ErrorMessage = "카드가 삽입되었습니다.";
                        mKIS_TITDIPDevice.PowerCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPowerCheck;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드 삽입됨]");
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "0")
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "C")
                    {
                        paymentControl.ErrorMessage = "카드[MST] 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.InWCC = "S";
                        mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[삼성페이 읽기 진행]");
                    }
                    //else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC != "F")
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    //}
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "F")
                    {
                        paymentControl.ErrorMessage = "카드[MS] 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[MS데이터 읽기 진행]");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "카드 삽입해 주시기 바랍니다...";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    break;
                case CardDeviceStatus.CardReaderStatus.CardPowerCheck:
                    //if (pKisPosAgent.outAgentData.Trim() == "1")
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    //}
                    //else
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 확인 실패 되었습니다.";

                    //    //mKIS_TITDIPDevice.InWCC = "F";
                    //    mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    //}
                    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    break;
                case CardDeviceStatus.CardReaderStatus.CardLockEject:
                    if (pKisPosAgent.outAgentData.Trim() == "1")
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    }
                    break;
                case CardDeviceStatus.CardReaderStatus.CardReadyEnd:
                    if (pKisPosAgent.outAgentData.Trim() == "CF")
                    {
                        //paymentControl.ErrorMessage = "카드[MS] 데이터를 읽는 중 입니다.";
                        mKIS_TITDIPDevice.InWCC = "F";
                        //mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                        //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[MS데이터 읽기 진행]");
                        paymentControl.ErrorMessage = "카드[IC] 리딩 실패";
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC 카드 리딩 실패]");
                    }
                    else if (pKisPosAgent.outAgentData.Trim().Length == 2)
                    {
                        paymentControl.ErrorMessage = "카드결제 실패";
                        //mKIS_TITDIPDevice.CardLockEjectFinish(pKisPosAgent);
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드리딩 실패]");
                    }
                    else
                    {
                        if (pKisPosAgent.outAgentData.Substring(7, 1).Equals("M"))
                        {
                            mKIS_TITDIPDevice.InWCC = "S";
                        }
                        paymentControl.ErrorMessage = "승인 진행 중입니다.";
                        //payData.outReaderData = axKisPosAgent1.outAgentData.Substring(0, 6);
                        if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                           || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString() ,mCurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), mCurrentNormalCarInfo.VanRegNo_Cancle);
                        }
                        else
                        {
                            mKIS_TITDIPDevice.CardApproval(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                        }
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드 결제요청 진행]");
                    }
                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(false);
                    }
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);

                    break;
                case CardDeviceStatus.CardReaderStatus.CardFullBack:
                    paymentControl.ErrorMessage = "카드[MS] 결제 요청중 입니다.";
                    if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString(), mCurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), mCurrentNormalCarInfo.VanRegNo_Cancle);
                    }
                    else
                    {
                        mKIS_TITDIPDevice.CardApproval(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                    }

                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                    break;
                case CardDeviceStatus.CardReaderStatus.CardApproval:
                    KisPosAgent_OnAgtComplete(pKisPosAgent);
                    break;
                case CardDeviceStatus.CardReaderStatus.CardLockEjectFinish:
                    paymentControl.ErrorMessage = "카드를 뽑아주세요.";
                    mKIS_TITDIPDevice.StatusCheckFinish(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish;
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);
                    break;
                case CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish:
                    paymentControl.ErrorMessage = "카드를 뽑아 주세요";
                    if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "0")
                    {
                        mKIS_TITDIPDevice.StatusCheckFinish(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish;
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "1")
                    {
                        mKIS_TITDIPDevice.CardLockEjectFinish(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEjectFinish;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "결제가 완료되었습니다";
                    }
                    PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardEject); //일부러주석처리
                    break;
            }

            Thread.Sleep(200);
        }



        private void KisPosAgent_OnAgtComplete(AxKisPosAgentLib.AxKisPosAgent pKisPosAgent)
        {
            try
            {
                mKIS_TITDIPDevice.KisSpec.GetResSpec(pKisPosAgent.outAgentData);

                // 리턴값:0 성공유무:True 응답코드:0000 단밀기번호:90100546 카드번호:541707 할부개월:   결제금액:40000    부가세액:         승인번호:90057260      거래일자:20161027 매입사코드:15 매입사명:하나카드             발급사코드:15 발급사명:하나카드             가맹점번호:8100000637           거래일련번호:       메시지1:승    인                                 메시지1:
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | KisPosAgent_OnAgtComplete", "[KISAGENT 응답결과]"
                                        + " 리턴값:" + pKisPosAgent.outRtn.ToString()
                                        + " 성공유무:" + (mKIS_TITDIPDevice.KisSpec.outReplyCode == "0000" ? true : false).ToString()
                                        + " 응답코드:" + mKIS_TITDIPDevice.KisSpec.outReplyCode
                                        + " 단밀기번호:" + mKIS_TITDIPDevice.KisSpec.CatID
                                        + " 카드번호:" + pKisPosAgent.outCardNo
                                        + " 할부개월:" + pKisPosAgent.outInstallment
                                        + " 결제금액:" + mKIS_TITDIPDevice.KisSpec.TotAmt
                                        + " 부가세액:" + mKIS_TITDIPDevice.KisSpec.VatAmt
                                        + " 승인번호:" + mKIS_TITDIPDevice.KisSpec.outAuthNo
                                        + " 거래일자:" + mKIS_TITDIPDevice.KisSpec.outReplyDate
                                        + " 매입사코드:" + mKIS_TITDIPDevice.KisSpec.outAccepterCode
                                        + " 매입사명:" + mKIS_TITDIPDevice.KisSpec.outAccepterName
                                        + " 발급사코드:" + mKIS_TITDIPDevice.KisSpec.outIssuerCode
                                        + " 발급사명:" + mKIS_TITDIPDevice.KisSpec.outIssuerName
                                        + " 가맹점번호:" + mKIS_TITDIPDevice.KisSpec.outMerchantRegNo
                                        + " 거래일련번호:" + mKIS_TITDIPDevice.KisSpec.outTranNo
                                        + " 메시지1:" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1);
                //+ " 메시지1:" + mKisSpec.outReplyMsg2);


                if (pKisPosAgent.outRtn == 0 && mKIS_TITDIPDevice.KisSpec.outReplyCode == "0000") // 카드결제성공
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | KisPosAgent_OnAgtComplete", "[카드결제 성공]");
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //string[] lCardNumData = mKisSpec.outCardNo.Split('=');
                    //if (lCardNumData[0].Length > 13)
                    //{
                    //    mCurrentNormalCarInfo.CardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    //}
                    //else
                    //{
                    //    mCurrentNormalCarInfo.CardNumber = lCardNumData[0];
                    //}
                    mCurrentNormalCarInfo.VanCardNumber = pKisPosAgent.outCardNo.Trim();
                    mCurrentNormalCarInfo.VanRegNo = mKIS_TITDIPDevice.KisSpec.outAuthNo.Trim();
                    mCurrentNormalCarInfo.VanDate = mKIS_TITDIPDevice.KisSpec.outReplyDate;
                    mCurrentNormalCarInfo.VanRescode = mKIS_TITDIPDevice.KisSpec.outReplyCode;
                    mCurrentNormalCarInfo.VanResMsg = mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    if (mKIS_TITDIPDevice.KisSpec.VatAmt.Trim() == string.Empty)
                    {
                        mKIS_TITDIPDevice.KisSpec.VatAmt = "0";
                    }
                    //mNormalCarInfo.SupplyPay = (Convert.ToInt32(mKisSpec.outTranAmt) - Convert.ToInt32(mKisSpec.outVatAmt));
                    mCurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(mKIS_TITDIPDevice.KisSpec.VatAmt);
                    mCurrentNormalCarInfo.VanSupplyPay = 0;
                    mCurrentNormalCarInfo.VanCardName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    mCurrentNormalCarInfo.VanBeforeCardPay = mCurrentNormalCarInfo.PaymentMoney;
                    mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    mCurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                    mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    mCurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                    mCurrentNormalCarInfo.VanIssueCode = mKIS_TITDIPDevice.KisSpec.outIssuerCode;
                    mCurrentNormalCarInfo.VanIssueName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    mCurrentNormalCarInfo.VanCardAcquirerCode = mKIS_TITDIPDevice.KisSpec.outAccepterCode;
                    mCurrentNormalCarInfo.VanCardAcquirerName = mKIS_TITDIPDevice.KisSpec.outAccepterName;



                    mCurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
                    mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney) + "원";
                    paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc) + "원";
                    mKIS_TITDIPDevice.CardLockEjectFinish(NPSYS.Device.KisPosAgent);


                }
                else if (mKIS_TITDIPDevice.KisSpec.outReplyCode == "8100")// 사용자취소
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    //KIS 할인처리시 처리문제
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess;
                    //응답코드:8000 타입아웃
                    //응답코드:8326 한도초과
                    //KIS 할인처리시 처리문제주석완료

                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(true);
                    }
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    mKIS_TITDIPDevice.InWCC = mKIS_TITDIPDevice.InWCC != "C" ? "C" : mKIS_TITDIPDevice.InWCC;
                    DoWork(pKisPosAgent);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행완료" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    return;
                }
                else // 카드결제실패
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(true);
                    }
                    //timerKis_TIT_DIP_IFM.Enabled = true;
                    //timerKis_TIT_DIP_IFM.Start();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1);
                    paymentControl.ErrorMessage = "카드결제실패:" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    Application.DoEvents();
                    PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);

                    Thread.Sleep(2000);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    mKIS_TITDIPDevice.InWCC = mKIS_TITDIPDevice.InWCC != "C" ? "C" : mKIS_TITDIPDevice.InWCC;
                    DoWork(pKisPosAgent);



                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | KisPosAgent_OnAgtComplete", ex.ToString());
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                DoWork(pKisPosAgent);
            }

        }

        #endregion
        // 2016.10.27  KIS_DIP 추가종료

        #region 스마트로 추가

        private bool SmatroDeveinCancle()
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[취소쓰레드웨이트]");
            mCreditCardThreadLock.WaitOne(3000);
            mCreditCardThreadLock.Reset();
            if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
            }
            mSmartroVCat.StartSoundTick = 0;
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청시작]");
            SmartroVCat.SmatroData smatrodata = mSmartroVCat.DeviceReInitialLizeSync(NPSYS.Device.SmtSndRcv);
            for (int i = 0; i < 16; i++)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청시작 종료]");
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청결과]" + smatrodata.Success.ToString() + " 응답코드:" + smatrodata.ReceiveReturnCode);
            mCreditCardThreadLock.Set();
            return smatrodata.Success;
        }

        private void SmartroCardApprovalAction()
        {

            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay
                || mCurrentNormalCarInfo.VanAmt != 0 || mCurrentNormalCarInfo.Current_Money > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (mCurrentNormalCarInfo.Current_Money == 0)
            {

                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat && NPSYS.Device.gIsUseCreditCardDevice
                    || NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.SmartroVCat && NPSYS.Device.gIsUseMagneticReaderDevice)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[승인쓰레드웨이트]");
                    mCreditCardThreadLock.WaitOne(2000);
                    mCreditCardThreadLock.Reset();
                    System.Threading.Thread.Sleep(100);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmartroCardApprovalAction", "[신용카드요금결제요청 시작]" + mCurrentNormalCarInfo.ReceiveMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    bool isSend = mSmartroVCat.CardApproval(NPSYS.Device.SmtSndRcv, Convert.ToInt32(mCurrentNormalCarInfo.ReceiveMoney), 600, ref errorMessage);
                    for (int i = 0; i < 20; i++)
                    {
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();

                    }
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmartroCardApprovalAction", "[신용카드요금결제요청 종료]" + mCurrentNormalCarInfo.ReceiveMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    mCreditCardThreadLock.Set();

                }

            }


        }


        /// <summary>
        /// 스마트로 추가 상태변화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnRcvState(object sender, AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEvent e)
        {


            inputtime = paymentInputTimer;
            string cardState = e.szType.ToString();
            string statusMessage = mSmartroVCat.OnReceiveStatusMessage(cardState);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드동작 변화]"
                                     + " 상태값:" + cardState.ToString()
                                     + " 상태명령:" + statusMessage
                                     + " 카드동작상태:" + mCardStatus.currentCardReaderStatus.ToString()
                                     );
            paymentControl.ErrorMessage = statusMessage;
            if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            {
                if (cardState == "1I")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드결제명령 내린상태로 다시 결제요청하지않음]");
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                }
                if (cardState == "AU")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드결제명령 내린상태로 다시 결제요청하지않음]");
                    SmatroDeveinCancle();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                }
                if (cardState == "CK")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드가 감지됨]");
                    mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardInsert;
                    PlaySoundCard();
                }

            }
        }
        /// <summary>
        /// 스마트로추가 성공이던 실패든 카드결제 발생시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnTermComplete(object sender, EventArgs e)
        {

            inputtime = paymentInputTimer;
            SmartroVCat.SmatroData currentSmatroReceiveData = new SmartroVCat.SmatroData();
            mSmartroVCat.ReceiveData(NPSYS.Device.SmtSndRcv, 1, ref currentSmatroReceiveData);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnTermComplete", "[스마트로 응답결과]"
                                    + " 작업내용:" + currentSmatroReceiveData.CurrentCmdTypeName
                                    + " 성공유무:" + currentSmatroReceiveData.Success.ToString()
                                    + " 응답코드:" + currentSmatroReceiveData.ReceiveReturnCode
                                    + " 응답메세지:" + currentSmatroReceiveData.ReceiveReturnMessage
                                    + " 화면메세지:" + currentSmatroReceiveData.ReceiveDisplayMsg
                                    + " 에러메세지:" + currentSmatroReceiveData.ErrorMessage
                                    + " 카드번호:" + currentSmatroReceiveData.ReceiveCardNumber
                                    + " 승인요청금액:" + currentSmatroReceiveData.ReceiveCardAmt
                                    + " 세금:" + currentSmatroReceiveData.ReceiveTaxAmt
                                    + " 봉사료:" + currentSmatroReceiveData.ReceiveBongSaInx
                                    + " 승인일자:" + currentSmatroReceiveData.ReceiveAppYmd
                                    + " 승인시간:" + currentSmatroReceiveData.ReceiveAppHms
                                    + " 필터1:" + currentSmatroReceiveData.ReceiveFIlter1
                                    + " 필터2:" + currentSmatroReceiveData.ReceiveFIlter2
                                    + " 할부개월:" + currentSmatroReceiveData.ReceiveHalbu
                                    + " 매입사코드:" + currentSmatroReceiveData.ReceiveMaipCode
                                    + " 매입사명:" + currentSmatroReceiveData.ReceiveMaipName
                                    + " 발급사코드:" + currentSmatroReceiveData.ReceiveBalgubCode
                                    + " 발급사명:" + currentSmatroReceiveData.ReceiveBalgubName
                                    + " 마스터키:" + currentSmatroReceiveData.ReceiveMasterKey
                                    + " 가맹점번호:" + currentSmatroReceiveData.ReceiveStoreNo
                                    + " 단말기번호:" + currentSmatroReceiveData.ReceiveTermNo
                                    + " 거래고유번호:" + currentSmatroReceiveData.ReceiveUniqueNo
                                    + " 워킹키:" + currentSmatroReceiveData.ReceiveWorkKey
                                    + " 승인번호:" + currentSmatroReceiveData.RecieveApprovalNumber
                                    + " 워킹인덱스:" + currentSmatroReceiveData.WoriingIndex
                                                    );
            switch (currentSmatroReceiveData.CurrentCmdType)
            {
                case SmartroVCat.SmatroData.CMDType.CardApprovalRespone:
                    CardApprovalRespone(currentSmatroReceiveData);
                    break;
                case SmartroVCat.SmatroData.CMDType.ApprovalInitializeRespone:
                    CardInitializeRespone(currentSmatroReceiveData);
                    break;
            }
        }
        /// <summary>
        /// 카드취소등의동작시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnTermExit(object sender, EventArgs e)
        {

            inputtime = paymentInputTimer;
            string errorMessage = string.Empty;
            int errorCode;
            errorCode = NPSYS.Device.SmtSndRcv.GetExitErrorCode();
            string errorName = string.Empty;
            switch (errorCode)
            {
                case -9:
                    errorName = "DATA미수신";
                    break;
                case -10:
                    errorName = "VCat Nack수신";
                    break;
                case -11:
                    errorName = "VCat 취소";
                    break;

                case -12:
                    errorName = "전문불량 etx없음";
                    break;

                case -13:
                    errorName = "전문불량 stx없음";
                    break;

            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnTermExit", "[카드비정상동작] 상태코드:" + errorCode.ToString() + " 상태값:" + errorName);

            //if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            //{
            //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPayCancle;
            //}
        }


        private void CardInitializeRespone(SmartroVCat.SmatroData pSmartroData)
        {
            if (pSmartroData.Success)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardInitializeRespone", "[카드요금취소성공]");
                if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPayCancle;
                }
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardInitializeRespone", "[카드요금취소실패]");
            }
        }
        /// <summary>
        /// 카드결제 성공또는 실패
        /// </summary>
        /// <param name="pSmartroData"></param>
        private void CardApprovalRespone(SmartroVCat.SmatroData pSmartroData)
        {
            try
            {

                if (pSmartroData.Success)
                {

                    //    CardApprovalRespone 성공유무:True 응답코드:00 응답메세지:정상 화면메세지:정상승인거래 
                    //에러메세지: 카드번호:541707********** 승인요청금액:50000 세금:4550 봉사료:0 
                    //승인일자:20160120 승인시간:220313 발급사코드: 발급사명: 화면메세지:정상승인거래 
                    //필터1: 필터2:1404712223446511 할부개월:00 매입사코드:0505 매입사명:외환카드사 마스터키:A7EB0482C68B8214 
                    //가맹점번호:00951685684     
                    //단말기번호:2114698013220310 
                    //거래고유번호:160120220312 워킹키:1 승인번호:90008063     워킹인덱스:

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardApprovalRespone", "[카드결제 성공]");
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, mCurrentNormalCarInfo, MoneyType.CreditCard, mCurrentNormalCarInfo.PaymentMoney, 0, "");
                    string[] lCardNumData = pSmartroData.ReceiveCardNumber.Split('=');
                    if (lCardNumData[0].Length > 13)
                    {
                        mCurrentNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    }
                    else
                    {
                        mCurrentNormalCarInfo.VanCardNumber = lCardNumData[0];
                    }
                    mCurrentNormalCarInfo.VanRegNo = pSmartroData.RecieveApprovalNumber.Trim();
                    mCurrentNormalCarInfo.VanDate = pSmartroData.ReceiveAppYmd;
                    mCurrentNormalCarInfo.VanRescode = pSmartroData.ReceiveReturnCode;
                    mCurrentNormalCarInfo.VanResMsg = pSmartroData.ReceiveReturnMessage;
                    mCurrentNormalCarInfo.VanSupplyPay = (Convert.ToInt32(pSmartroData.ReceiveCardAmt) - Convert.ToInt32(pSmartroData.ReceiveTaxAmt));
                    mCurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(pSmartroData.ReceiveTaxAmt);
                    mCurrentNormalCarInfo.VanCardName = pSmartroData.ReceiveBalgubName;
                    mCurrentNormalCarInfo.VanBeforeCardPay = mCurrentNormalCarInfo.PaymentMoney;
                    mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(pSmartroData.ReceiveAppYmd);
                    mCurrentNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(pSmartroData.ReceiveAppHms);
                    mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(pSmartroData.ReceiveAppYmd);
                    mCurrentNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(pSmartroData.ReceiveAppHms);

                    mCurrentNormalCarInfo.VanIssueCode = pSmartroData.ReceiveBalgubCode;
                    mCurrentNormalCarInfo.VanIssueName = pSmartroData.ReceiveBalgubName;
                    mCurrentNormalCarInfo.VanCardAcquirerCode = pSmartroData.ReceiveMaipCode;
                    mCurrentNormalCarInfo.VanCardAcquirerName = pSmartroData.ReceiveMaipName;


                    mCurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
                    //LPRDbSelect.SaveCardPay(mNormalCarInfo);
                    //결제완료된 정보를 보내야한다 아니면 가지고 있거나
                    mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                    paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);


                }
                else
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    string cardApprovalErrorMessage = string.Empty; // 카드승인에러메세지
                    cardApprovalErrorMessage = pSmartroData.ReceiveReturnMessage.Trim() == string.Empty ? pSmartroData.ReceiveDisplayMsg : pSmartroData.ReceiveReturnMessage;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardApprovalRespone", "[카드결제 실패]" + cardApprovalErrorMessage);
                    if (pSmartroData.ReceiveReturnCode == "EC")
                    {
                        paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardFrontEjuct;
                        SmatroDeveinCancle();
                        PlaySoundCard();


                    }
                    else if (pSmartroData.ReceiveReturnCode == "HD" || pSmartroData.ReceiveReturnCode == "TA")
                    {
                        paymentControl.ErrorMessage = "한도초과입니다.다른카드를 사용해주세요";
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.FullPay;
                        SmatroDeveinCancle();
                        PlaySoundCard();


                    }
                    else
                    {

                        paymentControl.ErrorMessage = "카드결제실패: " + cardApprovalErrorMessage;
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardFrontEjuct;
                        SmatroDeveinCancle();
                        PlaySoundCard();

                    }
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                    mSmartroVCat.StartSoundTick = 7;




                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | CardApprovalRespone", ex.ToString());
            }
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



        private void timerCardPay_Tick(object sender, EventArgs e)
        {
            if (mCurrentNormalCarInfo.VanAmt > 0)
            {
                timerKisCardPay.Enabled = false;
                timerKisCardPay.Stop();
                //카드실패전송
                mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                //카드실패전송완료
                PaymentComplete();
            }
        }

        delegate void Ctrl_Involk(Control ctrl, string text);

        public void setText(Control ctrl, string txtValue)
        {
            if (ctrl.InvokeRequired)
            {
                Ctrl_Involk CI = new Ctrl_Involk(setText);
                ctrl.Invoke(CI, ctrl, txtValue);
            }
            else
            {
                ctrl.Text = txtValue;
            }

        }
        private void PlayCardVideo(string pPreMovieName, string p_MovieName)
        {
            try
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + p_MovieName;
                axWindowsMediaPlayer1.uiMode = "none";
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1080 | PlayCardVideo", "동영상플레이:" + axWindowsMediaPlayer1.URL);
                //if (mIsPlayerOkStatus)
                //{
                axWindowsMediaPlayer1.Ctlcontrols.play();
                //}
                MovieTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | PlayCardVideo", ex.ToString());
            }
        }


        private void PlaySoundCard()
        {
            if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.CardInsert)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardInStep);

            }
            else if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.CardFrontEjuct)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardOutStep);

            }
            else if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.FullPay)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardFullPayStep);

            }
        }
        #endregion

        //스마트로 TIT_DIP EV-CAT 적용
        #region 스마트로 TIT DIP EV-CAT
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



        public void UnsetSmatro_DIPTIT_Evcat()
        {

            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu-CAT || UnsetSmatro_DIPTIT_Evcat ", "스마트로 DIP 기존요금취소 및 카드배출처리");
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                mSmartro_TITDIP_EVCat.CardReaderReset(NPSYS.Device.Smartro_TITDIP_Evcat);
                DateTime startDate = DateTime.Now;
                while (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop)
                {
                    TimeSpan diff = DateTime.Now - startDate;
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);


                    if (Convert.ToInt32(diff.TotalMilliseconds) > 3000)
                    {
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditPaymentMenu | SettingEnableDevice", "[상태요청 시간초과로 카드요금초기화 한걸로 처리 ");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReset;
                    }
                }
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

            }
            else
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu-CAT || UnsetSmatro_DIPTIT_Evcat ", "스마트로 DIP 기존요금취소 및 카드배출처리안함[이유:요금요청이 이전에 없음]");
            }


        }

        private void SmartroEvcat_QueryResults(object sender, AxDreampos_Ocx.__DP_Certification_Ocx_QueryResultsEvent e)
        {
            try
            {
                if (NPSYS.CurrentFormType != mCurrentFormType)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SmartroEvcat_QueryResults", "[요금폼이 아니라 실행안함 스마트로 최초 응답전문]" + e.returnData);
                    return;

                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SmartroEvcat_QueryResults", "[스마트로 최초 응답전문]" + e.returnData + " 폼타입:" + this.mCurrentFormType.ToString());
                //if (e.returnData == "FALL BACK 발생" || e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                //{
                //    paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요");
                //    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData", " 이벤트 [" + e.returnData + "] ");
                //    return;
                //}
                if (e.returnData == "FALL BACK 발생")
                {
                    paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                    return;
                }
                if (e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                {
                    return;

                }
                if (e.returnData.Contains("이미 요청중입니다!"))
                {

                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    return;
                }

                if (string.IsNullOrEmpty(e.returnData))
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 응답값 없어 처리를 안함 ");
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "[수신 전문 파싱 시작] " + e.returnData);
                if (e.returnData.Contains(((char)7).ToString()) && e.returnData.Contains(((char)3).ToString()))
                {

                    string recvData = e.returnData.Substring(e.returnData.IndexOf((char)7) + 1, e.returnData.IndexOf((char)3) - e.returnData.IndexOf((char)7) - 1);
                    string[] splitData = Regex.Split(recvData, ((char)6).ToString());

                    mSmartro_TITDIP_EVCat.RecvInfo.ClearRecvData();
                    mSmartro_TITDIP_EVCat.RecvInfo.rUserFixNo = splitData[0];
                    mSmartro_TITDIP_EVCat.RecvInfo.rResultCode = splitData[1];
                    mSmartro_TITDIP_EVCat.RecvInfo.rProcGuBun = splitData[2];
                    mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg = splitData[3];

                    if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "C")
                    {
                        if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "LIVE")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults LIVE", " EV-CAT 데몬 정상실행 확인 ");
                            return;
                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "00")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults 00", " 리더기 정상연결 " + CardDeviceStatus.CardReaderStatus.None.ToString());
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "RESET")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults RESET", " 카드리더기 리셋(강제배출) 성공 " + CardDeviceStatus.CardReaderStatus.CardReset.ToString());
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReset;

                            return;
                        }
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "리더기 상태 " + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                        return;
                    }
                    if ((mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "N")
                        && mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg.Length >= 18)
                    {
                        mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);

                        if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y")
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 성공] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 승인번호 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo + "] "
                                + " 승인일시 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate + " " + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime + "] "
                                + " 매입사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName + "] "
                                + " 발급사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName + "] "
                                + " 결제금액 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPayMoney + "] ");

                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                            paymentControl.ErrorMessage = "결제가성공하였습니다";
                            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, mCurrentNormalCarInfo, MoneyType.CreditCard, mCurrentNormalCarInfo.PaymentMoney, 0, "");

                            mCurrentNormalCarInfo.VanCheck = 1;
                            mCurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            mCurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                            mCurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                            mCurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                            mCurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                            mCurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                            mCurrentNormalCarInfo.VanSupplyPay = 0;
                            mCurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(mSmartro_TITDIP_EVCat.RecvInfo.rVat);
                            mCurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            mCurrentNormalCarInfo.VanBeforeCardPay = mCurrentNormalCarInfo.PaymentMoney;
                            mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            mCurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                            mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            mCurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                            //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                            mCurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                            mCurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            mCurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                            mCurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                            //만료차량 정기권요금제에서 일반요금제 변경기능주석완료

                            LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
                            mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제성공");

                            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney) + "원";
                            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc) + "원";
                        }
                        else
                        {

                            //카드실패전송

                            bool _isSuccessCreditSmartroParsing = mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);
                            if (_isSuccessCreditSmartroParsing)
                            {
                                mCurrentNormalCarInfo.VanCheck = 2;
                                mCurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                                mCurrentNormalCarInfo.VanCardFullNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                                mCurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                                mCurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                                mCurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                                mCurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                                mCurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                                mCurrentNormalCarInfo.VanSupplyPay = 0;
                                mCurrentNormalCarInfo.VanTaxPay =0;
                                mCurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                                mCurrentNormalCarInfo.VanBeforeCardPay = 0;
                                mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                                mCurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                                mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                                mCurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                                //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                                mCurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                                mCurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                                mCurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                                mCurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                                if (NPSYS.gUseCardFailSend)
                                {
                                    DateTime paydate = DateTime.Now;
                                    //카드실패전송
                                    mCurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                                    //카드실패전송 완료
                                    Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                                }

                            }
                            //카드실패전송 완료
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 실패] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 실패사유 [" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg + "] ");

                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                            //if (!NPSYS.gIsAutoBooth)
                            //{
                            //    if (btn_PrePage.Visible == false)
                            //    {
                            //        btn_PrePage.Visible = true;
                            //    }
                            //    if (btn_home.Visible == false)
                            //    {
                            //        btn_home.Visible = true;
                            //    }
                            //}
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제실패" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                            paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                            PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                            mSmartro_TITDIP_EVCat.StartSoundTick = 7;
                        }
                    }
                    else if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "E" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "N")
                    {

                        //카드실패전송

                        bool _isSuccessCreditSmartroParsing = mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);
                        if (_isSuccessCreditSmartroParsing)
                        {
                            mCurrentNormalCarInfo.VanCheck = 2;
                            mCurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            mCurrentNormalCarInfo.VanCardFullNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            mCurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                            mCurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                            mCurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                            mCurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                            mCurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                            mCurrentNormalCarInfo.VanSupplyPay = 0;
                            mCurrentNormalCarInfo.VanTaxPay = 0;
                            mCurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            mCurrentNormalCarInfo.VanBeforeCardPay = 0;
                            mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            mCurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                            mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            mCurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

         
                            mCurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                            mCurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            mCurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                            mCurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                            if (NPSYS.gUseCardFailSend)
                            {
                                DateTime paydate = DateTime.Now;
                      
                                mCurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                   
                                Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                            }

                        }
                        //카드실패전송 완료
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 실패] "
                            //+ " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                            + " 실패사유 [" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg + "] ");
                        if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg.Contains("동글이 요청 타임 오버"))
                        {
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [결제요청중 타임오버로 다시결제요청시도] ");
                            return;

                        }
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        //if (!NPSYS.gIsAutoBooth)
                        //{
                        //    if (btn_PrePage.Visible == false)
                        //    {
                        //        btn_PrePage.Visible = true;
                        //    }
                        //    if (btn_home.Visible == false)
                        //    {
                        //        btn_home.Visible = true;
                        //    }
                        //}
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제실패" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                        //paymentControl.ErrorMessage = "카드결제실패:" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;
                        paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                        PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        mSmartro_TITDIP_EVCat.StartSoundTick = 7;
                    }
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 비정상 응답 [ " + e.returnData + " ]");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 전문 파싱중 예외상황 [ " + ex.Message + " ]");
            }

        }
        // 스마트로 EVCAT 결제요청
        private void SmatroEVCAT_CardApprovalAction()
        {

            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess
                || mCurrentNormalCarInfo.VanAmt != 0 || mCurrentNormalCarInfo.GetInComeMoney > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (mCurrentNormalCarInfo.GetInComeMoney == 0)
            {
                paymentControl.ErrorMessage = string.Empty;
                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[승인쓰레드웨이트]");
                    mCreditCardThreadLock.WaitOne(1000);
                    mCreditCardThreadLock.Reset();
                    if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 신용카드 취소결제 시작]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                        mSmartro_TITDIP_EVCat.CanclePayment(NPSYS.Device.Smartro_TITDIP_Evcat, mCurrentNormalCarInfo.PaymentMoney.ToString(), mCurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), mCurrentNormalCarInfo.VanRegNo_Cancle);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 신용카드 취소결제 시작]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());

                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드요금결제요청 시작]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                        mSmartro_TITDIP_EVCat.CardApproval(NPSYS.Device.Smartro_TITDIP_Evcat, mCurrentNormalCarInfo.PaymentMoney.ToString());
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드요금결제요청 종료]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    }
                    mCreditCardThreadLock.Set();
                }
            }
        }
        // 스마트로 EVCAT 결제요금 변경
        private void SmatroEVCAT_ChangePayMoneyAction()
        {

            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess
                || mCurrentNormalCarInfo.VanAmt != 0 || mCurrentNormalCarInfo.GetInComeMoney > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (mCurrentNormalCarInfo.GetInComeMoney == 0)
            {
                paymentControl.ErrorMessage = string.Empty;
                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 요금변경 결제요청 시작]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    mSmartro_TITDIP_EVCat.ChangePayMoney(NPSYS.Device.Smartro_TITDIP_Evcat, mCurrentNormalCarInfo.PaymentMoney.ToString());
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 요금변경 결제요청 종료]" + mCurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                }
            }
        }

        //스마트로 TIT_DIP EV-CAT 적용완료



        //KOCSE 카드리더기 추가



        #region KOCSE TCM
        private void CardActionKocesTcm()
        {

            try
            {
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    KocesTcmMotor.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    KocesTcmMotor.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.None)
                {
                    bool isSuceessAccept = KocesTcmMotor.CardAccept();
                    if (isSuceessAccept)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }

                }
                int resultCardState = KocesTcmMotor.CardState();
                switch (resultCardState)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                        break;
                    default:
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerKocesTcmState_Tick", "[코세스 장비장애코드]" + resultCardState.ToString());
                        break;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    //KOCSE결제시 이미지 추가

                    pic_Wait_MSG_WAIT.BringToFront();
                    pic_Wait_MSG_WAIT.Visible = true;
                    // PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardInReading);
                    if (!NPSYS.gIsAutoBooth)
                    {
                        //if (btn_PrePage.Visible == true) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_PrePage.Visible = false; //뉴타입주석
                        //} //뉴타입주석
                        //if (btn_home.Visible == true) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_home.Visible = false; //뉴타입주석
                        //} //뉴타입주석
                    }
                    Application.DoEvents();
                    Thread.Sleep(700);
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    KocesTcmMotor.CardEject();
                    pic_Wait_MSG_WAIT.SendToBack();
                    pic_Wait_MSG_WAIT.Visible = false;
                    if (!NPSYS.gIsAutoBooth)
                    {
                        //if (btn_PrePage.Visible == false)  //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_PrePage.Visible = true; //뉴타입주석
                        //} //뉴타입주석
                        //if (btn_home.Visible == false) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_home.Visible = true; //뉴타입주석
                        //} //뉴타입주석
                    }
                    //KOCSE결제시 이미지 추가
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|timer_CardReader1_Tick", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else // 잘못된 티켓
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKocesTcmState_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch
            {
            }

        }
        #endregion
        //KICCTS141적용


        #region KSNET
        //KSNet 적용
        private void KsnetCardAction()
        {

            //btnCardApproval.Visible = false; //뉴타입주석

            PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gKSNetCardInStep);
            try
            {

                TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | KsnetCardAction", "[카드결제 KSNET장비에 요청]");
                Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                {
                    NPSYS.CashCreditCount += 1;
                    NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnCardAction_Click", "정상적인 카드결제");
                    if (mCurrentNormalCarInfo.PaymentMoney == 0)
                    {
                        //카드실패전송
                        mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                        //카드실패전송완료
                        PaymentComplete();

                        return;
                    }
                }
                else if (_CardpaySuccess.Message.Split(':')[0] == "0002")// 
                {

                    return;
                }
                else // 잘못된 티켓
                {
                    TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu|timer_CardReader1_Tick", "정상적인 카드결제안됨" + _CardpaySuccess.Message);
                    paymentControl.ErrorMessage = _CardpaySuccess.Message;
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                    return;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | btnCardAction_Click", ex.ToString());
            }
            finally
            {
                if (mCurrentNormalCarInfo.PaymentMoney != 0)
                {

                    StartPlayVideo();

                }
            }
        }
        //KSNet 적용완료
        #endregion


        #region KICC TS141

        private void btnCardApproval_Click(object sender, EventArgs e)
        {

            if (mCurrentNormalCarInfo.PaymentMoney > 0)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | btnCardApproval_Click", "[신용카드 결제버튼누름]");
                //포시즌 카드누를시 화면숨김
                mCardVisible = 10;
                //btnCardApproval.Visible = false;  //뉴타입주석

                //포시즌 카드누를시 화면숨김 주석완료
                if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
                {
                    KiccTs141.Initilize();
                    int Creditpaymoneys = mCurrentNormalCarInfo.PaymentMoney;

                    int taxsResult = (int)(Creditpaymoneys / 11);
                    int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가
                    bool isApproveSuccess = KiccTs141.Approval(Creditpaymoneys.ToString(), taxsResult.ToString());
                    if (isApproveSuccess)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        paymentControl.ErrorMessage = string.Empty;
                        timerKiccTs141State.Enabled = true;
                        timerKiccTs141State.Start();
                        timerKiccSoundPlay.Enabled = true;
                        timerKiccSoundPlay.Start();
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | btnCardApproval_Click", "[신용카드 결제요청됨]");
                    }
                }
                //KSNet 적용
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KSNET)
                {

                    KsnetCardAction();
                }
                //KSNet 적용완료

            }
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



        public void GetKiccState()
        {

            int cmd = 0;
            int gcd = 0;
            int jcd = 0;
            int rcd = 0;
            byte[] rdata = new byte[4086];
            byte[] rhexadata = new byte[4086];
            int result = KiccTs141.KGetEvent(ref cmd, ref gcd, ref jcd, ref rcd, rdata, rhexadata);
            if (result >= 0)
            {
                string rCmd = cmd.ToString();
                string rGcd = gcd.ToString();
                string rJcd = jcd.ToString();
                string rRcd = rcd.ToString();

                string rData = Encoding.Default.GetString(rdata);
                int subIndex = 0;
                subIndex += 2;
                string returnCode = Encoding.Default.GetString(rdata, subIndex, 4).Trim();


                if (returnCode == "0000")
                {
                    //btnCardApproval.Visible = false;  //뉴타입주석
                    subIndex += 4;
                    string tId = Encoding.Default.GetString(rdata, subIndex, 8).Trim(); //터미널ID
                    subIndex += 8;
                    string wcc = Encoding.Default.GetString(rdata, subIndex, 1).Trim();
                    subIndex += 1;
                    string cardNo = Encoding.Default.GetString(rdata, subIndex, 40).Trim();
                    subIndex += 40;
                    string halbu = Encoding.Default.GetString(rdata, subIndex, 2).Trim();
                    subIndex += 2;
                    string cardMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim();
                    subIndex += 8;
                    string bonsaMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim();
                    subIndex += 8;
                    string VatMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim(); // vat
                    subIndex += 8;
                    string approvalNo = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); //승인번호
                    subIndex += 12;
                    string approvalDate = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); //승인일시
                    subIndex += 12;
                    string issueCode = Encoding.Default.GetString(rdata, subIndex, 3).Trim(); // 발급사코드
                    subIndex += 3;
                    string issueName = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 발급사명
                    subIndex += 20;
                    string gamangCode = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); // 가맹점코드
                    subIndex += 12;
                    string accquireCode = Encoding.Default.GetString(rdata, subIndex, 3).Trim(); // 매입사코드
                    subIndex += 3;
                    string accquireName = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 매입사명
                    subIndex += 20;
                    string posSequnceNo = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 발급사명
                    subIndex += 20;

                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | GetKiccState", "[카드결제 성공]"
                        + " [TID단말기번호]" + tId
                        + " [승인번호]" + approvalNo
                        + " [승인시각]" + approvalDate
                        + " [매입사명]" + accquireName
                        + " [발급사명]" + issueName
                        + " [포스거래번호]" + posSequnceNo
                        + " [결제금액]" + cardMoney);
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, mCurrentNormalCarInfo, MoneyType.CreditCard, mCurrentNormalCarInfo.PaymentMoney, 0, "");
                    mCurrentNormalCarInfo.VanCheck = 1;
                    string[] lCardNumData = cardNo.Split('=');
                    if (lCardNumData[0].Length > 13)
                    {
                        mCurrentNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    }
                    else
                    {
                        mCurrentNormalCarInfo.VanCardNumber = lCardNumData[0];
                    }
                    approvalDate = "20" + approvalDate;
                    mCurrentNormalCarInfo.VanRegNo = approvalNo;
                    mCurrentNormalCarInfo.VanDate = approvalDate;
                    mCurrentNormalCarInfo.VanRescode = returnCode;
                    mCurrentNormalCarInfo.VanResMsg = "정상";
                    int Creditpaymoneys = mCurrentNormalCarInfo.PaymentMoney;

                    int taxsResult = (int)(Creditpaymoneys / 11);
                    int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가
                    mCurrentNormalCarInfo.VanSupplyPay = SupplyPay;
                    mCurrentNormalCarInfo.VanTaxPay = taxsResult;
                    mCurrentNormalCarInfo.VanCardName = issueName;
                    mCurrentNormalCarInfo.VanBeforeCardPay = Convert.ToInt32(mCurrentNormalCarInfo.PaymentMoney);
                    mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(approvalDate.Substring(0, 8));
                    mCurrentNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(approvalDate.Substring(8));
                    mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(approvalDate.Substring(0, 8));
                    mCurrentNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(approvalDate.Substring(8));

                    //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                    mCurrentNormalCarInfo.VanIssueCode = issueCode;
                    mCurrentNormalCarInfo.VanIssueName = issueName;
                    mCurrentNormalCarInfo.VanCardAcquirerCode = accquireCode;
                    mCurrentNormalCarInfo.VanCardAcquirerName = accquireName;
                    //만료차량 정기권요금제에서 일반요금제 변경기능주석완료


                    mCurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
                    //LPRDbSelect.SaveCardPay(mNormalCarInfo); 
                    //통합처리로 결제정보를 전달해야한다
                    mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | GetKiccState", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                    paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                }
                else // fallback거래등현재 확인결과 9999는 fallback으로보임
                {
                    if (returnCode == "9999")
                    {
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | GetKiccState", "사용자취소]");

                    }
                    else
                    {
                        KiccTs141.Initilize();
                        paymentControl.ErrorMessage = "[카드결제실패]" + "카드를 빼시고 카드결제버튼을 눌러 다시 시도요청";
                        PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        KiccTs141.StartSoundTick = 7;
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | GetKiccState", "[KICC카드기기 초기화]" + "원인코드:" + returnCode);
                    }
                }
            }
        }

        private void timerKiccTs141State_Tick(object sender, EventArgs e)
        {
            try
            {

                timerKiccTs141State.Stop();

                GetKiccState();
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCurrentNormalCarInfo.VanAmt > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    //카드실패전송
                    mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
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
                if (mCurrentNormalCarInfo.PaymentMoney != 0)
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

        #endregion
        //KICCTS141적용완료

        #endregion

        #region 지폐 및 동전관련


        private void StartMoneyInsert()
        {
            try
            {
                if (isOnlyCard())
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "카드결제라 동전지폐 관련시작 모든 작업 시작안함");
                    return;
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "동전지폐 관련 모든 작업 시작");
                if (!NPSYS.Device.UsingSettingCoinReader && !NPSYS.Device.UsingSettingBill)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|StartMoneyInsert", "지폐/동전 사용안함");

                    return;
                }
                if (NPSYS.Device.isUseDeviceCoinReaderDevice && NPSYS.Device.UsingSettingCoinReader)
                {

                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "동전 입수가능동작 시작진행");
                    CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "동전 입수가능동작 시작종료:" + l_CoinReaderResult.ToString());
                    if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "다시한번 동전 받는동작 작동");
                        l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                        if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                        {
                            NPSYS.Device.CoinReader.DisableCoinRead();
                            NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                            TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormPaymentMenu|StartMoneyInsert", l_CoinReaderResult.ToString());
                            SetLanuageDynamic(NPSYS.CurrentLanguageType);

                            NPSYS.LedLight();
                        }
                    }

                }

                if (NPSYS.Device.isUseDeviceBillReaderDevice && NPSYS.Device.UsingSettingBillReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | StartMoneyInsert", "지폐 입수가능동작 시작진행");
                    BillReader.BillRederStatusType curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | StartMoneyInsert", "지폐 입수가능동작 시작종료 성공유무:" + curentBillRederStatusType.ToString());

                    if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                    {
                        NPSYS.Device.BillReader.Reset();
                        curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StartMoneyInsert", "지폐을 받는 동작 작동 문제가 생겨서 다시시도 성공유무:" + curentBillRederStatusType.ToString());
                        if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                        {
                            NPSYS.Device.BillReader.Reset();
                            curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                            if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                            {
                                SetLanuageDynamic(NPSYS.CurrentLanguageType);
                                NPSYS.LedLight();

                            }

                        }

                    }
                }
                if (NPSYS.Device.isUseDeviceBillReaderDevice || NPSYS.Device.isUseDeviceCoinReaderDevice)
                {
                    tmrReadAccount.Enabled = true;
                    tmrReadAccount.Start();
                }



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymenu|StartInsert", ex.ToString());
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "동전지폐 관련 모든 작업 종료");
            }
        }


        private void StopMoneyInsert()
        {
            try
            {
                if (isOnlyCard())
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "카드만결제라 동전지폐 종료관련 모든 작업 시작안함");
                    return;
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StopMoneyInsert", "동전지폐 관련 모든 작업 시작");
                if (!NPSYS.Device.UsingSettingCoinReader && !NPSYS.Device.UsingSettingBill)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|StartMoneyInsert", "지폐/동전 사용안함");

                    return;
                }

                if (NPSYS.Device.isUseDeviceCoinReaderDevice && NPSYS.Device.UsingSettingCoinReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StopMoneyInsert", "동전을 받는 동작 작동 행위를 멈춤 시작");
                    CoinReader.CoinReaderStatusType Result = NPSYS.Device.CoinReader.DisableCoinRead();
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StopMoneyInsert", "동전을 받는 동작 작동 행위를 멈춤 종료:" + Result.ToString());

                }
                if (NPSYS.Device.isUseDeviceBillReaderDevice && NPSYS.Device.UsingSettingBillReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StopMoneyInsert", "지폐를 받는 동작 작동 행위를 멈춤 시작");
                    NPSYS.Device.BillReader.BillDIsableAccept();
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StopMoneyInsert", "지폐를 받는 동작 작동 행위를 멈춤 종료 성공유무:" + NPSYS.Device.BillReader.BillDIsableAccept().ToString());
                }


                if (tmrReadAccount.Enabled == true)
                {
                    tmrReadAccount.Enabled = false;
                    tmrReadAccount.Stop();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymenu|StopMoneyInsert", ex.ToString());
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StopMoneyInsert", "동전지폐 관련 모든 작업 종료");
            }

        }

        /// <summary>
        /// 들어온 지폐 및 동전을 로컬 DB에 저장한다.
        /// </summary>
        /// <param name="p_BillVaule"></param>
        private void InsertMoney(string p_BillVaule)
        {
            try
            {
                inputtime = paymentInputTimer;

                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int inMoney = 0;
                inMoney = Convert.ToInt32(p_BillVaule.ToUpper().Replace("QTY", ""));
                if (inMoney <= 0)
                {
                    TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|InsertMoney", "들어온 돈:0");
                    return;
                }


                paymentControl.ErrorMessage = "현금취소시 카드결제가능 합니다";

                // 스마트로 추가
                if (mCurrentNormalCarInfo.Current_Money == 0) // 최초동전넣었다면 취소
                {
                    paymentControl.ButtonEnable(ButtonEnableType.InsertCoin);
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
                    {
                        timerSmartroVCat.Enabled = false;
                        timerSmartroVCat.Stop();
                        SmatroDeveinCancle();
                    }
                    // 2016.10.27 KIS_DIP 추가
                    else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                    {
                        // Kis_TIT_DIpDeveinCancle();

                    }
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                    {

                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                        timerKisCardPay.Enabled = false;
                        timerKisCardPay.Stop();
                        timerSmatro_TITDIP_Evcat.Tick -= timerSmatro_TITDIP_Evcat_Tick;
                        this.timerSmatro_TITDIP_Evcat.Enabled = false;
                        this.timerSmatro_TITDIP_Evcat.Stop();
                        paymentControl.ErrorMessage = string.Empty;
                        UnsetSmatro_DIPTIT_Evcat();

                    }
                    // 2016.10.27  KIS_DIP 추가종료

                }
                InsertMoneyChangeValue(p_BillVaule);
                TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|InsertMoney", "지불해야할 요금:" + mCurrentNormalCarInfo.PaymentMoney.ToString() + "총투입금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString() + " 투입금액:" + inMoney.ToString() + " 거스름돈:" + mCurrentNormalCarInfo.GetChargeMoney);
                paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                // 스마트로 추가 종료


                //BoothControl.ProtocolData sendDiscountProtocol = new BoothControl.ProtocolData();
                //sendDiscountProtocol.CurrentCommand = BoothControl.ProtocolData.Command.GETCURRENT_PAYMONEY;
                //NPSYS.sendJunsanInfo(sendDiscountProtocol, mNormalCarInfo);
                //통합처리 동전들어갔을때 전문보내야함

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|InsertMoney", ex.ToString());
            }

        }


        /// <summary>
        /// 현재 투입금액을 증가시킨다.
        /// </summary>
        /// <param name="message"></param>
        public void InsertMoneyChangeValue(string message)
        {

            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                switch (message.ToUpper())
                {

                    case "50QTY":

                        mCurrentNormalCarInfo.InCome50Qty = mCurrentNormalCarInfo.InCome50Qty + 1;
                        break;
                    case "100QTY":

                        mCurrentNormalCarInfo.InCome100Qty = mCurrentNormalCarInfo.InCome100Qty + 1;
                        break;
                    case "500QTY":

                        mCurrentNormalCarInfo.InCome500Qty = mCurrentNormalCarInfo.InCome500Qty + 1;
                        break;

                    case "1000QTY":

                        mCurrentNormalCarInfo.InCome1000Qty = mCurrentNormalCarInfo.InCome1000Qty + 1;
                        break;
                    case "5000QTY":

                        mCurrentNormalCarInfo.InCome5000Qty = mCurrentNormalCarInfo.InCome5000Qty + 1;
                        break;
                    case "10000QTY":

                        mCurrentNormalCarInfo.InCome10000Qty = mCurrentNormalCarInfo.InCome10000Qty + 1;
                        break;

                    case "50000QTY":

                        mCurrentNormalCarInfo.InCome50000Qty = mCurrentNormalCarInfo.InCome50000Qty + 1;
                        break;


                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (message.ToUpper())
                {

                    case "1QTY":

                        mCurrentNormalCarInfo.InCome50Qty = mCurrentNormalCarInfo.InCome50Qty + 1;
                        break;
                    case "5QTY":

                        mCurrentNormalCarInfo.InCome100Qty = mCurrentNormalCarInfo.InCome100Qty + 1;
                        break;
                    case "10QTY":

                        mCurrentNormalCarInfo.InCome500Qty = mCurrentNormalCarInfo.InCome500Qty + 1;
                        break;

                    case "20QTY":

                        mCurrentNormalCarInfo.InCome1000Qty = mCurrentNormalCarInfo.InCome1000Qty + 1;
                        break;
                    case "50QTY":

                        mCurrentNormalCarInfo.InCome5000Qty = mCurrentNormalCarInfo.InCome5000Qty + 1;
                        break;
                    case "100QTY":

                        mCurrentNormalCarInfo.InCome10000Qty = mCurrentNormalCarInfo.InCome10000Qty + 1;
                        break;



                }
            }


        }


        ///// <summary>
        ///// 지폐를 돈통에 넣는 작업 , 지폐입수후 BillAccept 명령 실행하지 않을시 자동으로 돈이 앞으로 리젝된다
        ///// </summary>
        //private byte[] getBillInsert()
        //{
        //    return NPSYS.Device.BillReader.BillAccept();

        //}

        ///// <summary>
        ///// 지폐를 돈통에 넣지않는 작업 Reject
        ///// </summary>
        //private byte[] getBillReject()
        //{
        //    return NPSYS.Device.BillReader.BillReject();

        //}


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


                if (mCurrentNormalCarInfo.Current_Money > 0)
                {
                    paymentControl.CancelButtonVisible = true;
                }
                else
                {
                    paymentControl.CancelButtonVisible = false;
                }

                if (mCurrentNormalCarInfo.Current_Money == 0)
                {
                    tmrReadAccount.Start();
                    return;
                }

                if (mCurrentNormalCarInfo.PaymentMoney == 0)
                {
                    //카드실패전송
                    mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
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


        #endregion

        #endregion

        #region 결제성공

        /// <summary>
        /// 주차요금을 모두 고객이 지불했을시 실행되며 영수증 폼으로 이동
        /// </summary>
        private void PaymentComplete()
        {
            try
            {

                PaymentEndAction();
                CashRecipt();
                if (mCurrentNormalCarInfo.GetInComeMoney > 0)
                {
                    if (CompletOutChargeAction() != PaymentResult.Success)
                    {
                        //시제설정누락처리
                        mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessNotOut; //시제설정누락처리
                        ReceiptChargeErrorActions();
                        //카드실패전송
                        mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료

                    }
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "결제처리 전송시작");
                DateTime paydate = DateTime.Now;
                Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                if (currentCar.status.Success == false)
                {
                    // DB에 저장하고 재전송처리해야함
                    return;
                }
                NormalCarInfo SendcarInfo = new NormalCarInfo();
                SendcarInfo = CommonFuction.Clone<NormalCarInfo>(mCurrentNormalCarInfo);
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
            if (NPSYS.Device.UsingUsingSettingCashReceipt && mCurrentNormalCarInfo.Current_Money > 0) // 현금영수증 사용이라면
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CashRecipt", "[현금영수증 처리시도]");
                Result cashResult = m_PayCardandCash.CashRecipt(mCurrentNormalCarInfo);
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

        #endregion

        #region 취소버튼 처리

        private void btn_CashCancle_Click(object sender, EventArgs e)
        {
            if (mCurrentNormalCarInfo.PaymentMoney == 0 || mCurrentNormalCarInfo.Current_Money == 0)
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
        //시제설정누락처리 주석처리
        //private void CancleInitailize(NormalCarInfo.MoneyInOutType pInOutType)
        //{

        //    NormalCarInfo mCancleNormalCarinfo = new NormalCarInfo();
        //    mCancleNormalCarinfo = ObjectCopier.Clone<NormalCarInfo>(mCurrentNormalCarInfo);
        //    mCancleNormalCarinfo.CanCleClear(pInOutType);
        //    SetCarInfo(mCancleNormalCarinfo);
        //    mCancleNormalCarinfo = null;

        //}
        //시제설정누락처리
        private void CashCancleAction()
        {
            try
            {

                if (mCurrentNormalCarInfo.Current_Money > 0)  // 투입된 금액이 있다면
                {

                    paymentControl.ButtonEnable(ButtonEnableType.CashCancle);
                    SettingDisableDevice();

                    PaymentResult result = CancleRefundMoneyAction();

                    if (result == PaymentResult.Success) // 지폐나 동전불출할수 있으면
                    {
                        mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "취소시 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        //카드실패전송
                        mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료
                        Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }

                        mCurrentNormalCarInfo.CanCleClear();
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
                        mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                        //카드실패전송완료
                        mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelNotOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "취소시 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }
                        ReceiptCancleErrorActions();
                        mCurrentNormalCarInfo.CanCleClear();
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
        //시제설정누락처리 완료
        private void CashCancleFormCloseAction(bool pIsSetDisable = false)
        {
            try
            {

                if (mCurrentNormalCarInfo.Current_Money > 0)  // 투입된 금액이 있다면
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|CashCancleFormCloseAction", "시간초과로 입수된 돈 방출처리 방출금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString());

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
                            mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelOut; //시제설정누락처리
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 결제처리 전송시작");
                            DateTime paydate = DateTime.Now;
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                            //카드실패전송완료

                            Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                            if (currentCar.status.Success == false)
                            {
                                // DB에 저장하고 재전송처리해야함
                                return;
                            }
                            paymentControl.CancelButtonVisible = false;


                        }
                        else
                        {
                            mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CancelNotOut; //시제설정누락처리
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 결제처리 전송시작");
                            DateTime paydate = DateTime.Now;
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                            //카드실패전송완료

                            Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                            if (currentCar.status.Success == false)
                            {
                                // DB에 저장하고 재전송처리해야함
                                return;
                            }
                            paymentControl.CancelButtonVisible = false;
                            ReceiptCancleErrorActions(true);
                            mCurrentNormalCarInfo.CanCleClear();
                        }
                    }
                    else
                    {
                        mCurrentNormalCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.CommandOut; //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymentMenu | Payment", "시간초과로 취소시 강제입금 결제처리 전송시작");
                        DateTime paydate = DateTime.Now;
                        Payment currentCar = mHttpProcess.PaySave(mCurrentNormalCarInfo, paydate);
                        if (currentCar.status.Success == false)
                        {
                            // DB에 저장하고 재전송처리해야함
                            return;
                        }
                        mCurrentNormalCarInfo.CanCleClear();
                    }
                    //시제설정누락처리 완료

                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|btn_CashCancle_Click", ex.ToString());
            }
        }



        #endregion

        #region 보관증관련
        //시제설정누락처리 수정
        /// <summary>
        /// 동전 및 지폐장비 이상일때 현금 취소시 보관증 출력
        /// </summary>
        private void ReceiptCancleErrorActions(bool isFormClose = false)   // 보관증출력
        {

            LPRDbSelect.LogMoney(PaymentType.CashTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mCurrentNormalCarInfo, MoneyType.CashTicket, 0, mCurrentNormalCarInfo.GetNotDisChargeMoney, "보관증");
            TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|ReceiptErrorActions", "보관증발행액" + mCurrentNormalCarInfo.GetNotDisChargeMoney);
            Print.CashTicketNotCoinPrint(mCurrentNormalCarInfo, false, this.Name);
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

                LPRDbSelect.LogMoney(PaymentType.CashTicket, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mCurrentNormalCarInfo, MoneyType.CashTicket, 0, mCurrentNormalCarInfo.GetNotDisChargeMoney, "보관증");
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormRecipt|ReceiptErrorActions", "보관증발행액:" + mCurrentNormalCarInfo.GetNotDisChargeMoney);
                Print.CashTicketNotCoinPrint(mCurrentNormalCarInfo, true, this.Name);

                System.Threading.Thread.Sleep(500);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormRecipt|ReceiptErrorActions", "보관증발행");
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormRecipt|ReceiptErrorActions", "예외사항:" + ex.ToString());
            }

        }
        #endregion

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
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "취소명령시 입금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCancelMoney();

                    mCurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할금액: 5000원수량:" + mCurrentNormalCarInfo.Cancle5000Qty.ToString() + "  1000원수량:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString() + "  500원수량:" + mCurrentNormalCarInfo.Cancle500Qty.ToString() + "  100원수량:" + mCurrentNormalCarInfo.Cancle100Qty.ToString() + "  50원수량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (mCurrentNormalCarInfo.Cancle5000Qty > 0 || mCurrentNormalCarInfo.Cancle1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (mCurrentNormalCarInfo.Cancle1000Qty * 2) + (mCurrentNormalCarInfo.Cancle5000Qty * 10);
                            mCurrentNormalCarInfo.Cancle1000Qty = 0;
                            mCurrentNormalCarInfo.Cancle5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "[지폐대신 동전방출로 변경] 방출할500원 수량: " + mCurrentNormalCarInfo.Cancle500Qty.ToString());

                        }
                        else
                        {
                            if (mCurrentNormalCarInfo.Cancle5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "5000원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle5000Qty.ToString() + "개");
                                int out5000Qty = mCurrentNormalCarInfo.Cancle5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillCancleDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (mCurrentNormalCarInfo.Cancle1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1000원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString() + "개");
                                int out1000Qty = mCurrentNormalCarInfo.Cancle1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillCancleDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (mCurrentNormalCarInfo.Cancle1000Qty * 2) + (mCurrentNormalCarInfo.Cancle5000Qty * 10);
                        mCurrentNormalCarInfo.Cancle1000Qty = 0;
                        mCurrentNormalCarInfo.Cancle5000Qty = 0;

                    }
                    if (mCurrentNormalCarInfo.CurrentCancleCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할 동전이 없으므로 성공:" + mCurrentNormalCarInfo.CurrentCancleCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        mCurrentNormalCarInfo.NotdisChargeMoney500Qty = mCurrentNormalCarInfo.Cancle500Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney100Qty = mCurrentNormalCarInfo.Cancle100Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty;
                        mCurrentNormalCarInfo.Cancle50Qty = 0;
                        mCurrentNormalCarInfo.Cancle100Qty = 0;
                        mCurrentNormalCarInfo.Cancle500Qty = 0;

                        return PaymentResult.Fail;
                    }





                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (mCurrentNormalCarInfo.Cancle500Qty > 0) // 500원 방출수량이 있다면
                        {
                            mCurrentNormalCarInfo.Cancle100Qty = mCurrentNormalCarInfo.Cancle100Qty + (mCurrentNormalCarInfo.Cancle500Qty * 5); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "500원 장비 미사용 또는 장비고장으로 500원 100원으로 교체:" + mCurrentNormalCarInfo.Cancle100Qty.ToString());
                            mCurrentNormalCarInfo.Cancle500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < mCurrentNormalCarInfo.Cancle500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (mCurrentNormalCarInfo.Cancle500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "500원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle500Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (mCurrentNormalCarInfo.Cancle100Qty > 0)
                        {
                            mCurrentNormalCarInfo.Cancle50Qty = mCurrentNormalCarInfo.Cancle50Qty + (mCurrentNormalCarInfo.Cancle100Qty * 2); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "100원 장비 미사용 또는 장비고장으로 100원 50원으로 교체:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                            mCurrentNormalCarInfo.Cancle100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < mCurrentNormalCarInfo.Cancle100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (mCurrentNormalCarInfo.Cancle100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "100원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle100Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (mCurrentNormalCarInfo.Cancle50Qty > 0)
                        {
                            mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty;
                            mCurrentNormalCarInfo.Cancle50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.
                    if (cash50SettingQty < mCurrentNormalCarInfo.Cancle50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "동전 50원 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty - cash50SettingQty;
                        mCurrentNormalCarInfo.Cancle50Qty = cash50SettingQty;
                    }


                    if (mCurrentNormalCarInfo.Cancle50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "50원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle50Qty.ToString() + "개");
                        l_resultPayment = CoinOutCancleMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (mCurrentNormalCarInfo.GetNotDisChargeMoney > 0)
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
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "취소명령시 입금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCancelMoney();

                    mCurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할금액: 50PHP수량:" + mCurrentNormalCarInfo.Cancle5000Qty.ToString() + "  20pPHP원수량:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString() + "  10PHP원수량:" + mCurrentNormalCarInfo.Cancle500Qty.ToString() + "  5PHP수량:" + mCurrentNormalCarInfo.Cancle100Qty.ToString() + "  1PHP수량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (mCurrentNormalCarInfo.Cancle5000Qty > 0 || mCurrentNormalCarInfo.Cancle1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (mCurrentNormalCarInfo.Cancle1000Qty * 2) + (mCurrentNormalCarInfo.Cancle5000Qty * 5);
                            mCurrentNormalCarInfo.Cancle1000Qty = 0;
                            mCurrentNormalCarInfo.Cancle5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "[지폐대신 동전방출로 변경] 방출할10PHP 수량: " + mCurrentNormalCarInfo.Cancle500Qty.ToString());

                        }
                        else
                        {
                            if (mCurrentNormalCarInfo.Cancle5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "50PHP권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle5000Qty.ToString() + "개");
                                int out5000Qty = mCurrentNormalCarInfo.Cancle5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillCancleDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (mCurrentNormalCarInfo.Cancle1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "20PHP원권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString() + "개");
                                int out1000Qty = mCurrentNormalCarInfo.Cancle1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillCancleDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {

                        mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (mCurrentNormalCarInfo.Cancle1000Qty * 2) + (mCurrentNormalCarInfo.Cancle5000Qty * 5);
                        mCurrentNormalCarInfo.Cancle1000Qty = 0;
                        mCurrentNormalCarInfo.Cancle5000Qty = 0;

                    }
                    if (mCurrentNormalCarInfo.CurrentCancleCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "방출할 동전이 없으므로 성공:" + mCurrentNormalCarInfo.CurrentCancleCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        mCurrentNormalCarInfo.NotdisChargeMoney500Qty = mCurrentNormalCarInfo.Cancle500Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney100Qty = mCurrentNormalCarInfo.Cancle100Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty;
                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.



                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 10PHP 방출을 할수없을때
                    {
                        if (mCurrentNormalCarInfo.Cancle500Qty > 0) // 10PHP 방출수량이 있다면
                        {
                            mCurrentNormalCarInfo.Cancle100Qty = mCurrentNormalCarInfo.Cancle100Qty + (mCurrentNormalCarInfo.Cancle500Qty * 2); // 부족한 10PHP원 수량을 5PHP원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "10PHP 장비 미사용 또는 장비고장으로 10PHP 5PHP으로 교체:" + mCurrentNormalCarInfo.Cancle100Qty.ToString());
                            mCurrentNormalCarInfo.Cancle500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < mCurrentNormalCarInfo.Cancle500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (mCurrentNormalCarInfo.Cancle500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "10PHP권 방출명령 내림:" + mCurrentNormalCarInfo.Cancle500Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (mCurrentNormalCarInfo.Cancle100Qty > 0)
                        {
                            mCurrentNormalCarInfo.Cancle50Qty = mCurrentNormalCarInfo.Cancle50Qty + (mCurrentNormalCarInfo.Cancle100Qty * 5); // 부족한 5PHP 수량을 1PHP권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CancleRefundMoneyAction", "5PHP 장비 미사용 또는 장비고장으로 5PHP 1PHP으로 교체:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                            mCurrentNormalCarInfo.Cancle100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < mCurrentNormalCarInfo.Cancle100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (mCurrentNormalCarInfo.Cancle100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "5PHP 방출명령 내림:" + mCurrentNormalCarInfo.Cancle100Qty.ToString() + "개");
                        CoinOutCancleMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (mCurrentNormalCarInfo.Cancle50Qty > 0)
                        {
                            mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty;
                            mCurrentNormalCarInfo.Cancle50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }


                    if (cash50SettingQty < mCurrentNormalCarInfo.Cancle50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1PHP 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Cancle50Qty - cash50SettingQty;
                        mCurrentNormalCarInfo.Cancle50Qty = cash50SettingQty;
                    }


                    if (mCurrentNormalCarInfo.Cancle50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CancleRefundMoneyAction", "1PHP 방출명령 내림:" + mCurrentNormalCarInfo.Cancle50Qty.ToString() + "개");
                        l_resultPayment = CoinOutCancleMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (mCurrentNormalCarInfo.GetNotDisChargeMoney > 0)
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
                mCurrentNormalCarInfo.LastErrorMessage = "FormPaymenu | CancleRefundMoneyAction|Exception:" + ex.ToString();
                return PaymentResult.Fail;
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymenu | CancleRefundMoneyAction", "[방출한금액] 5000원수량:" + mCurrentNormalCarInfo.OutCome5000Qty.ToString() + "  1000원수량:" + mCurrentNormalCarInfo.OutCome1000Qty.ToString() + "  500원수량:" + mCurrentNormalCarInfo.OutCome500Qty.ToString() + "  100원수량:" + mCurrentNormalCarInfo.OutCome100Qty.ToString() + "  50원수량:" + mCurrentNormalCarInfo.OutCome50Qty.ToString());
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
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "결제완료 명령시 입금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCompleteMoney();

                    mCurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할금액: 5000원수량:" + mCurrentNormalCarInfo.Charge5000Qty.ToString() + "  1000원수량:" + mCurrentNormalCarInfo.Charge1000Qty.ToString() + "  500원수량:" + mCurrentNormalCarInfo.Charge500Qty.ToString() + "  100원수량:" + mCurrentNormalCarInfo.Charge100Qty.ToString() + "  50원수량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (mCurrentNormalCarInfo.Charge5000Qty > 0 || mCurrentNormalCarInfo.Charge1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (mCurrentNormalCarInfo.Charge1000Qty * 2) + (mCurrentNormalCarInfo.Charge5000Qty * 10);
                            mCurrentNormalCarInfo.Charge1000Qty = 0;
                            mCurrentNormalCarInfo.Charge5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "[지폐대신 동전방출로 변경] 방출할500원 수량: " + mCurrentNormalCarInfo.Charge500Qty.ToString());

                        }
                        else
                        {
                            if (mCurrentNormalCarInfo.Charge5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "5000원권 방출명령 내림:" + mCurrentNormalCarInfo.Charge5000Qty.ToString() + "개");
                                int out5000Qty = mCurrentNormalCarInfo.Charge5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillPaymentCompleteDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (mCurrentNormalCarInfo.Charge1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "1000원권 방출명령 내림:" + mCurrentNormalCarInfo.Charge1000Qty.ToString() + "개");
                                int out1000Qty = mCurrentNormalCarInfo.Charge1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillPaymentCompleteDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (mCurrentNormalCarInfo.Charge1000Qty * 2) + (mCurrentNormalCarInfo.Charge5000Qty * 10);
                        mCurrentNormalCarInfo.Charge1000Qty = 0;
                        mCurrentNormalCarInfo.Charge5000Qty = 0;

                    }
                    if (mCurrentNormalCarInfo.ChargeCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할 동전이 없으므로 성공:" + mCurrentNormalCarInfo.ChargeCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        mCurrentNormalCarInfo.NotdisChargeMoney500Qty = mCurrentNormalCarInfo.Charge500Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney100Qty = mCurrentNormalCarInfo.Charge100Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty;
                        mCurrentNormalCarInfo.Charge50Qty = 0;
                        mCurrentNormalCarInfo.Charge100Qty = 0;
                        mCurrentNormalCarInfo.Charge500Qty = 0;

                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.



                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (mCurrentNormalCarInfo.Charge500Qty > 0) // 500원 방출수량이 있다면
                        {
                            mCurrentNormalCarInfo.Charge100Qty = mCurrentNormalCarInfo.Charge100Qty + (mCurrentNormalCarInfo.Charge500Qty * 5); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "500원 장비 미사용 또는 장비고장으로 500원 100원으로 교체:" + mCurrentNormalCarInfo.Charge100Qty.ToString());
                            mCurrentNormalCarInfo.Charge500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < mCurrentNormalCarInfo.Charge500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (mCurrentNormalCarInfo.Charge500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CompletOutChargeAction", "500원권 방출명령 내림:" + mCurrentNormalCarInfo.Charge500Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (mCurrentNormalCarInfo.Charge100Qty > 0)
                        {
                            mCurrentNormalCarInfo.Charge50Qty = mCurrentNormalCarInfo.Charge50Qty + (mCurrentNormalCarInfo.Charge100Qty * 2); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "100원 장비 미사용 또는 장비고장으로 100원 50원으로 교체:" + mCurrentNormalCarInfo.Charge100Qty.ToString());
                            mCurrentNormalCarInfo.Charge100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < mCurrentNormalCarInfo.Charge100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (mCurrentNormalCarInfo.Charge100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CompletOutChargeAction", "100원권 방출명령 내림:" + mCurrentNormalCarInfo.Charge100Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (mCurrentNormalCarInfo.Charge50Qty > 0)
                        {
                            mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty;
                            mCurrentNormalCarInfo.Charge50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }


                    if (cash50SettingQty < mCurrentNormalCarInfo.Charge50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "동전 50원 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty - cash50SettingQty;
                        mCurrentNormalCarInfo.Charge50Qty = cash50SettingQty;
                    }


                    if (mCurrentNormalCarInfo.Charge50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "50원권 방출명령 내림:" + mCurrentNormalCarInfo.Charge50Qty.ToString() + "개");
                        l_resultPayment = CoinOutChargeMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (mCurrentNormalCarInfo.GetNotDisChargeMoney > 0)
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
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "결제완료 명령시 입금액:" + mCurrentNormalCarInfo.GetInComeMoney.ToString());

                    //////////
                    //지폐 방출
                    //////////

                    CheckCurrentOutCompleteMoney();

                    mCurrentNormalCarInfo.ClearDischargeMoney(); // 보관증 금액 초기화
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할금액: 50PHP수량:" + mCurrentNormalCarInfo.Charge5000Qty.ToString() + "  20PHP수량:" + mCurrentNormalCarInfo.Charge1000Qty.ToString() + "  10PHP수량:" + mCurrentNormalCarInfo.Charge500Qty.ToString() + "  5PHP수량:" + mCurrentNormalCarInfo.Charge100Qty.ToString() + "  1PHP수량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        int currentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss").ToString());
                        if (currentTime >= 235800 && (mCurrentNormalCarInfo.Charge5000Qty > 0 || mCurrentNormalCarInfo.Charge1000Qty > 0)) //현재시간이23시58분00초보다 크고 지폐방출할 수량이 있다면
                        {
                            mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (mCurrentNormalCarInfo.Charge1000Qty * 2) + (mCurrentNormalCarInfo.Charge5000Qty * 5);
                            mCurrentNormalCarInfo.Charge1000Qty = 0;
                            mCurrentNormalCarInfo.Charge5000Qty = 0;

                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "[지폐대신 동전방출로 변경] 방출할1PHP 수량: " + mCurrentNormalCarInfo.Charge500Qty.ToString());

                        }
                        else
                        {
                            if (mCurrentNormalCarInfo.Charge5000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "50PHP 방출명령 내림:" + mCurrentNormalCarInfo.Charge5000Qty.ToString() + "개");
                                int out5000Qty = mCurrentNormalCarInfo.Charge5000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref out5000Qty);
                                BillPaymentCompleteDischarge5000(logDate, out5000Qty, _result);
                            }

                            if (mCurrentNormalCarInfo.Charge1000Qty > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormPaymentMenu | CompletOutChargeAction", "20PHP 방출명령 내림:" + mCurrentNormalCarInfo.Charge1000Qty.ToString() + "개");
                                int out1000Qty = mCurrentNormalCarInfo.Charge1000Qty; // 방출해야할 수량
                                Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref out1000Qty);
                                BillPaymentCompleteDischarge1000(logDate, out1000Qty, _result);
                            }
                        }
                    }
                    else
                    {
                        mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (mCurrentNormalCarInfo.Charge1000Qty * 2) + (mCurrentNormalCarInfo.Charge5000Qty * 5);
                        mCurrentNormalCarInfo.Charge1000Qty = 0;
                        mCurrentNormalCarInfo.Charge5000Qty = 0;

                    }
                    if (mCurrentNormalCarInfo.ChargeCoinMoney <= 0)
                    {
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "방출할 동전이 없으므로 성공:" + mCurrentNormalCarInfo.ChargeCoinMoney.ToString());
                        return PaymentResult.Success;
                    }
                    if ((NPSYS.Device.gIsUseCoinDischarger50Device || NPSYS.Device.gIsUseCoinDischarger100Device || NPSYS.Device.gIsUseCoinDischarger500Device) == false)
                    {
                        mCurrentNormalCarInfo.NotdisChargeMoney500Qty = mCurrentNormalCarInfo.Charge500Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney100Qty = mCurrentNormalCarInfo.Charge100Qty;
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty;
                        return PaymentResult.Fail;
                    }

                    int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty)); // 현재 저장된 수량을 가져온다.



                    if (NPSYS.Device.gIsUseCoinDischarger500Device == false || NPSYS.Device.UsingSettingCoinCharger500 == false) // 500원 방출을 할수없을때
                    {
                        if (mCurrentNormalCarInfo.Charge500Qty > 0) // 500원 방출수량이 있다면
                        {
                            mCurrentNormalCarInfo.Charge100Qty = mCurrentNormalCarInfo.Charge100Qty + (mCurrentNormalCarInfo.Charge500Qty * 2); // 부족한 500원 수량을 100원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "10PHP 장비 미사용 또는 장비고장으로 10PHP 5PHP으로 교체:" + mCurrentNormalCarInfo.Charge100Qty.ToString());
                            mCurrentNormalCarInfo.Charge500Qty = 0;
                        }
                    }


                    int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                    if (cash500SettingQty < mCurrentNormalCarInfo.Charge500Qty) // 보유수량보다 500원 방출수량이 많을때
                    {
                        CheckCurrentOutCancelMoney();

                    }



                    PaymentResult l_resultPayment = PaymentResult.Success;
                    if (mCurrentNormalCarInfo.Charge500Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormPaymentMenu | CompletOutChargeAction", "10PHP 방출명령 내림:" + mCurrentNormalCarInfo.Charge500Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin500, logDate, cash500SettingQty, l_resultPayment);

                    }

                    if (NPSYS.Device.gIsUseCoinDischarger100Device == false || NPSYS.Device.UsingSettingCoinCharger100 == false)
                    {
                        if (mCurrentNormalCarInfo.Charge100Qty > 0)
                        {
                            mCurrentNormalCarInfo.Charge50Qty = mCurrentNormalCarInfo.Charge50Qty + (mCurrentNormalCarInfo.Charge100Qty * 5); // 부족한 100원 수량을 50원권 수량에 더함
                            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CompletOutChargeAction", "5PHP 장비 미사용 또는 장비고장으로 5PHP 1PHP원으로 교체:" + mCurrentNormalCarInfo.Charge50Qty.ToString());
                            mCurrentNormalCarInfo.Charge100Qty = 0;
                        }
                    }
                    int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                    if (cash100SettingQty < mCurrentNormalCarInfo.Charge100Qty)
                    {
                        CheckCurrentOutCancelMoney();

                    }

                    if (mCurrentNormalCarInfo.Charge100Qty > 0)
                    {

                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormPaymentMenu | CompletOutChargeAction", "5PHP 방출명령 내림:" + mCurrentNormalCarInfo.Charge100Qty.ToString() + "개");
                        CoinOutChargeMoneyDischarge(MoneyType.Coin100, logDate, cash100SettingQty, l_resultPayment);

                    }


                    if (NPSYS.Device.gIsUseCoinDischarger50Device == false || NPSYS.Device.UsingSettingCoinCharger50 == false)
                    {
                        if (mCurrentNormalCarInfo.Charge50Qty > 0)
                        {
                            mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty;
                            mCurrentNormalCarInfo.Charge50Qty = 0;
                            return PaymentResult.Fail;
                        }
                    }


                    if (cash50SettingQty < mCurrentNormalCarInfo.Charge50Qty)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "1PHP 잔액부족 현재보유량:" + cash50SettingQty.ToString() + "  동전요청량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());
                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty = mCurrentNormalCarInfo.Charge50Qty - cash50SettingQty;
                        mCurrentNormalCarInfo.Charge50Qty = cash50SettingQty;
                    }


                    if (mCurrentNormalCarInfo.Charge50Qty > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormPaymentMenu | CompletOutChargeAction", "1PHP 방출명령 내림:" + mCurrentNormalCarInfo.Charge50Qty.ToString() + "개");
                        l_resultPayment = CoinOutChargeMoneyDischarge(MoneyType.Coin50, logDate, cash50SettingQty, l_resultPayment);
                    }
                    if (mCurrentNormalCarInfo.GetNotDisChargeMoney > 0)
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
                mCurrentNormalCarInfo.LastErrorMessage = "FormPaymenu | CompletOutChargeAction | Exception:" + ex.ToString();
                return PaymentResult.Fail;
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymenu | CompletOutChargeAction", "[방출한금액] 5000원수량:" + mCurrentNormalCarInfo.OutCome5000Qty.ToString() + "  1000원수량:" + mCurrentNormalCarInfo.OutCome1000Qty.ToString() + "  500원수량:" + mCurrentNormalCarInfo.OutCome500Qty.ToString() + "  100원수량:" + mCurrentNormalCarInfo.OutCome100Qty.ToString() + "  50원수량:" + mCurrentNormalCarInfo.OutCome50Qty.ToString());
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
                    output_RequesetCointQty = mCurrentNormalCarInfo.Charge500Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor500;
                    break;
                case MoneyType.Coin100:
                    output_RequesetCointQty = mCurrentNormalCarInfo.Charge100Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor100;
                    break;
                case MoneyType.Coin50:
                    output_RequesetCointQty = mCurrentNormalCarInfo.Charge50Qty;
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
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 500, "");
                        mCurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 500원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 500원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "500원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        mCurrentNormalCarInfo.Charge100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 100원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "500원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름500원권100원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 100원권 방출해야할 수량:" + (mCurrentNormalCarInfo.Charge100Qty).ToString());
                        mCurrentNormalCarInfo.Charge500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 100, "");
                        mCurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 100원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "100원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        mCurrentNormalCarInfo.Charge50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 50원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름100원권50원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 50원권 방출해야할 수량:" + (mCurrentNormalCarInfo.Charge50Qty).ToString());
                        mCurrentNormalCarInfo.Charge100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 50, "");
                        mCurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutChargeMoneyDischarge", "50원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        mCurrentNormalCarInfo.Charge50Qty = 0;

                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty += (output_RequesetCointQty - output_ResponeCointQty); // //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "50원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (mCurrentNormalCarInfo.NotdisChargeMoney50Qty * 50).ToString());

                        break;
                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 10, "");
                        mCurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 10PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 10PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Charge100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 5PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "10PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름10PHP권5PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 5PHP권 방출해야할 수량:" + (mCurrentNormalCarInfo.Charge100Qty).ToString());
                        mCurrentNormalCarInfo.Charge500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 5, "");
                        mCurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 5PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Charge50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 1PHP원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "5PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름5PHP권 1PHP으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 1PHP 방출해야할 수량:" + (mCurrentNormalCarInfo.Charge50Qty).ToString());
                        mCurrentNormalCarInfo.Charge100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 1, "");
                        mCurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Charge50Qty = 0;

                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty += mCurrentNormalCarInfo.NotdisChargeMoney50Qty + (output_RequesetCointQty - output_ResponeCointQty);
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutChargeMoneyDischarge", "1PHP 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (mCurrentNormalCarInfo.NotdisChargeMoney50Qty * 1).ToString());

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
                    output_RequesetCointQty = mCurrentNormalCarInfo.Cancle500Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor500;
                    break;
                case MoneyType.Coin100:
                    output_RequesetCointQty = mCurrentNormalCarInfo.Cancle100Qty;
                    currentCoinDispenser = NPSYS.Device.CoinDispensor100;
                    break;
                case MoneyType.Coin50:
                    output_RequesetCointQty = mCurrentNormalCarInfo.Cancle50Qty;
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
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 500, "");
                        mCurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 500원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 500원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "500원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());

                        mCurrentNormalCarInfo.Cancle100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 100원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "500원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름500원권100원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 100원권 방출해야할 수량:" + (mCurrentNormalCarInfo.Cancle100Qty).ToString());
                        mCurrentNormalCarInfo.Cancle500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 100, "");
                        mCurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 100원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 100원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "100원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        mCurrentNormalCarInfo.Cancle50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 50원에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름100원권50원권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 50원권 방출해야할 수량:" + (mCurrentNormalCarInfo.Cancle50Qty).ToString());
                        mCurrentNormalCarInfo.Cancle100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 50, "");
                        mCurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; // 50원 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 50원 현재보유수량 방출된 금액에 제외하고 저장함
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | CoinOutCancleMoneyDischarge", "50원 보유시제변경" + (pCoinSettingQty - output_ResponeCointQty).ToString());
                        mCurrentNormalCarInfo.Cancle50Qty = 0;

                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty += (output_RequesetCointQty - output_ResponeCointQty); // //시제설정누락처리
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "50원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (mCurrentNormalCarInfo.NotdisChargeMoney50Qty * 50).ToString());

                        break;
                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (pMoneyType)
                {
                    case MoneyType.Coin500:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 10, "");
                        mCurrentNormalCarInfo.OutCome500Qty = output_ResponeCointQty; // 10PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash500SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 10PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Cancle100Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 2); // 미방출건이 있다면 5PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "10PHP권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름10PHP권5PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 2).ToString() + " 5PHP 방출해야할 수량:" + (mCurrentNormalCarInfo.Cancle100Qty).ToString());
                        mCurrentNormalCarInfo.Cancle500Qty = 0;

                        break;
                    case MoneyType.Coin100:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 5, "");
                        mCurrentNormalCarInfo.OutCome100Qty = output_ResponeCointQty; // 5PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash100SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); //5PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Cancle50Qty += +((output_RequesetCointQty - output_ResponeCointQty) * 5); // 미방출건이 있다면 1PHP에 방출에 추가로 더한다
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "100원권 방출된 수량:" + output_ResponeCointQty.ToString() + " 거스름5PHP권1PHP권으로수량변경:" + ((output_RequesetCointQty - output_ResponeCointQty) * 5).ToString() + " 1PHP 방출해야할 수량:" + (mCurrentNormalCarInfo.Cancle50Qty).ToString());
                        mCurrentNormalCarInfo.Cancle100Qty = 0;

                        break;
                    case MoneyType.Coin50:
                        LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, pMoneyType, 0, output_ResponeCointQty * 1, "");
                        mCurrentNormalCarInfo.OutCome50Qty = output_ResponeCointQty; //1PHP 방출금액
                        NPSYS.Config.SetValue(ConfigID.Cash50SettingQty, (pCoinSettingQty - output_ResponeCointQty).ToString()); // 1PHP 현재보유수량 방출된 금액에 제외하고 저장함
                        mCurrentNormalCarInfo.Cancle50Qty = 0;

                        mCurrentNormalCarInfo.NotdisChargeMoney50Qty += mCurrentNormalCarInfo.NotdisChargeMoney50Qty + (output_RequesetCointQty - output_ResponeCointQty);
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CoinOutCancleMoneyDischarge", "1PHP권 방출된 수량:" + output_ResponeCointQty.ToString() + " 보관증 금액:" + (mCurrentNormalCarInfo.NotdisChargeMoney50Qty * 1).ToString());

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
                int outSuccess5000Qty = mCurrentNormalCarInfo.Cancle5000Qty - pNotDischarge5000Qty; //5000원권 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Cancle5000Qty = 0;
                mCurrentNormalCarInfo.OutCome5000Qty = mCurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 5000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillCancleDischarge5000", "5000원 보유시제변경:" + (cash5000SettingQty - outSuccess5000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 5000, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge5000", "5000원권 불출안됨으로 1000원권으로수량변경:" + (pNotDischarge5000Qty * 5).ToString());
                    mCurrentNormalCarInfo.Cancle1000Qty = mCurrentNormalCarInfo.Cancle1000Qty + pNotDischarge5000Qty * 5;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess5000Qty = mCurrentNormalCarInfo.Cancle5000Qty - pNotDischarge5000Qty; //50PHP 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Cancle5000Qty = 0;
                mCurrentNormalCarInfo.OutCome5000Qty = mCurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 50PHP 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 50, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    int php50 = pNotDischarge5000Qty;
                    int php50Fee = php50 * 50;
                    int php20 = php50Fee / 20;
                    int php10 = (php50Fee - (php20 * 20)) / 10;
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge5000", "50PHP 불출안됨으로 20PHP으로수량변경:" + (php20).ToString() + "10PHP으로수량변경:" + php10.ToString());
                    mCurrentNormalCarInfo.Cancle1000Qty = mCurrentNormalCarInfo.Cancle1000Qty + php20;
                    mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + php10;

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
                int outSuccess5000Qty = mCurrentNormalCarInfo.Charge5000Qty - pNotDischarge5000Qty; //5000원권 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Charge5000Qty = 0;
                mCurrentNormalCarInfo.OutCome5000Qty = mCurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 5000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillPaymentCompleteDischarge5000", "5000원 보유시제변경:" + (cash5000SettingQty - outSuccess5000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 5000, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge5000", "5000원권 불출안됨으로 1000원권으로수량변경:" + (pNotDischarge5000Qty * 5).ToString());
                    mCurrentNormalCarInfo.Charge1000Qty = mCurrentNormalCarInfo.Charge1000Qty + pNotDischarge5000Qty * 5;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess5000Qty = mCurrentNormalCarInfo.Charge5000Qty - pNotDischarge5000Qty; //50PHP 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Charge5000Qty = 0;
                mCurrentNormalCarInfo.OutCome5000Qty = mCurrentNormalCarInfo.OutCome5000Qty + outSuccess5000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty)); // 시제 50PHP 수량
                NPSYS.Config.SetValue(ConfigID.Cash5000SettingQty, (cash5000SettingQty - outSuccess5000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill5000, 0, outSuccess5000Qty * 50, "취소에 의한 반환");
                if (pNotDischarge5000Qty > 0)
                {
                    int php50 = pNotDischarge5000Qty;
                    int php50Fee = php50 * 50;
                    int php20 = php50Fee / 20;
                    int php10 = (php50Fee - (php20 * 20)) / 10;
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge5000", "50PHP 불출안됨으로 20PHP권으로수량변경:" + php20.ToString() + "10PHP권으로수량변경:" + php10.ToString());
                    mCurrentNormalCarInfo.Charge1000Qty = mCurrentNormalCarInfo.Charge1000Qty + php20;
                    mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + php10;

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
                int outSuccess1000Qty = mCurrentNormalCarInfo.Cancle1000Qty - pNotDischarge1000Qty; //1000원권 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Cancle1000Qty = 0;
                mCurrentNormalCarInfo.OutCome1000Qty = mCurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 1000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillCancleDischarge1000", "1000원 보유시제변경:" + (cash1000SettingQty - outSuccess1000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 1000, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge1000", "1000원권 불출안됨으로 500원권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (pNotDischarge1000Qty * 2);

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess1000Qty = mCurrentNormalCarInfo.Cancle1000Qty - pNotDischarge1000Qty; //20PHP 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Cancle1000Qty = 0;
                mCurrentNormalCarInfo.OutCome1000Qty = mCurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 20PHP권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 20, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillDischarge1000", "20PHP 불출안됨으로 10PHP권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (pNotDischarge1000Qty * 2);

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
                int outSuccess1000Qty = mCurrentNormalCarInfo.Charge1000Qty - pNotDischarge1000Qty; //1000원권 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Charge1000Qty = 0;
                mCurrentNormalCarInfo.OutCome1000Qty = mCurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 1000원권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BillPaymentCompleteDischarge1000", "1000원 보유시제변경:" + (cash1000SettingQty - outSuccess1000Qty).ToString());
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 1000, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge1000", "1000원권 불출안됨으로 500원권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + pNotDischarge1000Qty * 2;

                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                int outSuccess1000Qty = mCurrentNormalCarInfo.Charge1000Qty - pNotDischarge1000Qty; //20PHP 배출 성공한수량을 저장
                mCurrentNormalCarInfo.Charge1000Qty = 0;
                mCurrentNormalCarInfo.OutCome1000Qty = mCurrentNormalCarInfo.OutCome1000Qty + outSuccess1000Qty; // 현재 방출수량을 OUTCOME 변수에 넣음
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty)); // 시제 20PHP권 수량
                NPSYS.Config.SetValue(ConfigID.Cash1000SettingQty, (cash1000SettingQty - outSuccess1000Qty).ToString()); // 시제 변경
                LPRDbSelect.LogMoney(PaymentType.Cash, logDate, mCurrentNormalCarInfo, MoneyType.Bill1000, 0, outSuccess1000Qty * 20, "취소에 의한 반환");
                if (pNotDischarge1000Qty > 0)
                {
                    TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | BillPaymentCompleteDischarge1000", "20PHP 불출안됨으로 10PHP권으로수량변경:" + (pNotDischarge1000Qty * 2).ToString());
                    mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + pNotDischarge1000Qty * 2;

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

                if ((cash5000SettingQty >= mCurrentNormalCarInfo.Charge5000Qty) &&
                    (cash1000SettingQty >= mCurrentNormalCarInfo.Charge1000Qty) &&
                    (cash500SettingQty >= mCurrentNormalCarInfo.Charge500Qty) &&
                    (cash100SettingQty >= mCurrentNormalCarInfo.Charge100Qty) &&
                    (cash50SettingQty >= mCurrentNormalCarInfo.Charge50Qty))
                {
                    return true;
                }
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    if (cash5000SettingQty < mCurrentNormalCarInfo.Charge5000Qty)  // 5000원권이 부죽한 경우 1000원권으로 넘김
                    {
                        int lack5000Qty = mCurrentNormalCarInfo.Charge5000Qty - cash5000SettingQty;  // 5000원권의 부족한 수량

                        mCurrentNormalCarInfo.Charge5000Qty = cash5000SettingQty; // 현재 남아있는 5000원권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Charge1000Qty = mCurrentNormalCarInfo.Charge1000Qty + (lack5000Qty * 5); // 부족한 5000원 수량을 1000원권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:5000원권 부족으로 1000원권으로 동전교환-현보유1000수량:" + cash5000SettingQty.ToString() + " 1000원수량:" + mCurrentNormalCarInfo.Charge1000Qty.ToString());

                    }

                    if (cash1000SettingQty < mCurrentNormalCarInfo.Charge1000Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                    {
                        int lack1000Qty = mCurrentNormalCarInfo.Charge1000Qty - cash1000SettingQty; // 1000원권의 부족한 수량

                        mCurrentNormalCarInfo.Charge1000Qty = cash1000SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (lack1000Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:1000원권 부족으로 500원권으로 동전교환-현보유1000수량:" + cash1000SettingQty.ToString() + " 500원수량:" + mCurrentNormalCarInfo.Charge500Qty.ToString());

                    }

                    if (cash500SettingQty < mCurrentNormalCarInfo.Charge500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = mCurrentNormalCarInfo.Charge500Qty - cash500SettingQty; // 500원권의 부족한 수량
                        mCurrentNormalCarInfo.Charge500Qty = cash500SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Charge100Qty = mCurrentNormalCarInfo.Charge100Qty + (lack500Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:500원권 부족으로 100원권으로 동전교환-현보유500수량:" + cash500SettingQty.ToString() + " 100원수량:" + mCurrentNormalCarInfo.Charge100Qty.ToString());
                    }

                    if (cash100SettingQty < mCurrentNormalCarInfo.Charge100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = mCurrentNormalCarInfo.Charge100Qty - cash100SettingQty; // 1000원권의 부족한 수량
                        mCurrentNormalCarInfo.Charge100Qty = cash100SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Charge50Qty = mCurrentNormalCarInfo.Charge50Qty + (lack100Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:100원권 부족으로 50원권으로 동전교환-현보유100수량:" + cash100SettingQty.ToString() + " 50원수량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());

                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    if (cash5000SettingQty < mCurrentNormalCarInfo.Charge5000Qty)  // 50PHP권이 부죽한 경우 20PHP원권으로 넘김
                    {
                        int lack5000Qty = mCurrentNormalCarInfo.Charge5000Qty - cash5000SettingQty;  // 50PHP권의 부족한 수량
                        int php50 = lack5000Qty;
                        int php50Fee = php50 * 50;
                        int php20 = php50Fee / 20;
                        int php10 = (php50Fee - (php20 * 20)) / 10;
                        mCurrentNormalCarInfo.Charge5000Qty = cash5000SettingQty; // 현재 남아있는 50PHP 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Charge1000Qty = mCurrentNormalCarInfo.Charge1000Qty + (php20); // 부족한 50PHP 수량을 20PHP 수량에 더함
                        mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (php10); // 부족한 50PHP원 수량을 10PHP권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:50PHP 부족으로 20PHP권으로 동전교환-현보유50PHP수량:" + cash5000SettingQty.ToString() + " 20PHP수량:" + mCurrentNormalCarInfo.Charge1000Qty.ToString());

                    }

                    if (cash1000SettingQty < mCurrentNormalCarInfo.Charge1000Qty) // 20PHP원권이 부족한 경우 10PHP원 으로 넘김
                    {
                        int lack1000Qty = mCurrentNormalCarInfo.Charge1000Qty - cash1000SettingQty; // 20PHP원권의 부족한 수량

                        mCurrentNormalCarInfo.Charge1000Qty = cash1000SettingQty; // 현재 남아있는 20PHP권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Charge500Qty = mCurrentNormalCarInfo.Charge500Qty + (lack1000Qty * 2); // 부족한 20PHP 수량을 10PHP원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 지폐부족:20PHP원권 부족으로 10PHP권으로 동전교환-현보유20PHP수량:" + cash1000SettingQty.ToString() + " 10PHP수량:" + mCurrentNormalCarInfo.Charge500Qty.ToString());

                    }

                    if (cash500SettingQty < mCurrentNormalCarInfo.Charge500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = mCurrentNormalCarInfo.Charge500Qty - cash500SettingQty; // 10PHP 부족한 수량
                        mCurrentNormalCarInfo.Charge500Qty = cash500SettingQty; // 현재 남아있는 10PHP 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Charge100Qty = mCurrentNormalCarInfo.Charge100Qty + (lack500Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 동전부족:10PHP 부족으로 5PHP권으로 동전교환-현보유10PHP수량:" + cash500SettingQty.ToString() + " 5PHP수량:" + mCurrentNormalCarInfo.Charge100Qty.ToString());
                    }

                    if (cash100SettingQty < mCurrentNormalCarInfo.Charge100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = mCurrentNormalCarInfo.Charge100Qty - cash100SettingQty; // 5PHP원권의 부족한 수량
                        mCurrentNormalCarInfo.Charge100Qty = cash100SettingQty; // 현재 남아있는 5PHP권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Charge50Qty = mCurrentNormalCarInfo.Charge50Qty + (lack100Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCompleteMoney", "거스름돈 반환시 동전부족:5PHP권 부족으로 1PHP원권으로 동전교환-현보유5PHP수량:" + cash100SettingQty.ToString() + " 1PHP수량:" + mCurrentNormalCarInfo.Charge50Qty.ToString());

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

                if ((cash5000SettingQty >= mCurrentNormalCarInfo.Cancle5000Qty) &&
                    (cash1000SettingQty >= mCurrentNormalCarInfo.Cancle1000Qty) &&
                    (cash500SettingQty >= mCurrentNormalCarInfo.Cancle500Qty) &&
                    (cash100SettingQty >= mCurrentNormalCarInfo.Cancle100Qty) &&
                    (cash50SettingQty >= mCurrentNormalCarInfo.Cancle50Qty))
                {
                    return true;
                }
                if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
                {
                    if (cash5000SettingQty < mCurrentNormalCarInfo.Cancle5000Qty)  // 5000원권이 부죽한 경우 1000원권으로 넘김
                    {
                        int lack5000Qty = mCurrentNormalCarInfo.Cancle5000Qty - cash5000SettingQty;  // 5000원권의 부족한 수량

                        mCurrentNormalCarInfo.Cancle5000Qty = cash5000SettingQty; // 현재 남아있는 5000원권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Cancle1000Qty = mCurrentNormalCarInfo.Cancle1000Qty + (lack5000Qty * 5); // 부족한 5000원 수량을 1000원권 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:5000원권 부족으로 1000원권으로 동전교환-현보유1000수량:" + cash5000SettingQty.ToString() + " 1000원수량:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString());

                    }

                    if (cash1000SettingQty < mCurrentNormalCarInfo.Cancle1000Qty) // 1000원권이 부족한 경우 500원 으로 넘김
                    {
                        int lack1000Qty = mCurrentNormalCarInfo.Cancle1000Qty - cash1000SettingQty; // 1000원권의 부족한 수량

                        mCurrentNormalCarInfo.Cancle1000Qty = cash1000SettingQty; // 현재 남아있는 1000원권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (lack1000Qty * 2); // 부족한 1000원 수량을 500원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:1000원권 부족으로 500원권으로 동전교환-현보유1000수량:" + cash1000SettingQty.ToString() + " 500원수량:" + mCurrentNormalCarInfo.Cancle500Qty.ToString());

                    }

                    if (cash500SettingQty < mCurrentNormalCarInfo.Cancle500Qty) // 500원 권에 대해서만 처리, 100원, 50원은 코인보드에서 자동으로 처리하기 때문에 안된다고 봄
                    {
                        int lack500Qty = mCurrentNormalCarInfo.Cancle500Qty - cash500SettingQty; // 500원권의 부족한 수량
                        mCurrentNormalCarInfo.Cancle500Qty = cash500SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Cancle100Qty = mCurrentNormalCarInfo.Cancle100Qty + (lack500Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:500원권 부족으로 100원권으로 동전교환-현보유500수량:" + cash500SettingQty.ToString() + " 100원수량:" + mCurrentNormalCarInfo.Cancle100Qty.ToString());
                    }

                    if (cash100SettingQty < mCurrentNormalCarInfo.Cancle100Qty) // 100원 권에 대해서만 처리
                    {
                        int lack100Qty = mCurrentNormalCarInfo.Cancle100Qty - cash100SettingQty; // 1000원권의 부족한 수량
                        mCurrentNormalCarInfo.Cancle100Qty = cash100SettingQty; // 현재 남아있는 500원권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Cancle50Qty = mCurrentNormalCarInfo.Cancle50Qty + (lack100Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:100원권 부족으로 50원권으로 동전교환-현보유100수량:" + cash100SettingQty.ToString() + " 50원수량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());

                    }
                }
                else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
                {
                    if (cash5000SettingQty < mCurrentNormalCarInfo.Cancle5000Qty)  // 50PHP권이 부죽한 경우 20PHP으로 넘김
                    {
                        int lack5000Qty = mCurrentNormalCarInfo.Cancle5000Qty - cash5000SettingQty;  // 50PHP권의 부족한 수량
                        int php50 = lack5000Qty;
                        int php50Fee = php50 * 50;
                        int php20 = php50Fee / 20;
                        int php10 = (php50Fee - (php20 * 20)) / 10;
                        mCurrentNormalCarInfo.Cancle5000Qty = cash5000SettingQty; // 현재 남아있는 50PHP권 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Cancle1000Qty = mCurrentNormalCarInfo.Cancle1000Qty + (php20); // 부족한 5000원 수량을 1000원권 수량에 더함
                        mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + php10;
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:50PHP 부족으로 20PHP로 교환 현보유50PHP수량:" + cash5000SettingQty.ToString() + " 20PHP수량:" + mCurrentNormalCarInfo.Cancle1000Qty.ToString());

                    }

                    if (cash1000SettingQty < mCurrentNormalCarInfo.Cancle1000Qty) // 20PHP권이 부족한 경우 10PHP원 으로 넘김
                    {
                        int lack1000Qty = mCurrentNormalCarInfo.Cancle1000Qty - cash1000SettingQty; // 20PHP의 부족한 수량

                        mCurrentNormalCarInfo.Cancle1000Qty = cash1000SettingQty; // 현재 남아있는 20PHP 수량을 거스름돈으로 대체
                        mCurrentNormalCarInfo.Cancle500Qty = mCurrentNormalCarInfo.Cancle500Qty + (lack1000Qty * 2); // 부족한 20PHP 수량을 10PHP원 수량에 더함
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:20PHP원권 부족으로 10PHP원권으로 동전교환-현보유20PHP수량:" + cash1000SettingQty.ToString() + " 10PHP원수량:" + mCurrentNormalCarInfo.Cancle500Qty.ToString());

                    }

                    if (cash500SettingQty < mCurrentNormalCarInfo.Cancle500Qty) // 10PHP원 권에 대해서만 처리, 
                    {
                        int lack500Qty = mCurrentNormalCarInfo.Cancle500Qty - cash500SettingQty; // 10PHP원권의 부족한 수량
                        mCurrentNormalCarInfo.Cancle500Qty = cash500SettingQty; // 현재 남아있는 10PHP권 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Cancle100Qty = mCurrentNormalCarInfo.Cancle100Qty + (lack500Qty * 2); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 동전부족:10PHP 부족으로 5PHP 동전교환-현보유10PHP:" + cash500SettingQty.ToString() + " 5PHP:" + mCurrentNormalCarInfo.Cancle100Qty.ToString());
                    }

                    if (cash100SettingQty < mCurrentNormalCarInfo.Cancle100Qty) // 5PHP원 권에 대해서만 처리
                    {
                        int lack100Qty = mCurrentNormalCarInfo.Cancle100Qty - cash100SettingQty; // 5PHP원권의 부족한 수량
                        mCurrentNormalCarInfo.Cancle100Qty = cash100SettingQty; // 현재 남아있는 5PHP 수량을 거스름돈으로 대체                        
                        mCurrentNormalCarInfo.Cancle50Qty = mCurrentNormalCarInfo.Cancle50Qty + (lack100Qty * 5); // 부족한 수량을 거스름돈으로 대체
                        TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | CheckCurrentOutCancelMoney", "거스름돈 반환시 지폐부족:5PHP원권 부족으로 1PHP원권으로 동전교환-현보유5PHP:" + cash100SettingQty.ToString() + " 5PHP수량:" + mCurrentNormalCarInfo.Cancle50Qty.ToString());

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


        #endregion

        #region 카드리더기 동작관련

        /// <summary>
        /// 스마트로 추가로 이함수 모두 변경
        /// </summary>
        private void StartTIcketCardRead()
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
                if (mCurrentNormalCarInfo.Current_Money > 0 || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess
                   || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || mCurrentNormalCarInfo.VanAmt > 0)
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
                    NPSYS.Device.CardDevice2.TIcketFrontEject();

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
                        StartTIcketCardRead();
                        TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", _TIcketStatus.Message);
                        // CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CA2, _TIcketStatus.ReultIntMessage); // 희주주석

                        return;

                    }

                }

                Result _result = NPSYS.Device.CardDevice2.readingTicketCardStart();
                Result l_ReadingTrackdata = NPSYS.Device.CardDevice2.readingTicketCardEnd();


                timer_CardReader2.Stop();
                paymentControl.ErrorMessage = string.Empty;

                if (l_ReadingTrackdata.ReultIntMessage != (int)TicketCardDevice.TicketAndCardResult.No_Return_Sensor_Ticket && l_ReadingTrackdata.ReultIntMessage != (int)TicketCardDevice.TicketAndCardResult.Empy
                    && (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season))
                {
                    lTicketActionResult = NPSYS.Device.CardDevice2.TIcketFrontEject();
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
                        Payment paymentAfterDisocunt = mHttpProcess.Discount(mCurrentNormalCarInfo, DcDetail.DIscountTicketType.MI, l_ReadingTrackdata.Message);
                        if (paymentAfterDisocunt.status.Success) // 정상적인 티켓이라면
                        {
                            int prepayment = mCurrentNormalCarInfo.PaymentMoney;

                            mCurrentNormalCarInfo.ParkingMin = paymentAfterDisocunt.parkingMin;
                            mCurrentNormalCarInfo.TotFee = Convert.ToInt32(paymentAfterDisocunt.totFee);
                            mCurrentNormalCarInfo.TotDc = Convert.ToInt32(paymentAfterDisocunt.totDc);
                            mCurrentNormalCarInfo.Change = Convert.ToInt32(paymentAfterDisocunt.change); //시제설정누락처리
                            mCurrentNormalCarInfo.RecvAmt = Convert.ToInt32(paymentAfterDisocunt.recvAmt); //시제설정누락처리
                            mCurrentNormalCarInfo.DcCnt = paymentAfterDisocunt.dcCnt;
                            mCurrentNormalCarInfo.RealFee = Convert.ToInt32(paymentAfterDisocunt.realFee);
                            //할인권 입수수량 표출
                            paymentControl.DiscountInputCount = (Convert.ToInt32(paymentControl.DiscountInputCount) + 1).ToString();
                            //할인권 입수수량 표출 주석완료

                            //요금할인권처리
                            paymentControl.ParkingFee = TextCore.ToCommaString(mCurrentNormalCarInfo.TotFee.ToString());
                            paymentControl.RecvMoney = TextCore.ToCommaString((mCurrentNormalCarInfo.RecvAmt - mCurrentNormalCarInfo.Change).ToString()); //시제설정누락처리

                            TextCore.INFO(TextCore.INFOS.DISCOUNTTICEKT_SUCCESS, "FormPaymentMenu|timer_CardReader2_Tick", "결제성공");
                            lTicketActionResult = NPSYS.Device.CardDevice2.TIcketBackEject();
                            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormPaymentMenu|timer_CardReader2_Tick", "티켓 뒤로배출:" + lTicketActionResult.ToString());
                            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);


                            if (prepayment == mCurrentNormalCarInfo.PaymentMoney)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|timer_CardReader1_Tick", "할인에 성공했지만 할인금액이 없음");
                                return;
                            }

                            BeforeChangePayValueAsCardReader();

                            if (mCurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                mCurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountCard;
                                //카드실패전송완료
                                PaymentComplete();

                                // db에 저장
                                // 영수증 출력화면
                                return;
                            }
                            else
                            {
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
                                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoAddDiscountTIcket);
                                return;


                            }
                            else if (paymentAfterDisocunt.status.currentStatus == Status.BodyStatus.Discount_PreUsed)
                            {
                                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.DuplicateDiscountTicket);
                                return;
                            }
                            else
                            {
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
                if (NPSYS.Device.gIsUseMagneticReaderDevice && mCurrentNormalCarInfo.PaymentMoney > 0)
                {

                    timer_CardReader2.Start();
                }
            }

        }

        #endregion

        private void Back()
        {
            this.Close();
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

        private void btn_home_Click(object sender, EventArgs e)
        {
            try
            {

                NPSYS.buttonSoundDingDong();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | btn_home_Click", "고객이 홈버튼 누름");
                if (mCurrentNormalCarInfo.ListDcDetail.Count > 0)
                {
                    foreach (DcDetail detailDcDetail in mCurrentNormalCarInfo.ListDcDetail)
                    {
                        if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                        {
                            mHttpProcess.DiscountCancle(mCurrentNormalCarInfo, detailDcDetail);
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

        private int inputtime = NPSYS.SettingInputTimeValue;
        private void inputTimer_Tick(object sender, EventArgs e)
        {
            if (inputtime < 0)
            {
                inputtime = 50000000;
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | inputTimer_Tick", "시간이지나서 처음으로 돌아감");
                CashCancleFormCloseAction(false);
                if (mCurrentNormalCarInfo.ListDcDetail.Count > 0)
                {
                    foreach (DcDetail detailDcDetail in mCurrentNormalCarInfo.ListDcDetail)
                    {
                        if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                        {
                            mHttpProcess.DiscountCancle(mCurrentNormalCarInfo, detailDcDetail);
                            detailDcDetail.UseYn = false;
                        }
                    }
                }
                mCurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
                EventExitPayForm(mCurrentFormType);
                return;

            }
            inputtime = inputtime - 3000;
        }

        int paymentInputTimer = (NPSYS.PaymentInsertInfinite == true ? 12000000 : NPSYS.SettingInputTimeValue);
        private void stopVideo()
        {
            try
            {

                axWindowsMediaPlayer1.Ctlcontrols.stop();

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | playVideo", "예외사항:" + ex.ToString());
            }
        }
        private void PausePlayVideo()
        {
            try
            {
                //if (mIsPlayerOkStatus == false)
                //{
                //    return;
                //}

                inputTimer.Enabled = false;
                inputtime = paymentInputTimer;
                MovieTimer.Enabled = false;
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                IsNextFormPlaying = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StartPlayVideo", "예외사항:" + ex.ToString());
            }
        }

        private void StartPlayVideo()
        {
            try
            {
                //if (mIsPlayerOkStatus == true)
                //{
                axWindowsMediaPlayer1.Ctlcontrols.play();
                //}

                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                IsNextFormPlaying = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|StartPlayVideo", "예외사항:" + ex.ToString());
            }
        }

        int MovieStopPlay = -1000;
        bool IsNextFormPlaying = false;
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




        private void button1_Click_1(object sender, EventArgs e)
        {
            InsertMoney("100QTY");
            if (mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                //카드실패전송
                mCurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                //카드실패전송완료
                PaymentComplete();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            EventExitPayForm(mCurrentFormType);

        }

        private void button2_Click(object sender, EventArgs e)
        {


            m_PayCardandCash.CreditCardPayResult(txtCardInfo.Text, mCurrentNormalCarInfo);
            if (mCurrentNormalCarInfo.PaymentMoney == 0)
            {
                //카드실패전송
                mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                //카드실패전송완료
                PaymentComplete();
            }
        }






        private void btn_PrePage_Click(object sender, EventArgs e)
        {


            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | btn_PrePage_Click", "이전 버튼 클릭");
            if (mCurrentNormalCarInfo.Current_Money > 0)
            {
                //CashCancleAction();
            }
            if (mCurrentNormalCarInfo.ListDcDetail.Count > 0)
            {
                foreach (DcDetail detailDcDetail in mCurrentNormalCarInfo.ListDcDetail)
                {
                    if (detailDcDetail.UseYn == true && detailDcDetail.currentDiscountTicketType == DcDetail.DIscountTicketType.BR)
                    {
                        mHttpProcess.DiscountCancle(mCurrentNormalCarInfo, detailDcDetail);
                        detailDcDetail.UseYn = false;
                    }
                }
            }
            mCurrentNormalCarInfo.ListDcDetail = new List<DcDetail>();
            EventExitPayForm(mCurrentFormType, NPSYS.FormType.Select);
        }

        private void btn_home_Click_1(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (mCurrentNormalCarInfo.Current_Money > 0)
            {
                //CashCancleAction();
            }

            TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | btn_MainPage_Click", "메인으로 이동버튼 클릭");
            EventExitPayForm(mCurrentFormType);


        }




        private void btnDiscountButton_Click(object sender, EventArgs e)
        {
            PausePlayVideo();
            playVideo(m_JuminDIscountMovie);


        }






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
            mCurrentNormalCarInfo.TotFee = 1004;
            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
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

        #region 바코드 처리
        //바코드할인 리스트로변경

        private void timerBarcode_Tick(object sender, EventArgs e)
        {
            if (NPSYS.CurrentFormType != mCurrentFormType)
            {
                return;
            }
            if (mCurrentNormalCarInfo.Current_Money > 0)
            {
                return;
            }
            if (mCurrentNormalCarInfo.PaymentMoney == 0)
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

                    if (mCurrentNormalCarInfo.PaymentMoney != 0)
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

                    if (mCurrentNormalCarInfo.PaymentMoney != 0)
                    {
                        timerBarcode.Start();
                    }

                }
            }
            //바코드모터드리블 사용완료
        }

        void BarcodeSerials_EventBarcode(object sender, string pBarcodeData)
        {
            if (this.Visible == false)
            {
                return;
            }
            mListBarcodeData.Add(pBarcodeData);
        }

        //바코드모터드리블 사용
        void BarcodeMotorSerials_EventBarcode(BarcodeMoter.BarcodeMotorResult pBarcodeMotorResult)
        {
            if (mCurrentFormType != NPSYS.CurrentFormType)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeMotorSerials_EventBarcode", "현재 요금폼이 아니라 바코드 처리안함");
                if (mListBarcodeData != null && mListBarcodeData.Count > 0)
                {
                    mListBarcodeData.Clear();
                }
                return;
            }
            mListBarcodeMotorData.Add(pBarcodeMotorResult);
        }



        private void BarcodeAction(string pBarcodeData)
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", "[바코드정보 처리] " + pBarcodeData);
            paymentControl.ErrorMessage = string.Empty;
            if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoRegExtensDiscount);
                return;
            }
            DcDetail precurrentDcdeatil = mCurrentNormalCarInfo.ListDcDetail.Find(x => x.DcTkno == pBarcodeData);
            if (precurrentDcdeatil != null && precurrentDcdeatil.DcTkno == pBarcodeData)
            {
                paymentControl.ErrorMessage = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_DUPLICATEBARCODE.ToString());
                return;
            }
            if (true) // true
            {

                int pPrePayMoney = mCurrentNormalCarInfo.PaymentMoney;
                //DIscountTicketOcs.TIcketReadingResult resultDiscount = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.Barcode, pBarcodeData, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                Payment paymentAfterDisocunt = mHttpProcess.Discount(mCurrentNormalCarInfo, DcDetail.DIscountTicketType.BR, pBarcodeData);

                if (paymentAfterDisocunt.status.Success) // 정상적인 티켓이라면
                {

                    DcDetail dcDetail = new DcDetail();
                    dcDetail.currentDiscountTicketType = DcDetail.DIscountTicketType.BR;
                    dcDetail.DcTkno = pBarcodeData;
                    dcDetail.UseYn = true;
                    mCurrentNormalCarInfo.ListDcDetail.Add(dcDetail);
                    mCurrentNormalCarInfo.ParkingMin = paymentAfterDisocunt.parkingMin;
                    mCurrentNormalCarInfo.TotFee = Convert.ToInt32(paymentAfterDisocunt.totFee);
                    mCurrentNormalCarInfo.TotDc = Convert.ToInt32(paymentAfterDisocunt.totDc);
                    mCurrentNormalCarInfo.Change = Convert.ToInt32(paymentAfterDisocunt.change);
                    mCurrentNormalCarInfo.RecvAmt = Convert.ToInt32(paymentAfterDisocunt.recvAmt); //시제설정누락처리

                    mCurrentNormalCarInfo.DcCnt = paymentAfterDisocunt.dcCnt;
                    mCurrentNormalCarInfo.RealFee = Convert.ToInt32(paymentAfterDisocunt.realFee);

                    if (pPrePayMoney == mCurrentNormalCarInfo.PaymentMoney)
                    {
                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인에 성공했지만 할인금액이 없음");

                    }
                    else
                    {
                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인성공");
                    }
                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                    {
                        BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectRear();
                        if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                        {
                            ejectResult = NPSYS.Device.BarcodeMoter.EjectRear();
                            if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                            {

                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 성공");
                            }
                            else
                            {
                                paymentControl.ErrorMessage = "바코드가 뒤로 배출되지 않습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 실패");
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 성공");
                        }
                    }
                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);



                    if (pPrePayMoney > mCurrentNormalCarInfo.PaymentMoney) // 현재 할인되서 금액이 할인됬다면
                    {
                        BeforeChangePayValueAsCardReader();
                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountBarcode;
                            //카드실패전송완료
                            PaymentComplete();
                            return;

                        }
                        ChangePayValueAsCardReader();
                    }

                }
                else
                {

                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
                    {

                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인실패 에러원인:" + paymentAfterDisocunt.status.code + " 설명:" + paymentAfterDisocunt.status.message);

                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        return;

                    }

                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                    {
                        BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                        if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                        {
                            ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                            if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                            {

                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 성공");
                            }
                            else
                            {
                                paymentControl.ErrorMessage = "바코드가 앞으로 배출되지 않습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 실패");
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드가 앞으로 배출성공.");
                        }
                    }
                }
            }
            else
            {
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                {
                    BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                    if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                    {
                        ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                        if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                        {

                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 성공");
                        }
                        else
                        {
                            paymentControl.ErrorMessage = "바코드가 앞으로 배출되지 않습니다.";
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 실패");
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드가 앞으로 배출성공.");
                    }
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", "현재 할인권은 " + paymentControl.ErrorMessage);
            }
        }

        //바코드할인 리스트로변경 주석처리
        //바코드모터드리블 사용완료
        #endregion

        private int GoConfigSequence = 0;
        private void panel_ConfigClick1_Click(object sender, EventArgs e)
        {

            GoConfigSequence = 1;
            NPSYS.buttonSoundDingDong();




        }

        private void panel_ConfigClick2_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (GoConfigSequence == 1)
            {
                EventExitPayForm(mCurrentFormType);
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");

            }
            else
            {
                GoConfigSequence = 0;

            }

        }

        private void btnBarcodeTestDiscount_Click(object sender, EventArgs e)
        {
            BarcodeAction(txtTestBarcodeDiscount.Text);
        }

        // 신분증인식기 적용
        private void SinbunProcess(SinBunReader.CardInfo info)
        {
            try
            {

                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "카드정보 들어옴 " + SinBunReader.CardInfo.GetCardTypeText(info.CardType));
                if (info.DiscountType == SinBunReader.DiscountType.BoHun || info.DiscountType == SinBunReader.DiscountType.BokJi)
                {
                    // 보훈카드는 8자리, 10자리 보훈번호가 존재하여 체크
                    if (info.DiscountType == SinBunReader.DiscountType.BoHun && !(info.BohunNum.Length == 8 || info.BohunNum.Length == 10))
                    {
                        return;
                    }

                    string type = ((int)info.DiscountType).ToString();              // 할인타입
                    string grade = info.Grade;                                      // 장애/보훈 등급
                    //string dcNo = LPRDbSelect.GetReductionDiscount(type, grade);    // 할인코드
                    //string feeNo = mDIscountTicketOcs.GetChangeFeeNo(dcNo);
                    //DIscountTicketOcs.TIcketReadingResult l_TIcketReadingResult = DIscountTicketOcs.TIcketReadingResult.NotTicket;
                    //TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "감면대상 확인 감면할인코드 : " + dcNo + " 변경요금코드 : " + feeNo);

                    //if (!string.IsNullOrEmpty(dcNo))
                    //{
                    //    if (mNormalCarInfo.ListDcDetail.Count > 0)
                    //    {
                    //        List<DcDetail> SortDiscount = new List<DcDetail>(mNormalCarInfo.ListDcDetail);

                    //        // 요금변경할인 우선 처리를 위해 먼저 넣어줌 순서 요금변경 -> 시간 -> 퍼센트(나머지)
                    //        SortDiscount = mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType == "0");
                    //        // 이후 시간할인권 처리를 위해 넣어줌
                    //        SortDiscount.AddRange(mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType == "1"));
                    //        // 나머지 할인권을 넣는다.
                    //        SortDiscount.AddRange(mNormalCarInfo.ListDcDetail.FindAll(x => x.DCType != "0" && x.DCType != "1"));
                    //        // 들어온 감면할인이 요금변경일 경우
                    //        if (feeNo != "0")
                    //        {
                    //            string preDcNo = string.Empty;
                    //            string preFeeNo = string.Empty;

                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                preFeeNo = mDIscountTicketOcs.GetChangeFeeNo(dcInfo.DcNo);

                    //                if (preFeeNo != "0" && preFeeNo != feeNo)
                    //                {
                    //                    preDcNo = dcInfo.DcNo;

                    //                    int parkMoney = FeeAction.FeeCalcMoney(Convert.ToInt32(NPSYS.ParkCode), Convert.ToInt32(feeNo), mNormalCarInfo.InYMD, mNormalCarInfo.InHMS, mNormalCarInfo.OutYmd, mNormalCarInfo.OutHms);
                    //                    if (parkMoney < mNormalCarInfo.ParkMoney)
                    //                    {
                    //                        SortDiscount.Remove(SortDiscount.Find(x => x.DcNo == preDcNo));
                    //                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "이전 변경요금제보다 할인율이 높으므로 요금변경처리 이전 요금변경 할인코드 : " + preDcNo);
                    //                        break;
                    //                    }
                    //                    else
                    //                    {
                    //                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "이전 할인요금제보다 할인율이 낮으므로 처리안함");
                    //                        return;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "기존 할인권 정리 후 감면할인 적용");
                    //        //모든할인 취소 처리
                    //        //할인권 입수수량 표출
                    //        lblDiscountInputCount.Text = "0";
                    //        //할인권 입수수량 표출 주석완료

                    //        //모든 할인처리 리셋
                    //        mNormalCarInfo.DiscountMoney = 0;
                    //        mNormalCarInfo.TotalDiscountTime = 0;
                    //        mNormalCarInfo.SumPreTotalDisocuntTime = 0;
                    //        mNormalCarInfo.ListDcDetail.Clear();
                    //        SaleTicketUsePermissions.clear();

                    //        //감면할인 포함하여 할인 재적용
                    //        if (feeNo != "0")
                    //        {
                    //            l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                DIscountTicketOcs.DIscountTicketType currentTicketType = (DIscountTicketOcs.DIscountTicketType)Enum.Parse(typeof(DIscountTicketOcs.DIscountTicketType), dcInfo.Reserve4);
                    //                l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(currentTicketType, dcInfo.DcTkNO, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);

                    //                if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //                {
                    //                    //할인권 입수수량 표출
                    //                    lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                    //할인권 입수수량 표출 주석완료
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            foreach (DcDetail dcInfo in SortDiscount)
                    //            {
                    //                DIscountTicketOcs.DIscountTicketType currentTicketType = (DIscountTicketOcs.DIscountTicketType)Enum.Parse(typeof(DIscountTicketOcs.DIscountTicketType), dcInfo.Reserve4);
                    //                l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(currentTicketType, dcInfo.DcTkNO, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);

                    //                if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //                {
                    //                    //할인권 입수수량 표출
                    //                    lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                    //할인권 입수수량 표출 주석완료
                    //                }
                    //            }
                    //            l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        }

                    //        //else
                    //        //{
                    //        //    l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        //    if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //        //    {
                    //        //        //할인권 입수수량 표출
                    //        //        lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //        //        //할인권 입수수량 표출 주석완료
                    //        //    }
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        l_TIcketReadingResult = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.ReductionDiscount, dcNo, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                    //        if (feeNo != "0")
                    //        {
                    //            if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success)
                    //            {
                    //                //할인권 입수수량 표출
                    //                lblDiscountInputCount.Text = (Convert.ToInt32(lblDiscountInputCount.Text) + 1).ToString();
                    //                //할인권 입수수량 표출 주석완료
                    //            }
                    //        }
                    //    }

                    //    if (l_TIcketReadingResult == DIscountTicketOcs.TIcketReadingResult.Success) // 정상적인 티켓이라면
                    //    {
                    //        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | SinbunProcess", "감면대상 할인처리 완료");

                    //        //요금할인권처리
                    //        paymentControl.ParkingFee = TextCore.ToCommaString(mNormalCarInfo.ParkMoney.ToString()) ;
                    //        //요금할인권처리 주석완료
                    //        paymentControl.Payment = TextCore.ToCommaString(mNormalCarInfo.PaymentMoney) ;
                    //        JungangPangDisplay();
                    //        if (mNormalCarInfo.PaymentMoney == 0)
                    //        {
                    //            Payment();
                    //            return;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SinbunProcess", "감면대상 할인코드가 없음");
                    //}
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu | SinbunProcess ", "예외상황 : " + ex.ToString());
            }
        }

        private void btnGamMyunTestDiscount_Click(object sender, EventArgs e)
        {
            SinBunReader.CardInfo info = new SinBunReader.CardInfo();

            switch (cbGamMyeonItem.SelectedIndex)
            {
                case 0:
                    info.DiscountType = SinBunReader.DiscountType.BoHun;
                    info.BohunNum = "12345678";
                    break;
                case 1:
                    info.DiscountType = SinBunReader.DiscountType.BokJi;
                    info.Grade = "1";
                    break;
                case 2:
                    info.DiscountType = SinBunReader.DiscountType.BokJi;
                    info.Grade = "5";
                    break;
            }


        }
        // 신분증인식기 적용완료


        private void CardActionKocesPayMGate()
        {
            try
            {


                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제금액이0원이고 카드가 들어가있다면
                {
                    //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                int result = KocesTcmMotor.CardState();
                if (result == 2)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                }
                else
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    if (_CardpaySuccess.Success)
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | OnTimerKocesPayMGateState", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | OnTimerKocesPayMGateState", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu | OnTimerKocesPayMGateState ", "예외상황 : " + ex.ToString());
            }

        }

        private void btnSamSungPay_Click(object sender, EventArgs e)
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | btnSamSungPay_Click", "삼성페이 결제버튼 누름");

            if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0
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
                        Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                        {
                            NPSYS.CashCreditCount += 1;
                            NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제됨");

                            if (mCurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
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
                        if (mCurrentNormalCarInfo.PaymentMoney > 0)
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

                        Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                        if (_CardpaySuccess.Success)
                        {
                            NPSYS.CashCreditCount += 1;
                            NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnSamSungPay_Click", "정상적인 카드결제됨");

                            if (mCurrentNormalCarInfo.PaymentMoney == 0)
                            {
                                //카드실패전송
                                mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
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
                        if (mCurrentNormalCarInfo.PaymentMoney > 0)
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

        public void SetRemoteDiscount(int pPrePayMoney, Payment pRemotePayment)
        {
            try
            {


                inputtime = NPSYS.SettingInputTimeValue;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SetRemoteDiscount", "원격할인 받은정보"
                                                                                                     + " 기존결제금액:" + pPrePayMoney.ToString()
                                                                                                     + " 현재남은금액:" + mCurrentNormalCarInfo.PaymentMoney.ToString()
                                                                                                     + " 기존정보 주차시간:" + mCurrentNormalCarInfo.ParkingMin.ToString()
                                                                                                     + " 기존정보 전체주차요금:" + mCurrentNormalCarInfo.TotFee.ToString()
                                                                                                     + " 기존정보 전체할인요금:" + mCurrentNormalCarInfo.TotDc.ToString()
                                                                                                     + " 기존정보 사정전산요금:" + mCurrentNormalCarInfo.RecvAmt.ToString()
                                                                                                     + " 기존정보 사정전산거스름돈:" + mCurrentNormalCarInfo.Change.ToString()
                                                                                                     + " 받은정보 주차시간:" + pRemotePayment.parkingMin.ToString()
                                                                                                     + " 받은정보 전체주차요금:" + pRemotePayment.totFee.ToString()
                                                                                                     + " 받은정보 전체할인요금:" + pRemotePayment.totDc.ToString()
                                                                                                     + " 받은정보 사정전산요금:" + pRemotePayment.recvAmt.ToString());

                mCurrentNormalCarInfo.ParkingMin = pRemotePayment.parkingMin;
                mCurrentNormalCarInfo.TotFee = Convert.ToInt32(pRemotePayment.totFee);
                mCurrentNormalCarInfo.TotDc = Convert.ToInt32(pRemotePayment.totDc);
                mCurrentNormalCarInfo.RecvAmt = Convert.ToInt32(pRemotePayment.recvAmt);
                mCurrentNormalCarInfo.Change = Convert.ToInt32(pRemotePayment.change);
                mCurrentNormalCarInfo.DcCnt = pRemotePayment.dcCnt;
                mCurrentNormalCarInfo.RealFee = Convert.ToInt32(pRemotePayment.realFee);


                paymentControl.ParkingFee = TextCore.ToCommaString(mCurrentNormalCarInfo.TotFee.ToString());
                paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                paymentControl.RecvMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.RecvAmt - mCurrentNormalCarInfo.Change); //시제설정누락처리
                paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                if (pPrePayMoney == mCurrentNormalCarInfo.PaymentMoney)
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
                if (mCurrentNormalCarInfo.PaymentMoney == 0)
                {
                    //카드실패전송
                    mCurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountRemote;
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


        private void btnEnglish_Click(object sender, EventArgs e)
        {

        }

        private void btnJapan_Click(object sender, EventArgs e)
        {

        }

        #region 강제테스트
        private void btnTestJson_Click(object sender, EventArgs e)
        {
            mCurrentNormalCarInfo.VanCheck = 1;
            mCurrentNormalCarInfo.VanCardNumber = "1234";
            mCurrentNormalCarInfo.VanRegNo = "1234567890";
            mCurrentNormalCarInfo.VanDate = DateTime.Now.ToString("yyyyMMdd");
            mCurrentNormalCarInfo.VanRescode = "0000";
            mCurrentNormalCarInfo.VanResMsg = "성공";
            mCurrentNormalCarInfo.VanCardName = "하나SK카드";
            mCurrentNormalCarInfo.VanBeforeCardPay = mCurrentNormalCarInfo.PaymentMoney;
            string senddate = DateTime.Now.ToString("yyyy-MM-dd");
            string sendtime = DateTime.Now.ToString("HH:mm:ss");
            mCurrentNormalCarInfo.VanCardApproveYmd = senddate;
            mCurrentNormalCarInfo.VanCardApproveHms = sendtime;
            mCurrentNormalCarInfo.VanCardApprovalYmd = senddate;
            mCurrentNormalCarInfo.VanCardApprovalHms = sendtime;
            mCurrentNormalCarInfo.VanIssueCode = "01";
            mCurrentNormalCarInfo.VanIssueName = "하나은행";
            mCurrentNormalCarInfo.VanCardAcquirerCode = "11";
            mCurrentNormalCarInfo.VanCardAcquirerName = "성공";
            mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
            LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
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
        #endregion

        #region 정기권 연장버튼처리

        private void btnOneMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(1);
        }

        private void btnTwoMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(2);

        }

        private void btnThreeMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(3);

        }

        private void btnFourMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(4);

        }

        private void btnFiveMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(5);

        }

        private void btnSixMonthAdd_Click(object sender, EventArgs e)
        {
            SetNextRegExipire(6);

        }

        private void SetNextRegExipire(int pMonth)
        {
            paymentControl.ButtonEnable(ButtonEnableType.AddMonthStart);
            int pPrePayMoney = mCurrentNormalCarInfo.PaymentMoney;
            mCurrentNormalCarInfo.SetRegCurrMonthSetting(pMonth);
            paymentControl.ElapsedTime = NPSYS.ConvetYears_Dash(mCurrentNormalCarInfo.NextExpiredYmd);
            paymentControl.ParkingFee = TextCore.ToCommaString(mCurrentNormalCarInfo.TotFee);
            paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
            paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SetNextRegExipire", "정기권연장일선택 개월수:" + pMonth.ToString() + " 연장가능일:" + NPSYS.ConvetYears_Dash(mCurrentNormalCarInfo.NextExpiredYmd));

            if (pPrePayMoney > mCurrentNormalCarInfo.PaymentMoney) // 현재 할인되서 금액이 할인됬다면
            {
                BeforeChangePayValueAsCardReader();
                ChangePayValueAsCardReader();
            }

            paymentControl.ButtonEnable(ButtonEnableType.AddMonthEnd);
        }

        #endregion

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void btnTestBarcodeJehan_Click(object sender, EventArgs e)
        {
            DcDetail dcDetail = new DcDetail();
            dcDetail.currentDiscountTicketType = DcDetail.DIscountTicketType.BR;
            dcDetail.DcTkno = txtTestBarcodeDiscount.Text;
            dcDetail.UseYn = true;
            mCurrentNormalCarInfo.ListDcDetail.Add(dcDetail);
        }
    }
}


