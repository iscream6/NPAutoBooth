using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using FadeFox;
using FadeFox.Text;
using NPCommon;
using System.Text.RegularExpressions;
using System.Threading;
using FadeFox.UI;
using NPCommon.DEVICE;
using NPCommon.Van;
using NPCommon.DTO;

namespace NPAutoBooth.UI
{
    public partial class FormDeviceTest : Form
    {
        //장비테스트 보강
        private const string mDeviceSuccess = "정상";
        private Color mDeviceSuccessColor = Color.Blue;
        private Color mDeviceNotUseColor = Color.Yellow;
        private const string mDeviceFail = "장애";
        private Color mDeviceFailColor = Color.Red;

        private NPSYS.FormType mPreFomrType = NPSYS.FormType.NONE;
        private NPSYS.FormType mCurrentFormType = NPSYS.FormType.DeviceTest;

        //스마트로 TIT_DIP EV-CAT 적용
        private Smatro_TITDIP_EVCAT mSmartro_TITDIP_EVCat = new Smatro_TITDIP_EVCAT();

  

        string gCardAuthNumber = string.Empty;
        string gCardAuthDate = string.Empty;
        bool UsePayCancle = false;
        bool KisEventStart = false;

        //스마트로 TIT_DIP EV-CAT 적용완료
        bool reciptPrintyn = true;
        public FormDeviceTest(NPSYS.FormType pPreFomrType)
        {
            InitializeComponent();
            NPSYS.CurrentFormType = mCurrentFormType;
            mPreFomrType = pPreFomrType;
            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeSerials.EventBarcode += new BarcodeSerial.BarcodeEvent(BarcodeSerials_EventBarcode);
            }
            //바코드모터드리블 사용완료

        }


        private void FormDeviceTest_Load(object sender, EventArgs e)
        {
            tmrReadAccount.Enabled = true;
            SetLanguage(NPSYS.CurrentLanguageType);
            DeviceStatus_ColorView();
            LblCardReader1.Text = NPSYS.Device.UsingSettingCardReadType.ToString();
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
            {

                NPSYS.Device.DoSensors.DosensorSignalEvent += new GoodTechContorlBoard.SignalEvent(DoSensors_DosensorSignalEvent);
            }

            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.None)
            {
                LblCardReader1.BackColor = mDeviceNotUseColor;
                btn_Cardrea1Read.Enabled = false;
                btn_CardRead1100Pay.Enabled = false;
                btn_CardRead1AlarmClear.Enabled = false;
                btn_CardRead1Insert.Enabled = false;
                btn_CardRead1Out.Enabled = false;
                btn_CardRead1PayCancle.Enabled = false;
                btn_CardRead1Reset.Enabled = false;
                btn_CardRead1Status.Enabled = false;

            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                btn_Cardrea1Read.Enabled = false;
                btn_CardRead1AlarmClear.Enabled = false;
                btn_CardRead1Reset.Enabled = false;
                btn_CardRead1Status.Enabled = false;
                btn_CardRead1Insert.Text = "KOCES 카드입수시도";
                btn_CardRead1Out.Text = "KOCES 카드방출시도";

            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM
                  || NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KICC_DIP_IFM
                  || NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_PAYMGATE
                  // FIRSTDATA처리
                  || NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
            // FIRSTDATA처리 완료
            {
                btn_CardRead1Out.Enabled = false;
                btn_CardRead1Insert.Enabled = false;
                btn_Cardrea1Read.Enabled = false;
                btn_CardRead1AlarmClear.Enabled = false;
                btn_CardRead1Reset.Enabled = false;
                btn_CardRead1Status.Enabled = false;
            }
            //스마트로 TIT_DIP EV-CAT 적용
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                btn_CardRead1Out.Enabled = false;
                btn_CardRead1Insert.Enabled = false;
                btn_Cardrea1Read.Enabled = false;
                btn_CardRead1AlarmClear.Enabled = false;
                btn_CardRead1Reset.Enabled = true;
                btn_CardRead1Status.Enabled = true;
            }
            //스마트로 TIT_DIP EV-CAT 적용완료
            else
            {
                btn_Cardrea1Read.Enabled = false;
                btn_CardRead1AlarmClear.Enabled = false;
                btn_CardRead1Reset.Enabled = false;
                btn_CardRead1Status.Enabled = false;
                btn_CardRead1Out.Enabled = false;
                btn_CardRead1Insert.Enabled = false;
                btn_CardRead1PayCancle.Enabled = false;
                btn_CardRead1100Pay.Enabled = false;
            }

            LblCardReader2.Text = NPSYS.Device.UsingSettingMagneticReadType.ToString();
            if (NPSYS.Device.UsingSettingMagneticReadType == NPCommon.ConfigID.CardReaderType.TItMagnetincDiscount)

            {
            }
            else if (NPSYS.Device.UsingSettingMagneticReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                btn_Cardread2Read.Enabled = false;
                btn_CardRead2AlarmClear.Enabled = false;
                btn_CardRead2Reset.Enabled = false;
                btn_CardRead2Status.Enabled = false;
                btn_CardRead2Insert.Text = "KOCES 카드입수시도";
                btn_CardRead2Out.Text = "KOCES 카드방출시도";

            }
            else
            {
                btn_Cardread2Read.Enabled = false;
                btn_CardRead2AlarmClear.Enabled = false;
                btn_CardRead2Reset.Enabled = false;
                btn_CardRead2Status.Enabled = false;
                btn_CardRead2Out.Enabled = false;
                btn_CardRead2Insert.Enabled = false;
            }

            //카드결제취소 적용
            //GetCardReader();
            //카드결제취소 적용완료
            LoadCreditCard();
            PrintStatus();
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.NEXPA)
            {
                GetSensorText();
            }
            else
            {
                lblSensor.BackColor = mDeviceNotUseColor;
            }
        }
        private void btn_Cardreader1Out_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();
            bool readerEcjt = false;
            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                readerEcjt = KocesTcmMotor.CardEject();
            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                readerEcjt = mKIS_TITDIPDevice.CardLockEject(NPSYS.Device.KisPosAgent);
            }
            lbl_Cardread1Status.Text = "";
            lbl_Cardread1Status.Text = (readerEcjt == true ? "정상배출" : "미배출");


        }
        private void btn_Cardreader1Insert_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            bool readerInsert = false;
            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                readerInsert = KocesTcmMotor.CardAccept();
            }

            lbl_Cardread1Status.Text = "";
            lbl_Cardread1Status.Text = (readerInsert == true ? "입수가능" : "입수불가");

        }
        private void btn_CardRead1Status_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();
            lbl_Cardread1Status.Text = "";
            this.Enabled = false;
            //스마트로 TIT_DIP EV-CAT 적용
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                mSmartro_TITDIP_EVCat.CardReaderStat(NPSYS.Device.Smartro_TITDIP_Evcat);
            }
            //스마트로 TIT_DIP EV-CAT 적용완료

            this.Enabled = true;



        }

        private void btn_Cardreader1Read_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


            this.Enabled = false;
            lbl_Cardread1Data.Text = "";

            this.Enabled = true;



        }

        private void btn_CardRead1Reset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();
            //스마트로 TIT_DIP EV-CAT 적용
            lbl_Cardread1Status.Text = "";
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | btn_CardRead1Reset_Click", "카드리더기 리셋명령");
                mSmartro_TITDIP_EVCat.CardReaderReset(NPSYS.Device.Smartro_TITDIP_Evcat);
            }
            else if (NPSYS.Device.GetCurrentUseDeviceCard() == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);
            }

            //스마트로 TIT_DIP EV-CAT 적용완료





        }

        private void btn_CardRead1AlarmClear_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();



            NPSYS.Device.gIsUseCreditCardDevice = true;
            TextCore.ACTION(TextCore.ACTIONS.CARDREADER1, "FormDeviceTest|btn_CardRead1AlarmClear_Click", "카드리더기1 정상으로 전환");
            DeviceStatus_ColorView();

        }






        private void btn_Cardreader2Read_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            lbl_Cardread2Data.Text = "";
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | btn_Cardreader2Read_Click", "할인권읽기버튼누름");

            this.Enabled = false;
            Result _result = NPSYS.Device.CardDevice2.readingTicketCardStart();


            Result l_ReadingTrackdata = NPSYS.Device.CardDevice2.readingTicketCardEnd();

            if (l_ReadingTrackdata.Success)    // 카드 또는 티켓이 삽입된 상태이고 정보값을 읽었다면
            {

                lbl_Cardread2Data.Text = l_ReadingTrackdata.Message;
            }
            this.Enabled = true;


        }

        private void btn_Cardreader2Out_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


            bool readerEcjt = false;
            if (NPSYS.Device.UsingSettingMagneticReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                readerEcjt = KocesTcmMotor.CardEject();
            }
            else
            {
                readerEcjt = (NPSYS.Device.CardDevice2.TIcketFrontEject() == 0 ? true : false);
            }
            lbl_Cardread2Status.Text = "";
            lbl_Cardread2Status.Text = (readerEcjt == true ? "정상배출" : "미배출");



        }

        private void btn_Cardreader2Insert_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            bool readerInsert = false;
            if (NPSYS.Device.UsingSettingMagneticReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                readerInsert = KocesTcmMotor.CardAccept();
            }
            else
            {
                NPSYS.Device.CardDevice2.TIcketBackEject();
                readerInsert = true;
            }
            lbl_Cardread2Status.Text = "";
            lbl_Cardread2Status.Text = (readerInsert == true ? "입수가능" : "입수불가");
        }

        private void btn_CardRead2Status_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


            this.Enabled = false;
            Result _TIcketStatus = NPSYS.Device.CardDevice2.GetStatus();
            lbl_Cardread2Status.Text = "";
            lbl_Cardread2Status.Text = _TIcketStatus.Message;
            this.Enabled = true;


        }
        private void btn_CardRead2Reset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


            this.Enabled = false;
            Result l_ResetStatus = NPSYS.Device.CardDevice2.SoftResetCreditDevice();
            lbl_Cardread1Status.Text = "";
            lbl_Cardread1Status.Text = l_ResetStatus.Message;
            this.Enabled = true;


        }
        private void btn_CardRead2AlarmClear_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();



            NPSYS.Device.gIsUseMagneticReaderDevice = true;
            TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormDeviceTest|btn_CardRead2AlarmClear_Click", "카드리더기2 정상으로 전환");
            DeviceStatus_ColorView();




        }



        private void btn_ok_Click(object sender, EventArgs e)
        {

            this.Close();
        }



        private void FormDeviceTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            tmrReadAccount.Enabled = false;
            NPSYS.CurrentFormType = mPreFomrType; // 기존폼타입을 반환한다.
            if (NPSYS.Device.isUseDeviceBillReaderDevice && NPSYS.Device.UsingSettingBillReader)
            {
                NPSYS.Device.BillReader.BillDIsableAccept();
            }
            if (NPSYS.Device.isUseDeviceCoinReaderDevice && NPSYS.Device.UsingSettingCoinReader)
            {
                NPSYS.Device.CoinReader.DisableCoinRead();
            }
            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
            {
                NPSYS.Device.BarcodeSerials.EventBarcode -= new BarcodeSerial.BarcodeEvent(BarcodeSerials_EventBarcode);
            }
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
            {

                NPSYS.Device.DoSensors.DosensorSignalEvent -= new GoodTechContorlBoard.SignalEvent(DoSensors_DosensorSignalEvent);
            }
            //바코드모터드리블 사용완료
            //결제취소 적용
            UnLoadCreditCard();
            //if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            //{
            //    mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);
            //    NPSYS.Device.KisPosAgent.OnAgtComplete -= new EventHandler(KisPosAgent_OnAgtComplete);
            //    NPSYS.Device.KisPosAgent.OnAgtState -= new EventHandler(KisPosAgent_OnAgtState);
            //}
            //if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_DIP_IFM)
            //{
            //    NPSYS.Device.KICC_TIT.CardEject();
            //}
            //결제취소 적용완료
            NPSYS.LedLight();
            this.Close();
            if (this != null)
            {
                this.Dispose();
            }
        }

        private void btn_InsertBill_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingBillReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "지폐리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            lbl_BillreadStatus.Text = "";
            BillReader.BillRederStatusType currentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
            if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
            {

                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormDeviceTest|btn_InsertBill_Click", "지폐을 받는 동작 작동 문제가 생겨서 다시시도");
                NPSYS.Device.BillReader.Reset();
                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormDeviceTest|btn_InsertBill_Click", "지폐리더기 소프트리셋");
                currentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormDeviceTest|btn_InsertBill_Click", "지폐을 받는 동작 작동 문제가 생겨서 다시시도");
                if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                {
                    lbl_BillreadStatus.Text = "에러";

                }
                else
                {
                    lbl_BillreadStatus.Text = "정상";
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormDeviceTest|btn_InsertBill_Click", "지폐리더기 정상");

                }
            }
            else
            {
                lbl_BillreadStatus.Text = "정상";
            }
        }

        private void btn_StopBillInsert_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingBillReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "지폐리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.BillReader.BillDIsableAccept();
        }

        private void btn_BillReaderStatus_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingBillReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "지폐리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            lbl_BillreadStatus.Text = "";
            NPSYS.Device.BillReader.CurrentStatus();
            if (NPSYS.Device.isUseDeviceBillReaderDevice == true)
            {
                lbl_BillreadStatus.Text = "정상";
            }

        }


        private void btn_BillReaderReset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingBillReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "지폐리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.BillReader.Disconnect();
            NPSYS.Device.BillReader.Connect();
            NPSYS.Device.BillReader.Reset();
            NPSYS.Device.BillReader.BillDIsableAccept();

        }

        /// <summary>
        /// 지폐를 돈통에 넣는 작업 , 지폐입수후 BillAccept 명령 실행하지 않을시 자동으로 돈이 앞으로 리젝된다
        /// </summary>
        private BillReader.BillRederStatusType getBillInsert()
        {
            return NPSYS.Device.BillReader.BillAccept();

        }

        /// <summary>
        /// 지폐를 돈통에 넣지않는 작업 Reject
        /// </summary>
        private BillReader.BillRederStatusType getBillReject()
        {
            return NPSYS.Device.BillReader.BillReject();

        }


        private void tmrReadAccount_Tick(object sender, EventArgs e)
        {
            tmrReadAccount.Enabled = false;

            try
            {
                tmrReadAccount.Enabled = false;

                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //동전연속투입관련 변경
                if (NPSYS.Device.UsingSettingCoinReader && NPSYS.Device.isUseDeviceCoinReaderDevice)
                {
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

                    if (BillReader.g_billValue.ToUpper() == "REJECT")
                    {
                        BillReader.g_billValue = "";
                        getBillReject();
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐불량으로 리젝트");

                    }
                    else
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest|tmrReadAccount_Tick", "지폐들어옴:" + billValue); //

                        BillReader.g_billValue = "";
                        getBillInsert();
                        if (NPSYS.Device.isUseDeviceBillReaderDevice)
                        {
                            InsertMoney(billValue);
                        }
                        else
                        {
                            getBillReject();
                            TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐불량으로 리젝트");

                        }
                    }
                }


                tmrReadAccount.Enabled = true;
                return;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest|tmrReadAccount_Tick", ex.ToString());
                tmrReadAccount.Enabled = true;


            }
            finally
            {
                tmrReadAccount.Enabled = true;
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

                int inMoney = 0;

                inMoney = Convert.ToInt32(p_BillVaule.ToUpper().Replace("QTY", ""));
                if (inMoney <= 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest|InsertMoney", "들어온 돈:0");

                    return;
                }

                InsertMoneyChangeValue(p_BillVaule);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest|InsertMoney", ex.ToString());
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
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 50).ToString();
                        break;
                    case "100QTY":
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 100).ToString();
                        break;
                    case "500QTY":
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 500).ToString();
                        break;

                    case "1000QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 1000).ToString();

                        break;
                    case "5000QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 5000).ToString();

                        break;
                    case "10000QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 10000).ToString();

                        break;

                    case "50000QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 50000).ToString();
                        break;


                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (message.ToUpper())
                {
                    case "1QTY":
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 1).ToString();
                        break;
                    case "5QTY":
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 5).ToString();
                        break;
                    case "10QTY":
                        lblCoinMoney.Text = (Convert.ToInt32(lblCoinMoney.Text) + 10).ToString();
                        break;

                    case "20QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 20).ToString();

                        break;
                    case "50QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 50).ToString();

                        break;
                    case "100QTY":
                        lblBillMoney.Text = (Convert.ToInt32(lblBillMoney.Text) + 100).ToString();

                        break;


                }
            }


        }



        private void DeviceStatus_ColorView()
        {
            if (!NPSYS.Device.gIsUseCreditCardDevice)
            {
                LblCardReader1.BackColor = Color.Red;
            }
            if (!NPSYS.Device.gIsUseMagneticReaderDevice)
            {
                LblCardReader2.BackColor = Color.Red;
            }
            if (!NPSYS.Device.gIsUseDeviceBillDischargeDevice)
            {
                lblBillDIscharger.BackColor = Color.Red;
            }
            if (!NPSYS.Device.isUseDeviceCoinReaderDevice)
            {
                lblCoinReader.BackColor = Color.Red;
            }
            if (!NPSYS.Device.isUseDeviceBillReaderDevice)
            {
                lblBillReader.BackColor = Color.Red;
            }
            if (!NPSYS.Device.gIsUseCoinDischarger50Device)
            {
                lblCoinDIscharger50.BackColor = Color.Red;
            }

            if (!NPSYS.Device.gIsUseCoinDischarger100Device)
            {
                lblCoinDIscharger100.BackColor = Color.Red;
            }

            if (!NPSYS.Device.gIsUseCoinDischarger500Device)
            {
                lblCoinDIscharger500.BackColor = Color.Red;
            }
            if (!NPSYS.Device.gIsUseBarcodeSerial)
            {
                lblBarcode.BackColor = Color.Red;
            }

            //영수증프린터알람해제처리 및 영수증 출력변경
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
            {
                GetPrintStatusText();
            }
            else
            {
                lblPrinter.BackColor = mDeviceNotUseColor;
            }
            if (!NPSYS.Device.gIsUseBarcodeSerial)
            {
                lblBarcode.BackColor = Color.Red;
            }
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode || NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.None)
            {
                btnMotorBarcodeInsert.Enabled = false;
                btnMotorBarcodeOut.Enabled = false;
                btnMotorBarcodeReding.Enabled = false;
                btnMotorBarcodeReset.Enabled = false;
                btnMotorBarcodeStatus.Enabled = false;
            }
            //영수증프린터알람해제처리 및 영수증 출력변경 주석완료
        }

        private void btn_OK2_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btn_InsertCoin_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            if (!NPSYS.Device.UsingSettingCoinReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            lbl_CoinreadStatus.Text = "";
            CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();

            if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
            {
                l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                {
                    NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                    lbl_CoinreadStatus.Text = l_CoinReaderResult.ToString();
                    TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormDeviceTest|btn_InsertCoin_Click", l_CoinReaderResult.ToString());
                }
                else
                {
                    lbl_CoinreadStatus.Text = "정상";
                }
            }
            else
            {
                lbl_CoinreadStatus.Text = "정상";
            }
        }

        private void btn_StopCoinInsert_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            lbl_CoinreadStatus.Text = "";
            if (!NPSYS.Device.UsingSettingCoinReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.DisableCoinRead();
            if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
            {
                l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                {
                    NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                    lbl_CoinreadStatus.Text = l_CoinReaderResult.ToString();
                    TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormDeviceTest|btn_StopCoinInsert_Click", l_CoinReaderResult.ToString());
                }
                else
                {
                    lbl_CoinreadStatus.Text = "정상";
                }
            }
            else
            {
                lbl_CoinreadStatus.Text = "정상";
            }

        }



        private void btn_CoinReaderStatus_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            if (!NPSYS.Device.UsingSettingCoinReader)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전리더기는 사용하지 않게 설정되어있습니다.");
                return;
            }
            lbl_CoinreadStatus.Text = "";
            CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();

            if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
            {
                l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                {
                    NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                    lbl_CoinreadStatus.Text = l_CoinReaderResult.ToString();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest|btn_CoinReaderStatus_Click", l_CoinReaderResult.ToString());
                }
                else
                {
                    lbl_CoinreadStatus.Text = "정상";
                }
            }
            else
            {
                lbl_CoinreadStatus.Text = "정상";
            }

        }

        private void btn_1000Out_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            lbl_BillDischargerStatus.Text = "";
            int bill1000qty = 1;
            Result _result = MoneyBillOutDeviice.OutPut1000Qty(ref bill1000qty);
            if (_result.Success == false)
            {
                lbl_BillDischargerStatus.Text = _result.Message;
                NPSYS.Device.BillDischargeDeviceErrorMessage = "불출실패:" + _result.Message + " 불출안된숫자:" + bill1000qty.ToString();
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormDeviceTest|btn_1000Out_Click", "불출실패:" + _result.Message + " 불출안된숫자:" + bill1000qty.ToString());
                return;

            }
            lbl_BillDischargerStatus.Text = "정상";
            TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormDeviceTest|btn_1000Out_Click", "1000원불출숫자:" + bill1000qty.ToString());

        }

        private void btn_5000Out_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            lbl_BillDischargerStatus.Text = "";
            int bill5000qty = 1;
            Result _result = MoneyBillOutDeviice.OutPut5000Qty(ref bill5000qty);
            if (_result.Success == false)
            {
                lbl_BillDischargerStatus.Text = _result.Message;
                NPSYS.Device.BillDischargeDeviceErrorMessage = "불출실패:" + _result.Message + " 불출안된숫자:" + bill5000qty.ToString();
                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormDeviceTest|btn_5000Out_Click", "불출실패:" + _result.Message + " 불출안된숫자:" + bill5000qty.ToString());

                return;

            }
            lbl_BillDischargerStatus.Text = "정상";
            TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormDeviceTest|btn_5000Out_Click", "5000원불출숫자:" + bill5000qty.ToString());

        }



        private void btn_BillDischargerReset_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            MoneyBillOutDeviice.BillPurgeJob();
        }

        private void btn_BillDischargerStatus_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();
            lbl_BillDischargerStatus.Text = "";
            Result _result = MoneyBillOutDeviice.MoneyBillStatus();
            lbl_BillDischargerStatus.Text = _result.Message;
        }



        private void btn_50Out_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();
            if (!NPSYS.Device.UsingSettingCoinCharger50)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            try
            {

                lbl_CoinDischarger50Status.Text = "";
                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = Convert.ToInt32(lbl50SettingQty.Text);
                TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormDeviceTest|btn_50Out_Click", "숫자:" + outCoin.ToString());

                CoinDispensor.CoinDispensorStatusType currentOutPutStatus = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor50, outCoin, ref outchargeResult);
                if (currentOutPutStatus == CoinDispensor.CoinDispensorStatusType.OK)
                {
                    lbl_CoinDischarger50Status.Text = "정상";

                }
                else
                {
                    TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormDeviceTest|btn_50Out_Click", "동전방출오류:" + currentOutPutStatus.ToString());
                    lbl_CoinDischarger50Status.Text = "동전방출오류:" + currentOutPutStatus.ToString();
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormDeviceTest|btn_50Out_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btn_CoinDischarger50Reset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger50)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor50.Disconnect();
            NPSYS.Device.CoinDispensor50.Connect();
            NPSYS.Device.CoinDispensor50.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor50.BaReadinessSignal();
            applicationDoevent(10);
        }

        private void btn_CoinDischarger50Status_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger50)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor50.Disconnect();
            NPSYS.Device.CoinDispensor50.Connect();
            NPSYS.Device.CoinDispensor50.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor50.BaReadinessSignal();
            applicationDoevent(10);
        }

        private void btn_100Out_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger100)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            try
            {

                lbl_CoinDischarger100Status.Text = "";
                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = Convert.ToInt32(lbl100SettingQty.Text);
                TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormDeviceTest|btn_100Out_Click", "숫자:" + outCoin.ToString());

                CoinDispensor.CoinDispensorStatusType currentOutPutStatus = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor100, outCoin, ref outchargeResult);
                if (currentOutPutStatus == CoinDispensor.CoinDispensorStatusType.OK)
                {
                    lbl_CoinDischarger100Status.Text = "정상";

                }
                else
                {
                    TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormDeviceTest|btn_100Out_Click", "장비에러:" + currentOutPutStatus.ToString());
                    lbl_CoinDischarger100Status.Text = "장비에러" + currentOutPutStatus.ToString();
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormDeviceTest|btn_100Out_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btn_CoinDischarger100Reset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger100)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor100.Disconnect();
            NPSYS.Device.CoinDispensor100.Connect();
            NPSYS.Device.CoinDispensor100.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor100.BaReadinessSignal();
            applicationDoevent(10);


        }

        private void btn_CoinDischarger100Status_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger100)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor100.Disconnect();
            NPSYS.Device.CoinDispensor100.Connect();
            NPSYS.Device.CoinDispensor100.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor100.BaReadinessSignal();
            applicationDoevent(10);
        }

        private void btn_CoinDischarger100AlarmClear_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger100)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.gIsUseCoinDischarger100Device = true;
        }

        private void btn_500Out_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger500)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            try
            {

                lbl_CoinDischarger500Status.Text = "";
                this.Enabled = false;

                int outCoin = 0;
                int outchargeResult = 0;
                outCoin = Convert.ToInt32(lbl500SettingQty.Text); ;
                TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormDeviceTest|btn_500Out_Click", "숫자:" + outCoin.ToString());
                CoinDispensor.CoinDispensorStatusType currentOutPutStatus = NPSYS.OutChargeCoin(NPSYS.Device.CoinDispensor500, outCoin, ref outchargeResult);
                if (currentOutPutStatus == CoinDispensor.CoinDispensorStatusType.OK)
                {
                    lbl_CoinDischarger500Status.Text = "정상";

                }
                else
                {
                    TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormDeviceTest|btn_500Out_Click", "장비에러:" + currentOutPutStatus.ToString());
                    lbl_CoinDischarger500Status.Text = "장비에러:" + currentOutPutStatus.ToString();
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormDeviceTest|btn_500Out_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }

        }

        private void btn_CoinDischarger500Reset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger500)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor500.Disconnect();
            NPSYS.Device.CoinDispensor500.Connect();
            NPSYS.Device.CoinDispensor500.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor500.BaReadinessSignal();
            applicationDoevent(10);
        }


        private void btn_CoinDischarger500Status_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            if (!NPSYS.Device.UsingSettingCoinCharger500)
            {
                MessageBox.Show(new Form { TopMost = true }, "동전방출기는는 사용하지 않게 설정되어있습니다.");
                return;
            }
            NPSYS.Device.CoinDispensor500.Disconnect();
            NPSYS.Device.CoinDispensor500.Connect();
            NPSYS.Device.CoinDispensor500.reset();
            applicationDoevent(40);
            NPSYS.Device.CoinDispensor500.BaReadinessSignal();
            applicationDoevent(10);

        }


        /// <summary>
        /// p_temrm은 1에 0.1초이며 p_term이 되면 작업을 마침
        /// </summary>
        /// <param name="p_term"></param>
        private void applicationDoevent(int p_term)
        {
            int i = 0;
            while (true)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                i++;
                if (i > p_term)
                {
                    break;
                }

            }

        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btn_FirmWareVersion_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


            NPSYS.Device.DoSensors.GetVersion();

            //lblFirmWareVersion.Text = NPSYS.Device.DoSensors.m_Version.ToString();

        }

        private void btnBoardRequset_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            NPSYS.Device.DoSensors.RequestBoardCheck();
        }

        private void btnResetTime_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();

            NPSYS.Device.DoSensors.RequestResetTimeSetting(200);
        }

        private void btnVersion0_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


        }

        private void btnVersion1_Click(object sender, EventArgs e)
        {

            NPSYS.buttonSoundDingDong();


        }



        void BarcodeSerials_EventBarcode(object sender, string pBarcode)
        {
            lblBarcodeData.Text = string.Empty;
            lblBarcodeData.Text = pBarcode;
        }

        //영수증프린터알람해제처리 및 영수증 출력변경

        private void btn_ReciptPrintStatus_Click(object sender, EventArgs e)
        {
            //NPSYS.buttonSoundDingDong();
            //if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
            //{
            //    HMC60.HmcStatus currentPrintStatus = NPSYS.Device.HMC60.Status();
            //    if (HMC60.CurrentStatus == HMC60.HmcStatus.OK)
            //    {
            //        lblReciptPrint.BackColor = Color.Blue;
            //        NPSYS.Device.ReceiptPrintDeviceErrorMessage = "정상";
            //        lblReciptPrint.Text = "정상";
            //        NPSYS.Device.gIsUseReceiptPrintDevice = true;
            //        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, (int)88);

            //    }
            //    else
            //    {
            //        lblReciptPrint.BackColor = Color.Red;
            //        NPSYS.Device.gIsUseReceiptPrintDevice = false;
            //        lblReciptPrint.Text = HMC60.CurrentStatus.ToString();
            //        NPSYS.Device.ReceiptPrintDeviceErrorMessage = HMC60.CurrentStatus.ToString();
            //        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.REP, (int)HMC60.CurrentStatus);
            //    }
            //    System.Threading.Thread.Sleep(1000);
            //}
        }
        //영수증프린터알람해제처리 및 영수증 출력변경 주석완료

        // 카드결제취소 적용
        NormalCarInfo mNormalCarInfo = new NormalCarInfo();
        KIS_TITDIPDevice mKIS_TITDIPDevice = new KIS_TITDIPDevice();
        SmartroVCat mSmartroVCat = new SmartroVCat();  // 스마트로 추가
        System.Windows.Forms.Timer timerKiccTs141State = new System.Windows.Forms.Timer();

        private void LoadCreditCard()
        {
            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                SetKisDipIFM();
            }
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.VNA_SMATRO)
            {
                mSmartroVCat.VCatIp = NPSYS.gVanIp;
                mSmartroVCat.VCatPort = Convert.ToInt32(NPSYS.gVanPort);
                NPSYS.Device.SmtSndRcv.OnRcvState += new AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEventHandler(SmtSndRcv_OnRcvState);
                NPSYS.Device.SmtSndRcv.OnTermComplete += new EventHandler(SmtSndRcv_OnTermComplete);
                NPSYS.Device.SmtSndRcv.OnTermExit += new EventHandler(SmtSndRcv_OnTermExit);
            }
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_TS141)
            {
                timerKiccTs141State.Tick += timerKiccTs141State_Tick;
                timerKiccTs141State.Interval = 1000;
            }
            //스마트로 TIT_DIP EV-CAT 적용
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += SmartroEvcat_QueryResults;
                mSmartro_TITDIP_EVCat.SendInfo.InitSendData();
            }
            //스마트로 TIT_DIP EV-CAT 적용완료
        }

        private void UnLoadCreditCard()
        {
            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);
            }
            if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.VAN_FIRSTDATA)
            {
                ;
            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KICC_DIP_IFM)
            {
                ;
            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KICC_TS141)
            {
                if (timerKiccTs141State.Enabled)
                {
                    timerKiccTs141State.Enabled = false;
                    timerKiccTs141State.Stop();
                    KiccTs141.Initilize();
                }
            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.KOCES_TCM)
            {
                KocesTcmMotor.CardEject();
            }
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.SmartroVCat)
            {
                SmartroVCat.SmatroData smatrodata = mSmartroVCat.DeviceReInitialLizeSync(NPSYS.Device.SmtSndRcv);
                NPSYS.Device.SmtSndRcv.OnRcvState -= new AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEventHandler(SmtSndRcv_OnRcvState);
                NPSYS.Device.SmtSndRcv.OnTermComplete -= new EventHandler(SmtSndRcv_OnTermComplete);
                NPSYS.Device.SmtSndRcv.OnTermExit -= new EventHandler(SmtSndRcv_OnTermExit);
            }
            //스마트로 TIT_DIP EV-CAT 적용
            else if (NPSYS.Device.UsingSettingCardReadType == NPCommon.ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults -= mSmartro_TITDIP_EVCat.SmartroEvcat_QueryResults;
            }
            //스마트로 TIT_DIP EV-CAT 적용완료
        }


        private void btn_CardRead1PayCancle_Click(object sender, EventArgs e)
        {
            CreditCardPayCancle();
        }

        public void CreditCardPayCancle()
        {
            TextCore.INFO(TextCore.INFOS.CARD, "FormPaymentMenu|CreditCardPayCancleResult", "카드결제취소시작");
            string terminalNo = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
            int Creditpaymoneys = 0;
            string CardAuthNumber = string.Empty;
            string CardAuthDate = string.Empty;
            string tkno = string.Empty;
            string carno = string.Empty;
            int taxsResult = (int)(Creditpaymoneys / 11);
            int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가

            try
            {
                string sql = "Select  NORKEY,RESCODE,RESMSG,AUTH_NUMBER,AUTH_DATE,IN_DATE, OUT_DATE ,PARKTIME , CAR_NUMBER , CREDIT_PAY ,CREDIT_TAX,CREDIT_SUPPLY, LOG_DATE"
                   + "    FROM CreditCard_LOG order by LOG_DATE desc LIMIT 1";


                DataTable CreditCard_LOG = NPSYS.NPPaymentLog.SelectT(sql);
                if (CreditCard_LOG != null && CreditCard_LOG.Rows.Count > 0)
                {
                    CardAuthDate = CreditCard_LOG.Rows[0]["AUTH_DATE"].ToString().Replace("-", "").Substring(2, 6);
                    CardAuthNumber = CreditCard_LOG.Rows[0]["AUTH_NUMBER"].ToString();
                    Creditpaymoneys = Convert.ToInt32(CreditCard_LOG.Rows[0]["CREDIT_PAY"].ToString());
                    tkno = CreditCard_LOG.Rows[0]["NORKEY"].ToString();
                    carno = CreditCard_LOG.Rows[0]["CAR_NUMBER"].ToString();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest|CreditCardPayCancleResult", "카드결제취소차량정보 :" + carno + " TkNO : " + tkno + " 취소금액 :" + Creditpaymoneys.ToString() + " 승인번호 :" + CardAuthNumber + " 승인날짜 :" + CardAuthDate);
                }
                else
                {
                    lbl_Cardread1Status.Text = "카드결제정보 없음";
                    TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormDeviceTest|CreditCardPayCancleResult", "카드결제정보 없음");
                    return;

                }

                if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_DIP_IFM)
                {
                    bool resultCardState = NPSYS.Device.KICC_TIT.GetCardInsert();
                    if (resultCardState)
                    {
                        lbl_Cardread1Status.Text = "결제취소 요청";
                        NPSYS.Device.KICC_TIT.SendData_D4(Creditpaymoneys.ToString(), CardAuthDate, CardAuthNumber, "10");
                        KICC_TIT.KICC_TIT_RECV_SUCCESS result = NPSYS.Device.KICC_TIT.GetRecvData();
                        if (result == null || result.SUC.Trim() == "")
                        {
                            TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormDeviceTest|CreditCardPayCancleResult", "신용카드 서버 접속안됨");
                            TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormDeviceTest|CreditCardPayCancleResult", "신용카드 서버 접속안됨");
                            NPSYS.Device.KICC_TIT.CardEject();
                            return;
                        }
                        string returnCode = result.RS04;
                        if (result.SUC == "00" && returnCode == "0000")
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPayCancleResult", "[카드결제취소성공]");

                            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string[] lCardNumData = result.RQ04.Split('=');

                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPayCancleResult", "[카드결제취소성공]"
                                                                      + " [카드번호]" + lCardNumData[0]
                                                                      + " [응답메세지]" + result.RS16.Trim()
                                                                      + " [응답코드]" + returnCode.Trim()
                                                                      + " [카드명]" + result.RS12.Trim()
                                                                      + " [승인번호]" + result.RS09.Trim()
                                                                      + " [승인일자]" + NPSYS.ConvetYears_Dash("20" + result.RS07.Substring(0, 6))
                                                                      + " [승인시간]" + NPSYS.ConvetDay_Dash(result.RS07.Substring(6, 6)));

                            //LPRDbSelect.PayCancleProc(mNormalCarInfo);
                            //통합처리 취소데이터관련
                            lbl_Cardread1Status.Text = "카드 결제취소성공";
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormDeviceTest|CreditCardPayCancleResult", "카드 결제취소성공");
                            NPSYS.Device.KICC_TIT.CardEject();
                            return;
                        }
                        else
                        {

                            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPayCancleResult", "[카드결제취소실패]");
                            if (returnCode != null)
                            {
                                returnCode = "9999";
                            }
                            mNormalCarInfo.VanRescode = returnCode;

                            mNormalCarInfo.VanAmt = 0;
                            if (result.RS16 == null)
                            {
                                result.RS16 = string.Empty;
                            }
                            if (result.MSG == null)
                            {
                                result.MSG = string.Empty;
                            }
                            mNormalCarInfo.VanResMsg = (result.RS16.Trim() == string.Empty ? result.MSG.Trim() : result.RS16.Trim());
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPayCancleResult", "[카드결제취소실패]"
                                                                      + " [응답메세지]" + mNormalCarInfo.VanResMsg
                                                                      + " [메세지]" + result.MSG.Trim()
                                                                      + " [응답코드]" + mNormalCarInfo.VanRescode);
                            TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormDeviceTest|CreditCardPayCancleResult", "카드 결제취소실패:" + mNormalCarInfo.VanResMsg);
                            lbl_Cardread1Status.Text = "카드 결제취소실패 : " + result.MSG.Trim();
                            NPSYS.Device.KICC_TIT.CardEject();
                            return;

                        }
                    }
                    //KICC DIP적용완료
                    else
                    {
                        lbl_Cardread1Status.Text = "리더기에 카드를 투입하여 주세요.";
                    }
                }
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                {
                    lbl_Cardread1Status.Text = "카드결제취소 요청";
                    mKIS_TITDIPDevice.CardApprovalCancle(NPSYS.Device.KisPosAgent, Creditpaymoneys.ToString(), CardAuthDate, CardAuthNumber);
                    Application.DoEvents();
                }
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_PAYMGATE || NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KOCES_TCM)
                {
                    if (KocesTcmMotor.CardState() == 2)
                    {
                        lbl_Cardread1Status.Text = "카드결제취소 요청";
                        if (CardAuthDate.Length == 6)
                        {
                            CardAuthDate = "20" + CardAuthDate;
                        }
                        KocesTcmMotor.CreditAuthSimpleExData result = KocesTcmMotor.CreditAuthSimpleExCancle(Creditpaymoneys, taxsResult, CardAuthNumber, CardAuthDate);
                        if (result == null || result.ResCode.Trim() == "")
                        {
                            TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayCancleResult", "신용카드 서버 접속안됨");
                            TextCore.DeviceError(TextCore.DEVICE.TCPIP, "FormPaymentMenu|CreditCardPayCancleResult", "신용카드 서버 접속안됨");

                        }
                        string returnCode = result.ResCode;
                        if (returnCode == "0000")
                        {
                            lbl_Cardread1Status.Text = "카드결제취소 완료";
                        }
                        else
                        {
                            lbl_Cardread1Status.Text = result.ResMsg;
                        }
                        Application.DoEvents();
                    }
                }
                //스마트로 TIT_DIP EV-CAT 적용
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    mSmartro_TITDIP_EVCat.CanclePayment(NPSYS.Device.Smartro_TITDIP_Evcat, Creditpaymoneys.ToString(), CardAuthDate, CardAuthNumber);
                }
                //스마트로 TIT_DIP EV-CAT 적용완료
                return;
            }
            catch (Exception ex)
            {
                lbl_Cardread1Status.Text = "결제취소 요청실패";
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|CreditCardPayCancleResult", "카드 결제취소중 예외상황:" + ex.ToString());
                return;
            }
            finally
            {

            }

        }

        private void btn_CardRead1100Pay_Click(object sender, EventArgs e)
        {
            CreditCardPay();
        }

        private void CreditCardPay()
        {
            lbl_Cardread1Status.Text = string.Empty;
            string terminalNo = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();


            int Creditpaymoneys = 100;

            //double tax = ((long)(Math.Floor((decimal)(Creditpaymoneys / 1.1))));
            int taxsResult = (int)(Creditpaymoneys / 11);
            int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가

            if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_DIP_IFM)
            {
                bool resultCardState = NPSYS.Device.KICC_TIT.GetCardInsert();
                if (resultCardState == false)
                {
                    lbl_Cardread1Status.Text = string.Empty;
                    lbl_Cardread1Status.Text = "카드를 넣어주시고 결제해주세요";
                    return;
                }
                else
                {
                    PayCardandCash m_PayCardandCash = new PayCardandCash();
                    NPSYS.Device.KICC_TIT.SendData_D1(Creditpaymoneys.ToString(), "0", "10");
                    KICC_TIT.KICC_TIT_RECV_SUCCESS result = NPSYS.Device.KICC_TIT.GetRecvData();
                    if (result == null || result.SUC.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormDeviceTest | CreditCardPay", "신용카드 서버 접속안됨");
                        lbl_Cardread1Status.Text = "신용카드 서버 접속안됨";
                    }
                    string returnCode = result.RS04;
                    if (result.SUC == "00" && returnCode == "0000")
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPay", "[카드결제성공]");

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        lbl_Cardread1Status.Text = "카드결제성공";
                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPay", "[카드결제실패]");
                        if (returnCode != null)
                        {
                            returnCode = "9999";
                        }
                        if (result.RS16 == null)
                        {
                            result.RS16 = string.Empty;
                        }
                        if (result.MSG == null)
                        {
                            result.MSG = string.Empty;
                        }
                        string CardResMsg = (result.RS16.Trim() == string.Empty ? result.MSG.Trim() : result.RS16.Trim());
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPay", "[카드결제실패]"
                                                                  + " [응답메세지]" + CardResMsg
                                                                  + " [메세지]" + result.MSG.Trim());

                        lbl_Cardread1Status.Text = "카드결제실패:" + CardResMsg;
                    }
                    NPSYS.Device.KICC_TIT.CardEject();

                }
                return;
            }
            // FIRSTDATA처리
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
            {

                FirstDataDip.readerStatus currentStatus = FirstDataDip.ReadState();
                if (currentStatus != FirstDataDip.readerStatus.ReaderIcIn)
                {
                    lbl_Cardread1Status.Text = string.Empty;
                    lbl_Cardread1Status.Text = "카드를 넣어주시고 결제해주세요";
                    return;
                }

                else
                {

                    FirstDataDip.CreditAuthSimpleExData result = FirstDataDip.CreditAuthSimpleEx(terminalNo, Creditpaymoneys.ToString());
                    FirstDataDip.CardEject();
                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormDeviceTest | CreditCardPay", "신용카드 서버 접속안됨");
                        lbl_Cardread1Status.Text = "신용카드 서버 접속안됨";

                    }
                    string returnCode = result.ResCode;
                    if (returnCode == FirstData.Success)
                    {
                        lbl_Cardread1Status.Text = "카드결제성공";
                    }
                    else
                    {

                        string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | CreditCardPay", "[카드결제실패]");
                        if (returnCode == null)
                        {
                            returnCode = "9999";
                        }

                        lbl_Cardread1Status.Text = "카드결제실패:" + result.ResMsg;

                    }


                }
                return;
            }
            // FIRSTDATA처리 완료
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                lbl_Cardread1Status.Text = "결제 요청";
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | Kis_TIT_DIpCardApprovalAction", "[승인쓰레드웨이트]");
          
                UsePayCancle = false;
                mNormalCarInfo.TotFee = Creditpaymoneys;
     
                if (!KisEventStart)
                {
                    KisEventStart = true;
                    NPSYS.Device.KisPosAgent.OnApprovalEnd += new EventHandler(KisPosAgent_OnApprovalEnd);
                    DoWork(NPSYS.Device.KisPosAgent);
                }
                Application.DoEvents();
            }
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.VAN_FIRSTDATA)
            {
                CreditAuthSimpleExData result = FirstData.CreditAuthSimpleEx(terminalNo, string.Empty, "S", Creditpaymoneys.ToString(), taxsResult.ToString());
                if (result == null || result.ResCode.Trim() == "")
                {
                    TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                    return;
                }
                string returnCode = result.ResCode;
                if (returnCode == FirstData.Success)
                {
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                    lbl_Cardread1Status.Text = "카드 결제성공";
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]"
                                                            + " [응답코드]" + returnCode.Trim()
                                                            + " [응답메세지]" + result.ResMsg.Trim());
                    lbl_Cardread1Status.Text = "카드 결제실패";
                }
            }
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_PAYMGATE || NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_TCM)
            {
                if (KocesTcmMotor.CardState() == 2)
                {
                    lbl_Cardread1Status.Text = "카드결제 요청";
                    KocesTcmMotor.CreditAuthSimpleExData result = KocesTcmMotor.CreditAuthSimpleEx(Creditpaymoneys, taxsResult);

                    if (result == null || result.ResCode.Trim() == "")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "신용카드 서버 접속안됨");
                        lbl_Cardread1Status.Text = "신용카드 서버 접속안됨";
                        return;
                    }
                    string returnCode = result.ResCode;
                    if (returnCode == "0000")
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                        lbl_Cardread1Status.Text = "카드 결제성공";
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "PayCardandCash | CreditCardPayResult", "[카드결제실패]"
                                                                + " [응답코드]" + returnCode.Trim()
                                                                + " [응답메세지]" + result.ResMsg.Trim());
                        lbl_Cardread1Status.Text = "카드 결제실패";
                    }
                }
            }
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat)
            {
                string errorMessage = string.Empty;
                bool isSend = mSmartroVCat.CardApproval(NPSYS.Device.SmtSndRcv, Creditpaymoneys, 600, ref errorMessage);
            }
            //스마트로 TIT_DIP EV-CAT 적용
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
            {
                mSmartro_TITDIP_EVCat.CardApproval(NPSYS.Device.Smartro_TITDIP_Evcat, Creditpaymoneys.ToString());
            }
            //스마트로 TIT_DIP EV-CAT 적용완료
            else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_TS141)
            {
                bool isApproveSuccess = KiccTs141.Approval(Creditpaymoneys.ToString(), taxsResult.ToString());

                if (isApproveSuccess)
                {
                    timerKiccTs141State.Enabled = true;
                    timerKiccTs141State.Start();
                }
            }
        }

        #region KiccTs141
        private void timerKiccTs141State_Tick(object sender, EventArgs e)
        {
            try
            {
                GetKiccState();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | timerKiccTs141State_Tick", ex.ToString());
            }
        }

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

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | GetKiccState", "[카드결제 성공]"
                        + " [TID단말기번호]" + tId
                        + " [승인번호]" + approvalNo
                        + " [승인시각]" + approvalDate
                        + " [매입사명]" + accquireName
                        + " [발급사명]" + issueName
                        + " [포스거래번호]" + posSequnceNo
                        + " [결제금액]" + cardMoney);
                    lbl_Cardread1Status.Text = "카드 결제성공";

                    timerKiccTs141State.Enabled = false;
                    timerKiccTs141State.Stop();
                }
                else // fallback거래등현재 확인결과 9999는 fallback으로보임
                {
                    KiccTs141.Initilize();
                    lbl_Cardread1Status.Text = "[카드결제실패]" + "카드를 빼시고 카드결제버튼을 눌러 다시 시도요청";
                }
            }
        }
        #endregion

        #region 스마트로
        /// <summary>
        /// 스마트로 추가 상태변화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnRcvState(object sender, AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEvent e)
        {
            string cardState = e.szType.ToString();
            string statusMessage = mSmartroVCat.OnReceiveStatusMessage(cardState);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드동작 변화]"
                                     + " 상태값:" + cardState.ToString()
                                     + " 상태명령:" + statusMessage
                                     );
            lbl_Cardread1Status.Text = statusMessage;
        }
        /// <summary>
        /// 스마트로추가 성공이던 실패든 카드결제 발생시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnTermComplete(object sender, EventArgs e)
        {
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
                                    + " 봉사료:" + currentSmatroReceiveData.ReceiveBongSaInx
                                    + " 승인일자:" + currentSmatroReceiveData.ReceiveAppYmd
                                    + " 승인시간:" + currentSmatroReceiveData.ReceiveAppHms
                                    + " 승인번호:" + currentSmatroReceiveData.RecieveApprovalNumber
                                                    );

            if (currentSmatroReceiveData.CurrentCmdType == SmartroVCat.SmatroData.CMDType.CardApprovalRespone)
            {
                if (currentSmatroReceiveData.Success)
                {
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");
                    lbl_Cardread1Status.Text = "카드 결제성공";
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.CARD_ERRPR, "FormPaymentMenu|CreditCardPayResult", "카드 결제실패 : " + currentSmatroReceiveData.ReceiveReturnMessage);
                    lbl_Cardread1Status.Text = "카드 결제실패 :" + currentSmatroReceiveData.ReceiveReturnMessage;
                }
            }
        }
        /// <summary>
        /// 카드취소등의동작시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SmtSndRcv_OnTermExit(object sender, EventArgs e)
        {
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
            lbl_Cardread1Status.Text = errorName;
        }
        #endregion

        #region KIS_TIT_DIP

        CardDeviceStatus mCardStatus = new CardDeviceStatus();
        private void KisPosAgent_OnApprovalEnd(object sender, EventArgs e)
        {
            //AxKisPosAgentLib.AxKisPosAgent axkisPosAgent = sender as AxKisPosAgentLib.AxKisPosAgent;
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
            //" ""01"" 카드 삽입 대기
            // ""02"" IC데이터 읽기 진행
            // ""03"" MS데이터 읽기 진행
            // ""04"" 카드 회수 대기
            // ""05"" IC데이터 읽기 실패"
            //  inputtime = paymentInputTimer;
            //AxKisPosAgentLib.AxKisPosAgent axkisPosAgent = sender as AxKisPosAgentLib.AxKisPosAgent;
            //string currentAgtState = axkisPosAgent.outAgtState;
            //int nCurrentAgtState = Int32.Parse(axkisPosAgent.outAgtState);
            //if (currentAgtState == "02" || currentAgtState == "03" || currentAgtState == "04")
            //{
            //    inputtime = paymentInputTimer;
            //}
            System.Threading.Thread.Sleep(200);
            switch (mCardStatus.currentCardReaderStatus)
            {
                case CardDeviceStatus.CardReaderStatus.None:
                    mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[최초 카드상태 체크]");
                    break;
                case CardDeviceStatus.CardReaderStatus.CardReady:
                    if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "1")
                    {
                        lbl_Cardread1Status.Text = "카드가 삽입되었습니다.";
                        mKIS_TITDIPDevice.PowerCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPowerCheck;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[카드 삽입됨]");
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "0")
                    {
                        lbl_Cardread1Status.Text = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "C")
                    {
                        lbl_Cardread1Status.Text = "카드 MST 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.InWCC = "S";
                        mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[삼성페이 읽기 진행]");
                    }
                    //else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC != "F")
                    //{
                    //    lbl_Cardread1Status.Text = "카드 IC 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[IC데이터 읽기 진행]");
                    //}
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "F")
                    {
                        lbl_Cardread1Status.Text = "카드 MS 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[MS데이터 읽기 진행]");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                    }
                    else
                    {
                        lbl_Cardread1Status.Text = "카드 삽입해 주시기 바랍니다...";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    break;
                case CardDeviceStatus.CardReaderStatus.CardPowerCheck:
                    //if (pKisPosAgent.outAgentData.Trim() == "1")
                    //{
                    //    lbl_Cardread1Status.Text = "카드 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[IC데이터 읽기 진행]");
                    //}
                    //else
                    //{
                    //    lbl_Cardread1Status.Text = "카드 확인 실패 되었습니다.";
                    //    mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    //}
                    lbl_Cardread1Status.Text = "카드 데이터를 읽는 중 입니다...";
                    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[IC데이터 읽기 진행]");
                    break;
                case CardDeviceStatus.CardReaderStatus.CardLockEject:
                    if (pKisPosAgent.outAgentData.Trim() == "1")
                    {
                        lbl_Cardread1Status.Text = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }
                    else
                    {
                        lbl_Cardread1Status.Text = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    }
                    break;
                case CardDeviceStatus.CardReaderStatus.CardReadyEnd:
                    lbl_Cardread1Status.Text = "카드[IC] 데이터를 읽고 있습니다.";
                    if (pKisPosAgent.outAgentData.Trim() == "CF")
                    {
                        mKIS_TITDIPDevice.InWCC = "F";
                        //mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                        //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[MS데이터 읽기 진행]");
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest | DoWork", "[IC 카드 리딩 실패]");
                    }
                    else if (pKisPosAgent.outAgentData.Trim().Length == 2)
                    {
                        lbl_Cardread1Status.Text = "카드결제 실패";
                        //mKIS_TITDIPDevice.CardLockEjectFinish(pKisPosAgent);
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[카드리딩 실패]");
                    }
                    else
                    {
                        if (pKisPosAgent.outAgentData.Substring(7, 1).Equals("M"))
                        {
                            mKIS_TITDIPDevice.InWCC = "S";
                        }
                        lbl_Cardread1Status.Text = "승인 진행 중입니다.";
                        //payData.outReaderData = axKisPosAgent1.outAgentData.Substring(0, 6);
                        if (UsePayCancle)
                        {
                            mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString(), gCardAuthDate, gCardAuthNumber);
                        }
                        else
                        {
                            mKIS_TITDIPDevice.CardApproval(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                        }
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[카드 결제요청 진행]");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                    }
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);

                    break;
                case CardDeviceStatus.CardReaderStatus.CardFullBack:
                    lbl_Cardread1Status.Text = "카드[MS] 결제 요청중 입니다.";
                    mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString(), gCardAuthDate, gCardAuthNumber);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                    break;
                case CardDeviceStatus.CardReaderStatus.CardApproval:
                    KisPosAgent_OnAgtComplete(pKisPosAgent);
                    break;
                case CardDeviceStatus.CardReaderStatus.CardLockEjectFinish:
                    lbl_Cardread1Status.Text = "카드를 뽑아주세요.";
                    mKIS_TITDIPDevice.StatusCheckFinish(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish;
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);
                    break;
                case CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish:
                    lbl_Cardread1Status.Text = "카드를 뽑아 주세요";
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
                        lbl_Cardread1Status.Text = "결제가 완료되었습니다";
                        //Delay(2000);
                        //this.Close();
                    }
                    //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtState", "[카드 회수 대기]");
                    break;
            }
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
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | KisPosAgent_OnAgtComplete", "[카드결제 성공]");
                    lbl_Cardread1Status.Text = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //mNormalCarInfo.PaymentMethod = NormalCarInfo.PaymentType.CreditCard;
                    ////LPRDbSelect.LogMoney(logDate, mNormalCarInfo, MoneyType.CreditCard, mNormalCarInfo.PaymentMoney, 0, "");
                    ////string[] lCardNumData = mKisSpec.outCardNo.Split('=');
                    ////if (lCardNumData[0].Length > 13)
                    ////{
                    ////    mNormalCarInfo.CardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    ////}
                    ////else
                    ////{
                    ////    mNormalCarInfo.CardNumber = lCardNumData[0];
                    ////}
                    //mNormalCarInfo.CardNumber = pKisPosAgent.outCardNo;
                    //mNormalCarInfo.CardAuthNumber = mKIS_TITDIPDevice.KisSpec.outAuthNo.Trim();
                    //mNormalCarInfo.CardAUthDate = mKIS_TITDIPDevice.KisSpec.outReplyDate;
                    //mNormalCarInfo.CardRescode = mKIS_TITDIPDevice.KisSpec.outReplyCode;
                    //mNormalCarInfo.CardResMsg = mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    ////if (mKisSpec.outVatAmt.Trim() == string.Empty)
                    ////{
                    ////    mKisSpec.outVatAmt = "0";
                    ////}
                    ////mNormalCarInfo.SupplyPay = (Convert.ToInt32(mKisSpec.outTranAmt) - Convert.ToInt32(mKisSpec.outVatAmt));
                    ////mNormalCarInfo.TaxPay = Convert.ToInt32(mKisSpec.outVatAmt);
                    //mNormalCarInfo.SupplyPay = 0;
                    //mNormalCarInfo.TaxPay = 0;
                    //mNormalCarInfo.CardName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    //mNormalCarInfo.BeforeCardPay = mNormalCarInfo.PaymentMoney;
                    //mNormalCarInfo.CardApproveYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    //mNormalCarInfo.CardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                    //mNormalCarInfo.CardApprovalYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    //mNormalCarInfo.CardApprovalHms = DateTime.Now.ToString("HH:mm:ss");
                    //mNormalCarInfo.CardDDDNumber = string.Empty;
                    ////만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                    //mNormalCarInfo.CardBankCode = mKIS_TITDIPDevice.KisSpec.outIssuerCode;
                    //mNormalCarInfo.CardBankName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    //mNormalCarInfo.CardAcquirerCode = mKIS_TITDIPDevice.KisSpec.outAccepterCode;
                    //mNormalCarInfo.CardAcquirerName = mKIS_TITDIPDevice.KisSpec.outAccepterName;
                    ////만료차량 정기권요금제에서 일반요금제 변경기능주석완료

                    //mNormalCarInfo.CardApprovalType = "1";
                    //mNormalCarInfo.CardVanType = 1;
                    //mNormalCarInfo.CardRescode = "0000";
                    ////LPRDbSelect.Creditcard_Log_INsert(mNormalCarInfo);
                    //mNormalCarInfo.CardPay = mNormalCarInfo.PaymentMoney;
                    //if (UsePayCancle)
                    //{
                    //    LPRDbSelect.PayCancleProc(mNormalCarInfo);
                    //}
                    //else
                    //{
                    //    LPRDbSelect.SaveCardPay(mNormalCarInfo);
                    //}
                    mKIS_TITDIPDevice.CardLockEjectFinish(NPSYS.Device.KisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEjectFinish;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제성공");
                }
                else if (mKIS_TITDIPDevice.KisSpec.outReplyCode == "8100")// 사용자취소
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    //KIS 할인처리시 처리문제
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess;
                    //응답코드:8000 타입아웃
                    //응답코드:8326 한도초과
                    //KIS 할인처리시 처리문제주석완료
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    DoWork(pKisPosAgent);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행완료" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    return;
                }
                else // 카드결제실패
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1);
                    lbl_Cardread1Status.Text = "카드결제실패:" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    DoWork(pKisPosAgent);
                    //mKIS_TITDIPDevice.StartSoundTick = 7;


                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | KisPosAgent_OnAgtComplete", ex.ToString());
            }
        }
        private void SetKisDipIFM()
        {
            lbl_Cardread1Status.Text = string.Empty;
            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            mKIS_TITDIPDevice.InWCC = "C";
            mKIS_TITDIPDevice.VanIP = NPSYS.gVanIp;
            mKIS_TITDIPDevice.VanPort = Convert.ToInt16(NPSYS.gVanPort);
            //NPSYS.Device.KisPosAgent.OnApprovalEnd += new EventHandler(KisPosAgent_OnApprovalEnd);
            mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);
        }

        public void UnSetKisDipIFM()
        {
            if (KisEventStart)
            {
                NPSYS.Device.KisPosAgent.OnApprovalEnd -= new EventHandler(KisPosAgent_OnApprovalEnd);
            }
        }
        #endregion



        private void btnMotorBarcodeReding_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                lblBarcodeData.Text = string.Empty;
                lblBarcodeStatus.Text = string.Empty;
                BarcodeMoter.BarcodeMotorResult currentResult = NPSYS.Device.BarcodeMoter.GetRead();
                lblBarcodeStatus.Text = currentResult.ResultStatus.ToString();
                lblBarcodeData.Text = currentResult.Data;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | btnMotorBarcodeReding_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }


        }

        private void btnMotorBarcodeOut_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                lblBarcodeData.Text = string.Empty;
                lblBarcodeStatus.Text = string.Empty;
                BarcodeMoter.BarcodeMotorResult currentResult = NPSYS.Device.BarcodeMoter.EjectFront();
                lblBarcodeStatus.Text = currentResult.ResultStatus.ToString();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | btnMotorBarcodeOut_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnMotorBarcodeInsert_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                lblBarcodeData.Text = string.Empty;
                lblBarcodeStatus.Text = string.Empty;
                BarcodeMoter.BarcodeMotorResult currentResult = NPSYS.Device.BarcodeMoter.EjectRear();
                lblBarcodeStatus.Text = currentResult.ResultStatus.ToString();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | btnMotorBarcodeInsert_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnMotorBarcodeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                lblBarcodeData.Text = string.Empty;
                lblBarcodeStatus.Text = string.Empty;
                BarcodeMoter.BarcodeMotorResult currentResult = NPSYS.Device.BarcodeMoter.GetStatus();
                lblBarcodeStatus.Text = currentResult.ResultStatus.ToString();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | btnMotorBarcodeStatus_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnMotorBarcodeReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                lblBarcodeData.Text = string.Empty;
                lblBarcodeStatus.Text = string.Empty;
                BarcodeMoter.BarcodeMotorResult currentResult = NPSYS.Device.BarcodeMoter.SetReset();
                lblBarcodeStatus.Text = currentResult.ResultStatus.ToString();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | btnMotorBarcodeReset_Click", ex.ToString());
            }
            finally
            {
                this.Enabled = true;
            }
        }


        // 카드결제취소 적용완료

        //장비테스트 보강 주석완료

        //스마트로 TIT_DIP EV-CAT 적용
        #region 스마트로 TIT_DIP EV-CAT
        private void SmartroEvcat_QueryResults(object sender, AxDreampos_Ocx.__DP_Certification_Ocx_QueryResultsEvent e)
        {
            try
            {
                if (e.returnData == "FALL BACK 발생" || e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults", " 이벤트 [" + e.returnData + "] ");
                    return;
                }
                if (e.returnData == "이미 요청중입니다!")
                {
                    lbl_Cardread1Status.Text = "이미 요청중임";
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults", " 이벤트 [" + e.returnData + "] ");
                    return;
                }

                if (string.IsNullOrEmpty(e.returnData))
                {
                    lbl_Cardread1Status.Text = "응답값 없어 처리를 안함";
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " 응답값 없어 처리를 안함 ");
                }
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults [수신 전문 파싱 시작] ", e.returnData);
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
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " EV-CAT 데몬 정상실행 확인 ");
                            return;
                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "00")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            lbl_Cardread1Status.Text = "카드리더기 정상연결됨";
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " 리더기 정상연결 ");
                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "RESET")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            lbl_Cardread1Status.Text = "카드리더기 리셋(강제배출) 성공";
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " 카드리더기 리셋(강제배출) 성공 ");
                            return;
                        }
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", "리더기 상태 " + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                        return;
                    }
                    if ((mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "E" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "N")
                        && mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg.Length >= 18)
                    {
                        mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);

                        if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y")
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " [카드결제/취소 성공] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 승인번호 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo + "] "
                                + " 승인일시 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate + " " + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime + "] "
                                + " 매입사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName + "] "
                                + " 발급사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName + "] "
                                + " 결제금액 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPayMoney + "] ");

                            lbl_Cardread1Status.Text = "결제가성공하였습니다";

                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormDeviceTest || SmartroEvcat_QueryResults ", "카드 결제성공");
                        }
                        else
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormDeviceTest || SmartroEvcat_QueryResults ", " [카드결제/취소 실패] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 실패사유 [" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg + "] ");


                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormDeviceTest || SmartroEvcat_QueryResults ", "카드 결제실패" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                            lbl_Cardread1Status.Text = "카드결제실패:" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;
                        }
                    }
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData ", " 비정상 응답 [ " + e.returnData + " ]");
                    lbl_Cardread1Status.Text = "비정상 응답: " + e.returnData;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SMATRO_TIT_DIP_EV-CAT || ParsingData ", " 전문 파싱중 예외상황 [ " + ex.Message + " ]");
                lbl_Cardread1Status.Text = "예외상황발생:" + ex.Message;
            }

        }

        private void btnTestCahngeMoney_Click(object sender, EventArgs e)
        {
            mSmartro_TITDIP_EVCat.ChangePayMoney(NPSYS.Device.Smartro_TITDIP_Evcat, "200");
        }


        #endregion
        //스마트로 TIT_DIP EV-CAT 적용완료

        #region 영수증 관련

        private void btnReceiptStatus_Click(object sender, EventArgs e)
        {
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormDeviceTest | btnReceiptStatus_Click", "[프린터상태확인 버튼누름]");
            PrintStatus();
        }
        private void btnReceiptPrint_Click(object sender, EventArgs e)
        {
            NPSYS.buttonSoundDingDong();

            try
            {
                btnTestPrint.Enabled = false;
                if (reciptPrintyn == false)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormAdminReceiptSetting|ReceiptButton_Siganl_Event", "reciptPrintyn 상태:" + reciptPrintyn.ToString());
                }
                reciptPrintyn = false;
                PrintAction();
            }
            catch
            {
            }
            finally
            {
                btnTestPrint.Enabled = true;
                reciptPrintyn = true;
            }
        }

        void DoSensors_DosensorSignalEvent(object sender, GoodTechContorlBoard.SignalType p_SignalType)
        {
            if (p_SignalType == GoodTechContorlBoard.SignalType.ReciptSignal)
            {
                if (btnTestPrint.Enabled == false)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormAdminReceiptSetting|ReceiptButton_Siganl_Event", "영수증 장비버튼 눌렀지만 아직 영수증출력 차례가 아니라서 무시");
                }

                if (reciptPrintyn == false)
                {
                    return;
                }
                try
                {
                    reciptPrintyn = false;
                    PrintAction();
                    TextCore.ACTION(TextCore.ACTIONS.USER, "FormAdminReceiptSetting|ReceiptButton_Siganl_Event", "영수증 장비버튼 누름");
                }
                catch
                {
                }
                finally
                {
                    reciptPrintyn = true;

                }
            }
        }

        private void PrintAction()
        {
            try
            {
                btnTestPrint.Enabled = false;
                int valueSpace = 38;
                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    NPSYS.Device.HMC60.FontSize(2, 2);
                    NPSYS.Device.HMC60.Print("주차요금 영수증\n");

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    NPSYS.Device.HMC60.Print("   주차장:" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    NPSYS.Device.HMC60.Print("   사업자:" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    NPSYS.Device.HMC60.Print("   주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    NPSYS.Device.HMC60.Print("   전화번호:" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    NPSYS.Device.HMC60.Print("   정산기명:" + NPSYS.BoothName + "\n");
                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    NPSYS.Device.HMC60.FontSize(2, 1);
                    NPSYS.Device.HMC60.Print("   *테스트영수증*\n\n");
                    NPSYS.Device.HMC60.Print(" 차량번호" + TextCore.ToRightAlignString(15, "서울11가1111") + "\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   입차일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash("20120101") + " " + NPSYS.ConvetDay_Dash("111010")) + "\n");
                    NPSYS.Device.HMC60.Print("   정산일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash("20120101") + " " + NPSYS.ConvetDay_Dash("122010")) + "\n");
                    NPSYS.Device.HMC60.Print("   주차시간" + TextCore.ToRightAlignString(valueSpace, string.Format("{0}일 {1}시간 {2}분", 0, 1, 10)) + "\n");
                    NPSYS.Device.HMC60.Print("   주차요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(1000) + "원") + "\n");
                    NPSYS.Device.HMC60.Print("   할인요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(500) + "원") + "\n");
                    NPSYS.Device.HMC60.Print("   -------------------------------------------------\n");

                    NPSYS.Device.HMC60.FontSize(2, 1);
                    NPSYS.Device.HMC60.Print(" 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(500) + "원") + "\n");

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    string paymentMethod = "";
                    paymentMethod = "신용카드";

                    NPSYS.Device.HMC60.Print("   할인명  " + TextCore.ToRightAlignString(valueSpace, "장례식장") + "\n");


                    string l_CardNumber = "****-****-****-1111";
                    NPSYS.Device.HMC60.Print("   지불방식" + TextCore.ToRightAlignString(valueSpace, paymentMethod) + "\n");

                    NPSYS.Device.HMC60.Print("   승인번호" + TextCore.ToRightAlignString(valueSpace, "11111111") + "\n");

                    NPSYS.Device.HMC60.Print("   카드명  " + TextCore.ToRightAlignString(valueSpace, "하나SK카드") + "\n");

                    NPSYS.Device.HMC60.Print("   카드번호" + TextCore.ToRightAlignString(valueSpace, l_CardNumber) + "\n");

                    NPSYS.Device.HMC60.Print("   승인일자" + TextCore.ToRightAlignString(valueSpace, "2012.01.02") + "\n");

                    NPSYS.Device.HMC60.Print("   공급가액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(900) + "원") + "\n");

                    NPSYS.Device.HMC60.Print("   부가세액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(100) + "원") + "\n");

                    NPSYS.Device.HMC60.Print("   청구액  " + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(1000) + "원") + "\n");





                    NPSYS.Device.HMC60.Print("   =================================================\n");

                    NPSYS.Device.HMC60.Print("   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpace - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기") + "\n");

                    NPSYS.Device.HMC60.Print("   이용해 주셔서 감사합니다." + "\n");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    valueSpace = 28;
                    NPSYS.Device.HMC60.FontSize(2, 2);
                    NPSYS.Device.HMC60.Print("주차요금 영수증\n");

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("====================================\n");
                    NPSYS.Device.HMC60.Print("주차장:" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    NPSYS.Device.HMC60.Print("사업자:" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    NPSYS.Device.HMC60.Print("주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    NPSYS.Device.HMC60.Print("전화번호:" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    NPSYS.Device.HMC60.Print("정산기명:" + NPSYS.BoothName + "\n");
                    NPSYS.Device.HMC60.Print("====================================\n");
                    NPSYS.Device.HMC60.FontSize(2, 1);
                    NPSYS.Device.HMC60.Print("   *테스트영수증*\n\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("차량번호" + TextCore.ToRightAlignString(valueSpace, "서울11가1111") + "\n");
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("입차일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash("20120101") + " " + NPSYS.ConvetDay_Dash("111010")) + "\n");
                    NPSYS.Device.HMC60.Print("정산일시" + TextCore.ToRightAlignString(valueSpace, NPSYS.ConvetYears_Dash("20120101") + " " + NPSYS.ConvetDay_Dash("122010")) + "\n");
                    NPSYS.Device.HMC60.Print("주차시간" + TextCore.ToRightAlignString(valueSpace, string.Format("{0}일 {1}시간 {2}분", 0, 1, 10)) + "\n");
                    NPSYS.Device.HMC60.Print("주차요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(1000) + "원") + "\n");
                    NPSYS.Device.HMC60.Print("할인요금" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(500) + "원") + "\n");
                    NPSYS.Device.HMC60.Print("------------------------------------\n");

                    NPSYS.Device.HMC60.FontSize(2, 1);
                    NPSYS.Device.HMC60.Print("정산요금" + TextCore.ToRightAlignString(10, TextCore.ToCommaString(999999) + "원") + "\n");

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    string paymentMethod = "";
                    paymentMethod = "신용카드";

                    NPSYS.Device.HMC60.Print("할인명  " + TextCore.ToRightAlignString(valueSpace, "장례식장") + "\n");


                    string l_CardNumber = "****-****-****-1111";
                    NPSYS.Device.HMC60.Print("지불방식" + TextCore.ToRightAlignString(valueSpace, paymentMethod) + "\n");

                    NPSYS.Device.HMC60.Print("승인번호" + TextCore.ToRightAlignString(valueSpace, "11111111") + "\n");

                    NPSYS.Device.HMC60.Print("카드명  " + TextCore.ToRightAlignString(valueSpace, "하나SK카드") + "\n");

                    NPSYS.Device.HMC60.Print("카드번호" + TextCore.ToRightAlignString(valueSpace, l_CardNumber) + "\n");

                    NPSYS.Device.HMC60.Print("승인일자" + TextCore.ToRightAlignString(valueSpace, "2012.01.02") + "\n");

                    NPSYS.Device.HMC60.Print("공급가액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(900) + "원") + "\n");

                    NPSYS.Device.HMC60.Print("부가세액" + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(100) + "원") + "\n");

                    NPSYS.Device.HMC60.Print("청구액  " + TextCore.ToRightAlignString(valueSpace, TextCore.ToCommaString(1000) + "원") + "\n");





                    NPSYS.Device.HMC60.Print("====================================\n");

                    NPSYS.Device.HMC60.Print(DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpace - 2, NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기") + "\n");

                    NPSYS.Device.HMC60.Print("이용해 주셔서 감사합니다." + "\n");

                }


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|RecipePrint", "영수증 출력에러:" + ex.ToString());
            }
            finally
            {
                NPSYS.Device.HMC60.Feeding(25);
                System.Threading.Thread.Sleep(200);
                if (NPSYS.g_UsePrintFullCuting)
                {
                    NPSYS.Device.HMC60.FullCutting();
                }
                else
                {
                    NPSYS.Device.HMC60.ParticalCutting();
                }

                btnTestPrint.Enabled = true;
            }
        }
        private void PrintStatus()
        {
            if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.NONE)
            {
                lblPrinter.ForeColor = mDeviceNotUseColor;
                return;
            }
            try
            {
                HMC60.HmcStatus currentStatus = NPSYS.Device.HMC60.HmcGetStatus();

                for (int i = 0; i < 10; i++)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                if (currentStatus == HMC60.HmcStatus.Communcation)
                {
                    NPSYS.Device.HMC60.CurrentPrinterStatus.SetIsComuniCationOk(false);
                    lblPrinter.ForeColor = mDeviceFailColor;
                    lblPrintCommuication.Text = mDeviceFail;
                    lblPrintCommuication.ForeColor = mDeviceFailColor;
                    return;
                }
                else
                {
                    NPSYS.Device.HMC60.CurrentPrinterStatus.SetIsComuniCationOk(true);
                    lblPrinter.ForeColor = mDeviceSuccessColor;
                    lblPrintCommuication.Text = mDeviceSuccess;
                    lblPrintCommuication.ForeColor = mDeviceSuccessColor;
                }
                GetPrintStatusText();
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormDeviceTest | PrintStatus", ex.ToString());
            }

        }

        public void GetPrintStatusText()
        {
            if (NPSYS.Device.HMC60.CurrentPrinterStatus.IsComuniCationError)
            {
                lblPrintCommuication.ForeColor = mDeviceFailColor;
                lblPrintCommuication.Text = mDeviceFail;
            }
            else
            {
                lblPrintCommuication.ForeColor = mDeviceSuccessColor;
                lblPrintCommuication.Text = mDeviceSuccess;

            }



            if (NPSYS.Device.HMC60.CurrentPrinterStatus.IsCutterUpError)
            {
                lblPrintHeadUpStatus.ForeColor = mDeviceFailColor;
                lblPrintHeadUpStatus.Text = mDeviceFail;
            }
            else
            {
                lblPrintHeadUpStatus.ForeColor = mDeviceSuccessColor;
                lblPrintHeadUpStatus.Text = mDeviceSuccess;

            }

            if (NPSYS.Device.HMC60.CurrentPrinterStatus.IsPageNearError)
            {
                lblPrintPageLackStatus.ForeColor = mDeviceFailColor;
                lblPrintPageLackStatus.Text = mDeviceFail;
            }
            else
            {
                lblPrintPageLackStatus.ForeColor = mDeviceSuccessColor;
                lblPrintPageLackStatus.Text = mDeviceSuccess;

            }

            if (NPSYS.Device.HMC60.CurrentPrinterStatus.IsPageEmptyError)
            {
                lblPrintPageEmptyStatus.ForeColor = mDeviceFailColor;
                lblPrintPageEmptyStatus.Text = mDeviceFail;
            }
            else
            {
                lblPrintPageEmptyStatus.ForeColor = mDeviceSuccessColor;
                lblPrintPageEmptyStatus.Text = mDeviceSuccess;

            }
        }

        #endregion

        #region 넥스파 센서
        private void btnSensorStatus_Click(object sender, EventArgs e)
        {
            GetSensorStatus();
        }

        private string mJapanOpen = "열림";
        private string mJapanClose = "닫힘";
        private void GetSensorStatus()
        {
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormDeviceTest | GetSensorStatus", "[센서확인 버튼누름]");
            if (NPSYS.Device.UsingSettingControlBoard != ConfigID.ControlBoardType.NEXPA)
            {
                lblSensor.ForeColor = mDeviceNotUseColor;
                return;
            }
            try
            {
                this.Enabled = false;
                if (NPSYS.Device.gIsUseDidoDevice)
                {
                    if (NPSYS.ControlBoardRecovery == ConfigID.ErrorRecoveryType.MANUAL) // auto가아니면
                    {
                        NPSYS.Device.NexpaDoSensor.isAutoRecovery = true;
                    }
                    NPSYS.Device.NexpaDoSensor.SendCallBoarStatusTimeOut();
                    GetSensorText();
                    if (NPSYS.ControlBoardRecovery == ConfigID.ErrorRecoveryType.MANUAL) // auto가아니면
                    {
                        NPSYS.Device.NexpaDoSensor.isAutoRecovery = false;
                    }



                }
                else
                {
                    lblSensor.ForeColor = mDeviceFailColor;
                    lblSenSorCommuniCation.ForeColor = mDeviceFailColor;
                    lblSenSorCommuniCation.Text = mDeviceFail;
                }
            }
            catch
            {
            }
            finally
            {
                this.Enabled = true;
            }




        }


        public void GetSensorText()
        {
            if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.NEXPA)
            {
                if (NPSYS.Device.gIsUseDidoDevice)
                {
                    if (NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.isDoorOff)
                    {
                        lblDoor1.Text = mJapanOpen;
                        lblDoor1.ForeColor = mDeviceFailColor;

                    }
                    else
                    {
                        lblDoor1.Text = mJapanClose;
                        lblDoor1.ForeColor = mDeviceSuccessColor;

                    }
                    if (NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.heaterOn)
                    {
                        lblSensorHeater.Text = "동작";

                    }
                    else
                    {
                        lblSensorHeater.Text = "중지";
                    }

                    if (NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.fanOn)
                    {
                        lblSensorFan.Text = "동작";

                    }
                    else
                    {
                        lblSensorFan.Text = "중지";
                    }
                    lblSensorTemp.Text = NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.tempAir;
                }
                else
                {
                    lblSensor.BackColor = mDeviceFailColor;
                }
            }
            else
            {
                lblSensor.BackColor = mDeviceNotUseColor;
                btnSensorStatus.Enabled = false;
            }
        }




        #endregion

        private void Label_Click(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {


                SimpleLabel control = sender as SimpleLabel;

                if (control != null)
                {
                    control.BackColor = Color.FromArgb(201, 201, 202);
                    npPad.LinkedSimpleLabel = control;
                }

                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is SimpleLabel)
                    {
                        if (ctrl != control)
                        {
                            ctrl.BackColor = Color.FromKnownColor(KnownColor.Window);
                        }
                    }
                }
            }
        }


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
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditMain | SetLanguage", controlItem.Name.ToString());
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

        private void SetLanuageDynamic(ConfigID.LanguageType pLanguageType)
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

        private void btn3500_Setting_Click(object sender, EventArgs e)
        {
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

            bool isSuccess = true;
            if (NPSYS.Device.TmoneySmartro3500.IsConnect == false)
            {
                isSuccess = NPSYS.Device.TmoneySmartro3500.Connect();
            }

            if (isSuccess)
            {
                //4. 설정 정보 셋팅 전문 송신
                NPSYS.Device.TmoneySmartro3500.RequestInitSetting();
            }
        }

        private void btn3500_Check_Click(object sender, EventArgs e)
        {
            //5. 장치 체크 전문 송신
            NPSYS.Device.TmoneySmartro3500.RequestVersionCheck();
        }
    }
}

