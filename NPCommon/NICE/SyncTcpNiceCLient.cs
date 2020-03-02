using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace NPCommon.NICE
{
    public class SyncTcpNiceClient
    {
        private int dIndex = 1;
        private NetworkStream dNStream;
        private Socket client;
        private bool dLoopFlag;
        private bool dConnected;           // 서버연결상태
        private List<Byte[]> mSendData = new List<byte[]>();
        public delegate void EventReceivedBytes(object sender, byte[] pReceiveByte);
        public event EventReceivedBytes ReceivedBytes;
        public delegate void EvnetDisconnected(object sender, string pMessage);
        public event EvnetDisconnected Disconnected;
        public delegate void EventConnected(object sender);
        public event EventConnected Connected;
        public delegate void EventSendOK(int vIndex, string vMsg);
        public event EventSendOK SendOK;
        private System.Object lockThis = new System.Object();
        private DateTime dIngDate = new DateTime();

        public void SyncTcpNiceCLient()
        {
            dConnected = false;
            dIndex = 1;

        }

        public string Ip
        {
            set;
            get;
        }

        public int Port
        {
            set;
            get;
        }
        public string LoCalIp
        {
            set;
            get;
        }

        public bool isConnect
        {
            get { return dConnected; }
        }

        public enum DeviceType
        {
            /// <summary>
            /// 입차LPR
            /// </summary>
            INLPR,
            /// <summary>
            /// 출차전광판
            /// </summary>
            OUTLPR,
            INDISPLAY,
            OUTDISPLAY
        }
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
        public bool Connect(bool pUseAutoCheck)
        {

            try
            {
                //' 서버와 연결된 상태에서 다시 연결 안됨
                if (dConnected == false)
                {

                    dConnected = true;

                    LoCalIp = LocalIPAddress();


                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(Ip, Port);

                    dNStream = new NetworkStream(client);

                    dLoopFlag = true;

                    Thread dSendThread;
                    Thread dAcceptThread;

                    //'스레드로 처리할 함수를 지정합니다.
                    try
                    {
                        dSendThread = new Thread(new ThreadStart(SendStringLoop));
                        dSendThread.IsBackground = true;
                        dSendThread.Start();
                    }
                    catch
                    {
                    }

                    //'스레드로 처리할 함수를 지정합니다.
                    try
                    {
                        dAcceptThread = new Thread(new ThreadStart(ReceiveBytes));
                        dAcceptThread.IsBackground = true;
                        dAcceptThread.Start();
                    }
                    catch
                    {
                    }

                    //'스레드로 처리할 함수를 지정합니다.
                    if (pUseAutoCheck)
                    {
                        try
                        {
                            Thread dThread;
                            dThread = new Thread(new ThreadStart(AutoCheck));
                            dThread.IsBackground = true;
                            dThread.Start();
                        }
                        catch
                        {
                        }
                    }

                    Connected(this);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                SendOK(1, "TcpClient| Connect :" + ex.ToString());
                dConnected = false;
                dLoopFlag = false;
                client.Close();
                client = null;
                return false;
            }
        }
        public void Disconnect(string vMsg)
        {
            try
            {
                if (dConnected == false)
                {
                    return;
                }

                dConnected = false;

                dLoopFlag = false;
                try
                {
                    dNStream.Flush();
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "AA::" + ex.StackTrace);
                }


                try
                {
                    dNStream.Close();
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "BB::" + ex.StackTrace);

                }

                try
                {
                    dNStream = null;
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "ZZ::" + ex.StackTrace);

                }

                try
                {
                    client.Close();
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "CC::" + ex.StackTrace);
                }

                try
                {
                    client = null;
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "DD::" + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                SendOK(dIndex, "LL::" + ex.StackTrace);
            }
            Disconnected(this, vMsg);
        }

        public void TcpSend(byte[] pByte)
        {
            if (dConnected == true)
            {
                mSendData.Add(pByte);
            }

        }

        public void TcpSend(string pMessage)
        {
            if (dConnected == true)
            {
                mSendData.Add(Encoding.UTF8.GetBytes(pMessage));
            }

        }

        public void SendStringLoop()
        {
            String mStr = "a";

            try
            {
                while (dLoopFlag)
                {
                    if (dConnected == true && mSendData.Count > 0)
                    {

                        // BinaryFormatter mFormatter = new BinaryFormatter();
                        try
                        {
                            // String 전송
                            lock (mSendData)
                            {
                                if (mSendData[0] == null)
                                {
                                    SendOK(1, "에러1:msenddata[0] null" + "연결상태:" + client.Connected.ToString());
                                    mSendData[0] = Encoding.Default.GetBytes("TEST");
                                    SendOK(1, "전송데이터:" + Encoding.Default.GetString(mSendData[0]) + client.Connected.ToString());
                                }
                                else
                                {
                                    SendOK(1, "전송데이터:" + Encoding.Default.GetString(mSendData[0]) + client.Connected.ToString());
                                    client.Send(mSendData[0]);
                                }

                            }

                            mSendData.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            ClearSendData();
                            SendOK(1, "[SendObject::" + mStr + "][" + ex.Message + " " + ex.StackTrace + "]");
                            Disconnect("[SendObject::" + mStr + "][" + ex.StackTrace + "]");
                            throw ex;
                        }
                    }
                    else if (dConnected == false && mSendData.Count > 0)
                    {
                        mSendData.RemoveAt(0);
                    }



                    Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                SendOK(dIndex, "GG::" + ex.StackTrace);
            }
        }
        public void ClearSendData()
        {
            mSendData.Clear();
        }
        private int receiveLength = 0;
        public void ReceiveBytes()
        {
            while (dLoopFlag)
            {
                try
                {
                    client.ReceiveBufferSize = 1024;
                    byte[] receveBuffer = new byte[client.ReceiveBufferSize];
                    try
                    {
                        receiveLength = client.Receive(receveBuffer, SocketFlags.None);
                        if (receiveLength > 0)
                        {
                            byte[] returnData = new byte[receiveLength];
                            Array.Copy(receveBuffer, returnData, receiveLength);
                            ReceivedBytes(this, returnData);
                        }
                        //dIngDate = DateTime.Now;
                        //ReceivedBytes(this, NullRemover(receveBuffer));

                    }
                    catch (Exception ex)
                    {
                        SendOK(1, "[ReceiveBytes][네트워크 상태 이상]" + ex.ToString());
                        Disconnect("[ReceiveBytes][네트워크 상태 이상]" + ex.ToString());
                        break;
                    }


                    Thread.Sleep(300);
                }
                catch
                {

                    break;
                }
            }
            SendOK(1, "[ReceiveBytes][네트워크 상태 이상]" + dLoopFlag.ToString());
            Disconnect("[ReceiveBytes][네트워크 상태 이상]");
        }

        private byte[] NullRemover(byte[] DataStream)
        {
            int i;
            byte[] temp = new byte[client.ReceiveBufferSize];
            for (i = 0; i < client.ReceiveBufferSize - 1; i++)
            {
                if (DataStream[i] == 0x00) break;
                temp[i] = DataStream[i];
            }
            byte[] NullLessDataStream = new byte[i];
            for (i = 0; i < NullLessDataStream.Length; i++)
            {
                NullLessDataStream[i] = temp[i];
            }
            return NullLessDataStream;
        }
        public void AutoCheck()   // 서버 살아 있는지 여부
        {
            int mCnt = 0;
            while (dLoopFlag)
            {
                try
                {
                    mCnt += 1;

                    if (mCnt > 10 * 9 * 1)
                    {
                        mCnt = 0;
                        TcpSend("PING" + "\r" + "\n");        //' 클라이언트가 살아 있음을 서버에 알림... 1분 마다..
                    }

                    if (DateTime.Now > dIngDate.AddMinutes(5))      // 5분간 받는 객체가 없다면 서버가 다운된 걸로 간주
                    {
                        //'Thread.Sleep(0)
                        //'Disconnect("[AutoCheck][서버 반응 없음]")
                        //'Exit While
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    SendOK(dIndex, "Kk::" + ex.StackTrace);
                    break;
                }
            }
        }


    }
}
