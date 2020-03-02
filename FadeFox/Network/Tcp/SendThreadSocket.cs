using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace FadeFox.Network.Tcp
{
    /// <summary>
    /// 쓰레드를 통해 데이터가 들어올떄마다 접속을해서 데이터 전송후 접속을 종료하는 클라이언트 소켓
    /// (동기쓰레드 통신방식)
    /// </summary>
    public class SendThreadSocket
    {
        #region 이벤트정리
        /// <summary>
        /// 동작관련 메세지
        /// </summary>
        /// <param name="message"></param>
        public delegate void ActionMessage(string pServerIp,string message);
        public event ActionMessage SendActtionMessage;
        /// <summary>
        /// 에러메세지 일떄만
        /// </summary>
        /// <param name="message"></param>
        public delegate void ErrorMessage(string pServerIp, string message);
        public event ErrorMessage SendErrorMessage;

        #endregion

        #region 지역변수정리
        private List<byte[]> mListSendData = new List<byte[]>(); // 전송받을 데이터를 모아두는곳
        private Thread SendThread = null; // 전송쓰레드
        private string mServerIp = string.Empty;
        private int mServerPort = 0;
        private string mDeviceName = string.Empty;
        private PingCheck mPingCheck = new PingCheck();
        /// <summary>
        /// 쓰레드를 계속 사용할지 유무
        /// </summary>
        private bool mIsThreadContinue = false;
        #endregion

        #region property
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
        #endregion





        /// <summary>
        /// 외부에서 데이터전송시 사용
        /// </summary>
        /// <param name="pMessage"></param>
        public void TcpSend(byte[] pMessage)
        {
            mListSendData.Add(pMessage);
        }
        /// <summary>
        /// 외부에서 데이터전송시 사용
        /// </summary>
        /// <param name="pMessage"></param>
        public void TcpSend(string pMessage)
        {
            mListSendData.Add(Encoding.Default.GetBytes(pMessage));
        }
        /// <summary>
        /// 통신 쓰레드 시작 / 연결 및 
        /// </summary>
        /// <param name="pIp"></param>
        /// <param name="pPort"></param>
        /// <returns></returns>
        public bool ThreadStart(string pIp, int pPort)
        {
            try
            {
                mServerIp = pIp;
                mServerPort = pPort;
                mIsThreadContinue = true;
                SendThread = new Thread(SendLoop);
                SendThread.IsBackground = true;
                SendThread.Start();
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | ThreadStart | 동작성공");
                return true;
            }
            catch(Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendThreadSocket | ThreadStart ] [오류원인]:" + ex.ToString());
                throw  ex;
            }
        }

        private void SendLoop()
        {
            while (mIsThreadContinue)
            {
                if (mListSendData.Count > 0 && mListSendData != null)
                {
                    lock (mListSendData)
                    {
                        bool isSendDataSuccess = sendMessage(mListSendData[0]);
                        if (isSendDataSuccess)
                        {
                            mListSendData.RemoveAt(0);
                        }

                    }
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        public void ThreadEnd()
        {
            mIsThreadContinue = false;
            try
            {
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | ThreadEnd]" + "종료시도");
                SendThread.Abort();
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | | ThreadEnd]" + "종료완료");
            }
            catch (Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendThreadSocket |  ThreadEnd ] [오류원인ex]" + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// 메인 Server 연결생성(Connected)
        /// </summary>
        /// <returns></returns>
        private Socket getConnection()
        {
            int connectionRetry = 2;    //  Retry Times

            IPEndPoint ipe = null;
            Socket s = null;
            try
            {
                if (!mPingCheck.isNetworkConnect(mServerIp))
                {
                    if (!mPingCheck.isNetworkConnect(mServerIp))
                    {
                        SendErrorMessage(this.mServerIp, "[SendThreadSocket | getConnection ] [오류원인] 핑체크 오류");
                        mListSendData.Clear();
                        return null;
                    }

                }
                ipe = new IPEndPoint(IPAddress.Parse(mServerIp), mServerPort);
                int retry = 0;
                while (retry < connectionRetry)
                {
                    Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    tempSocket.Connect(ipe);
                    if (tempSocket.Connected)
                    {
                        s = tempSocket;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                s = null;
                throw ex;
            }
            return s;
        }





        /// <summary>
        /// 메세지를 전송하고 응답메세지를 수신한다
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool sendMessage(string message)
        {
            Socket s = null;    // 주서버 소켓


            try
            {
                s = getConnection();
                
                if (s == null)
                {
                    SendErrorMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] [오류원인]" + "소켓오류:소켓 null");
                    return false;
                }
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] 서버 전송데이타 전송시도: " + message);
                // 메세지 전송
                s.Send(Encoding.Default.GetBytes(message.ToString()));
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] 서버 전송데이타 전송완료: " + message);
                //// 메세지 수신


            }
            catch (Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] [오류원인 ex]: " + ex.ToString());
                return false;

            }
            finally
            {
                if (s != null)
                    s.Close();
            }

            return true;
        }


        private bool sendMessage(byte[] message)
        {
            Socket s = null;    // 주서버 소켓


            try
            {

                s = getConnection();
                if (s == null)
                {
                    SendErrorMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] [오류원인]" + "소켓오류:소켓 null");
                    return false;
                }

                SendActtionMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] 서버 전송데이타 전송시도: " + Encoding.Default.GetString(message));
                // 메세지 전송
                s.Send(message);
                SendActtionMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] 서버 전송데이타 전송완료: " + Encoding.Default.GetString(message));
                //// 메세지 수신


            }
            catch (Exception ex)
            {
                SendErrorMessage(this.mServerIp, "[SendThreadSocket | sendMessage ] [오류원인 ex] " + ex.ToString());
                return false;

            }
            finally
            {
                if (s != null)
                    s.Close();
            }

            return true;
        }

    }
}
