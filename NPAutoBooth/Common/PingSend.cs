using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using FadeFox.Database;
using System.Windows.Forms;
using NPAutoBooth.UI;
using FadeFox.Text;
using NPCommon;

namespace NPAutoBooth.Common
{
    public static class PingSend
    {
        static int _PingTimeout = 120;
        static string _PingSendData = "Test";

        private static int PingTimeout { set { _PingTimeout = value; } get { return _PingTimeout; } }
        private static string PingSendData { set { _PingSendData = value; } get { return _PingSendData; } }

        /// <summary>
        /// 네트워크 상태확인
        /// </summary>
        /// <param name="sIP"></param>
        /// <returns></returns>
        public static bool isNetworkConnect(string sIP)
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

        public static void JungangpanaLiveCheck(Object state)
        {
            if (NPSYS.g_Autoboothenable == false)
            {
                return;
            }

           if (PingSend.isNetworkConnect("127.0.0.1"))
            {
                NPSYS.Device.gIsAliveJunganpan = true;
            }
            else
            {
                NPSYS.Device.gIsAliveJunganpan = false;
                NPSYS.Device.LprDisplayErrorMessage = "전광판 연결안됨";
            }
        }


    }
}
