using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using FadeFox.Database;
using System.Windows.Forms;
using NPAutoBooth.Common;
using System.Net;
using FadeFox.Text;
using FadeFox.UI;
using NPCommon.DTO;
using NPCommon;
using NPCommon.DTO.Receive;
using NPCommon.REST;
using NPAutoBooth.UI.BoothUC;

namespace NPAutoBooth.UI
{
    public partial class FormCreditSelectCarnumber : Form, ISubForm
    {
        /// <summary>
        /// 현재폼의 폼상태명 이곳은 차량선택
        /// </summary>
        
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.Select;
        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private PagingData mPages = null;
        private Car[] arrCarInfo;

        //TMAP연동
        private NormalCarInfo mSelectCarInfo = new NormalCarInfo();
        private int TimerTmapInt = 10;
        //TMAP연동완료

        private HttpProcess mHttpProcess = new HttpProcess();

        private bool mIsPlayerOkStatus = true;
        private int mCurrentCarListCount = 0;

        private SelectCarUC selectCarControl;

        /// <summary>
        /// 고객이 처음으로 등을 눌러 종료시
        /// </summary>
        public event ChangeView EventExitSelectForm;
        /// <summary>
        /// 고객이 차량을 찾아 다음 요금으로 넘어가야할시 이벤트(or 영수증 화면 이동)
        /// </summary>
        public event ChangeView<NormalCarInfo> EventExitSelectForm_CarInfo;
        public event ChangeView<InfoStatus> EventExitSelectForm_NextInfo;
        public event LanguageChange EventLanguageChange;
        public event EventHandlerAddCtrl OnAddCtrl;


        #region 폼생성 관련
        public FormCreditSelectCarnumber()
        {
            InitializeComponent();

            if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
            {
                this.ClientSize = new System.Drawing.Size(1024, 768);
                selectCarControl = FormFactory.GetInstance().GetDesignControl<SelectCarUC>(BoothCommonLib.ClientAreaRate._4vs3);
            }
            //1080해상도 적용
            else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
            {
                this.ClientSize = new System.Drawing.Size(1080, 1920);
                selectCarControl = FormFactory.GetInstance().GetDesignControl<SelectCarUC>(BoothCommonLib.ClientAreaRate._9vs16);
            }

            this.OnAddCtrl += new EventHandlerAddCtrl(AddCtrl);
        }

        private void FormSelectCarnumber_Load(object sender, EventArgs e)
        {
            Invoke(OnAddCtrl); //컨트롤 추가
            //1080해상도 적용 완료
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
            this.Controls.Add((Control)selectCarControl);
            selectCarControl.Dock = DockStyle.Fill;
            selectCarControl.BringToFront();
            selectCarControl.Initialize();
        }

        private void FormSelectCarnumber_Shown(object sender, EventArgs e)
        {


        }

        private void SetControl()
        {
            //Event 설정
            selectCarControl.ConfigCall += CallControlConfig;
            selectCarControl.NextPage_Click += btnNextPage_Click;
            selectCarControl.PrePage_Click += btnPrevPage_Click;
            selectCarControl.Home_Click += btn_TXT_HOME_Click;
            selectCarControl.PreForm_Click += btn_PrePage_Click;
            selectCarControl.Car_Selected += CarSelected_Click;
            selectCarControl.LanguageChange_Click += SearchCarControl_LanguageChange_Click;

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

        private void CarSelected_Click(int index)
        {
            NPSYS.buttonSoundDingDong();
            string logControlMessage;
            string logCarNumberMessage;

            picWait.Visible = true;
            picWait.BringToFront();

            if (index == 1)
            {
                if (selectCarControl.SelectedCarNumber.Trim() == "") return;

                selectCarControl.SetCarInfoColor(SelectCarUC.ENUM_CarIndex.One, Color.Blue);
                logControlMessage = "oneButtonClick";
                logCarNumberMessage = selectCarControl.SelectedCarNumber;
            }
            else if (index == 2)
            {
                if (selectCarControl.SelectedCarNumber.Trim() == "") return;

                selectCarControl.SetCarInfoColor(SelectCarUC.ENUM_CarIndex.Two, Color.Blue);
                logControlMessage = "twoButtonClick";
                logCarNumberMessage = selectCarControl.SelectedCarNumber;
            }
            else if (index == 3)
            {
                if (selectCarControl.SelectedCarNumber.Trim() == "") return;

                selectCarControl.SetCarInfoColor(SelectCarUC.ENUM_CarIndex.Three, Color.Blue);
                logControlMessage = "threeButtonClick";
                logCarNumberMessage = selectCarControl.SelectedCarNumber;
            }
            else if (index == 4)
            {
                if (selectCarControl.SelectedCarNumber.Trim() == "") return;

                selectCarControl.SetCarInfoColor(SelectCarUC.ENUM_CarIndex.Four, Color.Blue);
                logControlMessage = "fourButtonClick";
                logCarNumberMessage = selectCarControl.SelectedCarNumber;
            }
            else
            {
                picWait.Visible = false;
                picWait.SendToBack();
                return;
            }

            TextCore.ACTION(TextCore.ACTIONS.USER
                , $"FormSelectCarnumber|{logControlMessage}"
                , $"{index.ToString()}번째 사진 선택:{logCarNumberMessage}");  //뉴타입주석
            ControlEnable(false);
            TextCore.INFO(TextCore.INFOS.CARSEARCH
                , $"FormSelectCarnumber|{logControlMessage} "
                , $"차량번호:{logCarNumberMessage} 정기권여부:{arrCarInfo[index - 1].parkingType.ToString()}");  //뉴타입주석
            SelectdCarumber(arrCarInfo[index - 1]);
            ControlEnable(true);
        }

        private void CallControlConfig()
        {
            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber | panel_ConfigClick2_Click", "입력이 오랜동안 들어오지 않아 처음으로 이동");
            EventExitSelectForm(mCurrentFormType, NPSYS.FormType.NONE);
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
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber|Player_MediaError", "플레이어오류:" + e.ToString());
                mIsPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber|Player_MediaError", ex.ToString());
            }
        }

        void player_ErrorEvent(object sender, System.EventArgs e)
        {
            try
            {
                // Get the description of the first error. 
                string errDesc = axWindowsMediaPlayer1.Error.get_Item(0).errorDescription;

                // Display the error description.
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber | player_ErrorEvent", "에러내용" + errDesc);
                mIsPlayerOkStatus = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber |  player_ErrorEvent", ex.ToString());
            }
        }
        #endregion

        #region 폼활성화 / 종료시 이벤트 
        private void SettingEnableEvent()
        {
            axWindowsMediaPlayer1.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            axWindowsMediaPlayer1.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent += new EventHandler(player_ErrorEvent);

        }

        private void SettingDisableEvent()
        {
            axWindowsMediaPlayer1.PlayStateChange -= new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            axWindowsMediaPlayer1.MediaError -= new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
            axWindowsMediaPlayer1.ErrorEvent -= new EventHandler(player_ErrorEvent);
        }
        #endregion

        #region 폼 활성화 시작/폼 활성화 종료시 호출함수
        public void OpenView<T>(NPSYS.FormType pFormType, T param)
        {
            if (this.Visible == false)
            {
                CarList carList = param.To<CarList>();
                NPSYS.CurrentFormType = mCurrentFormType;
                inputtime = NPSYS.SettingCarSelectTimeValue;
                MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;
                OpenSetControl();
                mPreFomrType = pFormType;
                if (param != null)
                {
                    if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.AB_1024 || NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.PB_1024)
                    {
                        mPages = new PagingData(4, 1);
                    }
                    else if (NPSYS.CurrentBoothType == NPCommon.ConfigID.BoothType.NOMIVIEWPB_1080)
                    {
                        mPages = new PagingData(3, 1);
                    }
                    mPages.DataSource = carList;
                    PageView(1);

                }
                else
                {
                    PageView(1);
                }
                playVideo();
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                this.TopMost = true;
                this.Show();
                this.Activate();

                TextCore.INFO(TextCore.INFOS.MEMORY, "FormSelectCarnumber | OpenView", "차량선택화면로드됨|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }

        public void CloseView()
        {
            if (this.Visible == true)
            {
                StopVideo();
                inputTimer.Enabled = false;
                MovieTimer.Enabled = false;
                this.Hide();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormSelectCarnumber | CloseView", "차량선택화면 화면종료됨");
            }
        }

        public void CloseViewBeforeInfo()
        {
            if (this.Visible == true)
            {
                StopVideo();
                inputTimer.Enabled = false;
                MovieTimer.Enabled = false;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormSelectCarnumber | CloseViewBeforeInfo", "차량선택화면(메인화면으로 이동처리)");
                this.Hide();
            }
        }
        public void OpenViewBeforeInfo(NPSYS.FormType pFormType)
        {
            if (this.Visible == false)
            {
                NPSYS.CurrentFormType = mCurrentFormType;
                inputtime = NPSYS.SettingCarSelectTimeValue;
                MovieStopPlay = NPSYS.SettingMoviePlayTimeValue;

                mPreFomrType = pFormType;
                playVideo();
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                this.TopMost = true;
                this.Show();
                this.Activate(); ;
                TextCore.INFO(TextCore.INFOS.MEMORY, "FormSelectCarnumber | OpenView", "차량선택화면로드됨|사용메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
            }
        }


        private void OpenSetControl()
        {
            selectCarControl?.Clear();
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

        private void playVideo()
        {
            try
            {
                //axWindowsMediaPlayer1.URL = Application.StartupPath + @"\Movie\동영상-차량선택.avi";
                axWindowsMediaPlayer1.URL = Application.StartupPath + @"\MOVIE\" + NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_WAV_SelectCarNumber.ToString());
                axWindowsMediaPlayer1.uiMode = "none";
                if (mIsPlayerOkStatus)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
            catch (Exception ex)
            {

                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditSelectCarnumber | playVideo", ex.ToString());
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
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditSelectCarnumber | StopVideo", "예외사항:" + ex.ToString());
            }
        }

        public void PageView(int pPage)
        {
            CarList listNormalCarInfo = mPages[pPage];
            arrCarInfo = listNormalCarInfo.carDataList.ToArray();

            selectCarControl.CurrentPage = mPages.CurrentPage.ToString();
            selectCarControl.TotalPage = mPages.PageCount.ToString();
            selectCarControl.PrevPageEnable = mPages.PrevBlockExist;
            selectCarControl.NextPageEnable = mPages.NextBlockExist;

            selectCarControl.Clear();

            for (int i = 0; i < listNormalCarInfo.carDataList.Count; i++)
            {
                if (i == 4) break; //4개까지만....

                SelectCarUC.SelectCarInfo selectCar = new SelectCarUC.SelectCarInfo();
                selectCar.carNumber = arrCarInfo[i].inCarNo1;

                if (arrCarInfo[i].inImage1 != null && arrCarInfo[i].inImage1.Length > 0)
                {
                    selectCar.carImage = NPSYS.WebImageView(arrCarInfo[i].inImage1);

                }
                if (arrCarInfo[i].inDate > 0)
                {
                    selectCar.carInDateTime = NPSYS.LongTypeToDateTime(arrCarInfo[i].inDate).ToString("yyyy-MM-dd HH:mm");
                }

                switch (i)
                {
                    case 0:
                        selectCarControl.SetCarInfo(SelectCarUC.ENUM_CarIndex.One, selectCar);
                        break;
                    case 1:
                        selectCarControl.SetCarInfo(SelectCarUC.ENUM_CarIndex.Two, selectCar);
                        break;
                    case 2:
                        selectCarControl.SetCarInfo(SelectCarUC.ENUM_CarIndex.Three, selectCar);
                        break;
                    case 3:
                        selectCarControl.SetCarInfo(SelectCarUC.ENUM_CarIndex.Four, selectCar);
                        break;
                }
            }
        }


        private void FormSelectCarnumber_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormSelectCarnumber|FormSelectCarnumber_FormClosed", "차량선택 화면 종료");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber|FormSelectCarnumber_FormClosed", ex.ToString());
            }
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber|btnPrevPage_Click", "이전페이지 선택");
            PageView(mPages.CurrentPage - 1);
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber|btnNextPage_Click", "다음페이지 선택");

            PageView(mPages.CurrentPage + 1);

        }

        private void ControlEnable(bool isConvetrolValue)
        {
            selectCarControl.SetCarInfoEnable(SelectCarUC.ENUM_CarIndex.One, isConvetrolValue);
            selectCarControl.SetCarInfoEnable(SelectCarUC.ENUM_CarIndex.Two, isConvetrolValue);
            selectCarControl.SetCarInfoEnable(SelectCarUC.ENUM_CarIndex.Three, isConvetrolValue);
            selectCarControl.SetCarInfoEnable(SelectCarUC.ENUM_CarIndex.Four, isConvetrolValue);

        }

        //TMAP연동
        
        private void SelectdCarumber(Car p_CarInfoData)
        {
            mSelectCarInfo = new NormalCarInfo();
            mSelectCarInfo.CurrentCar = p_CarInfoData;
            Payment currentPayment = null;
            if (NPSYS.gUseTmap)
            {
                currentPayment = mHttpProcess.GetPaymentTmapFromTkno(p_CarInfoData);
            }
            else
            {
                currentPayment = mHttpProcess.GetPaymentFromTkno(p_CarInfoData); // 출차시 정기권이면 자동차단기 열림

                if (currentPayment.status.Success == false)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditSelectCarnumber | SelectdCarumber", "[차량조회선택오류]" + " 오류코드:" + currentPayment.status.code + " 오류메세지:" + currentPayment.status.message);
                    switch (currentPayment.status.currentStatus)
                    {
                        case Status.BodyStatus.PaySearch_AlreadExit:
                            EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                            break;
                        case Status.BodyStatus.PaySearch_Error:
                            EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                            break;
                        case Status.BodyStatus.PaySearch_NotFound:
                            EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                            break;
                        case Status.BodyStatus.PaySearch_NotIndata:
                            EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                            break;
                        default:
                            EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
                            break;
                    }
                    return;
                }
            }

            if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season
              || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_NotSeason)
            {
                EventExitSelectForm_NextInfo(NPSYS.FormType.Search, NPSYS.FormType.Info, InfoStatus.SeviceCar);
                return;
            }
            else if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_NotSeason)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditSelectCarnumber | SelectdCarumber", "정기권 차량으로 메인폼으로 이동");
                EventExitSelectForm(mCurrentFormType, NPSYS.FormType.NONE);
                return;
            }

            if (NPSYS.gUseTmap)
            {
                TimerTmapInt = 10;
                TimerTmapWait.Enabled = true;
            }
            else
            {
                SetSelectOnlyCar(currentPayment);
            }
        }

        public void SetSelectOnlyCar(Payment pCurrentPayment)
        {
            TimerTmapWait.Enabled = false;
            mSelectCarInfo.CurrentPayment = CommonFuction.Clone<Payment>(pCurrentPayment);
            mSelectCarInfo.SetCurrentPayment(NormalCarInfo.PaymentSetType.NormalPaymnet);
            if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_Hwacha_ // 출구무인결제시 0원인경우
              || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PreDisocunt
              || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_OutCar_PrePay)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormCreditMain | SelectdCarumber", "결제처리 전송시작");
                DateTime paydate = DateTime.Now;
                mSelectCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessOut; //시제설정누락처리

                //카드실패전송
                mSelectCarInfo.PaymentMethod = PaymentType.Free;
                //카드실패전송완료

                Payment currentCar = mHttpProcess.PaySave(mSelectCarInfo, paydate);

                if (currentCar.status.Success == false)
                {
                    // 재전송 처리
                    picWait.Visible = false;
                    picWait.SendToBack();
                    return;
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditSelectCarnumber | SelectdCarumber", "출구무인 0원결제 요청후 메인폼으로 이동");
                EventExitSelectForm(mCurrentFormType);
                picWait.Visible = false;
                picWait.SendToBack();
                return;
            }

            else if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_PreCar_Hwacha // 사전무인결제시 0원인경우
                   || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_PreCar_PreDisocunt
                   )
            {
                if ((mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_PreCar_Hwacha
                  && NPSYS.gPayRuleUseDiscountFreeCarQuestionPay)
                  || (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_PreCar_PreDisocunt
                  && NPSYS.gPayRuleUseDiscountFreeCarQuestionPay))
                {
                    if (NPSYS.gPayRuleUsePrePayConfirm)
                    {
                        DialogResult outMessage = new FormMessagePrePay(FormMessagePrePay.CarType.FreeCar, mSelectCarInfo.FreeTimeAfterPay.ToString()).ShowDialog();
                        if (outMessage == DialogResult.OK)
                        {
                            // 결제완료 처리
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditSelectCarnumber | SelectdCarumber", "차량결제 0원결제 요청");
                            // 요금처리화면으로 화면처리전환
                            DateTime payDaveDate = DateTime.Now;
                            mSelectCarInfo.CurrentMoneyInOutType = NormalCarInfo.MoneyInOutType.SuccessOut; //시제설정누락처리

                            //카드실패전송                                                                   
                            mSelectCarInfo.PaymentMethod = PaymentType.Free;
                            //카드실패전송완료

                            Payment currentCar = mHttpProcess.PaySave(mSelectCarInfo, payDaveDate);

                            if (currentCar.status.Success == false)
                            {
                                // 재전송 처리
                                picWait.Visible = false;
                                picWait.SendToBack();
                                return;
                            }

                            this.EventExitSelectForm_CarInfo(mCurrentFormType, NPSYS.FormType.Receipt, mSelectCarInfo);
                            picWait.Visible = false;
                            picWait.SendToBack();
                            return;
                        }
                    }
                    else
                    {
                        // 결제완료 처리
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditSelectCarnumber | SelectdCarumber", "차량결제 0원결제 요청");
                        // 요금처리화면으로 화면처리전환
                        DateTime payDaveDate = DateTime.Now;

                        //카드실패전송
                        mSelectCarInfo.PaymentMethod = PaymentType.Free;
                        //카드실패전송완료

                        Payment currentCar = mHttpProcess.PaySave(mSelectCarInfo, payDaveDate);

                        if (currentCar.status.Success == false)
                        {
                            // 재전송 처리
                            picWait.Visible = false;
                            picWait.SendToBack();
                            return;
                        }

                        this.EventExitSelectForm_CarInfo(mCurrentFormType, NPSYS.FormType.Receipt, mSelectCarInfo);
                        picWait.Visible = false;
                        picWait.SendToBack();
                        return;
                    }
                }
                else
                {
                    EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.SeviceCar);
                    picWait.Visible = false;
                    picWait.SendToBack();
                    return;
                }

            }
            else if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Free_PreCar_PrePay_RePay)
            {
                EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.CompletCar);
                picWait.Visible = false;
                picWait.SendToBack();
                return;
                // 차량 없을떄 화면표출
            }

            else if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_OutCar_None  // 출구무인요금이 나오는경우
                   || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_OutCar_PreDiscount
                   || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_OutCar_PrePay)
            {
                EventExitSelectForm_CarInfo(mCurrentFormType, NPSYS.FormType.Payment, mSelectCarInfo);
                picWait.Visible = false;
                picWait.SendToBack();
            }
            else if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_PreCar_None
                   || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_PreCar_PreDiscount
                   || mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_PreCar_PrePay_RePay)
            {
                if (mSelectCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.NotFree_PreCar_PrePay_RePay
                 && !NPSYS.gPayRuleUseRePay)
                {
                    EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.ExitBooth);
                    picWait.Visible = false;
                    picWait.SendToBack();
                    return;
                }
                if (NPSYS.gPayRuleUsePrePayConfirm)
                {
                    DialogResult outMessage = new FormMessagePrePay(FormMessagePrePay.CarType.PayCar, mSelectCarInfo.FreeTimeAfterPay.ToString()).ShowDialog();
                    if (outMessage == DialogResult.OK)
                    {
                        EventExitSelectForm_CarInfo(mCurrentFormType, NPSYS.FormType.Payment, mSelectCarInfo);
                        picWait.Visible = false;
                        picWait.SendToBack();
                        return;
                    }
                }
                else
                {
                    EventExitSelectForm_CarInfo(mCurrentFormType, NPSYS.FormType.Payment, mSelectCarInfo);
                    picWait.Visible = false;
                    picWait.SendToBack();
                    return;
                }
            }
        }


        //TMAP연동완료

        int inputtime = NPSYS.SettingCarSelectTimeValue;
        private void inputTimer_Tick(object sender, EventArgs e)
        {
            if (inputtime < 0)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber | inputTimer_Tick", "입력이 오랜동안 들어오지 않아 처음으로 이동");
                EventExitSelectForm(mCurrentFormType);
            }
            inputtime -= 3000;
        }

        int MovieStopPlay = -1000;
        bool IsNextFormPlaying = false;
        private void MovieTimer_Tick(object sender, EventArgs e)
        {
            if (IsNextFormPlaying == false)
            {
                MovieStopPlay -= 1000;
            }
            if (MovieStopPlay == 0 && IsNextFormPlaying == false)
            {
                if (mIsPlayerOkStatus)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
        }




        private void PausePlayVideo()
        {
            try
            {
                inputtime = NPSYS.SettingCarSelectTimeValue;
                inputTimer.Enabled = false;
                MovieTimer.Enabled = false;
                if (mIsPlayerOkStatus == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
                IsNextFormPlaying = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber|PausePlayVideo", ex.ToString());
            }
        }

        private void StartPlayVideo()
        {
            try
            {
                if (mIsPlayerOkStatus == true)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                inputTimer.Enabled = true;
                MovieTimer.Enabled = true;
                IsNextFormPlaying = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormSelectCarnumber|StartPlayVideo", ex.ToString());
            }
        }

        private void btn_PrePage_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber | btn_PrePage_Click", "이전버튼누름");
            EventExitSelectForm(mCurrentFormType, NPSYS.FormType.Search);
        }

       
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
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber | panel_ConfigClick2_Click", "입력이 오랜동안 들어오지 않아 처음으로 이동");
                EventExitSelectForm(mCurrentFormType);
            }
            else
            {
                GoConfigSequence = 0;

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
            TextCore.ACTION(TextCore.ACTIONS.USER, "FormSelectCarnumber | btn_TXT_HOME_Click", "처음으로 버튼누름");
            EventExitSelectForm(mCurrentFormType);
            return;
        }

        //TMAP연동
        private void TimerTmpaWait_Tick(object sender, EventArgs e)
        {
            TimerTmapInt -= 1;
            if (TimerTmapInt <= 0)
            {
                picWait.SendToBack();
                picWait.Visible = false;
                TimerTmapWait.Enabled = false;
                EventExitSelectForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCarSearch);
            }
        }
        //TMAP연동 완료
    }
}
