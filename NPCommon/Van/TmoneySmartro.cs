using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    public class TmoneySmartro
    {

        #region API 정의 관련

        // typedef int(__stdcall *SMT_S_ConnSndRcv)( BYTE  *szIP, short nPort, BYTE  *szSnd,	 int iTimeOut, BYTE  *szRcv);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_S_ConnSndRcv", CharSet = CharSet.Ansi)]
        private static extern int SMT_S_ConnSndRcv(byte[] ip, int port, byte[] send_data, int timeout, byte[] recv_data);

        // typedef int(__stdcall *SMT_Dongle_Start)(int iPortNum, long lBaud);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_Dongle_Start", CharSet = CharSet.Ansi)]
        private static extern int SMT_Dongle_Start(int port, int baud);

        // typedef int(__stdcall *SMT_Dongle_Stop)(void);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_Dongle_Stop", CharSet = CharSet.Ansi)]
        private static extern int SMT_Dongle_Stop();

        //// typedef int (__stdcall *SMT_Get_Sign_Screen)( BYTE  *cpWorkingKey, BYTE cKeyIndex, long lAmount, BYTE  *ucpSignData, BYTE  *ucpPadVersion, BYTE  *ucpHashData, BYTE  *ucpImgFileNm);
        //[DllImport("SmartroSign.dll", EntryPoint = "SMT_Get_Sign_Screen", CharSet = CharSet.Ansi)]
        //private static extern int SMT_Get_Sign_Screen(byte[] workingkey, byte keyindex, int amount, byte[] signdata, byte[] padversion, byte[] hashdata, byte[] filepath);

        // typedef int (__stdcall *SMT_Dongle_Initial)( int iFlag, BYTE *ucpSignpadInfo);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_Dongle_Initial", CharSet = CharSet.Ansi)]
        private static extern int SMT_Dongle_Initial(int flag, byte[] signpadinfo);

        // typedef int(__stdcall *SMT_TMoney_PSamInfo)( BYTE *ucpData, int iLength, BYTE *ucpSendData,int iTimeout);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_TMoney_PSamInfo", CharSet = CharSet.Ansi)]
        private static extern int SMT_TMoney_PSamInfo(byte[] recv_data, int length, byte[] send_data, int timeout);

        // typedef int(__stdcall *SMT_TMoney_Balance)( BYTE *ucpData, int iLength, BYTE *ucpSendData,int iTimeout);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_TMoney_Balance", CharSet = CharSet.Ansi)]
        private static extern int SMT_TMoney_Balance(byte[] recv_data, int length, byte[] senddata, int timeout);

        // typedef int(__stdcall *SMT_TMoney_Pay)( BYTE *ucpData, int iLength, BYTE *ucpSendData);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_TMoney_Pay", CharSet = CharSet.Ansi)]
        private static extern int SMT_TMoney_Pay(byte[] recv_data, int length, byte[] senddata);

        // typedef int(__stdcall *SMT_TMoney_Log_Del)( BYTE *ucpData, int iLength, BYTE *ucpSendData);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_TMoney_Log_Del", CharSet = CharSet.Ansi)]
        private static extern int SMT_TMoney_Log_Del(byte[] recv_data, int length, byte[] senddata);

        // typedef int(__stdcall *SMT_TMoney_Req_TR)( BYTE *ucpData, int iLength, BYTE *ucpSendData,int iTimeout);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_TMoney_Req_TR", CharSet = CharSet.Ansi)]
        private static extern int SMT_TMoney_Req_TR(byte[] recv_data, int length, byte[] senddata, int timeout);

        #endregion

        #region 변수 정의 관련

        /// <summary>
        /// 단말기 CAT ID
        /// </summary>
        public string pvs_catid = "1114913132";

        /// <summary>
        /// 단말기 Psam ID
        /// </summary>
        private string pvs_psamid = "";

        /// <summary>
        /// 단말기 일련번호 정보
        /// </summary>
        private string pvs_dongleinfo = "";

        /// <summary>
        /// 밴사 서버 IP (테스트 모드)
        /// </summary>
        public string pvs_vanip = "211.192.50.211";

        /// <summary>
        /// 밴사 서버 IP (테스트 모드)
        /// </summary>
        public int pvi_vanport = 5500;

        #endregion

        //[DllImport("VMdll.dll", EntryPoint = "POLL_A")]
        //private static extern bool POLL_A();

        //[DllImport("VMdll.dll", EntryPoint = "SetDongleParentHwnd")]
        //private static extern void SetDongleParentHwnd(IntPtr hWnd);

        //[DllImport("VMdll.dll", EntryPoint = "DongleConnect")]
        //private static extern bool DongleConnect(char connect, byte[] pStrPort, byte[] pStrBaud);
        //[DllImport("VMdll.dll", EntryPoint = "TMONEY_ReadBalance")]
        ////private static extern ulong TMONEY_ReadBalance([MarshalAs(UnmanagedType.LPArray)] byte[] Data, int size);
        //private static extern int TMONEY_ReadBalance(byte[] Data, int size);

        //[DllImport("VMdll.dll", EntryPoint = "WITCOM_ReadCSN")]
        //private static extern ulong WITCOM_ReadCSN(byte[] Data, int size);

        //// extern "C" __declspec(dllimport) VOID _stdcall COM_ComTimeout(DWORD nTimeout);
        //[DllImport("VMdll.dll", EntryPoint = "COM_ComTimeout")]
        //private static extern void COM_ComTimeout(int pmillSecond);





        //[DllImport("VMdll.dll", EntryPoint = "BEEP_Set")]
        //private static extern void BEEP_Set(char nOnOff);




        //// 원형 extern "C" __declspec(dllimport) VOID _stdcall SET_TerminalID(char *pID);  SET_TerminalID((LPSTR)(LPCSTR)m_strInfo);

        //[DllImport("VMdll.dll", EntryPoint = "SET_TerminalID")]
        //private static extern void SET_TerminalID(byte[] pID);


        //// 원형 extern "C" __declspec(dllimport) VOID _stdcall CARD_Timeout(unsigned char nTimeout);  	CARD_Timeout(1);		// 100MS 단위
        //[DllImport("VMdll.dll", EntryPoint = "CARD_Timeout")]
        //private static extern void CARD_Timeout(char pID);

        //// 원형 extern "C" __declspec(dllexport) void _stdcall SET_Time(void);
        //[DllImport("VMdll.dll", EntryPoint = "SET_Time")]
        //private static extern void SET_Time();


        //// 원형 extern "C" __declspec(dllimport) VOID _stdcall COM_ComTimeout(unsigned long nTimeout);  COM_ComTimeout(1000);
        //[DllImport("VMdll.dll", EntryPoint = "TMONEY_Purchase")]
        //private static extern char TMONEY_Purchase(int dwAmount, out int pBalance, byte[] outdate, out int nOutLen);


        /// <summary>
        /// FS (0x1C)
        /// </summary>
        private readonly string FS = Convert.ToString((char)0x1C);

        /// <summary>
        /// CR (0x0D)
        /// </summary>
        private readonly string CR = Convert.ToString((char)0x0D);

        /// <summary>
        /// 공백문자
        /// </summary>
        private readonly string mkSpace = string.Empty;

        private string GetHexatoData(byte[] lData, bool isOneType)
        {
            string lReturnData = string.Empty;
            if (isOneType)
            {
                foreach (byte item in lData)
                {
                    lReturnData += item.ToString("X2");
                }
            }
            else
            {
                lReturnData = HexaToDecimal(lData).ToString();
            }

            return lReturnData;
        }

        private string GetHexatoData(byte lData)
        {
            string lReturnData = string.Empty;
            lReturnData = lData.ToString("X2");
            return lReturnData;
        }

        public long HexaToDecimal(byte[] b)
        {


            Int64 r = 0;
            double m = 0;

            for (int i = b.Length; i > 0; i--)
            {
                m = (b.Length - i) * 8;

                r += (b[i - 1] * (Int64)Math.Pow(2, m));
            }
            return r;
        }

        public byte[] GetSubstringByte(byte[] pOriginaldata, int pStartIndex, int pLength)
        {
            byte[] lReturnByte = new byte[pLength];
            Array.Copy(pOriginaldata, pStartIndex, lReturnByte, 0, pLength);
            return lReturnByte;
        }
        public int mPortNumber = 0;
        public int mBaudrate = 0;


        public bool TmoneyConnet()
        {
            try
            {
                int ret = SMT_Dongle_Start(mPortNumber, mBaudrate);
                if (ret < 0)
                {
                    TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TmoneySmartro | TmoneyConnet", "포트오픈 실패");
                    return false;
                }
                else
                {

                    this.pvs_psamid = GetPsamID();               // 티머니 동글의 장착된 PsamID를 찾는다
                    this.pvs_dongleinfo = GetDongleInfo();       // 티머니 동글의 장착된 기기의 일련번호를 찾는다

                    //  this.pvs_dongleinfo="D350SPB503007766"; //테스트소스

                    TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TmoneySmartro | TmoneyConnet",
                        "동글 PsamID : " + this.pvs_psamid + " 기기일련번호: " + this.pvs_dongleinfo);
                    return true;
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TmoneySmartro | TmoneyConnet", ex.ToString());
                return false;
            }
        }
        public bool TmoneyClose()
        {
            try
            {
                int ret = SMT_Dongle_Stop();
                if (ret < 0)
                {
                    TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TmoneySmartro | TmoneyClose", "동글 포트닫기 실패");
                    return false;
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.TMONEY, "TmoneySmartro | TmoneyClose", "동글 포트닫기 성공");
                    return true;
                }
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.TMONEY, "TmoneySmartro | TmoneyConnet", ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 티머니 동글의 장착된 PsamID를 찾는다
        /// </summary>
        /// <returns>PsamID</returns>
        private string GetPsamID()
        {
            string ret_value = "";
            try
            {
                byte[] recv_data = new byte[2048];
                int lvi_ret = SMT_TMoney_PSamInfo(recv_data, 0, new byte[1], 30);
                if (lvi_ret > 0)
                {
                    ret_value = Encoding.Default.GetString(recv_data, 4, 16);   // PSAM ID
                }
            }
            catch (Exception)
            {
                ret_value = "";
            }
            return ret_value;
        }

        /// <summary>
        /// 티머니 동글의 장착된 기기의 일련번호를 찾는다
        /// </summary>
        /// <returns>일련번호</returns>
        private string GetDongleInfo()
        {
            string ret_value = "";
            try
            {
                byte[] recv_data = new byte[50];
                int lvi_ret = SMT_Dongle_Initial(2, recv_data);                                 // 1:통신사동글, 2:사인패드기본                
                if (lvi_ret > 0)
                {
                    ret_value = Encoding.Default.GetString(recv_data, 0, 16).Trim();
                }
            }
            catch (Exception)
            {
                ret_value = "";
            }
            return ret_value;
        }


        public bool TmoneyGashi(ref string pResultMessage)
        {
            try
            {
                pResultMessage = string.Empty;
                StringBuilder send_data = new StringBuilder();
                send_data.Append("TMY");                                                          // Message Header
                send_data.Append("0200");                                                         // Message Type
                send_data.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                    // 전문관리번호
                send_data.Append("94");                                                           // 전문구분    "94":부가장비 개시
                send_data.Append("N001");                                                         // 부가정보
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(this.pvs_catid);                                                 // 단말기번호  스마트로 부여 CATID
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("01");                                                           // 부가장비코드 "01":티머니동글
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(this.pvs_dongleinfo);                                            // 기기일련번호
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(this.pvs_dongleinfo);                                            // 동글 S/W 버전
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(this.pvs_psamid);                                                 // SamID  (개시거래시는 사용않함)
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(mkSpace.PadLeft(20, ' '));                                       // Filler
                send_data.Append(CR);                                                             // 0x0d
                send_data.Insert(0, String.Format("{0:0000}", send_data.Length));                 // 전문길이

                byte[] recv_data = new byte[2048];
                int lvi_ret = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(pvs_vanip), Convert.ToInt32(pvi_vanport), Encoding.Default.GetBytes(send_data.ToString()), 100, recv_data);
                if (lvi_ret > 0)
                {
                    string[] temps = Encoding.Default.GetString(recv_data).Split((char)0x1c);
                    if (temps != null && temps.Length >= 9)
                    {
                        string svr_tmy_tradeno = temps[6].ToString();                                // 거래고유번호
                        string svr_tmy_returncode = temps[7].ToString();                             // 응답코드
                        string svr_tmy_message1 = temps[8].ToString();                               // 응답메세지1
                        string svr_tmy_message2 = temps[9].ToString();                               // 응답메세지2

                        if (temps[7].ToString() == "00")
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyGashi", "[티머니개시거래]"
                                                                                                   + " 결과:" + svr_tmy_message1 + svr_tmy_message2);
                            pResultMessage = svr_tmy_message1;
                            return true;
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyGashi", "[티머니개시거래]"
                                                                                                   + " 결과:" + svr_tmy_message1 + svr_tmy_message2);
                            pResultMessage = svr_tmy_message1;
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro|TmoneyGashi", ex.ToString());
                return false;
            }

        }


        public bool TmoneyLogOn()
        {

            try
            {
                StringBuilder send_data2 = new StringBuilder();
                send_data2.Append("TMY");                                                    // Message Header
                send_data2.Append("0200");                                                   // Message Type
                send_data2.Append(DateTime.Now.ToString("yyMMddHHmmss", null));              // 전문관리번호
                send_data2.Append("07");                                                     // 전문구분    
                send_data2.Append("N111");                                                   // 부가정보
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(this.pvs_catid);                                           // 단말기번호  스마트로 부여 CATID
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("LO");                                                     // 부가장비코드 "TM":티머니지불
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(this.pvs_psamid);                                         // PSAM ID
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                         // PSAM거래일련번호
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(16, ' '));                                          // 카드번호
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                          // 카드거래일련번호
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("001");                                           // 거래구분 "001":지불거래, "064":거래취소
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                  // 거래 전 카드잔액
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0999999999");                                             // 거래 요청 금액("0999999999" 고정)
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                         // 거래 후 카드잔액
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(2, ' '));                                           // 알고리즘 ID
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(2, ' '));                                     // 개별거래 키 버전
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(2, ' '));                                      // 전자화폐 식별자
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                         // PSAM총액 거래 총 카운트
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("00000");                                         // PSAM개별 거래 건수
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append("0000000000");                                        // PSAM누적 거래 총액
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(8, ' '));                                       // SIGN
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(2, ' '));                                      // 사용자구분
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(2, ' '));                                      // 카드구분
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(8, ' '));                                  // 원거래일자
                send_data2.Append(FS);                                                       // 0x1c
                send_data2.Append(mkSpace.PadLeft(8, ' '));                                  // 원거래승인번호
                send_data2.Append(CR);                                                       // 0x0d
                send_data2.Insert(0, String.Format("{0:0000}", send_data2.Length));          // 전문길이


                byte[] recv_data2 = new byte[2048];
                byte[] send = Encoding.Default.GetBytes(send_data2.ToString());
                int lvi_ret2 = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(this.pvs_vanip), this.pvi_vanport, send, 100, recv_data2);
                if (lvi_ret2 > 0)
                {
                    string[] temps = Encoding.Default.GetString(recv_data2).Split((char)0x1c);
                    if (temps != null && temps.Length >= 14)
                    {
                        string svr_tmy_tradeno = temps[2].ToString();                          // 거래고유번호
                        string svr_tmy_datetime = temps[3].ToString();                         // 승인일시
                        string svr_tmy_afteramount = temps[6].ToString();                      // 거래 후 잔액
                        string svr_tmy_returncode = temps[8].ToString();                       // 응답코드
                        string svr_tmy_gamengcode = temps[9].ToString();                       // 가맹점코드
                        string svr_tmy_appnot = temps[10].ToString();                           // 승인번호
                        string stringsvr_tmy_message1 = temps[11].ToString();                        // 화면메세지

                        if (svr_tmy_returncode == "00")
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro | TmoneyLogOn", "로그온 응답 성공 " + temps[8].ToString());
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro | TmoneyLogOn", "로그온 응답 실패 " + temps[8].ToString());
                            return false;
                        }
                        #region 운영정보 전문 관련

                        StringBuilder send_data3 = new StringBuilder();
                        send_data3.Append("TMY");                                                          // Message Header
                        send_data3.Append("0200");                                                         // Message Type
                        send_data3.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                    // 전문관리번호
                        send_data3.Append("09");                                                           // 전문구분    
                        send_data3.Append("N001");                                                         // 부가정보
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append(this.pvs_catid);                                                 // 단말기번호  스마트로 부여 CATID
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append(temps[15].ToString().Substring(0, 8));                           // 부가장비코드   "발행사정보 파일명"
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append("00");                                                           // 발행사정보 등록 결과
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append(temps[16].ToString().Substring(0, 8));                           // 단말기기능정의 파일명
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append("00");                                                           // 단말기기능정의등록결과
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append(temps[17].ToString().Substring(0, 8));                           // 전자화폐사/Keyset 등록 파일명
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append("000000");                                                       // 전자화폐사 등록 결과
                        send_data3.Append(FS);                                                             // 0x1c
                        send_data3.Append("000000");                                                       // Keyset 등록 결과
                        send_data3.Append(CR);                                                             // 0x0d
                        send_data3.Insert(0, String.Format("{0:0000}", send_data3.Length));                // 전문길이

                        #endregion

                        byte[] recv_data3 = new byte[2048];
                        int lvi_ret3 = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(this.pvs_vanip), this.pvi_vanport, Encoding.Default.GetBytes(send_data3.ToString()), 100, recv_data3);
                        if (lvi_ret3 > 0)
                        {
                            string[] temps1 = Encoding.Default.GetString(recv_data3).Split((char)0x1c);
                            if (temps1 != null && temps1.Length >= 5)
                            {
                                if (temps1[4].ToString() == "00")
                                {
                                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro | TmoneyLogOn", "운영정보 응답 성공 " + temps1[5].ToString());
                                }
                                else
                                {
                                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro | TmoneyLogOn", "운영정보 응답 실패 " + temps1[5].ToString());
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro | TmoneyLogOn", "통신오류 ");
                            return false;
                        }


                    }
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro | TmoneyLogOn", "통신오류 ");
                    return false;

                }

                TmoneyLogDelete();  // 티머니 TR 로그를 삭제한다
                return true;

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "TmoneySmartro | TmoneyLogOn", "오류 " + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 티머니 TR 로그를 삭제한다
        /// </summary>
        private void TmoneyLogDelete()
        {
            try
            {
                byte[] recv_data = new byte[2048];
                SMT_TMoney_Log_Del(recv_data, 0, new byte[1]);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 티머니 잔액조회를 한다
        /// </summary>
        /// <param name="lvs_cardno">카드번호</param>
        /// <param name="lvi_amount">카드잔액</param>
        /// <returns>성공:true, 실패:false</returns>
        public bool TMoneyBalance(ref string lvs_cardno, ref int lvi_amount)
        {
            bool ret_check = false;
            lvs_cardno = "";         // 카드번호
            lvi_amount = 0;          // 카드잔액
            try
            {
                byte[] recv_data = new byte[2048];
                int lvi_ret = SMT_TMoney_Balance(recv_data, 0, new byte[1], 10);
                if (lvi_ret > 0)
                {
                    if (recv_data[3] == 0x00 || recv_data[3] == 0x01)
                    {
                        string lvs_data = Encoding.Default.GetString(recv_data);
                        lvs_cardno = lvs_data.Substring(4, 20).Trim().PadRight(20, ' ');  // 티머니 카드번호
                        lvi_amount = int.Parse(lvs_data.Substring(24, 10));               // 티머니 잔액

                        ret_check = true;
                    }
                }
            }
            catch (Exception)
            {
                ret_check = false;
            }
            return ret_check;
        }


        public TMONEY_CARDDATA TmoneyPayment(string pTmoneyCardNumber, int pPaymentMoney)
        {
            try
            {

                TMONEY_CARDDATA tmoney = new TMONEY_CARDDATA();
                tmoney.Success = false;
                byte[] recv_data1 = new byte[2048];
                StringBuilder sb = new StringBuilder();
                sb.Append(pTmoneyCardNumber);                                                           // 티머니 카드번호
                sb.Append(pPaymentMoney.ToString().PadLeft(10, '0'));                                 // 거래금액
                sb.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                          // 요청일시
                sb.Append("01");                                                                 // 사운드 효과(비프음사용)

                byte[] send_data1 = Encoding.Default.GetBytes(sb.ToString());
                int lvi_ret1 = SMT_TMoney_Pay(recv_data1, send_data1.Length, send_data1);        // 티머니 지불처리를 한다

                if (lvi_ret1 > 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyPayment", "[티머니결제시작] " + pPaymentMoney.ToString() + "원");
                    tmoney.T03_CARDtype = Encoding.Default.GetString(recv_data1, 146, 2);        // 카드구분
                    tmoney.T04_ALG = Encoding.Default.GetString(recv_data1, 107, 2);             // 알고리즘 ID
                    tmoney.T05_USERCODE = Encoding.Default.GetString(recv_data1, 148, 2); ;      // 사용자구분
                    tmoney.T06_TRT = Encoding.Default.GetString(recv_data1, 60, 3);              // 거래구분 "001":지불거래, "064":거래취소
                    tmoney.T07_Vkind_key = Encoding.Default.GetString(recv_data1, 109, 2);       // 개별거래 키 버전
                    tmoney.T08_IDcenter = Encoding.Default.GetString(recv_data1, 111, 2);        // 전자화폐 식별자
                    tmoney.T09_IDep = Encoding.Default.GetString(recv_data1, 30, 20).Trim();     // 카드번호
                    tmoney.T10_NTep = Encoding.Default.GetString(recv_data1, 50, 10);            // 카드거래일련번호
                    tmoney.T11_BALep = Encoding.Default.GetString(recv_data1, 83, 10);           // 거래 후 카드잔액
                    tmoney.T12_Mpda = Encoding.Default.GetString(recv_data1, 73, 10);            // 거래 요청 금액
                    tmoney.T13_IDsam = Encoding.Default.GetString(recv_data1, 4, 16);            // PSAM ID
                    tmoney.T14_NTsam = Encoding.Default.GetString(recv_data1, 20, 10);           // PSAM거래일련번호
                    tmoney.T15_NCsam = Encoding.Default.GetString(recv_data1, 113, 10);          // PSAM총액 거래 총 카운트
                    tmoney.T16_NIsam = Encoding.Default.GetString(recv_data1, 123, 5);           // PSAM개별 거래 건수
                    tmoney.T17_TOTsam = Encoding.Default.GetString(recv_data1, 128, 10);         // PSAM누적 거래 총액
                    tmoney.T18_SIGNind = Encoding.Default.GetString(recv_data1, 138, 8);         // SIGN
                    tmoney.T19_BeforeAmount = Encoding.Default.GetString(recv_data1, 63, 10);    // 거래 전 카드잔액
                    tmoney.T19_TrDateTime = Encoding.Default.GetString(recv_data1, 93, 14); ;    // 거래일시

                    StringBuilder send_data2 = new StringBuilder();
                    string lDate = DateTime.Now.ToString("yyMMddHHmmss", null);
                    send_data2.Append("TMY");                                                    // Message Header
                    send_data2.Append("0200");                                                   // Message Type
                    send_data2.Append(DateTime.Now.ToString("yyMMddHHmmss", null));              // 전문관리번호
                    send_data2.Append("07");                                                     // 전문구분    
                    send_data2.Append("N001");                                                   // 부가정보
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(this.pvs_catid);                                           // 단말기번호  스마트로 부여 CATID
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append("TM");                                                     // 부가장비코드 "TM":티머니지불
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T13_IDsam);                                         // PSAM ID
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T14_NTsam);                                         // PSAM거래일련번호
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T09_IDep);                                          // 카드번호
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T10_NTep);                                          // 카드거래일련번호
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T06_TRT);                                           // 거래구분 "001":지불거래, "064":거래취소
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T19_BeforeAmount);                                  // 거래 전 카드잔액
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T12_Mpda);                                          // 거래 요청 금액
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T11_BALep);                                         // 거래 후 카드잔액
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T04_ALG);                                           // 알고리즘 ID
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T07_Vkind_key);                                     // 개별거래 키 버전
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T08_IDcenter);                                      // 전자화폐 식별자
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T15_NCsam);                                         // PSAM총액 거래 총 카운트
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T16_NIsam);                                         // PSAM개별 거래 건수
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T17_TOTsam);                                        // PSAM누적 거래 총액
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T18_SIGNind);                                       // SIGN
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T05_USERCODE);                                      // 사용자구분
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(tmoney.T03_CARDtype);                                      // 카드구분
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(mkSpace.PadLeft(8, ' '));                                  // 원거래일자
                    send_data2.Append(FS);                                                       // 0x1c
                    send_data2.Append(mkSpace.PadLeft(8, ' '));                                  // 원거래승인번호
                    send_data2.Append(CR);                                                       // 0x0d
                    send_data2.Insert(0, String.Format("{0:0000}", send_data2.Length));          // 전문길이

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyLogOn", "[결제작업]:"
                                                                                          + " 단말기번호:" + NPSYS.TmoneyCatId
                                                                                          + " 부가장비코드:" + "TM"
                                                                                          + " 샘ID:" + tmoney.T13_IDsam
                                                                                          + " PSAM거래일련번호:" + tmoney.T14_NTsam
                                                                                          + " 카드번호:" + tmoney.T09_IDep
                                                                                          + " 카드거래일련번호:" + tmoney.T10_NTep
                                                                                          + " 거래구분:" + tmoney.T06_TRT
                                                                                          + " 거래전카드금액:" + tmoney.T19_BeforeAmount
                                                                                          + " 거래요청금액 :" + tmoney.T12_Mpda
                                                                                          + " 거래후카드금액:" + tmoney.T11_BALep
                                                                                          + " 알고리즘ID:" + tmoney.T04_ALG
                                                                                          + " PSAM누적 거래 총액:" + tmoney.T07_Vkind_key
                                                                                          + " 전자화폐식별자:" + tmoney.T08_IDcenter
                                                                                          + " PSAM총액거래 총카운트:" + tmoney.T15_NCsam
                                                                                          + " PSAM개별 거래 건수:" + tmoney.T16_NIsam
                                                                                          + " SIGN:" + tmoney.T18_SIGNind
                                                                                          + " 사용자구분:" + tmoney.T05_USERCODE
                                                                                          + " 카드구분:" + tmoney.T03_CARDtype
                                                                                          + " 원문:" + send_data2.ToString());
                    byte[] recv_data2 = new byte[2048];
                    byte[] send = Encoding.Default.GetBytes(send_data2.ToString());
                    int lvi_ret2 = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(this.pvs_vanip), Convert.ToInt32(this.pvi_vanport), send, 100, recv_data2);
                    if (lvi_ret2 > 0)
                    {
                        string[] temps = Encoding.Default.GetString(recv_data2).Split((char)0x1c);
                        if (temps != null && temps.Length >= 14)
                        {
                            string svr_tmy_tradeno = temps[2].ToString();                          // 거래고유번호
                            string svr_tmy_datetime = temps[3].ToString();                         // 승인일시
                            string svr_tmy_afteramount = temps[6].ToString();                      // 거래 후 잔액
                            string svr_tmy_returncode = temps[8].ToString();                       // 응답코드
                            string svr_tmy_gamengcode = temps[9].ToString();                       // 가맹점코드
                            tmoney.gamengcode = svr_tmy_gamengcode;
                            string svr_tmy_appno = temps[10].ToString();                           // 승인번호
                            tmoney.appno = svr_tmy_appno;
                            string svr_tmy_message1 = temps[11].ToString();                        // 화면메세지
                            tmoney.RetMessage = svr_tmy_message1;
                            if (svr_tmy_returncode == "00")
                            {
                                tmoney.Success = true;
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyLogOn", "[결제응답정보]:"
                                                                                                      + " 결제성공유무:" + tmoney.Success.ToString()
                                                                                                      + " 거래고유번호:" + svr_tmy_tradeno
                                                                                                      + " 승인일시:" + svr_tmy_datetime
                                                                                                      + " 거래후잔액:" + svr_tmy_afteramount
                                                                                                      + " 응답코드:" + svr_tmy_returncode
                                                                                                      + " 승인번호:" + svr_tmy_appno
                                                                                                      + " 승인메세지:" + tmoney.RetMessage);


                                return tmoney;
                            }
                            tmoney.Success = false;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyLogOn", "[결제응답정보]:"
                                                                                                  + " 결제성공유무:" + tmoney.Success.ToString()
                                                                                                  + " 거래고유번호:" + svr_tmy_tradeno
                                                                                                  + " 승인일시:" + svr_tmy_datetime
                                                                                                  + " 거래후잔액:" + svr_tmy_afteramount
                                                                                                  + " 응답코드:" + svr_tmy_returncode
                                                                                                  + " 승인번호:" + svr_tmy_appno
                                                                                                  + " 승인메세지:" + tmoney.RetMessage);
                            return tmoney;
                        }
                    }
                    return tmoney;
                }
                return tmoney;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "TmoneySmartro|TmoneyPayment", ex.ToString());
                return null;
            }

        }



    }
    /// <summary>
    /// 티머니 카드정보 데이터 구조체
    /// </summary>
    public class TMONEY_CARDDATA
    {
        /// <summary>
        /// 교통카드 구분 코드
        /// </summary>
        public string T03_CARDtype;

        /// <summary>
        /// 알고리즘 ID
        /// </summary>
        public string T04_ALG;

        /// <summary>
        /// 사용자 구분 코드
        /// </summary>
        public string T05_USERCODE;

        /// <summary>
        /// 거래유형(거래구분)
        /// </summary>
        public string T06_TRT;

        /// <summary>
        /// 개별거래 수집용 키 버젼
        /// </summary>
        public string T07_Vkind_key;

        /// <summary>
        /// 전자화페사 ID
        /// </summary>
        public string T08_IDcenter;

        /// <summary>
        /// 전자화폐 ID(카드번호)
        /// </summary>
        public string T09_IDep;

        /// <summary>
        /// 전자화페거래 일련번호
        /// </summary>
        public string T10_NTep;

        /// <summary>
        /// 거래후 전자화폐잔액
        /// </summary>
        public string T11_BALep;

        /// <summary>
        /// 거래금액
        /// </summary>
        public string T12_Mpda;

        /// <summary>
        /// 지불 SAM ID
        /// </summary>
        public string T13_IDsam;

        /// <summary>
        /// 지불 SAM 거래 일련번호
        /// </summary>
        public string T14_NTsam;

        /// <summary>
        /// 총액거래수집용 카운터
        /// </summary>
        public string T15_NCsam;

        /// <summary>
        /// 총액거래수집건수
        /// </summary>
        public string T16_NIsam;

        /// <summary>
        /// 지불 SAM 누적거래총액
        /// </summary>
        public string T17_TOTsam;

        /// <summary>
        /// 개별거래내역서명
        /// </summary>
        public string T18_SIGNind;

        /// <summary>
        /// 전자화폐잔액(거래전)
        /// </summary>
        public string T19_BeforeAmount;

        /// <summary>
        /// 거래일시
        /// </summary>
        public string T19_TrDateTime;
        /// <summary>
        /// 가맹점코드
        /// </summary>
        public string gamengcode = string.Empty;
        /// <summary>
        /// 승인번호
        /// </summary>
        public string appno = string.Empty;

        public string JunpyoNo = string.Empty;
        /// <summary>
        /// 응답메세지
        /// </summary>
        public string RetMessage = string.Empty;
        /// <summary>
        /// 티머니 카드정보 데이터
        /// </summary>
        /// <param name="i"></param>
        /// 
        public bool Success = false;

    }
}
