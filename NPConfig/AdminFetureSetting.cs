using FadeFox.Database.SQLite;
using FadeFox.Utility;
using NPCommon;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace NPConfig
{
    public partial class AdminFetureSetting : Form
    {
        string mConfigFilePath = "";
        ConfigDB3I mConfig = null;
        SQLite mDB = new SQLite();

        private const string m_DiscountReadingFormat_105 = "TRACK2ISO_TRACK3105";
        private const string m_DiscountReadingFormat_210 = "TRACK2ISO_TRACK3210";



        private const string m_TIcketDeviceBetstMagneticKorea = "베스트마그네틱발권기";
        private const string m_TIcketDeviceSamhwaBarcodeKorea = "삼화바코드발권기";

        private const string m_TIcketDeviceBetstMagnetic = "BESTMAGNETIC";
        private const string m_TIcketDeviceSamhwaBarcode = "SAMHWABARCODE";
        private const string m_BoothModeReal = "REAL";
        private const string m_BoothModeTest = "TEST";
        private const string m_Use = "사용";
        private const string m_NotUse = "사용안함";


        public AdminFetureSetting(string pConfigfilePath)
        {
            InitializeComponent();
          
            //toolTip1.SetToolTip(tbxInputTime, "주차요금 등이 표출되었을때 고객의 동작이(돈/신용카드 넣는동작도 포함)이 없다면 다시 처음으로 돌아가는 시간");
            //toolTip1.SetToolTip(tbxMoveiTime, " 10로 설정시 동영상 재생이 끝난 후 10초 있다 다시 동영상 재생됨 ");
            //toolTip1.SetToolTip(cbx_PaymentInsertMoneyTimeInfinite, "체크시 요금화면에서 동전이나 할인권을 투입하면 요금결제가 끝나거나 다음 차량이 올때까지 처음화면으로 돌아가지않음 ");
            mConfigFilePath = pConfigfilePath;
            mConfig = new ConfigDB3I(mConfigFilePath);


            mDB.Database = mConfigFilePath;
            mDB.Connect();
        }

        private void AdminFetureSetting_Load(object sender, EventArgs e)
        {
            cbxCarNumberType.Items.Add(ConfigID.CarNumberType.Digit4SetAUTO.ToString());
            cbxCarNumberType.Items.Add(ConfigID.CarNumberType.Digit4SetEnt.ToString());
            cbxCarNumberType.Items.Add(ConfigID.CarNumberType.Digit5Free.ToString());

            cbxAutoboorhSelect.Items.Add(ConfigID.BoothType.AB_1024.ToString());
            cbxAutoboorhSelect.Items.Add(ConfigID.BoothType.PB_1024.ToString());
            cbxAutoboorhSelect.Items.Add(ConfigID.BoothType.NOMIVIEWPB_1080.ToString());

            cbxLanguageType.Items.Add(ConfigID.LanguageType.ENGLISH.ToString());
            cbxLanguageType.Items.Add(ConfigID.LanguageType.JAPAN.ToString());
            cbxLanguageType.Items.Add(ConfigID.LanguageType.KOREAN.ToString());

            cbxDidoErrorRecoveryMode.Items.Add(ConfigID.ErrorRecoveryType.AUTO.ToString());
            cbxDidoErrorRecoveryMode.Items.Add(ConfigID.ErrorRecoveryType.MANUAL.ToString());


            cbxMoneyTray.Items.Add(ConfigID.MoneyType.WON.ToString());
            cbxMoneyTray.Items.Add(ConfigID.MoneyType.YEN.ToString());
            cbxMoneyTray.Items.Add(ConfigID.MoneyType.PHP.ToString());
            cbxMoneyTray.Items.Add(ConfigID.MoneyType.PINWONTEST.ToString());

            //카드실패전송
            cbxUseCardFailSend.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseCardFailSend).ToUpper() == "Y" ? "사용" : "사용안함");
            //카드실패전송 완료
            //TMAP연동
            cbxUseTmap.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseTmap).ToUpper() == "Y" ? "사용" : "사용안함");
            //TMAP연동완료

            //영수증 공급가/부가세 출력 적용
            cbxUseReciptSupplyPrint.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseReciptSupplyPrint).ToUpper() == "N" ? "사용안함" : "사용");
            //영수증 공급가/부가세 출력 적용완료

            txtCenterAliveTime.Text = mConfig.GetValue(ConfigID.FeatureSettingCenterAliveTime).Trim() == string.Empty ? "240" : mConfig.GetValue(ConfigID.FeatureSettingCenterAliveTime);
            switch (mConfig.GetValue(ConfigID.FeatureSettingButtonSound).ToUpper())
            {
                case "Y":
                    cbxButtonSound.Text = "사용";
                    break;
                case "N":
                    cbxButtonSound.Text = "사용안함";
                    break;
                default:
                    cbxButtonSound.Text = "사용안함";
                    break;
            }


            string CarNumberType = (mConfig.GetValue(ConfigID.FeatureSettingCarNumberType));
            NPCommon.ConfigID.CarNumberType currentCarNumberType = (mConfig.GetValue(ConfigID.FeatureSettingCarNumberType)) == string.Empty ? NPCommon.ConfigID.CarNumberType.Digit4SetAUTO : (NPCommon.ConfigID.CarNumberType)Enum.Parse(typeof(NPCommon.ConfigID.CarNumberType), (mConfig.GetValue(ConfigID.FeatureSettingCarNumberType)));
            cbxCarNumberType.Text = currentCarNumberType.ToString();


            cbxUseCreditAndTIcketSplit.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseCreditAndTIcketSplit).ToUpper() == "Y" ? "사용" : "사용안함");
    
            string lBoothname = string.Empty;


            cbxAutoboorhSelect.Text = mConfig.GetValue(ConfigID.FeatureSettingAutoboorhSelect) == string.Empty ? ConfigID.BoothType.AB_1024.ToString() : mConfig.GetValue(ConfigID.FeatureSettingAutoboorhSelect);


            cbxUseRealMode.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseRealMode).ToUpper() == "N" ? m_BoothModeTest : m_BoothModeReal); // 리얼모드인지 테스트모드인지
            txtReceiptSingalNumber.Text = (mConfig.GetValue(ConfigID.FeatureSettingReceiptSingalNumber).ToUpper() == string.Empty ? "9" : mConfig.GetValue(ConfigID.FeatureSettingReceiptSingalNumber).ToUpper());
            //Door신호관련
            txtDoorSingalNumber.Text = (mConfig.GetValue(ConfigID.FeatureSettingDoorSingalNumber).ToUpper() == string.Empty ? "1" : mConfig.GetValue(ConfigID.FeatureSettingDoorSingalNumber).ToUpper());
            //Door신호관련 주석완료


            cbxUseFreeCarPay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseFreeCarPay).ToUpper() == "Y" ? "사용" : "사용안함");
            //사전무인회차0원할인0원일때처리보강
            cbxUseDiscountFreeCarPay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseDiscountFreeCarPay).ToUpper() == "N" ? "사용안함" : "사용");
            cbxUsePrePayConfirm.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseDiscountFreeCarPay).ToUpper() == "Y" ? "사용" : "사용안함");
            //사전무인회차0원할인0원일때처리보강 주석완료

            //사전무인 정기권연장결제 적용
            cbxUseRegExtensionPay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseRegExtensionPay).ToUpper() == "Y" ? "사용" : "사용안함");
            //사전무인 정기권연장결제 적용완료

            cbxUseRepay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseRepay).ToUpper() == "Y" ? "사용" : "사용안함");

            switch (mConfig.GetValue(ConfigID.FeatureSettingAutoBooth).ToUpper())
            {
                case "Y":
                    cbx_AutoBoothEnable.Text = "사용";

                    break;
                case "N":
                    cbx_AutoBoothEnable.Text = "사용안함";
                    break;
                default:
                    cbx_AutoBoothEnable.Text = "사용";
                    break;
            }



            switch (mConfig.GetValue(ConfigID.FeatureSettingPaymentInsertMoneyTimeInfinite).ToUpper())
            {
                case "Y":
                    cbx_PaymentInsertMoneyTimeInfinite.Text = "사용";
                    break;
                case "N":
                    cbx_PaymentInsertMoneyTimeInfinite.Text = "사용안함";
                    break;
                default:
                    cbx_PaymentInsertMoneyTimeInfinite.Text = "사용안함";
                    break;
            }




            tbxMoveiTime.Text = mConfig.GetValue(ConfigID.MovieStopTime).ToUpper().Trim() == "" ?
                                             "10" : mConfig.GetValue(ConfigID.MovieStopTime).ToUpper().Trim();


            tbxInputTime.Text = mConfig.GetValue(ConfigID.InputTime).ToUpper().Trim() == "" ?
                                             "300" : mConfig.GetValue(ConfigID.InputTime).ToUpper().Trim();

            txtCarSearchTime.Text = mConfig.GetValue(ConfigID.CarSearchTIme).ToUpper().Trim() == "" ?
                                             "300" : mConfig.GetValue(ConfigID.CarSearchTIme).ToUpper().Trim();

            txtCarSelectTime.Text = mConfig.GetValue(ConfigID.CarSelectTime).ToUpper().Trim() == "" ?
                                             "300" : mConfig.GetValue(ConfigID.CarSelectTime).ToUpper().Trim();

            txtReceiptTime.Text = mConfig.GetValue(ConfigID.RecipetTIme).ToUpper().Trim() == "" ?
                                             "60" : mConfig.GetValue(ConfigID.RecipetTIme).ToUpper().Trim();

            switch (mConfig.GetValue(ConfigID.FeatureSettingInsert50000Qty).ToUpper())
            {
                case "Y":
                    cbx_Insert50000Qty.Text = "사용";
                    break;
                case "N":
                    cbx_Insert50000Qty.Text = "사용안함";
                    break;
                default:
                    cbx_Insert50000Qty.Text = "사용안함";
                    break;
            }






            cbxDiscountDIsplay.Text = mConfig.GetValue(ConfigID.FeatureSettingDiscountDIsplay).ToUpper().Trim() == "Y" ?
                                             "사용" : "사용안함";

            cbxUseFullcuting.Text = mConfig.GetValue(ConfigID.FeatureSettingFullcuting).ToUpper().Trim() == "Y" ?
                                            "사용" : "사용안함";








            cbxUseDiscountFormat.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseDiscountFormat).ToUpper() == string.Empty ? m_DiscountReadingFormat_210 : mConfig.GetValue(ConfigID.FeatureSettingUseDiscountFormat).ToUpper());
            string moneyOutputType = mConfig.GetValue(ConfigID.FeatureSettingMoneyOutputType);
            NPCommon.ConfigID.MoneyOutputType moneyoutEnum = (moneyOutputType.Trim() == string.Empty ? NPCommon.ConfigID.MoneyOutputType.None : (NPCommon.ConfigID.MoneyOutputType)Enum.Parse(typeof(NPCommon.ConfigID.MoneyOutputType), moneyOutputType.Trim()));
            switch (moneyoutEnum)
            {
                case ConfigID.MoneyOutputType.None:
                    cbxMoneyOutputType.SelectedIndex = 0;
                    break;
                case ConfigID.MoneyOutputType.Question:
                    cbxMoneyOutputType.SelectedIndex = 1;
                    break;
                case ConfigID.MoneyOutputType.NotOutPut:
                    cbxMoneyOutputType.SelectedIndex = 2;
                    break;
                default:
                    cbxMoneyOutputType.SelectedIndex = 0;
                    break;
            }
            cbxUseAutoMagam.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseAutoMagam).ToUpper() == "Y" ? "사용" : "사용안함");
            lblMagamEndTIme.Text = (mConfig.GetValue(ConfigID.FeatureSettingMagamEndTIme).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamEndTIme));
            lblMagamStartTIme.Text = (mConfig.GetValue(ConfigID.FeatureSettingMagamStartTIme).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamStartTIme));
            cbxUseMagamDelay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseMagamDelay).ToUpper() == "Y" ? "사용" : "사용안함");
            lblMagamDelayEndTime.Text = (mConfig.GetValue(ConfigID.FeatureSettingMagamDelayEndTime).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamDelayEndTime));
            lblMagamDelayStartTime.Text = (mConfig.GetValue(ConfigID.FeatureSettingMagamDelayStartTime).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamDelayStartTime));

            //사전정산시 요금없는차량 영수증발급안함 적용
            cbxUsePreFreeCarNoRecipt.Text = (mConfig.GetValue(ConfigID.FeatureSettingUsePreFreeCarNoRecipt).ToUpper() == "Y" ? "사용" : "사용안함");
            //사전정산시 요금없는차량 영수증발급안함 적용완료
     

            //삼성페이결제 적용
            cbxUseSamSungPay.Text = (mConfig.GetValue(ConfigID.FeatureSettingUseSamSungPay).ToUpper() == "Y" ? "사용" : "사용안함");
            //삼성페이결제 적용완료
            string LanguageData = mConfig.GetValue(ConfigID.FeatureSettingLanguage);
            NPCommon.ConfigID.LanguageType LanguageTypeData= (moneyOutputType.Trim() == string.Empty ? NPCommon.ConfigID.LanguageType.KOREAN : (NPCommon.ConfigID.LanguageType)Enum.Parse(typeof(NPCommon.ConfigID.LanguageType), LanguageData.Trim()));
            cbxLanguageType.Text = LanguageTypeData.ToString();

            cbxUseMultiLanguage.Text=(mConfig.GetValue(ConfigID.FeatureSettingUseMultiLanguage) == "Y" ? "USE" : "NOTUSE");

            string DIdoRecoveryType = mConfig.GetValue(ConfigID.FeatureSettingDidoErrorRecoveryMode);
            NPCommon.ConfigID.ErrorRecoveryType DIdoRecoveryTypeData = (DIdoRecoveryType.Trim() == string.Empty ? NPCommon.ConfigID.ErrorRecoveryType.MANUAL : (NPCommon.ConfigID.ErrorRecoveryType)Enum.Parse(typeof(NPCommon.ConfigID.ErrorRecoveryType), DIdoRecoveryType.Trim()));
            cbxDidoErrorRecoveryMode.Text = DIdoRecoveryTypeData.ToString();

            string MoneyType = mConfig.GetValue(ConfigID.FeatureSettingMoneyTypeDataMode);
            NPCommon.ConfigID.MoneyType MoneyTypeData = (MoneyType.Trim() == string.Empty ? NPCommon.ConfigID.MoneyType.WON : (NPCommon.ConfigID.MoneyType)Enum.Parse(typeof(NPCommon.ConfigID.MoneyType), MoneyType.Trim()));
            cbxMoneyTray.Text = MoneyTypeData.ToString();

            txt24ErrCode.Text = mConfig.GetValue(ConfigID.FeatureSettingTmap24ErrorCode);

            txt24Interval.Text = mConfig.GetValue(ConfigID.FeatureSettingTmapUseInterval);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //카드실패전송
            if (cbxUseCardFailSend.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseCardFailSend, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseCardFailSend, "N");
            }
            //카드실패전송 완료
            //TMAP연동
            if (cbxUseTmap.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseTmap, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseTmap, "N");
            }
            //TMAP연동완료

            //영수증 공급가/부가세 출력 적용
            if (cbxUseReciptSupplyPrint.Text.Trim() == "사용안함")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseReciptSupplyPrint, "N");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseReciptSupplyPrint, "Y");
            }
            //영수증 공급가/부가세 출력 적용완료


            if (cbxButtonSound.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingButtonSound, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingButtonSound, "N");
            }


            mConfig.SetValue(ConfigID.FeatureSettingCenterAliveTime, (txtCenterAliveTime.Text==string.Empty?"0": txtCenterAliveTime.Text));






            if (cbxUseCreditAndTIcketSplit.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseCreditAndTIcketSplit, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseCreditAndTIcketSplit, "N");
            }

            mConfig.SetValue(ConfigID.FeatureSettingAutoboorhSelect, cbxAutoboorhSelect.Text);

            if (cbxUseRealMode.Text != m_BoothModeReal)
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRealMode, "N");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRealMode, "Y");
            }

            if (txtReceiptSingalNumber.Text.Trim() == string.Empty)
            {
                mConfig.SetValue(ConfigID.FeatureSettingReceiptSingalNumber, "9");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingReceiptSingalNumber, txtReceiptSingalNumber.Text.Trim());
            }
            //Door신호관련
            if (txtDoorSingalNumber.Text.Trim() == string.Empty)
            {
                mConfig.SetValue(ConfigID.FeatureSettingDoorSingalNumber, "1");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingDoorSingalNumber, txtDoorSingalNumber.Text.Trim());
            }
            //Door신호관련 완료
     
            if (cbxUseFreeCarPay.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseFreeCarPay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseFreeCarPay, "N");
            }
            //사전무인회차0원할인0원일때처리보강

            if (cbxUseDiscountFreeCarPay.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseDiscountFreeCarPay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseDiscountFreeCarPay, "N");
            }

            if (cbxUsePrePayConfirm.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUsePrePayConfirm, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUsePrePayConfirm, "N");
            }

            //사전무인회차0원할인0원일때처리보강 주석완료

            //사전무인 정기권연장결제 적용
            if (cbxUseRegExtensionPay.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRegExtensionPay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRegExtensionPay, "N");
            }
            //사전무인 정기권연장결제 적용완료		

            if (cbxUseRepay.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRepay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseRepay, "N");
            }


            if (cbx_AutoBoothEnable.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingAutoBooth, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingAutoBooth, "N");
            }



            if (cbx_PaymentInsertMoneyTimeInfinite.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingPaymentInsertMoneyTimeInfinite, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingPaymentInsertMoneyTimeInfinite, "N");
            }

            if (tbxMoveiTime.Text.Trim() == "" || tbxMoveiTime.Text.Trim() == "0")
                tbxMoveiTime.Text = "10";

            if (tbxInputTime.Text.Trim() == "" || tbxInputTime.Text.Trim() == "0")
            {
                tbxInputTime.Text = "300";
            }

            mConfig.SetValue(ConfigID.MovieStopTime, tbxMoveiTime.Text);

            mConfig.SetValue(ConfigID.InputTime, tbxInputTime.Text);

            mConfig.SetValue(ConfigID.CarSearchTIme, (txtCarSearchTime.Text.Trim() == string.Empty ? "300" : txtCarSearchTime.Text.Trim()));

            mConfig.SetValue(ConfigID.CarSelectTime, (txtCarSelectTime.Text.Trim() == string.Empty ? "300" : txtCarSelectTime.Text.Trim()));

            mConfig.SetValue(ConfigID.RecipetTIme, (txtReceiptTime.Text.Trim() == string.Empty ? "60" : txtReceiptTime.Text.Trim()));

            if (cbx_Insert50000Qty.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingInsert50000Qty, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingInsert50000Qty, "N");
            }

            if (cbxDiscountDIsplay.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingDiscountDIsplay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingDiscountDIsplay, "N");
            }

            if (cbxUseFullcuting.Text == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingFullcuting, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingFullcuting, "N");
            }

            mConfig.SetValue(ConfigID.FeatureSettingUseDiscountFormat, cbxUseDiscountFormat.Text);

            switch (cbxMoneyOutputType.SelectedIndex)
            {
                case 0:
                    mConfig.SetValue(ConfigID.FeatureSettingMoneyOutputType, NPCommon.ConfigID.MoneyOutputType.None.ToString());
                    break;
                case 1:
                    mConfig.SetValue(ConfigID.FeatureSettingMoneyOutputType, NPCommon.ConfigID.MoneyOutputType.Question.ToString());
                    break;
                case 2:
                    mConfig.SetValue(ConfigID.FeatureSettingMoneyOutputType, NPCommon.ConfigID.MoneyOutputType.NotOutPut.ToString());
                    break;
            }

            if (cbxUseAutoMagam.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseAutoMagam, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseAutoMagam, "N");
            }

            mConfig.SetValue(ConfigID.FeatureSettingMagamEndTIme, lblMagamEndTIme.Text);
            mConfig.SetValue(ConfigID.FeatureSettingMagamStartTIme, lblMagamStartTIme.Text);

            if (cbxUseMagamDelay.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseMagamDelay, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseMagamDelay, "N");
            }

            mConfig.SetValue(ConfigID.FeatureSettingMagamDelayEndTime, lblMagamDelayEndTime.Text);
            mConfig.SetValue(ConfigID.FeatureSettingMagamDelayStartTime, lblMagamDelayStartTime.Text);
            // 2016-11-10 4자리 차량조회안될시 차량통과기능 주석종료

            //만공차 카운팅기능주석완료

            //사전정산시 요금없는차량 영수증발급안함 적용
            if (cbxUsePreFreeCarNoRecipt.Text.Trim() == "사용")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUsePreFreeCarNoRecipt, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUsePreFreeCarNoRecipt, "N");
            }
            //사전정산시 요금없는차량 영수증발급안함 적용완료



            //삼성페이결제 적용
            if (cbxUseSamSungPay.Text.Trim() == "사용안함")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseSamSungPay, "N");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseSamSungPay, "Y");
            }
            //삼성페이결제 적용완료

            if (cbxUseMultiLanguage.Text.Trim() == "USE")
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseMultiLanguage, "Y");
            }
            else
            {
                mConfig.SetValue(ConfigID.FeatureSettingUseMultiLanguage, "N");
            }

            mConfig.SetValue(ConfigID.FeatureSettingLanguage, cbxLanguageType.Text);

            mConfig.SetValue(ConfigID.FeatureSettingDidoErrorRecoveryMode, cbxDidoErrorRecoveryMode.Text);

            mConfig.SetValue(ConfigID.FeatureSettingMoneyTypeDataMode, cbxMoneyTray.Text);

            mConfig.SetValue(ConfigID.FeatureSettingCarNumberType, cbxCarNumberType.Text);

            mConfig.SetValue(ConfigID.FeatureSettingTmap24ErrorCode, txt24ErrCode.Text);

            DateTime outTime;
            if (DateTime.TryParseExact(txt24Interval.Text, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out outTime))
            {
                mConfig.SetValue(ConfigID.FeatureSettingTmapUseInterval, txt24Interval.Text);
            }
            else
            {
                try
                {
                    //Validation Check~!
                    string sInteval = txt24Interval.Text;
                    int h;
                    if (int.TryParse(sInteval.Substring(0, 2), out h))
                    {
                        if (h >= 24)
                        {
                            sInteval = "23" + sInteval.Substring(2);
                        }
                    }
                    int m;
                    if (int.TryParse(sInteval.Substring(3, 2), out m))
                    {
                        if (m >= 59 || h == 24)
                        {
                            sInteval = sInteval.Substring(0, 2) + ":59:" + sInteval.Substring(6);
                        }
                    }
                    int s;
                    if (int.TryParse(sInteval.Substring(6), out s))
                    {
                        if (s >= 59 || h == 24)
                        {
                            sInteval = sInteval.Substring(0, 6) + "59";
                        }
                    }
                    if (DateTime.TryParseExact(sInteval, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out outTime))
                    {
                        mConfig.SetValue(ConfigID.FeatureSettingTmapUseInterval, sInteval);
                    }
                    else
                    {
                        //강제로 적용
                        mConfig.SetValue(ConfigID.FeatureSettingTmapUseInterval, "23:59:59"); //Default 24시 시간 체크(실제론 1분 빠진다)
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("시(2자리숫자):분(2자리숫자):초(2자리숫차) 포멧을 맞춰 입력하세요.");
                    txt24Interval.Focus();
                    return;
                }

            }

            MessageBox.Show("저장하였습니다.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminFetureSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            mDB.Disconnect();
        }

        private void btn_locationSearch_Click(object sender, EventArgs e)
        {
            SearchImageLocation();
        }
        private void SearchImageLocation()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "(.jpg)|*.jpg|(.png)|*.png|(.gif)|*.gif";


            openFileDialog.FilterIndex = 1;     // FilterIndex는 1부터 시작 (여기서는 *.txt) 

            openFileDialog.RestoreDirectory = true;

            try
            {

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    //lblImageLoaction.Text = openFileDialog.FileName;    // Label에 파일경로 출력 
                    //if (lblImageLoaction.Text.Trim() != string.Empty)
                    //{
                    //    mConfig.SetValue(ConfigID.MainImageLocation, lblImageLoaction.Text);
                    //    //TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminNormalSetting|SearchImageLocation", "이미지변경:" + lblImageLoaction.Text);
                    //}
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show("예외사항:" + ex.ToString());

            }
        }

        private void cbxUseAutoMagam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxUseAutoMagam.Text == "사용")
            {
                VisibleMagamSetting(true);
            }
            else
            {
                VisibleMagamSetting(false);
            }
        }

        private void VisibleMagamSetting(bool pIsVisible)
        {
            lblMagamEndTIme.Visible = pIsVisible;
            lblMagamEndTImeName.Visible = pIsVisible;
            lblMagamStartTIme.Visible = pIsVisible;
            lblMagamStartTImeName.Visible = pIsVisible;
            lblMagamDelayEndTime.Visible = pIsVisible;
            lblMagmDelayEndTimeName.Visible = pIsVisible;
            lblMagamDelayStartTime.Visible = pIsVisible;
            lblMagmDelayStartTimeName.Visible = pIsVisible;
            lblUseMagmDelayName.Visible = pIsVisible;
            cbxUseMagamDelay.Visible = pIsVisible;
        }
    }
}
