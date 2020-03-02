using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace FadeFox.Network.Tcp
{
    class PingCheck
    {
        private int mPingTimeout = 120;
        private string mPingSendData = "Test";
        private string mCheckIp = string.Empty;
        private  int PingTimeout { set { mPingTimeout = value; } get { return mPingTimeout; } }
        private  string PingSendData { set { mPingSendData = value; } get { return mPingSendData; } }
        public string CheckIp { set { mCheckIp = value; } get { return mCheckIp; } }
        private bool misConnected = false;
        public bool IsConnected { set { misConnected = value; } get { return misConnected; } }
        /// <summary>
        /// 네트워크 상태확인
        /// </summary>
        /// <param name="sIP"></param>
        /// <returns></returns>
        public bool isNetworkConnect(string sIP)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;

            byte[] buffer = Encoding.ASCII.GetBytes(PingSendData);

            try
            {
                IPAddress ip = IPAddress.Parse(sIP);
                PingReply reply = pingSender.Send(ip, PingTimeout, buffer, options);

                return reply.Status == IPStatus.Success;
            }
            catch
            {

                return false;

            }
        }


    }
}
