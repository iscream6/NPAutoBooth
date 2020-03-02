using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{

    public class KiccTs141
    {
        //KICCTS141적용

        // KICC 결제 모듈 LOAD
        [DllImport("KiccPos.dll", EntryPoint = "KLoad")]
        private static extern int KLoad(int Port, int Baudrate, byte[] pErrMsg);

        // KICC 결제 모듈 UNLOAD
        [DllImport("KiccPos.dll", EntryPoint = "KUnLoad")]
        private static extern void KUnLoad();


        //Public Declare Function KGetEvent Lib "KiccPos.DLL" _
        //     (ByRef CMD As Long, ByRef GCD As Long, ByRef JCD As Long, ByRef RCD As Long, _
        //      ByVal RData As String, ByVal RHexData As String) As Long
        [DllImport("KiccPos.DLL", EntryPoint = "KGetEvent", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int KGetEvent(ref int cmd, ref int gcd, ref int jcd, ref int rcd, byte[] rData, byte[] hexaData);


        [DllImport("KiccPos.DLL", EntryPoint = "KReqCmd", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int KReqCmd(int cmd, int gcd, int jcd, string sendData, byte[] pOut);

        private static int mStartSoundTick = 0;
        public static int StartSoundTick
        {
            set { mStartSoundTick = value; }
            get { return mStartSoundTick; }
        }
        private static int mPort = 0;
        public static int Port
        {
            set { mPort = value; }
            get { return mPort; }
        }

        private static int mBaudRate = 0;
        public static int BaudRate
        {
            set { mBaudRate = value; }
            get { return mBaudRate; }
        }
        public static bool Connect()
        {
            byte[] ErrMsg = new byte[4086];
            try
            {
                int result = KLoad(Port, BaudRate, ErrMsg);
                bool isConnect = false;
                if (result >= 0)
                {
                    isConnect = true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Connect", "접속오류" + Encoding.Default.GetString(ErrMsg));
                }
                return isConnect;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Connect", ex.ToString());
                return false;
            }
        }

        public static bool Disconnect()
        {
            try
            {
                KUnLoad();
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Disconnect", ex.ToString());
                return false;
            }
        }

        public static bool Approval(string pAmount, string pTax)
        {
            try
            {
                int cmd = 253;
                int gcd = 0;
                int jcd = 0;
                StringBuilder senddata = new StringBuilder();
                senddata.Append("D1");
                senddata.Append(" "); //wcc
                senddata.Append("".PadLeft(40, ' ')); //카드번호
                senddata.Append("00"); //할부
                senddata.Append(string.Empty.PadLeft(6, ' ')); //승인일시
                senddata.Append(string.Empty.PadLeft(12, ' ')); //승인번호
                senddata.Append(pAmount.PadLeft(8, ' ')); // 금액
                senddata.Append("0".PadLeft(8, ' ')); // 봉사료
                senddata.Append(pTax.PadLeft(8, ' ')); // VAT
                senddata.Append(string.Empty.PadLeft(20, ' ')); // VAT

                byte[] recvData = new byte[4086];
                int result = KReqCmd(cmd, gcd, jcd, senddata.ToString(), recvData);
                if (result >= 0)
                {
                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Approval", "[승인KReqCmd오류]" + Encoding.Default.GetString(recvData));
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Approval", ex.ToString());
                return false;
            }


        }

        public static bool Initilize()
        {
            try
            {
                int cmd = 253;
                int gcd = 0;
                int jcd = 0;
                string senddatabyte = "TM";
                byte[] recvData = new byte[4086];
                int result = KReqCmd(cmd, gcd, jcd, senddatabyte, recvData);
                if (result >= 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KiccTs141 | Initilize", "[카드기기취소동작성공]");
                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Approval", "[기기취소KReqCmd오류]" + Encoding.Default.GetString(recvData));
                    return false;
                }

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KiccTs141 | Initilize", ex.ToString());
                return false;
            }
        }


        //KICCTS141적용완료

    }
}
