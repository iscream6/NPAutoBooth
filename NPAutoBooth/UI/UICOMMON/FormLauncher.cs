using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NPAutoBooth.Common;
using FadeFox.Document;
using FadeFox.Utility;
using FadeFox;
using System.IO;
using Microsoft.Win32;
using FadeFox.Database;
using HTS.Client;
using NPCommon;
using FadeFox.Text;
using AxSmtSndRcvVCATLib;
using FadeFox.UI;
using NPCommon.DEVICE;
using NPCommon.Van;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// 사용가능한 장비에 대하여 기기적인 초기작업을 한다. Port Open 등...
    /// </summary>
    public partial class FormLauncher : Form
    {

        SmartroVCat mSmartroVCat = null;
        KIS_TITDIPDevice mKIS_TITDIPDevice = null;
        //스마트로 TIT_DIP EV-CAT 적용
        Smatro_TITDIP_EVCAT evcat = new Smatro_TITDIP_EVCAT();
        //스마트로 TIT_DIP EV-CAT 적용완료

        private string mDeviceNone = string.Empty;
        private string mDeviceOk = string.Empty;
        private string mDeviceError = string.Empty;

        private bool mTotalSuccess = true;
        private int mSeq = 1;

        public FormLauncher()
        {
            InitializeComponent();
            loadPC_Config();
        }

        private void FormLauncher_Load(object sender, EventArgs e)
        {
            FormLauncherLoad();
            tmrLauncher.Enabled = true;

        }

        private void loadPC_Config()
        {


        }
        private void FormLauncherLoad()
        {
            SetLanguage(NPSYS.CurrentLanguageType);
            //지폐리더기 사용여부
            if (NPSYS.Device.UsingSettingBillReader)
            {
                lblBillReader.Text = "";
                lblBillReader.Visible = true;
                lblBillReaderSubject.Visible = true;
            }
            else
            {
                lblBillReader.Visible = false;
                lblBillReaderSubject.Visible = false;
            }
            //코인리더기 사용여부
            if (NPSYS.Device.UsingSettingCoinReader)
            {

                lblCoinReader.Text = "";
                lblCoinReader.Visible = true;
                lblCoinReaderSubject.Visible = true;
            }
            else
            {
                lblCoinReader.Visible = false;
                lblCoinReaderSubject.Visible = false;
            }
            //50원동전방출기 사용여부
            if (NPSYS.Device.UsingSettingCoinCharger50)
            {
                lblCoinCharger50.Text = "";
                lblCoinCharger50.Visible = true;
                lblCoinCharger50Subject.Visible = true;
            }
            else
            {
                lblCoinCharger50.Visible = false;
                lblCoinCharger50Subject.Visible = false;
            }
            //100원동전방출기 사용여부
            if (NPSYS.Device.UsingSettingCoinCharger100)
            {
                lblCoinCharger100.Text = "";
                lblCoinCharger100.Visible = true;
                lblCoinCharger100Subject.Visible = true;
            }
            else
            {
                lblCoinCharger100.Visible = false;
                lblCoinCharger100Subject.Visible = false;

            }
            //500원동전방출기 사용여부
            if (NPSYS.Device.UsingSettingCoinCharger500)
            {
                lblCoinCharger500.Text = "";
                lblCoinCharger500.Visible = true;
                lblCoinCharger500Subject.Visible = true;
            }
            else
            {
                lblCoinCharger500.Visible = false;
                lblCoinCharger500Subject.Visible = false;
            }
            //3.1.74 동전방출기 50원100원500원설정기능 주석완료

            //지폐방출기 사용여부
            if (NPSYS.Device.UsingSettingBill)
            {
                lblBill.Text = "";
                lblBill.Visible = true;
                lblBillSubject.Visible = true;
            }
            else
            {
                lblBillSubject.Visible = false;
                lblBill.Visible = false;
            }
            //영수증프린터 사용여부
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
            {
                lblPrint.Text = "";
                lblPrint.Visible = true;
                lblPrintSubject.Visible = true;
            }
            else
            {
                lblPrint.Visible = false;
                lblPrintSubject.Visible = false;
            }
            //좌측 카드리더기 사용여부
            if (NPSYS.Device.UsingSettingCardReadType != ConfigID.CardReaderType.None)
            {
                lblCreditLeft.Text = "";
                lblCreditLeft.Visible = true;
                lblCreditLeftSubject.Visible = true;
            }
            else
            {
                lblCreditLeft.Visible = false;
                lblCreditLeftSubject.Visible = false;
            }
            //마그네틱할인권리더기 사용여부
            if (NPSYS.Device.UsingSettingMagneticReadType != ConfigID.CardReaderType.None)
            {
                lblCreditRight.Text = "";
                lblCreditRight.Visible = true;
                lblCreditRightSubject.Visible = true;
            }
            else
            {
                lblCreditRight.Visible = false;
                lblCreditRightSubject.Visible = false;
            }
            //TMoney 사용여부
            if (NPSYS.Device.UsingSettingTmoney)
            {
                lblTmoney.Text = "";
                lblTmoney.Visible = true;
                lblTmoneySubject.Visible = true;
            }
            else
            {
                lblTmoney.Visible = false;
                lblTmoneySubject.Visible = false;
            }
            //컨트롤보드 사용여부
            if (NPSYS.Device.UsingSettingControlBoard != ConfigID.ControlBoardType.NONE)
            {
                lblDIdo.Text = "";
                lblDIdo.Visible = true;
                lblDIdoSubject.Visible = true;
            }
            else
            {
                lblDIdo.Visible = false;
                lblDIdoSubject.Visible = false;
            }
            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None)
            {
                lblBarcode.Text = "";
                lblBarcode.Visible = true;
                lblBarcodeSubject.Visible = true;
            }
            else
            {
                lblBarcode.Visible = false;
                lblBarcodeSubject.Visible = false;
            }
            //바코드모터드리블 사용완료

            // 신분증인식기 적용
            if (NPSYS.Device.UsingSettingSinbunReader)
            {
                lblSinbunReader.Text = string.Empty;
                lblSinbunReader.Visible = true;
                lblSinbunReaderSubject.Visible = true;
            }
            else
            {
                lblSinbunReader.Visible = false;
                lblSinbunReaderSubject.Visible = false;
            }
        }

        public void Initialize(int pSeq)
        {
            Result result = null;
            try
            {
                switch (pSeq)
                {
                    case 1:
                        // Log
                        {
                            IDatabase db = NPSYS.Servers[NPCommon.ServerID.NPPaymentLog].Database;
                            bool r = false;

                            try
                            {
                                r = db.Connect();
                                if (r)
                                {
                                    lblLog.Text = mDeviceOk;

                                    string sql = "";


                                    if (!db.IsExistTable("ERRORDEVICE_LOG"))
                                    {
                                        sql = "  CREATE TABLE ERRORDEVICE_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	DEVICECODE VARCHAR(80) NOT NULL,"  // 장비명
                                            + "  	STATUSCODE VARCHAR(80) NOT NULL," // 장애명
                                            + "  	ISSUCCESS VARCHAR(30),"
                                            + "  	RESERVE1 VARCHAR(50),"
                                            + "  	RESERVE2 VARCHAR(50),"
                                            + "  	RESERVE3 VARCHAR(50),"
                                            + "  	RESERVE4 VARCHAR(50),"
                                            + "  	RESERVE5 VARCHAR(50)"
                                            + "  	)";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("RESENDDATA_LOG"))
                                    {
                                        sql = "  CREATE TABLE RESENDDATA_LOG ("
                                            + "  	ID INTEGER PRIMARY KEY AUTOINCREMENT,"
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CURRENTURL VARCHAR(80) NOT NULL,"
                                            + "  	SENDDATA VARCHAR(10000) NOT NULL,"
                                            + "  	FAILCOUNT INTEGER,"
                                             + "  	UPDATE_YN VARCHAR(1)  NULL ,"
                                            + "  	RESERVE1 VARCHAR(50),"
                                            + "  	RESERVE2 VARCHAR(50),"
                                            + "  	RESERVE3 VARCHAR(50),"
                                            + "  	RESERVE4 VARCHAR(50),"
                                            + "  	RESERVE5 VARCHAR(50)"
                                            + "  	)";

                                        db.Execute(sql);
                                    }
                                    if (!db.IsExistTable("PAYMENT_LOG"))
                                    {
                                        sql = "  CREATE TABLE PAYMENT_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                            + "  	IN_DATE VARCHAR(20),"
                                            + "  	OUT_DATE VARCHAR(20),"
                                            + "  	[CURRENT_DATE] VARCHAR(20),"
                                            + "  	PAYMENT_METHOD VARCHAR(50),"
                                            + "  	ELAPSED_TOTAL_MINUTE INTEGER,"
                                            + "  	BASE_MINUTE INTEGER,"
                                            + "  	BASE_FEE INTEGER,"
                                            + "  	UNIT_MINUTE INTEGER,"
                                            + "  	FEE_PER_UNIT_MINUTE INTEGER,"
                                            + "  	PARKING_FEE INTEGER,"
                                            + "  	ELAPSED_MINUTE INTEGER,"
                                            + "  	ELAPSED_HOUR INTEGER,"
                                            + "  	ELAPSED_DAY INTEGER,"
                                            + "  	CURRENT_MONEY INTEGER,"
                                            + "  	CURRENT_5000_QTY INTEGER,"
                                            + "  	CURRENT_1000_QTY INTEGER,"
                                            + "  	CURRENT_500_QTY INTEGER,"
                                            + "  	CURRENT_100_QTY INTEGER,"
                                            + "  	CURRENT_50_QTY INTEGER,"
                                            + "  	CHARGE_MONEY INTEGER,"
                                            + "  	CHARGE_5000_QTY INTEGER,"
                                            + "  	CHARGE_1000_QTY INTEGER,"
                                            + "  	CHARGE_500_QTY INTEGER,"
                                            + "  	CHARGE_100_QTY INTEGER,"
                                            + "  	CHARGE_50_QTY INTEGER,"
                                            + "  	DISCOUNT_MINUTE INTEGER,"
                                            + "  	DISCOUNT_MONEY INTEGER,"
                                            + "  	RECEIPT_NO VARCHAR(50),"
                                            + "  	PAYMENT_MONEY INTEGER,"
                                            + "  	PAYMENT_LOG_ACTION_TYPE VARCHAR(50),"
                                            + "  	PAYMENT_LOG_RESULT_TYPE VARCHAR(50),"
                                            + "  	LATE_CHARGE_YN VARCHAR(1),"
                                            + "  	BEFORE_PAYMENT_MONEY INTEGER,"
                                            + "  	COMMENT VARCHAR(500)"
                                            + "  	)";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("EZ_TICKET_LOG"))
                                    {
                                        sql = "  CREATE TABLE EZ_TICKET_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	INDATE VARCHAR(50) NOT NULL,"
                                            + "  	INTIME VARCHAR(50) NOT NULL,"
                                            + "  	CODE VARCHAR(50) NOT NULL,"
                                            + "  	TDNO VARCHAR(50) NOT NULL,"
                                            + "  	CARNO VARCHAR(50),"
                                            + "  	LPRINFULLDATA VARCHAR(200),"
                                            + "  	MODE VARCHAR(5),"
                                            + "  	ATMYN CHAR(1),"
                                            + "  	JOBDATE VARCHAR(50),"
                                            + "  	OUTDATE VARCHAR(50),"
                                            + "  	OUTTIME VARCHAR(50),"
                                            + "  	DUEMIN INTEGER,"
                                            + "  	DCTOTAL INTEGER,"
                                            + "  	TOTAL INTEGER,"
                                            + "  	MODSTAT INTEGER,"
                                            + "  	COMMENT VARCHAR(500),"
                                            + "  	PATNO VARCHAR(50),"
                                            + "     LASTVISIT VARCHAR(50),"
                                            + "  	PRIMARY KEY (LOG_DATE, INDATE, INTIME, CODE, TDNO)"
                                            + "  	)";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("ADMIN_CASH_LOG"))
                                    {
                                        sql = "  CREATE TABLE ADMIN_CASH_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	LOG_TYPE VARCHAR(50),"
                                            + "  	IN_QTY_50 INTEGER,"
                                            + "  	IN_QTY_100 INTEGER,"
                                            + "  	IN_QTY_500 INTEGER,"
                                            + "  	IN_QTY_1000 INTEGER,"
                                            + "  	IN_QTY_5000 INTEGER,"
                                            + "  	OUT_QTY_50 INTEGER,"
                                            + "  	OUT_QTY_100 INTEGER,"
                                            + "  	OUT_QTY_500 INTEGER,"
                                            + "  	OUT_QTY_1000 INTEGER,"
                                            + "  	OUT_QTY_5000 INTEGER,"
                                            + "  	COMMENT VARCHAR(500),"
                                            + "  	PRIMARY KEY (LOG_DATE, LOG_TYPE)"
                                            + "  	)";

                                        db.Execute(sql);
                                    }

                                    //  수정중
                                    if (!db.IsExistTable("DISCOUNT_LOG"))
                                    {
                                        sql = "  CREATE TABLE DISCOUNT_LOG ("
                                            + "  	NORKEY VARCHAR(20) NOT NULL,"
                                            + "  	DISCOUNTNAME VARCHAR(30) NULL,"
                                            + "  	DISCOUNTCOUNT VARCHAR(30) NULL,"
                                            + "  	DISCOUNTCODE VARCHAR(30) NULL,"
                                            + "  	DISCOUNTMONEY INTEGER ,"
                                            + "  	DISCOUNTTIME  INTEGER NULL,"
                                            + "  	LOGDATE VARCHAR(20)  NOT NULL,"
                                            + "  	TYPE VARCHAR(20)  NULL ,"
                                            + "  	IMSI1 VARCHAR(20)  NULL , "
                                            + "  	IMSI2 VARCHAR(20)  NULL , "
                                            + "  	UPDATE_YN VARCHAR(1)  NULL)";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("MONEY_LOG"))
                                    {
                                        sql = "  CREATE TABLE MONEY_LOG ("
                                            + "  	NORKEY VARCHAR(20) NULL,"
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                            + "  	IN_DATE VARCHAR(20) NOT NULL,"
                                            + "  	PAYMENT_METHOD VARCHAR(50) NOT NULL,"
                                            + "  	IO_TYPE VARCHAR(10) NOT NULL,"
                                            + "  	IN_MONEY INTEGER,"
                                            + "  	OUT_MONEY INTEGER,"
                                            + "  	COMMENT VARCHAR(500),"
                                            + "  	UPDATE_YN VARCHAR(1))";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("CAR_LOG"))
                                    {
                                        sql = "  CREATE TABLE CAR_LOG ("
                                            + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                            + "  	RECEIVEMONEY INTEGER ,"
                                            + "  	CAR_TYPE VARCHAR(20) ,"             // "JUNGI" 0이면 정기권 1이면 "FREETIME" 프리타임 차량  "NORMAL"이면 요금차량
                                            + "  	IO_TYPE VARCHAR(20) NOT NULL,"
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	UPDATE_YN VARCHAR(1))";


                                        db.Execute(sql);
                                    }


                                    if (!db.IsExistTable("RECEIPT_LOG"))
                                    {
                                        sql = "  CREATE TABLE RECEIPT_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                            + "  	IN_DATE VARCHAR(20) NOT NULL,"
                                            + "  	RECEIPT_NO INTEGER)";

                                        db.Execute(sql);
                                    }

                                    if (!db.IsExistTable("CASHTICKET_LOG"))
                                    {
                                        sql = "  CREATE TABLE CASHTICKET_LOG ("
                                            + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                            + "  	IN_DATE VARCHAR(20) NOT NULL,"
                                            + "  	CASHTICKET_NO INTEGER)";

                                        db.Execute(sql);
                                    }


                                    if (!db.IsExistTable("CreditCard_LOG"))
                                    {
                                        sql = "  CREATE TABLE CreditCard_LOG ("
                                            + "  	NORKEY VARCHAR(20) NULL,"
                                            + "  	RESCODE VARCHAR(20) NULL,"
                                            + "  	RESMSG VARCHAR(100) NULL,"
                                            + "  	AUTH_NUMBER VARCHAR(20) NULL,"
                                            + "     AUTH_DATE VARCHAR(20)  NULL,"
                                            + "  	IN_DATE VARCHAR(20)  NULL,"
                                            + "  	OUT_DATE VARCHAR(20)  NULL,"
                                            + "  	PARKTIME VARCHAR(20) NULL,"
                                            + "  	CAR_NUMBER VARCHAR(10)  NULL,"
                                            + "  	CREDIT_PAY  INTEGER ,"
                                            + "  	CREDIT_TAX  INTEGER ,"
                                            + "  	CREDIT_SUPPLY  INTEGER ,"
                                            + "  	LOG_DATE VARCHAR(20)  NULL,"
                                            + "  	UPDATE_YN VARCHAR(1) NULL )";
                                        db.Execute(sql);
                                    }

                                    //Tmap연동
                                    //2020-02-25 이재영 Created.
                                    //24시간동안 결제가 없을 시 서버로 전문을 전송해달라는 요구
                                    if (!db.IsExistTable("LastestPayment_LOG"))
                                    {
                                        sql = "  CREATE TABLE LastestPayment_LOG ("
                                           + "  	CAR_NUMBER VARCHAR(10) NOT NULL,"
                                           + "  	RECEIVEMONEY INTEGER ,"
                                           + "  	IO_TYPE VARCHAR(20) NOT NULL,"
                                           + "  	LOG_DATE VARCHAR(20) NOT NULL,"
                                           + "  	LOG_DESC VARCHAR(20))";
                                        db.Execute(sql);
                                    }

                                }
                                else
                                {
                                    lblLog.Text = mDeviceError;
                                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormLauncher|Initialize", "내부테이블생성에러:");
                                }
                            }
                            catch (Exception ex)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormLauncher|Initialize", "내부테이블생성에러:" + ex.ToString());
                                lblLog.Text = mDeviceError;
                                r = false;
                            }
                            finally
                            {
                                if (db.IsConnect)
                                {
                                    db.Disconnect();
                                }
                            }

                            lblLog.ForeColor = (r ? Color.Blue : Color.Red);
                            if (r == false) mTotalSuccess = false;
                        }
                        break;

                    case 2:
                        if (NPSYS.Device.UsingSettingBillReader)
                        {
                            // 지폐인식기
                            NPSYS.Device.BillReader = new BillReader();
                            NPSYS.Device.BillReader.BaudRateString = NPSYS.SerialPorts[SerialPortID.BillReader].BaudRateString;
                            NPSYS.Device.BillReader.PortNameString = NPSYS.SerialPorts[SerialPortID.BillReader].PortNameString;
                            NPSYS.Device.BillReader.ParityString = NPSYS.SerialPorts[SerialPortID.BillReader].ParityString;
                            DataTable dtBillreaderErrorData = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.BRE);
                            NPSYS.Device.BillReader.CurrentReaderStatusManageMent.SendAllDeviveOk();
                            NPSYS.Device.BillReader.CurrentReaderStatusManageMent.SetDbErrorInfo(dtBillreaderErrorData);
                            bool resultReaderStatus = NPSYS.Device.BillReader.Connect();
                            if (resultReaderStatus == false)
                            {
                                lblBillReader.Text = mDeviceError;
                            }
                            else
                            {
                                NPSYS.Device.BillReader.Reset();
                                BillReader.BillRederStatusType billStatus = NPSYS.Device.BillReader.CurrentStatus();
                                if (billStatus == BillReader.BillRederStatusType.OK)
                                {
                                    billStatus = NPSYS.Device.BillReader.BillDIsableAccept();
                                    if (billStatus == BillReader.BillRederStatusType.OK)
                                    {
                                        lblBillReader.Text = mDeviceOk;
                                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormLauncher|Initialize", "지폐리더기접속:" + NPSYS.Device.isUseDeviceBillReaderDevice.ToString());
                                    }
                                    else
                                    {
                                        lblBillReader.Text = mDeviceError;
                                    }
                                }
                                else
                                {
                                    resultReaderStatus = false;
                                    lblBillReader.Text = mDeviceError;
                                }
                            }

                            lblBillReader.ForeColor = (resultReaderStatus ? Color.Blue : Color.Red);
                            if (resultReaderStatus == false) mTotalSuccess = false;
                        }
                        else
                        {
                            NPSYS.Device.isUseDeviceBillReaderDevice = false;
                        }
                        break;

                    case 3:
                        if (NPSYS.Device.UsingSettingCoinReader)
                        {
                            // 코인인식기
                            NPSYS.Device.CoinReader = new CoinReader();
                            NPSYS.Device.CoinReader.BaudRateString = NPSYS.SerialPorts[SerialPortID.CoinReader].BaudRateString;
                            NPSYS.Device.CoinReader.PortNameString = NPSYS.SerialPorts[SerialPortID.CoinReader].PortNameString;
                            NPSYS.Device.CoinReader.ParityString = NPSYS.SerialPorts[SerialPortID.CoinReader].ParityString;
                            DataTable dtCoinreaderErrorData = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.CRE);
                            NPSYS.Device.CoinReader.CurrentCoinReaderStatusManageMen.SendAllDeviveOk();
                            NPSYS.Device.CoinReader.CurrentCoinReaderStatusManageMen.SetDbErrorInfo(dtCoinreaderErrorData);
                            CoinReader.CoinReaderStatusType resultCoinreaderStatus = NPSYS.Device.CoinReader.Connect();
                            if (resultCoinreaderStatus != CoinReader.CoinReaderStatusType.OK)
                            {

                                lblCoinReader.Text = mDeviceError;
                            }
                            else
                            {
                                CoinReader.CoinReaderStatusType coinReaderResult = NPSYS.Device.CoinReader.DisableCoinRead();
                                if (coinReaderResult == CoinReader.CoinReaderStatusType.OK)
                                {
                                    lblCoinReader.Text = mDeviceOk;
                                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormLauncher|Initialize", "동전리더기접속성공");
                                }
                                else
                                {
                                    lblCoinReader.Text = mDeviceError;
                                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormLauncher|Initialize", "동전리더기 초기화실패:");

                                }
                            }

                            lblCoinReader.ForeColor = (resultCoinreaderStatus == CoinReader.CoinReaderStatusType.OK ? Color.Blue : Color.Red);
                            if (resultCoinreaderStatus != CoinReader.CoinReaderStatusType.OK) mTotalSuccess = false;
                        }
                        else
                        {
                            NPSYS.Device.isUseDeviceCoinReaderDevice = false;
                            NPSYS.Device.CoinReaderDeviceErrorMessage = "사용안함으로 설정";


                        }
                        break;

                    case 4:
                        //3.1.74 동전방출기 50원100원500원설정기능

                        if (NPSYS.Device.UsingSettingCoinCharger50)
                        {
                            NPSYS.Device.CoinDispensor50 = new CoinDispensor();
                            NPSYS.Device.CoinDispensor50.CurrentCoinType = CoinDispensor.CoinType.Money50Type;
                            NPSYS.Device.CoinDispensor50.BaudRateString = NPSYS.SerialPorts[SerialPortID.CoinCharger50].BaudRateString;
                            NPSYS.Device.CoinDispensor50.PortNameString = NPSYS.SerialPorts[SerialPortID.CoinCharger50].PortNameString;
                            NPSYS.Device.CoinDispensor50.ParityString = NPSYS.SerialPorts[SerialPortID.CoinCharger50].ParityString;
                            DataTable dtCoinDispensor1 = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.CC1);
                            NPSYS.Device.CoinDispensor50.CurrentCoinDispensorStatusManagement.SendAllDeviveOk();
                            NPSYS.Device.CoinDispensor50.CurrentCoinDispensorStatusManagement.SetDbErrorInfo(dtCoinDispensor1);
                            CoinDispensor.CoinDispensorStatusType resultCoinDispensor50Connect = NPSYS.Device.CoinDispensor50.Connect();
                            if (resultCoinDispensor50Connect != CoinDispensor.CoinDispensorStatusType.OK)
                            {
                                TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormLauncher|Initialize", "동전방출기 50 PORT OPEN 초기화실패");

                                lblCoinCharger50.Text = mDeviceError;

                            }
                            else
                            {
                                TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "FormLauncher|Initialize", "접속성공");
                            }
                            if (NPSYS.Device.gIsUseCoinDischarger50Device)
                            {
                                CoinDispensor.CoinDispensorStatusType CoinDispensortResult = NPSYS.Device.CoinDispensor50.reset();
                                System.Threading.Thread.Sleep(5000);
                                NPSYS.Device.CoinDispensor50.BaReadinessSignal();
                                if (CoinDispensortResult != CoinDispensor.CoinDispensorStatusType.OK)
                                {
                                    TextCore.DeviceError(TextCore.DEVICE.COIN50CHARGER, "FormLauncher|Initialize", CoinDispensortResult.ToString());
                                    NPSYS.Device.CoinDischarge50DeviceErrorMessage = "동전방출기 " + CoinDispensortResult.ToString();
                                    lblCoinCharger50.Text = mDeviceError;
                                    NPSYS.Device.gIsUseCoinDischarger50Device = false;

                                }
                                else
                                {
                                    lblCoinCharger50.Text = mDeviceOk;
                                    NPSYS.Device.gIsUseCoinDischarger50Device = true;

                                }
                            }
                        }
                        else
                        {
                            NPSYS.Device.gIsUseCoinDischarger50Device = false;
                            NPSYS.Device.CoinDischarge50DeviceErrorMessage = "사용안함으로 설정";

                        }
                        if (NPSYS.Device.UsingSettingCoinCharger100)
                        {
                            NPSYS.Device.CoinDispensor100 = new CoinDispensor();
                            NPSYS.Device.CoinDispensor100.CurrentCoinType = CoinDispensor.CoinType.Money100Type;
                            NPSYS.Device.CoinDispensor100.BaudRateString = NPSYS.SerialPorts[SerialPortID.CoinCharger100].BaudRateString;
                            NPSYS.Device.CoinDispensor100.PortNameString = NPSYS.SerialPorts[SerialPortID.CoinCharger100].PortNameString;
                            NPSYS.Device.CoinDispensor100.ParityString = NPSYS.SerialPorts[SerialPortID.CoinCharger100].ParityString;
                            DataTable dtCoinDispensor2 = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.CC2);
                            NPSYS.Device.CoinDispensor100.CurrentCoinDispensorStatusManagement.SendAllDeviveOk();
                            NPSYS.Device.CoinDispensor100.CurrentCoinDispensorStatusManagement.SetDbErrorInfo(dtCoinDispensor2);
                            CoinDispensor.CoinDispensorStatusType resultCoinDispensor100Connect = NPSYS.Device.CoinDispensor100.Connect();
                            if (resultCoinDispensor100Connect != CoinDispensor.CoinDispensorStatusType.OK)
                            {
                                TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormLauncher|Initialize", "동전방출기 초기화실패");
                                NPSYS.Device.CoinDischarge100DeviceErrorMessage = "동전방출기 초기화실패";

                                NPSYS.Device.gIsUseCoinDischarger100Device = false;
                                lblCoinCharger100.Text = mDeviceError;

                            }
                            else
                            {
                                NPSYS.Device.gIsUseCoinDischarger100Device = true;
                                TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "FormLauncher|Initialize", "접속성공");
                            }
                            if (NPSYS.Device.gIsUseCoinDischarger100Device)
                            {
                                CoinDispensor.CoinDispensorStatusType CoinDispensortResult = NPSYS.Device.CoinDispensor100.reset();
                                System.Threading.Thread.Sleep(5000);
                                NPSYS.Device.CoinDispensor100.BaReadinessSignal();
                                if (CoinDispensortResult != CoinDispensor.CoinDispensorStatusType.OK)
                                {
                                    TextCore.DeviceError(TextCore.DEVICE.COIN100CHARGER, "FormLauncher|Initialize", CoinDispensortResult.ToString());
                                    NPSYS.Device.CoinDischarge100DeviceErrorMessage = "동전방출기 " + CoinDispensortResult.ToString();
                                    lblCoinCharger100.Text = mDeviceError;
                                    NPSYS.Device.gIsUseCoinDischarger100Device = false;

                                }
                                else
                                {
                                    lblCoinCharger100.Text = mDeviceOk;

                                }
                            }
                        }
                        else
                        {
                            NPSYS.Device.gIsUseCoinDischarger100Device = false;
                            NPSYS.Device.CoinDischarge100DeviceErrorMessage = "사용안함으로 설정";

                        }
                        if (NPSYS.Device.UsingSettingCoinCharger500)
                        {
                            NPSYS.Device.CoinDispensor500 = new CoinDispensor();
                            NPSYS.Device.CoinDispensor500.CurrentCoinType = CoinDispensor.CoinType.Money500Type;
                            NPSYS.Device.CoinDispensor500.BaudRateString = NPSYS.SerialPorts[SerialPortID.CoinCharger500].BaudRateString;
                            NPSYS.Device.CoinDispensor500.PortNameString = NPSYS.SerialPorts[SerialPortID.CoinCharger500].PortNameString;
                            NPSYS.Device.CoinDispensor500.ParityString = NPSYS.SerialPorts[SerialPortID.CoinCharger500].ParityString;
                            DataTable dtCoinDispensor3 = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.CC3);
                            NPSYS.Device.CoinDispensor500.CurrentCoinDispensorStatusManagement.SendAllDeviveOk();
                            NPSYS.Device.CoinDispensor500.CurrentCoinDispensorStatusManagement.SetDbErrorInfo(dtCoinDispensor3);

                            CoinDispensor.CoinDispensorStatusType resultCoinDispensor500Connect = NPSYS.Device.CoinDispensor500.Connect();


                            if (resultCoinDispensor500Connect != CoinDispensor.CoinDispensorStatusType.OK)
                            {
                                TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormLauncher|Initialize", "동전방출기 초기화실패");
                                NPSYS.Device.CoinDischarge500DeviceErrorMessage = "동전방출기 초기화실패";
                                NPSYS.Device.gIsUseCoinDischarger500Device = false;
                                lblCoinCharger500.Text = mDeviceError;

                            }
                            else
                            {
                                TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "FormLauncher|Initialize", "접속성공");
                                NPSYS.Device.gIsUseCoinDischarger500Device = true;
                            }
                            if (NPSYS.Device.gIsUseCoinDischarger500Device)
                            {
                                CoinDispensor.CoinDispensorStatusType l_CoinDispensortResul500 = NPSYS.Device.CoinDispensor500.reset();
                                System.Threading.Thread.Sleep(5000);
                                NPSYS.Device.CoinDispensor500.BaReadinessSignal();
                                if (l_CoinDispensortResul500 != CoinDispensor.CoinDispensorStatusType.OK)
                                {
                                    TextCore.DeviceError(TextCore.DEVICE.COIN500CHARGER, "FormLauncher|Initialize", l_CoinDispensortResul500.ToString());
                                    NPSYS.Device.CoinDischarge500DeviceErrorMessage = "동전방출기 " + l_CoinDispensortResul500.ToString();
                                    lblCoinCharger500.Text = mDeviceError;
                                }
                                else
                                {
                                    lblCoinCharger500.Text = mDeviceOk;
                                }
                            }
                        }
                        else
                        {
                            NPSYS.Device.gIsUseCoinDischarger500Device = false;
                            NPSYS.Device.CoinDischarge500DeviceErrorMessage = "사용안함으로 설정";
                        }

                        break;
                    //3.1.74 동전방출기 50원100원500원설정기능 주석완료

                    case 5:
                        if (NPSYS.Device.UsingSettingBill)
                        {
                            // 지폐방출기 초기화
                            try
                            {
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormLauncher|Initialize", "지폐방출기초기화작업준비");
                                DataTable dtBchStatusData = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.BCH);
                                MoneyBillOutDeviice.BillDischargeStatusManageMent.SendAllDeviveOk();
                                MoneyBillOutDeviice.BillDischargeStatusManageMent.SetDbErrorInfo(dtBchStatusData);
                                result = MoneyBillOutDeviice.Init(NPSYS.SerialPorts[SerialPortID.BillCharger].PortNameString, Application.StartupPath + "\\DEVICELOG\\");
                            }
                            catch (Exception ex)
                            {
                                NPSYS.Device.BillDischargeDeviceErrorMessage = "초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormLauncher|Initialize", "초기화실패:" + ex.ToString());

                                result.Success = false;
                            }
                            if (result.Success == false)
                            {
                                NPSYS.Device.gIsUseDeviceBillDischargeDevice = false;
                                NPSYS.Device.BillDischargeDeviceErrorMessage = "초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.BILLCHARGER, "FormLauncher|Initialize", "초기화실패");
                                lblBill.Text = mDeviceError;

                            }
                            else
                            {
                                lblBill.Text = mDeviceOk;
                                NPSYS.Device.gIsUseDeviceBillDischargeDevice = true;
                                MoneyBillOutDeviice.BillPurgeJob();
                                TextCore.ACTION(TextCore.ACTIONS.BILLCHARGER, "FormLauncher|Initialize", "접속성공");


                            }

                            lblBill.ForeColor = (result.Success ? Color.Blue : Color.Red);
                            if (result.Success == false) mTotalSuccess = false;
                        }
                        else
                        {
                            NPSYS.Device.gIsUseDeviceBillDischargeDevice = false;
                            NPSYS.Device.BillDischargeDeviceErrorMessage = "사용안함으로 설정";

                        }
                        break;

                    case 6:
                        if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                        {
                            NPSYS.Device.HMC60 = new HMC60();
                            DataTable dtHmc = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.REP);
                            NPSYS.Device.HMC60.CurrentPrinterStatus.SendAllDeviveOk();
                            NPSYS.Device.HMC60.CurrentPrinterStatus.SetDbErrorInfo(dtHmc);
                            NPSYS.Device.HMC60.BaudRateString = NPSYS.SerialPorts[SerialPortID.ReceiptPrint].BaudRateString;
                            NPSYS.Device.HMC60.PortNameString = NPSYS.SerialPorts[SerialPortID.ReceiptPrint].PortNameString;
                            result = NPSYS.Device.HMC60.Connect(); ;
                            if (result.Success == false)
                            {
                                NPSYS.Device.ReceiptPrintDeviceErrorMessage = "초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.RECIPT, "FormLauncher|Initialize", "초기화실패");
                                NPSYS.Device.HMC60.CurrentPrinterStatus.SetIsComuniCationOk(false);

                            }
                            else
                            {
                                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "FormLauncher|Initialize", "접속성공");
                                HMC60.HmcStatus currentPrintStatus = NPSYS.Device.HMC60.HmcGetStatus();
                                if (currentPrintStatus != HMC60.HmcStatus.Communcation)
                                {
                                    NPSYS.Device.HMC60.CurrentPrinterStatus.SetIsComuniCationOk(true);
                                    result.Success = true;


                                }
                                else
                                {
                                    result.Success = false;
                                    NPSYS.Device.HMC60.CurrentPrinterStatus.SetIsComuniCationOk(false);


                                }
                                System.Threading.Thread.Sleep(1000);
                                NPSYS.Device.HMC60.AutoStatus();

                            }
                            if (result.Success)
                            {
                                lblPrint.Text = mDeviceOk;
                            }
                            else
                            {
                                lblPrint.Text = mDeviceError;
                            }
                            lblPrint.ForeColor = (result.Success ? Color.Blue : Color.Red);
                            if (result.Success == false) mTotalSuccess = false;
                        }
                        else
                        {
                            NPSYS.Device.gIsUseReceiptPrintDevice = false;
                            NPSYS.Device.ReceiptPrintDeviceErrorMessage = "사용안함으로 설정";

                        }
                        break;

                    case 7:
                        if (NPSYS.Device.UsingSettingTmoney)
                        {

                            NPSYS.Device.T_MoneySmartro = new TmoneySmartro();
                            string BaudRateString = NPSYS.SerialPorts[SerialPortID.TmoneyCardReader].BaudRateString;
                            string PortNameString = NPSYS.SerialPorts[SerialPortID.TmoneyCardReader].PortNameString;
                            int tmoneyPort = Convert.ToInt32(PortNameString.Replace("COM", ""));
                            int tmoneybaudrate = Convert.ToInt32(BaudRateString);
                            NPSYS.Device.T_MoneySmartro.pvs_catid = NPSYS.TmoneyCatId;
                            NPSYS.Device.T_MoneySmartro.pvs_vanip = NPSYS.TmoneyVanIp;
                            NPSYS.Device.T_MoneySmartro.pvi_vanport = Convert.ToInt32(NPSYS.TmoneyVanPort);
                            try
                            {
                                NPSYS.Device.T_MoneySmartro.pvi_vanport = Convert.ToInt32(NPSYS.TmoneyVanPort);
                            }
                            catch
                            {
                            }
                            NPSYS.Device.T_MoneySmartro.mPortNumber = tmoneyPort;
                            NPSYS.Device.T_MoneySmartro.mBaudrate = tmoneybaudrate;
                            bool lConnect = NPSYS.Device.T_MoneySmartro.TmoneyConnet();
                            if (lConnect)
                            {
                                string errorMessage = string.Empty;
                                bool isGashi = NPSYS.Device.T_MoneySmartro.TmoneyGashi(ref errorMessage);
                                if (isGashi == true)
                                {
                                    bool isLogON = NPSYS.Device.T_MoneySmartro.TmoneyLogOn();
                                    if (isLogON)
                                    {
                                        lblTmoney.Text = mDeviceOk;
                                        TextCore.ACTION(TextCore.ACTIONS.TMONEY, "FormLauncher|Initialize", "접속성공");
                                        NPSYS.Device.isUseDeviceTMoneyReaderDevice = true;
                                        NPSYS.Device.TMONEYDeviceErrorMessage = mDeviceOk;
                                        NPSYS.Device.UsingSettingTmoney = true;
                                    }
                                    else
                                    {
                                        lblTmoney.Text = "로그온실패";
                                        TextCore.ACTION(TextCore.ACTIONS.TMONEY, "FormLauncher|Initialize", "로그온실패");
                                        NPSYS.Device.isUseDeviceTMoneyReaderDevice = false;
                                        NPSYS.Device.UsingSettingTmoney = false;
                                        NPSYS.Device.TMONEYDeviceErrorMessage = "로그온실패";
                                    }
                                }
                                else
                                {
                                    lblTmoney.Text = "개시실패";
                                    TextCore.ACTION(TextCore.ACTIONS.TMONEY, "FormLauncher|Initialize", "개시실패");
                                    NPSYS.Device.isUseDeviceTMoneyReaderDevice = false;
                                    NPSYS.Device.UsingSettingTmoney = false;
                                    NPSYS.Device.TMONEYDeviceErrorMessage = "개시실패";
                                }

                            }
                            else
                            {
                                lblTmoney.Text = mDeviceError;
                                TextCore.ACTION(TextCore.ACTIONS.TMONEY, "FormLauncher|Initialize", "접속실패");
                                NPSYS.Device.isUseDeviceTMoneyReaderDevice = false;
                                NPSYS.Device.UsingSettingTmoney = false;
                                NPSYS.Device.TMONEYDeviceErrorMessage = "접속실패";

                            }
                            //  NPSYS.Device.T_MoneySmartro.TmoneyClose();
                            //}
                        }
                        break;


                    case 8:
                        if (NPSYS.Device.UsingSettingCardReadType != ConfigID.CardReaderType.None)
                        {
                            NPSYS.Device.CreditReaderStatusManageMent = new CreditReaderStatusManageMent();
                            DataTable dtCreditReader = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.CDR);
                            NPSYS.Device.CreditReaderStatusManageMent.SendAllDeviveOk();
                            NPSYS.Device.CreditReaderStatusManageMent.SetDbErrorInfo(dtCreditReader);
                        }

                        if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat)
                        {
                            System.Threading.Thread.Sleep(8000);
                            if (NPSYS.Device.SmtSndRcv == null)
                            {
                                NPSYS.Device.SmtSndRcv = new AxSmtSndRcvVCAT();
                                NPSYS.Device.SmtSndRcv.CreateControl();
                            }
                            if (mSmartroVCat == null)
                            {
                                mSmartroVCat = new SmartroVCat();
                            }
                            mSmartroVCat.VCatIp = NPSYS.gVanIp;
                            mSmartroVCat.VCatPort = Convert.ToInt32(NPSYS.gVanPort);
                            SmartroVCat.SmatroData smartroData = mSmartroVCat.DeviceInfo(NPSYS.Device.SmtSndRcv);
                            if (smartroData.Success)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "[VCAT 장비정보응답]"
                                                        + " 성공유무:" + smartroData.Success.ToString()
                                                        + " 응답코드:" + smartroData.ReceiveReturnCode
                                                        + " 사업자번호:" + smartroData.DeviceBusinessNum
                                                        + " 사업자명:" + smartroData.DeviceOwnerName
                                                        + " 사업자주소:" + smartroData.DeviceStoreAddr
                                                        + " 사업자명:" + smartroData.DeviceStoreName
                                                        + " 사업자전화번호:" + smartroData.DeviceStoreTel
                                                        + " 응답메세지:" + smartroData.ReceiveReturnMessage
                                                        + " 화면메세지:" + smartroData.ReceiveDisplayMsg
                                                        + " 에러메세지:" + smartroData.ErrorMessage);
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "VCat 초기화 실패";

                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "VCat 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }

                        // 2016.10.27 KIS_DIP 추가
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                        {
                            System.Threading.Thread.Sleep(2000);
                            if (NPSYS.Device.KisPosAgent == null)
                            {
                                NPSYS.Device.KisPosAgent = new AxKisPosAgentLib.AxKisPosAgent();
                                NPSYS.Device.KisPosAgent.CreateControl();
                            }
                            if (mKIS_TITDIPDevice == null)
                            {
                                mKIS_TITDIPDevice = new KIS_TITDIPDevice();
                            }
                            mKIS_TITDIPDevice.VanIP = NPSYS.gVanIp;
                            mKIS_TITDIPDevice.VanPort = Convert.ToInt16(NPSYS.gVanPort);
                            bool isSuccess = mKIS_TITDIPDevice.IsConnect(NPSYS.Device.KisPosAgent);
                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "KIS_TIT_DIP_IFM]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                                NPSYS.Device.UsingSettingCreditCard = true;
                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                NPSYS.Device.UsingSettingCreditCard = false;
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "KIS_TIT_DIP_IFM 초기화 실패";

                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "KIS_TIT_DIP_IFM 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }
                        // 2016.10.27  KIS_DIP 추가종료
                        //KOCSE 카드리더기 추가
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_TCM)
                        {
                            System.Threading.Thread.Sleep(2000);


                            KocesTcmMotor.mTerminalId = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
                            KocesTcmMotor.mSaup = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardSaupNo).ToUpper().Trim();
                            KocesTcmMotor.mSwVersion = NPSYS.gVanSoftVersion;
                            KocesTcmMotor.mSerial = NPSYS.gVanSerialVersion;
                            string errormessage = string.Empty;
                            NPSYS.Device.UsingSettingCreditCard = false;



                            bool isSuccess = KocesTcmMotor.ProgramDownLoadRequest(ref errormessage);
                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "KOCES_TCM]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                                KocesTcmMotor.CardEject();
                                NPSYS.Device.UsingSettingCreditCard = true;
                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "KOCES_TCM 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "KOCES_TCM 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }
                        //KOCSE 카드리더기 추가 주석
                        //KICC DIP적용

                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_DIP_IFM)
                        {
                            System.Threading.Thread.Sleep(500);
                            string errormessage = string.Empty;
                            NPSYS.Device.UsingSettingCreditCard = false;
                            NPSYS.Device.KICC_TIT = new KICC_TIT();

                            bool isSuccess = NPSYS.Device.KICC_TIT.CardEject();
                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "KICC_DIP_IFM]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                                //카드실패전송
                                NPSYS.Device.UsingSettingCreditCard = true;
                                //카드실패전송 완료

                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                NPSYS.Device.UsingSettingCreditCard = false;
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "KICC_DIP_IFM 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "KICC_DIP_IFM 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }
                        //KICC DIP적용완료
                        //KICCTS141적용

                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_TS141)
                        {
                            System.Threading.Thread.Sleep(2000);
                            string gCreditPort1 = NPSYS.SerialPorts[SerialPortID.CreditCardReader_1].PortNameString.Replace("COM", "");
                            string gCreditBaudrate1 = NPSYS.SerialPorts[SerialPortID.CreditCardReader_1].BaudRateString;
                            KiccTs141.Port = Convert.ToInt32(gCreditPort1);
                            KiccTs141.BaudRate = Convert.ToInt32(gCreditBaudrate1);
                            NPSYS.Device.UsingSettingCreditCard = true;


                            bool isSuccess = KiccTs141.Connect(); ;
                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "KICC_TS141]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                            }
                            else
                            {
                                NPSYS.Device.UsingSettingCreditCard = false;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "KICC_TS141 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "KICC_TS141 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }
                        //KICCTS141적용완료
                        //KOCES PAYMGATE 적용
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_PAYMGATE)
                        {
                            System.Threading.Thread.Sleep(2000);


                            KocesTcmMotor.mTerminalId = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
                            KocesTcmMotor.mSaup = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardSaupNo).ToUpper().Trim();
                            KocesTcmMotor.mSwVersion = NPSYS.gVanSoftVersion;
                            KocesTcmMotor.mSerial = NPSYS.gVanSerialVersion;
                            string errormessage = string.Empty;
                            NPSYS.Device.UsingSettingCreditCard = false;


                            bool isSuccess = KocesTcmMotor.ProgramDownLoadRequest(ref errormessage);
                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "KOCES_PAYMGATE]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                                NPSYS.Device.UsingSettingCreditCard = true;
                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "KOCES_PAYMGATE 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "KOCES_PAYMGATE 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }

                        }
                        //KOCES PAYMGATE 적용완료
                        // FIRSTDATA처리 
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP)
                        {
                            bool isSuccess = true; // 여기서 FirstDataDip.Connect 사용시 무한루프를 돌음 실제 메인폼에서만 함수를 사용해야한다..이유는모름

                            if (isSuccess)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                                         "FIRSTDATA_DIP]"
                                                        + " 성공유무:" + isSuccess.ToString());
                                lblCreditLeft.Text = mDeviceOk;
                                lblCreditLeft.ForeColor = Color.Blue;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                            }
                            else
                            {
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "FIRSTDATA_DIP 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "FIRSTDATA_DIP 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");

                            }
                        }
                        // FIRSTDATA처리  주석처리완료

                        //KSNET적용
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KSNET)
                        {
                            System.Threading.Thread.Sleep(2000);
                            NPSYS.Device.gIsUseCreditCardDevice = true;
                            lblCreditLeft.Text = mDeviceOk;
                            lblCreditLeft.ForeColor = Color.Blue;
                            NPSYS.Device.gIsUseCreditCardDevice = true;
                        }
                        //KSNET 적용완료
                        //스마트로 TIT_DIP EV-CAT 적용
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                        {
                            System.Threading.Thread.Sleep(500);

                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize", "접속 정보 [VAN IP] " + NPSYS.gVanIp);
                            NPSYS.Device.Smartro_TITDIP_Evcat.set_OcxHostIP(ref NPSYS.gVanIp);
                            NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults += evcat.SmartroEvcat_QueryResults;
                            evcat.eventRunStatus += resultInfo;
                            evcat.SendInfo.InitSendData();
                            evcat.EVCATStat(NPSYS.Device.Smartro_TITDIP_Evcat);
                            System.Threading.Thread.Sleep(1000);
                        }
                        //스마트로 TIT_DIP EV-CAT 적용완료
                        else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TL3500S)
                        {
                            System.Threading.Thread.Sleep(1000);

                            SmartroDTO divCheckDto = null;
                            NPSYS.Device.TmoneySmartro3500 = new Smartro_TL3500S();
                            //1. Serial Port 연결 정보 셋팅
                            NPSYS.Device.TmoneySmartro3500.PortName = NPSYS.SerialPorts[SerialPortID.CreditCardReader_1].PortNameString;
                            NPSYS.Device.TmoneySmartro3500.BaudRateString = NPSYS.SerialPorts[SerialPortID.CreditCardReader_1].BaudRateString;
                            NPSYS.Device.TmoneySmartro3500.ParityString = NPSYS.SerialPorts[SerialPortID.CreditCardReader_1].ParityString;

                            //2. 장치 체크 전문 수신 무기명 이벤트 설정
                            NPSYS.Device.TmoneySmartro3500.EventTMoneyData += (SmartroDTO dto) =>
                            {
                                divCheckDto = dto;
                            };

                            //3. 단말기 정보 셋팅
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

                            bool isSuccess = NPSYS.Device.TmoneySmartro3500.Connect();

                            if (isSuccess)
                            {
                                //4. 설정 정보 셋팅 전문 송신
                                NPSYS.Device.TmoneySmartro3500.RequestInitSetting();

                                //5. 장치 체크 전문 송신
                                NPSYS.Device.TmoneySmartro3500.RequestDeviceCheck();

                                System.Threading.Thread.Sleep(500); //잠시 대기

                                string status = "";
                                Header header = divCheckDto?.HeaderData as Header;
                                if (header == null || header.JobCode != "a")
                                {
                                    status = "장치체크송수신실패";
                                    isSuccess = false;
                                }
                                else
                                {
                                    ReceiveDeviceCheck deviceCheck = divCheckDto?.BodyData as ReceiveDeviceCheck;
                                    if (deviceCheck == null)
                                    {
                                        status = "장치체크 송수신 실패";
                                        isSuccess = false;
                                    }
                                    else
                                    {
                                        switch (deviceCheck.CardConnectStat)
                                        {
                                            case "N":
                                                status = "카드 모듈 통신 상태 미설치";
                                                isSuccess = false;
                                                break;

                                            case "X":
                                                status = "카드 모듈 통신 상태 오류";
                                                isSuccess = false;
                                                break;

                                            case "O":
                                                break;
                                        }

                                        if (deviceCheck.RFConnectStat == "X")
                                        {
                                            status = "Rf 모듈 통신 상태 오류";
                                            isSuccess = false;
                                        }

                                        switch (deviceCheck.VANConnectStat)
                                        {
                                            case "N":
                                                status = "VAN 서버 연결 상태 미설치";
                                                isSuccess = false;
                                                break;

                                            case "X":
                                                status = "VAN 서버 연결 디바이스 오류";
                                                isSuccess = false;
                                                break;

                                            case "F":
                                                status = "VAN 서버 연결 실패";
                                                isSuccess = false;
                                                break;

                                            case "O":
                                                break;
                                        }
                                    }
                                }

                                status = isSuccess ? "초기화 성공" : status;
                                CreditReaderStatusManageMent.CREDITReaderStatusType statusType =
                                    isSuccess ?
                                    CreditReaderStatusManageMent.CREDITReaderStatusType.OK :
                                    CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus;

                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize", $"SMTRO_TL3500S] {status}");

                                NPSYS.Device.UsingSettingCreditCard = isSuccess;
                                lblCreditLeft.Text = isSuccess ? mDeviceOk : status;
                                lblCreditLeft.ForeColor = isSuccess ? Color.Blue : Color.Red;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(statusType, true);
                            }
                            else
                            {
                                NPSYS.Device.UsingSettingCreditCard = false;
                                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                                lblCreditLeft.ForeColor = Color.Red;
                                lblCreditLeft.Text = "SMTRO_TL3500S 초기화 실패";
                                NPSYS.Device.CreditCardDeviceErrorMessage1 = "SMTRO_TL3500S 초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");
                            }
                        }
                        //TMoney 스마트로 적용완료
                        else
                        {
                            NPSYS.Device.CreditCardDeviceErrorMessage1 = "사용안함으로 설정";
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize", "카드리더기1사용안함");

                        }

                        if (NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.TItMagnetincDiscount)
                        {
                            NPSYS.gCreditPort2 = NPSYS.SerialPorts[SerialPortID.CreditCardReader_2].PortNameString.Replace("COM", "");
                            NPSYS.gCreditBaudrate2 = NPSYS.SerialPorts[SerialPortID.CreditCardReader_2].BaudRateString;


                            NPSYS.Device.CardDevice2 = new TicketCardDevice();
                            DataTable dtMagneticCardReader2 = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.MAR);
                            NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SendAllDeviveOk();
                            NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDbErrorInfo(dtMagneticCardReader2);

                            try
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher|Initialize", "할인권/신용카드겸용 카드리더기2초기화준비");
                                result = NPSYS.Device.CardDevice2.TIcektCreditCardopenDevice(NPSYS.gCreditPort2, NPSYS.gCreditBaudrate2);

                            }
                            catch (Exception ex)
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormLauncher|Initialize", ex.ToString());
                                result.Success = false;
                            }
                            if (result.Success == false)
                            {
                                NPSYS.Device.gIsUseMagneticReaderDevice = false;
                                NPSYS.Device.CreditCardDeviceErrorMessage2 = "초기화 실패";
                                TextCore.DeviceError(TextCore.DEVICE.CARDREADER2, "FormLauncher|Initialize", "초기화실패");


                            }
                            else
                            {
                                Result _TIcketStatus2 = NPSYS.Device.CardDevice2.GetStatus();
                                if (_TIcketStatus2.Success == true)
                                {
                                    NPSYS.Device.CardDevice2.CurrentTicketCardDeviceStatusManageMent.SetDeviceStatus(TicketCardDevice.TicketAndCardResult.OK, true);
                                    NPSYS.Device.gIsUseMagneticReaderDevice = true;
                                    NPSYS.Device.CardDevice2.TIcketFrontEject();
                                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormLauncher|Initialize", "할인권리더기 접속성공:" + _TIcketStatus2.Message);

                                }
                                else
                                {
                                    NPSYS.Device.gIsUseMagneticReaderDevice = false;
                                    TextCore.ACTION(TextCore.ACTIONS.CARDREADER2, "FormLauncher|Initialize", "할인권리더기 접속실패:" + _TIcketStatus2.Message);

                                }
                            }
                            lblCreditRight.Text = result.Message;
                            lblCreditRight.ForeColor = (result.Success ? Color.Blue : Color.Red);
                        }
                        else
                        {
                            NPSYS.Device.gIsUseMagneticReaderDevice = false;
                            NPSYS.Device.CreditCardDeviceErrorMessage2 = "사용안함으로 설정";
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize", "카드리더기2사용안함");

                        }


                        break;





                    case 9:
                        if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
                        {
                            // DIDO
                            NPSYS.Device.DoSensors = new GoodTechContorlBoard();
                            NPSYS.Device.DoSensors.PortName = NPSYS.SerialPorts[SerialPortID.Dido].PortNameString;
                            NPSYS.Device.DoSensors.BaudRateString = NPSYS.SerialPorts[SerialPortID.Dido].BaudRateString;
                            NPSYS.Device.DoSensors.ParityString = NPSYS.SerialPorts[SerialPortID.Dido].ParityString;
                            bool resultDoSensor = true;
                            resultDoSensor = NPSYS.Device.DoSensors.Connect();
                            if (resultDoSensor == false)
                            {
                                lblDIdo.Text = mDeviceError;
                                TextCore.DeviceError(TextCore.DEVICE.DIDO, "FormLauncher|Initialize", "초기화실패");
                                NPSYS.Device.DIDODeviceErrorMessage = "초기화실패";
                                NPSYS.Device.gIsUseDidoDevice = false;
                            }
                            else
                            {
                                lblDIdo.Text = mDeviceOk;
                                NPSYS.Device.gIsUseDidoDevice = true;

                                TextCore.ACTION(TextCore.ACTIONS.DIDO, "FormLauncher|Initialize", "DIDO 접속성공");
                            }

                            lblDIdo.ForeColor = (resultDoSensor == true ? Color.Blue : Color.Red);
                            if (resultDoSensor == false) mTotalSuccess = false;
                        }
                        else if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.NEXPA)
                        {

                            // DIDO
                            NPSYS.Device.NexpaDoSensor = new NexpaControlBoard();
                            DataTable dtDido = LPRDbSelect.GetDeivceErrorInfoFromDeviceCode(CommProtocol.device.DID);
                            NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.SetDbErrorInfo(dtDido);
                            NPSYS.Device.NexpaDoSensor.PortName = NPSYS.SerialPorts[SerialPortID.Dido].PortNameString;
                            NPSYS.Device.NexpaDoSensor.BaudRateString = NPSYS.SerialPorts[SerialPortID.Dido].BaudRateString;
                            NPSYS.Device.NexpaDoSensor.ParityString = NPSYS.SerialPorts[SerialPortID.Dido].ParityString;
                            bool resultDoSensor = true;
                            resultDoSensor = NPSYS.Device.NexpaDoSensor.Connect();
                            NPSYS.Device.NexpaDoSensor.eventreceiveBoardData += new NexpaControlBoard.receiveBoardData(actionReceiveBoardData);
                            NexpaControlBoard.BoarData boardData = null;
                            if (NPSYS.ControlBoardRecovery == ConfigID.ErrorRecoveryType.AUTO)
                            {
                                NPSYS.Device.NexpaDoSensor.isAutoRecovery = true;

                            }
                            if (NPSYS.ControlBoardRecovery == ConfigID.ErrorRecoveryType.MANUAL) // auto가아니면
                            {
                                System.Threading.Thread.Sleep(100);
                                NPSYS.Device.NexpaDoSensor.isAutoRecovery = true;
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher ", "NPSYS.Device.DoSensors.isAutoRecovery 왜안바껴" + NPSYS.Device.NexpaDoSensor.isAutoRecovery.ToString());
                                System.Threading.Thread.Sleep(100);
                            }
                            boardData = NPSYS.Device.NexpaDoSensor.SendCallBoarStatusTimeOut();

                            if (NPSYS.ControlBoardRecovery == ConfigID.ErrorRecoveryType.MANUAL) // auto가아니면
                            {

                                NPSYS.Device.NexpaDoSensor.isAutoRecovery = false;
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher ", "NPSYS.Device.DoSensors.isAutoRecovery" + NPSYS.Device.NexpaDoSensor.isAutoRecovery.ToString());
                            }
                            if (resultDoSensor == false || boardData == null || boardData.IsSuccess == false)
                            {
                                lblDIdo.Text = "fail";
                                TextCore.DeviceError(TextCore.DEVICE.DIDO, "FormLauncher|Initialize", "초기화실패");
                                NPSYS.Device.DIDODeviceErrorMessage = "Fail Open";
                                NPSYS.Device.gIsUseDidoDevice = false;
                                if (NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.isSensorCommunicationError == false) // 사전무인통신정상
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Fail, (int)NexpaControlBoard.BoardStatus.ErrorCode.SensorCommunication);
                                    NPSYS.Device.NexpaDoSensor.CurrentBoardStatus.isSensorCommunicationError = true;
                                }

                            }
                            else
                            {
                                lblDIdo.Text = "DOOR1:" + boardData.Door1.ToString() + " DOOR2:" + boardData.Door2.ToString() + " Fan:" + boardData.Fan.ToString();
                                NPSYS.Device.gIsUseDidoDevice = true;
                                TextCore.ACTION(TextCore.ACTIONS.DIDO, "FormLauncher|Initialize", "DIDO SUCCESS");
                            }
                        }
                        else
                        {
                        }

                        break;

                    case 10:
                        //바코드모터드리블 사용
                        if (NPSYS.Device.UsingSettingDiscountBarcodeSerial != ConfigID.BarcodeReaderType.None)
                        {
                            // DIDO
                            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode)
                            {
                                NPSYS.Device.BarcodeSerials = new BarcodeSerial();
                                NPSYS.Device.BarcodeSerials.PortName = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].PortNameString;
                                NPSYS.Device.BarcodeSerials.BaudRateString = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].BaudRateString;
                                NPSYS.Device.BarcodeSerials.ParityString = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].ParityString;
                                bool resultDoBarcode = true;
                                resultDoBarcode = NPSYS.Device.BarcodeSerials.Connect();
                                if (resultDoBarcode == false)
                                {
                                    lblBarcode.Text = mDeviceError;
                                    TextCore.DeviceError(TextCore.DEVICE.BACODE, "FormLauncher|Initialize", "초기화실패");
                                    NPSYS.Device.BARCODEDeviceErrorMessage = "초기화실패";
                                    NPSYS.Device.gIsUseBarcodeSerial = false;
                                }
                                else
                                {
                                    lblBarcode.Text = mDeviceOk;
                                    NPSYS.Device.gIsUseBarcodeSerial = true;
                                    TextCore.ACTION(TextCore.ACTIONS.BARCODE, "FormLauncher|Initialize", "바코드시리얼 접속성공");
                                }

                                lblBarcode.ForeColor = (resultDoBarcode == true ? Color.Blue : Color.Red);
                                if (resultDoBarcode == false) mTotalSuccess = false;
                            }
                            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000)
                            {
                                NPSYS.Device.BarcodeMoter = new BarcodeMoter();
                                NPSYS.Device.BarcodeMoter.PortName = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].PortNameString;
                                NPSYS.Device.BarcodeMoter.BaudRateString = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].BaudRateString;
                                NPSYS.Device.BarcodeMoter.ParityString = NPSYS.SerialPorts[SerialPortID.BarcodeSerialReader].ParityString;
                                bool resultDoBarcode = true;
                                resultDoBarcode = NPSYS.Device.BarcodeMoter.Connect();
                                if (resultDoBarcode)
                                {
                                    BarcodeMoter.BarcodeMotorResult resultBarcodeStatus = NPSYS.Device.BarcodeMoter.GetStatus();
                                    if (resultBarcodeStatus.ResultStatus == BarcodeMotorErrorCode.Error || resultBarcodeStatus.ResultStatus == BarcodeMotorErrorCode.timeOut)
                                    {
                                        resultDoBarcode = false;
                                    }
                                    else
                                    {
                                        resultDoBarcode = true;
                                    }
                                }

                                if (resultDoBarcode == false)
                                {
                                    lblBarcode.Text = mDeviceError;
                                    TextCore.DeviceError(TextCore.DEVICE.BACODE, "FormLauncher|Initialize", "초기화실패");
                                    NPSYS.Device.BARCODEDeviceErrorMessage = "초기화실패";
                                    NPSYS.Device.gIsUseBarcodeSerial = false;
                                }
                                else
                                {
                                    lblBarcode.Text = mDeviceOk;
                                    NPSYS.Device.gIsUseBarcodeSerial = true;
                                    TextCore.ACTION(TextCore.ACTIONS.BARCODE, "FormLauncher|Initialize", "바코드시리얼 접속성공");
                                }

                                lblBarcode.ForeColor = (resultDoBarcode == true ? Color.Blue : Color.Red);
                                if (resultDoBarcode == false) mTotalSuccess = false;
                            }

                        }

                        break;

                    case 11:


                        break;

                    case 12:


                        break;


                    case 13:

                        break;
                    // 신분증인식기 적용
                    case 14:
                        if (NPSYS.Device.UsingSettingSinbunReader)
                        {
                            NPSYS.Device.SinbunReader = new SinBunReader();
                            bool resultSinbunReaderStatus = false;
                            resultSinbunReaderStatus = NPSYS.Device.SinbunReader.Connect();
                            if (resultSinbunReaderStatus)
                            {
                                lblSinbunReader.Text = mDeviceOk;
                                NPSYS.Device.gIsUseSinbunReader = true;
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher|Initialize", "신분증인식기 연결성공");
                            }
                            else
                            {
                                lblSinbunReader.Text = mDeviceError;
                                NPSYS.Device.gIsUseSinbunReader = false;
                                NPSYS.Device.SinbunReaderErrorMessage = "초기화 실패";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher|Initialize", "신분증인식기 연결실패");
                            }
                        }
                        else
                        {
                            lblSinbunReaderSubject.Visible = false;
                            lblSinbunReader.Visible = false;
                        }
                        break;
                    // 신분증인식기 적용완료
                    //전동어닝 제어 적용
                    case 15:

                        break;
                    //전동어닝 제어 적용완료
                    case 16:
                        if (mTotalSuccess == true)
                        {

                        }
                        else
                        {
                            //      lblErrMessage.Visible = true;
                        }
                        //   btnOk.Enabled = true;
                        //   btnProgramExit.Enabled = true;
                        break;
                }
                mSeq++;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormLauncher | Initialize", ex.ToString());
                mSeq++;
            }

            //   
        }

        private void tmrLauncher_Tick(object sender, EventArgs e)
        {
            if (mSeq > 16)
            {
                tmrLauncher.Enabled = false;
                this.Close();
            }
            Initialize(mSeq);
        }
        //스마트로 TIT_DIP EV-CAT 적용

        private void resultInfo(bool result)
        {

            if (result)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormLauncher | Initialize",
                                         "스마트로 TIT_DIP EV-CAT]"
                                        + " 성공유무:" + result.ToString());
                lblCreditLeft.Text = mDeviceOk;
                lblCreditLeft.ForeColor = Color.Blue;
                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.OK, true);
                NPSYS.Device.UsingSettingCreditCard = true;
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults -= evcat.SmartroEvcat_QueryResults;
            }
            else
            {
                NPSYS.Device.UsingSettingCreditCard = false;
                lblCreditLeft.ForeColor = Color.Red;
                lblCreditLeft.Text = "스마트로 EV-CAT 초기화 실패";
                NPSYS.Device.CreditReaderStatusManageMent.SetCreditReaderDeviceStatus(CreditReaderStatusManageMent.CREDITReaderStatusType.DeviceStatus, false);
                NPSYS.Device.CreditCardDeviceErrorMessage1 = "스마트로 EV-CAT 초기화 실패";
                TextCore.DeviceError(TextCore.DEVICE.CARDREADER1, "FormLauncher|Initialize", "초기화실패");
                NPSYS.Device.Smartro_TITDIP_Evcat.QueryResults -= evcat.SmartroEvcat_QueryResults;
            }
        }

        //스마트로 TIT_DIP EV-CAT 적용완료

        void actionReceiveBoardData(NexpaControlBoard.BoarData pBoardData)
        {

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

            SetLanuageDynamic(NPSYS.CurrentLanguageType);
        }

        private void SetLanuageDynamic(ConfigID.LanguageType pLanguageType)
        {
            mDeviceNone = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_DEVICE_NONE.ToString());
            mDeviceOk = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_DEVICE_OK.ToString());
            mDeviceError = NPSYS.LanguageConvert.GetLanguageData(pLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_TXT_DEVICE_ERROR.ToString());
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
