using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.IO;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using static NPAutoBooth.UI.BoothUC.BoothCommonLib;

namespace NPAutoBooth.UI.BoothUC
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Ctl4by3Payment : PaymentUC
    {
        #region GetControl Logic

        public override T GetControl<T>(ENUM_Control control)
        {
            switch (control)
            {
                case ENUM_Control.btnCardApproval:
                    return (T)Convert.ChangeType(btnCardApproval, typeof(T));

                case ENUM_Control.btnEnglish:
                    return (T)Convert.ChangeType(btnEnglish, typeof(T));

                case ENUM_Control.btnFiveMonthAdd:
                    return (T)Convert.ChangeType(btnFiveMonthAdd, typeof(T));

                case ENUM_Control.btnFourMonthAdd:
                    return (T)Convert.ChangeType(btnFourMonthAdd, typeof(T));

                case ENUM_Control.btnJapan:
                    return (T)Convert.ChangeType(btnJapan, typeof(T));

                case ENUM_Control.btnOneMonthAdd:
                    return (T)Convert.ChangeType(btnOneMonthAdd, typeof(T));

                case ENUM_Control.btnSamsunPay:
                    return (T)Convert.ChangeType(btnSamsunPay, typeof(T));

                case ENUM_Control.btnSixMonthAdd:
                    return (T)Convert.ChangeType(btnSixMonthAdd, typeof(T));

                case ENUM_Control.btnThreeMonthAdd:
                    return (T)Convert.ChangeType(btnThreeMonthAdd, typeof(T));

                case ENUM_Control.btnTwoMonthAdd:
                    return (T)Convert.ChangeType(btnTwoMonthAdd, typeof(T));

                case ENUM_Control.btn_TXT_BACK:
                    return (T)Convert.ChangeType(btn_TXT_BACK, typeof(T));

                case ENUM_Control.btn_TXT_CANCEL:
                    return (T)Convert.ChangeType(btn_TXT_CANCEL, typeof(T));

                case ENUM_Control.btn_TXT_HOME:
                    return (T)Convert.ChangeType(btn_TXT_HOME, typeof(T));

                case ENUM_Control.lblCarnumber2:
                    return (T)Convert.ChangeType(lblCarnumber2, typeof(T));

                case ENUM_Control.lblDiscountCountName:
                    return (T)Convert.ChangeType(lblDiscountCountName, typeof(T));

                case ENUM_Control.lblDiscountInputCount:
                    return (T)Convert.ChangeType(lblDiscountInputCount, typeof(T));

                case ENUM_Control.lblElapsedTime:
                    return (T)Convert.ChangeType(lblElapsedTime, typeof(T));

                case ENUM_Control.lblIndate:
                    return (T)Convert.ChangeType(lblIndate, typeof(T));

                case ENUM_Control.lblParkingFee:
                    return (T)Convert.ChangeType(lblParkingFee, typeof(T));

                case ENUM_Control.lblRegExpireInfo:
                    return (T)Convert.ChangeType(lblRegExpireInfo, typeof(T));

                case ENUM_Control.lbl_CarNumber:
                    return (T)Convert.ChangeType(lbl_CarNumber, typeof(T));

                case ENUM_Control.lbl_CarType:
                    return (T)Convert.ChangeType(lbl_CarType, typeof(T));

                case ENUM_Control.lbl_DIscountMoney:
                    return (T)Convert.ChangeType(lbl_DIscountMoney, typeof(T));

                case ENUM_Control.lbl_MSG_DISCOUNTINFO:
                    return (T)Convert.ChangeType(lbl_MSG_DISCOUNTINFO, typeof(T));

                case ENUM_Control.lbl_MSG_PAYINFO:
                    return (T)Convert.ChangeType(lbl_MSG_PAYINFO, typeof(T));

                case ENUM_Control.lbl_Payment:
                    return (T)Convert.ChangeType(lbl_Payment, typeof(T));

                case ENUM_Control.lbl_RecvMoney:
                    return (T)Convert.ChangeType(lbl_RecvMoney, typeof(T));

                case ENUM_Control.lbl_TXT_AMOUNTFEE:
                    return (T)Convert.ChangeType(lbl_TXT_AMOUNTFEE, typeof(T));

                case ENUM_Control.lbl_TXT_CARNO:
                    return (T)Convert.ChangeType(lbl_TXT_CARNO, typeof(T));

                case ENUM_Control.lbl_TXT_DISCOUNTFEE:
                    return (T)Convert.ChangeType(lbl_TXT_DISCOUNTFEE, typeof(T));

                case ENUM_Control.lbl_TXT_ELAPSEDTIME:
                    return (T)Convert.ChangeType(lbl_TXT_ELAPSEDTIME, typeof(T));

                case ENUM_Control.lbl_TXT_INDATE:
                    return (T)Convert.ChangeType(lbl_TXT_INDATE, typeof(T));

                case ENUM_Control.lbl_TXT_PARKINGFEE:
                    return (T)Convert.ChangeType(lbl_TXT_PARKINGFEE, typeof(T));

                case ENUM_Control.lbl_TXT_PREFEE:
                    return (T)Convert.ChangeType(lbl_TXT_PREFEE, typeof(T));

                case ENUM_Control.lbl_UNIT_CASH1:
                    return (T)Convert.ChangeType(lbl_UNIT_CASH1, typeof(T));

                case ENUM_Control.lbl_UNIT_CASH2:
                    return (T)Convert.ChangeType(lbl_UNIT_CASH2, typeof(T));

                case ENUM_Control.lbl_UNIT_CASH3:
                    return (T)Convert.ChangeType(lbl_UNIT_CASH3, typeof(T));

                case ENUM_Control.lbl_UNIT_CASH4:
                    return (T)Convert.ChangeType(lbl_UNIT_CASH4, typeof(T));

                case ENUM_Control.pic_carimage:
                    return (T)Convert.ChangeType(pic_carimage, typeof(T));

                case ENUM_Control.lblErrorMessage:
                    return (T)Convert.ChangeType(lblErrorMessage, typeof(T));

                default:
                    return default(T);
            }
        }

        #endregion GetControl Logic

        public override event Event_ConfigCall ConfigCall;

        public override event Event_LanguageChange LanguageChange_Click;

        public override event EventHandler PreForm_Click;

        public override event EventHandler Home_Click;

        public override event EventHandler CashCancel_Click;

        public override event EventHandler SamsungPay_Click;

        public override event Event_SeasonCarAddMonth SeasonCarAddMonth;

        private int GoConfigSequence = 0;

        public override Image CarImage { get => pic_carimage.Image; set => pic_carimage.Image = value; }

        public override string ErrorMessage { get => lblErrorMessage.Text; set => lblErrorMessage.Text = value; }

        public override string Payment { get => lbl_Payment.Text; set => lbl_Payment.Text = value; }

        public override string DiscountMoney { get => lbl_DIscountMoney.Text; set => lbl_DIscountMoney.Text = value; }

        public override string ParkingFee { get => lblParkingFee.Text; set => lblParkingFee.Text = value; }

        public override string RecvMoney { get => lbl_RecvMoney.Text; set => lbl_RecvMoney.Text = value; }

        public override string ElapsedTime { get => lblElapsedTime.Text; set => lblElapsedTime.Text = value; }

        public override string DiscountInputCount { get => lblDiscountInputCount.Text; set => lblDiscountInputCount.Text = value; }

        public override bool CancelButtonVisible { get => btn_TXT_CANCEL.Visible; set => btn_TXT_CANCEL.Visible = value; }

        public Ctl4by3Payment()
        {
            InitializeComponent();
        }

        public override void ForeignLanguageVisible(bool visible)
        {
            btnEnglish.Visible = visible;
            btnJapan.Visible = visible;
        }

        /// <summary>
        /// 화면 지우기
        /// </summary>
        public override void Clear()
        {
            lblCarnumber2.Text = string.Empty;
            lbl_CarNumber.Text = string.Empty;
            lbl_CarType.Text = string.Empty;
            lbl_DIscountMoney.Text = "0";
            lbl_RecvMoney.Text = "0";
            lbl_Payment.Text = "0";
            lblParkingFee.Text = "0";
            lblDiscountInputCount.Text = "0";
            lblElapsedTime.Text = string.Empty;
            lblErrorMessage.Text = string.Empty;
            lblIndate.Text = string.Empty;
        }

        /// <summary>
        /// 화면 초기화
        /// </summary>
        public override void Initialize()
        {
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KSNET ||
                NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
            {
                btnCardApproval.Visible = true;
                btnCardApproval.Enabled = true;
            }

            if (NPSYS.gUseSamSungPay)
            {
                btnSamsunPay.Visible = true;
            }

            if (NPSYS.gIsAutoBooth)
            {
                btn_TXT_BACK.Visible = false;
                btn_TXT_HOME.Visible = false;
            }
        }

        /// <summary>
        /// 차량정보 설정
        /// </summary>
        /// <param name="pNormalCarInfo"></param>
        public override void SetCarInfo(NormalCarInfo pNormalCarInfo)
        {
            try
            {
                // 정기권관련기능(만료요금부과/연장관련)
                if (pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                           || pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                {
                    lbl_CarNumber.Text = pNormalCarInfo.InCarNo1;
                    lblCarnumber2.Text = pNormalCarInfo.InCarNo2;
                    lblIndate.Text = NPSYS.ConvetYears_Dash(pNormalCarInfo.CurrExpireYmd);
                    lblElapsedTime.Text = NPSYS.ConvetYears_Dash(pNormalCarInfo.NextExpiredYmd);
                    lblParkingFee.Text = TextCore.ToCommaString(pNormalCarInfo.TotFee.ToString());
                    lbl_RecvMoney.Text = TextCore.ToCommaString((pNormalCarInfo.RecvAmt - pNormalCarInfo.Change).ToString()); //시제설정누락처리
                    lbl_Payment.Text = TextCore.ToCommaString(pNormalCarInfo.PaymentMoney);
                    lbl_DIscountMoney.Text = TextCore.ToCommaString(pNormalCarInfo.TotDc);
                    lbl_CarType.Visible = true;
                    lbl_CarType.Text = "정기권연장";
                    TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu | SetCarInfo", "[정기권연장] 연장요금:" + pNormalCarInfo.TotFee.ToString()
                                                                                    + " 차량번호:" + pNormalCarInfo.OutCarNo1
                                                                                     + " 결제요금:" + pNormalCarInfo.PaymentMoney.ToString() + " "
                                                                                        + " 출차시간:" + NPSYS.datestringParser(pNormalCarInfo.OutYmd + pNormalCarInfo.OutHms)
                                                                                        + " 현재만료일:" + NPSYS.ConvetYears_Dash(pNormalCarInfo.CurrExpireYmd)
                                                                                        + " 연장가능일:" + NPSYS.ConvetYears_Dash(pNormalCarInfo.NextExpiredYmd));
                    lbl_RecvMoney.Visible = false;
                    lbl_DIscountMoney.Visible = false;
                    lbl_TXT_DISCOUNTFEE.Visible = false;
                    lbl_TXT_PREFEE.Visible = false;
                    lbl_TXT_PARKINGFEE.Visible = false;
                    lblParkingFee.Visible = false;
                    lblIndate.Visible = true;
                    lbl_TXT_INDATE.Visible = true;
                    lblElapsedTime.Visible = true;
                    lbl_TXT_ELAPSEDTIME.Visible = true;
                    lbl_UNIT_CASH1.Visible = false;
                    lbl_UNIT_CASH2.Visible = false;
                    lbl_UNIT_CASH3.Visible = false;
                    lblRegExpireInfo.Visible = true;
                    btnOneMonthAdd.Visible = false;
                    btnTwoMonthAdd.Visible = false;
                    btnThreeMonthAdd.Visible = false;
                    btnFourMonthAdd.Visible = false;
                    btnFiveMonthAdd.Visible = false;
                    btnSixMonthAdd.Visible = false;
                    if (pNormalCarInfo.mCurrentSeasonCarAmount != null)
                    {
                        foreach (SeasonCarAmounts carAmountItem in pNormalCarInfo.mCurrentSeasonCarAmount)
                        {
                            switch (carAmountItem.month)
                            {
                                case 1:
                                    btnOneMonthAdd.Visible = true;
                                    break;
                                case 2:
                                    btnTwoMonthAdd.Visible = true;
                                    break;
                                case 3:
                                    btnThreeMonthAdd.Visible = true;
                                    break;
                                case 4:
                                    btnFourMonthAdd.Visible = true;
                                    break;
                                case 5:
                                    btnFiveMonthAdd.Visible = true;
                                    break;
                                case 6:
                                    btnSixMonthAdd.Visible = true;
                                    break;
                            }
                        }
                    }
                }
                else if (pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || pNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                {
                    lbl_CarNumber.Text = pNormalCarInfo.InCarNo1;
                    lblCarnumber2.Text = pNormalCarInfo.InCarNo2;
                    lblIndate.Text = pNormalCarInfo.VanDate_Cancle + " " + pNormalCarInfo.VanTime_Cancle.Substring(0, 5);
                    lblParkingFee.Text = TextCore.ToCommaString(pNormalCarInfo.TotFee.ToString());
                    lbl_RecvMoney.Text = TextCore.ToCommaString((pNormalCarInfo.RecvAmt - pNormalCarInfo.Change).ToString()); //시제설정누락처리
                    lbl_Payment.Text = TextCore.ToCommaString(pNormalCarInfo.PaymentMoney);
                    lbl_DIscountMoney.Text = TextCore.ToCommaString(pNormalCarInfo.TotDc);
                    lbl_CarType.Visible = true;
                    lbl_CarType.Text = "카드취소";
                    lbl_RecvMoney.Visible = false;
                    lbl_DIscountMoney.Visible = false;
                    lbl_TXT_DISCOUNTFEE.Visible = false;
                    lbl_TXT_PREFEE.Visible = false;
                    lbl_TXT_PARKINGFEE.Visible = false;
                    lblParkingFee.Visible = false;
                    lblIndate.Visible = true;
                    lbl_TXT_INDATE.Visible = true;
                    lblElapsedTime.Visible = false;
                    lbl_TXT_ELAPSEDTIME.Visible = false;
                    lbl_UNIT_CASH1.Visible = false;
                    lbl_UNIT_CASH2.Visible = false;
                    lbl_UNIT_CASH3.Visible = false;
                    lblRegExpireInfo.Visible = false;
                    btnOneMonthAdd.Visible = false;
                    btnTwoMonthAdd.Visible = false;
                    btnThreeMonthAdd.Visible = false;
                    btnFourMonthAdd.Visible = false;
                    btnFiveMonthAdd.Visible = false;
                    btnSixMonthAdd.Visible = false;
                    TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu | SetCarInfo", "[카드결제취소] 요금:" + pNormalCarInfo.TotFee.ToString()
                                                                                    + " 차량번호:" + pNormalCarInfo.OutCarNo1
                                                                                     + " 결제요금:" + pNormalCarInfo.PaymentMoney.ToString() + " "
                                                                                     + " 기존승인번호:" + pNormalCarInfo.VanRegNo_Cancle.ToString()
                                                                                     + " 기존승인일자:" + pNormalCarInfo.VanDate_Cancle + " " + pNormalCarInfo.VanTime_Cancle);
                }
                else
                {
                    lbl_CarNumber.Text = pNormalCarInfo.InCarNo1;
                    lblCarnumber2.Text = pNormalCarInfo.InCarNo2;
                    //lblIndate.Text = NPSYS.ConvetYears_Dash(pNormalCarInfo.InYmd) + " " + NPSYS.ConvetDay_Dash(pNormalCarInfo.InHms).Substring(0, 5);
                    lblIndate.Text = NPSYS.ConvetYears_Dash(pNormalCarInfo.InYmd) + " " + NPSYS.ConvetDay_Dash(pNormalCarInfo.InHms).SafeSubstring(0, 5);
                    pNormalCarInfo.ElpaseMinute();
                    switch (NPSYS.CurrentLanguageType)
                    {
                        case ConfigID.LanguageType.ENGLISH:
                            lblElapsedTime.Text = pNormalCarInfo.ElapsedDay.ToString() + " Days " + pNormalCarInfo.ElapsedHour.ToString() +
                                             " Hours " + pNormalCarInfo.ElapsedMinute.ToString() + " Min";
                            break;
                        case ConfigID.LanguageType.JAPAN:
                            lblElapsedTime.Text = pNormalCarInfo.ElapsedDay.ToString() + " 日 " + pNormalCarInfo.ElapsedHour.ToString() +
                                             " 時間 " + pNormalCarInfo.ElapsedMinute.ToString() + " 分";
                            break;
                        case ConfigID.LanguageType.KOREAN:
                            lblElapsedTime.Text = pNormalCarInfo.ElapsedDay.ToString() + " 일 " + pNormalCarInfo.ElapsedHour.ToString() +
                                             " 시간 " + pNormalCarInfo.ElapsedMinute.ToString() + " 분";
                            break;
                    }

                    string payTypeName = string.Empty;

                    //요금할인권처리 주석완료
                    string sElapsedTime = lblElapsedTime.Text;
                    string sInTime = lblIndate.Text;
                    TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|SetCarInfo", "총주차요금:" + pNormalCarInfo.TotFee.ToString()
                                                                                     + " 사전정산금액:" + (pNormalCarInfo.RecvAmt - pNormalCarInfo.Change).ToString()//시제설정누락처리
                                                                                     + " 총주차요금:" + pNormalCarInfo.TotFee.ToString()
                                                                                     + " 총할인요금:" + pNormalCarInfo.TotDc.ToString()
                                                                                     + " 결제요금:" + pNormalCarInfo.PaymentMoney.ToString()
                                                                                     + " RealFee:" + pNormalCarInfo.RealFee.ToString()
                                                                                     + " " + "주차시간:" + sElapsedTime + " 입차시간:" + sInTime + "-출차시간:" + NPSYS.datestringParser(pNormalCarInfo.OutYmd + pNormalCarInfo.OutHms)
                                            + " 차량번호:" + pNormalCarInfo.OutCarNo1 + " 주차요금타입:" + payTypeName);
                    lblParkingFee.Text = TextCore.ToCommaString(pNormalCarInfo.TotFee.ToString());
                    lbl_RecvMoney.Text = TextCore.ToCommaString((pNormalCarInfo.RecvAmt - pNormalCarInfo.Change).ToString()); //시제설정누락처리
                    lbl_Payment.Text = TextCore.ToCommaString(pNormalCarInfo.PaymentMoney);
                    lbl_DIscountMoney.Text = TextCore.ToCommaString(pNormalCarInfo.TotDc);
                    lbl_CarType.Visible = true;
                    lbl_CarType.Text = payTypeName;

                    if (pNormalCarInfo.TotDc > 0)
                    {
                        lbl_CarType.Visible = true;
                        lbl_CarType.Text = "사전할인차량";
                    }

                    if (pNormalCarInfo.LastPayDate > 0)
                    {
                        lbl_CarType.Visible = true;
                        lbl_CarType.Text = "사전정산차량";
                    }

                    if (NPSYS.g_UseDicountDisplay)
                    {
                        lblDiscountCountName.Visible = true;
                        lblDiscountCountName.Text = "입수된할인권수량";
                        lblDiscountInputCount.Visible = true;
                        lblDiscountInputCount.Text = "0";
                    }
                    else
                    {
                        lblDiscountCountName.Visible = false;
                        lblDiscountInputCount.Visible = false;
                        lblDiscountInputCount.Text = "0";
                    }

                    lbl_RecvMoney.Visible = true;
                    lbl_DIscountMoney.Visible = true;
                    lbl_TXT_DISCOUNTFEE.Visible = true;
                    lbl_TXT_PREFEE.Visible = true;
                    lbl_TXT_PARKINGFEE.Visible = true;
                    lblParkingFee.Visible = true;
                    lblIndate.Visible = true;
                    lbl_TXT_INDATE.Visible = true;
                    lblElapsedTime.Visible = true;
                    lbl_TXT_ELAPSEDTIME.Visible = true;
                    lbl_UNIT_CASH1.Visible = true;
                    lbl_UNIT_CASH2.Visible = true;
                    lbl_UNIT_CASH3.Visible = true;
                    //할인권 입수수량 표출 주석완료
                    lblRegExpireInfo.Visible = false;
                    btnOneMonthAdd.Visible = false;
                    btnTwoMonthAdd.Visible = false;
                    btnThreeMonthAdd.Visible = false;
                    btnFourMonthAdd.Visible = false;
                    btnFiveMonthAdd.Visible = false;
                    btnSixMonthAdd.Visible = false;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|SetCarInfo", ex.ToString());
                lblErrorMessage.Text = "SetCarInfo:" + ex.ToString();
            }
        }

        public override void ButtonEnable(ButtonEnableType pEnableType)
        {
            switch (pEnableType)
            {
                case ButtonEnableType.InsertCoin:  //현금입수중인겨우
                    btn_TXT_CANCEL.Enabled = true;
                    btn_TXT_BACK.Enabled = false;
                    btnEnglish.Enabled = false;
                    btnJapan.Enabled = false;
                    btnCardApproval.Enabled = false;
                    btnSamsunPay.Enabled = false;
                    btnSixMonthAdd.Enabled = false;
                    btnFiveMonthAdd.Enabled = false;
                    btnFourMonthAdd.Enabled = false;
                    btnThreeMonthAdd.Enabled = false;
                    btnTwoMonthAdd.Enabled = false;
                    btnOneMonthAdd.Enabled = false;
                    btn_TXT_HOME.Enabled = false;
                    break;
                case ButtonEnableType.CashCancle:  //현금취소중인경우
                case ButtonEnableType.SamsumPayStart:
                case ButtonEnableType.PayFormEnd:
                case ButtonEnableType.AddMonthStart:
                    btn_TXT_CANCEL.Enabled = false;
                    btn_TXT_BACK.Enabled = false;
                    btnEnglish.Enabled = false;
                    btnJapan.Enabled = false;
                    btnCardApproval.Enabled = false;
                    btnSamsunPay.Enabled = false;
                    btnSixMonthAdd.Enabled = false;
                    btnFiveMonthAdd.Enabled = false;
                    btnFourMonthAdd.Enabled = false;
                    btnThreeMonthAdd.Enabled = false;
                    btnTwoMonthAdd.Enabled = false;
                    btnOneMonthAdd.Enabled = false;
                    btn_TXT_HOME.Enabled = false;
                    break;
                case ButtonEnableType.AddMonthEnd:
                    btn_TXT_CANCEL.Enabled = true;
                    btn_TXT_BACK.Enabled = true;
                    btnEnglish.Enabled = true;
                    btnJapan.Enabled = true;
                    btnCardApproval.Enabled = true;
                    btnSamsunPay.Enabled = true;
                    btnSixMonthAdd.Enabled = false;
                    btnFiveMonthAdd.Enabled = false;
                    btnFourMonthAdd.Enabled = false;
                    btnThreeMonthAdd.Enabled = false;
                    btnTwoMonthAdd.Enabled = false;
                    btnOneMonthAdd.Enabled = false;
                    btn_TXT_HOME.Enabled = false;
                    break;

                case ButtonEnableType.CashCancleStop: // 현금취소가 완료된경우
                case ButtonEnableType.SamsunPayEnd:
                case ButtonEnableType.PayFormStart:
                    btn_TXT_CANCEL.Enabled = true;
                    btn_TXT_BACK.Enabled = true;
                    btnEnglish.Enabled = true;
                    btnJapan.Enabled = true;
                    btnCardApproval.Enabled = true;
                    btnSamsunPay.Enabled = true;
                    btnSixMonthAdd.Enabled = true;
                    btnFiveMonthAdd.Enabled = true;
                    btnFourMonthAdd.Enabled = true;
                    btnThreeMonthAdd.Enabled = true;
                    btnTwoMonthAdd.Enabled = true;
                    btnOneMonthAdd.Enabled = true;
                    btn_TXT_HOME.Enabled = true;
                    //   btnRegExtension.Enabled = true;
                    break;
            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | ButtonEnable", "[버튼활성화 관련상태]" + pEnableType.ToString());
        }

        public override void SetLanguage(ConfigID.LanguageType pLanguageType, NormalCarInfo normalCarInfo = null)
        {
            if (normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season // 정기권 연장상황인 경우
                  || normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                lbl_TXT_INDATE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CURTERMDATE.ToString());
                lbl_TXT_ELAPSEDTIME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_NEXTTERMDATE.ToString());
                lbl_TXT_PARKINGFEE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKINGFEE.ToString());
                lbl_TXT_AMOUNTFEE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_TERMFEE.ToString());
            }
            else if (normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                   || normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                lbl_TXT_INDATE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PREPAYTIME.ToString());
                lbl_TXT_AMOUNTFEE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_CANCELFEE.ToString());

            }
            else // 일반요금 부과인경우
            {
                lbl_TXT_INDATE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_INDATE.ToString());
                lbl_TXT_ELAPSEDTIME.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_ELAPSEDTIME.ToString());
                lbl_TXT_PARKINGFEE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_PARKINGFEE.ToString());
                lbl_TXT_AMOUNTFEE.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_UNIT_AMOUNTFEE.ToString());
            }

            //바코드할인 문구 적용
            //상단문구
            if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None && NPSYS.Device.UsingSettingMagneticDCTicket)
            {
                lbl_MSG_DISCOUNTINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MAGNETIC.ToString());
            }
            else if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingMagneticDCTicket)
            {
                lbl_MSG_DISCOUNTINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MAGNETIC.ToString());
            }
            else if (NPSYS.Device.UsingSettingDiscountCard && NPSYS.Device.UsingSettingBarcodeDCTicket)
            {
                lbl_MSG_DISCOUNTINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_BARCODE_MAGNETIC.ToString());
            }
            else
            {
                lbl_MSG_DISCOUNTINFO.Visible = false;
            }

            //바코드모터드리블 사용
            if (NPSYS.IsUsedCashPay() == true && NPSYS.Device.gIsUseCreditCardDevice == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 현금,카드리더기,교통카드 장비 정상
            {
                //바코드모터드리블 사용완료
                //635, 419
                if (NPSYS.Device.UsingSettingCreditCard)  //  현금, 신용카드 , 교통카드
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MONEY_CREDIT_TMONEY.ToString());

                }
                else if (!NPSYS.Device.UsingSettingCreditCard) // 현금 , 교통카드
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MONEY_CREDIT_TMONEY.ToString());
                }
            }
            if (NPSYS.IsUsedCashPay() == true && NPSYS.Device.gIsUseCreditCardDevice == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false) // 현금,카드리더기 정상
            {
                //635, 419
                if (NPSYS.Device.UsingSettingCreditCard)// 현금, 신용카드 
                {

                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MONEY_CREDIT.ToString());
                }
                else if (!NPSYS.Device.UsingSettingCreditCard) // 현금
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MONEY.ToString());
                }
            }
            if (NPSYS.IsUsedCashPay() == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false && NPSYS.Device.gIsUseCreditCardDevice == false) // 현금 장비 정상
            {
                lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_MONEY.ToString());
            }
            if (NPSYS.IsUsedCashPay() == false && NPSYS.Device.gIsUseCreditCardDevice == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true) // 카드리더기 , 교통카드리더기 정상
            {
                if (NPSYS.Device.UsingSettingCreditCard) //  신용카드, 교통카드 
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_CREDIT_TMONEY.ToString());
                }
                else if (!NPSYS.Device.UsingSettingCreditCard) // 교통카드  
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_TMONEY.ToString());
                }
            }
            if (NPSYS.IsUsedCashPay() == false && NPSYS.Device.gIsUseCreditCardDevice == false && NPSYS.Device.isUseDeviceTMoneyReaderDevice == true)
            {
                lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_TMONEY.ToString());
            }

            if (NPSYS.IsUsedCashPay() == false && NPSYS.Device.gIsUseCreditCardDevice == true && NPSYS.Device.isUseDeviceTMoneyReaderDevice == false)
            {
                if (NPSYS.Device.UsingSettingCreditCard) //  신용카드, 교통카드 
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_CREDIT.ToString());
                }
                else
                {
                    lbl_MSG_PAYINFO.Text = string.Empty;
                }
            }

            if (normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
            || normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)

            {
                lbl_MSG_DISCOUNTINFO.Visible = false;
                if (NPSYS.Device.UsingSettingCreditCard)
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_CREDIT.ToString());
                }
            }
            else if (normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                     || normalCarInfo?.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
            {
                lbl_MSG_DISCOUNTINFO.Visible = false;
                if (NPSYS.Device.UsingSettingCreditCard)
                {
                    lbl_MSG_PAYINFO.Text = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_PAY_CREDIT.ToString());
                }
            }
        }

        /// <summary>
        /// 영문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnglish_Click(object sender, EventArgs e)
        {
            LanguageChange_Click?.Invoke(BoothCommonLib.LanguageType.ENGLISH);
        }

        /// <summary>
        /// 일본어
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJapan_Click(object sender, EventArgs e)
        {
            LanguageChange_Click?.Invoke(BoothCommonLib.LanguageType.JAPAN);
        }

        /// <summary>
        /// 이전 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_BACK_Click(object sender, EventArgs e)
        {
            PreForm_Click?.Invoke(sender, e);
        }

        /// <summary>
        /// 처음 화면
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TXT_HOME_Click(object sender, EventArgs e)
        {
            Home_Click?.Invoke(sender, e);
        }

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
                if (ConfigCall != null)
                {
                    ConfigCall();
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu | panel_ConfigClick2_Click", "메인화면으로 강제로 이동시킴");
                }
                GoConfigSequence = 0;
            }
            else
            {
                GoConfigSequence = 0;
            }
        }

        private void btn_TXT_CANCEL_Click(object sender, EventArgs e)
        {
            CashCancel_Click?.Invoke(sender, e);
        }

        private void btnSamsunPay_Click(object sender, EventArgs e)
        {
            SamsungPay_Click?.Invoke(sender, e);
        }

        #region 정기권 연장 개월수 선택 버튼 이벤트 처리

        private void btnOneMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(1);
        }

        private void btnTwoMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(2);
        }

        private void btnThreeMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(3);
        }

        private void btnFourMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(4);
        }

        private void btnFiveMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(5);
        }

        private void btnSixMonthAdd_Click(object sender, EventArgs e)
        {
            SeasonCarAddMonth?.Invoke(6);
        }

        #endregion 정기권 연장 개월수 선택 버튼 이벤트 처리
    }
}