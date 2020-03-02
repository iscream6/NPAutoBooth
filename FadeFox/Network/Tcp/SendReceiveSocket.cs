using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using FadeFox.Text;



namespace FadeFox.Network.Tcp
{
    /// <summary>
    /// 동기화 방식으로 데이터를 전송하고 리스브받는다.
    /// </summary>
    public class SendReceiveSocket
    {
        private const int _MAX_SIZE_ = 1024;
        private PingCheck mPingCheck = new PingCheck();

        /// <summary>
        /// 메인 Server 연결생성(Connected)
        /// </summary>
        /// <returns></returns>
        private Socket getConnection(string pServerIp, string pServerPort)
        {
            bool isPing = mPingCheck.isNetworkConnect(pServerIp);
            if (isPing == false)
            {
                Text.TextCore.INFO(Text.TextCore.INFOS.PROGRAM_ERROR, "SendReceiveSocket | getConnection", "PingCheck오류:" + pServerIp);
                return null;
            }
            int connectionRetry = 2;    //  Retry Times

            IPEndPoint ipe = null;
            Socket s = null;
            try
            {
                ipe = new IPEndPoint(IPAddress.Parse(pServerIp), Int32.Parse(pServerPort));
                int retry = 0;
                while (retry < connectionRetry)
                {
                    Socket tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    tempSocket.SendTimeout = 5000;
                    tempSocket.ReceiveTimeout = 7000;
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
                Text.TextCore.INFO(Text.TextCore.INFOS.PROGRAM_ERROR, "SendReceiveSocket | getConnection", ex.ToString());
                s = null;
            }
            return s;
        }





        /// <summary>
        /// 메세지를 전송하고 응답메세지를 수신한다
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public byte[] sendMessage(string pServerIp, string pServerPort, string message)
        {
            Socket s = null;    // 주서버 소켓

            StringBuilder recvMessage = null;

            try
            {
                s = getConnection(pServerIp, pServerPort);
                if (s == null)
                    return null;


                // 메세지 전송
                byte[] sendData = Encoding.Default.GetBytes(message.ToString());
                s.Send(sendData);

                // 메세지 수신
                s.ReceiveBufferSize = 1000;
                byte[] receveBuffer = new byte[s.ReceiveBufferSize];
                int receiveLength = s.Receive(receveBuffer, SocketFlags.None);
                if (receiveLength > 0)
                {
                    byte[] returnData = new byte[receiveLength];
                    Array.Copy(receveBuffer, returnData, receiveLength);
                    return returnData;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SendReceiveSocket | sendMessage", ex.ToString());
                recvMessage = new StringBuilder();
                recvMessage.Append("");
            }
            finally
            {
                if (s != null)
                    s.Close();
            }

            return null;
        }
    }

}
