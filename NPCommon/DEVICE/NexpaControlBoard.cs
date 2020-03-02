using FadeFox.Text;
using NPCommon;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Text;

namespace NPCommon.DEVICE
{
    public class NexpaControlBoard : AbstractSerialPort<bool>
    {
        public const byte STX = 0x24;
        public const byte ETX = 0x0A;

        ProtocolStep mStep = ProtocolStep.Ready;
        private BoarData mReceiveData = null;
        public BoardStatus CurrentBoardStatus = new BoardStatus();
        /// <summary>
        /// 자동복구할지 여부
        /// </summary>
        public bool isAutoRecovery = false;

        #region 속성

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override Boolean Connect()
        {
            try
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


                SerialPort.Open();
                Initialize();

                return true;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "DoSensor | Connect", "연결실패:" + ex.ToString());
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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "DoSensor | Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        #endregion



        public NexpaControlBoard()
        {

            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
            mStep = ProtocolStep.Ready;
        }



        //private byte[] mReceiveData = null;

        private enum ProtocolStep
        {
            Ready,
            DoCommand,
            ReceiveACK,
            SendENQ,
            SendACK,
            ReceiveData,
            RemainCoin
        }

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

        public delegate void receiveBoardData(BoarData pBoardData);
        public event receiveBoardData eventreceiveBoardData;
        List<byte> mReadBuffer = new List<byte>();
        /// <summary>
        /// 시리얼로 전송만 하고 응답값은 받지않는다.
        /// </summary>
        /// <param name="p_CmdParameter"></param>
        public void SendCommandNotTimeOut()
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "DoSensor | SendCommandNotTimeOut", "포트가 열려있지 않습니다.");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}

                mReadBuffer.Clear();


                SerialPort.Write(sendByte, 0, sendByte.Length);



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "DoSensor | SendCommandNotTimeOut", "예외사항:" + ex.ToString());
            }
        }


        public BoarData SendCallBoarStatusTimeOut()
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "DoSensor | SendCommandNotTimeOut", "포트가 열려있지 않습니다.");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                mReceiveData = null;
                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                SerialPort.Write(sendByte, 0, sendByte.Length);
                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.BILLREADER, "DoSensor | SendCommand", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return null;

                    }
                }

                return mReceiveData;



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "DoSensor | SendCommandNotTimeOut", "예외사항:" + ex.ToString());
                return null;
            }
        }

        private int mTimeOut = 3000;
        byte[] sendByte = { 0x24, 0x31, 0x50, 0x3B, 0x0A }; //$1P;+0x0a
        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                mStep = ProtocolStep.DoCommand;
                int length = SerialPort.BytesToRead;
                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)SerialPort.ReadByte());
                }
                byte[] resRemain = mReadBuffer.ToArray();
                string data = string.Empty;
                for (int i = 0; i < resRemain.Length; i++)
                {
                    data += i.ToString() + ":" + resRemain[i].ToString("X") + " ";
                }
                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "DoSensor | mSerialPort_DataReceived", "bytedata:" + data);
                int l_stxIndex = mReadBuffer.IndexOf(STX);
                int l_etxIndex = mReadBuffer.IndexOf(ETX);

                if (l_stxIndex == -1)
                {
                    mReadBuffer.Clear();
                    mStep = ProtocolStep.Ready;
                    return;
                }
                if (l_etxIndex == -1)
                {
                    mStep = ProtocolStep.DoCommand;
                    return;
                }
                BoarData boardData = new BoarData();
                boardData.SetData(mReadBuffer.ToArray());
                mReceiveData = boardData;
                eventreceiveBoardData(boardData);
                if (CurrentBoardStatus.isSensorCommunicationError)
                {
                    CurrentBoardStatus.isSensorCommunicationError = false;
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, (int)BoardStatus.ErrorCode.SensorCommunication);
                }

                if (boardData.Fan == BoarData.OnOffType.ON)
                {
                    CurrentBoardStatus.fanOn = true;
                }
                else
                {
                    CurrentBoardStatus.fanOn = false;
                }
                if (boardData.Heater == BoarData.OnOffType.ON)
                {
                    CurrentBoardStatus.heaterOn = true;
                }
                else
                {
                    CurrentBoardStatus.heaterOn = false;
                }
                CurrentBoardStatus.tempAir = boardData.TempAir;

                if (boardData.Door1 == BoarData.OnOffType.ON) // 현재 문이 열렸는대
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "현재문이열려있는대");
                    if (CurrentBoardStatus.isDoorOff == false) // 기존에 닫혀있다면
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "기존에 닫혔다면");
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Fail, (int)BoardStatus.ErrorCode.DoorOff);
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "현재문 열림으로설정");
                        CurrentBoardStatus.isDoorOff = true;
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "기존에도 문이열려있음");
                    }
                }
                else // 문이닫혀있는대
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "현재문이닫혀있는대");
                    if (CurrentBoardStatus.isDoorOff == true)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "isAutoRecovery" + isAutoRecovery.ToString());
                        if (isAutoRecovery)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "기존에 열려있다면");
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, (int)BoardStatus.ErrorCode.DoorOff);
                            CurrentBoardStatus.isDoorOff = false;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "현재문 닫힘으로 설정");
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "현재문 닫힘이지만 자동복구 모두가 아니라 알람보내지않음");
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | mSerialPort_DataReceived", "기존에도 문이닫혀있음");
                    }
                }
                mReadBuffer.RemoveRange(l_stxIndex, l_etxIndex + 1);
                TextCore.ACTION(TextCore.ACTIONS.RECIPT, "DoSensor | mSerialPort_DataReceived", " mStep = ProtocolStep.Ready");
                mStep = ProtocolStep.Ready;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.RECIPT, "DoSensor | mSerialPort_DataReceived", ex.ToString());
                mStep = ProtocolStep.Ready;
                mReadBuffer.Clear();

                return;

            }

        }

        public class BoardStatus
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
                        }
                        else if (printerItem["ISSUCCESS"].ToString() == CommProtocol.DeviceStatus.Fail.ToString())
                        {
                            isSuccess = false;
                        }
                        switch ((ErrorCode)statusCode)
                        {
                            case ErrorCode.DoorOff:
                                isDoorOff = !isSuccess;
                                if (isDoorOff)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;
                            case ErrorCode.SensorCommunication:
                                isSensorCommunicationError = !isSuccess;
                                if (isSensorCommunicationError)
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Fail, statusCode);
                                }
                                else
                                {
                                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, statusCode);
                                }
                                break;

                        }
                    }
                }
                else
                {
                    isSensorCommunicationError = false;
                    /// <summary>
                    /// 문열림
                    /// </summary>
                    isDoorOff = false;
                    /// <summary>
                    /// 문열림
                    /// </summary>
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, (int)ErrorCode.SensorCommunication);
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, CommProtocol.DeviceStatus.Success, (int)ErrorCode.DoorOff);

                }
            }
            public enum ErrorCode
            {
                SensorCommunication = 3902,
                DoorOff = 3903,
            }

            public bool isSensorCommunicationError = false;

            /// <summary>
            /// 문열림 true면 에러
            /// </summary>
            public bool isDoorOff = false;

            public bool fanOn = false; // true면 팬동작
            public bool heaterOn = false;
            public string tempAir = string.Empty;

        }


        public class BoarData
        {

            private bool mIsSuccess = false;
            public bool IsSuccess
            {
                set { mIsSuccess = value; }
                get { return mIsSuccess; }
            }
            public enum OnOffType
            {
                ON,
                Off
            }
            public void SetData(byte[] pByte)
            {
                string data = Encoding.Default.GetString(pByte);
                StringBuilder koreadata = new StringBuilder();
                string[] splitData = data.Split(';');
                splitData[1].Substring(0, 1);
                splitData[1].Substring(1, 1);
                splitData[1].Substring(2, 1);
                Heater = (splitData[1].Substring(3, 1) == "0" ? OnOffType.Off : OnOffType.ON);
                Door1 = splitData[2].Substring(0, 1) == "1" ? OnOffType.Off : OnOffType.ON;
                Door2 = splitData[2].Substring(1, 1) == "1" ? OnOffType.Off : OnOffType.ON;
                Fan = splitData[2].Substring(2, 1) == "0" ? OnOffType.Off : OnOffType.ON;
                TempAir = splitData[3].Substring(0, 3);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSensor | SetData", "[DOOR센서]"
                                                                                  + " Heater:" + Heater.ToString()
                                                                                  + " Door1:" + Door1.ToString()
                                                                                  + " Door2:" + Door2.ToString()
                                                                                  + " Fan:" + Fan.ToString()
                                                                                  + " TempAir:" + TempAir);

                IsSuccess = true;

            }
            public OnOffType Heater
            {
                set; get;
            }
            /// <summary>
            /// on 이면 Door가 열려있음 Off면 Door가 닫혀있음
            /// </summary>

            public OnOffType Door1
            {
                set; get;
            }
            /// <summary>
            /// on 이면 Door가 열려있음 Off면 Door가 닫혀있음
            /// </summary>
            public OnOffType Door2
            {
                set; get;
            }
            public OnOffType Fan
            {
                set; get;
            }
            public string TempAir
            {
                set; get;
            }
        }

    }

}
