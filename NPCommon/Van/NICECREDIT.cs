using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    /// <summary>
    /// 나이스 마그네틱 처리
    /// </summary>
    public class NICECREDIT
    {
        public const string Success = "0000";

        //[운영]
        //IP : 211.33.136.2
        //PORT : 7709

        //[테스트]
        //IP : 211.33.136.19
        //PORT : 40777
        //단말기번호:2393300001
        /// <summary>
        /// 공백문자
        /// </summary>
        private static readonly string mkSpace = string.Empty;
        //typedef long (__stdcall* Pos_Send)( char* IP, long PORT, char* send_data, char* sign_data, char* recv_data);
        [DllImport("NicePosV205.dll", EntryPoint = "Pos_Send", CharSet = CharSet.Ansi)]
        private static extern int Pos_Send(byte[] ip, int port, byte[] send_data, byte[] sing_data, byte[] recv_data);

        public static CreditAuthSimpleExData CreditAuthSimpleEx(string VanIp, int VanPort, string pTerminalNumber, string pCreditInfo, string pTotalAmount, string pTaxAmount)
        {
            try
            {

                CreditAuthSimpleExData exData = new CreditAuthSimpleExData();
                exData.MagneMentCode = DateTime.Now.ToString("MMddHHmmss", null);
                exData.TerminalNumber = pTerminalNumber;
                exData.CreditInfo = pCreditInfo;
                exData.TotalAmount = pTotalAmount;
                exData.TaxAmount = pTaxAmount;
                exData.ResCode = string.Empty;
                StringBuilder send_data = new StringBuilder();
                //send_data.Append("0257");                                                          // Message Header
                send_data.Append("TAX");                                                                // 1.전문텍스트
                send_data.Append(TextCore.ToLeftAlignString(10, exData.TerminalNumber, ' ') + DateTime.Now.ToString("MMddHHmmss", null));    // 2.전문관리번호
                send_data.Append("0200");                                                               // 3.전문구분    승인/인증 "0200" 취소 "0420"
                send_data.Append("10");                                                                 // 4.거래구분
                send_data.Append("H1");                                                                 // 5. 기종구분
                send_data.Append(mkSpace.PadLeft(10, ' '));                                             // 6.기기구분구분
                send_data.Append(TextCore.ToLeftAlignString(10, exData.TerminalNumber, ' '));             // 7.단말기번호  CATID
                send_data.Append("A");                                                                  // 8.WCC            
                send_data.Append("37" + TextCore.ToLeftAlignString(37, exData.CreditInfo, ' '));         // 9.TRACK2
                send_data.Append("00");                                                                 // 10.할부기간
                send_data.Append(mkSpace.PadLeft(9, '0'));                                              // 11.봉사료
                send_data.Append(int.Parse(pTaxAmount).ToString().PadLeft(9, '0'));                   // 12.세금
                send_data.Append(int.Parse(pTotalAmount).ToString().PadLeft(9, '0'));                   // 13.거래금액
                send_data.Append(mkSpace.PadLeft(8, ' '));                                              // 14.원거래승인번호
                send_data.Append(mkSpace.PadLeft(6, ' '));                                              // 15.원거래승인일자
                send_data.Append(mkSpace.PadLeft(12, ' '));                                              // 16.원거래고유번호
                send_data.Append(TextCore.ToLeftAlignString(10, NPSYS.gVanSaup, '0'));                     // 17.사업자번호
                send_data.Append(mkSpace.PadLeft(13, '0'));                                              // 18.주민번호
                send_data.Append(mkSpace.PadLeft(16, ' '));                                              // 19.PIN번호
                send_data.Append(mkSpace.PadLeft(30, ' '));                                              // 20.도메인
                send_data.Append(mkSpace.PadLeft(20, ' '));                                              // 21.Address
                send_data.Append(mkSpace.PadLeft(1, ' '));                                               // 22.FILTER
                send_data.Append("N");                                                                   // 23.SIGN구분값
                send_data.Insert(0, String.Format("{0:0000}", send_data.Length));
                if (!NPSYS.isBoothRealMode)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NICECREDIT | CreditAuthSimpleEx", "전문" + send_data.ToString());
                }
                byte[] recv_data = new byte[450];
                byte[] sing_data = new byte[1];
                int lvi_ret = Pos_Send(Encoding.Default.GetBytes(VanIp), VanPort, Encoding.Default.GetBytes(send_data.ToString()), sing_data, recv_data);
                if (lvi_ret > 0)
                {
                    string temps = Encoding.Default.GetString(recv_data);
                    if (temps != null && temps.Length >= 0)
                    {
                        int subIndex = 0;
                        string recvheader = string.Empty;
                        recvheader = Encoding.Default.GetString(recv_data, subIndex, 4); // 0.Message Header
                        subIndex += 4;
                        string recvTax = string.Empty;
                        recvTax = Encoding.Default.GetString(recv_data, subIndex, 3); // 1.전문텍스트
                        subIndex += 3;
                        exData.MagneMentCode = Encoding.Default.GetString(recv_data, subIndex, 20); // 2.전문관리번호
                        subIndex += 20;
                        string maganeGubun = Encoding.Default.GetString(recv_data, subIndex, 4); //3.전문구분
                        subIndex += 4;
                        string geraeGubun = Encoding.Default.GetString(recv_data, subIndex, 2); //4.거래구분
                        subIndex += 2;
                        string deviceGubun = Encoding.Default.GetString(recv_data, subIndex, 2); //5.기종구분
                        subIndex += 2;
                        string devicetypeGubun = Encoding.Default.GetString(recv_data, subIndex, 10); //6.기기구분
                        subIndex += 10;
                        exData.TerminalNumber = Encoding.Default.GetString(recv_data, subIndex, 10);  // 7.단말기번호
                        subIndex += 10;

                        exData.ResCode = Encoding.Default.GetString(recv_data, subIndex, 4);   // 8.응답 코드(정상 승인여부 응답코드로 판별)
                        subIndex += 4;
                        string wcc = Encoding.Default.GetString(recv_data, subIndex, 1); // 9.WCC
                        subIndex += 1;
                        exData.CreditInfo = Encoding.Default.GetString(recv_data, subIndex, 39).Substring(2, 37);   // 10.카드번호
                        subIndex += 39;
                        string halbu = Encoding.Default.GetString(recv_data, subIndex, 2); // 11.할부
                        subIndex += 2;
                        string discountMoney = Encoding.Default.GetString(recv_data, subIndex, 9); // 12.봉사료
                        subIndex += 9;
                        string taxMoney = Encoding.Default.GetString(recv_data, subIndex, 9); // 13.세금
                        subIndex += 9;
                        string approvalMoney = Encoding.Default.GetString(recv_data, subIndex, 9); // 14.거래금액
                        subIndex += 9;
                        string saupNo = Encoding.Default.GetString(recv_data, subIndex, 10); // 15.사업자번호
                        subIndex += 10;
                        string jumino = Encoding.Default.GetString(recv_data, subIndex, 13); // 16.주민번호
                        subIndex += 13;
                        string pinNo = Encoding.Default.GetString(recv_data, subIndex, 16); // 17.PIN번호
                        subIndex += 16;
                        exData.IssuerCode = Encoding.Default.GetString(recv_data, subIndex, 2).Trim();                           // 18.발급사코드
                        subIndex += 2;
                        exData.IssuerName = Encoding.Default.GetString(recv_data, subIndex, 20).Trim();                          // 19.발급사명
                        subIndex += 20;
                        exData.AcquirerCode = Encoding.Default.GetString(recv_data, subIndex, 2).Trim();                         // 20.매입사코드
                        subIndex += 2;
                        exData.AcquirerName = Encoding.Default.GetString(recv_data, subIndex, 20).Trim();                        // 21.매입사명
                        subIndex += 20;
                        exData.MemberNumber = Encoding.Default.GetString(recv_data, subIndex, 15).Trim();                        // 22.가맹점번호
                        subIndex += 15;
                        exData.AuthDate = "20" + Encoding.Default.GetString(recv_data, subIndex, 6).Trim();                                  // 23.승인 일자
                        subIndex += 6;
                        exData.AuthTime = Encoding.Default.GetString(recv_data, subIndex, 6).Trim();                                   // 23.승인 시간
                        subIndex += 6;
                        exData.AuthNumber = Encoding.Default.GetString(recv_data, subIndex, 12).Trim();  // 24.승인번호
                        subIndex += 12;
                        string originalNo = Encoding.Default.GetString(recv_data, subIndex, 12).Trim(); // 25.거래고유번호
                        exData.OriginalNumber = originalNo;
                        subIndex += 12;
                        exData.DdcFlag = Encoding.Default.GetString(recv_data, subIndex, 1).Trim();                            // 매입구분
                        subIndex += 1;
                        exData.ResMsg = Encoding.Default.GetString(recv_data, subIndex, 40).Trim();                         // 응답메세지1
                        subIndex += 40;
                        //exData.ResMsg = Encoding.Default.GetString(recv_data,subIndex, 24).Trim();                         // 응답메세지2
                        //subIndex += 24;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NICECREDIT | CreditAuthSimpleExData", "[신용응답정보]"
                            + " [거래고유번호]" + originalNo
                            + " [응답메세지]" + exData.ResMsg
                            + " [가맹점번호]" + exData.MemberNumber
                            + " [승인일자]" + exData.AuthDate
                            + " [승인시간]" + exData.AuthTime
                            + " [거래금액]" + approvalMoney.ToString()
                            + " [응답코드]" + exData.ResCode
                            + " [승인번호]" + exData.AuthNumber
                            + " [단말기번호]" + exData.TerminalNumber);

                        //recvheader = temps.Substring(subIndex, 4); // 0.Message Header
                        //subIndex += 4;
                        //string recvTax = string.Empty;
                        //recvTax = temps.Substring(subIndex, 3); // 1.전문텍스트
                        //subIndex += 3;
                        //exData.MagneMentCode = temps.Substring(subIndex, 20); // 2.전문관리번호
                        //subIndex += 20;
                        //string maganeGubun =  temps.Substring(subIndex, 4); //3.전문구분
                        //subIndex += 4;
                        //string geraeGubun = temps.Substring(subIndex, 2); //4.거래구분
                        //subIndex += 2;
                        //string deviceGubun = temps.Substring(subIndex, 2); //5.기종구분
                        //subIndex += 2;
                        //string devicetypeGubun = temps.Substring(subIndex, 10); //6.기기구분
                        //subIndex += 10;
                        //exData.TerminalNumber = temps.Substring(subIndex, 10);  // 7.단말기번호
                        //subIndex += 10;

                        //exData.ResCode = temps.Substring(subIndex, 4);   // 8.응답 코드(정상 승인여부 응답코드로 판별)
                        //subIndex += 4;
                        //string wcc = temps.Substring(subIndex, 1); // 9.WCC
                        //subIndex += 1;
                        //exData.CreditInfo = temps.Substring(subIndex, 39).Substring(2,37);   // 10.카드번호
                        //subIndex += 39;
                        //string halbu =  temps.Substring(subIndex, 2); // 11.할부
                        //subIndex += 2;
                        //string discountMoney = temps.Substring(subIndex, 9); // 12.봉사료
                        //subIndex += 9;
                        //string taxMoney = temps.Substring(subIndex, 9); // 13.세금
                        //subIndex += 9;
                        //string approvalMoney = temps.Substring(subIndex, 9); // 14.거래금액
                        //subIndex += 9;
                        //string saupNo = temps.Substring(subIndex, 10); // 15.사업자번호
                        //subIndex += 10;
                        //string jumino = temps.Substring(subIndex, 13); // 16.주민번호
                        //subIndex += 13;
                        //string pinNo = temps.Substring(subIndex, 16); // 17.PIN번호
                        //subIndex += 16;
                        //exData.IssuerCode = temps.Substring(subIndex, 2).Trim();                           // 18.발급사코드
                        //subIndex += 2;
                        //exData.IssuerName = temps.Substring(subIndex, 20).Trim();                          // 19.발급사명
                        //subIndex += 20;
                        //exData.AcquirerCode = temps.Substring(subIndex, 2).Trim();                         // 20.매입사코드
                        //subIndex += 2;
                        //exData.AcquirerName = temps.Substring(subIndex, 20).Trim();                        // 21.매입사명
                        //subIndex += 20;
                        //exData.MemberNumber = temps.Substring(subIndex, 15).Trim();                        // 22.가맹점번호
                        //subIndex += 15;
                        //exData.AuthDate = "20"+temps.Substring(subIndex, 6).Trim();                                  // 23.승인 일자
                        //subIndex += 6;
                        //exData.AuthTime = temps.Substring(subIndex, 6).Trim();                                   // 23.승인 시간
                        //subIndex += 6;
                        //exData.AuthNumber = temps.Substring(subIndex, 12).Trim();  // 24.승인번호
                        //subIndex += 12;
                        //string originalNo = temps.Substring(subIndex, 12).Trim(); // 25.거래고유번호
                        //subIndex += 12;
                        //exData.DdcFlag = temps.Substring(subIndex, 1).Trim();                            // 매입구분
                        //subIndex += 1;
                        //exData.ResMsg = temps.Substring(subIndex, 40).Trim();                         // 응답메세지1
                        //subIndex += 40;
                        //exData.ResMsg = temps.Substring(subIndex, 24).Trim();                         // 응답메세지2
                        //subIndex += 24;

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

        //public static CashReceiptAuthData CashReceiptAuthEx(string VanIp, int VanPort, string pTerminalNumber, string pTotalAmount, string pTaxAmount)
        //{
        //    try
        //    {



        //        CashReceiptAuthData exData = new CashReceiptAuthData();
        //        exData.TerminalNumber = pTerminalNumber;
        //        exData.TotalAmount = pTotalAmount;
        //        exData.TaxAmount = pTaxAmount;
        //        exData.ResCode = string.Empty;
        //        StringBuilder send_data = new StringBuilder();
        //        send_data.Append("POS");                                                          // Message Header
        //        send_data.Append("0200");                                                         // Message Type
        //        send_data.Append(DateTime.Now.ToString("yyMMddHHmmss", null));                    // 전문관리번호
        //        send_data.Append("01");                                                           // 전문구분    "10":신용승인, "11":신용강제취소, "15":신용망상취소, "72":인증/인증+승인
        //        send_data.Append("N001");                                                         // 부가정보    현금여수증
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append(exData.TerminalNumber);                                                 // 단말기번호  CATID
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append("02");                                                           // 리딩모드    "00":스와핑, "02":Keyin
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append("0100001234");                               // Track II Data
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append("00");                                                           // 사용구분
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append(int.Parse(pTotalAmount).ToString().PadLeft(12, '0'));         // 승인금액
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append(mkSpace.PadLeft(8, '0'));                                        // 봉사료
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append(int.Parse(pTaxAmount).ToString().PadLeft(8, '0'));                                        // 세금
        //        send_data.Append(FS);                                                             // 0x1c
        //        send_data.Append(mkSpace.PadLeft(12, ' '));                                       // 
        //        send_data.Append(FS);                                                             // 
        //        send_data.Append(mkSpace.PadLeft(6, ' '));                                       //                 0x1c
        //        send_data.Append(FS);                                                             // 
        //        send_data.Append(mkSpace.PadLeft(512, ' '));                                       //                 0x1c
        //        send_data.Append(CR);                                                             // 0x0d
        //        send_data.Insert(0, String.Format("{0:0000}", send_data.Length));                 // 전문길이 제일앞부분 추가
        //        if (!NPSYS.isBoothRealMode)
        //        {

        //            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SMATROCREDIT|CreditAuthSimpleEx", "전문" + send_data.ToString());
        //        }
        //        byte[] recv_data = new byte[2048];
        //        int lvi_ret = SMT_S_ConnSndRcv(Encoding.Default.GetBytes(VanIp), VanPort, Encoding.Default.GetBytes(send_data.ToString()), 100, recv_data);
        //        if (lvi_ret > 0)
        //        {
        //            string[] temps = Encoding.Default.GetString(recv_data).Split((char)0x1c);
        //            if (temps != null && temps.Length >= 11)
        //            {
        //                exData.TerminalNumber = temps[1].ToString();                                     // 단말기번호
        //                exData.ResCode = temps[2].ToString();                                      // 응답 코드(정상 승인여부 응답코드로 판별)
        //                exData.AuthDate = temps[3].ToString();                                   // 승인 일자
        //                exData.AuthTime = temps[4].ToString();                                   // 승인 시간
        //                exData.AuthNumber = temps[6].ToString();                                // 발급사코드
        //                exData.ResMsg = temps[7].ToString();                                // 발급사코드
        //                exData.ResMsg2 = temps[9].ToString();                                // 발급사코드
        //            }
        //        }
        //        return exData;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SMATROCREDIT | CashReceiptAuthEx", "예외사항" + ex.ToString());
        //        return null;
        //    }

        //}

        //public static string GetAcquirerName(string pAcquireCode)
        //{
        //    string lAcquireName = string.Empty;
        //    switch (pAcquireCode)
        //    {
        //        case "0170":
        //            lAcquireName = "국민카드사";
        //            break;

        //        case "0171":
        //            lAcquireName = "농협";
        //            break;

        //        case "0172":
        //            lAcquireName = "씨티카드";
        //            break;

        //        case "0173":
        //            lAcquireName = "수협";
        //            break;

        //        case "0175":
        //            lAcquireName = "우리카드";
        //            break;

        //        case "0176":
        //            lAcquireName = "씨티은행";
        //            break;

        //        case "0177":
        //            lAcquireName = "신세계한미";
        //            break;

        //        case "0200":
        //            lAcquireName = "해외카드";
        //            break;

        //        case "0203":
        //            lAcquireName = "해외카드(LG)";
        //            break;

        //        case "0300":
        //            lAcquireName = "신한카드사";
        //            break;

        //        case "0400":
        //            lAcquireName = "BC카드사";
        //            break;

        //        case "0505":
        //            lAcquireName = "외환카드사";
        //            break;

        //        case "0506":
        //            lAcquireName = "광주은행";
        //            break;

        //        case "0507":
        //            lAcquireName = "제주은행";
        //            break;

        //        case "0508":
        //            lAcquireName = "전북은행";
        //            break;

        //        case "0511":
        //            lAcquireName = "조홍은행";
        //            break;

        //        case "7300":
        //            lAcquireName = "LG My Point";
        //            break;

        //        case "7301":
        //            lAcquireName = "LG정유";
        //            break;

        //        case "7302":
        //            lAcquireName = "LG 보너스클럽";
        //            break;

        //        case "7400":
        //            lAcquireName = "OK CashBag";
        //            break;

        //        case "7500":
        //            lAcquireName = "드림라이프카드";
        //            break;

        //        case "5100":
        //            lAcquireName = "크라운베이커리";
        //            break;

        //        case "7900":
        //            lAcquireName = "All@카드";
        //            break;

        //        case "8002":
        //            lAcquireName = "뱅크타움(수표조회)";
        //            break;

        //        case "8003":
        //            lAcquireName = "금융결제원(직불)";
        //            break;

        //        case "9100":
        //            lAcquireName = "Secutec";
        //            break;

        //        case "9200":
        //            lAcquireName = "KT";
        //            break;

        //        case "9300":
        //            lAcquireName = "INTERBANK";
        //            break;

        //        case "9400":
        //            lAcquireName = "KTF";
        //            break;

        //        case "0514":
        //            lAcquireName = "신한카드";
        //            break;

        //        case "0516":
        //            lAcquireName = "주택은행";
        //            break;


        //        case "0521":
        //            lAcquireName = "하나SK카드";
        //            break;

        //        case "0522":
        //            lAcquireName = "산은캐피탈";
        //            break;


        //        case "1100":
        //            lAcquireName = "롯데아맥스카드";
        //            break;

        //        case "1200":
        //            lAcquireName = "현대카드사";
        //            break;


        //        case "1300":
        //            lAcquireName = "삼성카드사";
        //            break;

        //        case "1400":
        //            lAcquireName = "롯데카드";
        //            break;

        //    }
        //    return lAcquireName;
        //}
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
            private string mResMsg2 = string.Empty;
            /// <summary>
            /// 응답메시지, 거절시 거절사유
            /// </summary>
            public string ResMsg2
            {
                set { mResMsg2 = value; }
                get { return mResMsg2; }

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
            private string mOriginalNumber = string.Empty;
            public string OriginalNumber
            {
                set { mOriginalNumber = value; }
                get { return mOriginalNumber; }


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
