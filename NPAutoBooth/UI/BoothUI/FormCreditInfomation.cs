using FadeFox.Text;
using FadeFox.UI;
using NPAutoBooth.Common;
using NPAutoBooth.UI.BoothUC;
using NPCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{

    public partial class FormCreditInfomation : Form, ISubForm
    {

        /// <summary>
        /// 이전폼에서 활성화 되있떤 폼타입
        /// </summary>
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        /// <summary>
        /// 현재폼에서 활성화 해야하는 폼타입
        /// </summary>
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Info;

        private InformationUC InfoControl;

        //카드/할인권 투입정보 화면 종료시간 적용
        public int formCloseTime = 10;
        public int openTime = 0;
        //카드/할인권 투입정보 화면 종료시간 적용완료

        public event ChangeView EventExitInfoForm_EventNextForm;
        public event EventHandlerAddCtrl OnAddCtrl;

        private InfoStatus mCurrentStatus = InfoStatus.Commuter;

        #region 폼생성관련
        public FormCreditInfomation()
        {
            try
            {
                InitializeComponent();
                this.Location = new Point(0, 0);
                
                if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
                {
                    this.ClientSize = new System.Drawing.Size(1024, 768);
                    InfoControl = FormFactory.GetInstance().GetDesignControl<InformationUC>(BoothCommonLib.ClientAreaRate._4vs3);
                }
                else if ( NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
                {
                    this.ClientSize = new System.Drawing.Size(1080, 1920);
                    InfoControl = FormFactory.GetInstance().GetDesignControl<InformationUC>(BoothCommonLib.ClientAreaRate._9vs16);
                }
                //1080해상도 적용 완료

                this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|FormInfomation", ex.ToString());
            }
        }

        public void SetCurrentStatus(InfoStatus pInfoStatus)
        {
            mCurrentStatus = pInfoStatus;
        }

        #endregion

        #region 동영상 관련 이벤트처리
        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|Player_MediaError", "플레이어오류:" + e.ToString());

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|Player_MediaError", ex.ToString());
            }
        }

        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {
                // Get the description of the first error. 
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|player_ErrorEvent", "에러내용" + errDesc);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|player_ErrorEvent", ex.ToString());
            }
        }


        void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsStopped)
                {

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|Player_PlayStateChange", ex.ToString());
            }
        }


        #endregion

        #region 폼활성화 / 종료시 이벤트 
        public void SettingEnableEvent()
        {
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent += new EventHandler(player_ErrorEvent);
        }

        public void SettingDisableEvent()
        {
            axWindowsMediaPlayer1.PlayStateChange -= new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            axWindowsMediaPlayer1.MediaError -= new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent -= new EventHandler(player_ErrorEvent);
        }
        #endregion

        #region 폼 활성화 시작/폼 활성화 종료시 호출함수

        /// <summary>
        /// 화면을 새로 띄울떄 또는 이전등으로 띄울떄 모두 사용한다
        /// </summary>
        /// <param name="pFormType"></param>
        /// <param name="pNormalCarInfo">NULL이면 다음단계에서 이전단계로 폼이동시 사용</param>
        public void OpenView<T>(NPSYS.FormType pFormType, T param)
        {
            if (this.Visible == false)
            {
                NPSYS.CurrentFormType = mCurrentFormType;
                InfoStatus infoStatus = param.To<InfoStatus>();

                OpenSetControl();
                mPreFomrType = pFormType;
                mCurrentStatus = infoStatus;
                SetLanuageDynamic(infoStatus, NPSYS.CurrentLanguageType);
                playVideo();
                formCloseTime = 10;
                openTime = 0;
                timerFormClose.Enabled = true;
                timerFormClose.Start();
                this.Show();
                this.Activate();

                TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditInfomation | SetView", "정보창|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }

        public void CloseView()
        {
            if (this.Visible)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                this.Hide();
                timerFormClose.Enabled = false;
                timerFormClose.Stop();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditInfomation | CloseView", "장버칭 화면종료됨");
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

        private void OpenSetControl()
        {
            axWindowsMediaPlayer1.SendToBack();
            axWindowsMediaPlayer1.Visible = false;
        }

        #endregion


        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                NPSYS.buttonSoundDingDong();

                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditInfomation | btnOk_Click", "화면보기 종료 버튼누름");
                EventExitInfoForm_EventNextForm(mCurrentFormType, mPreFomrType);
                return;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditInfomation | btnOk_Click", "예외사항:" + ex.ToString());
            }
        }



        private void playVideo()
        {
            try
            {
                switch (mCurrentStatus)
                {
                    case InfoStatus.CompletCar:
                        break;
                    case InfoStatus.NotCarSearch:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\차량번호없음.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NotCarSearch.ToString());

                        break;
                    case InfoStatus.SeviceCar:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\서비스기간.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_SeviceCar.ToString());
                        break;
                    case InfoStatus.Commuter:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\정기권차량.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_Commuter.ToString());
                        break;
                    case InfoStatus.KakaoMember:
                        break;
                    case InfoStatus.NoRegExtensCar:
                    case InfoStatus.NoRegExtensDiscount:
                    break;

                    case InfoStatus.AddPayment:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\추가요금발생.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_AddPayment.ToString());
                        break;
                    case InfoStatus.NotEnoghfMoney:
                       // axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\보관증.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NotEnoghfMoney.ToString());
                        break;


                    case InfoStatus.CorrectCard:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\카드투입방향.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_CorrectCard.ToString());
                        break;

                    case InfoStatus.NotOutCard:
                        // axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\카드배출안됨.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NotOutCard.ToString());
                        break;

                    case InfoStatus.NotCardPay:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\카드유효하지않음.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NotCardPay.ToString());
                        break;



                    case InfoStatus.NoDiscountTicket:
                       // axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\유효하지않은할인권.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NoDiscountTicket.ToString());
                        break;

                    case InfoStatus.DuplicateDiscountTicket:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\중복할인권중지.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DuplicateDiscountTicket.ToString());

                        break;
                    case InfoStatus.NoAddDiscountTIcket:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\할인권제한수량중지.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NoAddDiscountTIcket.ToString());
                        break;
                    case InfoStatus.NoBarcode:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\유효하지않은할인권.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NoBarcode.ToString());
                        break;
                    case InfoStatus.DuplicateBarcode:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\중복할인권중지.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_DuplicateBarcode.ToString());

                        break;

                    case InfoStatus.ExitBooth:
                        break;
                    default:
                        //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\카드투입방향.avi";
                        axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_NoDiscountTicket.ToString());
                        break;
                }
                axWindowsMediaPlayer1.uiMode = "none";

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|playVideo", ex.ToString());
            }

        }

        private void FormCreditInfomation_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditInfomation | FormDeviiceErrorMessagePage_FormClosed", "[인포메이션폼 완전종료]");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormInfomation|FormDeviiceErrorMessagePage_FormClosed", ex.ToString());
            }
        }

        private void FormCreditInfomation_Shown(object sender, EventArgs e)
        {
         

        }


        private void timerFormClose_Tick(object sender, EventArgs e)
        {
            if (formCloseTime > openTime)
            {
                openTime += 1;
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormInfomation | timerFormClose_Tick", "폼종료타임을 초과하여 메지시창 종료");
                EventExitInfoForm_EventNextForm(mCurrentFormType, mPreFomrType);
                timerFormClose.Stop();
                return;
            }
        }

        private void FormCreditInfomation_Load(object sender, EventArgs e)
        {
            TextCore.INFO(TextCore.INFOS.MEMORY, "FormInfomation | FormCreditInfomation_Load ", "정보화면로드|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            Invoke(OnAddCtrl); //컨트롤 추가
            //2020.01.22 이재영 : 신규 디자인 이벤트 설정
            InfoControl.PreForm_Click += btnOk_Click;
            //신규 디자인 이벤트 설정 완료

            SetLanguage(NPSYS.CurrentLanguageType);
            SettingEnableEvent();

        }

        void AddCtrl()
        {
            //2020.01.22 이재영 : 신규 디자인 적용
            this.Controls.Add((Control)InfoControl);
            InfoControl.Dock = DockStyle.Fill;
            InfoControl.BringToFront();
            //신규 디자인 적용 완료
        }

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
        }

        private void SetLanuageDynamic(InfoStatus p_CurrentStatus, NPCommon.ConfigID.LanguageType pLanguageType)
        {
            switch (p_CurrentStatus)
            {
                case InfoStatus.CompletCar:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_COMPLETCAR.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NotCarSearch:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOTCARSEARCH.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.SeviceCar:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_SERVICECAR.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.Commuter:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_COMPLETCAR.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.KakaoMember:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_KAKAOMEMBER.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.NoRegExtensCar:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOREGEXTENSCAR.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.NoRegExtensDiscount:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOREGEXTENSDISCOUNT.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;


                case InfoStatus.AddPayment:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_ADDPAYMENT1.ToString());
                    if (InfoControl.SecondMessageVisible == false) InfoControl.SecondMessageVisible = true;
                    InfoControl.SecondMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_ADDPAYMENT2.ToString());
                    break;

                case InfoStatus.NotEnoghfMoney:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOTENOGHFMONEY1.ToString());
                    if (InfoControl.SecondMessageVisible == false) InfoControl.SecondMessageVisible = true;
                    InfoControl.SecondMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOTENOGHFMONEY2.ToString());
                    break;

                case InfoStatus.CorrectCard:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_CORRECTCARD.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NotOutCard:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOTOUTCARD.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NotCardPay:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOTCARDPAY.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NoDiscountTicket:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NODISCOUNTTICEKT.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NoAddDiscountTIcket:
                case InfoStatus.DuplicateDiscountTicket:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOADDDISCOUNTTICKET.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;

                case InfoStatus.NoBarcode:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_NOBARCODE.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.DuplicateBarcode:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_DUPLICATEBARCODE.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
                case InfoStatus.ExitBooth:
                    InfoControl.FirstMessage = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_EXITBOOTH.ToString());
                    InfoControl.SecondMessageVisible = false;
                    break;
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


    }
}
