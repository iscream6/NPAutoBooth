using FadeFox.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NPCommon.NICE
{
    //나이스연동

    public class NiceTcpClinet
    {
        private string mIp = string.Empty;
        public string Ip
        {
            set { mIp = value; }
            get { return mIp; }
        }
        private int mPort = 0;
        public int Port
        {
            set { mPort = value; }
            get { return mPort; }
        }
        private SyncTcpNiceClient mDeviceTcp = null;
        private Thread ConnectThread = null;
        private bool bForever = true;
        private bool mIsConnected = false;
        public ManualResetEvent ResetEventReceive = new ManualResetEvent(true);
        public ManualResetEvent ResetEventSend = new ManualResetEvent(true);
        public delegate void EventTcpData(NiceSetData pNiceSetData);
        public event EventTcpData Event_TcpData;

        public NiceTcpClinet()
        {
            NiceEventLoad();
            TextCore.ACTION(TextCore.ACTIONS.LPR, "NiceTcpClinet | NiceTcpClinet", "LPR 이벤트 로드");
        }
        /// <summary>
        /// LPR에서 통신열결하여 자료처리할준비를 쓰레드로 구현
        /// </summary>
        public void ThreadStart()
        {
            ConnectThread = new Thread(ReConnect);
            ConnectThread.IsBackground = true;
            ConnectThread.Start();
            TextCore.ACTION(TextCore.ACTIONS.LPR, "NiceTcpClinet | ThreadStart", "NICECLINET 동작시킴");
        }

        public void ThreadEnd()
        {
            bForever = false;
            try
            {
                mDeviceTcp.Disconnect("Disconnect");
                ConnectThread.Abort();
                TextCore.ACTION(TextCore.ACTIONS.LPR, "NiceTcpClinet | ThreadEnd", "NICECLINET 종료시킴");
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | ThreadEnd", ex.ToString());
            }
        }
        /// <summary>
        /// LPR Evnet구현부
        /// </summary>
        private void NiceEventLoad()
        {
            mDeviceTcp = new SyncTcpNiceClient();
            mDeviceTcp.Connected += new SyncTcpNiceClient.EventConnected(Connected);
            mDeviceTcp.Disconnected += new SyncTcpNiceClient.EvnetDisconnected(Disconnected);
            mDeviceTcp.ReceivedBytes += new SyncTcpNiceClient.EventReceivedBytes(ReceiveDataEvent);
            mDeviceTcp.SendOK += new SyncTcpNiceClient.EventSendOK(SendOK);
        }




        public void SendData(NiceSetData.Cmd pCmd, string pMessage)
        {

            try
            {

                ResetEventSend.WaitOne(2000);
                ResetEventSend.Reset();
                Thread.Sleep(50);
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NiceTcpClinet | SendData", "[나이스 전문전송]"
                                                                                 + " 전송커맨드:" + pCmd.ToString()
                                                                                 + " 전송데이터:" + pMessage.ToString());

                mDeviceTcp.TcpSend(pMessage);
                Thread.Sleep(50);


            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | Lpr_BarOpen", ex.ToString());
            }
            finally
            {
                ResetEventSend.Set();
            }

        }


        private void ReConnect()
        {
            while (bForever)
            {
                IsConnect();
                Thread.Sleep(10000);
            }
        }

        private void IsConnect()
        {
            try
            {
                if (mIsConnected == false)
                {
                    if (NPSYS.Device.gIsUseNiceDevice == false)
                    {
                        TextCore.DeviceError(TextCore.DEVICE.TCPIP, "NiceTcpClinet | IsConnect", "연결안됨");
                    }

                    NPSYS.Device.gIsUseNiceDevice = false;

                    mDeviceTcp.Ip = Ip;

                    mDeviceTcp.Port = Port;

                    mIsConnected = mDeviceTcp.Connect(true);

                }
                else
                {
                    NPSYS.Device.gIsUseNiceDevice = true;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | IsConnect", ex.ToString());
            }

        }

        void Connected(object sender)
        {
            try
            {
                TextCore.ACTION(TextCore.ACTIONS.LPR, "NiceTcpClinet | Connected", "[통신연결됨]");
                mIsConnected = true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | Connected", ex.ToString());
            }
        }

        void Disconnected(object sender, string mMsg)
        {
            try
            {
                TextCore.ACTION(TextCore.ACTIONS.LPR, "NiceTcpClinet | Disconnected", "[통신연결종료됨]");
                mIsConnected = false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | Disconnected", ex.ToString());
            }
        }

        void SendOK(int vIndex, string vMsg)
        {
        }

        private String gNPServerData = String.Empty;

        byte[] etx = { 0x0D, 0x0A };
        public List<byte> mReadBuffer = new List<byte>();
        /// <summary>
        /// 데이터 최초 수신
        /// </summary>
        /// <param name="vIndex"></param>
        /// <param name="mByte"></param>
        void ReceiveDataEvent(object sender, byte[] mByte)
        {

            try
            {

                ResetEventReceive.WaitOne(10000);
                ResetEventReceive.Reset();

                for (int i = 0; i < mByte.Length; i++)
                {
                    mReadBuffer.Add(mByte[i]);
                }

                string l_StrReceiveData = "";
                for (int i = 0; i < mReadBuffer.Count; i++)
                {
                    l_StrReceiveData += "[" + mReadBuffer[i].ToString("X2") + "]";
                }


                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NiceTcpClinet | ReceiveDataEvent", "나이스 데이터수신:" + Encoding.UTF8.GetString(mReadBuffer.ToArray()) + l_StrReceiveData);

                int l_etxIndex = 0; // 종료점
                do
                {
                    byte[] l_ReciveDataByte = mReadBuffer.ToArray();

                    l_etxIndex = TextCore.ByteSearch(l_ReciveDataByte, etx, 0);
                    if (l_etxIndex == -1)
                    {
                        break;
                    }
                    List<byte> mBodyData = mReadBuffer.GetRange(0, l_etxIndex);

                    string receiveData = Encoding.UTF8.GetString(mBodyData.ToArray());
                    mReadBuffer.RemoveRange(0, l_etxIndex + 2);
                    if (receiveData == "PONG" || receiveData == "PING")
                    {
                        return;
                    }
                    NiceSetData niceSetData = new NiceSetData();
                    niceSetData.SetDataParsing(receiveData);
                    Event_TcpData(niceSetData);
                } while (l_etxIndex != -1);

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NiceTcpClinet | CarReciveDataFromLPR", "예외사항:" + ex.ToString());
            }
            finally
            {
                ResetEventReceive.Set();
            }
        }


    }
}

//나이스연동완료