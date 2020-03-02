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
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    #region #### Delegate for Form Event ####

    public delegate void LanguageChange(NPCommon.ConfigID.LanguageType languageType);
    public delegate void EventHandlerAddCtrl();
    public delegate void ChangeView(NPSYS.FormType closeView, NPSYS.FormType openView = NPSYS.FormType.NONE);
    public delegate void ChangeView<T>(NPSYS.FormType closeView, NPSYS.FormType openView, T param);

    #endregion

    public partial class FormCreditMain : Form
    {
        #region 변수선언부
        private List<NormalCarInfo> m_LIst_CarInfo = new List<NormalCarInfo>();

        private System.Threading.Timer GcClolector_Thread = null;
        private System.Threading.Timer CenterAlive_Thread = null;

        //Tmap연동
        private NPTimer PaymentIntervalCheck_Thread = new NPTimer();
        private DateTime paymentCheckDate = DateTime.Now;
        private bool isFirstPaymentCheck = false;
        private const int paymentInterval = 6000000; //10분(millisecond)
        private const int paymentCheckErrCode = 13;
        //Tmap연동완료

        public NPSYS.FormType mMainFormType = NPSYS.FormType.Main;
        System.Timers.Timer timerAutoMagam = new System.Timers.Timer();

        private FormCreditSearchCarNumber mFormCreditSearchCarNumber = null;
        private FormCreditSelectCarnumber mFormCreditSelectCarnumber = null;
        private FormCreditPaymentMenu mFormCreditPaymentMenu = null;
        private FormCreditRecipt mFormCreditRecipt = null;
        private FormCreditInfomation mFormCreditInfomation = null;

        private MainUC MainControl;
        private Dictionary<NPSYS.FormType, ISubForm> dicSubForms = new Dictionary<NPSYS.FormType, ISubForm>();

        Queue<ReceveDataFromRestServer> mReceiveQueue = new Queue<ReceveDataFromRestServer>();
        object mLockObject = new object();
        NPHttpServer mNpHttpServer = new NPHttpServer();
        private HttpProcess mHttpProcess = new HttpProcess();
        SendDeviceErrorThread mSendErrorThread = new SendDeviceErrorThread();
        SendResendThread mSendResendThread = new SendResendThread();
        //스마트로 TIT_DIP EV-CAT 적용
        private Smatro_TITDIP_EVCAT mSmartro_TITDIP_EVCat = new Smatro_TITDIP_EVCAT();
        //스마트로 TIT_DIP EV-CAT 적용완료
        #endregion

        #region 폼로딩 작업

        public FormCreditMain()
        {
            InitializeComponent();

            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
                this.Location = new Point(0, 0);
                MainControl = FormFactory.GetInstance().GetDesignControl<MainUC>(BoothCommonLib.ClientAreaRate._4vs3);
            }
            else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                this.ClientSize = new System.Drawing.Size(1080, 1920);
                this.Location = new Point(0, 0);
                MainControl = FormFactory.GetInstance().GetDesignControl<MainUC>(BoothCommonLib.ClientAreaRate._9vs16);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                lblStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                bool isSuccessCert = new NPSYS.LocalDbSetting().Load_LocalDbInfo(); // 개국성공
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_Load", "프로그램 시작됨");
                if (isSuccessCert == false)
                {
                    string webFailMsg = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_WEBFAIL_OPERATION.ToString());
                    DialogResult result = new FormMessagePrePay(FormMessagePrePay.CarType.ERROREXIT, webFailMsg).ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        NPSYS.gIsApplicationExit = true;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|ProgramInfo", "프로그램 오류로 접속종료시킴 에러내용:" + "개국실패 프로그램 종료요청");
                        Application.Exit();
                        return;

                    }
                }

                TextCore.BoothName = NPSYS.BoothName;
                lblDeviceName.Text = NPSYS.BoothName;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_Load", "프로그램 시작됨");
                SetLanguage(NPSYS.CurrentLanguageType);

                //2020.01.22 이재영 : 신규 디자인 셋팅
                this.Controls.Add((Control)MainControl);
                MainControl.Dock = DockStyle.Fill;
                MainControl.BringToFront();
                MainControl.Initialize();
                //Event 설정
                MainControl.ConfigCall += this.MainControl_ConfigCall;
                MainControl.SearchCar_Click += SearchCar_Click;
                //신규 디자인 셋팅 완료

                ControlBackColorConvertImage();
                TextCore.makeLogFileFolder();

                lblVersion.Text = NPSYS.ProgramVersion();


                if (NPSYS.Device.UsingSettingRestFul)
                {
                    //// HTTP
                    mNpHttpServer.Start(Convert.ToInt32(NPSYS.gRestFulLocalPort));
                    mNpHttpServer.EventOnReceiveData += new NPHttpServer.ReceiveData(RestFulReceive);



                }

                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat)
                {
                    NPSYS.Device.SmtSndRcv = axSmtSndRcvVCAT;
                }
                // 2016.10.27 KIS_DIP 추가
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                {
                    NPSYS.Device.KisPosAgent = axKisPosAgent;
                    //NPSYS.Device.KisPosAgent.OnAgtComplete += new EventHandler(KisPosAgent_OnAgtComplete);
                    //NPSYS.Device.KisPosAgent.OnAgtState += new EventHandler(KisPosAgent_OnAgtState);
                }
                //스마트로 TIT_DIP EV-CAT 적용
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    NPSYS.Device.Smartro_TITDIP_Evcat = axSmartroEvcat;
                    NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;



                }
                axSmartroEvcat.SendToBack();
                //스마트로 TIT_DIP EV-CAT 적용완료

                axKisPosAgent.SendToBack();
                // 2016.10.27  KIS_DIP 추가종료
                axSmtSndRcvVCAT.SendToBack();

                Initialize();
                if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.NEXPA && NPSYS.Device.gIsUseDidoDevice)
                {
                    NPSYS.Device.NexpaDoSensor.eventreceiveBoardData += new NexpaControlBoard.receiveBoardData(actionReceiveBoardData);
                }
                if (NPSYS.Device.UsingSettingRestFul)
                {
                    //// HTTP
                    mSendResendThread.StartSendThread();
                    mSendErrorThread.StartSendThread();
                    CenterAlive_Thread = new System.Threading.Timer(new TimerCallback(CenterAliveTimer.CenterAliveTimerAction), null, 0, NPSYS.gCenterAliveTime * 1000);

                }

                //Tmap연동
                if (NPSYS.gUseTmap)
                {
                    //정산 간격 체크 Thread 생성
                    PaymentIntervalCheck_Thread.Interval = paymentInterval;
                    PaymentIntervalCheck_Thread.Tick += PaymentIntervalCheckAction;
                    PaymentIntervalCheck_Thread.Start();
                }
                //Tmap연동완료

                Thread.Sleep(1000);
                SetEachFormInitialize();
                SetEachFormSetEvent();
                // db에서 출차무인정산기 가동여부를 가져온다.
                NPSYS.g_Autoboothenable = NPSYS.GetAutoboothEnable();
                CheckForIllegalCrossThreadCalls = false;

                TimerFormLauncher.Enabled = true;


                GcClolector_Thread = new System.Threading.Timer(new TimerCallback(GabigeCollectDelete.GcCollecetDelete), null, 0, 1000);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_Load", "스타트데몬이후");
                GetDeviceStatus();

                NPSYS.CurrentDirctory = Environment.CurrentDirectory;
                NPSYS.BoothID = NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_Load", "시작시 메모리양:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());


                timerDeviceStatus.Enabled = true;
                timerCardreaderFrontEject.Enabled = true;

                if (NPSYS.isBoothRealMode)
                {
                    //   Cursor.Hide();  // 희주test
                }


                if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
                {
                    NPSYS.LedLight();
                    NPSYS.Device.DoSensors.DosensorSignalEvent += new GoodTechContorlBoard.SignalEvent(DoSensors_DosensorSignalEvent);
                }
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode)
                {
                    NPSYS.Device.BarcodeSerials.EventBarcode += new BarcodeSerial.BarcodeEvent(BarcodeSerials_EventBarcode);
                }
                //바코드모터드리블 사용
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000)
                {
                    NPSYS.Device.BarcodeMoter.EventAutoRedingData += new BarcodeMoter.GetAutoRedingData(BarcodeMoter_EventAutoRedingDataEvent);
                }
                //바코드모터드리블 사용완료


                if (NPSYS.gUseAutoMagam)
                {
                    timerAutoMagam.Interval = 30000;
                    timerAutoMagam.Elapsed += new ElapsedEventHandler(timerAutoMagam_Tick);
                    timerAutoMagam.Enabled = true;
                    timerAutoMagam.Start();

                }

                axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
                axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
                axWindowsMediaPlayer1.ErrorEvent += new EventHandler(player_ErrorEvent);
                axWindowsMediaPlayer1.SendToBack();
                //나이스연동완료

                // FIRSTDATA처리
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP || NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
                {

                    bool isSuccess = FirstDataDip.Connect(NPSYS.gVanIp, NPSYS.gVanPort);

                    string errormessage = string.Empty;
                    NPSYS.Device.UsingSettingCreditCard = false;

                    if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
                    {
                        if (isSuccess)
                        {
                            NPSYS.Device.gIsUseCreditCardDevice = true;
                            NPSYS.Device.UsingSettingCreditCard = true;
                        }
                        else
                        {
                            NPSYS.Device.gIsUseCreditCardDevice = false;
                            NPSYS.Device.UsingSettingCreditCard = false;
                            NPSYS.Device.CreditCardDeviceErrorMessage1 = "FIRSTDATA_DIP 초기화 실패";
                            TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                        }
                    }
                    else if (NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
                    {
                        if (isSuccess)
                        {
                            NPSYS.Device.gIsUseMagneticReaderDevice = true;
                            NPSYS.Device.UsingSettingCreditCard = true;
                        }
                        else
                        {
                            NPSYS.Device.gIsUseMagneticReaderDevice = false;
                            NPSYS.Device.UsingSettingCreditCard = false;
                            NPSYS.Device.CreditCardDeviceErrorMessage2 = "FIRSTDATA_DIP 초기화 실패";
                            TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "FormLauncher|Initialize", "초기화실패");
                        }
                    }
                }
                NPSYS.NoCheckCargeMoneyOut();
                this.Activate();
            }
            catch (Exception ex)
            {
                // 2016.10.27 KIS_DIP 추가
                DialogResult result = new FormMessagePrePay(FormMessagePrePay.CarType.ERROREXIT, ex.ToString()).ShowDialog(); ;
                if (result == DialogResult.Cancel)
                {
                    NPSYS.gIsApplicationExit = true;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|ProgramInfo", "프로그램 오류로 접속종료시킴 에러내용:" + ex.ToString());
                    Application.Exit();
                    ;
                }
                // 2016.10.27 KIS_DIP 종료
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|FormMain_Load", "예외사항:" + ex.ToString());
            }


        }

        /// <summary>
        /// 정산 간격을 체크하여 24시간 내 정산이 없을 시 서버로 알림을 전송한다.
        /// </summary>
        /// <param name="state"></param>
        private void PaymentIntervalCheckAction(object sender, EventArgs e)
        {
            //DB에서 마지막 정산 정보를 가져온다.
            DataTable dt = LPRDbSelect.GetLastestPaymentLog();

            if (dt == null && isFirstPaymentCheck == false) //이 프로그램이 켜진 이후로 한번도 정산이 발생하지 않음. 
            {
                paymentCheckDate = DateTime.Now;
                isFirstPaymentCheck = true;
            }
            else if (dt == null && isFirstPaymentCheck) //이 프로그램이 켜진 이후로 여러번 체크를 했지만 한번도 정산이 발생하지 않음.
            {
                //이 경우에도 24시간 Check를 하여 서버로 전문을 보내도록 한다.
                TimeSpan result = DateTime.Now - paymentCheckDate;
                if (result.TotalHours >= 24)
                {
                    //TODO : 전문 송신
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.APS, CommProtocol.DeviceStatus.NotUse, paymentCheckErrCode);
                    paymentCheckDate = DateTime.Now;
                }
            }
            else //DB에 값이 있음.
            {
                string logDate = dt.Rows[0]["LOG_DATE"].ToString();
                DateTime date = DateTime.ParseExact(logDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                TimeSpan result = DateTime.Now - date;
                if (result.TotalHours >= 24)
                {
                    //TODO : 전문 송신
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.APS, CommProtocol.DeviceStatus.NotUse, paymentCheckErrCode);
                    paymentCheckDate = DateTime.Now;
                }
            }

        }

        private void BarcodeMoter_EventAutoRedingData(BarcodeMoter.BarcodeMotorResult pBarcodeResult)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SHOW로 생성할 폼들을 객체생성한다
        /// </summary>
        private void SetEachFormInitialize()
        {
            mFormCreditSearchCarNumber = new FormCreditSearchCarNumber();
            mFormCreditSelectCarnumber = new FormCreditSelectCarnumber();
            mFormCreditPaymentMenu = new FormCreditPaymentMenu();
            mFormCreditRecipt = new FormCreditRecipt();
            mFormCreditInfomation = new FormCreditInfomation();
        }

        private void SetEachFormSetEvent()
        {
            mFormCreditSearchCarNumber.EventExitSearchForm_NextSelectForm += ActionChangeView;
            mFormCreditSearchCarNumber.EventExitSerachForm += ActionChangeView;
            mFormCreditSearchCarNumber.EventExitSearchForm_NextInfo += ActionChangeView;
            mFormCreditSearchCarNumber.EventLanguageChange += new LanguageChange(ActionLanguaChange);

            mFormCreditSelectCarnumber.EventExitSelectForm += ActionChangeView;
            mFormCreditSelectCarnumber.EventExitSelectForm_CarInfo += ActionChangeView;
            mFormCreditSelectCarnumber.EventExitSelectForm_NextInfo += ActionChangeView;
            mFormCreditSelectCarnumber.EventLanguageChange += new LanguageChange(ActionLanguaChange);

            mFormCreditPaymentMenu.EventExitPayForm += ActionChangeView;
            mFormCreditPaymentMenu.EventExitPayForm_NextReceiptForm += ActionChangeView;
            mFormCreditPaymentMenu.EventExitPayForm_NextInfo += ActionChangeView;

            mFormCreditRecipt.EventExitReceiptForm += ActionChangeView;
            mFormCreditInfomation.EventExitInfoForm_EventNextForm += ActionChangeView;

            SubscribeForm(NPSYS.FormType.Search, mFormCreditSearchCarNumber);
            SubscribeForm(NPSYS.FormType.Select, mFormCreditSelectCarnumber);
            SubscribeForm(NPSYS.FormType.Payment, mFormCreditPaymentMenu);
            SubscribeForm(NPSYS.FormType.Receipt, mFormCreditRecipt);
            SubscribeForm(NPSYS.FormType.Info, mFormCreditInfomation);
        }

        private void SubscribeForm(NPSYS.FormType kind, ISubForm form)
        {
            if (!dicSubForms.ContainsKey(kind))
            {
                dicSubForms.Add(kind, form);
            }
        }

        /// <summary>
        /// MainForm이 보유하고 있는 SubForm을 반환한다.
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        public ISubForm GetSubForm(NPSYS.FormType formType)
        {
            return dicSubForms[formType];
        }

        void ActionLanguaChange(ConfigID.LanguageType pLanguageType)
        {
            SetLanguage(pLanguageType);
            mFormCreditSearchCarNumber.SetLanguage(pLanguageType);
        }

        /// <summary>
        /// 화면을 전환한다.
        /// </summary>
        /// <param name="closeView">닫을 화면</param>
        /// <param name="openView">열 화면</param>
        public void ActionChangeView(NPSYS.FormType closeView, NPSYS.FormType openView)
        {
            if (closeView != NPSYS.FormType.NONE)
            {
                var closeForm = GetSubForm(closeView);
                if (closeForm != null)
                {
                    if (openView != NPSYS.FormType.NONE)
                    {
                        var openForm = GetSubForm(openView);
                        if (closeView == NPSYS.FormType.Info) openForm.OpenViewBeforeInfo(closeView);
                        else
                        {
                            ((Form)openForm).TopMost = true;
                            openForm.OpenView(closeView, (object)null);
                        }

                        ((Form)openForm).TopMost = true;
                        Application.DoEvents();
                    }

                    if (openView == NPSYS.FormType.Info) closeForm.CloseViewBeforeInfo();
                    else closeForm.CloseView();
                }
            }
            else
            {
                ((Form)GetSubForm(openView)).TopMost = true;
                GetSubForm(openView).OpenView(closeView, (object)null);
            }
        }

        /// <summary>
        /// 화면을 전환한다.
        /// </summary>
        /// <typeparam name="T">전달 할 Parameter의 Type을 지정한다.</typeparam>
        /// <param name="closeView">닫을 화면</param>
        /// <param name="openView">열 화면</param>
        /// <param name="param">전달 Parameter</param>
        public void ActionChangeView<T>(NPSYS.FormType closeView, NPSYS.FormType openView, T param)
        {
            if (closeView != NPSYS.FormType.NONE)
            {
                var closeForm = GetSubForm(closeView);
                if (closeForm != null)
                {
                    var openForm = GetSubForm(openView);
                    //param은 무조건 존재 함..
                    if (closeView == NPSYS.FormType.Info) openForm.OpenViewBeforeInfo(closeView);
                    else
                    {
                        ((Form)openForm).TopMost = true;
                        openForm.OpenView<T>(closeView, param);
                    }
                    ((Form)openForm).TopMost = true;
                    Application.DoEvents();

                    if (openView == NPSYS.FormType.Info) closeForm.CloseViewBeforeInfo();
                    else closeForm.CloseView();
                }
            }
            else
            {
                ((Form)GetSubForm(openView)).TopMost = true;
                GetSubForm(openView).OpenView<T>(closeView, param);
            }
        }

        // 통합처리

        void DoSensors_DosensorSignalEvent(object sender, GoodTechContorlBoard.SignalType p_SignalType)
        {
            try { }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|DoSensors_DosensorSignalEvent", ex.ToString());
            }
        }

        void BarcodeSerials_EventBarcode(object sender, string pBarcodeData)
        {
            try { }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | BarcodeSerials_EventBarcode", ex.ToString());
            }
        }
        //바코드모터드리블 사용
        void BarcodeMoter_EventAutoRedingDataEvent(BarcodeMoter.BarcodeMotorResult pResultBarcode)
        {
            try { }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | BarcodeMoter_EventAutoRedingDataEvent", ex.ToString());
            }

        }
        //바코드모터드리블 사용완료

        /// <summary>
        /// 환경설정들을 불러오고 장비를 구동시킨다.
        /// </summary>
        public void Initialize()
        {
            FormLauncher _FormLauncher = new FormLauncher();
            _FormLauncher.ShowDialog();
        }

        /// <summary>
        /// 메인화면에 올라와있는 이미지들을 투명화시킨다.
        /// </summary>
        private void ControlBackColorConvertImage()
        {
            lblStartDate.BringToFront();
            lbl_status_BillCharge.BringToFront();
            lbl_status_CardRead.BringToFront();
            lbl_status_CardRead2.BringToFront();
            lbl_status_CoinCharge50.BringToFront();
            lbl_status_MoneyInsert.BringToFront();
            lbl_status_CoinInsert.BringToFront();
            lbl_status_ReciptPrint.BringToFront();

            lblTcpIpName.BringToFront();
            grupTest.BringToFront();

            if (NPSYS.isBoothRealMode)
            {
                grupTest.Visible = false;
            }
            else
            {
                grupTest.Visible = true;
            }

        }

        #endregion

        #region 언어변환
        /// <summary>
        /// 언어변경
        /// </summary>
        private void SetLanguage(ConfigID.LanguageType pLanguageType)
        {

            NPSYS.Config.SetValue(ConfigID.FeatureSettingLanguage, pLanguageType.ToString()); // 메인폼에서 받아서 저장한다

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

        // 나이스연동
        bool isPlayerOkStatus = true;
        private List<string> mListWavFile = new List<string>();
        void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

            if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsStopped)
            {
                //   MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
            }
        }

        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | Player_MediaError", "플레이어오류:" + e.ToString());
                isPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | Player_MediaError", ex.ToString());
            }
        }
        //
        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {

                // Get the description of the first error. 
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | player_ErrorEvent", "에러내용" + errDesc);
                isPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | player_ErrorEvent", ex.ToString());
            }
        }

        private void playVideo(string pfileName)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain | playVideo", "[나이스Wave실행]" + pfileName);
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Movie\" + pfileName;
                axWindowsMediaPlayer1.uiMode = "none";
                if (isPlayerOkStatus == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|playVideo", "예외사항:" + ex.ToString());
            }
        }

        #region    SERVER에서 받은데이터 처리
        void RestFulReceive(HttpServer.RequestEventArgs pEvent, NPHttpServer.CmdType pCmdType, string pMethod, string pData)
        {

            ReceveDataFromRestServer mReceveDataFromRestServer = new ReceveDataFromRestServer();
            mReceveDataFromRestServer.Event = pEvent;
            mReceveDataFromRestServer.CmdType = pCmdType;
            mReceveDataFromRestServer.Method = pMethod;
            mReceveDataFromRestServer.Data = pData;
            mReceiveQueue.Enqueue(mReceveDataFromRestServer);
        }

        #endregion

        #region 폼종료작업


        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_FormClosed", "프로그램 종료작업 시작");

                GcClolector_Thread?.Dispose();
                CenterAlive_Thread?.Dispose();

                //Tmap연동
                if (PaymentIntervalCheck_Thread != null && PaymentIntervalCheck_Thread.Enabled)
                {
                    PaymentIntervalCheck_Thread.Enabled = false;
                    PaymentIntervalCheck_Thread.Dispose();
                }
                //Tmap연동완료
                if (NPSYS.Device.UsingSettingRestFul)
                {
                    mNpHttpServer.Stop();
                    mSendErrorThread.EndSendThread();
                    mSendResendThread.EndSendThread();
                }

                NPSYS.CloseDeviceAll();
                mFormCreditInfomation.Close();
                mFormCreditSearchCarNumber.Close();
                mFormCreditSelectCarnumber.Close();
                mFormCreditPaymentMenu.Close();
                mFormCreditRecipt.Close();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|FormMain_FormClosed", "프로그램 종료됨");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|FormMain_FormClosed", ex.ToString());
            }

        }

        #endregion

        private void btn_setAdminCahsiSetting_Click(object sender, EventArgs e)
        {
            FormAdminCashSetting _FormAdminCashSetting = new FormAdminCashSetting();
            _FormAdminCashSetting.ShowDialog();
        }

        /// <summary>
        /// 요금정산중인 화면들중에 해당하는 화면을 죽인다
        /// </summary>
        private void paymentFormTypesClose()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditMain | paymentFormTypesClose", "[현재 활성화된 폼 종료]" + NPSYS.CurrentFormType.ToString());
            mFormCreditInfomation.CloseView();
            mFormCreditRecipt.CloseView();
            mFormCreditPaymentMenu.CloseView();
            mFormCreditSelectCarnumber.CloseView();
            mFormCreditSearchCarNumber.CloseView();
            NPSYS.CurrentFormType = NPSYS.FormType.Main;

        }
        /// <summary>
        /// 정기권의 통과여부 (일반 차량이 요금이 찍힌상태로 후진등을 하여 처리가 안됬을때 정기권 차량이 들어오면 화면전환을 하기위해 만듬)
        /// </summary>
        private void TimerFormLauncher_Tick(object sender, EventArgs e)
        {
            try
            {

                if (mReceiveQueue.Count == 0)
                {
                    return;
                }
                TimerFormLauncher.Enabled = false;
                TimerFormLauncher.Stop();
                if (mReceiveQueue.Count >= 1)
                {
                    ReceveDataFromRestServer receveDataFromRestServer = new ReceveDataFromRestServer();

                    if (mReceiveQueue.Count > 0)
                    {
                        receveDataFromRestServer = mReceiveQueue.Dequeue();
                    }
                    if (mReceiveQueue.Count > 0)
                    {
                        receveDataFromRestServer = mReceiveQueue.Dequeue();
                    }


                    ProcessCarFromRestFullServer(receveDataFromRestServer.Event, receveDataFromRestServer.CmdType, receveDataFromRestServer.Method, receveDataFromRestServer.Data);



                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|TimerFormLauncher_Tick", ex.ToString());
            }
            finally
            {
                TimerFormLauncher.Enabled = true;
                TimerFormLauncher.Start();
            }
        }

        private void ProcessCarFromRestFullServer(HttpServer.RequestEventArgs pEvent, NPHttpServer.CmdType pCmdType, string pMethod, string pData)
        {
            if (pCmdType == NPHttpServer.CmdType.none)
            {
                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문오류", "현재 해당전문 없음", false));
                pEvent.Response.Body.Write(sendData, 0, sendData.Length);


                return;
            }
            switch (pCmdType)
            {
                //TMAP연동

                case NPHttpServer.CmdType.interworking_cars:
                    CarList remote_CarList = mHttpProcess.GetCarListTmap(pData);
                    if (remote_CarList == null || remote_CarList.status.Success == false)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "원격 차량리스트 통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (remote_CarList.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        else
                        {
                            if (NPSYS.CurrentFormType == NPSYS.FormType.Search)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격차량리스트", "원격차량리스트 정상", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                mFormCreditSearchCarNumber.SetCurrentCarList(remote_CarList);
                                return;
                            }
                            else
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격차량리스트", "원격차량리스트 전송시간초과", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                        }
                    }

                    break;
                //TMAP연동완료
                case NPHttpServer.CmdType.remote_cacnel_payments:
                    Payment remote_cardPayment = mHttpProcess.GetRemoteCardPayment(pData);



                    if (remote_cardPayment == null)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "원격카드취소 통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (remote_cardPayment.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        else
                        {
                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else
                            {
                                paymentFormTypesClose();
                                NormalCarInfo cardCancleNormalCar = new NormalCarInfo();
                                cardCancleNormalCar.CurrentPayment = remote_cardPayment;
                                cardCancleNormalCar.SetCurrentPayment(NormalCarInfo.PaymentSetType.RemoteCard);
                                if (cardCancleNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                                    || cardCancleNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "카드결제취소가능", "카드결제 취소진행 가능", true));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                    mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, cardCancleNormalCar);
                                }
                                else
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "카드결제취소불가", "카드결제 취소진행 불가 취소내역없음", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);


                                }

                            }

                        }
                    }
                    break;
                case NPHttpServer.CmdType.remote_print_payments:
                    Payment remote_ReceiptPayment = mHttpProcess.GetReceiptPayment(pData);
                    if (remote_ReceiptPayment == null)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "원격영수증 통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (remote_ReceiptPayment.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        else
                        {

                            NormalCarInfo remoteReceiptNormalCar = new NormalCarInfo();
                            remoteReceiptNormalCar.CurrentPayment = remote_ReceiptPayment;
                            remoteReceiptNormalCar.SetCurrentPayment(NormalCarInfo.PaymentSetType.RemoteReceipt);
                            if (remoteReceiptNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteReceipt)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격영수증데이터정상", "원격영수증데이터 정상받음응답", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                NPSYS.RecipePrint(remoteReceiptNormalCar, this.Name);
                            }
                            else
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격영수증데이터비정상", "원격영수증데이터 비정상으로 출력안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }

                        }
                    }
                    break;
                case NPHttpServer.CmdType.remote_discounts:
                    Payment remoteDiscountPayment = mHttpProcess.GetRemoteDiscount(pData);
                    if (remoteDiscountPayment == null)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "원격할인 통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (remoteDiscountPayment.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        else
                        {
                            if (NPSYS.CurrentFormType == NPSYS.FormType.Payment)
                            {
                                NormalCarInfo.CarPayStatus currentPayStatus = NormalCarInfo.CarPayStatus.NotSearch_Outcar;
                                currentPayStatus = mFormCreditPaymentMenu.mCurrentNormalCarInfo.CurrentCarPayStatus;

                                string currentPayTkno = string.Empty;
                                currentPayTkno = mFormCreditPaymentMenu.mCurrentNormalCarInfo.TkNO;


                                if (currentPayTkno != remoteDiscountPayment.car.tkNo)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "차량매칭오류", "현재 할인하려는 차량과 실제차량과 다름", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                    return;
                                }
                                else if (currentPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar  // 카드취소중에는 원격할인 안받음
                                       || currentPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격할인불가", "현재 카드취소요청중", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                    return;

                                }
                                else
                                {
                                    if (NPSYS.CurrentBusyType == NPSYS.BusyType.Paying)
                                    {
                                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격할인 불가", "결제를 진행중이라 원격할인 불가", false));
                                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                        return;
                                    }
                                    else
                                    {
                                        if (mFormCreditPaymentMenu.mCurrentNormalCarInfo.GetInComeMoney > 0)
                                        {
                                            byte[] sendFailData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격할인 불가", "현금투입중이라 원격 할인 불가", false));
                                            pEvent.Response.Body.Write(sendFailData, 0, sendFailData.Length);
                                        }
                                        else
                                        {
                                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "원격할인처리", "원격할인처리 정상", true));
                                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                            int pPrePaymoney = mFormCreditPaymentMenu.mCurrentNormalCarInfo.PaymentMoney;
                                            mFormCreditPaymentMenu.SetRemoteDiscount(pPrePaymoney, remoteDiscountPayment);
                                        }

                                        return;
                                    }
                                }
                            }
                            else
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "요금폼오류", "현재 요금화면 떠있지않음", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                return;

                            }
                        }
                    }
                case NPHttpServer.CmdType.remote_payments:
                    Payment remotePayment = mHttpProcess.GetRemotePayment(pData);
                    if (remotePayment == null)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (remotePayment.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        NormalCarInfo remoteNormalCar = new NormalCarInfo();
                        remoteNormalCar.CurrentPayment = remotePayment;
                        remoteNormalCar.SetCurrentPayment(NormalCarInfo.PaymentSetType.NormalPaymnet);
                        if (remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_Hwacha_ // 출구무인결제시 0원인경우
                          || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PreDisocunt
                          || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PrePay)
                        {

                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 처리안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "원격회차처리완료", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "결제처리 전송시작");
                                DateTime payDaveDate = DateTime.Now;
                                remoteNormalCar.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessOut;
                                //카드실패전송
                                remoteNormalCar.PaymentMethod = PaymentType.Free;
                                //카드실패전송 완료
                                Payment currentCar = mHttpProcess.PaySave(remoteNormalCar, payDaveDate);

                                if (currentCar.status.Success == false)
                                {

                                    // 재전송 처리
                                    return;
                                }

                            }

                        }
                        else if (remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_NotSeason
                                || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_NotSeason
                                || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                                || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)// 정기권인경우
                        {

                            if (remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                              || remoteNormalCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season) // 연장정기권인 경우
                            {

                                if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                }
                                else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 연장처리안함", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                }
                                else
                                {
                                    paymentFormTypesClose();
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "정기권 연장가능", true));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + "연장정기권 요금표출 진행");
                                    mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, remoteNormalCar);
                                }
                            }
                            else // 비연장정기권인경우
                            {
                                if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                }
                                else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 처리안함", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                }
                                else
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "정상 출차처리", true));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);


                                }
                            }
                        }
                        else // 일반차량 요금나오는 경우
                        {
                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "원격요금표출 가능", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + "일반차량 요금표출 진행");
                                mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, remoteNormalCar);
                            }

                        }
                    }
                    break;
                case NPHttpServer.CmdType.payments:
                    //TMAP연동
                    if (NPSYS.CurrentFormType == NPSYS.FormType.Select)
                    {
                        Payment currentSelectTmapRemotePayment = mHttpProcess.GetPaymentTmap(pData);
                        if (currentSelectTmapRemotePayment == null)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "차량 확정선택 잘못된 데이터라 처리안함", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;

                        }

                        mFormCreditSelectCarnumber.SetSelectOnlyCar(currentSelectTmapRemotePayment);

                        return;
                    }
                    //TMAP연동 완료
                    Payment outPayment = mHttpProcess.GetPayment(pData);
                    if (outPayment == null)
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "통신데이터 잘못된 데이터라 처리안함", false));
                        pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                        return;
                    }
                    else
                    {
                        if (outPayment.status.Success == false)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "전문Parsing오류", "상태값 오류", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            return;
                        }
                        NormalCarInfo paymentCar = new NormalCarInfo();
                        paymentCar.CurrentPayment = outPayment;
                        paymentCar.SetCurrentPayment(NormalCarInfo.PaymentSetType.NormalPaymnet);
                        if (paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_Hwacha_ // 출구무인결제시 0원인경우
                          || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PreDisocunt
                          || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PrePay)
                        {

                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "회차처리완료", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "결제처리 전송시작");
                                DateTime paydate = DateTime.Now;
                                paymentCar.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessOut; //시제설정누락처리
                                                                                                            //카드실패전송
                                paymentCar.PaymentMethod = PaymentType.Free;
                                //카드실패전송 완료
                                Payment currentCar = mHttpProcess.PaySave(paymentCar, paydate);

                                if (currentCar.status.Success == false)
                                {
                                    // 재전송 처리
                                    return;
                                }

                            }

                        }
                        else if (paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_NotSeason
                                || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_NotSeason
                                || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                                || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)// 정기권인경우
                        {

                            if (paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                              || paymentCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season) // 연장정기권인 경우
                            {

                                if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                }
                                else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                }
                                else
                                {
                                    paymentFormTypesClose();
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "정기권 연장가능", true));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + "연장정기권 요금표출 진행");
                                    mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, paymentCar);
                                }
                            }
                            else // 비연장정기권인경우
                            {
                                if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                }
                                else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                }
                                else
                                {
                                    byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "정상 출차처리", true));
                                    pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                }
                            }
                        }
                        else // 일반차량 요금나오는 경우
                        {
                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "요금표출 가능", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + "일반차량 요금표출 진행");
                                mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, paymentCar);
                            }

                        }
                    }
                    break;

                case NPHttpServer.CmdType.cars:
                    Car exitCarInfo = mHttpProcess.ExitCarInfo(pData);
                    if (exitCarInfo.status.Success == false)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditMain | RestFulReceive", "[출차차량정보오류" + NPHttpServer.CmdType.cars.ToString() + "]" + " 오류코드:" + exitCarInfo.status.code + " 오류메시지:" + exitCarInfo.status.message);
                        return;
                    }
                    NormalCarInfo exitCar = new NormalCarInfo();

                    exitCar.CurrentCar = exitCarInfo;
                    if (exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_NotSeason
                       || exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_NotSeason
                       || exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                       || exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season
                       ) // 정기권인경우
                    {

                        if (exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season || exitCar.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season) // 연장정기권인 경우
                        {
                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정기차량", "정기권 연장가능", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + "연장정기권 요금표출 진행");
                                mFormCreditPaymentMenu.OpenView(NPSYS.FormType.Main, exitCar);
                            }
                        }

                        else // 비연장정기권인경우
                        {
                            if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                            {
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            }
                            else
                            {
                                paymentFormTypesClose();
                                byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정기차량", "정상 출차처리", true));
                                pEvent.Response.Body.Write(sendData, 0, sendData.Length);

                            }
                        }
                    }
                    else
                    {
                        if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.Paying) // 없무중이면
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "다른차량 결제중", "현재 현금 또는 카드로 결제중", true));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            return;

                        }
                        else if (NPSYS.GetCurrentBusy() == NPSYS.BusyType.ManagerMode)
                        {
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "관리자모드", "관리자 모드라 요금표출안함", false));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                        }
                        else
                        {
                            paymentFormTypesClose();
                            byte[] sendData = Encoding.UTF8.GetBytes(mHttpProcess.GetReturnData(pCmdType, "정상", "차량4자리 조회가능", true));
                            pEvent.Response.Body.Write(sendData, 0, sendData.Length);
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | RestFulReceive", "수행업무 [" + pCmdType.ToString() + "]" + " 차량조회 화면 표출");
                            mFormCreditSearchCarNumber.OpenView(NPSYS.FormType.Main, exitCarInfo);
                        }
                    }
                    break;
            }
        }



        private void timerDeviceStatus_Tick(object sender, EventArgs e)
        {

            ////GS POS할인
            //if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
            //{
            //    if (HMC60.gIsCutterUp)
            //    {
            //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain  | timerDeviceStatus_Tick", "영수증프린터 CUTTERUP이라 상태표출");
            //        HMC60.HmcStatus currentPrintStatus = NPSYS.Device.HMC60.Status();
            //        HMC60.gIsCutterUp = false;
            //    }
            //}


            GetDeviceStatus();
        }

        /// <summary>
        /// 장비상태체크후 메인화면에 장비이상유무 표시
        /// </summary>
        private void GetDeviceStatus()
        {
            if (NPSYS.Device.UsingSettingBillReader)
            {
                lbl_status_MoneyInsert.Visible = !NPSYS.Device.isUseDeviceBillReaderDevice;
            }
            if (NPSYS.Device.UsingSettingCoinReader)
            {
                lbl_status_CoinInsert.Visible = !NPSYS.Device.isUseDeviceCoinReaderDevice;
            }

            if (NPSYS.Device.UsingSettingCardReadType != ConfigID.CardReaderType.None)
            {
                lbl_status_CardRead.Visible = !NPSYS.Device.gIsUseCreditCardDevice;
            }
            if (NPSYS.Device.UsingSettingMagneticReadType != ConfigID.CardReaderType.None)
            {
                lbl_status_CardRead2.Visible = !NPSYS.Device.gIsUseMagneticReaderDevice;
            }
            if (NPSYS.Device.UsingSettingBill)
            {
                lbl_status_BillCharge.Visible = !NPSYS.Device.gIsUseDeviceBillDischargeDevice;
            }

            if (NPSYS.Device.UsingSettingCoinCharger50)
            {
                lbl_status_CoinCharge50.Visible = !NPSYS.Device.gIsUseCoinDischarger50Device;
            }
            if (NPSYS.Device.UsingSettingCoinCharger100)
            {
                lbl_status_CoinCharge100.Visible = !NPSYS.Device.gIsUseCoinDischarger100Device;
            }
            if (NPSYS.Device.UsingSettingCoinCharger500)
            {
                lbl_status_CoinCharge500.Visible = !NPSYS.Device.gIsUseCoinDischarger500Device;
            }
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
            {
                lbl_status_ReciptPrint.Visible = !NPSYS.Device.gIsUseReceiptPrintDevice;
            }



            // 신분증인식기 적용
            if (NPSYS.Device.UsingSettingSinbunReader)
            {
                lbl_status_SinbunReader.Visible = !NPSYS.Device.gIsUseSinbunReader;
            }
            // 신분증인식기 적용완료
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //   CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BCH, 56);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //      CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, (byte)0x04);
        }

        private void button5_Click(object sender, EventArgs e)
        {
        }

        // 메인화면에서 환경설정으로 이동한다.
        private void MainControl_ConfigCall()
        {
            DialogResult result = new FormAdminLogin(mMainFormType).ShowDialog();
            if (result == DialogResult.Cancel)
            {
                NPSYS.CurrentFormType = NPSYS.FormType.Main;
            }
            else
            {
                NPSYS.CurrentFormType = NPSYS.FormType.Main;
            }
        }
        
        public bool isErrorConnect = false;
        /// <summary>
        /// Dll로 제어하는 카드리더기가 활성화되면 폴링방식이기에 카드를 받아들이는 명령어 사용시 카드가 들오올때 까지 계속 적으로 카드를 먹는문제 발생해서 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCardreaderFrontEject_Tick(object sender, EventArgs e)
        {
            try
            {
                if (NPSYS.CurrentFormType != NPSYS.FormType.Payment && NPSYS.CurrentFormType != NPSYS.FormType.DeviceTest)
                {

                    if (NPSYS.Device.gIsUseCreditCardDevice && (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_DIP_IFM))
                    {
                        try
                        {
                            if (NPSYS.Device.KICC_TIT.GetCardInsert())
                            {
                                NPSYS.Device.KICC_TIT.CardEject();
                            }
                        }
                        catch
                        {
                            // NPSYS.Device.gIsUseCreditCardDevice1 = false;
                        }
                    }
                    ////KOCSE 카드리더기 추가
                    if (NPSYS.Device.gIsUseCreditCardDevice && (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_TCM))
                    {
                        try
                        {
                            int resultCardState = KocesTcmMotor.CardState();
                            if (resultCardState == 2)
                            {
                                KocesTcmMotor.CardEject();
                            }
                        }
                        catch
                        {
                            // NPSYS.Device.gIsUseCreditCardDevice1 = false;
                        }
                    }

                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.FIRSTDATA_DIP)
                    {
                        FirstDataDip.readerStatus currentStatus = FirstDataDip.ReadState();
                        if (currentStatus == FirstDataDip.readerStatus.ReaderIcIn)
                        {
                            FirstDataDip.CardEject();
                        }
                    }
                    //스마트로 TIT_DIP EV-CAT 적용
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain | timerCardreaderFrontEject_Tick", "스마트로 카드리더기 리셋" + " 폼타입:" + NPSYS.CurrentFormType.ToString());
                        mSmartro_TITDIP_EVCat.CardReaderReset(NPSYS.Device.Smartro_TITDIP_Evcat);
                    }
                    //스마트로 TIT_DIP EV-CAT 적용완료

                    if (NPSYS.Device.gIsUseMagneticReaderDevice && NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.TItMagnetincDiscount) //2016-01-19 스마트로추가
                    {
                        try
                        {
                            if (NPSYS.Device.CardDevice2.TIcketFrontEject() != 0)
                            {
                                Result _CardReader2_Status = NPSYS.Device.CardDevice2.GetStatus();
                                NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus((TicketCardDevice.TicketAndCardResult)_CardReader2_Status.ReultIntMessage, false);

                                if (_CardReader2_Status.Success == false)
                                {
                                    NPSYS.Device.CreditCardDeviceErrorMessage2 = _CardReader2_Status.Message;
                                    TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "FormMain|timerCardreaderFrontEject_Tick", _CardReader2_Status.Message);
                                    NPSYS.LedLight();
                                    GetStatusDeivce();
                                }
                            }
                        }
                        catch
                        {
                            NPSYS.Device.gIsUseMagneticReaderDevice = false;
                        }
                    }
                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                    {
                        BarcodeMoter.BarcodeMotorResult result = NPSYS.Device.BarcodeMoter.GetSensor();
                        if (result.ResultSensor != BarcodeMotorTicketSenSor.None)
                        {
                            NPSYS.Device.BarcodeMoter.EjectFront();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain | timerCardreaderFrontEject_Tick", "바코드 할인권 배출");
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|timerCardreaderFrontEject_Tick", ex.ToString());
            }
        }


        public void GetStatusDeivce()
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|GetStatusDeivce", "장비체크시작");
                if (NPSYS.Device.UsingSettingBillReader && NPSYS.Device.BillReader.IsConnect)
                {
                    NPSYS.Device.BillReader.CurrentStatus();

                }
                else
                {
                    TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "FormMain|GetStatusDeivce", "지폐리더기상태:" + "연결안됨");

                }

                if (NPSYS.Device.UsingSettingCoinReader && NPSYS.Device.CoinReader.IsConnect)
                {
                    CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.DisableCoinRead();
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormMain|GetStatusDeivce", "동전리더기상태:" + l_CoinReaderResult.ToString());
                    if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormMain|GetStatusDeivce", "동전리더기상태:" + l_CoinReaderResult.ToString());
                        NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                    }
                }
                else
                {
                    TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormMain|GetStatusDeivce", "동전리더기상태:" + "연결안됨");
                    NPSYS.Device.CoinReaderDeviceErrorMessage = "연결끊김";
                    NPSYS.Device.isUseDeviceCoinReaderDevice = false;
                }
                if (NPSYS.Device.UsingSettingBill)
                {
                    Result _result = MoneyBillOutDeviice.MoneyBillStatus();
                    TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormMain|GetStatusDeivce", "지폐방출기상태:" + _result.Message);
                    if (_result.Success == false)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormMain|GetStatusDeivce", "지폐방출기상태:" + _result.Message);
                        NPSYS.Device.BillDischargeDeviceErrorMessage = _result.Message;
                        NPSYS.Device.gIsUseDeviceBillDischargeDevice = false;
                    }
                }

                TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormMain|GetStatusDeivce", "지폐방출기50연결상태:" + NPSYS.Device.CoinDispensor50.IsConnect.ToString());
                TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormMain|GetStatusDeivce", "지폐방출기100연결상태:" + NPSYS.Device.CoinDispensor100.IsConnect.ToString());
                TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormMain|GetStatusDeivce", "지폐방출기500연결상태:" + NPSYS.Device.CoinDispensor500.IsConnect.ToString());
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|GetStatusDeivce", "장비체크종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|GetStatusDeivce", ex.ToString());
            }

        }





        private void FormMain_Shown(object sender, EventArgs e)
        {

        }



        private void button6_Click(object sender, EventArgs e)
        {
            NPSYS.Device.DoSensors.RequestBoardCheck();
        }

        private void picBackGround_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

        }





        private void SearchCar_Click(object sender, EventArgs e)
        {
            try
            {

                NPSYS.buttonSoundDingDong();
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormMain|bnt_Search_Click", "차량찾기 선택누름");
                Car mNormalCarInfo = new Car();
                mFormCreditSearchCarNumber.OpenView(NPSYS.FormType.Main, mNormalCarInfo);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|bnt_Search_Click", ex.ToString());
            }
        }


        void timerAutoMagam_Tick(object source, ElapsedEventArgs e)
        {
            lock (new object())
            {
                try
                {
                    NPSYS.GetCurrentBoothTime(NPSYS.CurrentFormType == NPSYS.FormType.Payment, (string info, string err) =>
                    {
                        FormMessageShortBox mFormMessageShortBox = new FormMessageShortBox(info, err);
                        mFormMessageShortBox.ShowDialog();
                    });
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain | timerAutoMagam_Tick", ex.ToString());
                }
            }
        }

        private void btn_RegExtensionPay_Click(object sender, EventArgs e)
        {
            try
            {
                //NPSYS.buttonSoundDingDong();
                //TextCore.ACTION(TextCore.ACTIONS.USER, "FormMain|btn_RegExtensionPay_Click", "정기권 연장 버튼누름");
                //if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.CAB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.CPB_1024)
                //{
                //    NPSYS.gRegExtensionPay = true;
                //    FormCreditSearchCarNumber _FormSearchCarNumber = new FormCreditSearchCarNumber();
                //    _FormSearchCarNumber.ShowDialog();
                //}
            }
            catch (Exception ex)
            {
                //    NPSYS.gRegExtensionPay = false;
                //    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormMain|btn_RegExtensionPay_Click", ex.ToString());
            }
        }

        private void btnEnglish_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
            SetLanguage(ConfigID.LanguageType.ENGLISH);
        }

        private void btnJapan_Click(object sender, EventArgs e)
        {
            NPSYS.CurrentLanguageType = ConfigID.LanguageType.JAPAN;
            SetLanguage(ConfigID.LanguageType.JAPAN);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            NormalCarInfo nornallist = new NormalCarInfo();

            nornallist.InCarNo1 = "足立500き11-11";
            nornallist.OutCarNo1 = "足立500き11-11";
            nornallist.InYmd = "20181115";
            nornallist.InHms = "101019";
            nornallist.OutYmd = "20181116";
            nornallist.OutHms = "105019";
            nornallist.InImage1 = @"\\10.27.63.1\MSImage\2019\03\18\CH1_20190318041243_85서4850.jpg";
            nornallist.TotFee = 1000;
            mFormCreditRecipt.OpenView(NPSYS.FormType.Payment, nornallist);
            mFormCreditRecipt.Show();
        }


        private void btnTestParkInfo_Click(object sender, EventArgs e)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            Certs currentCert = new Certs();
            mNPHttpControl.SetHeader(NPHttpControl.UrlCmdType.certs);
            currentCert = (Certs)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.certs, mNPHttpControl.SendMethodGet());
            if (currentCert == null)
            {
                DateTime currentDateTime = NPSYS.LongTypeToDateTime(currentCert.date.unixTime);
                CommonFuction.SetSystemDateTime(currentDateTime.ToString("yyyyMMddHHmmss")); // 시간동기화
                switch (currentCert.nation.code)
                {
                    case "JP":
                        NPSYS.CurrentLanguageType = ConfigID.LanguageType.KOREAN;
                        break;

                    case "KOR":
                        break;
                }
                NPSYS.BoothName = currentCert.unit.name;
            }
            else
            {
            }
        }

        private void btnTestSearchCar_Click(object sender, EventArgs e)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            Car currentCar = new Car();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.cars;
            mNPHttpControl.SetHeader(currnetUrl, "9760");
            currentCar = (Car)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            if (currentCar == null)
            {
            }
            else
            {
            }

        }

        private void btnTestPayInfo_Click(object sender, EventArgs e)
        {
            {
                NPHttpControl mNPHttpControl = new NPHttpControl();
                ParkingReceiveData restClassParser = new ParkingReceiveData();
                Payment currentCar = new Payment();
                NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.payments;
                mNPHttpControl.SetHeader(currnetUrl, string.Empty, "0082-0000-0001-0001-0008-0001-180424013644");
                currentCar = (Payment)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
                if (currentCar == null)
                {
                }
                else
                {
                }
            }
        }

        private void btnTestMagam_Click(object sender, EventArgs e)
        {
            NPHttpControl mNPHttpControl = new NPHttpControl();
            ParkingReceiveData restClassParser = new ParkingReceiveData();
            Close currentClose = new Close();
            NPHttpControl.UrlCmdType currnetUrl = NPHttpControl.UrlCmdType.closeInfo;
            mNPHttpControl.SetHeader(currnetUrl, string.Empty, string.Empty);
            currentClose = (Close)restClassParser.SetDataParsing(currnetUrl, mNPHttpControl.SendMethodGet());
            if (currentClose == null)
            {
            }
            else
            {
            }
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            DateTime currTIme = DateTime.ParseExact("2019-05-01 13:33:12", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            long currrrrrtime = NPSYS.DateTimeToLongType(currTIme);
            DateTime currdate = NPSYS.LongTypeToDateTime(currrrrrtime);
            //1556685192

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            NPSYS.NoCheckCargeMoneyOut();
        }
        //스마트로 TIT_DIP EV-CAT 적용
        private void SmartroEvcat_QueryResults(object sender, AxDreampos_Ocx.__DP_Certification_Ocx_QueryResultsEvent e)
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditMain | SmartroEvcat_QueryResults ", e.returnData);
        }
        //스마트로 TIT_DIP EV-CAT 적용완료
        void actionReceiveBoardData(NexpaControlBoard.BoarData pData)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //  string data="status":{ "code":"042200","message":"요금 조회 성공","description":"요금 조회 성공"},"data":{ "payment":{ "id":1216,"car":{ "id":2999,"tkNo":"0000-0000-0000-0000-0008-0000-0000-200109132621","parkingType":0,"carType":1,"inDate":1578542700,"inCarNo1":"83너2593","outDate":1578544310,"outChk":2,"outImage1":"/images/lpr/2020/01/09/0001_1578544310_83너2593.jpg","outCarNo1":"83너2593","outRecog1":1,"outUnit":{ "id":2,"area":{ "id":2,"parkingZone":{ "id":1,"parkingLot":{ "id":1,"operator":{ "id":1,"country":{ "id":1,"name":"대한민국","code":"0082"},"name":"ADT","code":"0004","createdDate":1565908396,"status":"U"},"code":"0100","name":"TMAP 테스트","address":"","businessNo":"205-81-21711","ceoName":"","phone":"","createdDate":1565908412,"updatedDate":1578535919,"status":"U"},"name":"기본 구역","code":"0100","fullCode":"0082-0004-0100-0100","seasonCarLimit":"A","operationMode":"A","fullCarLimit":"A","inOperation":"Y","useOneWay":"Y","zoneType":"N","ctrlWebDc":"Y","ctrlWebDcTime":0,"carSearchTerm":0,"createdDate":1565908445,"updatedDate":1578535927,"status":"U","entryUnrecogAction":"open","exitUnrecogAction":"none","parkingSpotStatusNotiCycle":"10","facilitiesStatusNotiCycle":"15"},"name":"출구1","type":"O","useBar":"Y","createdDate":1565908480,"updatedDate":1578537401,"status":"U","code":"0102","takeAction":"GATE","seasonTicketTakeAction":"GATE","whiteListTakeAction":"PCC","gateId":"FCL0002835"},"unitGroup":{ "id":3,"type":"0009","name":"출구LPR"},"code":"0001","name":"출구1 LPR","ip":"10.22.5.202","port":4098,"fullCode":"0082-0004-0100-0100-0009-0001","updatedDate":1578537780,"status":"U","content":"{\"term\":0,\"primary\":\"Y\"}","facilitiesId":"FCL0002841"},"parkingZone":{ "id":1,"parkingLot":{ "id":1,"operator":{ "id":1,"country":{ "id":1,"name":"대한민국","code":"0082"},"name":"ADT","code":"0004","createdDate":1565908396,"status":"U"},"code":"0100","name":"TMAP 테스트","address":"","businessNo":"205-81-21711","ceoName":"","phone":"","createdDate":1565908412,"updatedDate":1578535919,"status":"U"},"name":"기본 구역","code":"0100","fullCode":"0082-0004-0100-0100","seasonCarLimit":"A","operationMode":"A","fullCarLimit":"A","inOperation":"Y","useOneWay":"Y","zoneType":"N","ctrlWebDc":"Y","ctrlWebDcTime":0,"carSearchTerm":0,"createdDate":1565908445,"updatedDate":1578535927,"status":"U","entryUnrecogAction":"open","exitUnrecogAction":"none","parkingSpotStatusNotiCycle":"10","facilitiesStatusNotiCycle":"15"},"status":1,"requestId":"TPD0000021_20200109133234_A","sessionId":"PSN000000092636","recogType":"L"},"discount":[],"paymentDetail":[],"parkingMin":28,"totFee":5000.0,"totDc":0.0,"realFee":5000.0,"payAmt":0.0,"recvAmt":0.0,"retAmt":0.0,"notRetAmt":0.0,"dcCnt":0,"unpaid":5000.0,"lastPayDate":0,"type":"N","feeGroup":{"id":1,"feeName":"기본 요금 그룹","feeMax":1000000.0,"freeTime":10,"isBasicGroup":"Y","dayMaxCriteria":"I","dayMaxAmt":1000000.0,"freeTimeAfterPay":10,"discountCriteria":"F","basicFeeApplyType":"O","parkingLot":{"id":1,"operator":{"id":1,"country":{"id":1,"name":"대한민국","code":"0082"},"name":"ADT","code":"0004","createdDate":1565908396,"status":"U"},"code":"0100","name":"TMAP 테스트","address":"","businessNo":"205-81-21711","ceoName":"","phone":"","parkingZone":[{"id":1,"name":"기본 구역","code":"0100","fullCode":"0082-0004-0100-0100","seasonCarLimit":"A","operationMode":"A","fullCarLimit":"A","inOperation":"Y","useOneWay":"Y","zoneType":"N","ctrlWebDc":"Y","ctrlWebDcTime":0,"carSearchTerm":0,"createdDate":1565908445,"updatedDate":1578535927,"status":"U","entryUnrecogAction":"open","exitUnrecogAction":"none","parkingSpotStatusNotiCycle":"10","facilitiesStatusNotiCycle":"15"}],"createdDate":1565908412,"updatedDate":1578535919,"status":"U"},"feeItem":[{"id":1,"workingDay":10,"feeItemName":"항상","carType":1,"startTime":"00:00:00","endTime":"23:59:59","basicTime":30,"basicFee":1000.0,"freeTime":0,"perFee":500.0,"perTime":10,"maxFee":100000.0,"createdDate":1565911791,"status":"U"}],"createdDate":1565911760,"status":"U"},"chargingId":"CHG000000079810","chargingAmt":5000.0,"inVehicleDateTime":"20200109130500","chargingRequestDateTime":"20200109133234","outVehicleAllowYn":"N"}}}

        }
    }

}