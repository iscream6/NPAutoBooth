using FadeFox.Text;
using NPCommon;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using static NPCommon.DEVICE.CoinReader;

namespace NPCommon.DEVICE
{

    /// <summary>
    /// 동전리더기 제어
    /// </summary>
    public class CoinReader : AbstractSerialPort<CoinReaderStatusType>
    {
        ProtocolStep mStep = ProtocolStep.Ready;
        public CoinReaderStatusManageMent CurrentCoinReaderStatusManageMen = new CoinReaderStatusManageMent();

        #region 속성

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override CoinReaderStatusType Connect()
        {

            if (SerialPort.IsOpen)
                SerialPort.Close();


            SerialPort.ReadTimeout = 1000;
            SerialPort.WriteTimeout = 1000;
            SerialPort.DtrEnable = true;
            SerialPort.RtsEnable = true;

            SerialPort.DataBits = 8;
            SerialPort.StopBits = System.IO.Ports.StopBits.One;
            SerialPort.Handshake = System.IO.Ports.Handshake.None;

            try
            {
                SerialPort.Open();
                Initialize();
                CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.PortOpenStatus, true);
                return CoinReaderStatusType.OK;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COINREADER, "CoinReader|Connect", "연결실패:" + ex.ToString());
                CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.PortOpenStatus, false);
                return CoinReaderStatusType.PortOpenStatus;
            }
        }

        public override void Disconnect()
        {
            try
            {
                SerialPort.Close();
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COINREADER, "CoinReader|Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        #endregion



        public CoinReader()
        {
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }



        private byte[] mReceiveData = null;

        private enum ProtocolStep
        {
            Ready,
            SendCommand,
            ReceiveACK,
            SendENQ,
            SendACK,
            ReceiveData
        }

        /////////////////////// 명령어들 정리 ///////////////////////////
        /// <summary>
        /// 프로토콜 시작문자
        /// </summary>
        private const byte g_SYNC = 0x90;
        /// <summary>
        /// 프로토콜 종료문자
        /// </summary>
        private const byte g_EXT = 0x03;
        /// <summary>
        /// 프로토콜의 길이
        /// </summary>
        private const byte g_ProtocolFiveLength = 0x05;
        /// <summary>
        /// 동전읽기 시작
        /// </summary>
        private const byte g_CMD_EnableUca = 0x01;
        /// <summary>
        /// 동전읽기 중지
        /// </summary>
        private const byte g_CMD_DisableUca = 0x02;
        /// <summary>
        /// 현재상태 요청
        /// </summary>
        private const byte g_CMD_CurrentStatus = 0x11;

        private const byte g_Respone_Ack = 0x50;
        private const byte g_Respone_Nak = 0x4B;
        private const byte g_CMD_Accepting = 0x12;


        private const byte g_TIMEOUT_STATUS = 0x99;

        ///////////////////////////////////////////////////////


        private enum Cmd
        {
            EnableUca,
            DisableUca,
            CurrentStatus
        }
        private byte cmdSelect(Cmd p_CmdData)
        {
            byte cmdData = new byte();
            switch (p_CmdData)
            {
                case Cmd.EnableUca:
                    cmdData = g_CMD_EnableUca;
                    break;
                case Cmd.DisableUca:
                    cmdData = g_CMD_DisableUca;
                    break;
                case Cmd.CurrentStatus:
                    cmdData = g_CMD_CurrentStatus;
                    break;

            }
            return cmdData;
        }


        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

        public List<byte> mReadBuffer = new List<byte>();



        //동전연속투입관련 변경
        public List<string> mLIstQty = new List<string>();
        //동전연속투입관련 변경
        /// <summary>
        /// 기본 응답포맷은 SYNC(1)+LNG(1)+CMD(1)+DATA(0-250)+EXT(1)+CHECKSUM(1)으로 이루어짐 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int length = SerialPort.BytesToRead;

                mStep = ProtocolStep.ReceiveData;

                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)SerialPort.ReadByte());
                }

                byte[] resData = mReadBuffer.ToArray();

                string data = "";
                for (int i = 0; i < resData.Length; i++)
                {
                    data += i.ToString() + ":" + resData[i].ToString("X") + " ";
                }
                TextCore.ACTION(TextCore.ACTIONS.COINREADER, "CoinReader|mSerialPort_DataReceived", "데이터수신:" + data);

                if (mReadBuffer.Contains(g_EXT))
                {
                    if (!mReadBuffer.Contains(g_SYNC)) // 시작 데이터가 없으면 버린다.
                    {
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return;
                    }
                    int l_DataStart = -1;
                    int l_DataExit = -1;
                    int l_Datalength = -1;

                    byte[] res = mReadBuffer.ToArray();

                    for (int i = 0; i < res.Length; i++)
                    {
                        switch (res[i])
                        {
                            case g_EXT:
                                l_DataExit = i; // 프로토콜 종료점(여기에 checksum이 1 byte 추가로 더 들어간다
                                break;
                            case g_SYNC:
                                l_DataStart = i; // 프로토콜 시작점
                                l_Datalength = Convert.ToInt32(res[i + 1]); // 프로토콜상의 전문길이
                                break;

                        }
                    }

                    if (res[l_DataExit] == g_EXT && res[l_DataStart + 2] == g_CMD_Accepting) // Cmd가 0x12로 오면 동전이 들어온 경우임
                    {

                        // 싱가폴

                        if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.WON)

                        {
                            //동전연속투입관련 변경
                            switch (res[l_DataStart + 3])
                            {
                                case 0x03: //50원
                                    mLIstQty.Add("50Qty");
                                    break;
                                case 0x04: //100원
                                    mLIstQty.Add("100Qty");
                                    break;
                                case 0x05: //500원
                                    mLIstQty.Add("500Qty");
                                    break;

                            }
                        }

                        if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.PHP)

                        {
                            //동전연속투입관련 변경
                            switch (res[l_DataStart + 3])
                            {
                                case 0x03:
                                case 0x12:
                                case 0x01: //50원
                                    mLIstQty.Add("1Qty");
                                    break;
                                case 0x05: //100원
                                case 0x07:
                                    mLIstQty.Add("5Qty");
                                    break;
                                case 0x06: //500원
                                case 0x08: //500원
                                    mLIstQty.Add("10Qty");
                                    break;

                            }
                        }

                        if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.PINWONTEST)
                        {

                            //동전연속투입관련 변경
                            switch (res[l_DataStart + 3])
                            {
                                case 0x03:
                                case 0x12:
                                case 0x01: //50원
                                    mLIstQty.Add("50Qty");
                                    break;
                                case 0x05: //100원
                                case 0x07:
                                    mLIstQty.Add("100Qty");
                                    break;
                                case 0x06: //500원
                                case 0x08: //500원
                                    mLIstQty.Add("500Qty");
                                    break;

                            }

                        }



                        //동전연속투입관련 변경
                        mReadBuffer.Clear();

                        System.Threading.Thread.Sleep(300);

                        mReceiveData = res;
                        mStep = ProtocolStep.Ready;
                    }
                    else if (res[l_DataExit] == g_EXT && (res.Length >= l_Datalength + l_DataStart)) // 데이터가 다들어옴. (정상적이든, 에러든)
                    {
                        mStep = ProtocolStep.ReceiveData;
                        ////////////////////////////////////// 2012-08-13 일 추가 
                        mReadBuffer.Clear(); //test
                        byte[] res2 = new byte[res.Length - l_DataStart];

                        int resStart = 0;
                        for (int i = l_DataStart; i < res.Length; i++)
                        {
                            res2[resStart] = res[i];
                            resStart = resStart + 1;

                        }

                        mReceiveData = res2;
                        string data2 = "";
                        for (int i = 0; i < res2.Length; i++)
                        {
                            data2 += i.ToString() + ":" + res2[i].ToString("X") + " ";
                        }
                        TextCore.ACTION(TextCore.ACTIONS.COINREADER, "CoinReader|mSerialPort_DataReceived", "걸러낸최종명령 데이터수신:" + data2);
                        /////////////////////////////////////
                        mStep = ProtocolStep.Ready;
                    }
                }

            }
            catch (Exception ex)
            {

                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinReader|mSerialPort_DataReceived", ex.ToString());
            }
        }

        private int mTimeOut = 3000;
        private byte[] mTimeOutData = { g_SYNC, 0x00, g_TIMEOUT_STATUS, g_EXT, 0x00 };
        private byte[] SendCommend(Cmd CmdData)
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //}

                mReadBuffer.Clear();

                byte GetBccData = (byte)(g_SYNC + g_ProtocolFiveLength + cmdSelect(CmdData) + g_EXT);

                byte[] l_SendCommand = new byte[5];

                l_SendCommand[0] = g_SYNC;  // start
                l_SendCommand[1] = g_ProtocolFiveLength;    //length
                l_SendCommand[2] = cmdSelect(CmdData);  // command
                l_SendCommand[3] = g_EXT;  // end
                l_SendCommand[4] = GetBccData;  // BCC

                mStep = ProtocolStep.SendCommand;
                TextCore.ACTION(TextCore.ACTIONS.COINREADER, "CoinReader|SendCommand", "명령어보냄:" + CmdData.ToString());
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        mStep = ProtocolStep.Ready;
                        TextCore.DeviceError(TextCore.DEVICE.COINREADER, "COINREADER|SendCommand", "응답시간초과");
                        return mTimeOutData;
                    }
                }
                // NPSYS.WriteLog(mReceiveData[0].ToString("X") + " " + mReceiveData[1].ToString("X") + " " + mReceiveData[2].ToString("X") + " " + mReceiveData[3].ToString("X") + " " + mReceiveData[4].ToString("X"));

                return mReceiveData;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "COINREADER|SendCommand", "예외사항:" + ex.ToString());
                return mTimeOutData;
            }
        }


        public CoinReaderStatusType EnableCoinRead()
        {

            byte[] result = SendCommend(Cmd.EnableUca);
            return ConvertStatusTypeToByte(result);
        }

        public CoinReaderStatusType DisableCoinRead()
        {
            byte[] result = SendCommend(Cmd.DisableUca);
            return ConvertStatusTypeToByte(result);
        }


        public CoinReaderStatusType CurrentStatus()
        {
            byte[] result = SendCommend(Cmd.CurrentStatus);
            return ConvertStatusTypeToByte(result);

        }




        public enum CoinReaderStatusType
        {
            DeviceStatus = 2301,
            CommuniCationStatus = 2302,
            PortOpenStatus = 2303,
            NAK = 2304,
            Reserve = 2305,
            Disable = 2306,
            Fishing = 2307,
            Sensor_1_Problem_COIL1 = 2308,
            Sensor_2_Problem_COIL2 = 2309,
            Sensor_3_Problem_DROP2 = 2310,
            Sensor_4_Problem_DROP1 = 2311,
            Sensor_5_Problem_COINRTURN = 2312,
            UNKNOWN_ERROR = 2313,
            NONE = 2314,
            OK = 2315,

        }

        public CoinReaderStatusType ConvertStatusTypeToByte(byte[] pRawData)
        {


            if (pRawData.Length < 5)
            {
                return CoinReaderStatusType.NONE;
            }

            if (pRawData[0] != 0x90)
            {
                return CoinReaderStatusType.NONE;
            }
            if (pRawData[2] == 0x50 || pRawData[2] == 0x12)
            {
                CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.OK, true);
                return CoinReaderStatusType.OK;
            }
            else
            {
                return ConvertStatusTypeToByte(pRawData[2], pRawData[3]);

            }

        }
        public CoinReaderStatusType ConvertStatusTypeToByte(byte status, byte errorStatus)
        {


            switch (status)
            {
                case 0x4B:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.NAK, false);
                    return CoinReaderStatusType.NAK;


                case 0x13:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Reserve, false);
                    return CoinReaderStatusType.Reserve;

                case 0x14:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Disable, false);
                    return CoinReaderStatusType.Disable;

                case 0x15:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Reserve, false);
                    return CoinReaderStatusType.Reserve;

                case 0x17:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Fishing, false);
                    return CoinReaderStatusType.Fishing;
                case 0x18:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Fishing, false);
                    return CoinReaderStatusType.Fishing;
                case 0x19:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Reserve, false);
                    return CoinReaderStatusType.Reserve;
                case 0x99:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.CommuniCationStatus, false);
                    return CoinReaderStatusType.CommuniCationStatus;
            }


            switch (errorStatus)
            {
                case 0x01:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Sensor_1_Problem_COIL1, false);
                    return CoinReaderStatusType.Sensor_1_Problem_COIL1;
                case 0x02:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Sensor_2_Problem_COIL2, false);
                    return CoinReaderStatusType.Sensor_2_Problem_COIL2;
                case 0x03:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Sensor_3_Problem_DROP2, false);
                    return CoinReaderStatusType.Sensor_3_Problem_DROP2;
                case 0x04:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Sensor_4_Problem_DROP1, false);
                    return CoinReaderStatusType.Sensor_4_Problem_DROP1;
                case 0x05:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.Sensor_5_Problem_COINRTURN, false);
                    return CoinReaderStatusType.Sensor_5_Problem_COINRTURN;
                case 0x99:
                    CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.CommuniCationStatus, false);
                    return CoinReaderStatusType.CommuniCationStatus;

            }
            CurrentCoinReaderStatusManageMen.SetCoinReaderDeviceStatus(CoinReaderStatusType.CommuniCationStatus, false);
            return CoinReaderStatusType.CommuniCationStatus;

        }

        public class CoinReaderStatusManageMent
        {
            public void SetDbErrorInfo(DataTable pPrinterData)
            {
                if (pPrinterData != null && pPrinterData.Rows.Count > 0)
                {
                    foreach (DataRow printerItem in pPrinterData.Rows)
                    {
                        int statusCode = Convert.ToInt32(printerItem["STATUSCODE"].ToString());
                        bool isSuccess = true;

                        if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Success.ToString())
                        {
                            isSuccess = true;
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, statusCode);
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            isSuccess = false;
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, statusCode);
                        }

                        switch ((CoinReaderStatusType)statusCode)
                        {
                            case CoinReaderStatusType.CommuniCationStatus:
                                mIsComuniCationError = !isSuccess;

                                break;
                            case CoinReaderStatusType.DeviceStatus:
                                mIsDeviceError = !isSuccess;

                                break;
                            case CoinReaderStatusType.Disable:
                                mIsDisableError = !isSuccess;

                                break;

                            case CoinReaderStatusType.Fishing:
                                mIsFishingError = !isSuccess;

                                break;

                            case CoinReaderStatusType.NAK:
                                mIsNakError = !isSuccess;

                                break;

                            case CoinReaderStatusType.PortOpenStatus:
                                mIsPortOpenError = !isSuccess;

                                break;

                            case CoinReaderStatusType.Reserve:
                                mIsReserveError = !isSuccess;

                                break;

                            case CoinReaderStatusType.Sensor_1_Problem_COIL1:
                                mIsSensor_1_Problem_COIL1Error = !isSuccess;

                                break;

                            case CoinReaderStatusType.Sensor_2_Problem_COIL2:
                                mIsSensor_2_Problem_COIL2Error = !isSuccess;

                                break;

                            case CoinReaderStatusType.Sensor_3_Problem_DROP2:
                                mIsSensor_3_Problem_DROP2Error = !isSuccess;

                                break;

                            case CoinReaderStatusType.Sensor_4_Problem_DROP1:
                                mIsSensor_4_Problem_DROP1Error = !isSuccess;

                                break;


                            case CoinReaderStatusType.Sensor_5_Problem_COINRTURN:
                                mIsSensor_5_Problem_COINRTURN = !isSuccess;

                                break;

                            case CoinReaderStatusType.UNKNOWN_ERROR:
                                mIsUNKOWN_ERROR = !isSuccess;

                                break;



                        }
                    }
                    NPSYS.Device.isUseDeviceCoinReaderDevice = GetCoinReaderDeveiceOpertationYn();
                }
                else
                {
                    mIsComuniCationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.CommuniCationStatus);

                    mIsDeviceError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.DeviceStatus);

                    mIsDisableError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Disable);

                    mIsFishingError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Fishing);

                    mIsNakError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.NAK);

                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.PortOpenStatus);

                    mIsReserveError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Reserve);

                    mIsSensor_1_Problem_COIL1Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_1_Problem_COIL1);

                    mIsSensor_2_Problem_COIL2Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_2_Problem_COIL2);

                    mIsSensor_3_Problem_DROP2Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_3_Problem_DROP2);

                    mIsSensor_4_Problem_DROP1Error = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_4_Problem_DROP1);


                    mIsSensor_5_Problem_COINRTURN = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_5_Problem_COINRTURN);

                    mIsUNKOWN_ERROR = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.UNKNOWN_ERROR);



                    NPSYS.Device.isUseDeviceCoinReaderDevice = GetCoinReaderDeveiceOpertationYn();
                }
            }

            public void SendAllDeviveOk()
            {

                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.CommuniCationStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.DeviceStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Disable);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Fishing);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.NAK);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.PortOpenStatus);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Reserve);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_1_Problem_COIL1);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_2_Problem_COIL2);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_3_Problem_DROP2);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_4_Problem_DROP1);



                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_5_Problem_COINRTURN);


                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.UNKNOWN_ERROR);

            }

            public void SetCoinReaderDeviceStatus(CoinReaderStatusType pResultType, bool isSuccess)
            {
                switch (pResultType)
                {
                    case CoinReaderStatusType.CommuniCationStatus:
                        SetComuniCationOk(isSuccess);
                        break;
                    case CoinReaderStatusType.DeviceStatus:
                        SetDeviceOk(isSuccess);
                        break;
                    case CoinReaderStatusType.Disable:
                        SetDisableOk(isSuccess);
                        break;
                    case CoinReaderStatusType.Fishing:
                        SetFishingOk(isSuccess);
                        break;
                    case CoinReaderStatusType.NAK:
                        SetNakOk(isSuccess);
                        break;
                    case CoinReaderStatusType.OK:
                        SetComuniCationOk(isSuccess);
                        SetDeviceOk(isSuccess);
                        SetDisableOk(isSuccess);
                        SetFishingOk(isSuccess);
                        SetNakOk(isSuccess);
                        SetPortOpenOk(isSuccess);
                        SetReserveOk(isSuccess);
                        SetSensor1Ok(isSuccess);
                        SetSensor2Ok(isSuccess);
                        SetSensor3Ok(isSuccess);
                        SetSensor4Ok(isSuccess);
                        SetSensor5Ok(isSuccess);
                        SetUnKnownOk(isSuccess);
                        break;

                    case CoinReaderStatusType.PortOpenStatus:
                        SetPortOpenOk(isSuccess);
                        break;

                    case CoinReaderStatusType.Reserve:
                        SetReserveOk(isSuccess);
                        break;

                    case CoinReaderStatusType.Sensor_1_Problem_COIL1:
                        SetSensor1Ok(isSuccess);
                        break;

                    case CoinReaderStatusType.Sensor_2_Problem_COIL2:
                        SetSensor2Ok(isSuccess);
                        break;

                    case CoinReaderStatusType.Sensor_3_Problem_DROP2:
                        SetSensor3Ok(isSuccess);
                        break;
                    case CoinReaderStatusType.Sensor_4_Problem_DROP1:
                        SetSensor4Ok(isSuccess);
                        break;
                    case CoinReaderStatusType.Sensor_5_Problem_COINRTURN:
                        SetSensor5Ok(isSuccess);
                        break;
                    case CoinReaderStatusType.UNKNOWN_ERROR:
                        SetUnKnownOk(isSuccess);
                        break;


                }

                NPSYS.Device.isUseDeviceCoinReaderDevice = GetCoinReaderDeveiceOpertationYn();
            }

            private bool GetCoinReaderDeveiceOpertationYn()
            {
                if ((mIsComuniCationError
                    || mIsDeviceError
                    || mIsDisableError
                    || mIsFishingError
                    || mIsNakError
                    || mIsPortOpenError
                    || mIsReserveError
                    || mIsSensor_1_Problem_COIL1Error
                    || mIsSensor_2_Problem_COIL2Error
                    || mIsSensor_3_Problem_DROP2Error
                    || mIsSensor_4_Problem_DROP1Error
                    || mIsSensor_5_Problem_COINRTURN
                    || mIsUNKOWN_ERROR
                    ) == true)
                {
                    return false;
                }
                return true;
            }


            private bool mIsComuniCationError = false;
            public bool IsComuniCationError
            {
                get { return mIsComuniCationError; }
            }


            private void SetComuniCationOk(bool pIscommunication)
            {
                if (mIsComuniCationError) // 기존에 통신에러였다면
                {
                    if (pIscommunication) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.CommuniCationStatus);
                        mIsComuniCationError = !pIscommunication;

                    }
                }
                else  // 기존에 정상일때
                {
                    if (pIscommunication == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.CommuniCationStatus);

                        mIsComuniCationError = !pIscommunication;

                    }
                }

            }




            private bool mIsDeviceError = false;
            public bool IsDeviceError
            {
                get { return mIsDeviceError; }
            }


            private void SetDeviceOk(bool pIsDeviceOk)
            {
                if (mIsDeviceError) // 기존에 에러였다면
                {
                    if (pIsDeviceOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.DeviceStatus);
                        mIsDeviceError = !pIsDeviceOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDeviceOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.DeviceStatus);

                        mIsDeviceError = !pIsDeviceOk;

                    }
                }

            }

            private bool mIsDisableError = false;
            public bool IsDisableError
            {
                get { return mIsDisableError; }
            }


            private void SetDisableOk(bool pIsDisableOk)
            {
                if (mIsDisableError) // 기존에 에러였다면
                {
                    if (pIsDisableOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Disable);
                        mIsDisableError = !pIsDisableOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsDisableOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Disable);

                        mIsDisableError = !pIsDisableOk;

                    }
                }

            }



            private bool mIsFishingError = false;
            public bool IsFishingError
            {
                get { return mIsFishingError; }
            }


            private void SetFishingOk(bool pIsFishingeOk)
            {
                if (mIsFishingError) // 기존에 에러였다면
                {
                    if (pIsFishingeOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Fishing);
                        mIsFishingError = !pIsFishingeOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsFishingeOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Fishing);

                        mIsFishingError = !pIsFishingeOk;

                    }
                }
            }


            private bool mIsNakError = false;
            public bool IsNakError
            {
                get { return mIsNakError; }
            }


            private void SetNakOk(bool pIsNakOk)
            {
                if (mIsNakError) // 기존에 에러였다면
                {
                    if (pIsNakOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.NAK);
                        mIsNakError = !pIsNakOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsNakOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.NAK);

                        mIsNakError = !pIsNakOk;

                    }
                }
            }


            private bool mIsPortOpenError = false;
            public bool IsPortOpenError
            {
                get { return mIsPortOpenError; }
            }


            private void SetPortOpenOk(bool pIsPortOpenOk)
            {
                if (mIsPortOpenError) // 기존에 에러였다면
                {
                    if (pIsPortOpenOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.PortOpenStatus);
                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsPortOpenOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.PortOpenStatus);

                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
            }



            private bool mIsReserveError = false;
            public bool IsReserveError
            {
                get { return mIsReserveError; }
            }

            private void SetReserveOk(bool pIsReserveOk)
            {
                if (mIsReserveError) // 기존에 에러였다면
                {
                    if (pIsReserveOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Reserve);
                        mIsReserveError = !pIsReserveOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsReserveOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Reserve);

                        mIsReserveError = !pIsReserveOk;

                    }
                }
            }


            private bool mIsSensor_1_Problem_COIL1Error = false;
            public bool IsSensor_1_Problem_COIL1Error
            {
                get { return mIsSensor_1_Problem_COIL1Error; }
            }

            private void SetSensor1Ok(bool pIsSensor1Ok)
            {
                if (mIsSensor_1_Problem_COIL1Error) // 기존에 에러였다면
                {
                    if (pIsSensor1Ok) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_1_Problem_COIL1);
                        mIsSensor_1_Problem_COIL1Error = !pIsSensor1Ok;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSensor1Ok == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Sensor_1_Problem_COIL1);

                        mIsSensor_1_Problem_COIL1Error = !pIsSensor1Ok;

                    }
                }
            }


            private bool mIsSensor_2_Problem_COIL2Error = false;
            public bool IsSensor_2_Problem_COIL2Error
            {
                get { return mIsSensor_2_Problem_COIL2Error; }
            }

            private void SetSensor2Ok(bool pIsSensor2Ok)
            {
                if (mIsSensor_2_Problem_COIL2Error) // 기존에 에러였다면
                {
                    if (pIsSensor2Ok) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_2_Problem_COIL2);
                        mIsSensor_2_Problem_COIL2Error = !pIsSensor2Ok;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSensor2Ok == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Sensor_2_Problem_COIL2);

                        mIsSensor_2_Problem_COIL2Error = !pIsSensor2Ok;

                    }
                }
            }


            private bool mIsSensor_3_Problem_DROP2Error = false;
            public bool IsSensor_3_Problem_DROP2Error
            {
                get { return mIsSensor_3_Problem_DROP2Error; }
            }

            private void SetSensor3Ok(bool pIsSensor3Ok)
            {
                if (mIsSensor_3_Problem_DROP2Error) // 기존에 에러였다면
                {
                    if (pIsSensor3Ok) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_3_Problem_DROP2);
                        mIsSensor_3_Problem_DROP2Error = !pIsSensor3Ok;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSensor3Ok == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Sensor_3_Problem_DROP2);

                        mIsSensor_3_Problem_DROP2Error = !pIsSensor3Ok;

                    }
                }
            }


            private bool mIsSensor_4_Problem_DROP1Error = false;
            public bool IsSensor_4_Problem_DROP1Error
            {
                get { return mIsSensor_4_Problem_DROP1Error; }
            }

            private void SetSensor4Ok(bool pIsSensor4Ok)
            {
                if (mIsSensor_4_Problem_DROP1Error) // 기존에 에러였다면
                {
                    if (pIsSensor4Ok) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_4_Problem_DROP1);
                        mIsSensor_4_Problem_DROP1Error = !pIsSensor4Ok;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSensor4Ok == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Sensor_4_Problem_DROP1);

                        mIsSensor_4_Problem_DROP1Error = !pIsSensor4Ok;

                    }
                }
            }


            private bool mIsSensor_5_Problem_COINRTURN = false;
            public bool IsSensor_5_Problem_COINRTURN
            {
                get { return mIsSensor_5_Problem_COINRTURN; }
            }

            private void SetSensor5Ok(bool pIsSensor5Ok)
            {
                if (mIsSensor_5_Problem_COINRTURN) // 기존에 에러였다면
                {
                    if (pIsSensor5Ok) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.Sensor_5_Problem_COINRTURN);
                        mIsSensor_5_Problem_COINRTURN = !pIsSensor5Ok;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsSensor5Ok == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.Sensor_5_Problem_COINRTURN);

                        mIsSensor_5_Problem_COINRTURN = !pIsSensor5Ok;

                    }
                }
            }

            private bool mIsUNKOWN_ERROR = false;
            public bool IsUNKOWN_ERROR
            {
                get { return mIsUNKOWN_ERROR; }
            }

            private void SetUnKnownOk(bool pIsUnknownStatusOk)
            {
                if (mIsUNKOWN_ERROR) // 기존에 에러였다면
                {
                    if (pIsUnknownStatusOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Success, (int)CoinReaderStatusType.UNKNOWN_ERROR);
                        mIsUNKOWN_ERROR = !pIsUnknownStatusOk;

                    }
                }
                else  // 기존에  정상일때
                {
                    if (pIsUnknownStatusOk == false) // 현재 장비가 에러라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CRE, CommProtocol.DeviceStatus.Fail, (int)CoinReaderStatusType.UNKNOWN_ERROR);

                        mIsUNKOWN_ERROR = !pIsUnknownStatusOk;

                    }
                }
            }
        }


    }
}

