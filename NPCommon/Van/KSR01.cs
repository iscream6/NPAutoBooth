using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    public class KSR01
    {
        #region DllImport

        // 승인요청
        [DllImport(@"ksnetcomm.dll")]
        public static extern int KSCATApproval(byte[] responseTelegram, String ip, int port, String requestTelegram, int RequestLen, int option);

        #endregion



        /// <summary>
        /// 승인 요청
        /// </summary>
        /// <param name="TeminalID"></param>        // (입력)터미널 ID
        /// <param name="Amount"></param>           // (입력)금액
        /// <param name="VanNo"></param>            // (출력)승인번호
        /// <param name="VanDate"></param>          // (출력)승인일시
        /// <returns></returns>                     // (결과)승인 여부
        public static CreditAuthSimpleExData CreditAuthSimpleEx(string VanIp, int VanPort, string pTerminalNumber, string pAmount, string pSupplyPay, string pTax)
        {
            try
            {
                CreditAuthSimpleExData exData = new CreditAuthSimpleExData();
                exData.MagneMentCode = DateTime.Now.ToString("yyMMddHHmmss", null);
                exData.TerminalNumber = pTerminalNumber;
                exData.CreditInfo = string.Empty;
                exData.TotalAmount = pAmount;
                exData.TaxAmount = pTax;


                String SendData = String.Empty;

                String SendData_Len = String.Empty;


                byte[] RecvData = new byte[4096];

                int result = 0;


                String Padding = String.Empty;
                String STX = Convert.ToChar(2).ToString();
                String ETX = Convert.ToChar(3).ToString();
                String CR = Convert.ToChar(13).ToString();
                String FS = Convert.ToChar(28).ToString();


                // 로컬에서 Agent를 통해 결제 하기 때문에 승인서버 설정을 로컬로 해야 정상 결제 이뤄짐
                String ip = VanIp;

                int port = VanPort;


                SendData = "";

                // STX            
                SendData = SendData + STX;

                // 전문구분
                SendData = SendData + "IC";

                // 업무구분 
                SendData = SendData + "01";

                // 거래구분 (승인)
                SendData = SendData + "0200";

                // 거래형태 
                SendData = SendData + "N";

                // 단말기 번호
                SendData = SendData + pTerminalNumber;

                // 업체정보
                SendData = SendData + "    ";

                // 전문일련번호
                SendData = SendData + "000000000000";

                // Pos Entry Mode
                SendData = SendData + " ";

                // 거래고유번호
                SendData = SendData + "                    ";

                // 암호화하지않은 카드번호
                SendData = SendData + "                    ";

                // 암호화여부
                SendData = SendData + " ";

                // SW모델번호
                SendData = SendData + "################";

                // CAT or Reader 모델번호
                SendData = SendData + "################";

                // 암호화정보
                SendData = SendData + "                                        ";

                // 카드번호(Track2 Data)
                SendData = SendData + "                                     ";

                // FS
                SendData = SendData + FS;

                // 할부개월수
                SendData = SendData + "00";

                //  총금액
                SendData = SendData + pAmount.ToString().PadLeft(12, '0');

                //  봉사료
                SendData = SendData + "000000000000";

                //  세금
                SendData = SendData + pTax.PadLeft(12, '0');

                //  공급금액
                SendData = SendData + pSupplyPay.PadLeft(12, '0');

                //  면세금액
                SendData = SendData + "000000000000";

                //  WorkingKey Index 
                SendData = SendData + "AA";

                // 비밀번호
                SendData = SendData + "0000000000000000";

                //  원거래승인번호 
                SendData = SendData + "            ";

                //  원거래승인일자 
                SendData = SendData + "      ";

                //  사용자정보~DCC 
                SendData = SendData + Padding.PadLeft(163, ' ');

                //  전자서명유무     
                //SendData = SendData + "N";
                if (Convert.ToInt32(pAmount) >= 50000)
                {
                    SendData = SendData + "T";
                }
                else
                {
                    SendData = SendData + "N";
                }


                // ETX
                SendData = SendData + ETX;

                // CR
                SendData = SendData + CR;

                // 전문길이
                SendData_Len = String.Format("{0:D4}", SendData.Length);


                // STX~CR까지 파싱
                SendData = SendData_Len + SendData;


                // 승인 요청
                result = KSCATApproval(RecvData, ip, port, SendData, SendData.Length, 0);



                System.Text.Encoding EncodeData = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

                //   ASCIIEncoding ascii = new ASCIIEncoding();


                String Temp = EncodeData.GetString(RecvData, 0, RecvData.Length);
                // EncodeData.GetString(RecvData,

                //// 승인일자 취득
                //VanDate = Temp.Substring(49, 6);

                //// 승인번호 취득
                //VanNo = Temp.Substring(74, 8) + "    ";

                if (Temp.Length > 300)
                {
                    if (result <= 0)
                    {
                        exData.ResCode = "0001";

                    }
                    exData.ResCode = EncodeData.GetString(RecvData, 40, 1); // ’O’ : 정상(승인/취소)  ‘X’ : VAN 또는 카드사 거절  ‘F’ : KSCAT 거절
                    if (exData.ResCode == "F") // 거절
                    {
                        exData.ResCode = "0002";
                    }
                    if (exData.ResCode == "X") // KSCAT거절
                    {
                        exData.ResCode = "0003";
                    }
                    exData.TerminalNumber = EncodeData.GetString(RecvData, 14, 10).Trim();                                     // 단말기번호
                    if (exData.ResCode != "0001" && exData.ResCode != "0002" && exData.ResCode != "0003")
                    {
                        if (exData.ResCode == "O")
                        {
                            exData.ResCode = "0000";                                      // 응답 코드(정상 승인여부 응답코드로 판별)
                        }
                    }
                    //0454IC010210NAT0161015A    000000000000O00000000160813142223N하나카드        OK: 03029237    03029237    936072263475        00977069970    24하나카드        03하나카드        6691DFE4643C48C7CC000000000000000000000000000000000000TEL)1544-4700       KSNET제출                                                                            541707**********                                                                                             
                    exData.AuthDate = "20" + EncodeData.GetString(RecvData, 49, 6);                                   // 승인 일자
                    exData.AuthTime = EncodeData.GetString(RecvData, 55, 6);                                   // 승인 시간
                    exData.IssuerCode = EncodeData.GetString(RecvData, 141, 2);                                 // 발급사코드
                    exData.IssuerName = EncodeData.GetString(RecvData, 143, 16).Trim();                      // 발급사명
                    exData.AcquirerCode = EncodeData.GetString(RecvData, 159, 2).Trim();                                   // 매입사코드
                    exData.AcquirerName = EncodeData.GetString(RecvData, 161, 16).Trim();
                    exData.MemberNumber = EncodeData.GetString(RecvData, 126, 15).Trim();                         // 가맹점번호
                    exData.CreditInfo = EncodeData.GetString(RecvData, 336, 30).Trim();                                     // 카드번호
                    exData.AuthNumber = EncodeData.GetString(RecvData, 94, 12).Trim();                                 // 승인번호
                    exData.DdcFlag = "D";                                // 매입구분
                    exData.ResMsg = EncodeData.GetString(RecvData, 62, 16).Trim();                          // 응답메세지1
                    exData.ResMsg2 = EncodeData.GetString(RecvData, 78, 16).Trim();                          // 응답메세지2
                    return exData;


                }

                //if (Temp.Length > 300)
                //{
                //    if (result <= 0)
                //    {
                //         exData.ResCode ="0001";

                //    }
                //    exData.ResCode = Temp.Substring(40, 1); // ’O’ : 정상(승인/취소)  ‘X’ : VAN 또는 카드사 거절  ‘F’ : KSCAT 거절
                //    if (exData.ResCode == "F") // 거절
                //    {
                //        exData.ResCode = "0002";
                //    }
                //    if (exData.ResCode == "X") // KSCAT거절
                //    {
                //        exData.ResCode = "0003";
                //    }
                //    exData.TerminalNumber = Temp.Substring(14, 10).Trim();                                     // 단말기번호
                //    if (exData.ResCode != "0001" && exData.ResCode != "0002" && exData.ResCode != "0003")
                //    {
                //        if (exData.ResCode == "O")
                //        {
                //            exData.ResCode = "0000";                                      // 응답 코드(정상 승인여부 응답코드로 판별)
                //        }
                //    }
                //    //0454IC010210NAT0161015A    000000000000O00000000160813142223N하나카드        OK: 03029237    03029237    936072263475        00977069970    24하나카드        03하나카드        6691DFE4643C48C7CC000000000000000000000000000000000000TEL)1544-4700       KSNET제출                                                                            541707**********                                                                                             
                //    exData.AuthDate = "20"+Temp.Substring(49,6);                                   // 승인 일자
                //    exData.AuthTime = Temp.Substring(55,6);                                   // 승인 시간
                //    exData.IssuerCode = Temp.Substring(141, 2);                                 // 발급사코드
                //    exData.IssuerName = Temp.Substring(146, 16).Trim();                      // 발급사명
                //    exData.AcquirerCode = Temp.Substring(159, 2).Trim();                                   // 매입사코드
                //    exData.AcquirerName = Temp.Substring(161, 16).Trim();
                //    exData.MemberNumber = Temp.Substring(126, 15).Trim();                         // 가맹점번호
                //    exData.CreditInfo = Temp.Substring(336, 30).Trim();                                     // 카드번호
                //    exData.AuthNumber = Temp.Substring(94, 12).Trim();                                 // 승인번호
                //    exData.DdcFlag = "D";                                // 매입구분
                //    exData.ResMsg = Temp.Substring(62, 16).Trim();                          // 응답메세지1
                //    exData.ResMsg2 = Temp.Substring(78, 16).Trim();                          // 응답메세지2
                //    return exData;


                //}
                // 승인 실패

                else
                {
                    return exData;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KSR01 | CreditAuthSimpleExData", "예외사항" + ex.ToString());
                return null;
            }



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


        /// <summary>
        /// 승인 취소
        /// </summary>
        /// <param name="TeminalID"></param>        // (입력)터미널 ID
        /// <param name="Amount"></param>           // (입력)금액
        /// <param name="VanNo"></param>            // (입력)승인번호
        /// <param name="VanDate"></param>          // (입력)승인일시
        /// <returns></returns>                     // (결과)승인 취소 여부
        public bool KSR01_Approval_Cancel(String TeminalID, String Amount, String VanNo, String VanDate)
        {
            String SendData = String.Empty;

            String SendData_Len = String.Empty;


            byte[] RecvData = new byte[4096];

            int result = 0;


            String Padding = String.Empty;
            String STX = Convert.ToChar(2).ToString();
            String ETX = Convert.ToChar(3).ToString();
            String CR = Convert.ToChar(13).ToString();
            String FS = Convert.ToChar(28).ToString();


            // 로컬에서 Agent를 통해 결제 하기 때문에 승인서버 설정을 로컬로 해야 정상 결제 이뤄짐
            String ip = "127.0.0.1";

            int port = 27015;


            SendData = "";

            // STX            
            SendData = SendData + STX;

            // 전문구분
            SendData = SendData + "IC";

            // 업무구분 
            SendData = SendData + "01";

            // 거래구분 (승인 취소)
            SendData = SendData + "0420";

            // 거래형태 
            SendData = SendData + "N";

            // 단말기 번호
            SendData = SendData + TeminalID;

            // 업체정보
            SendData = SendData + "    ";

            // 전문일련번호
            SendData = SendData + "000000000000";

            // Pos Entry Mode
            SendData = SendData + " ";

            // 거래고유번호
            SendData = SendData + "                    ";

            // 암호화하지않은 카드번호
            SendData = SendData + "                    ";

            // 암호화여부
            SendData = SendData + " ";

            // SW모델번호
            SendData = SendData + "################";

            // CAT or Reader 모델번호
            SendData = SendData + "################";

            // 암호화정보
            SendData = SendData + "                                        ";

            // 카드번호(Track2 Data)
            SendData = SendData + "                                     ";

            // FS
            SendData = SendData + FS;

            // 할부개월수
            SendData = SendData + "00";

            // 총금액
            SendData = SendData + Amount;

            // 봉사료
            SendData = SendData + "000000000000";

            // 세금
            SendData = SendData + "000000000000";

            // 공급금액
            SendData = SendData + "000000000000";

            // 면세금액
            SendData = SendData + "000000000000";

            // WorkingKey Index 
            SendData = SendData + "AA";

            // 비밀번호
            SendData = SendData + "0000000000000000";

            // 원거래승인번호 
            SendData = SendData + VanNo;

            // 원거래승인일자 
            SendData = SendData + VanDate;

            // 사용자정보~DCC 
            SendData = SendData + Padding.PadLeft(163, ' ');

            //  전자서명유무     
            SendData = SendData + "N";

            // ETX
            SendData = SendData + ETX;

            // CR
            SendData = SendData + CR;

            // 전문길이
            SendData_Len = String.Format("{0:D4}", SendData.Length);


            // STX~CR까지 파싱
            SendData = SendData_Len + SendData;


            // 승인 취소 요청
            result = KSCATApproval(RecvData, ip, port, SendData, SendData.Length, 0);



            System.Text.Encoding EncodeData = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            ASCIIEncoding ascii = new ASCIIEncoding();


            String Temp = EncodeData.GetString(RecvData, 0, RecvData.Length);


            // 승인 실패
            if ((result <= 0) || Temp.Substring(40, 1) == "X" || Temp.Substring(40, 1) == "F")
            {
                return false;
            }

            return true;
        }
    }
}
