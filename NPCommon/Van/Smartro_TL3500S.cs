using FadeFox.Text;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCommon.Van
{
    public delegate void ReceiveTMoneyData(SmartroDTO pDTO);
    /// <summary>
    /// Tmoney 통합버전 VAN 장비
    /// </summary>
    /// <remarks>
    /// Serial 통신을 기본으로 한다.
    /// Byte Ordering은 Little Endian으로 한다.
    /// </remarks>
    /// <history>
    /// 2020-02-11 이재영 Create
    /// </history>
    public partial class Smartro_TL3500S : AbstractSerialPort<bool>
    {
        #region ENUM

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

        #endregion

        #region Const Fields

        public const byte _STX_ = 0x02;
        public const byte _ETX_ = 0x03;
        public const byte _ACK_ = 0x06;         //응답 정상 수신
        public const byte _NACK_ = 0x15;        //응답 재전송 요청 

        #endregion

        #region Member Fields

        ProtocolStep mStep = ProtocolStep.Ready;
        public event ReceiveTMoneyData EventTMoneyData;
        private SmartroDTO testDTO;
        #endregion

        #region Properties

        /// <summary>
        /// 이더넷 통신 IP 를 설정한다.
        /// </summary>
        public string EthernetIP { get => mEthernetIP; set => mEthernetIP = value; }
        /// <summary>
        /// 이더넷 통신 PORT 를 설정한다
        /// </summary>
        public string EthernetPort { get => mEthernetPort; set => mEthernetPort = value; }
        /// <summary>
        /// 가상ID 를 설정한다
        /// </summary>
        public string MID { get => mMID; set => mMID = value; }
        /// <summary>
        /// VAN 서버 IP 를 설정한다
        /// </summary>
        public string VanIP { get => mVanIP; set => mVanIP = value; }
        /// <summary>
        /// VAN 서버 PORT 를 설정한다
        /// </summary>
        public string VanPort { get => mVanPort; set => mVanPort = value; }
        /// <summary>
        /// 단말기 ID 를 설정한다
        /// </summary>
        public string TID { get => mTID; set => mTID = value; }
        /// <summary>
        /// 단말기 IP 방식 "0"[DHCP], "1"[STATIC] 을 설정한다
        /// </summary>
        public string DeviceIPType { get => mDeviceIPType; set => mDeviceIPType = value; }
        /// <summary>
        /// 단말기 IP 를 설정한다
        /// </summary>
        public string DeviceIP { get => mDeviceIP; set => mDeviceIP = value; }
        /// <summary>
        /// 단말기 SUBNET 을 설정한다
        /// </summary>
        public string DeviceSubNet { get => mDeviceSubNet; set => mDeviceSubNet = value; }
        /// <summary>
        /// 단말기 GATEWAY 를 설정한다
        /// </summary>
        public string DeviceGateWay { get => mDeviceGateWay; set => mDeviceGateWay = value; }
        /// <summary>
        /// 장비 통신포트 "0"[COM], "1"[USB], "2"[이더넷] 을 설정한다
        /// </summary>
        public string DeviceType { get => mDeviceType; set => mDeviceType = value; }
        /// <summary>
        /// Test용 읽기전용 DTO
        /// </summary>
        public SmartroDTO TestDTO { get => testDTO; }

        #endregion

        #region Constructor

        public Smartro_TL3500S()
        {
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            SerialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);
        }

        #endregion

        #region Implements AbstractSerialPort

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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "TMoneySmatro_EVCAT | Connect", "연결실패:" + ex.ToString());
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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "TMoneySmatro_EVCAT | Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public void TestEventHandler(SmartroDTO dto)
        {
            testDTO = dto;
        }

        protected override void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(10);
            byte[] buffer = new byte[SerialPort.BytesToRead];
            SerialPort.Read(buffer, 0, SerialPort.BytesToRead);

            //buffer.ToList().Remove(_ACK_); //아래 ACK Byte 제거 코드와 중복되는거 같아 주석 처리.

            var idx = Array.IndexOf(buffer, _ACK_);
            buffer = idx >= 0 ? buffer.Where((val, index) => index != idx).ToArray() : buffer; //ACK Byte 제거
            idx = Array.IndexOf(buffer, _NACK_);
            buffer = idx >= 0 ? buffer.Where((val, index) => index != idx).ToArray() : buffer; //NACK Byte 제거

            TextCore.ACTION(TextCore.ACTIONS.BARCODE, "TMoneySmatro_EVCAT | mSerialPort_DataReceived", "데이터수신:" + buffer);

            int l_stxIndex = 0; // 시작점
            int l_etxIndex = 0; // 종료점
            l_stxIndex = Array.IndexOf(buffer, _STX_);
            l_etxIndex = Array.IndexOf(buffer, _ETX_);
            if (l_etxIndex > 0)
            {
                if (l_stxIndex > l_etxIndex) // 시작이 종료부호보다 더 뒤에 있다면 데이터 클리어
                {
                    buffer.Initialize();
                    return;
                }
            }
            if (l_stxIndex == -1)
            {
                buffer.Initialize();
                return;
            }
            if (l_etxIndex == -1)
            {
                mStep = ProtocolStep.DoCommand;
                return;
            }

            SmartroDTO smartroDTO = SmartroDTO.Parse(buffer);
            if (smartroDTO.IsNull == false)
            {
                SendByte(new byte[] { _ACK_ });
                EventTMoneyData?.Invoke(smartroDTO);
            }

            return;
        }

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //Do nothing...
        }

        #endregion

        private bool SendByte(Byte[] pSendData, int pTImeOut = 1)
        {
            lock (new object())
            {
                try
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TMoneySmatro_EVCAT | SendByte", "[데이터전송]");
                    mStep = ProtocolStep.DoCommand;
                    SerialPort.Write(pSendData, 0, pSendData.Length);
                    DateTime startDate = DateTime.Now;
                    while (mStep != ProtocolStep.Ready)
                    {
                        TimeSpan diff = DateTime.Now - startDate;

                        if (Convert.ToInt32(diff.TotalMilliseconds) > pTImeOut)
                        {
                            mStep = ProtocolStep.Ready;
                            return false;
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoneySmatro_EVCAT | SendByte", "예외사항:" + ex.ToString());
                    mStep = ProtocolStep.Ready;
                    return false;
                }
            }
        }

        #region 요청
        /// <summary>
        /// 단말기 상태체크
        /// </summary>
        public void RequestDeviceCheck()
        {
            bool isSuccess = this.IsConnect;
            if (isSuccess == false) isSuccess = this.Connect();

            if (isSuccess)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TMoneySmatro_EVCAT | RequestDeviceCheck", "티머니 포트연결 성공");
                RequestSendByte("A", null);
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoneySmatro_EVCAT | RequestDeviceCheck", "포트연결 실패");
            }
        }

        public void RequestSettingCheck()
        {
            bool isSuccess = this.IsConnect;
            if (isSuccess == false) isSuccess = this.Connect();

            if (isSuccess)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TMoneySmatro_EVCAT | RequestDeviceCheck", "티머니 포트연결 성공");
                RequestSendByte("J", null);
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoneySmatro_EVCAT | RequestDeviceCheck", "포트연결 실패");
            }
        }

        public void RequestVersionCheck()
        {
            bool isSuccess = this.IsConnect;
            if (isSuccess == false) isSuccess = this.Connect();

            if (isSuccess)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TMoneySmatro_EVCAT | RequestDeviceCheck", "티머니 포트연결 성공");
                RequestSendByte("V", null);
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TMoneySmatro_EVCAT | RequestDeviceCheck", "포트연결 실패");
            }
        }

        /// <summary>
        /// 단말기 리셋
        /// </summary>
        public void RequestDeviceReset()
        {
            RequestSendByte("R", null);
        }
        /// <summary>
        /// 결제 대기
        /// </summary>
        public void RequestApprovalWait()
        {
            RequestSendByte("E", null);
        }
        /// <summary>
        /// 단말기 정보 셋팅
        /// </summary>
        public void RequestInitSetting()
        {
            //바디
            SendSetting body = new SendSetting();
            body.TerminalID = TID;
            body.SAMSlot1 = "\0";
            body.SAMSlot2 = "\0";
            body.SAMSlot3 = "\0";
            body.SAMSlot4 = "\0";
            switch (NPSYS.TmoneyTerminalSamSlot)
            {
                case NPCommon.ConfigID.SAMSLOT.SLOT1:
                    body.SAMSlot1 = "1";
                    break;
                case NPCommon.ConfigID.SAMSLOT.SLOT2:
                    body.SAMSlot2 = "1";
                    break;
                case NPCommon.ConfigID.SAMSLOT.SLOT3:
                    body.SAMSlot3 = "1";
                    break;
                case NPCommon.ConfigID.SAMSLOT.SLOT4:
                    body.SAMSlot4 = "1";
                    break;
            }

            body.DeviceType = DeviceType;
            body.EthernetIP = EthernetIP;
            body.EthernetPort = EthernetPort;
            body.MID = MID;
            body.VanIP = VanIP;
            body.VanPort = VanPort;
            body.DeviceIPType = DeviceIPType;
            body.DeviceIP = DeviceIP;
            body.DeviceSubNet = DeviceSubNet;
            body.DeviceGateWay = DeviceGateWay;

            RequestSendByte("I", body);
        }
        /// <summary>
        /// 단말기 화면&음성 셋팅
        /// </summary>
        public void RequestScreenSetting()
        {
            //바디
            SendReceiveScreenNSoundSetting body = new SendReceiveScreenNSoundSetting();
            body.ScreenBrightness = "9";
            body.VoiceVolume = "0";
            body.TouchVolume = "0";

            RequestSendByte("S", body);
        }
        /// <summary>
        /// 결제 요청
        /// </summary>
        public void RequestApproval(string pPayMoney)
        {
            //바디
            SendApproval body = new SendApproval();
            body.TradCode = "1";
            body.PayMoney = pPayMoney;
            body.Vat = string.Empty;
            body.Service = string.Empty;
            body.InstMonth = string.Empty;
            body.SignYN = "1";
            //송신
            RequestSendByte("B", body);
        }
        /// <summary>
        /// 결제취소 요청
        /// </summary>
        /// <param name="pPayMoney">취소 할 금액</param>
        /// <param name="pAcceptDateTime">yyyyMMddHHmmss</param>
        public void RequestApprovalCancle(string pPayMoney, string pAcceptDateTime)
        {
            //바디
            SendApprovalCancel body = new SendApprovalCancel();
            body.CancleCode = "1";
            body.TradCode = "1";
            body.PayMoney = pPayMoney;
            body.Vat = string.Empty;
            body.Service = string.Empty;
            body.InstMonth = string.Empty;
            body.SignYN = "1";
            body.AcceptNo = string.Empty;
            body.AcceptDate = pAcceptDateTime.SafeSubstring(0, 8);
            body.AcceptTime = pAcceptDateTime.SafeSubstring(8, 6);
            //송신
            RequestSendByte("C", body);
        }

        public void RequestSendByte(string pJobCode, ISerializable pBody)
        {
            List<byte> sendList = new List<byte>();
            Header header;
            Tail tail;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                /*========헤더========*/
                //일단... GetNewDTO 에서 헤더랑...뭐시기 초기화 하는건 빼도 될듯....
                //차라리 Header 로 GetNewDTO를 만들도록 하는게 나을거 같다.
                header = new Header();
                header.STX = _STX_;
                header.TerminalID = NPSYS.TmoneyCatId;
                header.DateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                header.JobCode = pJobCode;
                header.ResponseCode = 0x00;
                if (pBody == null) header.DataLength = 0;
                else header.DataLength = pBody.Size;
                header.Serialize(bw);
                /*========바디========*/
                if (pBody != null)
                {
                    pBody.Serialize(bw);
                }
                /*========테일========*/
                tail = new Tail();
                tail.ETX = _ETX_;
                bw.Flush();
                tail.BCC = tail.MakeBCC(ms.ToArray());
                tail.Serialize(bw);

                bw.Flush();
                SendByte(ms.ToArray());
            }
        }
        #endregion

        #region 응답

        //장치체크 응답전문 처리

        /// <summary>
        /// 장치체크 응답전문을 처리한다.
        /// </summary>
        /// <param name="deviceCheck"></param>
        /// <returns></returns>
        public static string ResponseDeviceCheckHandler(ReceiveDeviceCheck deviceCheck)
        {
            string status = "";

            switch (deviceCheck.CardConnectStat)
            {
                case "N":
                    status = "카드 모듈 통신 상태 미설치";
                    break;
                case "X":
                    status = "카드 모듈 통신 상태 오류";
                    break;
                case "O":
                    status = "장치체크정상";
                    break;
            }

            if (deviceCheck.RFConnectStat == "X")
            {
                status = "Rf 모듈 통신 상태 오류";
            }

            switch (deviceCheck.VANConnectStat)
            {
                case "N":
                    status = "VAN 서버 연결 상태 미설치";
                    break;
                case "X":
                    status = "VAN 서버 연결 디바이스 오류";
                    break;
                case "F":
                    status = "VAN 서버 연결 실패";
                    break;
                case "O":
                    status = "장치체크정상";
                    break;
            }

            return status;
        }


        #endregion
    }
}
