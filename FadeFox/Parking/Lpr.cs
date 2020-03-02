using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FadeFox.Network.Tcp;
namespace FadeFox.LPR
{
    /// <summary>
    /// LPR 이벤트처리
    /// </summary>
    public class Lpr
    {
        #region 변수선언
        private SendReceiveThreadSocket mLprSocket = null;
        private bool mIsLprConnected = false;
        public bool IsLprConnect
        {
            get { return mIsLprConnected; }
        }

        private ManualResetEvent SendLprData = new ManualResetEvent(true);
        #endregion

        #region 이벤트선언
        public delegate void SendCarData(string pServerIp, LprParsing.LprData pData);
        public event SendCarData EventSendCarData;
        public delegate void SendActionData(string pServerIp, string pData);
        public event SendActionData EventSendActionData;
        public delegate void SendErrorData(string pServerIp, string pData);
        public event SendErrorData EventSendErrorData;
        /// <summary>
        /// 연결종료 이벤트 사용안함
        /// </summary>
        /// <param name="pMessage"></param>
        public delegate void Disconnected(string pServerIp, string pMessage);
        /// <summary>
        /// 연결종료이벤트 사용안함
        /// </summary>
        public event Disconnected EventDisconnected;
        /// <summary>
        /// 연결이벤트 사용안함
        /// </summary>
        /// <param name="sender"></param>
        public delegate void Connected(string pServerIp,string pMessage);
        /// <summary>
        /// 연결이벤트 사용안함
        /// </summary>
        public event Connected EventConnected;

        #endregion
        private string mServerIp = string.Empty;
        public string ServerIp
        {
            set { mServerIp = value; }

        }

        private int mServerPort = 0;
        public int ServerPort
        {
            set { mServerPort = value; }
        }
        private LprParsing mParing = new LprParsing();
        private LprParsing.LprData mLprData = new  LprParsing.LprData();
        #region 함수
        public Lpr(string pServerIp, int pServerPort)
        {
            mServerIp = pServerIp;
            mServerPort = pServerPort;
            LprEventLoad();

        }
         
        public Lpr()
        {
            LprEventLoad();
        }
        /// <summary>
        /// 얼라이브 전송 및 리시브 체크 보낸값에 따라 일정주기로 데이터를 전송하고 일정주기동안 응답이 없으면 접속해지후 재접속함)
        /// </summary>
        /// <param name="pUseAliveSendData">server에 데이터를 전송할지 유무</param>
        /// <param name="pSendSecountTIme">server에 전송하는 시간간격(초)</param>
        /// <param name="pALiveSendData">server에 전송하는 데이터</param>
        /// <param name="pUseAliveReceiveData">server에서 전송받는시간 체크유무</param>
        /// <param name="pNotReceiveDIsconnectSecondTime">server에서 전송되는 값이 없을시 재접속하는 시간</param>
        public void SetAlive(bool pUseAliveSendData,int pSendSecountTIme, string pALiveSendData, bool pUseAliveReceiveData, int pNotReceiveDIsconnectSecondTime)
        {
            mLprSocket.SetAutoCheck(pUseAliveSendData, pSendSecountTIme, pALiveSendData, pUseAliveReceiveData, pNotReceiveDIsconnectSecondTime);
        }
        /// <summary>
        /// LPR에서 통신열결하여 자료처리할준비를 쓰레드로 구현
        /// </summary>
        public void LprThreadStart()
        {
            mLprSocket.ServerIp = mServerIp;
            mLprSocket.ServerPort = mServerPort;
            mLprSocket.ThreadStart(mServerIp, mServerPort, true);
            EventSendActionData(this.mServerIp, "Lpr | LprThreadStart| LPR 동작시킴");
        }

        public void LprThreadEnd()
        {
            try
            {
                EventSendActionData(this.mServerIp, "Lpr | LprThreadEnd |LPR 종료시킴");
                mLprSocket.ThreadEnd();
            }
            catch (Exception ex)
            {
                EventSendErrorData(this.mServerIp, "Lpr | LprThreadEnd | 오류:" + ex.ToString());
            }
        }
        /// <summary>
        /// LPR Evnet구현부
        /// </summary>
        private void LprEventLoad()
        {
            mLprSocket = new SendReceiveThreadSocket();
            mLprSocket.Connected += new SendReceiveThreadSocket.EventConnected(LprConnected);
            mLprSocket.Disconnected += new SendReceiveThreadSocket.EvnetDisconnected(LprDisconnected);
            mLprSocket.ReceivedBytes += new SendReceiveThreadSocket.EventReceivedBytes(CarReciveDataFromLPR);
            mLprSocket.SendActtionMessage += new SendReceiveThreadSocket.ActionMessage(SocketSendActionMessage);
            mLprSocket.SendErrorMessage += new SendReceiveThreadSocket.ErrorMessage(SocketSendErrorMessage);
        }
        /// <summary>
        /// LPR연결이벤트
        /// </summary>
        /// <param name="Sender"></param>
        void LprConnected(string pserverIp, object Sender)
        {
            try
            {
                EventConnected(this.mServerIp, "Lpr | LprConnected | LPR연결됨");
                mIsLprConnected = true;
            }
            catch (Exception ex)
            {
                EventSendErrorData(this.mServerIp, "Lpr | LprConnected | 오류:" + ex.ToString());
            }
        }
        /// <summary>
        /// LPR종료이벤트
        /// </summary>
        /// <param name="mMsg"></param>
        void LprDisconnected(string pserverIp, string mMsg)
        {
            try
            {
                EventDisconnected(this.mServerIp, "Lpr | LprDisconnected | LPR연결종료됨");
                mIsLprConnected = false;
            }
            catch (Exception ex)
            {
                EventSendErrorData(this.mServerIp, "Lpr | LprDisconnected | 오류:" + ex.ToString());
            }
        }
        /// <summary>
        /// 일반적인 데이타내용 전송
        /// </summary>
        /// <param name="pMesagess"></param>
        void SocketSendActionMessage(string pserverIp, string pMesagess)
        {
            EventSendActionData(this.mServerIp, pMesagess);
        }
        /// <summary>
        /// 에러이벤트 전송
        /// </summary>
        /// <param name="pMessage"></param>
        void SocketSendErrorMessage(string pserverIp, string pMessage)
        {
            EventSendErrorData(this.mServerIp, pMessage);
        }

        /// <summary>
        /// 채널넘버를 통해 차단기 오픈
        /// </summary>
        /// <param name="pChannelNumber"></param>
        public void Lpr_BarOpen(string pChannelNumber)
        {

            try
            {
                SendLprData.WaitOne(2);
                SendLprData.Reset();
                string sBarOpen = "BAR_OPEN_" + pChannelNumber;
                Thread.Sleep(100);
                mLprSocket.TcpSend(sBarOpen);
                Thread.Sleep(100);
                 EventSendActionData(this.mServerIp, "Lpr | Lpr_BarOpen |차단기열기명령내림:"+sBarOpen);
            }
            catch (Exception ex)
            {
                EventSendErrorData(this.mServerIp, "Lpr | Lpr_BarOpen | 오류:" + ex.ToString());
            }
            finally
            {
                SendLprData.Set();
            }

        }







        /// <summary>
        /// LPR로부터 받는 최초 수신부
        /// </summary>
        /// <param name="vIndex"></param>
        /// <param name="pByte"></param>
        void CarReciveDataFromLPR(string pserverIp, object sender, Byte[] pByte)
        {
            EventSendActionData(this.mServerIp, "Lpr | CarReciveDataFromLPR | 자료 수신:" + Encoding.Default.GetString(pByte));
            mLprData.Clear();
            mParing.ParsingLprData(this.mLprSocket.LoCalIp, this.mServerIp, ref mLprData, pByte);
            EventSendCarData(this.mServerIp,mLprData);
        }




        #endregion


    }
}
