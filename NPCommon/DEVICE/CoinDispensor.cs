using FadeFox.Text;
using NPCommon;
using NPCommon.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using static NPCommon.DEVICE.CoinDispensor;

namespace NPCommon.DEVICE
{

    /// <summary>
    /// 동전방출기 제어
    /// </summary>
    public class CoinDispensor : AbstractSerialPort<CoinDispensorStatusType>
    {
        ProtocolStep mStep = ProtocolStep.Ready;
        private CoinType mCoinType = CoinType.Money500Type;
        public CoinDispensorStatusManageMent CurrentCoinDispensorStatusManagement = new CoinDispensorStatusManageMent();
        public enum CoinType
        {
            Money50Type,
            Money100Type,
            Money500Type
        }
        #region 속성
        public CoinType CurrentCoinType
        {
            set
            {
                mCoinType = value;
                CurrentCoinDispensorStatusManagement.currentCoinType = value;
            }
            get { return mCoinType; }
        }

        public override void Initialize()
        {
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }

        public override CoinDispensorStatusType Connect()
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

                CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.PortOpenStatus, true);
                return CoinDispensorStatusType.OK;
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.COINCHARGER, "CoinDispensor|Connect", "연결실패:" + ex.ToString());
                CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.PortOpenStatus, false);
                return CoinDispensorStatusType.PortOpenStatus;
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
                TextCore.DeviceError(TextCore.DEVICE.COINCHARGER, "CoinDispensor|Disconnect", "연결종료실패:" + ex.ToString());
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
                int length = SerialPort.BytesToRead;

                if (mStep == ProtocolStep.RemainCoin)
                {
                    for (int i = 0; i < length; i++)
                    {
                        mReadBuffer.Add((byte)SerialPort.ReadByte());
                    }

                    byte[] resRemain = mReadBuffer.ToArray();
                    string data = "";
                    for (int i = 0; i < resRemain.Length; i++)
                    {
                        data += i.ToString() + ":" + resRemain[i].ToString("X") + " ";
                    }
                    TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|mSerialPort_DataReceived", "RemainCoin:" + data);

                    if (resRemain.Length == 1)
                    {
                        return;
                    }

                    mReadBuffer.Clear();
                    mReceiveData = resRemain;

                    mStep = ProtocolStep.Ready;
                    return;

                }

                if (mStep == ProtocolStep.OurResult)  // 동전방출시에는 응답값이 5E,3E 가오고 방출실패면 5E,5E 가 옴
                {
                    //if (mReadBuffer.Count >= 2)
                    //{
                    //    return;
                    //}
                    //else
                    //{
                    for (int i = 0; i < length; i++)
                    {
                        mReadBuffer.Add((byte)SerialPort.ReadByte());
                    }
                    byte[] resRemain = mReadBuffer.ToArray();

                    string l_SerialRemainCoinReadData = "";
                    for (int i = 0; i < resRemain.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                l_SerialRemainCoinReadData += resRemain[i].ToString("X");
                                break;
                            case 1:
                                l_SerialRemainCoinReadData += resRemain[i].ToString("X");
                                break;
                            case 2:
                                l_SerialRemainCoinReadData += resRemain[i].ToString("X");
                                break;
                            case 3:
                                l_SerialRemainCoinReadData += resRemain[i].ToString("X");
                                break;
                            case 4:
                                l_SerialRemainCoinReadData += resRemain[i].ToString("X");
                                break;

                        }
                    }

                    TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|mSerialPort_DataReceived", "동전방출결과:" + resRemain.Length.ToString() + " 데이터값:" + l_SerialRemainCoinReadData);
                    if (mReadBuffer.Contains(0x5E))
                    {
                        int ranange = mReadBuffer.IndexOf(0x5E);
                        int reangecount = ranange + 2;
                        if (mReadBuffer.IndexOf(0x5E) + 2 <= mReadBuffer.Count)
                        {
                            if (mReadBuffer[mReadBuffer.IndexOf(0x5E) + 1] == 0x3E)
                            {
                                mReceiveData = new byte[1] { 0x3E };
                                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|mSerialPort_DataReceived", "동전방출결과성공");
                                mReadBuffer.Clear();
                                mStep = ProtocolStep.Ready;
                                return;
                            }
                            else
                            {
                                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|mSerialPort_DataReceived", "동전방출결과실패");
                                mReceiveData = new byte[1] { 0x5E };
                                mReadBuffer.Clear();
                                mStep = ProtocolStep.Ready;
                                return;

                            }
                        }
                    }
                    return;

                }

                mStep = ProtocolStep.ReceiveData;

                for (int i = 0; i < length; i++)
                {
                    mReadBuffer.Add((byte)SerialPort.ReadByte());
                }

                byte[] res = mReadBuffer.ToArray();

                string l_SerialReadData = "";
                for (int i = 0; i < res.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            l_SerialReadData += res[i].ToString("X");
                            break;
                        case 1:
                            l_SerialReadData += res[i].ToString("X");
                            break;
                        case 2:
                            l_SerialReadData += res[i].ToString("X");
                            break;
                        case 3:
                            l_SerialReadData += res[i].ToString("X");
                            break;
                        case 4:
                            l_SerialReadData += res[i].ToString("X");
                            break;

                    }
                }

                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|mSerialPort_DataReceived", "데이터수신:" + " 데이터길이:" + res.Length.ToString() + " 데이터값:" + l_SerialReadData);

                if (res.Length > 1)
                {
                    res[0] = mReadBuffer[mReadBuffer.Count - 1];
                }


                mStep = ProtocolStep.ReceiveData;

                mReadBuffer.Clear();
                mReceiveData = res;

                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|mSerialPort_DataReceived", "데이터수신중 예외사항:" + ex.ToString());
            }
        }

        #endregion



        public CoinDispensor()
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
            ReceiveData,
            RemainCoin,
            OurResult
        }

        /// <summary>
        /// // inhibiy ba,if hopper problems occured
        /// </summary>
        private const byte g_CMD_DisableOutCoin = 0x05;
        /// <summary>
        /// // Enable ba,if hopper problems occured
        /// </summary>
        private const byte g_CMD_EnableOutCoin = 0x03;
        /// <summary>
        /// // 장비가 시리얼에서 보낸 신호를 받는지 확인 명령어
        /// </summary>
        private const byte g_CMD_BaReadinessSignal = 0x80;
        /// <summary>
        /// // 상태요청 명령어
        /// </summary>
        private const byte g_CMD_CurrentStatus = 0x72;
        /// <summary>
        ///  // 동전방출을 제외한 모든 제어명령어의 처음에 들어간다.
        /// </summary>
        private const byte g_CMD_StartParameter = 0x70;
        /// <summary>
        /// // 장비 리셋명령      
        /// </summary>
        private const byte g_CMD_Reset = 0x73;
        /// <summary>
        /// // 동전방출한다는 명령어
        /// </summary>
        private const byte g_CMD_BillInEscrow = 0x81;
        /// <summary>
        /// // 동전방출 실행명령
        /// </summary>
        private const byte g_CMD_BillDispense = 0x10;
        /// <summary>
        /// // 동전방출 실행명령
        /// </summary>
        private const byte g_CMD_BillDispenseCancle = 0x11;
        /// <summary>
        /// 타임아웃
        /// </summary>
        private const byte g_TIMEOUT_STATUS = 0x99;

        /// <summary>
        /// 동전 방출을 장비에서 거절
        /// </summary>
        private const byte g_COIN_OUT_REJECT = 0x97;
        /// <summary>
        /// 동전방출 미완료
        /// </summary>
        private const byte g_COIN_NOT_OUT_FULL = 0x98;

        /// <summary>
        /// 동전방출에서 기본적인 동작에 대한 정상 응답값 
        /// </summary>
        private const byte g_OKSTATUS = 0x12;

        private byte[] mTimeOutData = { 0x99 };
        private byte[] mOkData = { 0x98 };

        private enum Cmd
        {
            StartParameter,
            EnableOutCoin,
            DisableOutCoin,
            CurrentStatus,
            Reset,
            BillInEscrow,
            BillDispense,
            BillDispenseCancle,
            BaReadinessSignal
        }
        private byte cmdSelect(Cmd p_CmdData)
        {
            byte cmdData = new byte();

            switch (p_CmdData)
            {
                case Cmd.EnableOutCoin:
                    cmdData = g_CMD_EnableOutCoin;
                    break;
                case Cmd.DisableOutCoin:
                    cmdData = g_CMD_DisableOutCoin;
                    break;
                case Cmd.CurrentStatus:
                    cmdData = g_CMD_CurrentStatus;
                    break;
                case Cmd.StartParameter:
                    cmdData = g_CMD_StartParameter;
                    break;
                case Cmd.Reset:
                    cmdData = g_CMD_Reset;
                    break;
                case Cmd.BillInEscrow:
                    cmdData = g_CMD_BillInEscrow;
                    break;
                case Cmd.BillDispense:
                    cmdData = g_CMD_BillDispense;
                    mStep = ProtocolStep.OurResult;
                    break;
                case Cmd.BillDispenseCancle:
                    cmdData = g_CMD_BillDispense;
                    break;
                case Cmd.BaReadinessSignal:
                    cmdData = g_CMD_BaReadinessSignal;
                    break;


            }
            return cmdData;
        }


        public List<byte> mReadBuffer = new List<byte>();

        private int mTimeOut = 20000;
        private byte[] mOk = { g_OKSTATUS };
        private byte[] mCoin_Reject = { g_COIN_OUT_REJECT };
        private byte[] mCoin_Not_Full = { g_COIN_NOT_OUT_FULL };


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
                mStep = ProtocolStep.SendCommand;
                byte[] l_SendCommand = new byte[1];
                l_SendCommand[0] = cmdSelect(p_CmdParameter);
                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|SendCommand", "명령어보냄:" + p_CmdParameter.ToString());
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COINCHARGER, "CoinDispensor|SendCommand", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CoinDispensor | SendCommand", "방출결과" + mReceiveData[0].ToString("X"));
                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|SendCommand", "예외사항:" + ex.ToString());
                return mTimeOutData;
            }
        }



        private byte[] SendCommand(byte p_Parameter)
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
                mStep = ProtocolStep.SendCommand;
                if (p_Parameter == 0x71)
                {
                    mStep = ProtocolStep.RemainCoin;
                }
                byte[] l_SendCommand = new byte[1];
                l_SendCommand[0] = p_Parameter;
                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|SendCommand", "명령어보냄:" + p_Parameter.ToString());
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);

                DateTime startDate = DateTime.Now;

                while (mStep != ProtocolStep.Ready)
                {
                    TimeSpan diff = DateTime.Now - startDate;

                    if (Convert.ToInt32(diff.TotalMilliseconds) > mTimeOut)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.COINCHARGER, "CoinDispensor|SendCommand", "응답시간초과");
                        mReadBuffer.Clear();
                        mStep = ProtocolStep.Ready;
                        return mTimeOutData;
                    }
                }

                return mReceiveData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|SendCommand", "예외사항:" + ex.ToString());

                return mTimeOutData;
            }
        }

        public byte[] BaReadinessSignal()
        {
            return SendCommand(Cmd.BaReadinessSignal);
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
                mStep = ProtocolStep.SendCommand;
                byte[] l_SendCommand = new byte[1];
                l_SendCommand[0] = cmdSelect(p_CmdParameter);
                TextCore.ACTION(TextCore.ACTIONS.COINCHARGER, "CoinDispensor|SendCommandNotTimeOut", "명령어보냄:" + p_CmdParameter.ToString());
                SerialPort.Write(l_SendCommand, 0, l_SendCommand.Length);
                mStep = ProtocolStep.Ready;


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|SendCommandNotTimeOut", "예외사항:" + ex.ToString());
            }
        }


        //public void EnableOutCoin()
        //{
        //    SendCommandNotTimeOut(Cmd.EnableOutCoin);
        //}

        //public void DisableOutCoin()
        //{
        //    SendCommandNotTimeOut(Cmd.DisableOutCoin);
        //}


        /// <summary>
        /// 장비리셋
        /// </summary>
        /// <returns></returns>
        public CoinDispensorStatusType reset()
        {
            try
            {
                byte[] result = SendCommand(Cmd.StartParameter);
                if (result[0] == g_OKSTATUS)
                {
                    SendCommandNotTimeOut(Cmd.Reset);

                }
                else
                {
                    return ConvertStatusTypeToByte(mTimeOutData[0]);


                }
                return ConvertStatusTypeToByte(result[0]);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|reset", "장비 리셋중 예외사항:" + ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }

        }
        /// <summary>
        /// 동전방출
        /// </summary>
        /// <param name="p_DispensValue"></param>
        /// <returns></returns>
        public CoinDispensorStatusType OutCharge(int p_DispensValue)
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CoinDispensor | OutCharge", "방출명령내림 " + Cmd.BillInEscrow.ToString());
                SendCommandNotTimeOut(Cmd.BillInEscrow);
                byte l_DispensValue;
                switch (p_DispensValue)
                {
                    case 1:
                        l_DispensValue = 0x40;
                        break;
                    case 2:
                        l_DispensValue = 0x41;
                        break;
                    case 3:
                        l_DispensValue = 0x42;
                        break;
                    case 4:
                        l_DispensValue = 0x43;
                        break;
                    case 5:
                        l_DispensValue = 0x44;
                        break;
                    case 6:
                        l_DispensValue = 0x45;
                        break;
                    case 7:
                        l_DispensValue = 0x46;
                        break;
                    case 8:
                        l_DispensValue = 0x47;
                        break;
                    case 9:
                        l_DispensValue = 0x48;
                        break;
                    case 10:
                        l_DispensValue = 0x49;
                        break;
                    case 11:
                        l_DispensValue = 0x4A;
                        break;
                    case 12:
                        l_DispensValue = 0x4B;
                        break;
                    case 13:
                        l_DispensValue = 0x4C;
                        break;
                    case 14:
                        l_DispensValue = 0x4D;
                        break;
                    case 15:
                        l_DispensValue = 0x4E;
                        break;
                    default:
                        l_DispensValue = 0x40;
                        break;


                }
                SendCommand(l_DispensValue);

                if (mReceiveData[0] == 0x02) // 방출 할수있음
                {
                    byte[] result = SendCommand(Cmd.BillDispense);
                    if (result[0] == 0x3E)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CoinDispensor | OutCharge", "방출완료");
                        return ConvertStatusTypeToByte(mOk[0]);

                    }
                    if (result[0] == 0x99)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor | OutCharge", "방출하려고 했는대 타임아웃");
                        return ConvertStatusTypeToByte(mTimeOutData[0]);

                    }
                    else if (result[0] == 0x5E)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor | OutCharge", "방출하려고 했는대 방출을 다 하지못함");
                        return ConvertStatusTypeToByte(mCoin_Reject[0]);
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor | OutCharge", "방출하려고 했는대 이상한 값이옴");
                        return ConvertStatusTypeToByte(mCoin_Reject[0]);
                    }

                }
                else if (mReceiveData[0] == 0x0F)  // 방출하려고 했는대 방출이 거절당함
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|OutCharge", "방출하려고 했는대 방출이 거절당함");
                    SendCommandNotTimeOut(Cmd.BillDispenseCancle);

                    return ConvertStatusTypeToByte(mCoin_Reject[0]);
                }

                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|OutCharge", "동전방출중 예외사항:" + ex.ToString());
                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }

        /// <summary>
        /// 현재상태체크 가장 마지막에 기억된 정보로 처리
        /// </summary>
        /// <returns></returns>
        public CoinDispensorStatusType CurrentStatus()
        {
            try
            {
                byte[] l_resultCurrentStatus = SendCommand(Cmd.StartParameter);

                if (l_resultCurrentStatus[0] == g_OKSTATUS)
                {
                    byte[] l_result = SendCommand(Cmd.CurrentStatus);
                    return ConvertStatusTypeToByte(l_result[0]);
                }
                else
                {
                    return ConvertStatusTypeToByte(mTimeOutData[0]);
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|CurrentStatus", "상태확인중 예외사항:" + ex.ToString());

                return ConvertStatusTypeToByte(mTimeOutData[0]);
            }
        }

        /// <summary>
        /// 동전 미반환 숫자리턴
        /// </summary>
        /// <returns></returns>
        public int RemainCoin()
        {
            try
            {
                int l_remainCoinCount = 0;
                byte[] l_RemainCoinStatus = SendCommand(Cmd.StartParameter);
                if (l_RemainCoinStatus[0] == g_OKSTATUS)
                {
                    byte[] l_result = SendCommand(0x71);
                    if (l_result.Length > 1)
                    {
                        l_remainCoinCount = TextCore.HexaToDecimal(l_result);
                    }

                    return l_remainCoinCount;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CoinDispensor|RemainCoin", "ELSE");
                    return 0;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "CoinDispensor|RemainCoin", "방출된 동전 확인중 예외사항:" + ex.ToString());
                return 0;
            }
        }

        public CoinDispensorStatusType ConvertStatusTypeToByte(byte status)
        {
            switch (status)
            {
                case 0x01:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.MotorProblemStatus, false);
                    return CoinDispensorStatusType.MotorProblemStatus;


                case 0x02:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.CheckForcoinAvailablityStatus, false);
                    return CoinDispensorStatusType.CheckForcoinAvailablityStatus;
                case 0x03:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.ReadJustShaft_CoinSizeVariesStatus, false);
                    return CoinDispensorStatusType.ReadJustShaft_CoinSizeVariesStatus;
                case 0x04:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.OK, true);
                    return CoinDispensorStatusType.NONE;
                case 0x05:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.PrismSensorStatus, false);
                    return CoinDispensorStatusType.PrismSensorStatus;
                case 0x06:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.ShaftSensorStatus, false);
                    return CoinDispensorStatusType.ShaftSensorStatus;
                case 0x12:
                case 0x00:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.OK, true);
                    return CoinDispensorStatusType.OK;
                case 0x97:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.RejectCoinStatus, false);
                    return CoinDispensorStatusType.RejectCoinStatus;
                case 0x98:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.PartiallyReleasedStatus, false);
                    return CoinDispensorStatusType.PartiallyReleasedStatus;
                case 0x99:
                    CurrentCoinDispensorStatusManagement.SettingDeviceStatus(CoinDispensorStatusType.CommuniCationStatus, false);
                    return CoinDispensorStatusType.CommuniCationStatus;

                default:
                    return CoinDispensorStatusType.NONE;

            }
        }

        public enum CoinDispensorStatusType
        {
            NONE,
            OK,
            DeviceStatus,
            CommuniCationStatus,
            RejectCoinStatus,
            PartiallyReleasedStatus,
            MotorProblemStatus,
            CheckForcoinAvailablityStatus,
            ReadJustShaft_CoinSizeVariesStatus,
            ShaftSensorStatus,
            PortOpenStatus,
            PrismSensorStatus
        }

        public class CoinDispensorStatusManageMent
        {
            public enum CoinDetailStatusType
            {
                Coin1_DeviceStatus = 2501,
                Coin1_CommuniCationStatus = 2502,
                Coin1_RejectCoinStatus = 2508,
                Coin1_PartiallyReleasedStatus = 2509,
                Coin1_MotorProblemStatus = 2510,
                Coin1_CheckForcoinAvailablityStatus = 2511,
                Coin1_ReadJustShaft_CoinSizeVariesStatus = 2512,
                Coin1_ShaftSensorStatus = 2513,
                Coin1_PortOpenStatus = 2514,
                Coin1_PrismSensorStatus = 2515,
                Coin2_DeviceStatus = 2601,
                Coin2_CommuniCationStatus = 2602,
                Coin2_RejectCoinStatus = 2608,
                Coin2_PartiallyReleasedStatus = 2609,
                Coin2_MotorProblemStatus = 2610,
                Coin2_CheckForcoinAvailablityStatus = 2611,
                Coin2_ReadJustShaft_CoinSizeVariesStatus = 2612,
                Coin2_ShaftSensorStatus = 2613,
                Coin2_PortOpenStatus = 2614,
                Coin2_PrismSensorStatus = 2615,
                Coin3_DeviceStatus = 2701,
                Coin3_CommuniCationStatus = 2702,
                Coin3_RejectCoinStatus = 2708,
                Coin3_PartiallyReleasedStatus = 2709,
                Coin3_MotorProblemStatus = 2710,
                Coin3_CheckForcoinAvailablityStatus = 2711,
                Coin3_ReadJustShaft_CoinSizeVariesStatus = 2712,
                Coin3_ShaftSensorStatus = 2713,
                Coin3_PortOpenStatus = 2714,
                Coin3_PrismSensorStatus = 2715,
                NONE_50Alarm = 2505,
                NONE_100Alarm = 2605,
                NONE_500Alarm = 2705,
                MIN_50Alarm = 2504,
                MIN_100Alarm = 2604,
                MIN_500Alarm = 2704,
                MAX_50Alarm = 2506,
                MAX_100Alarm = 2606,
                MAX_500Alarm = 2706,

            }

            public CoinDispensor.CoinType currentCoinType = CoinDispensor.CoinType.Money50Type;
            public void SettingDeviceStatus(CoinDispensorStatusType pResultType, bool isSuccess)
            {
                switch (pResultType)
                {
                    case CoinDispensorStatusType.OK:
                        SetCheckForcoinAvailablityStatus(isSuccess);
                        SetCommunicationStatus(isSuccess);
                        SetDeviceStatus(isSuccess);
                        SetMotorProblemStatus(isSuccess);
                        SetPartiallyReleasedStatus(isSuccess);
                        SetPortOpenStatus(isSuccess);
                        SetReadJustShaft_CoinSizeVariesStatus(isSuccess);
                        SetRejectCoinStatus(isSuccess);
                        SetShaftSensorStatus(isSuccess);
                        SetPrismSensorError(isSuccess);

                        break;

                    case CoinDispensorStatusType.CheckForcoinAvailablityStatus:
                        SetCheckForcoinAvailablityStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.CommuniCationStatus:
                        SetCommunicationStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.DeviceStatus:
                        SetDeviceStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.MotorProblemStatus:
                        SetMotorProblemStatus(isSuccess);
                        break;

                    case CoinDispensorStatusType.PartiallyReleasedStatus:
                        SetPartiallyReleasedStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.PortOpenStatus:
                        SetPortOpenStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.ReadJustShaft_CoinSizeVariesStatus:
                        SetReadJustShaft_CoinSizeVariesStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.RejectCoinStatus:
                        SetRejectCoinStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.ShaftSensorStatus:
                        SetShaftSensorStatus(isSuccess);
                        break;
                    case CoinDispensorStatusType.PrismSensorStatus:
                        SetPrismSensorError(isSuccess);
                        break;



                }
                GetCoinDispensorDeveiceOpertationYn();
            }


            private void GetCoinDispensorDeveiceOpertationYn()
            {

                bool opertationYn = false;
                if ((mCoin_CheckForcoinAvailablityError || mCoin_CommunicationError || mCoin_DeviceError
                                                            || mCoin_MotorProblemError || mCoin_PartiallyReleasedError || mCoin_PortOpenError
                                                            || mCoin_ReadJustShaft_CoinSizeVaries || mCoin_RejectCoinError || mCoin_ShaftSensorFailure
                                                            || mCoin_PrismSensorError) == true)
                {
                    opertationYn = false;
                }
                else
                {
                    opertationYn = true;
                }

                if (currentCoinType == CoinType.Money50Type)
                {
                    NPSYS.Device.gIsUseCoinDischarger50Device = opertationYn;
                }
                if (currentCoinType == CoinType.Money100Type)
                {
                    NPSYS.Device.gIsUseCoinDischarger100Device = opertationYn;
                }
                if (currentCoinType == CoinType.Money500Type)
                {
                    NPSYS.Device.gIsUseCoinDischarger500Device = opertationYn;
                }

            }

            /// <summary>
            /// 현재 지정된 코인방출기의 최종상태를 보낸다 true면 정상 false면 비정상
            /// </summary>
            /// <returns></returns>
            public bool GetCoindischargerFianlStatus()
            {
                if (currentCoinType == CoinType.Money50Type)
                {
                    return NPSYS.Device.gIsUseCoinDischarger50Device;

                }
                if (currentCoinType == CoinType.Money100Type)
                {
                    return NPSYS.Device.gIsUseCoinDischarger100Device;
                }
                if (currentCoinType == CoinType.Money500Type)
                {
                    return NPSYS.Device.gIsUseCoinDischarger500Device;
                }
                return false;

            }


            private bool mCoin_DeviceError = false;


            private void SetDeviceStatus(bool pIsDeviceOk)
            {

                if (mCoin_DeviceError)
                {
                    if (pIsDeviceOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_DeviceStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_DeviceStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_DeviceStatus);
                        }

                        mCoin_DeviceError = !pIsDeviceOk;

                    }
                }
                else
                {
                    if (pIsDeviceOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_DeviceStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_DeviceStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_DeviceStatus);
                        }
                        mCoin_DeviceError = !pIsDeviceOk;

                    }
                }
            }


            private bool mCoin_CommunicationError = false;
            //public bool Coin_CommunicationError
            //{
            //    get { return mCoin_CommunicationError; }
            //}

            private void SetCommunicationStatus(bool pIsCommunicationOk)
            {

                if (mCoin_CommunicationError)
                {
                    if (pIsCommunicationOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CommuniCationStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CommuniCationStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CommuniCationStatus);
                        }

                        mCoin_CommunicationError = !pIsCommunicationOk;

                    }
                }
                else
                {
                    if (pIsCommunicationOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_CommuniCationStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_CommuniCationStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_CommuniCationStatus);
                        }
                        mCoin_CommunicationError = !pIsCommunicationOk;

                    }
                }
            }



            private bool mCoin_RejectCoinError = false;
            //public bool Coin_RejectCoinError
            //{
            //    get { return mCoin_RejectCoinError; }
            //}

            private void SetRejectCoinStatus(bool pIsCoinRejectOk)
            {

                if (mCoin_RejectCoinError)
                {
                    if (pIsCoinRejectOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_RejectCoinStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_RejectCoinStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_RejectCoinStatus);
                        }

                        mCoin_RejectCoinError = !pIsCoinRejectOk;

                    }
                }
                else
                {
                    if (pIsCoinRejectOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_RejectCoinStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_RejectCoinStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_RejectCoinStatus);
                        }
                        mCoin_RejectCoinError = !pIsCoinRejectOk;

                    }
                }
            }


            private bool mCoin_PartiallyReleasedError = false;
            //public bool Coin_PartiallyReleasedErrorr
            //{
            //    get { return mCoin_PartiallyReleasedError; }
            //}

            private void SetPartiallyReleasedStatus(bool pIsPartiallyReleasedOk)
            {

                if (mCoin_PartiallyReleasedError)
                {
                    if (pIsPartiallyReleasedOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PartiallyReleasedStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PartiallyReleasedStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PartiallyReleasedStatus);
                        }

                        mCoin_PartiallyReleasedError = !pIsPartiallyReleasedOk;

                    }
                }
                else
                {
                    if (pIsPartiallyReleasedOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_PartiallyReleasedStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_PartiallyReleasedStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_PartiallyReleasedStatus);
                        }
                        mCoin_PartiallyReleasedError = !pIsPartiallyReleasedOk;

                    }
                }
            }



            private bool mCoin_MotorProblemError = false;


            private void SetMotorProblemStatus(bool pIsMotorProblemOk)
            {

                if (mCoin_MotorProblemError)
                {
                    if (pIsMotorProblemOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_MotorProblemStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_MotorProblemStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_MotorProblemStatus);
                        }

                        mCoin_MotorProblemError = !pIsMotorProblemOk;

                    }
                }
                else
                {
                    if (pIsMotorProblemOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_MotorProblemStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_MotorProblemStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_MotorProblemStatus);
                        }
                        mCoin_MotorProblemError = !pIsMotorProblemOk;

                    }
                }
            }



            private bool mCoin_CheckForcoinAvailablityError = false;


            private void SetCheckForcoinAvailablityStatus(bool pIsCheckForcoinAvailablityOk)
            {

                if (mCoin_CheckForcoinAvailablityError)
                {
                    if (pIsCheckForcoinAvailablityOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus);
                        }

                        mCoin_CheckForcoinAvailablityError = !pIsCheckForcoinAvailablityOk;

                    }
                }
                else
                {
                    if (pIsCheckForcoinAvailablityOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus);
                        }
                        mCoin_CheckForcoinAvailablityError = !pIsCheckForcoinAvailablityOk;

                    }
                }
            }

            private bool mCoin_ReadJustShaft_CoinSizeVaries = false;


            private void SetReadJustShaft_CoinSizeVariesStatus(bool pIsReadJustShaft_CoinSizeVariesOk)
            {

                if (mCoin_ReadJustShaft_CoinSizeVaries)
                {
                    if (pIsReadJustShaft_CoinSizeVariesOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus);
                        }

                        mCoin_ReadJustShaft_CoinSizeVaries = !pIsReadJustShaft_CoinSizeVariesOk;

                    }
                }
                else
                {
                    if (pIsReadJustShaft_CoinSizeVariesOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus);
                        }
                        mCoin_ReadJustShaft_CoinSizeVaries = !pIsReadJustShaft_CoinSizeVariesOk;

                    }
                }
            }


            private bool mCoin_ShaftSensorFailure = false;


            private void SetShaftSensorStatus(bool pIsShaftSensorFailureOk)
            {

                if (mCoin_ShaftSensorFailure)
                {
                    if (pIsShaftSensorFailureOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ShaftSensorStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ShaftSensorStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ShaftSensorStatus);
                        }

                        mCoin_ShaftSensorFailure = !pIsShaftSensorFailureOk;

                    }
                }
                else
                {
                    if (pIsShaftSensorFailureOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_ShaftSensorStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_ShaftSensorStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_ShaftSensorStatus);
                        }
                        mCoin_ShaftSensorFailure = !pIsShaftSensorFailureOk;

                    }
                }
            }


            private bool mCoin_PrismSensorError = false;


            private void SetPrismSensorError(bool pIsPrismSensorOk)
            {

                if (mCoin_PrismSensorError)
                {
                    if (pIsPrismSensorOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PrismSensorStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PrismSensorStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PrismSensorStatus);
                        }

                        mCoin_PrismSensorError = !pIsPrismSensorOk;

                    }
                }
                else
                {
                    if (pIsPrismSensorOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_PrismSensorStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_PrismSensorStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_PrismSensorStatus);
                        }
                        mCoin_PrismSensorError = !pIsPrismSensorOk;

                    }
                }
            }

            private bool mCoin_PortOpenError = false;


            private void SetPortOpenStatus(bool pIsPortOpenOk)
            {

                if (mCoin_PortOpenError)
                {
                    if (pIsPortOpenOk)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PortOpenStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PortOpenStatus);
                        }
                        else if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PortOpenStatus);
                        }

                        mCoin_PortOpenError = !pIsPortOpenOk;

                    }
                }
                else
                {
                    if (pIsPortOpenOk == false)
                    {
                        if (currentCoinType == CoinDispensor.CoinType.Money50Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin1_PortOpenStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money100Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin2_PortOpenStatus);
                        }
                        if (currentCoinType == CoinDispensor.CoinType.Money500Type)
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.Coin3_PortOpenStatus);
                        }
                        mCoin_PortOpenError = !pIsPortOpenOk;

                    }
                }
            }


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
                        switch ((CoinDetailStatusType)statusCode)
                        {
                            case CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus:
                            case CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus:
                            case CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus:
                                mCoin_CheckForcoinAvailablityError = !isSuccess;
                                if (mCoin_CheckForcoinAvailablityError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;


                            case CoinDetailStatusType.Coin1_CommuniCationStatus:
                            case CoinDetailStatusType.Coin2_CommuniCationStatus:
                            case CoinDetailStatusType.Coin3_CommuniCationStatus:
                                mCoin_CommunicationError = !isSuccess;
                                if (mCoin_CommunicationError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_CommuniCationStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }


                                }
                                break;

                            case CoinDetailStatusType.Coin1_DeviceStatus:
                            case CoinDetailStatusType.Coin2_DeviceStatus:
                            case CoinDetailStatusType.Coin3_DeviceStatus:
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "CoinDispensor | SetDbErrorInfo", "Coin_DeviceStatus"
                                                                                                           + " 장비명:" + statusCode.ToString()
                                                                                                           + " 정상여부:" + isSuccess.ToString());
                                mCoin_DeviceError = !isSuccess;
                                if (mCoin_DeviceError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_DeviceStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;



                            case CoinDetailStatusType.Coin1_MotorProblemStatus:
                            case CoinDetailStatusType.Coin2_MotorProblemStatus:
                            case CoinDetailStatusType.Coin3_MotorProblemStatus:

                                mCoin_MotorProblemError = !isSuccess;
                                if (mCoin_MotorProblemError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_MotorProblemStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.Coin1_PartiallyReleasedStatus:
                            case CoinDetailStatusType.Coin2_PartiallyReleasedStatus:
                            case CoinDetailStatusType.Coin3_PartiallyReleasedStatus:
                                mCoin_PartiallyReleasedError = !isSuccess;
                                if (mCoin_PartiallyReleasedError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PartiallyReleasedStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.Coin1_PortOpenStatus:
                            case CoinDetailStatusType.Coin2_PortOpenStatus:
                            case CoinDetailStatusType.Coin3_PortOpenStatus:
                                mCoin_PortOpenError = !isSuccess;
                                if (mCoin_PortOpenError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PortOpenStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus:
                            case CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus:
                            case CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus:
                                mCoin_ReadJustShaft_CoinSizeVaries = !isSuccess;
                                if (mCoin_ReadJustShaft_CoinSizeVaries)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.Coin1_RejectCoinStatus:
                            case CoinDetailStatusType.Coin2_RejectCoinStatus:
                            case CoinDetailStatusType.Coin3_RejectCoinStatus:
                                mCoin_RejectCoinError = !isSuccess;
                                if (mCoin_RejectCoinError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_RejectCoinStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.Coin1_ShaftSensorStatus:
                            case CoinDetailStatusType.Coin2_ShaftSensorStatus:
                            case CoinDetailStatusType.Coin3_ShaftSensorStatus:
                                mCoin_ShaftSensorFailure = !isSuccess;
                                if (mCoin_ShaftSensorFailure)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_ShaftSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;
                            case CoinDetailStatusType.Coin1_PrismSensorStatus:
                            case CoinDetailStatusType.Coin2_PrismSensorStatus:
                            case CoinDetailStatusType.Coin3_PrismSensorStatus:
                                mCoin_PrismSensorError = !isSuccess;
                                if (mCoin_PrismSensorError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin1_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin2_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.Coin3_PrismSensorStatus)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;
                            case CoinDetailStatusType.MAX_50Alarm:
                            case CoinDetailStatusType.MAX_100Alarm:
                            case CoinDetailStatusType.MAX_500Alarm:
                                mIsMaxQtyError = !isSuccess;
                                if (mIsMaxQtyError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MAX_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.MIN_50Alarm:
                            case CoinDetailStatusType.MIN_100Alarm:
                            case CoinDetailStatusType.MIN_500Alarm:
                                mIsMinQtyError = !isSuccess;
                                if (mIsMinQtyError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.MIN_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                            case CoinDetailStatusType.NONE_50Alarm:
                            case CoinDetailStatusType.NONE_100Alarm:
                            case CoinDetailStatusType.NONE_500Alarm:
                                mIsNoneQtyError = !isSuccess;
                                if (mIsMinQtyError)
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Fail, statusCode);
                                    }

                                }
                                else
                                {
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_50Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_100Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, statusCode);
                                    }
                                    if ((CoinDetailStatusType)statusCode == CoinDetailStatusType.NONE_500Alarm)
                                    {
                                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, statusCode);
                                    }

                                }
                                break;

                        }
                    }
                    GetCoinDispensorDeveiceOpertationYn();
                }
                else
                {
                    mCoin_CheckForcoinAvailablityError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus);
                    }

                    mCoin_CommunicationError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CommuniCationStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CommuniCationStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CommuniCationStatus);
                    }

                    mCoin_DeviceError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_DeviceStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_DeviceStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_DeviceStatus);
                    }


                    mCoin_MotorProblemError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_MotorProblemStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_MotorProblemStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_MotorProblemStatus);
                    }

                    mCoin_PartiallyReleasedError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PartiallyReleasedStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PartiallyReleasedStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PartiallyReleasedStatus);
                    }

                    mCoin_PortOpenError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PortOpenStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PortOpenStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PortOpenStatus);
                    }


                    mCoin_ReadJustShaft_CoinSizeVaries = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus);
                    }

                    mCoin_RejectCoinError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_RejectCoinStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_RejectCoinStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_RejectCoinStatus);
                    }



                    mCoin_ShaftSensorFailure = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ShaftSensorStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ShaftSensorStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ShaftSensorStatus);
                    }

                    mCoin_PrismSensorError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PrismSensorStatus);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PrismSensorStatus);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PrismSensorStatus);
                    }

                    mIsNoneQtyError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_50Alarm);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_100Alarm);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_500Alarm);
                    }

                    mIsMaxQtyError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_50Alarm);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_100Alarm);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_500Alarm);
                    }

                    mIsMinQtyError = false;
                    if (currentCoinType == CoinType.Money50Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_50Alarm);
                    }
                    else if (currentCoinType == CoinType.Money100Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_100Alarm);
                    }
                    else if (currentCoinType == CoinType.Money500Type)
                    {
                        CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_500Alarm);
                    }

                    GetCoinDispensorDeveiceOpertationYn();

                }
            }


            public void SendAllDeviveOk()
            {
                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CheckForcoinAvailablityStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CheckForcoinAvailablityStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CheckForcoinAvailablityStatus);
                }

                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_CommuniCationStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_CommuniCationStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_CommuniCationStatus);
                }

                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_DeviceStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_DeviceStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_DeviceStatus);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_MotorProblemStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_MotorProblemStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_MotorProblemStatus);
                }

                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PartiallyReleasedStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PartiallyReleasedStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PartiallyReleasedStatus);
                }

                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PortOpenStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PortOpenStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PortOpenStatus);
                }



                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ReadJustShaft_CoinSizeVariesStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ReadJustShaft_CoinSizeVariesStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ReadJustShaft_CoinSizeVariesStatus);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_RejectCoinStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_RejectCoinStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_RejectCoinStatus);
                }




                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_ShaftSensorStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_ShaftSensorStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_ShaftSensorStatus);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin1_PrismSensorStatus);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin2_PrismSensorStatus);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.Coin3_PrismSensorStatus);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_50Alarm);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_100Alarm);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_500Alarm);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_50Alarm);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_100Alarm);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_500Alarm);
                }


                if (currentCoinType == CoinType.Money50Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_50Alarm);
                }
                else if (currentCoinType == CoinType.Money100Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC2, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_100Alarm);
                }
                else if (currentCoinType == CoinType.Money500Type)
                {
                    CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC3, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_500Alarm);
                }
            }

            private bool mIsMaxQtyError = false;
            public bool IsMaxQtyError
            {
                get { return mIsMaxQtyError; }
            }

            private bool mIsMinQtyError = false;
            public bool IsMinQtyError
            {
                get { return mIsMinQtyError; }
            }


            private bool mIsNoneQtyError = false;
            public bool IsNoneQtyError
            {
                get { return mIsNoneQtyError; }
            }



            public void setCash(int cash5000SettingQty, int cash1000SettingQty, int cash500SettingQty, int cash100SettingQty, int cash50SettingQty, int cash50MinQqty, int cash100MinQqty, int cash500MinQqty, int cash1000MinQqty, int cash5000MinQqty, int cash50MaxQqty, int cash100MaxQqty, int cash500MaxQqty, int cash1000MaxQqty, int cash5000MaxQqty)
            {

                if (NPSYS.Device.UsingSettingCoinCharger50 && currentCoinType == CoinDispensor.CoinType.Money50Type)
                {
                    if (cash50SettingQty <= 0)  // 동전이 없다면
                    {
                        if (mIsNoneQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.NONE_50Alarm);
                            mIsNoneQtyError = true;
                        }
                    }
                    else // 50원이 있다면
                    {
                        if (mIsNoneQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_50Alarm);
                            mIsNoneQtyError = false;
                        }

                    }
                    if (cash50SettingQty <= cash50MinQqty) // 최소수량 부족이면
                    {

                        if (mIsMinQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MIN_50Alarm);
                            mIsMinQtyError = true;
                        }
                    }
                    else // 50원이 있다면
                    {
                        if (mIsMinQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_50Alarm);
                            mIsMinQtyError = false;
                        }

                    }
                    if (cash50SettingQty >= cash50MaxQqty) // 최대수량 경고이면
                    {

                        if (mIsMaxQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MAX_50Alarm);
                            mIsMaxQtyError = true;
                        }
                    }
                    else // 50원이 있다면
                    {
                        if (mIsMaxQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_50Alarm);
                            mIsMaxQtyError = false;
                        }

                    }
                }

                if (NPSYS.Device.UsingSettingCoinCharger100 && currentCoinType == CoinDispensor.CoinType.Money100Type)
                {
                    if (cash100SettingQty <= 0)  // 동전이 없다면
                    {
                        if (mIsNoneQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.NONE_100Alarm);
                            mIsNoneQtyError = true;
                        }
                    }
                    else // 100원이 있다면
                    {
                        if (mIsNoneQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_100Alarm);
                            mIsNoneQtyError = false;
                        }

                    }
                    if (cash100SettingQty <= cash100MinQqty) // 최소수량 부족이면
                    {

                        if (mIsMinQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MIN_100Alarm);
                            mIsMinQtyError = true;
                        }
                    }
                    else // 100원이 있다면
                    {
                        if (mIsMinQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_100Alarm);
                            mIsMinQtyError = false;
                        }

                    }
                    if (cash100SettingQty >= cash100MaxQqty) // 최대수량 경고이면
                    {

                        if (mIsMaxQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MAX_100Alarm);
                            mIsMaxQtyError = true;
                        }
                    }
                    else // 100원이 있다면
                    {
                        if (mIsMaxQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_100Alarm);
                            mIsMaxQtyError = false;
                        }

                    }
                }


                if (NPSYS.Device.UsingSettingCoinCharger500 && currentCoinType == CoinDispensor.CoinType.Money500Type)
                {
                    if (cash500SettingQty <= 0)  // 동전이 없다면
                    {
                        if (mIsNoneQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.NONE_500Alarm);
                            mIsNoneQtyError = true;
                        }
                    }
                    else // 500원이 있다면
                    {
                        if (mIsNoneQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.NONE_500Alarm);
                            mIsNoneQtyError = false;
                        }

                    }
                    if (cash500SettingQty <= cash500MinQqty) // 최소수량 부족이면
                    {

                        if (mIsMinQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MIN_500Alarm);
                            mIsMinQtyError = true;
                        }
                    }
                    else // 500원이 있다면
                    {
                        if (mIsMinQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MIN_500Alarm);
                            mIsMinQtyError = false;
                        }

                    }
                    if (cash500SettingQty >= cash500MaxQqty) // 최대수량 경고이면
                    {

                        if (mIsMaxQtyError == false) // 기존에 에러가 없다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Fail, (int)CoinDetailStatusType.MAX_500Alarm);
                            mIsMaxQtyError = true;
                        }
                    }
                    else // 500원이 있다면
                    {
                        if (mIsMaxQtyError == true) // 기존에 에러였다면
                        {
                            CommProtocol.MakeDevice_RestfulStatus(CommProtocol.device.CC1, CommProtocol.DeviceStatus.Success, (int)CoinDetailStatusType.MAX_500Alarm);
                            mIsMaxQtyError = false;
                        }

                    }
                }

            }


        }
    }

}
