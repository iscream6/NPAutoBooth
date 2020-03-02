using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;


namespace FadeFox.Network.Tcp
{
    /// <summary>
    /// 쓰레드를 통해 연결을 유지하고 쓰레드를 통해 데이터를 보내고 쓰레드를 통해 데이터를 받는 소켓 
    /// 얼라이브 기능있음(동기쓰레드 통신방식)
    /// </summary>
    public class SendReceiveThreadSocket
    {
        private NetworkStream mStream;
        private Socket mClient;
        private bool mThreadLoopFlag; // 쓰레드 유지여부 true면 send,receive,autocheck 쓰레드를 유지한다.
        private bool mIsConnected;           // 서버연결상태
        public delegate void EventReceivedBytes(string pServerIp, object sender, byte[] pReceiveByte);
        public event EventReceivedBytes ReceivedBytes;
        public delegate void EvnetDisconnected(string pServerIp, string pMessage);
        public event EvnetDisconnected Disconnected;
        public delegate void EventConnected(string pServerIp,object sender);
        public event EventConnected Connected;
        private System.Object lockThis = new System.Object();

        private Thread mSendThread = null; // 전송쓰레드
        private Thread mConnectThread = null; // 연결쓰레드
        private Thread mReceiveThread = null; // 응답쓰레드
        private Thread mAutocheckThread = null; // server와 통신유지
        /// <summary>
        /// 소켓연결 쓰레드를 계속적으로 사용할지유무
        /// </summary>
        private bool mConnectLoop = false;
        /// <summary>
        /// 동작관련 메세지
        /// </summary>
        /// <param name="message"></param>
        public delegate void ActionMessage(string pServerIp, string message);
        /// <summary>
        /// 일반동작관련 이벤트내용전달
        /// </summary>
        public event ActionMessage SendActtionMessage;
        /// <summary>
        /// 에러메세지 일떄만
        /// </summary>
        /// <param name="message"></param>
        public delegate void ErrorMessage(string pServerIp, string message);
        /// <summary>
        /// 에러메세지 동작관련 이벤트 내용전달
        /// </summary>
        public event ErrorMessage SendErrorMessage;

        private List<byte[]> mListSendData = new List<byte[]>();
        /// <summary>
        /// 서버에서 전송을 받은날짜
        /// </summary>
        private DateTime mRecieveDate = DateTime.Now;
        

        public SendReceiveThreadSocket()
        {
            mIsConnected = false;

        }
        private string mServerIp = string.Empty;
        private int mServerPort = 0;

        /// <summary>
        /// 
        /// </summary>
        public string ServerIp
        {

            set { mServerIp = value; }
            get { return mServerIp; }
        }

        public int ServerPort
        {
            set { mServerPort = value; }
            get { return mServerPort; }
        }

        public string LoCalIp
        {
            set;
            get;
        }

        public bool isConnect
        {
            get { return mIsConnected; }
        }
        private bool mIsAutoCheck = false;
        /// <summary>
        /// alive데이터를 Server에 전송할지 여부
        /// </summary>
        private bool mUseAliveSendData = false;
        /// <summary>
        /// 클라이언트에서 센터로 센드보내는시가
        /// </summary>
        private int mSendSecountTIme = 0;
        /// <summary>
        /// alive데이터 내용
        /// </summary>
        private string mAliveSendData = string.Empty;
        /// <summary>
        /// 리시브데이타로 데이터 연결중인지 확인할지 여부
        /// </summary>
        private bool mUseAliveReceiveData = false;
        /// <summary>
        /// 리스브데이터가 없을떄 연결을 끊는시간 10이면 10초
        /// </summary>
        private int mNotReceiveDisconnectedTime = 0;

        public void SetAutoCheck(bool pUseAliveSendData,int pSendSecountTIme, string pALiveSendData, bool pUseAliveReceiveData, int pNotReceiveDIsconnectSecondTime)
        {
            mUseAliveSendData = pUseAliveSendData;
            mSendSecountTIme= pSendSecountTIme;
            mAliveSendData = pALiveSendData;
            mUseAliveReceiveData = pUseAliveReceiveData;

            mNotReceiveDisconnectedTime = pNotReceiveDIsconnectSecondTime;
        }

        /// <summary>
        /// 쓰레드 소켓클라이언트 연결시작
        /// </summary>
        /// <param name="pIp"></param>
        /// <param name="pPort"></param>
        /// <param name="pActionAutocheck"></param>
        /// <returns></returns>
        public bool ThreadStart(string pIp, int pPort, bool pActionAutocheck)
        {
            try
            {
                mServerIp = pIp;
                mServerPort = pPort;
                mConnectLoop = true;
                mThreadLoopFlag = true;
                mSendThread = new Thread(SendLoop);
                mSendThread.IsBackground = true;
                mSendThread.Start();
                mConnectThread = new Thread(Connect);
                mConnectThread.IsBackground = true;
                mConnectThread.Start();
                mReceiveThread = new Thread(Received);
                mReceiveThread.IsBackground = true;
                mReceiveThread.Start();
                mIsAutoCheck = pActionAutocheck;
                if (mIsAutoCheck)
                {
                    mAutocheckThread = new Thread(AliveCheck);
                    mAutocheckThread.IsBackground = true;
                    mAutocheckThread.Start();
                }
                SendActtionMessage( this.mServerIp,"[SendReceiveThreadSocket | ThreadStart]  IP:" + mServerIp);
                return true;

            }
            catch (Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | ThreadStart]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 클라이언트에서 서버로 연결함수
        /// </summary>
        private void Connect()
        {

            while (mConnectLoop)
            {
                try
                {
                    if (mIsConnected == false) // 비연결상태
                    {
                        if (mClient != null)
                        {
                            mClient = null;
                        }
                        mRecieveDate = DateTime.Now;
                        LoCalIp = LocalIPAddress();
                        mClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        mClient.Connect(mServerIp, mServerPort);
                        if (mStream != null)
                        {
                            mStream = null;
                        }
                        mStream = new NetworkStream(mClient);
                        mIsConnected = true;
                        mThreadLoopFlag = true;
                        Connected(this.mServerIp,this);
                    }
                    System.Threading.Thread.Sleep(5000);
                }
                catch (Exception ex)
                {
                    SendErrorMessage(this.mServerIp,"[SendReceiveThreadSocket | Connect]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                }
            }

        }

        private string LocalIPAddress()
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



        /// <summary>
        /// 데이터가 있으면 쓰레드를 이용하여 전송한다.
        /// </summary>
        private void SendLoop()
        {

            while (mThreadLoopFlag)
            {
                try
                {
                    if (mIsConnected == true && mListSendData.Count > 0)
                    {

                        // BinaryFormatter mFormatter = new BinaryFormatter();
                        try
                        {
                            // String 전송
                            lock (mListSendData)
                            {
                                if (mListSendData[0] == null)
                                {
                                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | SendLoop]  IP:" + mServerIp + " [오류원인]" + "msenddata[0]" + "연결상태:" + mClient.Connected.ToString());
                                    mListSendData[0] = Encoding.Default.GetBytes("TEST");
                                }
                                else
                                {
                                    SendActtionMessage(this.mServerIp, "전송데이터:" + Encoding.Default.GetString(mListSendData[0]));
                                    mClient.Send(mListSendData[0]);
                                }

                            }

                            mListSendData.RemoveAt(0);
                        }
                        catch (Exception ex)
                        {
                            mListSendData.Clear();
                            Disconnect("[SendReceiveThreadSocket | SendLoop]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                            SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | ThreadStart]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                            // throw ex;
                        }
                    }
                    else if (mIsConnected == false && mListSendData.Count > 0)
                    {
                        mListSendData.RemoveAt(0);
                    }

                    Thread.Sleep(10);
                }
                catch (Exception exp)
                {
                    Disconnect("[SendReceiveThreadSocket | SendLoop]  IP:" + mServerIp + " [오류원인exp]" + exp.ToString());
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | SendLoop]  IP:" + mServerIp + " [오류원인exp]" + exp.ToString());
                }

            }
        }
        /// <summary>
        /// 통신종료 & 쓰레드종료
        /// </summary>
        public void ThreadEnd()
        {
            mConnectLoop = false;
            mThreadLoopFlag = false;
            try
            {
                SendActtionMessage(this.mServerIp, "[SendReceiveThreadSocket | ThreadEnd]  IP:" + mServerIp + " [정상종료시도]");
                mSendThread.Abort();
                mConnectThread.Abort();
                mReceiveThread.Abort();
                if (mIsAutoCheck)
                {
                    mAutocheckThread.Abort();
                }
                Disconnect("[SendReceiveThreadSocket | ThreadEnd]  IP:" + mServerIp + " [정상종료]");
                SendActtionMessage(this.mServerIp, "[SendReceiveThreadSocket | ThreadEnd]  IP:" + mServerIp + " [정상종료됨]");
            }
            catch (Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | ThreadEnd]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 연결종료
        /// </summary>
        /// <param name="pMessage"></param>
        public void Disconnect(string pMessage)
        {
            try
            {
                if (mIsConnected == false)
                {
                    return;
                }

                mIsConnected = false;

                try
                {
                    mStream.Flush();
                }
                catch (Exception ex1)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex1]" + ex1.ToString());
                }


                try
                {
                    mStream.Close();
                }
                catch (Exception ex2)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex2]" + ex2.ToString());
                }

                try
                {
                    mStream = null;
                }
                catch (Exception ex3)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex3]" + ex3.ToString());
                }

                try
                {
                    mClient.Close();
                }
                catch (Exception ex4)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex4]" + ex4.ToString());
                }

                try
                {
                    mClient = null;
                }
                catch (Exception ex5)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex5]" + ex5.ToString());
                }
            }
            catch (Exception ex6)
            {
                SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Disconnect]  IP:" + mServerIp + " [오류원인ex6]" + ex6.ToString());
            }
            Disconnected(this.mServerIp, pMessage);
        }

        public void TcpSend(byte[] pByte)
        {
            if (mIsConnected == true)
            {
                mListSendData.Add(pByte);
            }

        }

        public void TcpSend(string pMessage)
        {
            if (mIsConnected == true)
            {
                mListSendData.Add(Encoding.Default.GetBytes(pMessage));
            }

        }



        private int receiveLength = 0;
        /// <summary>
        /// 리시브처리
        /// </summary>
        public void Received()
        {
            while (mThreadLoopFlag)
            {
                try
                {
                    if (mClient == null || !isConnect)
                    {
                        continue;
                    }
                    try
                    {

                        mClient.ReceiveBufferSize = 1024;
                        byte[] receveBuffer = new byte[mClient.ReceiveBufferSize];
                        receiveLength = mClient.Receive(receveBuffer, SocketFlags.None);
                        if (receiveLength > 0)
                        {
                            byte[] returnData = new byte[receiveLength];
                            Array.Copy(receveBuffer, returnData, receiveLength);
                            mRecieveDate = DateTime.Now;
                            ReceivedBytes(this.mServerIp, this, returnData);
                        }

                    }
                    catch (Exception ex)
                    {
                        SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Received]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                        Disconnect("[SendReceiveThreadSocket | Received]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                    }


                    Thread.Sleep(300);
                }
                catch (Exception exp)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | Received]  IP:" + mServerIp + " [오류원인exp]" + exp.ToString());
                    Disconnect("[SendReceiveThreadSocket | Received]  IP:" + mServerIp + " [오류원인ex]" + exp.ToString());
                    break;
                }
            }
        }
        /// <summary>
        /// 서버와 Alive체크기능
        /// </summary>
        public void AliveCheck()   // 서버 살아 있는지 여부
        {
            int timeCheck = 0; // mSendSecountTIme값이 되면 서버연결종료
            while (mThreadLoopFlag)
            {
                try
                {
                    if (!mIsConnected)
                    {
                        continue;
                    }
                    timeCheck += 1;

                    if (timeCheck > mSendSecountTIme)  // 30초마다 체크
                    {
                        timeCheck = 0;
                        TcpSend(mAliveSendData);        //' 클라이언트가 살아 있음을 서버에 알림... 1분 마다..
                    }
                    if (mUseAliveReceiveData)
                    {
                        if (DateTime.Now > mRecieveDate.AddSeconds(mNotReceiveDisconnectedTime))   //설정한 시간까지 데이터가 없다면
                        {
                            mRecieveDate = DateTime.Now;
                            Disconnect("[SendReceiveThreadSocket | AutoCheck]  IP:" + mServerIp + " [오류원인]" + "시간내 응답없음");
                            
                        }
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    SendErrorMessage(this.mServerIp, "[SendReceiveThreadSocket | AutoCheck]  IP:" + mServerIp + " [오류원인ex]" + ex.ToString());
                    break;
                }
            }
        }




    }
}
