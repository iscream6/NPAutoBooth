using FadeFox.Text;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace NPCommon.DEVICE
{

    public enum BarcodeMotorCmd
    {
        None = 0xFF,
        SetReset = 0x20,
        /// <summary>
        /// 상태체크명령 응답 에러코드에 정상아니면 에러두개만온다
        /// </summary>
        GetStatus = 0x30,
        GetFirmwareVersion = 0x31,
        GetSenSor = 0x32,
        /// <summary>
        /// 배출명령 응답 에러코드에 배출이 성공하면 정상이고 그외 종이가 없다면 종이없음발생 그외 에러에는 그외에러발생
        /// </summary>
        SetEjectFront = 0x33,
        /// <summary>
        /// 배출명령 응답 에러코드에 배출이 성공하면 정상이고 그외 종이가 없다면 종이없음발생 그외 에러에는 그외에러발생
        /// </summary>
        SetEjectRear = 0x34,
        /// <summary>
        /// 리딩명령 응답 에러코드에 배출이 성공하면 정상이고 그외 종이가 없다면 종이없음발생 그외 에러에는 그외에러발생
        /// 자동으로 실행시에는 용지를 넣을때 자동으로 들어가면서 응답값을리턴한다
        /// </summary>
        GetRead = 0x35,
        SetEnabel = 0x36,
        SetDisable = 0x37,
        TImeOut = 0x99

    }
    public enum BarcodeMotorErrorCode
    {
        None = 0x00,
        Ok = 0x30,
        Error = 0x31,
        NoTicketError = 0x32,
        BlackMarkReadError = 0x33,
        TIcketReadError = 0x34,
        TicketJamError = 0x35,
        CommouniCationError = 0x40,
        timeOut = 0x99,

    }

    public enum BarcodeMotorTicketSenSor
    {
        None,
        /// <summary>
        /// 투입구 앞에있을때
        /// </summary>
        TicketIn,
        /// <summary>
        /// 티켓마크감지부
        /// </summary>
        BlckMark,
        /// <summary>
        /// 티켓판독위치부
        /// </summary>
        TIcketRead,
        /// <summary>
        /// 투입후 바코드를 읽은상태일때 
        /// </summary>
        TicketEject
    }



    //GS POS할인
    /// <summary>
    /// LED , 영수증 버튼 관련 제어
    /// </summary>
    public class BarcodeMoter : AbstractSerialPort<bool>
    {
        ProtocolStep mStep = ProtocolStep.Ready;
        private int mTimeOut = 10000; //2초
        private byte[] mReceiveData = null;
        /// <summary>
        /// AUTO일때만 이벤트로 데이터를 전송하고 전송에 대한 응답에 대한 싱크(SendCommand함수에 대한 전송과 응답기준)
        /// </summary>
        public static bool mModulAutoMode = false;

        // 상태체크 응답 0x02,0x22,0x20,0x30,0x32,0x03,0xFF
        // 펌웨워버젼 0x02,0x2A,0x31,0x30,0x56,0x65,0x72,0x33,0x2E,0x32,0x30,0x41,0x34,0x03
        #region 속성

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override bool Connect()
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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "BarcodeMoter | Connect", "연결실패:" + ex.ToString());
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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "BarcodeMoter | Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(10);
                int length = SerialPort.BytesToRead;


                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)SerialPort.ReadByte());
                }

                byte[] res = mReadBuffer.ToArray();

                string data = "";
                for (int i = 0; i < res.Length; i++)
                {
                    data += i.ToString() + ":" + res[i].ToString("X2") + " ";
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BarcodeMoter | mSerialPort_DataReceived", "데이터수신:" + data.ToString());

                if (mReadBuffer.Contains(startParameter))
                {
                    if (!mReadBuffer.Contains(endParameter))
                    {
                        return;
                    }
                    else
                    {

                        int stxIndex = mReadBuffer.IndexOf(startParameter);
                        int lenIndex = stxIndex + 1;
                        int cmdIndex = stxIndex + 2;
                        int errorIndex = stxIndex + 3;
                        int recvLenLength = 0;
                        int etxIndex = mReadBuffer.IndexOf(endParameter);
                        if (mReadBuffer.Count >= stxIndex + 2)   // 들어온 데이터가 length까지 왔다면
                        {
                            recvLenLength = (int)mReadBuffer[lenIndex] - 0x20;
                        }
                        mReceiveData = mReadBuffer.GetRange(cmdIndex, recvLenLength).ToArray();
                        mReadBuffer.Clear();

                        if (mReceiveData[0] == (int)BarcodeMotorCmd.GetRead)
                        {
                            if (mModulAutoMode)
                            {

                                BarcodeMotorResult realBarcodeMotorResult = new BarcodeMotorResult(mReceiveData);
                                if (realBarcodeMotorResult.ResultCmd == BarcodeMotorCmd.GetRead)
                                {
                                    EventAutoRedingData(realBarcodeMotorResult);
                                }
                                mStep = ProtocolStep.Ready;
                            }
                            else
                            {
                                mStep = ProtocolStep.ReceivSuccess;
                            }
                        }
                        else
                        {
                            mStep = ProtocolStep.ReceivSuccess;
                        }

                    }
                    return;
                }
                else
                {
                    mReadBuffer.Clear();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BarcodeMoter | mSerialPort_DataReceived", "수신된데이터가 STX가없어서 버퍼삭제:");
                    mStep = ProtocolStep.Ready;
                    return;
                }
            }

            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "DoSensor|mSerialPort_DataReceived", "자료수신중 예외상황:" + ex.ToString());
            }
        }

        #endregion

        #region Event
        public delegate void GetAutoRedingData(BarcodeMotorResult pBarcodeResult);
        public event GetAutoRedingData EventAutoRedingData;
        #endregion

        public BarcodeMoter()
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
            ReceivSuccess
        }

        /// <summary>
        /// // inhibiy ba,if hopper problems occured
        /// </summary>
        public const byte startParameter = 0x02;
        public const byte endParameter = 0x03;

        public List<byte> mReadBuffer = new List<byte>();


        public BarcodeMotorResult SendCommand(byte[] p_Parameter)
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }

                mReadBuffer.Clear();
                DateTime startDate = DateTime.Now;
                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SendCommand", "응답하기전 응답시간초과");
                        mStep = ProtocolStep.Ready;
                    }
                }
                mStep = ProtocolStep.DoCommand;
                string sendData = string.Empty;
                for (int i = 0; i < p_Parameter.Length; i++)
                {
                    sendData += i.ToString() + ":" + p_Parameter[i].ToString("X2") + " ";
                }
                string cmdName = string.Empty;
                BarcodeMotorCmd currentCmd = (BarcodeMotorCmd)(int)p_Parameter[2];

                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BarcodeMoter | SendCommand", "데이터SERIAL에전달:" + " [" + currentCmd.ToString() + "]" + " [전송데이터]" + sendData.ToString());

                SerialPort.Write(p_Parameter, 0, p_Parameter.Length);

                startDate = DateTime.Now;

                while (mStep != ProtocolStep.ReceivSuccess)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SendCommand", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                        return barcodeMotorResult;
                    }
                }
                mStep = ProtocolStep.Ready;
                BarcodeMotorResult realBarcodeMotorResult = new BarcodeMotorResult(mReceiveData);
                //if (realBarcodeMotorResult.ResultCmd == BarcodeMotorCmd.GetRead)
                //{
                //    EventAutoRedingData(realBarcodeMotorResult);
                //}
                return realBarcodeMotorResult;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SendCommand", "예외사항:" + ex.ToString());

                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        public BarcodeMotorResult GetStatus()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.GetStatus, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | GetStatus", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        /// <summary>
        /// 용지가 앞에있다면 모터가 돌아가면서 가져가서 읽는다
        /// 용지가 없다면 에러코드로 용지없음 표출하고 용지가 들어가 있는 상태에서 읽기를 실행하면 용지젬 응답이온다
        /// </summary>
        /// <returns></returns>
        public BarcodeMotorResult GetRead()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.GetRead, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | GetRead", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        public BarcodeMotorResult EjectFront()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.SetEjectFront, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | GetRead", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }


        public BarcodeMotorResult EjectRear()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.SetEjectRear, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | GetRead", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        /// <summary>
        /// 명령전송시 이후에 앞으로 용지를 넣으면 용지가 들어간다
        /// 명령전송시에 용지를 넣고 있으면 바로 용지를 읽으며 응답커맨드가 SetEnabel이 아닌 GetRead로온다
        /// </summary>
        /// <returns></returns>
        public BarcodeMotorResult SetEnable()
        {
            try
            {
                mModulAutoMode = true;
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.SetEnabel, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SetEnable", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }
        /// <summary>
        /// 명령전송시 이후에 앞으로 용지를 넣으면 용지가 들어가지 않는다
        /// </summary>
        /// <returns></returns>
        public BarcodeMotorResult SetDIsable()
        {
            try
            {
                mModulAutoMode = false;
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.SetDisable, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SetDIsable", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        /// <summary>
        /// 사용시 용지를 넣는 투입구 부분에 있으면 TicketIn, 바코드를 읽기 완료되면 티켓배출구인 TicketEject부분에 감지된다 티켓마크감지부와 티켓판독위치부는 상황발생을 못해봄
        /// </summary>
        /// <returns></returns>
        public BarcodeMotorResult GetSensor()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.GetSenSor, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SetDIsable", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }
        }

        public BarcodeMotorResult SetReset()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.SetReset, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | SetReset", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }

        }

        public BarcodeMotorResult GetFirmwareVersion()
        {
            try
            {
                byte[] cmdData = new byte[] { 0x02, 0x21, (byte)BarcodeMotorCmd.GetFirmwareVersion, 0xFF, 0x03 };
                return SendCommand(cmdData);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "BarcodeMoter | GetFirmwareVersion", ex.ToString());
                BarcodeMotorResult barcodeMotorResult = new BarcodeMotorResult(new byte[] { (int)BarcodeMotorCmd.TImeOut, (int)BarcodeMotorErrorCode.timeOut });
                return barcodeMotorResult;
            }

        }

        public class BarcodeMotorResult
        {
            public BarcodeMotorCmd ResultCmd { get; set; }
            public BarcodeMotorErrorCode ResultStatus { get; set; }
            public BarcodeMotorTicketSenSor ResultSensor { get; set; }
            public string Message { get; set; }  // 상태 혹은 에러메시지가 포함될 수 있음.
            public string Data { get; set; }
            public byte ResultCode { get; set; } // 결과 코드
            public byte[] DataByte = null;
            private byte mCr = (byte)0x0D;
            private byte[] mRawData = null;
            public byte[] RawData
            {
                get { return mRawData; }
            }

            public BarcodeMotorResult()
            {
            }
            private List<byte> mListRecvData = new List<byte>();
            public BarcodeMotorResult(byte[] pRawData)
            {
                this.mRawData = pRawData;
                ResultStatus = BarcodeMotorErrorCode.None;
                ResultCmd = BarcodeMotorCmd.None;
                Message = string.Empty;
                ResultSensor = BarcodeMotorTicketSenSor.None;
                foreach (byte item in pRawData)
                {
                    mListRecvData.Add((byte)item);
                }
                ResultCmd = (BarcodeMotorCmd)mListRecvData[0];
                ResultStatus = (BarcodeMotorErrorCode)mListRecvData[1];
                ResultCode = (byte)ResultStatus;
                if (ResultCmd == BarcodeMotorCmd.GetRead) // 읽기 명령 실행시
                {
                    if (ResultStatus == BarcodeMotorErrorCode.Ok)
                    {
                        if (mListRecvData.Contains(mCr)) // carriageReturng이 있다면
                        {
                            int crIndex = mListRecvData.IndexOf(mCr);
                            mListRecvData.Remove((byte)0x1E);
                            mListRecvData.Remove((byte)0x1E);
                            mListRecvData.Remove((byte)0x1E);
                            Data = Encoding.Default.GetString(mListRecvData.GetRange(2, crIndex - 2).ToArray());
                            Data = Data.Replace("\r", "");
                            Data = Data.Replace("?", "");
                        }
                        else
                        {
                            Data = string.Empty;
                        }
                        //mListRecvData.Remove((byte)0x1E);
                        //Data = Encoding.Default.GetString(mListRecvData.GetRange(2, mListRecvData.Count - 2).ToArray());
                    }
                }
                else if (ResultCmd == BarcodeMotorCmd.GetFirmwareVersion)
                {
                    if (ResultStatus == BarcodeMotorErrorCode.Ok)
                    {

                        Data = Encoding.Default.GetString(mListRecvData.GetRange(2, mListRecvData.Count - 2).ToArray());
                    }
                }
                else if (ResultCmd == BarcodeMotorCmd.GetSenSor)
                {
                    if (ResultStatus == BarcodeMotorErrorCode.Ok || ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                    {
                        Data = Encoding.Default.GetString(mListRecvData.GetRange(2, mListRecvData.Count - 2).ToArray());
                        int noneCheck = Convert.ToInt32(Data);
                        if (noneCheck > 0)
                        {
                            string ticketIn = Data.Substring(0, 1);
                            string blackMark = Data.Substring(1, 1);
                            string TicketRead = Data.Substring(2, 1);
                            string TicketEject = Data.Substring(3, 1);
                            if (ticketIn == "1")
                            {
                                ResultSensor = BarcodeMotorTicketSenSor.TicketIn;
                            }
                            else if (blackMark == "1")
                            {
                                ResultSensor = BarcodeMotorTicketSenSor.BlckMark;
                            }
                            else if (TicketRead == "1")
                            {
                                ResultSensor = BarcodeMotorTicketSenSor.TIcketRead;
                            }
                            else if (TicketEject == "1")
                            {
                                ResultSensor = BarcodeMotorTicketSenSor.TicketEject;
                            }

                        }
                        else
                        {
                            ResultSensor = BarcodeMotorTicketSenSor.None;
                        }
                    }

                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "BarcodeMotorResult | BarcodeMotorResult", "[바코드모터리더기 응답데이터 파싱]"
                                                                                                        + " [명령어]" + ResultCmd.ToString()
                                                                                                        + " [상태]" + ResultStatus.ToString()
                                                                                                        + " [메세지]" + Message
                                                                                                        + " [데이터]" + Data
                                                                                                        + " [센서]" + ResultSensor.ToString()
                                                                                                        + " [코드]" + ResultCode.ToString("X2"));



            }

        }
    }
}
