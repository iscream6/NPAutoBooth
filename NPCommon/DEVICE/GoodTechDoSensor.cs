using FadeFox.Text;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace NPCommon.DEVICE
{
    //GS POS할인
    /// <summary>
    /// LED , 영수증 버튼 관련 제어
    /// </summary>
    public class GoodTechContorlBoard : AbstractSerialPort<bool>
    {
        ProtocolStep mStep = ProtocolStep.Ready;

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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | Connect", "연결실패:" + ex.ToString());
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
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | Disconnect", "연결종료실패:" + ex.ToString());
            }
        }

        #endregion



        public GoodTechContorlBoard()
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

        /// <summary>
        /// // inhibiy ba,if hopper problems occured
        /// </summary>
        public const byte startParameter = 0xbc;

        /// <summary>
        /// 보드상태 요청
        /// </summary>
        public const byte DoCommandRequestBoradStatusParameter = 0x01;
        /// <summary>
        /// LED출력요청
        /// </summary>
        public const byte DoCommandRequestConstrainOnParameter = 0x02;
        /// <summary>
        /// 버젼요청
        /// </summary>
        public const byte DoCommandVersionParameter = 0x03;
        /// <summary>
        /// 리셋 활성 비활성 상태확인
        /// </summary>
        public const byte DoCommandUseResetSeting = 0x04;
        /// <summary>
        /// UsbresetTime설정
        /// </summary>
        public const byte DoCommandSettingUsbResetTime = 0x0a;
        /// <summary>
        /// // Enable ba,if hopper problems occured
        /// </summary>
        public const byte DoLength = 0x07;
        /// <summary>
        /// 어떤 동작이 일어나면(예: 영수증 버튼,도어열기 등) 자동으로 이벤트로 오는 정보의 커맨드
        /// </summary>
        public const byte DoReplayCommandActionEvent = 0x81;

        /// <summary>
        /// LED출력요청에 대한 응답
        /// </summary>
        public const byte DoReplyCommandConstarinOn = 0x82;
        /// <summary>
        /// 정산기Version체크 응답
        /// </summary>
        public const byte DoReplyCommandVersion = 0x83;

        /// <summary>
        /// USB리셋설정에 대한 응답값
        /// </summary>
        public const byte DoReplyCommandUseResetSeting = 0x84;

        /// <summary>
        /// DoCommandRequestBoradStatusParameter 에 대한 보드상태값
        /// </summary>
        public const byte DoReplayCommandBoardStatus = 0x91;
        /// <summary>
        /// usb Reset Time설정에 대한 응답
        /// </summary>
        public const byte DoReplayCommandUsbResetTime = 0x8a;

        /// <summary>
        /// 동전방출에서 기본적인 동작에 대한 정상 응답값 
        /// </summary>
        public const byte g_OKSTATUS = 0x12;

        private byte[] mTimeOutData = { 0x99 };
        private byte[] mOkData = { 0x98 };

        protected override void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            mStep = ProtocolStep.Ready;
        }

        public List<byte> mReadBuffer = new List<byte>();



        public enum SignalType
        {
            ReciptSignal
        }
        public delegate void SignalEvent(object sender, SignalType m_Signal);
        public event SignalEvent DosensorSignalEvent;
        //public delegate void BoradAliveEvnet(object sender, string m_Board);
        //public event BoradAliveEvnet DoAliveEvent;
        public static string m_SignalData = "";

        public string m_ResetStatus = string.Empty;


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
                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "데이터수신:" + data.ToString());

                if (mReadBuffer.Contains(startParameter))
                {
                    if (mReadBuffer.Contains(DoReplyCommandConstarinOn))
                    {
                        //getWriteLogData("센서불: " + data); //test                        
                        mReadBuffer.Clear();
                        return;
                    }
                    if (mReadBuffer.Contains(DoReplyCommandVersion) && res.Length >= 5)
                    {

                        mReadBuffer.Clear();
                        return;
                    }
                    if (mReadBuffer.Contains(DoReplyCommandUseResetSeting) && res.Length >= 5)
                    {
                        int lResetStatus = Convert.ToInt32(res[3]);
                        string lResetString = string.Empty;
                        switch (lResetStatus)
                        {
                            case 0:
                                lResetString = "비활성 상태";
                                break;
                            case 1:
                                lResetString = "활성화 상태";
                                break;

                        }
                        m_ResetStatus = string.Empty;
                        m_ResetStatus = lResetString;
                        TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "리셋기능" + lResetString.ToString());
                        mReadBuffer.Clear();
                        return;
                    }
                    if (mReadBuffer.Contains(DoReplayCommandBoardStatus) && res.Length >= 5)
                    {
                        mReadBuffer.Clear();
                        return;
                    }
                    if (mReadBuffer.Contains(DoReplayCommandActionEvent) && res.Length >= 11)
                    {
                        if (mReadBuffer.IndexOf(DoReplayCommandActionEvent) == 2)
                        {
                            //Door신호관련 
                            //m_SignalData = "";
                            //string signal1 = TextCore.lPad(Convert.ToString(res[3], 2), 8, "0");
                            //string signal2 = TextCore.lPad(Convert.ToString(res[4], 2), 8, "0");
                            //TextCore.ACTION(TextCore.ACTIONS.DIDO, "DoSensor | mSerialPort_DataReceived", "시그널1: " + signal1 + " 시그널2: " + signal2);
                            //if (NPSYS.gReceiptSignalNumber >= 9)
                            //{
                            //    if (Convert.ToInt32(signal2) != 0)
                            //    {
                            //        TextCore.ACTION(TextCore.ACTIONS.DIDO, "DoSensor | mSerialPort_DataReceived", "영수증도어신호누름");
                            //        // m_IsReceiptSignal = true;
                            //        if (this != null)
                            //        {
                            //            DosensorSignalEvent(this, SignalType.ReciptSignal);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    int currentSignalNumber = NPSYS.gReceiptSignalNumber - 1;
                            //    if (signal1.Substring(currentSignalNumber, 1) == "1")
                            //    {
                            //        TextCore.ACTION(TextCore.ACTIONS.DIDO, "DoSensor | mSerialPort_DataReceived", "영수증도어신호누름");
                            //        // m_IsReceiptSignal = true;
                            //        if (this != null)
                            //        {
                            //            DosensorSignalEvent(this, SignalType.ReciptSignal);
                            //        }
                            //    }

                            //}
                            //. 현금카드용 정산기
                            //  1) DOOR OPEN –> DI - 0
                            //  2) 영수증 프린트 버튼 –> DI - 1


                            //2) 신용카드전용 정산기
                            //  1) DOOR OPEN –> DI - 0
                            //  2) 영수증 프린트 버튼 –> DI - 4
                            // 현금신용무인 00000000 시그널2: 00000010
                            // 신용무인 0001

                            m_SignalData = "";
                            string signal1 = TextCore.lPad(Convert.ToString(res[3], 2), 8, "0");
                            string signal2 = TextCore.lPad(Convert.ToString(res[4], 2), 8, "0");
                            int[] diPort = new int[11];
                            diPort[0] = Convert.ToInt32(signal1.Substring(7, 1));
                            diPort[1] = Convert.ToInt32(signal1.Substring(6, 1));
                            diPort[2] = Convert.ToInt32(signal1.Substring(5, 1));
                            diPort[3] = Convert.ToInt32(signal1.Substring(4, 1));
                            diPort[4] = Convert.ToInt32(signal1.Substring(3, 1));
                            diPort[5] = Convert.ToInt32(signal1.Substring(2, 1));
                            diPort[6] = Convert.ToInt32(signal1.Substring(1, 1));
                            diPort[7] = Convert.ToInt32(signal1.Substring(0, 1));
                            diPort[8] = Convert.ToInt32(signal2.Substring(7, 1));
                            diPort[9] = Convert.ToInt32(signal2.Substring(6, 1));
                            diPort[10] = Convert.ToInt32(signal2.Substring(5, 1));

                            for (int i = 0; i < diPort.Length; i++)
                            {
                                if (diPort[i] == 1)
                                {
                                    TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "DI" + i.ToString() + " 에서 신호옴");
                                }
                            }

                            //Door신호관련
                            if (diPort[NPSYS.gDoorSignalNumber] == 1)
                            {

                                //CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, (byte)0x6A);// Door열림
                                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "Door열림");
                            }
                            if (diPort[NPSYS.gDoorSignalNumber] == 0)
                            {

                                //CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.DID, (byte)0x88); // Door답힘
                                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "Door닫힘");
                            }
                            //Door신호관련 완료
                            if (diPort[NPSYS.gReceiptSignalNumber] == 1)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | mSerialPort_DataReceived", "영수증도어신호누름");
                                // m_IsReceiptSignal = true;
                                if (this != null)
                                {
                                    DosensorSignalEvent(this, SignalType.ReciptSignal);
                                }
                            }

                            //Door신호관련 완료

                            m_SignalData = signal1 + signal2;
                        }
                        mReadBuffer.Clear();

                        return;
                    }
                    else
                    {
                        return;
                    }
                }



            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "GoodTechContorlBoard | mSerialPort_DataReceived", "자료수신중 예외상황:" + ex.ToString());
            }
        }


        public void RequestBoardCheck()
        {
            try
            {
                if (NPSYS.Device.UsingSettingControlBoard != NPCommon.ConfigID.ControlBoardType.GOODTECH)
                {
                    TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "사용안함");
                }

                if (!SerialPort.IsOpen)
                {
                    TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | RequestBoardCheck", "포트가 열려있지 않습니다");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //   TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "DoSendor|RequestResetTimeSetting", "보드리셋타임설정:" + pResetSecond.ToString() + "초");
                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[4];
                l_SendCommand[0] = startParameter;
                l_SendCommand[1] = 0x04;
                l_SendCommand[2] = DoCommandRequestBoradStatusParameter;
                l_SendCommand[3] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2]);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "전송중 에러:" + ex.ToString());
            }
        }

        public void RequestResetTimeSetting(int pResetSecond)
        {
            try
            {
                if (!SerialPort.IsOpen)
                {
                    TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | RequestResetTimeSetting", "포트가 열려있지 않습니다");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "GoodTechContorlBoard | RequestResetTimeSetting", "보드리셋타임설정:" + pResetSecond.ToString() + "초");
                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[5];
                l_SendCommand[0] = startParameter;
                l_SendCommand[1] = 0x05;
                l_SendCommand[2] = DoCommandSettingUsbResetTime;
                l_SendCommand[3] = Convert.ToByte(pResetSecond);
                l_SendCommand[4] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3]);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "전송중 에러:" + ex.ToString());
            }
        }
        public enum ResetSeting
        {
            UnResetSetting = 0,
            ResetSetting = 1,
            ResetStatus = 2
        }
        /// <summary>
        /// 0이면 비활성요청, 1이면 활성요청 2이면 상태확인
        /// </summary>
        /// <param name="pResetSecond"></param>
        public void RequestResetSetting(ResetSeting pResetSeting)
        {
            try
            {
                if (NPSYS.Device.UsingSettingControlBoard != NPCommon.ConfigID.ControlBoardType.GOODTECH)
                {
                    TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "사용안함");
                }

                if (!SerialPort.IsOpen)
                {
                    TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | RequestResetTimeSetting", "포트가 열려있지 않습니다");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                string lStatus = string.Empty;
                switch (pResetSeting)
                {
                    case ResetSeting.UnResetSetting:
                        lStatus = "비활성 요청";
                        break;
                    case ResetSeting.ResetSetting:
                        lStatus = "활성화 요청";
                        break;
                    case ResetSeting.ResetStatus:
                        lStatus = "상태요청";
                        break;

                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "GoodTechContorlBoard | RequestResetTimeSetting", "보드리셋설정:" + lStatus);
                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[5];
                l_SendCommand[0] = startParameter;
                l_SendCommand[1] = 0x05;
                l_SendCommand[2] = DoCommandUseResetSeting;
                int lSeting = (int)pResetSeting;
                l_SendCommand[3] = Convert.ToByte(lSeting);
                l_SendCommand[4] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3]);
                string data = string.Empty;
                for (int i = 0; i < l_SendCommand.Length; i++)
                {
                    data += i.ToString() + ":" + l_SendCommand[i].ToString("X2") + " ";
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "GoodTechContorlBoard | RequestResetTimeSetting", "보드리셋설정커맨드값:" + data);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "전송중 에러:" + ex.ToString());
            }
        }

        public void GetVersion()
        {
            try
            {
                if (NPSYS.Device.UsingSettingControlBoard != NPCommon.ConfigID.ControlBoardType.GOODTECH)
                {
                    TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "사용안함");
                    return;
                }

                if (!SerialPort.IsOpen)
                {
                    TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | GetVersion", "포트가 열려있지 않습니다");
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "GoodTechContorlBoard | GetVersion", "버젼정보요청");
                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[4];
                l_SendCommand[0] = startParameter; // 0xbc
                l_SendCommand[1] = 0x04;
                l_SendCommand[2] = DoCommandVersionParameter; // 0x03
                l_SendCommand[3] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2]); //0xBB
                string lSendData = string.Empty;
                for (int i = 0; i < l_SendCommand.Length; i++)
                {
                    lSendData += " " + i.ToString() + ":" + l_SendCommand[i].ToString("X2");
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "GoodTechContorlBoard | GetVersion", "버젼정보요청:" + lSendData);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;



            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | GetVersion", "전송중 에러:" + ex.ToString());
            }
        }
        /// <summary>
        /// 시리얼로 전송만 하고 응답값은 받지않는다.(기존장비로 양주드림월드,대구New&new,가리온빌딩,덕현빌딩,성원상떼빌,신한데뷰,광명역사점 1차까지 이장비로 나감)
        /// </summary>
        /// <param name="p_CmdParameter"></param>
        public void DoCoinstrainOn(bool p_Do0, bool p_Do1, bool p_Do2, bool p_Do3, bool p_Do4, bool p_Do5, bool p_Do6, bool p_Do7, bool p_Do8, bool p_Do9)
        {
            try
            {
                if (NPSYS.Device.UsingSettingControlBoard != NPCommon.ConfigID.ControlBoardType.GOODTECH)
                {

                    return;
                }
                if (!SerialPort.IsOpen)
                {
                    throw new Exception("포트가 열려있지 않습니다.");
                }
                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}
                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "version 0 LED제어실행");
                byte l_do0 = 0x00;
                byte l_do1 = 0x00;
                byte l_do2 = 0x00;
                byte l_do3 = 0x00;
                byte l_do4 = 0x00;
                byte l_do5 = 0x00;
                byte l_do6 = 0x00;
                byte l_do7 = 0x00;
                byte l_do8 = 0x00;
                byte l_do9 = 0x00;


                if (p_Do0)
                {
                    l_do0 = 0x01;
                }
                else
                {
                    l_do0 = 0x00;
                }
                if (p_Do1)
                {
                    l_do1 = 0x04;
                }
                else
                {
                    l_do1 = 0x00;
                }
                if (p_Do2)
                {
                    l_do2 = 0x10;
                }
                else
                {
                    l_do2 = 0x00;
                }
                if (p_Do3)
                {
                    l_do3 = 0x40;
                }
                else
                {
                    l_do3 = 0x00;
                }
                if (p_Do4)
                {
                    l_do4 = 0x01;
                }
                else
                {
                    l_do4 = 0x00;
                }
                if (p_Do5)
                {
                    l_do5 = 0x04;
                }
                else
                {
                    l_do5 = 0x00;
                }
                if (p_Do6)
                {
                    l_do6 = 0x10;
                }
                else
                {
                    l_do6 = 0x00;
                }
                if (p_Do7)
                {
                    l_do7 = 0x40;
                }
                else
                {
                    l_do7 = 0x00;
                }
                if (p_Do8)
                {
                    l_do8 = 0x01;
                }
                else
                {
                    l_do8 = 0x00;
                }
                if (p_Do9)
                {
                    l_do9 = 0x04;
                }
                else
                {
                    l_do9 = 0x00;
                }


                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[7];
                l_SendCommand[0] = startParameter;
                l_SendCommand[1] = 0x07;
                l_SendCommand[2] = DoCommandRequestConstrainOnParameter;
                l_SendCommand[3] = (byte)(l_do0 | l_do1 | l_do2 | l_do3);
                l_SendCommand[4] = (byte)(l_do4 | l_do5 | l_do6 | l_do7);
                l_SendCommand[5] = (byte)(l_do8 | l_do9);
                l_SendCommand[6] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5]);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "전송중 에러:" + ex.ToString());
            }
        }


        /// <summary>
        /// 시리얼로 전송만 하고 응답값은 받지않는다.(기존장비로 양주드림월드,대구New&new,가리온빌딩,덕현빌딩,성원상떼빌,신한데뷰,광명역사점 1차 이후 나감)
        /// </summary>
        /// <param name="p_CmdParameter"></param>
        public void DoCoinstrainNewOn(bool p_Do0, bool p_Do1, bool p_Do2, bool p_Do3, bool p_Do4, bool p_Do5, bool p_Do6, bool p_Do7, bool p_Do8, bool p_Do9, bool p_Do10, bool p_Do11, bool p_Do12, bool p_Do13)
        {
            try
            {
                if (NPSYS.Device.UsingSettingControlBoard != NPCommon.ConfigID.ControlBoardType.GOODTECH)
                {
                    return;
                }

                if (!SerialPort.IsOpen)
                {
                    TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "포트가 열려있지 않습니다");
                    throw new Exception("포트가 열려있지 않습니다.");
                }

                //while (mStep != ProtocolStep.Ready)
                //{
                //    System.Windows.Forms.Application.DoEvents();
                //    //;
                //}
                byte l_do0 = 0x00;
                byte l_do1 = 0x00;
                byte l_do2 = 0x00;
                byte l_do3 = 0x00;
                byte l_do4 = 0x00;
                byte l_do5 = 0x00;
                byte l_do6 = 0x00;
                byte l_do7 = 0x00;
                byte l_do8 = 0x00;
                byte l_do9 = 0x00;
                byte l_do10 = 0x00;
                byte l_do11 = 0x00;
                byte l_do12 = 0x00;
                byte l_do13 = 0x00;


                if (p_Do0)
                {
                    l_do0 = 0x01;
                }
                else
                {
                    l_do0 = 0x00;
                }
                if (p_Do1)
                {
                    l_do1 = 0x04;
                }
                else
                {
                    l_do1 = 0x00;
                }
                if (p_Do2)
                {
                    l_do2 = 0x10;
                }
                else
                {
                    l_do2 = 0x00;
                }
                if (p_Do3)
                {
                    l_do3 = 0x40;
                }
                else
                {
                    l_do3 = 0x00;
                }
                if (p_Do4)
                {
                    l_do4 = 0x01;
                }
                else
                {
                    l_do4 = 0x00;
                }
                if (p_Do5)
                {
                    l_do5 = 0x04;
                }
                else
                {
                    l_do5 = 0x00;
                }
                if (p_Do6)
                {
                    l_do6 = 0x10;
                }
                else
                {
                    l_do6 = 0x00;
                }
                if (p_Do7)
                {
                    l_do7 = 0x40;
                }
                else
                {
                    l_do7 = 0x00;
                }
                if (p_Do8)
                {
                    l_do8 = 0x01;
                }
                else
                {
                    l_do8 = 0x00;
                }
                if (p_Do9)
                {
                    l_do9 = 0x04;
                }
                else
                {
                    l_do9 = 0x00;
                }
                if (p_Do10)
                {
                    l_do10 = 0x10;
                }
                else
                {
                    l_do10 = 0x00;
                }
                if (p_Do11)
                {
                    l_do11 = 0x40;
                }
                else
                {
                    l_do11 = 0x00;
                }
                if (p_Do12)
                {
                    l_do12 = 0x01;
                }
                else
                {
                    l_do12 = 0x00;
                }
                if (p_Do13)
                {
                    l_do13 = 0x04;
                }
                else
                {
                    l_do13 = 0x00;
                }


                mReadBuffer.Clear();
                mStep = ProtocolStep.DoCommand;
                byte[] l_SendCommand = new byte[8];
                l_SendCommand[0] = startParameter;
                l_SendCommand[1] = 0x08;
                l_SendCommand[2] = DoCommandRequestConstrainOnParameter;
                l_SendCommand[3] = (byte)(l_do0 | l_do1 | l_do2 | l_do3);
                l_SendCommand[4] = (byte)(l_do4 | l_do5 | l_do6 | l_do7);
                l_SendCommand[5] = (byte)(l_do8 | l_do9 | l_do10 | l_do11);
                l_SendCommand[6] = (byte)(l_do12 | l_do13);
                l_SendCommand[7] = (byte)(l_SendCommand[0] ^ l_SendCommand[1] ^ l_SendCommand[2] ^ l_SendCommand[3] ^ l_SendCommand[4] ^ l_SendCommand[5] ^ l_SendCommand[6]);
                string lSendData = string.Empty;
                for (int i = 0; i < l_SendCommand.Length; i++)
                {
                    lSendData += " " + i.ToString() + ":" + l_SendCommand[i].ToString("X2");
                }
                TextCore.ACTION(TextCore.ACTIONS.DIDO, "GoodTechContorlBoard | DoCoinstrainNewOn", "version 1 LED제어실행:" + lSendData);
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {

                TextCore.DeviceError(TextCore.DEVICE.DIDO, "GoodTechContorlBoard | DoCoinstrainOn", "전송중 에러:" + ex.ToString());
            }
        }

        //private int mTimeOut = 10000;



    }
    //GS POS할인 주석완료
}
