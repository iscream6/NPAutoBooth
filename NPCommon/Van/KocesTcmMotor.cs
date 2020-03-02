using FadeFox.Text;
using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    public class KocesTcmMotor
    {
        // 테스트 정보
        //private static string mTerminalId = "0710000001";
        //private static string mSwVersion = "07201";
        //private static string mSerial = "1000000007";
        //private static string mSaup = "2148631917";


        // 리얼사용시     
        public static string mTerminalId = string.Empty;
        public static string mSwVersion = string.Empty;
        public static string mSerial = string.Empty;
        public static string mSaup = string.Empty;


        [DllImport(@"C:\Koces\KocesICPos\KocesICLib.dll", EntryPoint = "KocesICRequest", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int KocesICReq(ref PGFAuthResAppr tagAuthResAppr,
            string pTrdType, string pAuData, string pMonth, string pTrdAmt,
            string pSvcAmt, string pTaxAmt, string pTaxFreeAmt, string pAuNo,
            string pAuDate, string pKeyYn, string pInsYn, string pCashNum,
            string pCancelReason, string pPtSvrCd, string pPtInsYn, string pPtCardCd);


        [DllImport(@"C:\Koces\KocesICPos\KocesICLib.dll", EntryPoint = "KocesCardAccept", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int KocesICardAccept(); //0성공 -1포트오픈실패 -3KocesCPos미실행

        [DllImport(@"C:\Koces\KocesICPos\KocesICLib.dll", EntryPoint = "KocesCardEject", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int KocesCardEject(); //0성공 -1포트오픈실패 -3KocesCPos미실행


        [DllImport(@"C:\Koces\KocesICPos\KocesICLib.dll", EntryPoint = "KocesRequestCardState", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int KocesRequestCardState(byte[] pRes); // 0성공 -1포트오픈실패 -3KocesCPos미실행

        [DllImport(@"C:\Koces\KocesICPos\KocesICLib.dll", EntryPoint = "DownLoadRequest", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern int DownLoadRequest(ref PGFDownLoadResAppr tagDownLoadResAppr, string strTermID, string strMchData, string strSftVer, string strSerial, string strBsnNo);


        //공통헤더
        [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
        public struct PGFComHead
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] strApprVer; /* 전문버전 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] strServiceType; /* 서비스종류 서비스전문표 참조 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] strTrdType; /* 업무 구분, 업무구분표 참조 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] strSndType; /* 전송구분, 'S': PG->VAN, 'R': VAN->PG */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] strTermID; /* 단말기번호 터미널아이디 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] strTrdDate; /* 거래일시 YYMMDDhhmmss */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] strTrdNo; /* 거래일련번호 (응답시 반환) */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] strMchData; /* 가맹점 데이타 (응답시 반환) */

            /// <summary>
            /// 응답코드
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] strAnsCode; /* 응답코드 (요청시는 스페이스) */
        }



        // 다운로드
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct PGFDownLoadResAppr
        {
            public PGFComHead Header;      /* 공통 헤더 부분 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] strSerial;    /* 제조일련번호               */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public char[] strMsgA01;    /* 응답메세지                 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public char[] strSftVer;    /* Terminal S/W Version       */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public char[] strShpNm;    /* 가맹점명칭                 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] strBsnNo;    /* 가맹점사업자번호           */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] strPreNm;    /* 대표자명                   */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] strShpAdr;    /* 가맹점주소                 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public char[] strShpTel;    /* 가맹점전화번호             */
        }
        /* 신용승인(인증) 응답전문 259 byte */
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct PGFAuthResAppr
        {
            public PGFComHead Header;      /* 공통 헤더 부분 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strTradeNo; /* Van 서 부여하는 거래 고유번호 LJFS */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strAuNo; /* 승인번호 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] strTradeDate; /* 승인시간 'YYMMDDhhmmss' */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] strMessage; /* 응답 메시지(거절 시 응답 Message) */




            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public char[] strCardNo; /* 카드 BIN */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strCardKind; /* 카드종류명 */


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] strOrdCd; /* 발급사 코드 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strOrdNm; /* 발급사 명 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] strInpCd; /* 매입사 코드 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] strInpNm; /* 매입사 명 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] strMchNo; /* 가맹점번호 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] strPtCardCd; /* 포인트카드 코드*/

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] strPtCardNm; /* 포인트카드사 명 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public char[] strJukPoint; /* 적립포인트 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public char[] strGayPoint; /* 가용포인트 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public char[] strNujPoint; /* 누적포인트 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public char[] strSaleRate; /* 할인율 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] strPtAuNo; /* 적립승인번호 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public char[] strPtMchNo; /* 적립가맹점번호 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] strPtAnswerCd; /* 적립응답코드 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48)]
            public char[] strPtMessage; /* 적립응답메세지 */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] strDDCYN; /* DDC 여부      */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] strEDIYN; /* EDI 여부      */

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] strCardType; // 카드구분 (1:신용,2:체크,3:기프트,4:선불)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strTrdKey; // 거래고유키 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] strKeyRenewal;  // 키 갱신일자

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] strFilter; /* Working Key 비밀번호 사용시 TMK[TPK] */
        }

        /// <summary>
        /// 현금영수증
        /// </summary>
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct PGFCashBillResAppr
        {
            public PGFComHead Header;      /* 공통 헤더 부분 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
            public char[] strTrdDate;//거래일시 YYMMDDhhmmss
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] strAuNo;//승인번호
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public char[] strMessage;//응답메세지
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] strFiller;//여유필드
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] strcCR;//CR  0x0d
        }

        public static bool CardAccept()
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | CardAccept", "[KOCSE카드리더기 입수명령]");
                int returnStatus = KocesICardAccept();
                if (returnStatus == 0)
                {

                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | CardAccept", "카드방출오류" + returnStatus.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | CardAccept", ex.ToString());
                return false;
            }
        }

        public static bool CardEject()
        {
            try
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | CardEject", "[KOCSE카드리더기 방출명령]");
                int returnStatus = KocesCardEject();
                if (returnStatus == 0)
                {
                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | CardEject", "카드방출오류" + returnStatus.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | CardEject", ex.ToString());
                return false;
            }
        }

        public static int CardState()
        {
            try
            {
                byte[] cardState = new byte[2];
                int state = KocesRequestCardState(cardState);
                if (state == 0)
                {
                    return Convert.ToInt32(Encoding.Default.GetString(cardState));
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | CardState", "state값 비정상:" + state.ToString());
                    return state;

                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | CardState", ex.ToString());
                return 3;
            }
        }
        public static bool ProgramDownLoadRequest(ref string strErrorMessage)
        {
            int nResult = -1;

            //if (m_strTerminalID == string.Empty || m_strTerminalID.Equals("0000000000"))
            //{
            //    return -101;
            //}

            // SetServer(m_strServerIP, int.Parse(m_strServerPort));

            string strMchData = "다운로드 요청";
            PGFDownLoadResAppr tagDownLoadResAppr = new PGFDownLoadResAppr();
            tagDownLoadResAppr.Header = new PGFComHead();

            try
            {
                nResult = DownLoadRequest(ref tagDownLoadResAppr, mTerminalId, strMchData, mSwVersion, mSerial, mSaup);

                if (nResult == 0)
                {
                    string strResult = new string(tagDownLoadResAppr.Header.strAnsCode);

                    if (strResult != "0000")
                    {
                        nResult = -1;

                        strErrorMessage = new string(tagDownLoadResAppr.strMsgA01);

                        strErrorMessage = strErrorMessage.Replace("\0", " ");
                        strErrorMessage = strErrorMessage.Trim();
                    }
                }


                if (nResult == 0)
                {
                    string gamanjum = "가맹정명칭:" + new string(tagDownLoadResAppr.strShpAdr);
                    string gamanjumjuso = "가맹점주소:" + new string(tagDownLoadResAppr.strShpAdr);
                    string device = "기기번호:" + new string(tagDownLoadResAppr.strSerial);

                    nResult = 1;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | ProgramDownLoadRequest", "[가맹점 획득정보]"
                                                                                                         + " 가맹정명칭:" + gamanjum
                                                                                                         + " 가맹점주소:" + gamanjumjuso
                                                                                                         + " 기기번호:" + device
                                                                                                         );
                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | ProgramDownLoadRequest", "[실패]");
                    return false;
                }

                // LogSave("다운로드 요청(" + strErrorMessage + "), 결과(" + strFinalResult + ")");

            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KocesTcmMotor | ProgramDownLoadRequest", ex.ToString());
            }

            return false;
        }

        public class CreditAuthSimpleExData
        {
            public enum TradeType
            {
                /// <summary>
                /// IC승인
                /// </summary>
                F1,
                /// <summary>
                /// IC취소
                /// </summary>
                F2,
                /// <summary>
                /// 현금승인
                /// </summary>
                H3,
                /// <summary>
                /// 현금취소
                /// </summary>
                H4
            }



            public TradeType currentTrade = TradeType.F1;

            private string mAuData = TextCore.ToLeftAlignString(20, string.Empty, ' ');
            /// <summary>
            /// 가맹점사용영역
            /// </summary>
            public string AuData
            {
                set { mAuData = TextCore.ToLeftAlignString(20, value, ' '); }
                get { return mAuData; }
            }
            /// <summary>
            /// 할부
            /// </summary>
            public string Month = "00";

            /// <summary>
            /// 금액입력
            /// </summary>
            /// <param name="pAmount">총금액</param>
            /// <param name="pTax">세금</param>
            public void SetAmount(int pAmount, int pTax)
            {
                switch (currentTrade)
                {
                    case TradeType.F1:
                        SetTrdAmount = pAmount - pTax;
                        SetTaxAmount = pTax;
                        break;
                    case TradeType.F2:
                        SetTrdAmount = pAmount;
                        SetTaxAmount = 0;
                        break;
                }
            }

            private int mSetTrdAmount = 0;

            //- 승인요청 승인요청 시
            //순매출액 순매출액 : 봉사료 , 세금을 제외한 제외한 순매출액 순매출액
            //- 취소요청 취소요청 시
            //순매출액 순매출액 : 승인요청시의 승인요청시의 봉사료 봉사료, 세금을 합산한 합산한 금액
            //나.현금영수증 현금영수증 현금영수증 거래
            //- 승인요청 승인요청 시
            //순매출액 순매출액 : 봉사료 , 세금을 제외한 제외한 순매출액 순매출액
            //- 취소요청 취소요청 시
            //순매출액 순매출액 : 봉사료 , 세금을 제외한 제외한 순매출액 순매출액
            /// <summary>
            /// 순매출액
            /// </summary>
            /// 
            public int SetTrdAmount
            {
                set { mSetTrdAmount = value; }
                get { return mSetTrdAmount; }
            }
            private int mSetServiceAmount = 0;
            /// <summary>
            /// 봉사료, 미입력시 ''
            /// </summary>
            public int SetServiceAmount
            {
                get { return mSetServiceAmount; }
            }

            private int mSetTaxAmount = 0;
            /// <summary>
            /// 세금, 미입력시 ''
            /// </summary>
            public int SetTaxAmount
            {
                set { mSetTaxAmount = value; }
                get { return mSetTaxAmount; }
            }
            private int mSetTaxFreeAmt = 0;
            public int SetTaxFreeAmt
            {

                get { return mSetTaxFreeAmt; }

            }

            private string mOriginalAuthNo = TextCore.ToLeftAlignString(12, string.Empty, ' ');
            /// <summary>
            /// 원승인번호(왼쪽정령이며 승인시 스페이스) 12자리
            /// </summary>
            public string OriginalAuthNo
            {
                set { mOriginalAuthNo = TextCore.ToLeftAlignString(12, value, ' '); }
                get { return mOriginalAuthNo; }


            }

            private string mOriginalAuthDate = TextCore.ToLeftAlignString(8, string.Empty, ' ');
            /// <summary>
            /// 원승인일시(왼쪽정령이며 승인시 스페이스)YYYYMMDD 8자리
            /// </summary>
            public string OriginalAuthDate
            {
                set { mOriginalAuthDate = TextCore.ToLeftAlignString(8, value, ' '); }
                get { return mOriginalAuthDate; }

            }

            private string mKeyYn = "I";
            /// <summary>
            /// Swipe구분  (I: 신용승인 , S: 현금영수증시 카드 ,  K:휴 대폰번호 )
            /// </summary>

            public string KeyYn
            {
                set { mKeyYn = value; }
                get { return mKeyYn; }

            }
            private string mInsYn = " ";
            /// <summary>
            /// 현금영수증 구분(1: 개인 2: 법인 3: 자진 발급 4: 원천 징수)
            /// </summary>
            public string InsYn
            {
                set { mInsYn = value; }
                get { return mInsYn; }

            }
            private string mCashNum = TextCore.ToLeftAlignString(13, string.Empty, ' ');
            /// <summary>
            /// 고객번호
            /// </summary>
            public string CashNum
            {
                set { mCashNum = TextCore.ToLeftAlignString(13, value, ' '); }
                get { return mCashNum; }

            }
            private string mCancelReson = TextCore.ToLeftAlignString(1, string.Empty, ' ');
            /// <summary>
            /// 취소사유 ((1:거래취소 , 2:오류발급 , 3:기타 ))
            /// </summary>
            public string CancelReson
            {
                set { mCancelReson = TextCore.ToLeftAlignString(1, value, ' '); }
                get { return mCancelReson; }

            }
            /// <summary>
            /// 서비스구분 서비스구분   ("01":적립 ,  "02":적립취소 ,  "03":사용 "04":사용취소"05":조회  "11 ":포인트 할인요청, "12 ":포인트 할인요청 취소 )
            /// </summary>
            public string PtSvrCdp = "  ";


            private string mPInsYn = "  ";
            /// <summary>
            ///결제구분("01":현금,"02":신용)
            ///단 "01"현금거래중에서 포인트 단독일겨웅에만 해당,소비자소득공제 및 사업자 지출증빙은 해당사항아님
            /// </summary>
            public string PInsYn
            {
                set { mPInsYn = TextCore.ToLeftAlignString(2, value, ' '); }
                get { return mPInsYn; }

            }
            private string mPtCardCd = "  ";
            /// <summary>
            /// 포인트코드 01:캐쉬백,07:엠플러스,16:코엑스
            /// </summary>
            public string PtCardCd
            {
                set { mPtCardCd = TextCore.ToLeftAlignString(2, value, ' '); }
                get { return mPtCardCd; }

            }
            /// <summary>
            /// 거래고유번호
            /// </summary>
            public string TradNo
            {
                get;
                set;

            }

            public string CardNo
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

            ///// <summary>
            ///// DDC 전표 번호,
            ///// </summary>
            //public string DdcNumber
            //{
            //    get;
            //    set;
            //}

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

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }



        public static CreditAuthSimpleExData CreditAuthSimpleEx(int pAmount, int pTax)
        {
            int rtn;
            rtn = 0;

            PGFAuthResAppr tagPGFAuthResAppr = new PGFAuthResAppr();
            tagPGFAuthResAppr.Header = new PGFComHead();
            // 승인
            CreditAuthSimpleExData creditAuthSimpleExData = new CreditAuthSimpleExData();
            creditAuthSimpleExData.currentTrade = CreditAuthSimpleExData.TradeType.F1;
            creditAuthSimpleExData.SetAmount(pAmount, pTax);


            rtn = KocesICReq(ref tagPGFAuthResAppr,
                creditAuthSimpleExData.currentTrade.ToString() //거래구분
                , creditAuthSimpleExData.AuData  // 가맹점영역
                , creditAuthSimpleExData.Month // 할부
                , creditAuthSimpleExData.SetTrdAmount.ToString() // 거래금액.Text
                , creditAuthSimpleExData.SetServiceAmount.ToString()//  봉사료.Text
                , creditAuthSimpleExData.SetTaxAmount.ToString()// 세금.Text
                , creditAuthSimpleExData.SetTaxFreeAmt.ToString() // 비과세.Text
                , creditAuthSimpleExData.OriginalAuthNo // 승인번호.Text
                , creditAuthSimpleExData.OriginalAuthDate// 승인일자.Text
                , creditAuthSimpleExData.KeyYn // 키인유무.Text
                , creditAuthSimpleExData.InsYn// 용도구분.Text
                , creditAuthSimpleExData.CashNum // 고객번호.Text
                , creditAuthSimpleExData.CancelReson// 취소사유.Text
                , creditAuthSimpleExData.PtCardCd// " "
                , creditAuthSimpleExData.PInsYn// " "
                , creditAuthSimpleExData.PtCardCd // " "
                );



            creditAuthSimpleExData.ResCode = string.Empty;
            creditAuthSimpleExData.TradNo = string.Empty;
            creditAuthSimpleExData.AuthNumber = string.Empty;
            creditAuthSimpleExData.AuthDate = string.Empty;
            creditAuthSimpleExData.AuthTime = string.Empty;
            creditAuthSimpleExData.ResMsg = string.Empty;

            creditAuthSimpleExData.CardNo = string.Empty;
            //    new string(tagPGFAuthResAppr.strCardKind); /* 카드종류명 */
            creditAuthSimpleExData.IssuerCode = string.Empty;

            creditAuthSimpleExData.IssuerName = string.Empty;
            creditAuthSimpleExData.CardName = string.Empty;
            creditAuthSimpleExData.AcquirerCode = string.Empty;

            creditAuthSimpleExData.AcquirerName = string.Empty;

            creditAuthSimpleExData.MemberNumber = string.Empty;
            if (rtn == 0)
            {
                creditAuthSimpleExData.ResCode = new string(tagPGFAuthResAppr.Header.strAnsCode);
                creditAuthSimpleExData.TradNo = new string(tagPGFAuthResAppr.strTradeNo); /* Van 서 부여하는 거래 고유번호 LJFS */
                creditAuthSimpleExData.AuthNumber = new string(tagPGFAuthResAppr.strAuNo); /* 승인번호 */
                creditAuthSimpleExData.AuthDate = new string(tagPGFAuthResAppr.strTradeDate).Substring(0, 8); /* 승인시간 'YYMMDDhhmmss' */
                creditAuthSimpleExData.AuthTime = new string(tagPGFAuthResAppr.strTradeDate).Substring(8);
                creditAuthSimpleExData.ResMsg = new string(tagPGFAuthResAppr.strMessage).Replace("\0", "").Trim();  /* 응답 메시지(거절 시 응답 Message) */

                creditAuthSimpleExData.CardNo = new string(tagPGFAuthResAppr.strCardNo); /* 카드 BIN */
                //    new string(tagPGFAuthResAppr.strCardKind); /* 카드종류명 */
                creditAuthSimpleExData.IssuerCode = new string(tagPGFAuthResAppr.strOrdCd); /* 발급사 코드 */

                creditAuthSimpleExData.IssuerName = new string(tagPGFAuthResAppr.strOrdNm).Replace("\0", "").Trim();  /* 발급사 명 */
                creditAuthSimpleExData.CardName = creditAuthSimpleExData.IssuerName;
                creditAuthSimpleExData.AcquirerCode = new string(tagPGFAuthResAppr.strInpCd); /* 매입사 코드 */

                creditAuthSimpleExData.AcquirerName = new string(tagPGFAuthResAppr.strInpNm).Replace("\0", "").Trim();  /* 매입사 명 */

                creditAuthSimpleExData.MemberNumber = new string(tagPGFAuthResAppr.strMchNo); /* 가맹점번호 */

            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | CreditAuthSimpleEx", "[신용카드 결제정보]"
                                                                                           + " [응답코드]" + creditAuthSimpleExData.ResCode
                                                                                           + " [거래번호]" + creditAuthSimpleExData.TradNo
                                                                                           + " [승인번호]" + creditAuthSimpleExData.AuthNumber
                                                                                           + " [승인일자]" + creditAuthSimpleExData.AuthDate
                                                                                           + " [승인시간]" + creditAuthSimpleExData.AuthTime
                                                                                           + " [응답메세지]" + creditAuthSimpleExData.ResMsg
                                                                                           + " [카드번호]" + creditAuthSimpleExData.CardNo
                                                                                           + " [발급사코드]" + creditAuthSimpleExData.IssuerCode
                                                                                           + " [발급사명]" + creditAuthSimpleExData.IssuerName
                                                                                           + " [카드사명]" + creditAuthSimpleExData.CardName
                                                                                           + " [매입사코드]" + creditAuthSimpleExData.AcquirerCode
                                                                                           + " [매입사명]" + creditAuthSimpleExData.AcquirerName
                                                                                           + " [가맹정명]" + creditAuthSimpleExData.MemberNumber);



            return creditAuthSimpleExData;
        }
        /// <summary>
        /// 승인취소
        /// </summary>
        /// <param name="pAmount"></param>
        /// <param name="pTax"></param>
        /// <param name="pOrginalAuthNo"></param>
        /// <param name="pOringalAuthDate"></param>
        /// <returns></returns>
        public static CreditAuthSimpleExData CreditAuthSimpleExCancle(int pAmount, int pTax, string pOrginalAuthNo, string pOringalAuthDate)
        {
            int rtn;
            rtn = 0;

            PGFAuthResAppr tagPGFAuthResAppr = new PGFAuthResAppr();
            tagPGFAuthResAppr.Header = new PGFComHead();
            // 승인
            CreditAuthSimpleExData creditAuthSimpleExData = new CreditAuthSimpleExData();
            creditAuthSimpleExData.currentTrade = CreditAuthSimpleExData.TradeType.F2;
            creditAuthSimpleExData.SetAmount(pAmount, pTax);
            creditAuthSimpleExData.OriginalAuthNo = pOrginalAuthNo;
            creditAuthSimpleExData.OriginalAuthDate = pOringalAuthDate;
            creditAuthSimpleExData.CancelReson = "1";


            rtn = KocesICReq(ref tagPGFAuthResAppr,
                creditAuthSimpleExData.currentTrade.ToString() //거래구분
                , creditAuthSimpleExData.AuData  // 가맹점영역
                , creditAuthSimpleExData.Month // 할부
                , creditAuthSimpleExData.SetTrdAmount.ToString() // 거래금액.Text
                , creditAuthSimpleExData.SetTaxAmount.ToString()// 세금.Text
                , creditAuthSimpleExData.SetServiceAmount.ToString()//  봉사료.Text
                , creditAuthSimpleExData.SetTaxFreeAmt.ToString() // 비과세.Text
                , creditAuthSimpleExData.OriginalAuthNo // 승인번호.Text
                , creditAuthSimpleExData.OriginalAuthDate// 승인일자.Text
                , creditAuthSimpleExData.KeyYn // 키인유무.Text
                , creditAuthSimpleExData.InsYn// 용도구분.Text
                , creditAuthSimpleExData.CashNum // 고객번호.Text
                , creditAuthSimpleExData.CancelReson// 취소사유.Text
                , creditAuthSimpleExData.PtCardCd// " "
                , creditAuthSimpleExData.PInsYn// " "
                , creditAuthSimpleExData.PtCardCd // " "
                );


            creditAuthSimpleExData.ResCode = string.Empty;
            creditAuthSimpleExData.TradNo = string.Empty;
            creditAuthSimpleExData.AuthNumber = string.Empty;
            creditAuthSimpleExData.AuthDate = string.Empty;
            creditAuthSimpleExData.AuthTime = string.Empty;
            creditAuthSimpleExData.ResMsg = string.Empty;

            creditAuthSimpleExData.CardNo = string.Empty;

            creditAuthSimpleExData.IssuerCode = string.Empty;

            creditAuthSimpleExData.IssuerName = string.Empty;
            creditAuthSimpleExData.CardName = string.Empty;
            creditAuthSimpleExData.AcquirerCode = string.Empty;

            creditAuthSimpleExData.AcquirerName = string.Empty;

            creditAuthSimpleExData.MemberNumber = string.Empty;
            if (rtn == 0)
            {
                creditAuthSimpleExData.ResCode = new string(tagPGFAuthResAppr.Header.strAnsCode);
                creditAuthSimpleExData.TradNo = new string(tagPGFAuthResAppr.strTradeNo); /* Van 서 부여하는 거래 고유번호 LJFS */
                creditAuthSimpleExData.AuthNumber = new string(tagPGFAuthResAppr.strAuNo); /* 승인번호 */
                creditAuthSimpleExData.AuthDate = new string(tagPGFAuthResAppr.strTradeDate).Substring(0, 8); /* 승인시간 'YYMMDDhhmmss' */
                creditAuthSimpleExData.AuthTime = new string(tagPGFAuthResAppr.strTradeDate).Substring(8);
                creditAuthSimpleExData.ResMsg = new string(tagPGFAuthResAppr.strMessage); /* 응답 메시지(거절 시 응답 Message) */

                creditAuthSimpleExData.CardNo = new string(tagPGFAuthResAppr.strCardNo); /* 카드 BIN */
                //    new string(tagPGFAuthResAppr.strCardKind); /* 카드종류명 */
                creditAuthSimpleExData.IssuerCode = new string(tagPGFAuthResAppr.strOrdCd); /* 발급사 코드 */

                creditAuthSimpleExData.IssuerName = new string(tagPGFAuthResAppr.strOrdNm).Replace("\r", ""); ; /* 발급사 명 */

                creditAuthSimpleExData.AcquirerCode = new string(tagPGFAuthResAppr.strInpCd); /* 매입사 코드 */

                creditAuthSimpleExData.AcquirerName = new string(tagPGFAuthResAppr.strInpNm).Replace("\r", ""); ; /* 매입사 명 */

                creditAuthSimpleExData.MemberNumber = new string(tagPGFAuthResAppr.strMchNo); /* 가맹점번호 */

            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KocesTcmMotor | CreditAuthSimpleEx", "[신용카드 결제취소정보]"
                                                                                           + " [응답코드]" + creditAuthSimpleExData.ResCode
                                                                                           + " [거래번호]" + creditAuthSimpleExData.TradNo
                                                                                           + " [승인번호]" + creditAuthSimpleExData.AuthNumber
                                                                                           + " [승인일자]" + creditAuthSimpleExData.AuthDate
                                                                                           + " [승인시간]" + creditAuthSimpleExData.AuthTime
                                                                                           + " [응답메세지]" + creditAuthSimpleExData.ResMsg
                                                                                           + " [카드번호]" + creditAuthSimpleExData.CardNo
                                                                                           + " [발급사코드]" + creditAuthSimpleExData.IssuerCode
                                                                                           + " [발급사명]" + creditAuthSimpleExData.IssuerName
                                                                                           + " [카드사명]" + creditAuthSimpleExData.CardName
                                                                                           + " [매입사코드]" + creditAuthSimpleExData.AcquirerCode
                                                                                           + " [매입사명]" + creditAuthSimpleExData.AcquirerName
                                                                                           + " [가맹정명]" + creditAuthSimpleExData.MemberNumber);
            return creditAuthSimpleExData;
        }

    }
}
