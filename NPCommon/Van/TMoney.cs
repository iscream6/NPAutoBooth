using FadeFox.Text;
using System;
using System.Collections.Generic;
using System.IO.Ports;


namespace NPCommon.Van
{
    public enum TMoneyResultType
    {
        NoneStatus,     // 아무것도 아님.
        OkStatus,   // 정상
        NoDetectCard,
        Error
    }


    public class TMoney
    {
        private SerialPort mSerialPort = new SerialPort();
        ProtocolStep mStep = ProtocolStep.Ready;


        #region 속성

        public bool IsConnect
        {
            get { return mSerialPort.IsOpen; }
        }

        private string mPortNameString = "";
        public string PortNameString
        {
            get { return mPortNameString; }
            set
            {
                mPortNameString = value;

                if (mPortNameString != "")
                    mSerialPort.PortName = mPortNameString;
            }
        }

        public string PortName
        {
            get { return mSerialPort.PortName; }
            set
            {
                mSerialPort.PortName = value;
                mPortNameString = value;
            }
        }

        private string mBaudRateString = "";
        public string BaudRateString
        {
            get { return mBaudRateString; }
            set
            {
                mBaudRateString = value;
                mSerialPort.BaudRate = Convert.ToInt32(mBaudRateString);
            }
        }

        public int BoudRate
        {
            get { return mSerialPort.BaudRate; }
            set
            {
                mSerialPort.BaudRate = value;
                mBaudRateString = value.ToString();
            }
        }

        private string mParityString = "";
        public string ParityString
        {
            get { return mParityString; }
            set
            {
                mParityString = value;

                switch (mParityString.ToUpper())
                {
                    case "NONE":
                        mSerialPort.Parity = Parity.None;
                        break;

                    case "EVEN":
                        mSerialPort.Parity = Parity.Even;
                        break;

                    case "ODD":
                        mSerialPort.Parity = Parity.Odd;
                        break;

                    default:
                        mSerialPort.Parity = Parity.None;
                        break;
                }
            }
        }

        public Parity Parity
        {
            get { return mSerialPort.Parity; }
            set
            {
                mSerialPort.Parity = value;
                mParityString = value.ToString();
            }
        }

        private string mDatabitsString = "";
        public string DatabitsString
        {
            get { return mDatabitsString; }
            set
            {
                mDatabitsString = value;
                if (mDatabitsString.Trim() == "")
                {
                    mDatabitsString = "8";
                }
                mSerialPort.DataBits = Convert.ToInt32(mDatabitsString);
            }
        }

        public int DataBits
        {
            get { return mSerialPort.DataBits; }
            set
            {
                mSerialPort.DataBits = value;
                mDatabitsString = value.ToString();
            }
        }

        private string mStopbitsString = "";
        public string StopbitsString
        {
            get { return mStopbitsString; }
            set
            {
                mStopbitsString = value;

                switch (mStopbitsString)
                {
                    case "1":
                        mSerialPort.StopBits = StopBits.One;
                        break;

                    case "2":
                        mSerialPort.StopBits = StopBits.Two;
                        break;

                    default:
                        mSerialPort.StopBits = StopBits.One;
                        break;
                }
            }
        }

        public StopBits StopBits
        {
            get { return mSerialPort.StopBits; }
            set
            {
                mSerialPort.StopBits = value;
                mStopbitsString = value.ToString();
            }
        }

        private string mHandshakeString = "";
        public string HandshakeString
        {
            get { return mHandshakeString; }
            set
            {
                mHandshakeString = value;

                switch (mHandshakeString.ToUpper())
                {
                    case "NONE":
                        mSerialPort.Handshake = Handshake.None;
                        break;

                    case "RTS":
                        mSerialPort.Handshake = Handshake.RequestToSend;
                        break;

                    case "XON/XOFF":
                        mSerialPort.Handshake = Handshake.XOnXOff;
                        break;

                    case "XONXOFF":
                        mSerialPort.Handshake = Handshake.XOnXOff;
                        break;

                    case "BOTH":
                        mSerialPort.Handshake = Handshake.RequestToSendXOnXOff;
                        break;
                }
            }
        }

        public Handshake Handshake
        {
            get { return mSerialPort.Handshake; }
            set
            {
                mSerialPort.Handshake = value;
                mHandshakeString = value.ToString();
            }
        }

        public void Initialize()
        {
            mSerialPort.DiscardInBuffer();
            mSerialPort.DiscardOutBuffer();
        }

        public Boolean Connect()
        {

            if (mSerialPort.IsOpen)
                mSerialPort.Close();

            mSerialPort.ReadTimeout = 1000;
            mSerialPort.WriteTimeout = 1000;
            mSerialPort.DtrEnable = true;
            mSerialPort.RtsEnable = true;
            mSerialPort.Parity = System.IO.Ports.Parity.None;
            mSerialPort.DataBits = 8;
            mSerialPort.StopBits = System.IO.Ports.StopBits.One;
            mSerialPort.Handshake = System.IO.Ports.Handshake.None;

            try
            {
                mSerialPort.Open();
                Initialize();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            mSerialPort.Close();
        }

        #endregion



        public TMoney()
        {

            mSerialPort.DataReceived += new SerialDataReceivedEventHandler(mSerialPort_DataReceived);
            mSerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(mSerialPort_ErrorReceived);
        }



        private byte[] mReceiveData = null;

        private enum ProtocolStep
        {
            Ready,
            None,
            SendCommand,
            ReceiveData,
        }

        public const byte OkStatus = 0x00;
        public const byte NoCardDetect = 0x30;
        public const byte _STX_ = 0x02;
        public const byte _ETX_ = 0x03;
        private const byte m_CMD_CardRead = 0x20;
        private const byte m_CMD_TerminalSave = 0x30;
        private const byte m_CMD_Payment = 0x54;
        private const byte m_CMD_TerminalInfo = 0x00;

        private void mSerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

        List<byte> mReadBuffer = new List<byte>();

        List<String> aaa = new List<string>();

        public delegate void Billinsert(object sender, string insertMessage);


        private void mSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int length = mSerialPort.BytesToRead;
                mStep = ProtocolStep.ReceiveData;

                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)mSerialPort.ReadByte());
                }

                if (mReadBuffer.Contains(_ETX_))
                {
                    if (mReadBuffer.Count < 8) // 최소 8자리이상임
                    {
                        return;
                    }
                    if (!mReadBuffer.Contains(_STX_)) // 시작 데이터가 없으면 버린다.
                    {
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return;
                    }
                    int l_DataStart = -1;


                    byte[] res = mReadBuffer.ToArray();

                    for (int i = 0; i < res.Length; i++)
                    {
                        if (l_DataStart < 0)
                        {
                            if (res[i] == _STX_)
                            {
                                l_DataStart = i;
                            }
                        }
                    }


                    int l_Data_length = Convert.ToInt32(res[l_DataStart + 5]) + 8;  // 들어온 데이터의 자리수인 데이터값 길이에 나머지 커맨드를 합친값(정상적인 데이터길이산정)

                    if (res.Length != l_Data_length)
                    {

                        return;
                    }


                    if (res.Length > 8)
                    {
                        if (res[6] != NoCardDetect)
                        {
                            string data = "";
                            for (int i = 0; i < res.Length; i++)
                            {
                                data += i.ToString() + ":" + res[i].ToString("X") + " ";
                            }
                            TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TMoney|mSerialPort_DataReceived", "데이터수신:" + data);
                        }
                    }


                    mReceiveData = res;

                    mStep = ProtocolStep.Ready;
                }

            }

            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoney|mSerialPort_DataReceived", ex.ToString());
            }
        }

        private int mTimeOut = 10000;
        private byte[] mTimeOutData = { 0x99 };
        private byte[] mOkData = { 0x98 };

        public byte[] TMoneyDongleInfomation()
        {
            try
            {
                if (!mSerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}

                mReadBuffer.Clear();
                byte[] l_SendCommand = new byte[8];
                l_SendCommand[0] = _STX_; // stx
                l_SendCommand[1] = 0x00;  // cmd
                l_SendCommand[2] = 0x01;  // p1
                l_SendCommand[3] = 0x00;  // p2
                l_SendCommand[4] = 0x00;  // len0
                l_SendCommand[5] = 0x00;  // len1
                l_SendCommand[6] = _ETX_; // ETX
                l_SendCommand[7] = (byte)(l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5] ^ l_SendCommand[6]);
                TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TMoney|TMoneyDongleInfomation", "명령어받음:상태값 확인");
                mStep = ProtocolStep.SendCommand;
                mSerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TMoney|TMoneyDongleInfomation", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }

                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoney|TMoneyDongleInfomation", ex.ToString());
                return mTimeOutData;
            }
        }

        public byte[] TMoneyReading()
        {
            try
            {
                if (!mSerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}

                mReadBuffer.Clear();
                byte[] l_SendCommand = new byte[9];
                l_SendCommand[0] = _STX_; // stx
                l_SendCommand[1] = 0x20;  // cmd
                l_SendCommand[2] = 0x03;  // p1
                l_SendCommand[3] = 0x00;  // p2
                l_SendCommand[4] = 0x00;  // len0
                l_SendCommand[5] = 0x01;  // len1
                l_SendCommand[6] = 0x05;  // Data
                l_SendCommand[7] = _ETX_; // ETX
                l_SendCommand[8] = (byte)(l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5] ^ l_SendCommand[6] ^ l_SendCommand[7]);
                TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TMoney|TMoneyReading", "카드리딩");
                mStep = ProtocolStep.SendCommand;
                mSerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TMoney|TMoneyReading", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }
                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoney|TMoneyReading", ex.ToString());
                return mTimeOutData;
            }
        }


        public byte[] TMoneyPayment(int payTime, int p_pay)
        {
            try
            {
                if (!mSerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}

                mReadBuffer.Clear();
                byte[] l_SendCommand = new byte[13];
                l_SendCommand[0] = _STX_; // stx
                l_SendCommand[1] = 0x54;  // cmd
                l_SendCommand[2] = 0x02;  // p1
                l_SendCommand[3] = 0x00;  // p2
                l_SendCommand[4] = 0x00;  // len0
                l_SendCommand[5] = 0x05;  // len1
                l_SendCommand[6] = Convert.ToByte(payTime);  // Data
                byte[] l_Paymnet = TextCore.Dec2Hexa(p_pay);

                for (int i = 0; i < l_Paymnet.Length; i++)
                {
                    l_SendCommand[7 + i] = l_Paymnet[i];
                }

                l_SendCommand[11] = _ETX_; // ETX
                l_SendCommand[12] = (byte)(l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5] ^ l_SendCommand[6] ^ l_SendCommand[7] ^ l_SendCommand[8] ^ l_SendCommand[9] ^ l_SendCommand[10] ^ l_SendCommand[11]);
                TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TMoney|TMoneyPayment", "카드결제");
                mStep = ProtocolStep.SendCommand;
                mSerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TMoney|TMoneyPayment", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }
                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoney|TMoneyPayment", ex.ToString());
                return mTimeOutData;
            }
        }
        public byte[] TMoneyDongleNumberSave(int p_SerialNumber)
        {
            try
            {
                if (!mSerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}

                mReadBuffer.Clear();
                byte[] l_SendCommand = new byte[13];
                l_SendCommand[0] = _STX_; // stx
                l_SendCommand[1] = 0x30;  // cmd
                l_SendCommand[2] = 0x02;  // p1
                l_SendCommand[3] = 0x00;  // p2
                l_SendCommand[4] = 0x00;  // len0
                l_SendCommand[5] = 0x05;  // len1
                byte[] l_terminalNumber = TextCore.ToBCD(p_SerialNumber, 5);
                for (int i = 0; i < l_terminalNumber.Length; i++)
                {
                    l_SendCommand[6 + i] = l_terminalNumber[i];
                }
                //l_SendCommand[6] = 0x00;  // Data
                //l_SendCommand[7] = 0x08;  // Data
                //l_SendCommand[8] = 0x70;  // Data
                //l_SendCommand[9] = 0x00;  // Data
                //l_SendCommand[10] = 0x50;  // Data
                l_SendCommand[11] = _ETX_; // ETX   //0 8 112 0 80
                l_SendCommand[12] = (byte)(l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5] ^ l_SendCommand[6] ^ l_SendCommand[7] ^ l_SendCommand[8] ^ l_SendCommand[9] ^ l_SendCommand[10] ^ l_SendCommand[11]);
                TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TMoney|TMoneyDongleNumberSave", "단말기저장");
                mStep = ProtocolStep.SendCommand;
                mSerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TMoney|TMoneyDongleNumberSave", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }
                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoney|TMoneyDongleNumberSave", ex.ToString());
                return mTimeOutData;
            }
        }

        public static string TMoneyErrorMessage(byte p_errorMessage)
        {
            switch (p_errorMessage)
            {
                case 0x01:
                    return "ETX receive error from Host";
                case 0x02:
                    return "BCC check error from Host";
                case 0x22:
                    return "Undefined Command";
                case 0x23:
                    return "Flash Memory Boot error(Rebooting 수행)";

                case 0x67:
                    return "환승 정보 읽기 error";

                case 0x68:
                    return "카드 잔액 부족";

                case 0x69:
                    return "지원하지 않는 카드";

                case 0x82:
                    return "정보 읽기/쓰기 도중 카드 바뀜";

                case 0x83:
                    return "거래 내역 없음";

                case 0x84:
                    return "거래 내역 data 오류";

                case 0x85:
                    return "유통영역 인증자 오류";

                case 0x86:
                    return "발행사ID 인증 실패";

                case 0x87:
                    return "발행사ID 등록할 공간 부족";

                case 0x88:
                    return "발행사ID 등록/삭제 error";

                case 0x91:
                    return "내부 Memory 읽기 오류";

                case 0x92:
                    return "내부 Memory 쓰기 오류";

                case 0x93:
                    return "SAM 장비 오류";

                case 0x30:
                    return "Card detect error";

                case 0x31:
                    return "Select DF error";

                case 0x33:
                    return "이전 거래 정상종료 상태임";

                case 0x37:
                    return "재거래 요망 (카드 지불은 완료, 명령 재수행 필요)";

                case 0x40:
                    return "이전거래 미실행 (재거래 불가)";

                case 0x47:
                    return "잔액 부족";

                case 0x50:
                    return "Update memory error";

                case 0x51:
                    return "한도금액 초과";

                case 0x52:
                    return "단말기 Serial 미설정";

                case 0x53:
                    return "카드타입 체크오류";

                case 0x54:
                    return "카드상태 체크오류";

                case 0x55:
                    return "RID 체크오류";

                case 0x56:
                    return "교통카드종류 체크오류";

                case 0x57:
                    return "발급자 인증오류";


                case 0x58:
                    return "Value Block Broken오류";

                case 0x59:
                    return "RFTSAM CSN Read오류";

                case 0x5A:
                    return "교통카드 상태표시자 오류";

                case 0x5B:
                    return "거래기록 인증자생성 오류";

                case 0x5C:
                    return "지불 재거래 횟수 초과오류";

                case 0x5D:
                    return "Get Working Key 오류";

                case 0x5E:
                    return "거래전 금액쓰기 오류";

                case 0x5F:
                    return "교통지갑 복구불능";

                case 0x70:
                    return "Read Block(Block2) Error";

                case 0x71:
                    return "Read Block(Block4) Error";

                case 0x72:
                    return "Read Block(Block5) Error";

                case 0x73:
                    return "Read Block(Block6) Error";

                case 0x74:
                    return "Read Block(Block8) Error";

                case 0x75:
                    return "Read Block(Block14) Error";

                case 0x76:
                    return "Value Block Recovery 1st Block Read Error";

                case 0x77:
                    return "Value Block Recovery 2nd Block Read Error";

                case 0x78:
                    return "Value Block Recovery 2nd -> 1st Block Error";

                case 0x79:
                    return "Value Block Recovery 1st -> 2nd Block Error";

                case 0x7A:
                    return "거래카운터 감소 오류";

                case 0x7B:
                    return "거래카운터 백업 오류";

                case 0x7C:
                    return "잔액과 백업잔액 불일치";

                case 0x7D:
                    return "잔액 감소중 오류";

                case 0x7E:
                    return "잔액 백업중 오류";

                case 0x7F:
                    return "거래후 잔액읽기 오류";

                case 0xA0:
                    return "Openkey – 후불SAM 미인증상태";

                case 0xAA:
                    return "Mifare Card Read Block 오류";


                case 0xAB:
                    return "Get Sector Key 오류";

                case 0xAC:
                    return "Get Response 오류";
                case 0x99:
                    return "TimeOut 오류";

                default:
                    return "Unkown Errpr";

            }
        }

    }

    public class TMoneyResult
    {
        public TMoneyResultType ResultType { get; set; }
        public string Message { get; set; }  // 상태 혹은 에러메시지가 포함될 수 있음.
        public string Data { get; set; }
        public byte ResultCode { get; set; } // 결과 코드
        public byte[] ResultData { get; set; } // 응답전문모두
        private byte[] mRawData = null;
        public byte[] RawData
        {
            get { return mRawData; }
        }


        public TMoneyResult(byte[] pRawData)
        {
            this.mRawData = pRawData;

            if (mRawData.Length == 1)
            {
                ResultType = TMoneyResultType.Error;
                Message = "장비의 결과 에러";
                ResultCode = mRawData[0];
                ResultData = pRawData;
                return;

            }

            if (mRawData[6] == TMoney.OkStatus)
            {
                ResultType = TMoneyResultType.OkStatus;
                Message = "장비의 결과 정상";
                ResultCode = mRawData[6];
                ResultData = pRawData;

            }
            else if (mRawData[6] == TMoney.NoCardDetect)
            {
                ResultType = TMoneyResultType.NoDetectCard;
                Message = "장비의 결과 정상";
                ResultCode = mRawData[6];
                ResultData = pRawData;

            }
            else
            {
                ResultType = TMoneyResultType.Error;
                ResultCode = mRawData[6];
                Message = TMoney.TMoneyErrorMessage(mRawData[6]);
            }
        }

    }



}
