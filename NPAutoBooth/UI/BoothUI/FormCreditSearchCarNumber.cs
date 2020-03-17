using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Database;
using NPAutoBooth.Common;
using FadeFox.Text;
using FadeFox;
using System.Threading;
using FadeFox.UI;
using NPCommon.REST;
using NPCommon;
using NPCommon.DTO.Receive;
using NPAutoBooth.UI.BoothUC;

namespace NPAutoBooth.UI
{
    public partial class FormCreditSearchCarNumber : Form, ISubForm
    {
        private string mCarInputNumber = "";

        /// <summary>
        /// 이전폼에서 활성화 되있떤 폼타입
        /// </summary>
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        /// <summary>
        /// 현재폼에서 활성화 해야하는 폼타입
        /// </summary>
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Search;

        private Car mSearchCarInfo = new Car();
        private HttpProcess mHttpProcess = new HttpProcess();
        public event EventHandlerAddCtrl OnAddCtrl;
        private SearchCarUC searchCarControl;

        #region 폼이동이벤트
        //public delegate void mExitSerachForm(NPSYS.FormType pCurrentFormType);
        ///// <summary>
        ///// 고객이 처음으로 / 시간초과 등으로 종료시
        ///// </summary>
        //public event mExitSerachForm EventExitSerachForm;


        //public delegate void mExitSearchForm_NextSelectForm(NPSYS.FormType pCurrentFormType, ParkingReceiveData.carList pNormalCarInfo);
        ///// <summary>
        ///// 고객이 차량을 찾아 다음 차량선택으로 넘어가야할시 이벤트
        ///// </summary>
        //public event mExitSearchForm_NextSelectForm EventExitSearchForm_NextSelectForm;


        //public delegate void mExitSearchForm_NextSearchTimeForm(NPSYS.FormType pCurrentFormType);
        ///// <summary>
        ///// 고객이 차량을 찾아 다음 차량선택으로 넘어가야할시 이벤트
        ///// </summary>
        //public event mExitSearchForm_NextSearchTimeForm EventExitSearchForm_NextSearchTimeForm;

        //public delegate void mExitSearchForm_NextInfo(NPSYS.FormType pCurrentFormType, InfoStatus pInfoStatus);
        //public event mExitSearchForm_NextInfo EventExitSearchForm_NextInfo;

        //public delegate void mLanguageChange(NPCommon.ConfigID.LanguageType pLanguageType);
        //public event mLanguageChange EventLanguageChange;


        public event LanguageChange EventLanguageChange;
        public event ChangeView EventExitSerachForm;
        public event ChangeView<CarList> EventExitSearchForm_NextSelectForm;
        public event ChangeView<InfoStatus> EventExitSearchForm_NextInfo;

        #endregion

        #region 폼생성관련
        public FormCreditSearchCarNumber()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.DoubleBuffer |
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint,
                    true);
            this.UpdateStyles();

            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024)
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
                searchCarControl = FormFactory.GetInstance().GetDesignControl<SearchCarUC>(BoothCommonLib.ClientAreaRate._4vs3);
            }
            //1080해상도 적용
            else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                this.ClientSize = new System.Drawing.Size(1080, 1920);
                searchCarControl = FormFactory.GetInstance().GetDesignControl<SearchCarUC>(BoothCommonLib.ClientAreaRate._9vs16);
            }
            //1080해상도 적용 완료

            this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);
        }

        private void FormSearchCarNumber_Load(object sender, EventArgs e)
        {
            Invoke(OnAddCtrl); //컨트롤 추가

            this.Location = new Point(0, 0);
            SetControl();
            SetLanguage(NPSYS.CurrentLanguageType);


            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                axWindowsMediaPlayer1.SendToBack();
            }
            SettingEnableEvent();
        }

        void AddCtrl()
        {
            this.Controls.Add((Control)searchCarControl);
            searchCarControl.Dock = DockStyle.Fill;
            searchCarControl.BringToFront();
            searchCarControl.Initialize();
        }

        private void FormSearchCarNumber_Shown(object sender, EventArgs e)
        {


        }
        private void SetControl()
        {
            //Event 설정
            searchCarControl.ConfigCall += MainControl_ConfigCall;
            searchCarControl.Confirm_Click += btnConfirm_Click;
            searchCarControl.BackNumber_Click += btnBackNumber_Click;
            searchCarControl.Home_Click += btn_TXT_HOME_Click;
            searchCarControl.PreForm_Click += btn_home_Click;
            searchCarControl.Number_Click += SearchCarControl_Number_Click;
            searchCarControl.LanguageChange_Click += SearchCarControl_LanguageChange_Click;

            //picWait 화면을 부모폼 중앙에 위치시켜야 겠다.
            //picWait의 Location 위치를 잡자. 잡는 방법은
            //부모폼의 Location은 0,0 이므로 부모폼의 Width 에서 picWait의 Width를 뺀 값의 반이 X좌표
            //Y좌표 또한 X좌표 구하는 방법과 동일하다.
            picWait.Location = new Point
            {
                X = (this.Width - picWait.Width) / 2,
                Y = (this.Height - picWait.Height) / 2,
            };
        }
        #endregion

        #region 동영상 관련 이벤트처리
        void Player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {

            if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsStopped)
            {
                MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
            }
        }

        void Player_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|Player_MediaError", "플레이어오류:" + e.ToString());

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
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|player_ErrorEvent", ex.ToString());
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
                Car car = param.To<Car>();
                NPSYS.CurrentFormType = mCurrentFormType;
                picWait.SendToBack();
                inputtime = NPSYS.SettingCarSearchTimeValue;
                MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                OpenSetControl();
                mPreFomrType = pFormType;
                playVideo();
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;

                if (car != null) // 새로폼을 생성할때면
                {
                    mSearchCarInfo = CommonFuction.Clone<Car>(car);
                    car = null;
                }

                this.TopMost = true;
                this.Show();
                this.Activate();
                TextCore.INFO(TextCore.INFOS.MEMORY, "FormSearchCarNumber | OpenView", "차량검색 화면로드됨|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }

        /// <summary>
        /// 화면을 비활성활시 사용한다
        /// </summary>
        public void CloseView()
        {
            if (this.Visible)
            {
                StopVideo();
                inputTimer.Enabled = false;
                MovieTimer.Enabled = false;
                //TMAP연동
                TimerTmpaWait.Enabled = false;
                //TMAP연동완료
                this.TopMost = false;
                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormSearchCarNumber | CloseView", "차량검색 화면종료됨");
            }
        }

        public void CloseViewBeforeInfo()
        {
            if (this.Visible)
            {
                StopVideo();
                inputTimer.Enabled = false;
                MovieTimer.Enabled = false;
                //TMAP연동
                TimerTmpaWait.Enabled = false;
                //TMAP연동완료
                this.TopMost = false;
                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormSearchCarNumber | CloseViewBeforeInfo", "차량검색 화면종료(인포메이션폼을위해)");
            }
        }
        public void OpenViewBeforeInfo(NPSYS.FormType pFormType)
        {
            if (this.Visible == false)
            {
                NPSYS.CurrentFormType = mCurrentFormType;
                inputtime = NPSYS.SettingCarSearchTimeValue;
                picWait.SendToBack();
                MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                OpenSetControl();
                mPreFomrType = pFormType;
                playVideo();
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                this.TopMost = true;
                this.Show();
                this.Activate();
                TextCore.INFO(TextCore.INFOS.MEMORY, "FormSearchCarNumber | OpenViewBeforeInfo", "차량검색 다시로드됨|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }

        private void StopVideo()
        {
            try
            {

                axWindowsMediaPlayer1.Ctlcontrols.stop();

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber | StopVideo", "예외사항:" + ex.ToString());
            }
        }
        
        private void OpenSetControl()
        {
            clearLableText();
            mCarInputNumber = "";
            searchCarControl?.OpenSetControl();
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

        #region User Control Event Handler

        private void MainControl_ConfigCall()
        {
            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber | panel_ConfigClick2_Click", "유저가 강제로 화면종료");
            EventExitSerachForm(mCurrentFormType);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            GetSearch(mCarInputNumber, true);
        }

        private void GetSearch(string carnumber, bool pEnter = false)
        {
            try
            {
                bool isSearchCar = false;

                if (pEnter) // 엔터키를 누른경우
                {
                    if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit5Free && mCarInputNumber.Length > 0)
                    {
                        isSearchCar = true;
                    }
                    else if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetEnt && mCarInputNumber.Length == 4)
                    {
                        isSearchCar = true;
                    }
                }
                else // 엔터키를 누르지않은경우
                {
                    if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO && carnumber.Length >= 4)
                    {
                        isSearchCar = true;
                    }
                }

                if (isSearchCar)
                {

                    if ((NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO || NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetEnt) && carnumber == "0000")
                    {
                        mCarInputNumber = "";
                        clearLableText();
                        EventExitSearchForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                        return;
                    }

                    picWait.BringToFront();
                    picWait.Visible = true;

                    Application.DoEvents(); //차량번호 마지막 자리 숫자 표시되는 시간을 딜레이 시키지 않기 위해...

                    System.Threading.Thread.Sleep(300);

                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|GetSerach|차량번호4자리조회", "차량번호4자리조회:" + carnumber);

                    //TMAP연동
                    if (NPSYS.gUseTmap)
                    {
                        CarList currentCarList = mHttpProcess.GetSearch4NumberTmap(carnumber);
                        TimerTmapInt = 10;
                        TimerTmpaWait.Enabled = true;
                        return;
                    }
                    //TMAP연동완료

                    CarList currentLiistCar = mHttpProcess.GetSearch4Number(carnumber);

                    if (currentLiistCar.status.Success == true)
                        EventExitSearchForm_NextSelectForm(mCurrentFormType, NPSYS.FormType.Select, currentLiistCar);
                    else // 차량 없을떄 화면표출
                        EventExitSearchForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);

                    picWait.Visible = false;
                    picWait.SendToBack();

                    return;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|GetSerach", "예외사항:" + ex.ToString());
            }
        }

        private void SearchCarControl_Number_Click(string number)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }
            mCarInputNumber += number;
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnOne_Click", $"{number} 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void SearchCarControl_LanguageChange_Click(BoothCommonLib.LanguageType languageType)
        {
            switch (languageType)
            {
                case BoothCommonLib.LanguageType.ENGLISH:
                    EventLanguageChange(NPCommon.ConfigID.LanguageType.ENGLISH);
                    break;
                case BoothCommonLib.LanguageType.JAPAN:
                    EventLanguageChange(NPCommon.ConfigID.LanguageType.JAPAN);
                    break;
                default:
                    EventLanguageChange(NPCommon.ConfigID.LanguageType.KOREAN);
                    break;
            }
        }

        #endregion

        private void clearLableText()
        {
            if (searchCarControl != null)
            {
                searchCarControl.SearchCarTextBar.SetOneText = string.Empty;
                searchCarControl.SearchCarTextBar.SetTwoText = string.Empty; ;
                searchCarControl.SearchCarTextBar.SetThreeText = string.Empty;
                searchCarControl.SearchCarTextBar.SetFourText = string.Empty;
                searchCarControl.SearchCarTextBar.SetFiveText = string.Empty;
            }
        }
  

        private void playVideo()
        {
            try
            {
                //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Movie\동영상-고객님의차량번호4자리만.avi";
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_SearchCarNumber.ToString());
                axWindowsMediaPlayer1.uiMode = "none";
                axWindowsMediaPlayer1.Ctlcontrols.play();

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|playVideo", "예외사항:" + ex.ToString());
            }
        }

        private void lblCarNumberInsert()
        {

            NPSYS.buttonSoundDingDong();

            clearLableText();

            for (int i = 0; i < mCarInputNumber.Length; i++)
            {
                if (i == 0)
                {
                    searchCarControl.SearchCarTextBar.SetOneText = mCarInputNumber.Substring(i, 1);
                }
                if (i == 1)
                {
                    searchCarControl.SearchCarTextBar.SetTwoText = mCarInputNumber.Substring(i, 1);

                }
                if (i == 2)
                {
                    searchCarControl.SearchCarTextBar.SetThreeText = mCarInputNumber.Substring(i, 1);
                }

                if (i == 3)
                {
                    searchCarControl.SearchCarTextBar.SetFourText = mCarInputNumber.Substring(i, 1);
                }
                if (i == 4)
                {
                    searchCarControl.SearchCarTextBar.SetFiveText = mCarInputNumber.Substring(i, 1);
                }
            }
        }

        private bool IsNextButtonConfirm()
        {
            if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetAUTO || NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit4SetEnt)
            {
                if (mCarInputNumber.Length >= 4)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber| IsNextButtonConfirm", "4자리이상 누르기막기");
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else if (NPSYS.CurrentCarNumType == NPCommon.ConfigID.CarNumberType.Digit5Free)
            {
                if (mCarInputNumber.Length >= 5)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber| IsNextButtonConfirm", "5자리이상 누르기막기");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private void btnOne_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }
            mCarInputNumber += "1";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnOne_Click", "1 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }
        private void btnTwo_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "2";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnTwo_Click", "2 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "3";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnThree_Click", "3 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnFour_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "4";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnFour_Click", "4 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnFive_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "5";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnFive_Click", "5 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnSix_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "6";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnSix_Click", "6 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnSeven_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }

            mCarInputNumber += "7";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnSix_Click", "7 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnEight_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }
            mCarInputNumber += "8";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnEight_Click", "8 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnNine_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }
            mCarInputNumber += "9";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnNine_Click", "9 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            if (IsNextButtonConfirm() == false)
            {
                return;
            }
            mCarInputNumber += "0";
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnZero_Click", "0 누름");
            lblCarNumberInsert();
            GetSearch(mCarInputNumber);
        }

        private void btnBackNumber_Click(object sender, EventArgs e)
        {
            ActionKeyBackNumber();
        }

        private void ActionKeyBackNumber()
        {
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btnBackNumber_Click", "한칸 지우기 누름");
            if (mCarInputNumber.Length > 0)
            {
                if (mCarInputNumber.Length == 1)
                {
                    mCarInputNumber = string.Empty;
                }
                else
                {
                    mCarInputNumber = mCarInputNumber.Substring(0, mCarInputNumber.Length - 1);
                }
            }
            lblCarNumberInsert();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            GetSearch(mCarInputNumber, true);
        }

        private void ActionKeyClearCarNumber()
        {
           // GetSerach(mCarInputNumber,true);
        }

        private void FormSearchCarNumber_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //SettingDisableEvent();
                //axWindowsMediaPlayer1.Ctlcontrols.stop();
                //axWindowsMediaPlayer1.close();
                //axWindowsMediaPlayer1.Dispose();
                //NPSYS.CurrentFormType = mPreFomrType;

                //this.Close();
                //if (this != null)
                //{
                //    this.Dispose();
                //}
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormSearchCarNumber|FormSearchCarNumber_FormClosed", "차량검색 폼종료");

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|FormSearchCarNumber_FormClosed", ex.ToString());
            }
        }
  

        private void btn_home_Click(object sender, EventArgs e)
        {
            try
            {
                NPSYS.buttonSoundDingDong();

                TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|btn_home_Click", "처음으로 버튼누름");
                EventExitSerachForm(mCurrentFormType);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|Receive", "예외사항:" + ex.ToString());
            }
        }

        private void PausePlayVideo()
        {
            try
            {
                inputTimer.Enabled = false;
                inputtime = NPSYS.SettingCarSearchTimeValue;
                MovieTimer.Enabled = false;
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                IsNextFormPlaying = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|PausePlayVideo", "예외사항:" + ex.ToString());
            }
        }

        private void StartPlayVideo()
        {
            try
            {
                 axWindowsMediaPlayer1.Ctlcontrols.play();
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                IsNextFormPlaying = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|StartPlayVideo", "예외사항:" + ex.ToString());
            }
        }


        int inputtime = NPSYS.SettingCarSearchTimeValue;
        private void inputTimer_Tick(object sender, EventArgs e)
        {
            if (inputtime < 0)
            {
 
               TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber|inputTimer_Tick", "입력이 오랜동안 들어오지 않아 처음으로 이동");
                EventExitSerachForm(mCurrentFormType);
            }
            inputtime -= 3000;
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
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSearchCarNumber|MovieTimer_Tick", "예외사항:" + ex.ToString());
            }
        }

        private void applicationDoevent(int p_time)
        {
            int i = 0;
            while (true)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                i++;
                if (i > p_time)
                {
                    break;
                }
            }
        }

        private void btnEnglish_Click(object sender, EventArgs e)
        {
            EventLanguageChange(NPCommon.ConfigID.LanguageType.ENGLISH);
        }

        private void btnJapan_Click(object sender, EventArgs e)
        {
            EventLanguageChange(NPCommon.ConfigID.LanguageType.JAPAN);
        }

        private void btn_TXT_HOME_Click(object sender, EventArgs e)
        {
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSearchCarNumber | btn_TXT_HOME_Click", "처음으로 버튼누름");
            EventExitSerachForm(mCurrentFormType);
            return;
        }

        //TMAP연동
        private int TimerTmapInt = 10;
        private void TimerTmpaWait_Tick(object sender, EventArgs e)
        {
            TimerTmapInt -= 1;
            if (TimerTmapInt <= 0)
            {
                picWait.SendToBack();
                picWait.Visible = false;
                TimerTmpaWait.Enabled = false;
                mCarInputNumber = "";
                clearLableText();
                EventExitSearchForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
            }
        }

        public void SetCurrentCarList(CarList pCarList)
        {
            TimerTmapInt = 10;
            TimerTmpaWait.Enabled = false;
            if (pCarList.status.Success == true)
            {
                picWait.Visible = false;
                picWait.SendToBack();
                EventExitSearchForm_NextSelectForm(mCurrentFormType, NPSYS.FormType.Select, pCarList);
                return;
            }
            else
            {
                EventExitSearchForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                return;
                // 차량 없을떄 화면표출
            }
        }
        //TMAP연동완료

    }
}
