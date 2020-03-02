using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
//using System.Linq;
using System.Text;

namespace NPCommon.Van
{
    /// <summary>
    /// KICC DIP적용
    /// </summary>
    public class KICC_TIT
    {
        [DllImport("USER32.DLL")]
        public static extern uint FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        // 전문구분 : 신용승인
        public const string KICC_TRANS_CODE_D1 = "D1";

        // 전문구분 : 현금승인(공제)
        public const string KICC_TRANS_CODE_B1 = "B1";

        // 전문구분 : 마지막 자료 재전송
        public const string KICC_TRANS_CODE_LA = "LA";

        // 전문구분 : EPSON 코드 출력
        public const string KICC_TRANS_CODE_EP = "EP";

        // 전문구분 : 전표 재출력 요청
        public const string KICC_TRANS_CODE_PR = "PR";

        // 전문구분 : 취소 (신용거래 창이 닫힌다.)
        public const string KICC_TRANS_CODE_CC = "CC";

        // 전문구분 : 당일취소/반품/환불
        public const string KICC_TRANS_CODE_D4 = "D4";

        // 전문구분 : 현금취소 (공제)
        public const string KICC_TRANS_CODE_B2 = "B2";


        // 현금영수증거래용도 : 개인현금
        public const string KICC_B1_TYPE_00 = "00";

        // 현금영수증거래용도 : 사업자현금
        public const string KICC_B1_TYPE_01 = "01";


        // 할부 : 일시불
        public const string KICC_INSTALL_PLAN_00 = "00";

        // 할부 : 할부 N개월 
        public const string KICC_INSTALL_PLAN_N = "N";


        // KEYIN 허용
        public const string KICC_KEY_IN = "Y";


        // 부가세 : 자동계산
        public const string KICC_VAT_AUTO = "A";

        // 부가세 : 부가세금액
        public const string KICC_VAT_MANU = "M";

        // 부가세 : 면세
        public const string KICC_VAT_EXEMPT = "F";


        // 단말기 구분 : 일반거래
        public const string KICC_DEVICE_TYPE_NORMAL = "40";

        // 단말기 구분 : 텃밭마트(BC현장할인)
        public const string KICC_DEVICE_TYPE_BC = "WV";

        // 단말기 구분 : LG U+ 멤버쉽
        public const string KICC_DEVICE_TYPE_LGUP = "LM";

        // 단말기 구분 : BC Oh! POINT
        public const string KICC_DEVICE_TYPE_BCOP = "OH";

        //TMAP연동
        public const string KICC_USER_CANCLECODE = "999999";
        //TMAP연동 완료

        // 응답결과 : 성공 유무
        public string mRecvStatus = string.Empty;

        public string RecvStatus
        {
            set { mRecvStatus = value; }

            get { return mRecvStatus; }
        }

        /// <summary>
        /// 응답결과 : 실패
        /// </summary>
        public class KICC_TIT_RECV_ERROR
        {
            public string SUC;      // 성공유무

            public string MSG;      // 실패 사유 메시지
        }

        /// <summary>
        /// 응답결과 : 성공
        /// </summary>
        public class KICC_TIT_RECV_SUCCESS
        {
            /// <summary>
            /// 성공유무 00이면성공
            /// </summary>
            public string SUC = string.Empty;      // 성공유무
            /// <summary>
            /// 전문구분
            /// </summary>
            public string RQ01 = string.Empty;     // 전문구분
            /// <summary>
            /// 단말기번호
            /// </summary>
            public string RQ02 = string.Empty;     // 단말기번호
            /// <summary>
            /// 카드입력번호
            /// </summary>
            public string RQ03 = string.Empty;     // 카드입력구분
            /// <summary>
            /// 카드번호
            /// </summary>
            public string RQ04 = string.Empty;     // 카드번호
            /// <summary>
            /// 유효기간
            /// </summary>
            public string RQ05 = string.Empty;     // 유효기간
            /// <summary>
            /// 할부개월
            /// </summary>
            public string RQ06 = string.Empty;     // 할부개월
            /// <summary>
            /// 금액
            /// </summary>
            public string RQ07 = string.Empty;     // 금액
            /// <summary>
            /// 현금영수증거래용도
            /// </summary>
            public string RQ08 = string.Empty;     // 현금영수증 거래용도
            /// <summary>
            /// 상품코드
            /// </summary>
            public string RQ09 = string.Empty;     // 상품코드
            /// <summary>
            /// 원승인번호
            /// </summary>
            public string RQ10 = string.Empty;     // 원승인번호
            /// <summary>
            /// 원승인일자
            /// </summary>
            public string RQ11 = string.Empty;     // 원승인일자
            /// <summary>
            /// 봉사료
            /// </summary>
            public string RQ12 = string.Empty;     // 봉사료
            /// <summary>
            /// 부가세
            /// </summary>
            public string RQ13 = string.Empty;     // 부가세
            /// <summary>
            /// 임시판매번호
            /// </summary>
            public string RQ14 = string.Empty;     // 임시판매번호
            /// <summary>
            /// 웹전송메세지
            /// </summary>
            public string RQ15 = string.Empty;     // 웹전송메시지
            /// <summary>
            /// 거래제어코드
            /// </summary>
            public string RS01 = string.Empty;     // 거래제어코드
            /// <summary>
            /// 정산인덱스
            /// </summary>
            public string RS02 = string.Empty;     // 정산INDEX
            /// <summary>
            /// 거래일련번호
            /// </summary>
            public string RS03 = string.Empty;     // 거래일련번호
            /// <summary>
            /// 응답코드 0000이면 정상 그외 거절
            /// </summary>
            public string RS04 = string.Empty;     // 응답코드
            /// <summary>
            /// 매입사코드
            /// </summary>
            public string RS05 = string.Empty;     // 매입사 코드
            /// <summary>
            /// 매입일련번호
            /// </summary>
            public string RS06 = string.Empty;     // 매입일련번호
            /// <summary>
            /// 승인일시
            /// </summary>
            public string RS07 = string.Empty;     // 승인일시
            /// <summary>
            /// 거래고유번호
            /// </summary>
            public string RS08 = string.Empty;     // 거래 고유번호
            /// <summary>
            /// 승인번호
            /// </summary>
            public string RS09 = string.Empty;     // 승인번호
            /// <summary>
            /// 체크카드유무
            /// </summary>
            public string RS10 = string.Empty;     // 체크카드 유무
            /// <summary>
            /// 발급사코드
            /// </summary>
            public string RS11 = string.Empty;     // 발급사코드
            /// <summary>
            /// 발급사명
            /// </summary>
            public string RS12 = string.Empty;     // 발급사명
            /// <summary>
            /// 가맹점번호
            /// </summary>
            public string RS13 = string.Empty;     // 가맹점 번호
            /// <summary>
            /// 매입사명
            /// </summary>
            public string RS14 = string.Empty;     // 매입사명
            /// <summary>
            /// 화면제어코드
            /// </summary>
            public string RS15 = string.Empty;     // 화면제어코드
            /// <summary>
            /// 화면표시 응답메세지
            /// </summary>
            public string RS16 = string.Empty;     // 화면표시
            /// <summary>
            /// NOTICE
            /// </summary>
            public string RS17 = string.Empty;     // Notice
            /// <summary>
            /// 전자서명유무
            /// </summary>
            public string RS18 = string.Empty;     // 전자서명 유무
            /// <summary>
            /// 사업자번호
            /// </summary>
            public string RS19 = string.Empty;     // 사업자번호
            /// <summary>
            /// 서명BMPKEY
            /// </summary>
            public string RS20 = string.Empty;     // 서명BMP 키
            /// <summary>
            /// MSG
            /// </summary>
            public string MSG = string.Empty;
            public void clear()
            {
                SUC = string.Empty;      // 성공유무

                RQ01 = string.Empty;     // 전문구분
                RQ02 = string.Empty;     // 단말기번호
                RQ03 = string.Empty;     // 카드입력구분
                RQ04 = string.Empty;     // 카드번호
                RQ05 = string.Empty;     // 유효기간
                RQ06 = string.Empty;     // 할부개월
                RQ07 = string.Empty;     // 금액
                RQ08 = string.Empty;     // 현금영수증 거래용도
                RQ09 = string.Empty;     // 상품코드
                RQ10 = string.Empty;     // 원승인번호
                RQ11 = string.Empty;     // 원승인일자
                RQ12 = string.Empty;     // 봉사료
                RQ13 = string.Empty;     // 부가세
                RQ14 = string.Empty;     // 임시판매번호
                RQ15 = string.Empty;     // 웹전송메시지

                RS01 = string.Empty;     // 거래제어코드
                RS02 = string.Empty;     // 정산INDEX
                RS03 = string.Empty;     // 거래일련번호
                RS04 = string.Empty;     // 응답코드
                RS05 = string.Empty;     // 매입사 코드
                RS06 = string.Empty;     // 매입일련번호
                RS07 = string.Empty;     // 승인일시
                RS08 = string.Empty;     // 거래 고유번호
                RS09 = string.Empty;     // 승인번호
                RS10 = string.Empty;     // 체크카드 유무
                RS11 = string.Empty;     // 발급사코드
                RS12 = string.Empty;     // 발급사명
                RS13 = string.Empty;     // 가맹점 번호
                RS14 = string.Empty;     // 매입사명
                RS15 = string.Empty;     // 화면제어코드
                RS16 = string.Empty;     // 화면표시
                RS17 = string.Empty;     // Notice
                RS18 = string.Empty;     // 전자서명 유무
                RS19 = string.Empty;     // 사업자번호
                RS20 = string.Empty;     // 서명BMP 키
                MSG = string.Empty;
            }
        }

        /// <summary>
        /// 응답 전문 데이터 구분
        /// </summary>
        enum KICC_TIT_RECV_MSG
        {
            MSG,                    // 실패 사유 메시지

            SUC,                    // 성공유무

            RQ01,                   // 전문구분
            RQ02,                   // 단말기번호
            RQ03,                   // 카드입력구분
            RQ04,                   // 카드번호
            RQ05,                   // 유효기간
            RQ06,                   // 할부개월
            RQ07,                   // 금액
            RQ08,                   // 현금영수증 거래용도
            RQ09,                   // 상품코드
            RQ10,                   // 원승인번호
            RQ11,                   // 원승인일자
            RQ12,                   // 봉사료
            RQ13,                   // 부가세
            RQ14,                   // 임시판매번호
            RQ15,                   // 웹전송메시지

            RS01,                   // 거래제어코드
            RS02,                   // 정산INDEX
            RS03,                   // 거래일련번호
            RS04,                   // 응답코드
            RS05,                   // 매입사 코드
            RS06,                   // 매입일련번호
            RS07,                   // 승인일시
            RS08,                   // 거래 고유번호
            RS09,                   // 승인번호
            RS10,                   // 체크카드 유무
            RS11,                   // 발급사코드
            RS12,                   // 발급사명
            RS13,                   // 가맹점 번호
            RS14,                   // 매입사명
            RS15,                   // 화면제어코드
            RS16,                   // 화면표시
            RS17,                   // Notice
            RS18,                   // 전자서명 유무
            RS19,                   // 사업자번호
            RS20                    // 서명BMP 키
        }



        // 응답 결과
        KICC_TIT_RECV_SUCCESS mKICC_TIT_Recv_Success = new KICC_TIT_RECV_SUCCESS();

        // 송신 데이터
        StringBuilder mSendData = new StringBuilder();

        HttpWebRequest mSender = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public KICC_TIT()
        {
        }

        /// <summary>
        /// HTTP 초기화
        /// </summary>
        public void InitHTTP()
        {
            Uri sendUri = new Uri("http://127.0.0.1:8090/"); //최신버전 설치 시 8090으로 고정됨.

            mSender = (HttpWebRequest)WebRequest.Create(sendUri);

            mSender.Method = "POST";

            mSender.ContentType = "application/x-www-form-urlencoded";
        }


        /// <summary>
        /// 데이터 설정 : 응답 성공 
        /// </summary>
        /// <param name="ParseData"></param>
        public void SetRecvData(bool isSuccess, JObject ParseData)
        {
            // 성공 여부
            mKICC_TIT_Recv_Success.SUC = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];
            mKICC_TIT_Recv_Success.MSG = (string)ParseData[KICC_TIT_RECV_MSG.MSG.ToString()];
            if (isSuccess == false) // 실패면 리턴
            {
                return;
            }


            // RQ 요청 : 전문구분
            mKICC_TIT_Recv_Success.RQ01 = (string)ParseData[KICC_TIT_RECV_MSG.RQ01.ToString()];

            // RQ 요청 : 단말기번호
            mKICC_TIT_Recv_Success.RQ02 = (string)ParseData[KICC_TIT_RECV_MSG.RQ02.ToString()];

            // RQ 요청 : 카드입력구분
            mKICC_TIT_Recv_Success.RQ03 = (string)ParseData[KICC_TIT_RECV_MSG.RQ03.ToString()];

            // RQ 요청 : 카드번호
            mKICC_TIT_Recv_Success.RQ04 = (string)ParseData[KICC_TIT_RECV_MSG.RQ04.ToString()];

            // RQ 요청 : 유효기간
            mKICC_TIT_Recv_Success.RQ05 = (string)ParseData[KICC_TIT_RECV_MSG.RQ05.ToString()];

            // RQ 요청 : 할부개월
            mKICC_TIT_Recv_Success.RQ06 = (string)ParseData[KICC_TIT_RECV_MSG.RQ06.ToString()];

            // RQ 요청 : 금액
            mKICC_TIT_Recv_Success.RQ07 = (string)ParseData[KICC_TIT_RECV_MSG.RQ07.ToString()];

            // RQ 요청 : 현금영수증 거래용도
            mKICC_TIT_Recv_Success.RQ08 = (string)ParseData[KICC_TIT_RECV_MSG.RQ08.ToString()];

            // RQ 요청 : 상품코드
            mKICC_TIT_Recv_Success.RQ09 = (string)ParseData[KICC_TIT_RECV_MSG.RQ09.ToString()];

            // RQ 요청 : 원승인번호
            mKICC_TIT_Recv_Success.RQ10 = (string)ParseData[KICC_TIT_RECV_MSG.RQ10.ToString()];

            // RQ 요청 : 원승인일자
            mKICC_TIT_Recv_Success.RQ11 = (string)ParseData[KICC_TIT_RECV_MSG.RQ11.ToString()];

            // RQ 요청 : 봉사료
            mKICC_TIT_Recv_Success.RQ12 = (string)ParseData[KICC_TIT_RECV_MSG.RQ12.ToString()];

            // RQ 요청 : 부가세
            mKICC_TIT_Recv_Success.RQ13 = (string)ParseData[KICC_TIT_RECV_MSG.RQ13.ToString()];

            // RQ 요청 : 임시판매번호
            mKICC_TIT_Recv_Success.RQ14 = (string)ParseData[KICC_TIT_RECV_MSG.RQ14.ToString()];

            // RQ 요청 : 웹전송메시지
            mKICC_TIT_Recv_Success.RQ15 = (string)ParseData[KICC_TIT_RECV_MSG.RQ15.ToString()];


            // RS 응답 : 거래제어코드
            mKICC_TIT_Recv_Success.RS01 = (string)ParseData[KICC_TIT_RECV_MSG.RS01.ToString()];

            // RS 응답 : 정산INDEX
            mKICC_TIT_Recv_Success.RS02 = (string)ParseData[KICC_TIT_RECV_MSG.RS02.ToString()];

            // RS 응답 : 거래일련번호
            mKICC_TIT_Recv_Success.RS03 = (string)ParseData[KICC_TIT_RECV_MSG.RS03.ToString()];

            // RS 응답 : 응답코드
            mKICC_TIT_Recv_Success.RS04 = (string)ParseData[KICC_TIT_RECV_MSG.RS04.ToString()];

            // RS 응답 : 매입사코드
            mKICC_TIT_Recv_Success.RS05 = (string)ParseData[KICC_TIT_RECV_MSG.RS05.ToString()];

            // RS 응답 : 매입 일련번호
            mKICC_TIT_Recv_Success.RS06 = (string)ParseData[KICC_TIT_RECV_MSG.RS06.ToString()];

            // RS 응답 : 승인 일시
            mKICC_TIT_Recv_Success.RS07 = (string)ParseData[KICC_TIT_RECV_MSG.RS07.ToString()];

            // RS 응답 : 거래 고유번호
            mKICC_TIT_Recv_Success.RS08 = (string)ParseData[KICC_TIT_RECV_MSG.RS08.ToString()];

            // RS 응답 : 승인 번호
            mKICC_TIT_Recv_Success.RS09 = (string)ParseData[KICC_TIT_RECV_MSG.RS09.ToString()];

            // RS 응답 : 체크카드 유무
            mKICC_TIT_Recv_Success.RS10 = (string)ParseData[KICC_TIT_RECV_MSG.RS10.ToString()];

            // RS 응답 : 발급사코드
            mKICC_TIT_Recv_Success.RS11 = (string)ParseData[KICC_TIT_RECV_MSG.RS11.ToString()];

            // RS 응답 : 발급사명
            mKICC_TIT_Recv_Success.RS12 = (string)ParseData[KICC_TIT_RECV_MSG.RS12.ToString()];

            // RS 응답 : 가맹점 번호
            mKICC_TIT_Recv_Success.RS13 = (string)ParseData[KICC_TIT_RECV_MSG.RS13.ToString()];

            // RS 응답 : 매입사명
            mKICC_TIT_Recv_Success.RS14 = (string)ParseData[KICC_TIT_RECV_MSG.RS14.ToString()];

            // RS 응답 : 화면제어코드
            mKICC_TIT_Recv_Success.RS15 = (string)ParseData[KICC_TIT_RECV_MSG.RS15.ToString()];

            // RS 응답 : 화면표시
            mKICC_TIT_Recv_Success.RS16 = (string)ParseData[KICC_TIT_RECV_MSG.RS16.ToString()];

            // RS 응답 : Notice
            mKICC_TIT_Recv_Success.RS17 = (string)ParseData[KICC_TIT_RECV_MSG.RS17.ToString()];

            // RS 응답 : 전자서명 유무
            mKICC_TIT_Recv_Success.RS18 = (string)ParseData[KICC_TIT_RECV_MSG.RS18.ToString()];

            // RS 응답 : 사업자번호
            mKICC_TIT_Recv_Success.RS19 = (string)ParseData[KICC_TIT_RECV_MSG.RS19.ToString()];

            // RS 응답 : 서명BMP 키
            mKICC_TIT_Recv_Success.RS20 = (string)ParseData[KICC_TIT_RECV_MSG.RS20.ToString()];
        }


        /// <summary>
        /// 데이터 취득 : 응답 성공 
        /// </summary>
        /// <returns></returns>
        public KICC_TIT_RECV_SUCCESS GetRecvData()
        {
            return mKICC_TIT_Recv_Success;
        }

        /// <summary>
        /// 카드삽입상태 확인
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="installplan"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool GetCardInsert(bool retry = false)
        {
            // 수신
            try
            {
                mKICC_TIT_Recv_Success.clear();
                mSendData.Remove(0, mSendData.Length);

                InitHTTP();


                mSendData.Append("callback=jsonp12345678983543344&");
                //KICC 신버전 대응 카드삽입체크 전문 수정
                if (retry)
                {
                    // 전문구분
                    mSendData.Append("REQ=CS^^");


                    // 금액
                    mSendData.Append(string.Format("{0}^", "0"));

                    // 할부
                    mSendData.Append(string.Format("{0}^^^^", "0"));

                    // 임시판매번호
                    mSendData.Append("1234567890^");

                    // 웹전송메시지
                    mSendData.Append("WEB1234567890^^^");

                    // 타임아웃
                    mSendData.Append(string.Format("{0}^^^", "0"));
                }
                else
                {
                    mSendData.Append("REQ=CR^FB^11^34^02^");
                }
                //KICC 신버전 대응 카드삽입체크 전문 수정완료


                // 송신
                mSender.ContentLength = (long)mSendData.ToString().Length;

                StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


                streamWriter.Write(mSendData);

                streamWriter.Flush();

                streamWriter.Close();

                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.LastIndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);


                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];
                    if (RecvStatus != "00")
                    {
                        if (retry)
                        {
                            return false;
                        }
                        if (GetCardInsert(true))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (retry)
                        {
                            if (RecvStatus == "00")
                            {
                                SetRecvData(true, ParseData);
                                return true;
                            }
                            else
                            {
                                SetRecvData(false, ParseData);
                                return false;
                            }
                        }
                        else
                        {
                            string rHexData = (string)ParseData["RHEXDATA"];
                            string carInStat = rHexData.Substring(5, 4);
                            string carLockStat = rHexData.Substring(rHexData.Length - 6, 2);

                            // 응답 결과 : 성공
                            if (RecvStatus == "00" && carInStat == "0101" && carLockStat == "01")
                            {
                                SetRecvData(true, ParseData);
                                return true;
                            }
                            else
                            {
                                SetRecvData(false, ParseData);
                                return false;
                            }
                        }
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 카드배출시 카드가 기기에 있던없던 true반환
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="installplan"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool CardEject()
        {


            // 수신
            try
            {
                mKICC_TIT_Recv_Success.clear();
                mSendData.Remove(0, mSendData.Length);

                InitHTTP();


                mSendData.Append("callback=jsonp12345678983543344&");

                // 전문구분
                mSendData.Append("REQ=EJ^^");

                // 금액
                mSendData.Append(string.Format("{0}^", "0"));

                // 할부
                mSendData.Append(string.Format("{0}^^^^", "0"));

                // 임시판매번호
                mSendData.Append("1234567890^");

                // 웹전송메시지
                mSendData.Append("WEB1234567890^^^");

                // 타임아웃
                mSendData.Append(string.Format("{0}^^^", "0"));


                // 송신
                mSender.ContentLength = (long)mSendData.ToString().Length;

                StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


                streamWriter.Write(mSendData);

                streamWriter.Flush();

                streamWriter.Close();

                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.IndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);


                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];


                    // 응답 결과 : 성공
                    if (RecvStatus == "00")
                    {
                        return true;
                    }
                    else // "01"이면 카드 안들어감
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 신용승인
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="amount"></param>
        /// <param name="installplan"></param>
        /// <param name="timeout"></param>
        public void SendData_D1(string amount /*금액*/, string installplan /*할부기간*/, string timeout /*타임아웃*/)
        {
            mKICC_TIT_Recv_Success.clear();
            mSendData.Remove(0, mSendData.Length);

            InitHTTP();


            mSendData.Append("callback=jsonp12345678983543344&");

            // 전문구분
            mSendData.Append("REQ=D1^^");

            // 금액
            mSendData.Append(string.Format("{0}^", amount));

            // 할부
            mSendData.Append(string.Format("{0}^^^^", installplan));

            // 임시판매번호
            mSendData.Append("1234567890^");

            // 웹전송메시지
            mSendData.Append("WEB1234567890^^^");

            // 타임아웃
            mSendData.Append(string.Format("{0}^^^", timeout)); //타임아웃,부가세,추가필드

            // 송신
            mSender.ContentLength = (long)mSendData.ToString().Length;

            StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


            streamWriter.Write(mSendData);

            streamWriter.Flush();

            streamWriter.Close();


            // 수신
            try
            {
                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.LastIndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);


                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];


                    // 응답 결과 : 성공
                    if (RecvStatus == "00")
                    {
                        SetRecvData(true, ParseData);
                    }
                    else
                    {
                        SetRecvData(false, ParseData);
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// 신용취소
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="approvenum"></param>
        /// <param name="timeout"></param>
        public void SendData_D4(string amount /*금액*/, string approvedate/*원승인일자*/, string approvenum /*승인번호*/, string timeout /*타임아웃*/)
        {
            mKICC_TIT_Recv_Success.clear();
            mSendData.Remove(0, mSendData.Length);

            InitHTTP();


            mSendData.Append("callback=jsonp12345678983543344&");

            // 전문구분
            mSendData.Append("REQ=D4^^");

            // 금액
            mSendData.Append(string.Format("{0}^^", amount));

            // 취소 원승인일자
            mSendData.Append(string.Format("{0}^", approvedate));

            // 취소 원승인번호
            mSendData.Append(string.Format("{0}^", approvenum));

            // 상품코드
            mSendData.Append("23^");

            // 임시판매번호
            mSendData.Append("1234567890^");

            // 웹전송메시지
            mSendData.Append("WEB1234567890^^^");

            // 타임아웃
            mSendData.Append(string.Format("{0}^^", timeout));


            // 송신
            mSender.ContentLength = (long)mSendData.ToString().Length;

            StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


            streamWriter.Write(mSendData);

            streamWriter.Flush();

            streamWriter.Close();


            // 수신
            try
            {
                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.IndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);

                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];


                    // 응답 결과 : 성공
                    if (RecvStatus == "00")
                    {
                        SetRecvData(true, ParseData);
                    }
                    else
                    {
                        SetRecvData(false, ParseData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 현금승인
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="approvenum"></param>
        /// <param name="timeout"></param>
        public void SendData_B1(string amount /*금액*/)
        {
            // 수신
            try
            {
                mKICC_TIT_Recv_Success.clear();
                mSendData.Remove(0, mSendData.Length);

                InitHTTP();


                mSendData.Append("callback=jsonp12345678983543344&");

                // 전문구분
                mSendData.Append("REQ=B1^");

                // 현금영수증 거래용도
                mSendData.Append("00^");

                // 금액
                mSendData.Append(string.Format("{0}^^^^^", amount));

                // 임시판매번호
                mSendData.Append("1234567890^");

                // 웹전송메시지
                mSendData.Append("WEB1234567890^^^^^^");


                // 송신
                mSender.ContentLength = (long)mSendData.ToString().Length;

                StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


                streamWriter.Write(mSendData);

                streamWriter.Flush();

                streamWriter.Close();
                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.IndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);

                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];


                    // 응답 결과 : 성공
                    if (RecvStatus == "00")
                    {
                        SetRecvData(true, ParseData);
                    }
                    else
                    {
                        SetRecvData(false, ParseData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 현금취소
        /// </summary>
        /// <param name="amount"></param>
        public void SendData_B2(string amount /*금액*/, string timeout /*타임아웃*/)
        {
            mSendData.Remove(0, mSendData.Length);

            InitHTTP();


            mSendData.Append("callback=jsonp12345678983543344&");

            // 전문구분
            mSendData.Append("REQ=B2^");

            // 현금영수증 거래용도
            mSendData.Append("00^");

            // 금액
            mSendData.Append(string.Format("{0}^^", amount));

            // 취소 원승인일자
            mSendData.Append(string.Format("{0}^", DateTime.Now.ToString("yyMMdd")));

            // 취소 원승인번호
            mSendData.Append("147139609^");

            // 상품코드
            mSendData.Append("23^");

            // 임시판매번호
            mSendData.Append("1234567890^");

            // 웹전송메시지
            mSendData.Append("WEB1234567890^^^^");

            // 타임아웃
            mSendData.Append(string.Format("{0}^^", timeout));


            // 송신
            mSender.ContentLength = (long)mSendData.ToString().Length;

            StreamWriter streamWriter = new StreamWriter(mSender.GetRequestStream());


            streamWriter.Write(mSendData);

            streamWriter.Flush();

            streamWriter.Close();


            // 수신
            try
            {
                HttpWebResponse resp = (HttpWebResponse)mSender.GetResponse();

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    // 수신 데이터 취득
                    Stream RespStream = resp.GetResponseStream();

                    StreamReader sr = new StreamReader(RespStream, Encoding.Default, true);

                    String RecvData = sr.ReadToEnd();

                    sr.Close();

                    RespStream.Close();


                    // JSON 포맷 데이터만 취득
                    int sPos = RecvData.IndexOf('{');

                    int ePos = RecvData.IndexOf(')');

                    RecvData = RecvData.Substring(sPos, ePos - sPos);


                    // 파싱
                    JObject ParseData = JObject.Parse(RecvData);

                    RecvStatus = (string)ParseData[KICC_TIT_RECV_MSG.SUC.ToString()];


                    // 응답 결과 : 성공
                    if (RecvStatus == "00")
                    {
                        SetRecvData(true, ParseData);
                    }
                    else
                    {
                        SetRecvData(false, ParseData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
