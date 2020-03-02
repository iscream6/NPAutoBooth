using FadeFox.Text;
using NPCommon;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;

namespace NPCommon.DEVICE
{

    /// <summary>
    /// 지폐리더기 제어
    /// </summary>
    public class BillReader : AbstractSerialPort<bool>
    {
        ProtocolStep mStep = ProtocolStep.Ready;
        public BillReaderStatusManageMent CurrentReaderStatusManageMent = new BillReaderStatusManageMent();

        #region 속성

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override bool Connect()
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
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BillReader|Connect", "연결정보:Baudrate:" + SerialPort.BaudRate.ToString() + " Parity:" + SerialPort.Parity.ToString() + " StopBit:" + SerialPort.StopBits.ToString() + " HandShake:" + SerialPort.Handshake.ToString() + " Databit:" + SerialPort.DataBits.ToString());

            try
            {
                SerialPort.Open();
                Initialize();
                CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.PortOpenStatus, true);
                return true;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "BillReader|Connect", "연결실패:" + ex.ToString());
                CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.PortOpenStatus, false);
                return false;
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
                TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "BillReader|Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        #endregion



        public BillReader()
        {
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }



        private byte[] mReceiveData = null;

        private enum ProtocolStep
        {
            Ready,
            None,
            SendCommand,
            ReceiveData,
            Reset,
            BillAccept,
            BillInsertEnable,
            BillInsertDisalbe,
            CurrentStatus
        }
        /// <summary>
        /// 지폐입수가능 명령
        /// </summary>
        private const byte g_CommandBillEnableAccept = 0x3E;
        /// <summary>
        /// 지폐입수 불가 명령
        /// </summary>
        private const byte g_CommandBillDisableAccept = 0x5E;
        /// <summary>
        /// 현재 상태 요청 명령
        /// </summary>
        private const byte g_CommandBillCurrentStatus = 0x0C;
        /// <summary>
        /// 리셋명령
        /// </summary>
        private const byte g_CommandBillReset = 0x30;

        /// <summary>
        /// 돈통에 지폐넣음
        /// </summary>
        private const byte g_CommandBillAccept = 0x02;

        /// <summary>
        /// 돈통에서 지폐방출
        /// </summary>
        private const byte g_CommandBillReject = 0x0F;



        /// <summary>
        /// 지폐입수명령에 대한 정상응답
        /// </summary>
        private const byte BillAcceptompleteStatus = 0x10;
        /// <summary>
        /// 지폐입수 불가명령에 대한 정상응답
        /// </summary>
        private const byte BillAcceptInompleteStatus = 0x11;


        private const byte OkStatus = 0x98;
        //////////////////////////////////// error message //////////////////////////////        
        private const byte MotorFailureError = 0x20;
        private const byte ChecksumError = 0x21;
        private const byte BillJamError = 0x22;
        private const byte BillRemoveError = 0x23;
        private const byte StackerOpenError = 0x24;
        private const byte SensorProblemError = 0x25;
        private const byte BillFishError = 0x27;
        private const byte StackerProblemError = 0x28;
        private const byte BillRejectError = 0x29;
        private const byte InvalidCommandError = 0x2A;
        private const byte BillAcceptEnableStatus = 0x3E;
        private const byte BillAcceptInhibitOrDIsableStatus = 0x5E;
        private const byte BillTimeOutError = 0x99;

        ////////////////////////////////////////////////////////////////////////

        private enum Cmd
        {
            CommandBillEnableAccept,
            CommandBillDisableAccept,
            CommandBillCurrentStatus,
            CommandBillReset,
            AcceptBill,
            RejectBill
        }
        private byte cmdSelect(Cmd p_CmdData)
        {
            byte cmdData = new byte();
            switch (p_CmdData)
            {
                case Cmd.CommandBillEnableAccept:
                    cmdData = g_CommandBillEnableAccept;
                    break;
                case Cmd.CommandBillDisableAccept:
                    cmdData = g_CommandBillDisableAccept;
                    break;
                case Cmd.CommandBillCurrentStatus:
                    cmdData = g_CommandBillCurrentStatus;
                    break;
                case Cmd.CommandBillReset:
                    cmdData = g_CommandBillReset;
                    break;
                case Cmd.AcceptBill:
                    cmdData = g_CommandBillAccept;
                    break;
                case Cmd.RejectBill:
                    cmdData = g_CommandBillReject;
                    break;

            }
            return cmdData;
        }

        public List<byte> mReadBuffer = new List<byte>();

        public static string g_billValue = "";

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

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

                byte[] res = mReadBuffer.ToArray();

                string data = "";
                for (int i = 0; i < res.Length; i++)
                {
                    data += i.ToString() + ":" + res[i].ToString("X") + " ";
                }
                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + data);

                if (mReadBuffer.Contains(0x10))
                {
                    byte[] returnValue = { 0x10 };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                else if (mReadBuffer.Contains(0x11))
                {
                    byte[] returnValue = { 0x11 };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;

                }

                if (mReadBuffer.Contains(BillAcceptompleteStatus))
                {
                    byte[] returnValue = { BillAcceptompleteStatus };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(MotorFailureError))
                {
                    byte[] returnValue = { MotorFailureError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "MotorFailureError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(ChecksumError))
                {
                    byte[] returnValue = { ChecksumError };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillJamError))
                {
                    byte[] returnValue = { BillJamError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "BillJamError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillRemoveError))
                {
                    byte[] returnValue = { BillRemoveError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "BillRemoveError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(StackerOpenError))
                {
                    byte[] returnValue = { StackerOpenError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "StackerOpenError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(SensorProblemError))
                {
                    byte[] returnValue = { SensorProblemError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "SensorProblemError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillFishError))
                {
                    byte[] returnValue = { BillFishError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "BillFishError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(StackerProblemError))
                {
                    byte[] returnValue = { StackerProblemError };
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "StackerProblemError");
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillRejectError))
                {
                    if (mReadBuffer.Contains(0x2F))  // 잘못된 지폐여서 다시 앞으로나옴
                    {
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|mSerialPort_DataReceived", "데이터수신:" + "잘못된 지폐 확인");
                        byte[] returnValue = { OkStatus };
                        mReceiveData = returnValue;
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return;
                    }
                    else
                    {
                        mStep = ProtocolStep.ReceiveData;
                        return;
                    }
                }
                if (mReadBuffer.Contains(InvalidCommandError))
                {
                    byte[] returnValue = { InvalidCommandError };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillAcceptEnableStatus))
                {
                    byte[] returnValue = { BillAcceptEnableStatus };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(BillAcceptInhibitOrDIsableStatus))
                {
                    byte[] returnValue = { BillAcceptInhibitOrDIsableStatus };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (mReadBuffer.Contains(0x47)) // response when error status is exclusion
                {
                    byte[] returnValue = { 0x47 };
                    mReceiveData = returnValue;
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }

                if (mReadBuffer.Contains(0x81))
                {
                    if (mReadBuffer.Contains(0x40) || mReadBuffer.Contains(0x41) || mReadBuffer.Contains(0x42) || mReadBuffer.Contains(0x43))
                    {
                        //if (!mReadBuffer.Contains(0x81))
                        //{
                        //    mReadBuffer.Clear();
                        //    mStep = ProtocolStep.Ready;
                        //    return;
                        //}
                        int l_DataStart = -1;

                        for (int i = 0; i < res.Length; i++)
                        {
                            switch (res[i])
                            {
                                case 0x81:
                                    l_DataStart = i;
                                    break;

                            }
                        }
                        g_billValue = "";

                        // 싱가폴 : 0x40=20 , 0x41=50, 0x42=100페소
                        if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.WON)
                        {
                            switch (res[l_DataStart + 1])
                            {
                                case 0x40:

                                    g_billValue = "1000Qty";
                                    break;
                                case 0x41:
                                    g_billValue = "5000Qty";
                                    break;
                                case 0x42:
                                    g_billValue = "10000Qty";
                                    break;
                                case 0x43:
                                    g_billValue = "50000Qty";
                                    break;

                            }
                        }
                        if (NPSYS.CurrentMoneyType == NPCommon.ConfigID.MoneyType.PHP)
                        {
                            switch (res[l_DataStart + 1])
                            {
                                case 0x40:

                                    g_billValue = "20Qty";
                                    break;
                                case 0x41:
                                    g_billValue = "50Qty";
                                    break;
                                case 0x42:
                                    g_billValue = "100Qty";
                                    break;

                            }
                        }
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return;
                    }
                    else
                    {
                        mStep = ProtocolStep.Ready;
                        return;
                    }
                }



                if (mReadBuffer.Count > 2)
                {
                    byte[] returnValue = mReadBuffer.ToArray();

                    mStep = ProtocolStep.ReceiveData;

                    mReceiveData = returnValue;

                    mStep = ProtocolStep.Ready;
                    mReadBuffer.Clear();
                    return;
                }

            }

            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|mSerialPort_DataReceived", ex.ToString());
            }
        }

        private byte[] SendCommand(Cmd p_CmdParameter)
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
                //    //;
                //}

                mReadBuffer.Clear();
                byte[] l_SendCommand = new byte[1];
                l_SendCommand[0] = cmdSelect(p_CmdParameter);
                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|SendCommand", "명령어받음:" + p_CmdParameter.ToString());
                mStep = ProtocolStep.SendCommand;
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "BillReader|SendCommand", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }



                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|SendCommand", "예외사항:" + ex.ToString());
                return mTimeOutData;
            }
        }

        /// <summary>
        /// 시리얼로 전송만 하고 응답값은 받지않는다.
        /// </summary>
        /// <param name="p_CmdParameter"></param>
        private void SendCommandNotTimeOut(Cmd p_CmdParameter)
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
                //    //;
                //}

                mReadBuffer.Clear();

                byte[] l_SendCommand = new byte[1];
                l_SendCommand[0] = cmdSelect(p_CmdParameter);
                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|SendCommandNotTimeOut", "명령어보냄:" + p_CmdParameter.ToString());
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|SendCommandNotTimeOut", "예외사항:" + ex.ToString());
            }
        }

        private int mTimeOut = 5000;
        private byte[] mTimeOutData = { 0x99 };
        private byte[] mOkData = { 0x98 };
        public BillRederStatusType BillEnableAccept()
        {
            try
            {
                SendCommandNotTimeOut(Cmd.CommandBillEnableAccept);
                byte[] result = SendCommand(Cmd.CommandBillCurrentStatus);
                return ConvertStatusTypeToByte(result[0]);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|BillEnableAccept", ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }

        public BillRederStatusType BillDIsableAccept()
        {
            try
            {
                SendCommandNotTimeOut(Cmd.CommandBillDisableAccept);
                byte[] result = SendCommand(Cmd.CommandBillCurrentStatus);
                return ConvertStatusTypeToByte(result[0]);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|BillDIsableAccept", ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }


        public BillRederStatusType BillAccept()
        {
            try
            {
                byte[] result = SendCommand(Cmd.AcceptBill);

                if (result[0] == 0x10) // 돈 돈통에 들어감
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|BillAccept", "돈 돈통에 들어감");
                    result[0] = OkStatus;

                    return ConvertStatusTypeToByte(result[0]);
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "BillReader|BillAccept", "돈 돈통에 돈 안들어감 " + result[0].ToString("X"));
                    if (result[0] == OkStatus)
                    {
                        result[0] = BillTimeOutError;
                    }
                    return ConvertStatusTypeToByte(result[0]);


                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|BillAccept", ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }

        public BillRederStatusType BillReject()
        {
            try
            {
                byte[] result = SendCommand(Cmd.RejectBill);

                if (result[0] == 0x10)
                {

                    result[0] = OkStatus;
                    return ConvertStatusTypeToByte(result[0]);
                }
                else
                {
                    return CurrentStatus();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|BillReject", ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }


        public BillRederStatusType CurrentStatus()
        {
            try
            {
                byte[] result = SendCommand(Cmd.CommandBillCurrentStatus);
                return ConvertStatusTypeToByte(result[0]);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|CurrentStatus", ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }

        public BillRederStatusType Reset()
        {
            try
            {

                byte[] result = SendCommand(Cmd.CommandBillReset);
                if (result[0] == 0x99) //timeout이면
                {
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return ConvertStatusTypeToByte(result[0]);

                }
                if (result[0] == 0x80 || result[1] == 0x8F)
                {
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    SendCommandNotTimeOut(Cmd.AcceptBill);
                    return ConvertStatusTypeToByte(mOkData[0]);
                }
                else
                {
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return ConvertStatusTypeToByte(result[0]);
                }



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BillReader|Reset", ex.ToString());

                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }



        public BillRederStatusType ConvertStatusTypeToByte(byte p_errorMessage)
        {

            switch (p_errorMessage)
            {
                case BillReader.BillAcceptInompleteStatus:
                case BillReader.g_CommandBillEnableAccept:
                case BillReader.g_CommandBillDisableAccept:
                case BillReader.OkStatus:
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.OK, true);
                    return BillRederStatusType.OK;

                case 0x20:   //2106
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.MotorFailureError, false);
                    return BillRederStatusType.MotorFailureError;
                case 0x21:  //2107
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.ChecksumError, false);
                    return BillRederStatusType.ChecksumError;
                case 0x22: //2103
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.BillJamError, false);
                    return BillRederStatusType.BillJamError;
                case 0x23:  //2108
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.BillRemoveError, false);
                    return BillRederStatusType.BillRemoveError;

                case 0x24: //2105
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.StackOpenError, false);
                    return BillRederStatusType.StackOpenError;

                case 0x25:  //2104
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.SensorError, false);
                    return BillRederStatusType.SensorError;

                case 0x27:  //2109
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.BillFishError, false);
                    return BillRederStatusType.BillFishError;

                case 0x28: //2110
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.StackerProblemError, false);
                    return BillRederStatusType.StackerProblemError;
                //case 0x29:
                //    return "BillRejectError";
                case 0x2A: //2111
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.InvalidCommandError, false);
                    return BillRederStatusType.InvalidCommandError;
                //case 0x3e:
                //    return "BillAcceptEnableStatus";
                //case 0x5e:
                //    return "BillAcceptInhibitOrDIsableStatus";
                case 0x99: //2102
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.CommuniCationError, false);
                    return BillRederStatusType.CommuniCationError;

                default:   //2101
                    CurrentReaderStatusManageMent.SetDeviceStatus(BillRederStatusType.OK, true);
                    return BillRederStatusType.OK;

            }
        }

        public enum BillRederStatusType
        {
            OK = 0001,
            DeviceError = 2101,
            CommuniCationError = 2102,
            BillJamError = 2103,
            SensorError = 2104,
            StackOpenError = 2105,
            MotorFailureError = 2106,
            ChecksumError = 2107,
            BillRemoveError = 2108,
            BillFishError = 2109,
            StackerProblemError = 2110,
            InvalidCommandError = 2111,
            PortOpenStatus = 2112
        }

        public class BillReaderStatusManageMent
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
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, statusCode);
                            isSuccess = true;
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, statusCode);
                            isSuccess = false;
                        }
                        switch ((BillRederStatusType)statusCode)
                        {
                            case BillRederStatusType.PortOpenStatus:
                                mIsPortOpenError = !isSuccess;

                                break;


                            case BillRederStatusType.BillFishError:
                                mIsBillFishError = !isSuccess;
                                break;
                            case BillRederStatusType.BillJamError:
                                mIsBillJamError = !isSuccess;
                                break;
                            case BillRederStatusType.BillRemoveError:
                                mIsBillRemoveError = !isSuccess;
                                break;
                            case BillRederStatusType.ChecksumError:
                                mIsChecksumError = !isSuccess;
                                break;
                            case BillRederStatusType.CommuniCationError:
                                mIsComuniCationError = !isSuccess;
                                break;
                            case BillRederStatusType.DeviceError:
                                mIsDeviceError = !isSuccess;
                                break;
                            case BillRederStatusType.InvalidCommandError:
                                mIsInvalidCommandError = !isSuccess;
                                break;
                            case BillRederStatusType.MotorFailureError:
                                mIsMotorFailureError = !isSuccess;

                                break;

                            case BillRederStatusType.SensorError:
                                mIsSensorError = !isSuccess;
                                break;
                            case BillRederStatusType.StackerProblemError:
                                mIsStackerProblemError = !isSuccess;
                                break;
                            case BillRederStatusType.StackOpenError:
                                mIsStackOpenError = !isSuccess;
                                break;




                        }
                    }
                    NPSYS.Device.isUseDeviceBillReaderDevice = GetBillreaderDeveiceOpertationYn();
                }
                else
                {
                    mIsPortOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.PortOpenStatus);

                    mIsBillFishError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillFishError);

                    mIsBillJamError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillJamError);

                    mIsBillRemoveError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillRemoveError);

                    mIsChecksumError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.ChecksumError);

                    mIsComuniCationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.CommuniCationError);

                    mIsDeviceError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.DeviceError);

                    mIsInvalidCommandError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.InvalidCommandError);

                    mIsMotorFailureError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.MotorFailureError);

                    mIsSensorError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.SensorError);

                    mIsStackerProblemError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackerProblemError);

                    mIsStackOpenError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackOpenError);
                    NPSYS.Device.isUseDeviceBillReaderDevice = GetBillreaderDeveiceOpertationYn();
                }
            }

            public void SendAllDeviveOk()
            {
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.PortOpenStatus);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillFishError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillJamError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillRemoveError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.ChecksumError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.CommuniCationError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.DeviceError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.InvalidCommandError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.MotorFailureError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.SensorError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackerProblemError);
                CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackOpenError);
            }


            public void SetDeviceStatus(BillRederStatusType pResultType, bool isSuccess)
            {
                switch (pResultType)
                {
                    case BillRederStatusType.PortOpenStatus:
                        SetPortOpenOk(isSuccess);
                        break;
                    case BillRederStatusType.OK:
                        SetPortOpenOk(isSuccess);
                        SetBillFishOk(isSuccess);
                        SetBillJamOk(isSuccess);
                        SetBillRemoveOk(isSuccess);
                        SetChecksumOk(isSuccess);
                        SetComuniCationOk(isSuccess);
                        SetDeviceOk(isSuccess);
                        SetInvalidCommandOk(isSuccess);
                        SetMotorOk(isSuccess);
                        SetSensorOk(isSuccess);
                        SetStackerProblemOk(isSuccess);
                        SetStackOpenOk(isSuccess);
                        break;
                    case BillRederStatusType.BillFishError:
                        SetBillFishOk(isSuccess);
                        break;
                    case BillRederStatusType.BillJamError:
                        SetBillJamOk(isSuccess);
                        break;
                    case BillRederStatusType.BillRemoveError:
                        SetBillRemoveOk(isSuccess);
                        break;
                    case BillRederStatusType.ChecksumError:
                        SetChecksumOk(isSuccess);
                        break;
                    case BillRederStatusType.CommuniCationError:
                        SetComuniCationOk(isSuccess);
                        break;
                    case BillRederStatusType.DeviceError:
                        SetDeviceOk(isSuccess);
                        break;
                    case BillRederStatusType.InvalidCommandError:
                        SetInvalidCommandOk(isSuccess);
                        break;
                    case BillRederStatusType.MotorFailureError:
                        SetMotorOk(isSuccess);
                        break;
                    case BillRederStatusType.SensorError:
                        SetSensorOk(isSuccess);
                        break;
                    case BillRederStatusType.StackerProblemError:
                        SetStackerProblemOk(isSuccess);
                        break;
                    case BillRederStatusType.StackOpenError:
                        SetStackOpenOk(isSuccess);
                        break;

                }

                NPSYS.Device.isUseDeviceBillReaderDevice = GetBillreaderDeveiceOpertationYn();
            }

            private bool GetBillreaderDeveiceOpertationYn()
            {
                if ((mIsBillFishError
                    || mIsBillJamError
                    || mIsBillRemoveError
                    || mIsChecksumError
                    || mIsComuniCationError
                    || mIsDeviceError
                    || mIsInvalidCommandError
                    || mIsMotorFailureError
                    || mIsSensorError
                    || mIsStackerProblemError
                    || mIsStackOpenError
                    || mIsPortOpenError
                    ) == true)
                {
                    return false;
                }
                return true;
            }

            private bool mIsDeviceError = false;
            public bool IsDeviceError
            {
                get { return mIsDeviceError; }
            }


            private void SetDeviceOk(bool pIsDeviceOk)
            {
                if (mIsDeviceError) // 기존에 통신에러였다면
                {
                    if (pIsDeviceOk) // 정상상태로 바뀌면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.DeviceError);
                        mIsDeviceError = !pIsDeviceOk;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIsDeviceOk == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.DeviceError);

                        mIsDeviceError = !pIsDeviceOk;

                    }
                }

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
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.CommuniCationError);
                        mIsComuniCationError = !pIscommunication;

                    }
                }
                else  // 기존에 동전입수가기 정상일때
                {
                    if (pIscommunication == false) // 현재 동전입수기가 정상이라면
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.CommuniCationError);

                        mIsComuniCationError = !pIscommunication;

                    }
                }

            }

            private bool mIsBillJamError = false;
            public bool IsBillJamError
            {
                get { return mIsBillJamError; }
            }


            private void SetBillJamOk(bool pIsBilljamOk)
            {
                if (mIsBillJamError)
                {
                    if (pIsBilljamOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillJamError);
                        mIsBillJamError = !pIsBilljamOk;

                    }
                }
                else
                {
                    if (pIsBilljamOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.BillJamError);

                        mIsBillJamError = !pIsBilljamOk;

                    }
                }

            }



            private bool mIsSensorError = false;
            public bool IsSensorError
            {
                get { return mIsSensorError; }
            }


            private void SetSensorOk(bool pIsSensorOk)
            {
                if (mIsSensorError)
                {
                    if (pIsSensorOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.SensorError);
                        mIsSensorError = !pIsSensorOk;

                    }
                }
                else
                {
                    if (pIsSensorOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.SensorError);

                        mIsSensorError = !pIsSensorOk;

                    }
                }

            }


            private bool mIsStackOpenError = false;
            public bool IsStackOpenError
            {
                get { return mIsStackOpenError; }
            }


            private void SetStackOpenOk(bool pIsStackOpenOk)
            {
                if (mIsStackOpenError)
                {
                    if (pIsStackOpenOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackOpenError);
                        mIsStackOpenError = !pIsStackOpenOk;

                    }
                }
                else
                {
                    if (pIsStackOpenOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.StackOpenError);

                        mIsStackOpenError = !pIsStackOpenOk;

                    }
                }
            }

            private bool mIsMotorFailureError = false;
            public bool IsMotorFailureError
            {
                get { return mIsMotorFailureError; }
            }


            private void SetMotorOk(bool pIsMotorOk)
            {
                if (mIsMotorFailureError)
                {
                    if (pIsMotorOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.MotorFailureError);
                        mIsMotorFailureError = !pIsMotorOk;

                    }
                }
                else
                {
                    if (pIsMotorOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.MotorFailureError);

                        mIsMotorFailureError = !pIsMotorOk;

                    }
                }
            }



            private bool mIsChecksumError = false;
            public bool IsChecksumError
            {
                get { return mIsChecksumError; }
            }


            private void SetChecksumOk(bool pIsCheckSumOk)
            {
                if (mIsChecksumError)
                {
                    if (pIsCheckSumOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.ChecksumError);
                        mIsChecksumError = !pIsCheckSumOk;

                    }
                }
                else
                {
                    if (pIsCheckSumOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.ChecksumError);

                        mIsChecksumError = !pIsCheckSumOk;

                    }
                }
            }


            private bool mIsBillRemoveError = false;
            public bool IsBillRemoveError
            {
                get { return mIsBillRemoveError; }
            }


            private void SetBillRemoveOk(bool pIsBillRemoveOk)
            {
                if (mIsBillRemoveError)
                {
                    if (pIsBillRemoveOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillRemoveError);
                        mIsBillRemoveError = !pIsBillRemoveOk;

                    }
                }
                else
                {
                    if (pIsBillRemoveOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.BillRemoveError);

                        mIsBillRemoveError = !pIsBillRemoveOk;

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
                if (mIsPortOpenError)
                {
                    if (pIsPortOpenOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.PortOpenStatus);
                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
                else
                {
                    if (pIsPortOpenOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.PortOpenStatus);

                        mIsPortOpenError = !pIsPortOpenOk;

                    }
                }
            }


            private bool mIsBillFishError = false;
            public bool IsBillFishErrorr
            {
                get { return mIsBillFishError; }
            }



            private void SetBillFishOk(bool pIsBillFishOk)
            {
                if (mIsBillFishError)
                {
                    if (pIsBillFishOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.BillFishError);
                        mIsBillFishError = !pIsBillFishOk;

                    }
                }
                else
                {
                    if (pIsBillFishOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.BillFishError);

                        mIsBillFishError = !pIsBillFishOk;

                    }
                }
            }


            private bool mIsStackerProblemError = false;
            public bool IsStackerProblemError
            {
                get { return mIsStackerProblemError; }
            }


            private void SetStackerProblemOk(bool pIsStackerProblemOk)
            {
                if (mIsStackerProblemError)
                {
                    if (pIsStackerProblemOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.StackerProblemError);
                        mIsStackerProblemError = !pIsStackerProblemOk;

                    }
                }
                else
                {
                    if (pIsStackerProblemOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.StackerProblemError);

                        mIsStackerProblemError = !pIsStackerProblemOk;

                    }
                }
            }


            private bool mIsInvalidCommandError = false;
            public bool IsInvalidCommandError
            {
                get { return mIsInvalidCommandError; }
            }


            private void SetInvalidCommandOk(bool pIsInvalidCommandOk)
            {
                if (mIsInvalidCommandError)
                {
                    if (pIsInvalidCommandOk)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Success, (int)BillRederStatusType.InvalidCommandError);
                        mIsInvalidCommandError = !pIsInvalidCommandOk;

                    }
                }
                else
                {
                    if (pIsInvalidCommandOk == false)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.BRE, CommProtocol.DeviceStatus.Fail, (int)BillRederStatusType.InvalidCommandError);

                        mIsInvalidCommandError = !pIsInvalidCommandOk;

                    }
                }
            }
        }


    }

    //public class BillReaderResult
    //{
    //    public BillStatusResultType ResultType { get; set; }
    //    public string Message { get; set; }  // 상태 혹은 에러메시지가 포함될 수 있음.
    //    public string Data { get; set; }
    //    public byte ResultCode { get; set; } // 결과 코드

    //    private byte[] mRawData = null;
    //    public byte[] RawData
    //    {
    //        get { return mRawData; }
    //    }

    //    public BillReaderResult()
    //    {
    //    }
    //    public BillReaderResult(byte[] pRawData)
    //    {
    //        this.mRawData = pRawData;

    //        if (mRawData[0] == BillReader.BillAcceptInompleteStatus || mRawData[0] == BillReader.BillAcceptInompleteStatus || mRawData[0] == BillReader.g_CommandBillEnableAccept || mRawData[0] == BillReader.g_CommandBillDisableAccept || mRawData[0] == BillReader.OkStatus)
    //        {
    //            ResultType = BillStatusResultType.OkStatus;
    //            Message = "장비의 결과 정상";
    //            ResultCode = (byte)0x88;

    //        }
    //        else
    //        {
    //            ResultType = BillStatusResultType.Error;
    //            Message = BillReader.BillReaderErrorMessage(mRawData[0]);
    //            ResultCode = mRawData[0];

    //        }


    //    }

    //}
}
