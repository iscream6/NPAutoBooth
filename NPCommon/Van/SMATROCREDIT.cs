using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    public class SMATROCREDIT
    {
        public const string Success = "00";
        /// <summary>
        /// FS (0x1C)
        /// </summary>
        private static readonly string FS = Convert.ToString((char)0x1C);

        /// <summary>
        /// CR (0x0D)
        /// </summary>
        private static readonly string CR = Convert.ToString((char)0x0D);

        /// <summary>
        /// 공백문자
        /// </summary>
        private static readonly string mkSpace = string.Empty;
        // typedef int(__stdcall *SMT_S_ConnSndRcv)( BYTE  *szIP, short nPort, BYTE  *szSnd,	 int iTimeOut, BYTE  *szRcv);
        [DllImport("SmartroSign.dll", EntryPoint = "SMT_S_ConnSndRcv", CharSet = CharSet.Ansi)]
        private static extern int SMT_S_ConnSndRcv(byte[] ip, int port, byte[] send_data, int timeout, byte[] recv_data);

        public static CreditAuthSimpleExData CreditAuthSimpleEx(string VanIp, int VanPort, string pTerminalNumber, string pCreditInfo, string pTotalAmount, string pTaxAmount)
        {
            try
            {

                CreditAuthSimpleExData exData = new CreditAuthSimpleExData();
                exData.MagneMentCode = DateTime.Now.ToString("yyMMddHHmmss", null);
                exData.TerminalNumber = pTerminalNumber;
                exData.CreditInfo = pCreditInfo;
                exData.TotalAmount = pTotalAmount;
                exData.TaxAmount = pTaxAmount;
                exData.ResCode = string.Empty;
                StringBuilder send_data = new StringBuilder();
                send_data.Append("POS");                                                          // Message Header
                send_data.Append("0200");                                                         // Message Type
                send_data.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                    // 전문관리번호
                send_data.Append("10");                                                           // 전문구분    "10":신용승인, "11":신용강제취소, "15":신용망상취소, "72":인증/인증+승인
                send_data.Append("N001");                                                         // 부가정보    "N"사인데이터 미존재
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(exData.TerminalNumber);                                                 // 단말기번호  CATID
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("00");                                                           // 리딩모드    "00":스와핑, "02":Keyin
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(pCreditInfo.Substring(0, 37).PadLeft(37, ' '));                               // Track II Data
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("00");                                                           // 할부기간
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(int.Parse(pTotalAmount).ToString().PadLeft(12, '0'));         // 승인금액
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(mkSpace.PadLeft(8, '0'));                                        // 봉사료
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(int.Parse(pTaxAmount).ToString().PadLeft(8, '0'));                                        // 세금
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(mkSpace.PadLeft(20, ' '));                                       // 
                send_data.Append(CR);                                                             // 0x0d
                send_data.Insert(0, String.Format("{0:0000}", send_data.Length));                 // 전문길이 제일앞부분 추가
                if (!NPSYS.isBoothRealMode)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SMATROCREDIT|CreditAuthSimpleEx", "전문" + send_data.ToString());
                }
                byte[] recv_data = new byte[2048];
                int lvi_ret = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(VanIp), VanPort, Encoding.Default.GetBytes(send_data.ToString()), 100, recv_data);
                if (lvi_ret > 0)
                {
                    string[] temps = Encoding.Default.GetString(recv_data).Split((char)0x1c);
                    if (temps != null && temps.Length >= 14)
                    {
                        exData.MagneMentCode = temps[0].ToString().Substring(11, 12);                 // 전문관리번호
                        exData.TerminalNumber = temps[1].ToString();                                     // 단말기번호
                        exData.ResCode = temps[2].ToString();                                      // 응답 코드(정상 승인여부 응답코드로 판별)
                        exData.AuthDate = temps[3].ToString();                                   // 승인 일자
                        exData.AuthTime = temps[4].ToString();                                   // 승인 시간
                        exData.IssuerCode = temps[5].ToString();                                // 발급사코드
                        exData.IssuerName = temps[6].ToString().Trim();                         // 발급사명
                        exData.AcquirerCode = temps[7].ToString().Trim();                                  // 매입사코드
                        exData.AcquirerName = GetAcquirerName(temps[7].ToString().Trim());
                        exData.MemberNumber = temps[8].ToString().Trim();                         // 가맹점번호
                        exData.CreditInfo = temps[9].ToString();                                    // 카드번호
                        exData.AuthNumber = temps[10].ToString().Trim();                             // 승인번호
                        exData.DdcFlag = temps[11].ToString();                                // 매입구분
                        exData.ResMsg = temps[12].ToString().Trim();                          // 응답메세지1
                        exData.ResMsg2 = temps[13].ToString().Trim();                          // 응답메세지2
                    }
                }
                return exData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SMATROCREDIT|CreditAuthSimpleExData", "예외사항" + ex.ToString());
                return null;
            }

        }

        public static CashReceiptAuthData CashReceiptAuthEx(string VanIp, int VanPort, string pTerminalNumber, string pTotalAmount, string pTaxAmount)
        {
            try
            {



                CashReceiptAuthData exData = new CashReceiptAuthData();
                exData.TerminalNumber = pTerminalNumber;
                exData.TotalAmount = pTotalAmount;
                exData.TaxAmount = pTaxAmount;
                exData.ResCode = string.Empty;
                StringBuilder send_data = new StringBuilder();
                send_data.Append("POS");                                                          // Message Header
                send_data.Append("0200");                                                         // Message Type
                send_data.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                    // 전문관리번호
                send_data.Append("01");                                                           // 전문구분    "10":신용승인, "11":신용강제취소, "15":신용망상취소, "72":인증/인증+승인
                send_data.Append("N001");                                                         // 부가정보    현금여수증
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(exData.TerminalNumber);                                                 // 단말기번호  CATID
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("02");                                                           // 리딩모드    "00":스와핑, "02":Keyin
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("0100001234");                               // Track II Data
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append("00");                                                           // 사용구분
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(int.Parse(pTotalAmount).ToString().PadLeft(12, '0'));         // 승인금액
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(mkSpace.PadLeft(8, '0'));                                        // 봉사료
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(int.Parse(pTaxAmount).ToString().PadLeft(8, '0'));                                        // 세금
                send_data.Append(FS);                                                             // 0x1c
                send_data.Append(mkSpace.PadLeft(12, ' '));                                       // 
                send_data.Append(FS);                                                             // 
                send_data.Append(mkSpace.PadLeft(6, ' '));                                       //                 0x1c
                send_data.Append(FS);                                                             // 
                send_data.Append(mkSpace.PadLeft(512, ' '));                                       //                 0x1c
                send_data.Append(CR);                                                             // 0x0d
                send_data.Insert(0, String.Format("{0:0000}", send_data.Length));                 // 전문길이 제일앞부분 추가
                if (!NPSYS.isBoothRealMode)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SMATROCREDIT|CreditAuthSimpleEx", "전문" + send_data.ToString());
                }
                byte[] recv_data = new byte[2048];
                int lvi_ret = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(VanIp), VanPort, Encoding.Default.GetBytes(send_data.ToString()), 100, recv_data);
                if (lvi_ret > 0)
                {
                    string[] temps = Encoding.Default.GetString(recv_data).Split((char)0x1c);
                    if (temps != null && temps.Length >= 11)
                    {
                        exData.TerminalNumber = temps[1].ToString();                                     // 단말기번호
                        exData.ResCode = temps[2].ToString();                                      // 응답 코드(정상 승인여부 응답코드로 판별)
                        exData.AuthDate = temps[3].ToString();                                   // 승인 일자
                        exData.AuthTime = temps[4].ToString();                                   // 승인 시간
                        exData.AuthNumber = temps[6].ToString();                                // 발급사코드
                        exData.ResMsg = temps[7].ToString();                                // 발급사코드
                        exData.ResMsg2 = temps[9].ToString();                                // 발급사코드
                    }
                }
                return exData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SMATROCREDIT | CashReceiptAuthEx", "예외사항" + ex.ToString());
                return null;
            }

        }

        public static string GetAcquirerName(string pAcquireCode)
        {
            string lAcquireName = string.Empty;
            switch (pAcquireCode)
            {
                case "0170":
                    lAcquireName = "국민카드사";
                    break;

                case "0171":
                    lAcquireName = "농협";
                    break;

                case "0172":
                    lAcquireName = "씨티카드";
                    break;

                case "0173":
                    lAcquireName = "수협";
                    break;

                case "0175":
                    lAcquireName = "우리카드";
                    break;

                case "0176":
                    lAcquireName = "씨티은행";
                    break;

                case "0177":
                    lAcquireName = "신세계한미";
                    break;

                case "0200":
                    lAcquireName = "해외카드";
                    break;

                case "0203":
                    lAcquireName = "해외카드(LG)";
                    break;

                case "0300":
                    lAcquireName = "신한카드사";
                    break;

                case "0400":
                    lAcquireName = "BC카드사";
                    break;

                case "0505":
                    lAcquireName = "외환카드사";
                    break;

                case "0506":
                    lAcquireName = "광주은행";
                    break;

                case "0507":
                    lAcquireName = "제주은행";
                    break;

                case "0508":
                    lAcquireName = "전북은행";
                    break;

                case "0511":
                    lAcquireName = "조홍은행";
                    break;

                case "7300":
                    lAcquireName = "LG My Point";
                    break;

                case "7301":
                    lAcquireName = "LG정유";
                    break;

                case "7302":
                    lAcquireName = "LG 보너스클럽";
                    break;

                case "7400":
                    lAcquireName = "OK CashBag";
                    break;

                case "7500":
                    lAcquireName = "드림라이프카드";
                    break;

                case "5100":
                    lAcquireName = "크라운베이커리";
                    break;

                case "7900":
                    lAcquireName = "All@카드";
                    break;

                case "8002":
                    lAcquireName = "뱅크타움(수표조회)";
                    break;

                case "8003":
                    lAcquireName = "금융결제원(직불)";
                    break;

                case "9100":
                    lAcquireName = "Secutec";
                    break;

                case "9200":
                    lAcquireName = "KT";
                    break;

                case "9300":
                    lAcquireName = "INTERBANK";
                    break;

                case "9400":
                    lAcquireName = "KTF";
                    break;

                case "0514":
                    lAcquireName = "신한카드";
                    break;

                case "0516":
                    lAcquireName = "주택은행";
                    break;


                case "0521":
                    lAcquireName = "하나SK카드";
                    break;

                case "0522":
                    lAcquireName = "산은캐피탈";
                    break;


                case "1100":
                    lAcquireName = "롯데아맥스카드";
                    break;

                case "1200":
                    lAcquireName = "현대카드사";
                    break;


                case "1300":
                    lAcquireName = "삼성카드사";
                    break;

                case "1400":
                    lAcquireName = "롯데카드";
                    break;

            }
            return lAcquireName;
        }
        public class CreditAuthSimpleExData
        {
            /// <summary>
            /// 리턴코드
            /// 0 - 성공
            /// 1 – 자동 망 취소 수행됨
            /// -1000 – 응답코드가 '0000' 이 아님
            /// -1001 – POS Initial, 가맹점 임의정보 설정 실패
            /// -1002 – 싞용카드 정보, 싞용카드 입력구붂 설정 실패
            /// -1003 – 거래금액,할부개월,봉사료,세금 설정 실패
            /// -1005 - OCB카드 정보, OCB카드 입력구붂 설정 실패
            /// -1007 – 사인데이타,사인압축방식,사인MAC 설정 실패
            /// -1008 – 응답코드 획득 실패.
            /// </summary>
            public int ReturnCode
            {
                get;
                set;
            }

            /// <summary>
            /// 단말기 번호
            /// </summary>
            public string TerminalNumber
            {
                get;
                set;
            }

            /// <summary>
            /// 거래일련번호
            /// </summary>
            public string SequenceNumber
            {
                get;
                set;
            }

            /// <summary>
            /// POS업체 Initial
            /// </summary>
            public string PosInitial
            {
                get;
                set;
            }

            /// <summary>
            /// 가맹점 임의정보
            /// </summary>
            public string TempInfo
            {
                get;
                set;
            }

            /// <summary>
            /// 신용카드 정보 Track2Data(37byte) Key_in시 카드번호 +'='+ 유효기간(yymm)
            /// </summary>
            public string CreditInfo
            {
                get;
                set;
            }

            /// <summary>
            /// 입력구분, S=Swipe, K=Keyin, O=이통사동글, R=일반동글
            /// </summary>
            public string CreditInputType
            {
                get;
                set;
            }

            /// <summary>
            /// 할부개월, 일시불일경우 '', ex)3개월= '03'
            /// </summary>
            public string InstallPeriod
            {
                get;
                set;
            }

            /// <summary>
            /// 거래금액
            /// </summary>
            public string TotalAmount
            {
                get;
                set;
            }

            /// <summary>
            /// 봉사료, 미입력시 ''
            /// </summary>
            public string ServiceAmount
            {
                get;
                set;
            }

            /// <summary>
            /// 세금, 미입력시 ''
            /// </summary>
            public string TaxAmount
            {
                get;
                set;
            }

            /// <summary>
            /// OBC카드 정보 미입력시 ''
            /// </summary>
            public string OcbInfo
            {
                get;
                set;
            }

            /// <summary>
            /// OCB입력 구분, S=Swipe, K=Keyin, O=이통사동글, R=일반동글
            /// </summary>
            public string OcbInputType
            {
                get;
                set;
            }

            /// <summary>
            /// 사인압축방식, 미입력시 ''
            /// </summary>
            public string SignCompMethod
            {
                get;
                set;
            }

            /// <summary>
            /// 사인MAC, 미입력시 ''
            /// </summary>
            public string SignMac
            {
                get;
                set;
            }

            /// <summary>
            /// 사인데이터(최대 1536byte), 미입력시 ''
            /// </summary>
            public string SignData
            {
                get;
                set;
            }

            /// <summary>
            /// 출력전표Flag 'O'-전표출력, 'X'-전표미출력
            /// </summary>
            public string PrintFlag
            {
                get;
                set;
            }

            /// <summary>
            /// 응답코드, '0000' 정상
            /// </summary>
            public string ResCode
            {
                get;
                set;
            }

            /// <summary>
            /// 승인번호
            /// </summary>
            public string AuthNumber
            {
                get;
                set;
            }

            /// <summary>
            /// 승인일자(YYYYMMDD)
            /// </summary>
            public string AuthDate
            {
                get;
                set;
            }

            /// <summary>
            /// 승인시간(HHmmss)
            /// </summary>
            public string AuthTime
            {
                get;
                set;
            }

            /// <summary>
            /// 가맹점 번호
            /// </summary>
            public string MemberNumber
            {
                get;
                set;
            }

            /// <summary>
            /// DDC Flag, D=DDC, ''=기타
            /// </summary>
            public string DdcFlag
            {
                get;
                set;
            }

            /// <summary>
            /// DDC 전표 번호,
            /// </summary>
            public string DdcNumber
            {
                get;
                set;
            }

            /// <summary>
            /// 응답메시지, 거절시 거절사유
            /// </summary>
            public string ResMsg
            {
                get;
                set;
            }

            /// <summary>
            /// 응답메시지, 거절시 거절사유
            /// </summary>
            public string ResMsg2
            {
                get;
                set;
            }

            /// <summary>
            /// 카드명
            /// </summary>
            public string CardName
            {
                get;
                set;
            }

            /// <summary>
            /// 발급사 코드
            /// </summary>
            public string IssuerCode
            {
                get;
                set;
            }

            /// <summary>
            /// 발급사 명
            /// </summary>
            public string IssuerName
            {
                get;
                set;
            }

            /// <summary>
            /// 매입사 코드
            /// </summary>
            public string AcquirerCode
            {
                get;
                set;
            }

            /// <summary>
            /// 매입사 명
            /// </summary>
            public string AcquirerName
            {
                get;
                set;
            }

            /// <summary>
            /// 잔액(Gift카드)
            /// </summary>
            public string GiftCash
            {
                get;
                set;
            }

            /// <summary>
            /// Notice
            /// </summary>
            public string Notice
            {
                get;
                set;
            }

            /// <summary>
            /// OCB 응답코드
            /// </summary>
            public string OcbResCode
            {
                get;
                set;
            }

            /// <summary>
            /// 고객명
            /// </summary>
            public string CustomerName
            {
                get;
                set;
            }

            /// <summary>
            /// 적립포인트
            /// </summary>
            public string AddPoint
            {
                get;
                set;
            }

            /// <summary>
            /// 누적포인트
            /// </summary>
            public string SavePoint
            {
                get;
                set;
            }

            /// <summary>
            /// 가용포인트
            /// </summary>
            public string UsablePoint
            {
                get;
                set;
            }

            /// <summary>
            /// 알림메시지
            /// </summary>
            public string BoardMsg
            {
                get;
                set;
            }

            /// <summary>
            /// 체크카드 플래그 (체크카드 'Y', 그외 'N')
            /// </summary>
            public string ChkFlag
            {
                get;
                set;
            }

            public string MagneMentCode
            {
                get;
                set;

            }
        }

        public class CashReceiptAuthData
        {
            /// <summary>
            /// 리턴코드
            /// 0 - 성공
            /// 1 – 자동 망 취소 수행됨
            /// -1000 – 응답코드가 '0000' 이 아님
            /// -1001 – POS Initial, 가맹점 임의정보 설정 실패
            /// -1002 – 싞용카드 정보, 싞용카드 입력구붂 설정 실패
            /// -1003 – 거래금액,할부개월,봉사료,세금 설정 실패
            /// -1005 - OCB카드 정보, OCB카드 입력구붂 설정 실패
            /// -1007 – 사인데이타,사인압축방식,사인MAC 설정 실패
            /// -1008 – 응답코드 획득 실패.
            /// </summary>
            public int ReturnCode
            {
                get;
                set;
            }

            /// <summary>
            /// 단말기 번호
            /// </summary>
            public string TerminalNumber
            {
                get;
                set;
            }

            /// <summary>
            /// 거래일련번호
            /// </summary>
            public string SequenceNumber
            {
                get;
                set;
            }

            /// <summary>
            /// POS업체 Initial
            /// </summary>
            public string PosInitial
            {
                get;
                set;
            }

            /// <summary>
            /// 가맹점 임의정보
            /// </summary>
            public string TempInfo
            {
                get;
                set;
            }

            /// <summary>
            /// 신분정보(최대37byte) 카드번호: Swipe거래만 가능  신분번호: Keyin거래만 가능 (신분번호:주민번호,사업자번호,핸드폰번호)
            /// </summary>
            public string id_info
            {
                get;
                set;
            }

            /// <summary>
            /// 입력구분, S=Swipe, K=Keyin, O=이통사동글, R=일반동글
            /// </summary>
            public string _id_input_type
            {
                get;
                set;
            }

            /// <summary>
            /// 현금영수증거래구분(2byte)  '00'-소비자소득공제, '01'-사업자지출증빙
            /// </summary>
            public string _tran_type
            {
                get;
                set;
            }

            /// <summary>
            /// 거래금액
            /// </summary>
            public string TotalAmount
            {
                get;
                set;
            }

            /// <summary>
            /// 봉사료, 미입력시 ''
            /// </summary>
            public string ServiceAmount
            {
                get;
                set;
            }

            /// <summary>
            /// 세금, 미입력시 ''
            /// </summary>
            public string TaxAmount
            {
                get;
                set;
            }


            /// <summary>
            /// 출력전표Flag 'O'-전표출력, 'X'-전표미출력
            /// </summary>
            public string PrintFlag
            {
                get;
                set;
            }

            /// <summary>
            /// 응답코드, '0000' 정상
            /// </summary>
            public string ResCode
            {
                get;
                set;
            }

            /// <summary>
            /// 승인번호
            /// </summary>
            public string AuthNumber
            {
                get;
                set;
            }

            /// <summary>
            /// 승인일자(YYYYMMDD)
            /// </summary>
            public string AuthDate
            {
                get;
                set;
            }

            public string AuthTime
            {
                get;
                set;
            }

            /// <summary>
            /// 사업자 번호
            /// </summary>
            public string business_number
            {
                get;
                set;
            }


            /// <summary>
            /// 응답메시지, 거절시 거절사유
            /// </summary>
            public string ResMsg
            {
                get;
                set;
            }

            public string ResMsg2
            {
                get;
                set;
            }


            /// <summary>
            /// 알림메시지
            /// </summary>
            public string BoardMsg
            {
                get;
                set;
            }

        }
    }
}
