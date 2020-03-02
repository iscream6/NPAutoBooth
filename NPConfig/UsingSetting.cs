using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FadeFox.Database.SQLite;
using FadeFox.Security;
using FadeFox.Utility;
using FadeFox.Text;
using NPCommon;

namespace NPConfig
{
    public partial class UsingSetting : Form
    {
        string mConfigFilePath = "";
        ConfigDB3I mConfig = null;


        public UsingSetting(string pConfigfilePath)
        {
            InitializeComponent();
            mConfigFilePath = pConfigfilePath;
            mConfig = new ConfigDB3I(mConfigFilePath);
        }


        private void UsingSetting_Load(object sender, EventArgs e)
        {

            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.None.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KICC_DIP_IFM.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KIS_TIT_DIP_IFM.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.SmartroVCat.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KOCES_TCM.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KOCES_PAYMGATE.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KICC_TS141.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.FIRSTDATA_DIP.ToString());
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.KSNET.ToString());
            //스마트로 TIT_DIP EV-CAT 적용
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.SMATRO_TIT_DIP.ToString());
            //스마트로 TIT_DIP EV-CAT 적용완료
            //Tmoney 스마트로 적용
            cbxCardreaderType1.Items.Add(ConfigID.CardReaderType.SMATRO_TL3500S.ToString());
            //Tmoney 스마트로 적용완료
            cbxCardreaderType2.Items.Add(ConfigID.CardReaderType.None.ToString());
            cbxCardreaderType2.Items.Add(ConfigID.CardReaderType.TItMagnetincDiscount.ToString());



            cbxBarcodereaderType.Items.Add(ConfigID.BarcodeReaderType.None.ToString());
            cbxBarcodereaderType.Items.Add(ConfigID.BarcodeReaderType.NormaBarcode.ToString());
            cbxBarcodereaderType.Items.Add(ConfigID.BarcodeReaderType.SVS2000.ToString());
   

            cbxPrintType.Items.Add(ConfigID.PrinterType.NONE.ToString());
            cbxPrintType.Items.Add(ConfigID.PrinterType.HMK054.ToString());
            cbxPrintType.Items.Add(ConfigID.PrinterType.HMK825.ToString());

            cbxControlBoard.Items.Add(ConfigID.ControlBoardType.NONE.ToString());
            cbxControlBoard.Items.Add(ConfigID.ControlBoardType.GOODTECH.ToString());
            cbxControlBoard.Items.Add(ConfigID.ControlBoardType.NEXPA.ToString());


            chkBill.Checked = (mConfig.GetValue(ConfigID.UsingSettingBill) == "Y" ? true : false);
            //할인권 타입설정 추가
            rbNoDCTicket.Checked = (mConfig.GetValue(ConfigID.UsingSettingDiscountCard) == "Y" ? false : true);
            rbMagneticDCTicket.Checked = (mConfig.GetValue(ConfigID.UsingSettingMagneticDCTicket) == "Y" ? true : false);
            rbBarcodeDCTicket.Checked = (mConfig.GetValue(ConfigID.UsingSettingBarcodeDCTicket) == "Y" ? true : false);
            //할인권 타입설정 추가완료

            chkRestFulServer.Checked = (mConfig.GetValue(ConfigID.UsingSettingRestFul) == "Y" ? true : false);

            //바코드모터드리블 사용
            string barcodeType = mConfig.GetValue(ConfigID.UsingSettingDiscountBarcode);
            if (barcodeType.Trim() == string.Empty)
            {
                barcodeType = ConfigID.BarcodeReaderType.None.ToString();
            }
            else if (barcodeType.Trim() == "Y")
            {
                barcodeType = ConfigID.BarcodeReaderType.NormaBarcode.ToString();
            }
            else if (barcodeType.Trim() == "N")
            {
                barcodeType = ConfigID.BarcodeReaderType.None.ToString();
            }
            cbxBarcodereaderType.Text = barcodeType.ToString();
            //chkDiscountBarcode.Checked = (mConfig.GetValue(ConfigID.UsingSettingDiscountBarcode) == "Y" ? true : false);
            //바코드모터드리블 사용완료


            chkBillReader.Checked = (mConfig.GetValue(ConfigID.UsingSettingBillReader) == "Y" ? true : false);
            chkCoinReader.Checked = (mConfig.GetValue(ConfigID.UsingSettingCoinReader) == "Y" ? true : false);
 
            chkCash50Use.Checked = (mConfig.GetValue(ConfigID.Cash50Use) == "Y" ? true : false);
            chkCash100Use.Checked = (mConfig.GetValue(ConfigID.Cash100Use) == "N" ? false : true);
            chkCash500Use.Checked = (mConfig.GetValue(ConfigID.Cash500Use) == "N" ? false : true);

            string printerType = mConfig.GetValue(ConfigID.UsingSettingPrinterType);
            printerType = printerType.Trim() == string.Empty ? NPCommon.ConfigID.PrinterType.NONE.ToString() : printerType.Trim();
            NPCommon.ConfigID.PrinterType currentPrintType = (ConfigID.PrinterType)Enum.Parse(typeof(ConfigID.PrinterType), printerType);
            cbxPrintType.Text = currentPrintType.ToString();


            chkTmoney.Checked = (mConfig.GetValue(ConfigID.UsingSettingTmoney) == "Y" ? true : false);
            chkHubulMoney.Checked = (mConfig.GetValue(ConfigID.UsingSettingHubulMoney) == "Y" ? true : false);
            chkTTS.Checked = (mConfig.GetValue(ConfigID.UsingSettingTTS) == "Y" ? true : false);
            chkSound.Checked = (mConfig.GetValue(ConfigID.UsingSettingSoundRead) == "Y" ? true : false);

            string controlBoardType = mConfig.GetValue(ConfigID.UsingSettingControlBoard);
            if (controlBoardType == "N" || controlBoardType == string.Empty)
            {
                controlBoardType = NPCommon.ConfigID.ControlBoardType.NONE.ToString();
            }
            NPCommon.ConfigID.ControlBoardType currentContorlBoardType = (ConfigID.ControlBoardType)Enum.Parse(typeof(ConfigID.ControlBoardType), controlBoardType);
            cbxControlBoard.Text = currentContorlBoardType.ToString();


            // 견광등 로직관련

            // 신분증인식기 적용
            chkSinbunReader.Checked = (mConfig.GetValue(ConfigID.UsingSettingSinbunReader) == "Y" ? true : false);
            // 신분증인식기 적용완료


            string cardrederLeft = mConfig.GetValue(ConfigID.UsingSettingCardRederTypeLeft);

            cardrederLeft = cardrederLeft.Trim() == string.Empty ? NPCommon.ConfigID.CardReaderType.None.ToString() : cardrederLeft.Trim();
            NPCommon.ConfigID.CardReaderType currentCardreaderLeft = (NPCommon.ConfigID.CardReaderType)Enum.Parse(typeof(NPCommon.ConfigID.CardReaderType), cardrederLeft);
            cbxCardreaderType1.Text = currentCardreaderLeft.ToString();

            string cardrederRight = mConfig.GetValue(ConfigID.UsingSettingCardRederTypeRight);

            cardrederRight = cardrederRight.Trim() == string.Empty ? NPCommon.ConfigID.CardReaderType.None.ToString() : cardrederRight.Trim();
            NPCommon.ConfigID.CardReaderType currentcardrederRight = (NPCommon.ConfigID.CardReaderType)Enum.Parse(typeof(NPCommon.ConfigID.CardReaderType), cardrederRight);
            cbxCardreaderType2.Text = currentcardrederRight.ToString();
       


        }

        private void btnOk_Click(object sender, EventArgs e)
        {
    
            mConfig.SetValue(ConfigID.UsingSettingBillReader, (chkBillReader.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingCoinReader, (chkCoinReader.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.Cash50Use, (chkCash50Use.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.Cash100Use, (chkCash100Use.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.Cash500Use, (chkCash500Use.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingBill, (chkBill.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingRestFul, (chkRestFulServer.Checked ? "Y" : "N"));
            NPCommon.ConfigID.PrinterType currentPrintType = (ConfigID.PrinterType)Enum.Parse(typeof(ConfigID.PrinterType),cbxPrintType.Text);
            switch (currentPrintType)
            {
                case  ConfigID.PrinterType.NONE:
                    mConfig.SetValue(ConfigID.UsingSettingPrinterType, NPCommon.ConfigID.PrinterType.NONE.ToString());
                    break;
                case ConfigID.PrinterType.HMK054:
                    mConfig.SetValue(ConfigID.UsingSettingPrinterType, NPCommon.ConfigID.PrinterType.HMK054.ToString());
                    break;
                case ConfigID.PrinterType.HMK825:
                    mConfig.SetValue(ConfigID.UsingSettingPrinterType, NPCommon.ConfigID.PrinterType.HMK825.ToString());
                    break;
            }
            mConfig.SetValue(ConfigID.UsingSettingTmoney, (chkTmoney.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingHubulMoney, (chkHubulMoney.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingTTS, (chkTTS.Checked ? "Y" : "N"));

            //할인권 타입설정 추가
            mConfig.SetValue(ConfigID.UsingSettingDiscountCard, (rbNoDCTicket.Checked ? "N" : "Y"));
            mConfig.SetValue(ConfigID.UsingSettingMagneticDCTicket, (rbMagneticDCTicket.Checked ? "Y" : "N"));
            mConfig.SetValue(ConfigID.UsingSettingBarcodeDCTicket, (rbBarcodeDCTicket.Checked ? "Y" : "N"));
            //할인권 타입설정 추가완료


            //바코드모터드리블 사용
            ConfigID.BarcodeReaderType currentBarcodeReaderType = (ConfigID.BarcodeReaderType)Enum.Parse(typeof(ConfigID.BarcodeReaderType), cbxBarcodereaderType.Text);
            mConfig.SetValue(ConfigID.UsingSettingDiscountBarcode, currentBarcodeReaderType.ToString());
            //mConfig.SetValue(ConfigID.UsingSettingDiscountBarcode, (chkDiscountBarcode.Checked ? "Y" : "N"));
            //바코드모터드리블 사용완료


            mConfig.SetValue(ConfigID.UsingSettingSoundRead, (chkSound.Checked ? "Y" : "N"));


            ConfigID.ControlBoardType currentControlBoardType = (ConfigID.ControlBoardType)Enum.Parse(typeof(ConfigID.ControlBoardType), cbxControlBoard.Text);
            mConfig.SetValue(ConfigID.UsingSettingControlBoard, currentControlBoardType.ToString());


            // 신분증인식기 적용
            mConfig.SetValue(ConfigID.UsingSettingSinbunReader, (chkSinbunReader.Checked ? "Y" : "N"));
            // 신분증인식기 적용완료

            mConfig.SetValue(ConfigID.UsingSettingCardRederTypeLeft,cbxCardreaderType1.Text);
            mConfig.SetValue(ConfigID.UsingSettingCardRederTypeRight, cbxCardreaderType2.Text);


            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkCoinBoard_CheckedChanged(object sender, EventArgs e)
        {
            //	chkBill.Checked = chkCoinBoard.Checked;
        }

        private void chkBill_CheckedChanged(object sender, EventArgs e)
        {
            //	chkCoinBoard.Checked = chkBill.Checked;
        }

    }
}
