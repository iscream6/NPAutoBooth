using FadeFox.Text;
using System;
using System.Text.RegularExpressions;

namespace NPCommon.Van
{
    public class Smatro_TITDIP_EVCAT
    {
        #region 속성
        #region EV-CAT 연동통신 멤버 선언


        #region 송신 구조체
        public class SEND_APPROVAL_DATA
        {
            public string sStx;             // 시작문자[2 Byte] - Char(7) & Char(6)
            public string sPayType;         // 결제/제어 방법[4 Byte] - 1000(카드), 2299(카드삽입 이벤트 요청), 2288(카드삽입 상태 리턴), 5555(결제금액변경), 8811(리더기 상태 확인), 8899(EVCAT 상태 확인)
            public string sFS01;            // File Separator1 [1 Byte] - Char(28)
            public string sPayGubun;        // 결제 구분[4 Byte] - 1100(승인), 2200(취소), 2200(결제금액변경), 0000(상태확인)
            public string sFS02;            // File Separator2 [1 Byte] - Char(28)
            public string sPayMoney;        // 결제금액[* Byte]
            public string sFS03;            // File Separator3 [1 Byte] - Char(28)
            public string sVat;             // 부가세[* Byte]
            public string sFS04;            // File Separator4 [1 Byte] - Char(28)
            public string sServiceMoney;    // 봉사료[* Byte]
            public string sFS05;            // File Separator5 [1 Byte] - Char(28)
            public string sInstMonth;       // 할부기간[2 Byte]
            public string sFS06;            // File Separator6 [1 Byte] - Char(28)
            public string sFilter;          // Filter[- Byte]
            public string sFS07;            // File Separator7 [1 Byte] - Char(28)
            public string sCashReceptNo;    // 현금영수증 번호[- Byte]
            public string sFS08;            // File Separator8 [1 Byte] - Char(28)
            public string sCashReceptGubun; // 현금영수증 구분[1 Byte]
            public string sFS09;            // File Separator9 [1 Byte] - Char(28)
            public string sAcceptDate;      // 원거래일[8 Byte]
            public string sFS10;            // File Separator10 [1 Byte] - Char(28)
            public string sAcceptNo;        // 원승인번호[10 Byte]
            public string sFS11;            // File Separator11 [1 Byte] - Char(28)
            public string sUserFixNo;        // 사용자 고유 번호[14 Byte]
            public string sFS12;            // File Separator12 [1 Byte] - Char(28)
            //public PGData sPGData;          // PG 자료[350 Byte] 
            public string sBlTID;          // BL TID[40 Byte]
            public string sBuyName;        // 구매자명[30 Byte]
            public string sBuyPNum;        // 전화번호[20 Byte] 
            public string sBuyEmail;       // 이메일[40 Byte] 
            public string sBuyMNum;        // 구매자 핸드폰번호[20 Byte]
            public string sProductName;    // 제품명[50 Byte]
            public string sAddress;        // 주소[100 Byte]
            public string sRecieverMsg;    // 수취인 메세지[50 Byte]
            public string sFS13;            // File Separator13 [1 Byte] - Char(28)
            //public MuCardCancleData sMuCardCancleData; // 무카드 취소자료[8 Byte]
            public string sAccecptGubun;   // 승인구분[2 Byte]
            public string sHT01;           // Horizontal Tab1[1 Byte] - Char(9)
            public string sMuCardAcceptNo;       // 승인시 거래 전문일련번호[3 Byte]
            public string sHT02;           // Horizontal Tab2[1 Byte] - Char(9)
            public string sPosID;          // POS ID[1 Byte]
            public string sFS14;            // File Separator14 [1 Byte] - Char(28)
            public string sSpace01;         // 여유필드1[* Byte]
            public string sFS15;            // File Separator15 [1 Byte] - Char(28)
            public string sSpace02;         // 여유필드2[* Byte]
            public string sAck;             // Acknowledgment[1 Byte] - Char(6)
            public string sEtx;             // Carriage Return[1 Byte] - Char(13)

            //public class PGData            // PG 자료[350 Byte]
            //{
            //    public string sBlTID;          // BL TID[40 Byte]
            //    public string sBuyName;        // 구매자명[30 Byte]
            //    public string sBuyPNum;        // 전화번호[20 Byte] 
            //    public string sBuyEmail;       // 이메일[40 Byte] 
            //    public string sBuyMNum;        // 구매자 핸드폰번호[20 Byte]
            //    public string sProductName;    // 제품명[50 Byte]
            //    public string sAddress;        // 주소[100 Byte]
            //    public string sRecieverMsg;    // 수취인 메세지[50 Byte]
            //}
            //public class MuCardCancleData  // 무카드 취소자료[8 Byte]
            //{
            //    public string sAccecptGubun;   // 승인구분[2 Byte]
            //    public string sHT01;           // Horizontal Tab1[1 Byte] - Char(9)
            //    public string sAcceptNo;       // 승인시 거래 전문일련번호[3 Byte]
            //    public string sHT02;           // Horizontal Tab2[1 Byte] - Char(9)
            //    public string sPosID;          // POS ID[1 Byte]
            //}

            public void InitSendData()
            {
                this.sStx = STX.ToString() + ASK.ToString();
                this.sFS01 = this.sFS02 = this.sFS03 = this.sFS04 = this.sFS05 = this.sFS06 = this.sFS07 = this.sFS08 = this.sFS09 = this.sFS10 = this.sFS11 = this.sFS12 = this.sFS13 = this.sFS14 = this.sFS15 = FS.ToString();
                this.sAck = ASK.ToString();
                this.sEtx = CR.ToString();


                this.sBlTID = getWordByByte("" + "".PadLeft(40, ' '), 40);
                this.sBuyName = getWordByByte("" + "".PadLeft(40, ' '), 40);
                this.sBuyPNum = getWordByByte("" + "".PadLeft(20, ' '), 20);
                this.sBuyEmail = getWordByByte("" + "".PadLeft(40, ' '), 40);
                this.sBuyMNum = getWordByByte("" + "".PadLeft(20, ' '), 20);
                this.sProductName = getWordByByte("" + "".PadLeft(50, ' '), 50);
                this.sAddress = getWordByByte("" + "".PadLeft(100, ' '), 100);
                this.sRecieverMsg = getWordByByte("" + "".PadLeft(50, ' '), 50);
                this.sServiceMoney = this.sInstMonth = "0";
                this.sAccecptGubun = this.sHT01 = this.sAcceptNo = this.sHT02 = this.sPosID = string.Empty;
                //this.sMuCardCancleData.sHT01 = this.sMuCardCancleData.sHT02 = HT.ToString();

            }

            public void ClearSendData()
            {
                this.sPayType = string.Empty;
                this.sPayGubun = string.Empty;        // 결제 구분[4 Byte] - 1100(승인), 2200(취소), 2200(결제금액변경), 0000(상태확인)
                this.sPayMoney = string.Empty;        // 결제금액[* Byte]
                this.sVat = string.Empty;             // 부가세[* Byte]
                this.sServiceMoney = string.Empty;    // 봉사료[* Byte]
                this.sInstMonth = string.Empty;       // 할부기간[2 Byte]
                this.sFilter = string.Empty;          // Filter[- Byte]
                this.sCashReceptNo = string.Empty;    // 현금영수증 번호[- Byte]
                this.sCashReceptGubun = string.Empty; // 현금영수증 구분[1 Byte]
                this.sAcceptDate = string.Empty;      // 원거래일[8 Byte]
                this.sAcceptNo = string.Empty;        // 원승인번호[10 Byte]
                this.sUserFixNo = string.Empty;       // 사용자 고유 번호[14 Byte]
                this.sSpace01 = string.Empty;         // 여유필드1[* Byte]
                this.sSpace02 = string.Empty;         // 여유필드2[* Byte]
            }
        }
        #endregion

        #region 수신 구조체
        /// <summary>
        /// Smatro_TITDIP_EVCAT 결제 승인 수신 데이터 구조체
        /// </summary>
        public class RECV_APPROVAL_DATA
        {
            public string rStx;                 // 시작문자[2 Byte] - Char(7)
            public string rUserFixNo;           // 사용자 고유 번호[14 Byte]
            public string rACK01;               // Acknowledgment1[1 Byte] - Char(6)
            public string rResultCode;          // 응답코드[1 Byte] - Y(성공), E(에러), N(실패) 
            public string rACK02;               // Acknowledgment2[1 Byte] - Char(6)
            public string rProcGuBun;           // 처리 구분자
            public string rACK03;               // Acknowledgment3[1 Byte] - Char(6)
            public string rVanResultCode;       // VAN사 응답코드[2 Byte]] - 00(정상승인), 그외 거절 코드
            public string rFS01;                // File Separator1 [1 Byte] - Char(28)
            public string rPayGubun;            // 결제구분[2 Byte]
            public string rFS02;                // File Separator2 [1 Byte] - Char(28)
            public string rPayMedia;            // 결제매체[2 Byte]
            public string rFS03;                // File Separator3 [1 Byte] - Char(28)
            public string rAcceptDate;          // 결제일자[8 Byte]
            public string rFS04;                // File Separator4 [1 Byte] - Char(28)
            public string rAcceptTime;          // 결제시간[6 Byte]
            public string rFS05;                // File Separator5 [1 Byte] - Char(28)
            public string rCardNum;             // 응답카드번호[16 Byte] - 123456**********
            public string rFS06;                // File Separator6 [1 Byte] - Char(28)
            public string rAcceptNo;            // 응답승인번호[12 Byte]
            public string rFS07;                // File Separator7 [1 Byte] - Char(28)
            public string rFranchiNo;           // 가맹점번호[12 Byte]
            public string rFS08;                // File Separator8 [1 Byte] - Char(28)
            public string rPurchaseGubun;       // 매입구분[1 Byte]
            public string rFS09;                // File Separator9 [1 Byte] - Char(28)
            public string rIssuerCode;          // 발급사코드[4 Byte]
            public string rFS10;                // File Separator10 [1 Byte] - Char(28)
            public string rIssuerName;          // 발급사명[* Byte]
            public string rFS11;                // File Separator11 [1 Byte] - Char(28)
            public string rPurchaseCode;        // 매입사코드[4 Byte]
            public string rsFS12;               // File Separator12 [1 Byte] - Char(28)
            public string rPurchaseName;        // 매입사명[4 Byte]
            public string rFS13;                // File Separator13 [1 Byte] - Char(28)
            public string rReceiveMsg;          // 응답 내용[60 Byte]
            public string rFS14;                // File Separator14 [1 Byte] - Char(28)
            public string rPrePayCardMoney;     // 선불카드 잔액(기프트카드)[* Byte]
            public string rFS15;                // File Separator15 [1 Byte] - Char(28)
            public string rPrePayCardClaimData; // 선불카드(티머니) 청구용 자료[100 Byte]
            public string rFS16;                // File Separator16 [1 Byte] - Char(28)
            public string rPosUserFixNo;        // POS 응답 사용자 고유 번호[14 Byte]
            public string rFS17;                // File Separator17 [1 Byte] - Char(28)
            public string rPayMoney;            // 결제금액[* Byte]
            public string rFS18;                // File Separator18 [1 Byte] - Char(28)
            public string rVat;                 // 부가세[* Byte]
            public string rFS19;                // File Separator19 [1 Byte] - Char(28)
            public string rPayPosID;            // 결제 POS ID[20 Byte]
            public string rFS20;                // File Separator20 [1 Byte] - Char(28)
            public string rSpecNo;              // 전문 일련번호[6 Byte]
            public string rFS21;                // File Separator21 [1 Byte] - Char(28)
            public string rPGTradNo;            // PG거래 일련번호[30 Byte]
            public string sEtx;                 // ETX[1 Byte] - Char(3)

            public void ClearRecvData()
            {
                this.rUserFixNo = string.Empty;           // 사용자 고유 번호[14 Byte]
                this.rResultCode = string.Empty;          // 응답코드[1 Byte] - Y(성공), E(에러), N(실패) 
                this.rProcGuBun = string.Empty;           // 처리 구분자
                this.rVanResultCode = string.Empty;       // VAN사 응답코드[2 Byte]] - 00(정상승인), 그외 거절 코드
                this.rPayGubun = string.Empty;            // 결제구분[2 Byte]
                this.rPayMedia = string.Empty;            // 결제매체[2 Byte]
                this.rAcceptDate = string.Empty;          // 결제일자[8 Byte]
                this.rAcceptTime = string.Empty;          // 결제시간[6 Byte]
                this.rCardNum = string.Empty;             // 응답카드번호[16 Byte] - 123456**********
                this.rAcceptNo = string.Empty;            // 응답승인번호[12 Byte]
                this.rFranchiNo = string.Empty;           // 가맹점번호[12 Byte]
                this.rPurchaseGubun = string.Empty;       // 매입구분[1 Byte]
                this.rIssuerCode = string.Empty;          // 발급사코드[4 Byte]
                this.rIssuerName = string.Empty;          // 발급사명[* Byte]
                this.rPurchaseCode = string.Empty;        // 매입사코드[4 Byte]
                this.rPurchaseName = string.Empty;        // 매입사명[4 Byte]
                this.rReceiveMsg = string.Empty;          // 응답 내용[60 Byte]
                this.rPrePayCardMoney = string.Empty;     // 선불카드 잔액(기프트카드)[* Byte]
                this.rPrePayCardClaimData = string.Empty; // 선불카드(티머니) 청구용 자료[100 Byte]
                this.rPosUserFixNo = string.Empty;        // POS 응답 사용자 고유 번호[14 Byte]
                this.rPayMoney = string.Empty;            // 결제금액[* Byte]
                this.rVat = string.Empty;                 // 부가세[* Byte]
                this.rPayPosID = string.Empty;            // 결제 POS ID[20 Byte]
                this.rSpecNo = string.Empty;              // 전문 일련번호[6 Byte]
                this.rPGTradNo = string.Empty;            // PG거래 일련번호[30 Byte]
            }
        }
        #endregion


        #endregion

        #region 멤버 변수
        private SEND_APPROVAL_DATA mSEND_APPROVAL_DATA = new SEND_APPROVAL_DATA();
        private RECV_APPROVAL_DATA mRECV_APPROVAL_DATA = new RECV_APPROVAL_DATA();

        public SEND_APPROVAL_DATA SendInfo { get { return mSEND_APPROVAL_DATA; } set { mSEND_APPROVAL_DATA = value; } }
        public RECV_APPROVAL_DATA RecvInfo { get { return mRECV_APPROVAL_DATA; } set { mRECV_APPROVAL_DATA = value; } }

        public delegate void RunStatus(bool flag);
        public event RunStatus eventRunStatus;

        private const char ASK = (char)0x06;
        private const char STX = (char)0x07;
        private const char HT = (char)0x09;
        private const char CR = (char)0x0D;
        private const char FS = (char)0x1C;
        private const char ETX = (char)0x03;

        private string mEVCAT_IP;
        public string EVCAT_IP
        {
            get { return mEVCAT_IP; }
            set { mEVCAT_IP = value; }
        }

        private bool misConnect = false;
        public bool isConnect
        {
            get { return misConnect; }
            set { misConnect = value; }
        }

        private int mStartSoundTick = 0;
        public int StartSoundTick
        {
            set { mStartSoundTick = value; }
            get { return mStartSoundTick; }
        }
        #endregion


        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        public Smatro_TITDIP_EVCAT()
        {
            this.SendInfo.InitSendData();
        }

        #region EV-CAT 데몬 관련
        /// <summary>
        /// EV-CAT 데몬 상태 확인
        /// </summary>
        public void EVCATStat(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT)
        {
            this.mSEND_APPROVAL_DATA.ClearSendData();
            this.mSEND_APPROVAL_DATA.sPayType = "8899";
            this.mSEND_APPROVAL_DATA.sPayGubun = "0000";
            this.mSEND_APPROVAL_DATA.sPayMoney = "0";
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }

        /// <summary>
        /// EV-CAT 데몬 재시작
        /// </summary>
        /// <returns></returns>
        public void EVCATRestart(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT)
        {
            this.mSEND_APPROVAL_DATA.ClearSendData();
            this.mSEND_APPROVAL_DATA.sPayType = "7777";
            this.mSEND_APPROVAL_DATA.sPayGubun = "0000";
            this.mSEND_APPROVAL_DATA.sPayMoney = "0";
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }
        #endregion

        #region 카드 리더기 관련 함수
        /// <summary>
        /// 카드리더기 상태확인
        /// </summary>
        /// <returns></returns>
        public void CardReaderStat(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT)
        {
            this.mSEND_APPROVAL_DATA.ClearSendData();
            this.mSEND_APPROVAL_DATA.sPayType = "8811";
            this.mSEND_APPROVAL_DATA.sPayGubun = "0000";
            this.mSEND_APPROVAL_DATA.sPayMoney = "0";
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }

        /// <summary>
        /// 카드리더기 현재 요금취소 및 배출 ( 사용시 카드배출완료 이벤트가 떨어짐 이벤트 응답번에 요금요청을 하면 이벤트가 오지않음)
        /// </summary>
        /// <returns></returns>
        public void CardReaderReset(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT)
        {
            bool flag = false;
            evCAT.set_xProcess(ref flag);
            this.mSEND_APPROVAL_DATA.ClearSendData();
            this.mSEND_APPROVAL_DATA.sPayType = "8888";
            this.mSEND_APPROVAL_DATA.sPayGubun = "2200";
            this.mSEND_APPROVAL_DATA.sPayMoney = "0";
            this.mSEND_APPROVAL_DATA.sVat = "0";
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";
            this.mSEND_APPROVAL_DATA.sInstMonth = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }
        #endregion

        #region 카드결제/취소 관련
        /// <summary>
        /// 요금결제 사용시 정상적이면 이벤트가 없고 오류일결우만 이미실행중이라고 나옴
        /// </summary>
        /// <returns></returns>
        public void CardApproval(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT, string Amount)
        {
            this.mSEND_APPROVAL_DATA.sPayType = "1000";
            this.mSEND_APPROVAL_DATA.sPayGubun = "1100";
            this.mSEND_APPROVAL_DATA.sPayMoney = Amount;
            this.mSEND_APPROVAL_DATA.sVat = "0";
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";
            this.mSEND_APPROVAL_DATA.sInstMonth = "00";
            string pgData = string.Empty;
            pgData = getWordByByte("".PadLeft(350, ' '), 350);
            //this.mSEND_APPROVAL_DATA.sUserFixNo = "123456789000";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sAcceptDate
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sAcceptNo
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + pgData
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }

        /// <summary>
        /// 결제요금 변경(카드승인이 없는 상태에서 변경시에만 이벤트로 오류가 오고 정상인경우 값이 오지않음)
        /// </summary>
        /// <returns></returns>
        public void ChangePayMoney(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT, string Amount)
        {
            this.mSEND_APPROVAL_DATA.sPayType = "5555";
            this.mSEND_APPROVAL_DATA.sPayGubun = "2200";
            this.mSEND_APPROVAL_DATA.sPayMoney = Amount;
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sAcceptDate
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sAcceptNo
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }

        /// <summary>
        /// 결제취소
        /// </summary>
        /// <returns></returns>
        public void CanclePayment(AxDreampos_Ocx.AxDP_Certification_Ocx evCAT, string Amount, string AcceptDate, string AcceptNo)
        {
            this.mSEND_APPROVAL_DATA.sPayType = "1000";
            this.mSEND_APPROVAL_DATA.sPayGubun = "2200";
            this.mSEND_APPROVAL_DATA.sPayMoney = Amount;
            this.mSEND_APPROVAL_DATA.sAcceptDate = AcceptDate;
            this.mSEND_APPROVAL_DATA.sAcceptNo = AcceptNo;
            this.mSEND_APPROVAL_DATA.sServiceMoney = "0";

            string sendData = this.mSEND_APPROVAL_DATA.sStx
                            + this.mSEND_APPROVAL_DATA.sPayType
                            + this.mSEND_APPROVAL_DATA.sFS01
                            + this.mSEND_APPROVAL_DATA.sPayGubun
                            + this.mSEND_APPROVAL_DATA.sFS02
                            + this.mSEND_APPROVAL_DATA.sPayMoney
                            + this.mSEND_APPROVAL_DATA.sFS03
                            + this.mSEND_APPROVAL_DATA.sVat
                            + this.mSEND_APPROVAL_DATA.sFS04
                            + this.mSEND_APPROVAL_DATA.sServiceMoney
                            + this.mSEND_APPROVAL_DATA.sFS05
                            + this.mSEND_APPROVAL_DATA.sInstMonth
                            + this.mSEND_APPROVAL_DATA.sFS06
                            + this.mSEND_APPROVAL_DATA.sFS07
                            + this.mSEND_APPROVAL_DATA.sFS08
                            + this.mSEND_APPROVAL_DATA.sFS09
                            + this.mSEND_APPROVAL_DATA.sAcceptDate
                            + this.mSEND_APPROVAL_DATA.sFS10
                            + this.mSEND_APPROVAL_DATA.sAcceptNo
                            + this.mSEND_APPROVAL_DATA.sFS11
                            + this.mSEND_APPROVAL_DATA.sUserFixNo
                            + this.mSEND_APPROVAL_DATA.sFS12
                            + this.mSEND_APPROVAL_DATA.sFS13
                            + this.mSEND_APPROVAL_DATA.sFS14
                            + this.mSEND_APPROVAL_DATA.sFS15
                            + this.mSEND_APPROVAL_DATA.sAck
                            + this.mSEND_APPROVAL_DATA.sEtx;

            evCAT.set_OcxHostSendJunMun(ref sendData);
            evCAT.SendRecv();
        }
        #endregion

        #region 기타

        /// <summary>
        /// EV-CAT 수신데이터 파싱
        /// </summary>
        /// <param name="rData"></param>
        /// <remarks>
        /// 2020-01-15 : 카드실패전송 bool로 변경 실패확인을 위해 리턴코드 확인 
        /// </remarks>
        public bool ParsingData(string resultData)
        {
            try
            {
                string[] resultInfo = Regex.Split(resultData, ((Char)28).ToString());

                this.mRECV_APPROVAL_DATA.rVanResultCode = resultInfo[0];
                this.mRECV_APPROVAL_DATA.rPayGubun = resultInfo[1];
                this.mRECV_APPROVAL_DATA.rPayMedia = resultInfo[2];
                this.mRECV_APPROVAL_DATA.rAcceptDate = resultInfo[3];
                this.mRECV_APPROVAL_DATA.rAcceptTime = resultInfo[4];
                this.mRECV_APPROVAL_DATA.rCardNum = resultInfo[5];
                this.mRECV_APPROVAL_DATA.rAcceptNo = resultInfo[6];
                this.mRECV_APPROVAL_DATA.rFranchiNo = resultInfo[7];
                this.mRECV_APPROVAL_DATA.rPurchaseGubun = resultInfo[8];
                this.mRECV_APPROVAL_DATA.rIssuerCode = resultInfo[9];
                this.mRECV_APPROVAL_DATA.rIssuerName = resultInfo[10];
                this.mRECV_APPROVAL_DATA.rPurchaseCode = resultInfo[11];
                this.mRECV_APPROVAL_DATA.rPurchaseName = resultInfo[12];
                this.mRECV_APPROVAL_DATA.rReceiveMsg = resultInfo[13];
                this.mRECV_APPROVAL_DATA.rPrePayCardMoney = resultInfo[14];
                this.mRECV_APPROVAL_DATA.rPrePayCardClaimData = resultInfo[15];
                this.mRECV_APPROVAL_DATA.rPosUserFixNo = resultInfo[16];
                this.mRECV_APPROVAL_DATA.rPayMoney = resultInfo[17];
                this.mRECV_APPROVAL_DATA.rVat = resultInfo[18];
                if (resultData.Length >= 19)
                {
                    this.mRECV_APPROVAL_DATA.rPayPosID = resultInfo[19];
                }
                if (resultData.Length >= 20)
                {
                    this.mRECV_APPROVAL_DATA.rSpecNo = resultInfo[20];
                }
                if (resultData.Length >= 22)
                {
                    this.mRECV_APPROVAL_DATA.rPGTradNo = resultInfo[21];
                }
                //TMAP연동 : 카드실패전송 bool로 변경 실패확인을 위해 리턴코드 확인 
                return true;
            }
            catch (Exception ex)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "Smatro_TITDIP_EVCAT || ParsingData 예외상황발생 : ", ex.ToString());
                //TMAP연동 : 카드실패전송 bool로 변경 실패확인을 위해 리턴코드 확인 
                return false;
            }
        }
        
        public static string getWordByByte(string src, int byteCount)
        {
            System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987");

            byte[] buf = myEncoding.GetBytes(src);

            return myEncoding.GetString(buf, 0, byteCount);
        }

        public void SmartroEvcat_QueryResults(object sender, AxDreampos_Ocx.__DP_Certification_Ocx_QueryResultsEvent e)
        {
            try
            {
                if (e.returnData == "FALL BACK 발생" || e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData", " 이벤트 [" + e.returnData + "] ");
                    eventRunStatus(false);
                    return;
                }

                if (string.IsNullOrEmpty(e.returnData))
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData ", " 응답값 없어 처리를 안함 ");
                    eventRunStatus(false);
                    return;
                }
                TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData [수신 전문 파싱 시작] ", e.returnData);
                if (e.returnData.Contains(((char)7).ToString()) && e.returnData.Contains(((char)3).ToString()))
                {

                    string recvData = e.returnData.Substring(e.returnData.IndexOf((char)7) + 1, e.returnData.IndexOf((char)3) - e.returnData.IndexOf((char)7) - 1);
                    string[] splitData = Regex.Split(recvData, ((char)6).ToString());

                    this.RecvInfo.ClearRecvData();
                    this.RecvInfo.rUserFixNo = splitData[0];
                    this.RecvInfo.rResultCode = splitData[1];
                    this.RecvInfo.rProcGuBun = splitData[2];
                    this.RecvInfo.rReceiveMsg = splitData[3];

                    if (this.RecvInfo.rResultCode == "C")
                    {
                        if (this.RecvInfo.rReceiveMsg == "LIVE")
                        {
                            this.isConnect = true;
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " EV-CAT 데몬 정상실행 확인 ");
                            eventRunStatus(true);
                            return;
                        }
                        else if (this.RecvInfo.rReceiveMsg == "00")
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 리더기 정상연결 ");
                            eventRunStatus(true);
                            return;
                        }
                        else if (this.RecvInfo.rReceiveMsg == "RESET")
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 카드리더기 리셋(강제배출) 성공 ");
                            eventRunStatus(true);
                            return;
                        }
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "리더기 상태 " + this.RecvInfo.rReceiveMsg);
                        eventRunStatus(false);
                        return;
                    }
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || SmartroEvcat_QueryResults ", " 비정상 응답 [ " + e.returnData + " ]");
                    eventRunStatus(false);
                    return;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SMATRO_TIT_DIP_EV-CAT || SmartroEvcat_QueryResults ", " 전문 파싱중 예외상황 [ " + ex.Message + " ]");
                eventRunStatus(false);
                return;
            }

        }
        #endregion
    }
}
