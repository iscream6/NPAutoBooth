using FadeFox.Database;
using FadeFox.Database.SQLite;
using FadeFox.IO;
using FadeFox.Security;
using FadeFox.Text;
using FadeFox.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPCommon.DEVICE;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.NICE;
using NPCommon.IO;
using NPCommon.REST;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NPCommon
{
    #region ENUM

    public enum AdminCashLogType
    {
        None,
        Setting,
        HopperCoinOut,
        PaymentOut,
        PaymentIn
    }

    public enum InfoStatus
    {
        CompletCar,
        NotCarSearch,
        SeviceCar,
        Commuter,
        KakaoMember,
        NoRegExtensCar,
        NoRegExtensDiscount,
        AddPayment,
        NotEnoghfMoney,
        CorrectCard,
        NotOutCard,
        NotCardPay,
        /// <summary>
        /// 잘못된 할인권
        /// </summary>
        NoDiscountTicket,
        /// <summary>
        /// 동일한할인권 사용제한
        /// </summary>
        DuplicateDiscountTicket,
        /// <summary>
        /// 할인권 수량제한
        /// </summary>
        NoAddDiscountTIcket,
        NoBarcode,
        DuplicateBarcode,
        ExitBooth,
    }

    public enum PaymentLogResultType
    {
        None,
        Success,                // 정상 처리
        CashTicketCharge,       // 정산후 거스름돈 부족에 대한 보관증 발급
        CashTicketRefund,       // 투입금액에 대한 거스름돈 부죽에 대한 보관증 발급
        NotEnoughCharge,        // 거스름돈 부족
        NotEnoughCurrentMoney,  // 투입금액 부족
        NotEnoughTmoneyMoney,   // Tmoney
        ErrorCoinOut,           // 동전방출 오류
        ErrorBillOut,           // 지폐방출 오류
        ErrorUnknown,           // 알수 없는 에러
        DeviceError
    }

    public enum MoneyType
    {
        None,
        Coin50,      // 동전
        Coin100,     // 동전
        Coin500,     // 동전
        Bill1000,    // 지폐 1000
        Bill5000,    // 지폐 5000
        Bill10000,   // 지폐 10000
        Bill50000,   // 지폐 50000
        CreditCard,  // 신용카드
        TmoneyCard,  // 교툥카드
        CashTicket   // 캐쉬티켓
    }

    public enum DiscountReadingFormat
    {
        TRACK2ISO_TRACK3105,
        TRACK2ISO_TRACK3210
    }

    public enum Car_Type
    {
        JUNGI,
        FREETIME,
        NORMAL
    }

    /// <summary>
    /// 요금결제과 가능한지의에 대한 리턴값 Success 문제없음, NotSuccess 문제있지만 사용가능,Fail 문제가 있고 요금메뉴를 사용할수 없는상태
    /// </summary>
    public enum isPaymentUse
    {
        Success,
        NotSuccess,
        Fail
    }

    /// <summary>
    /// 장비명 enumtype
    /// </summary>
    public enum DeviceTypeName
    {
        /// <summary>
        /// 입차LPR
        /// </summary>
        InLpr = 8,
        /// <summary>
        /// 출차LPR
        /// </summary>
        OutLpr = 9,
        /// <summary>
        /// 출차전광판
        /// </summary>
        OutDisplay = 11,
        /// <summary>
        /// 사전무인정산기
        /// </summary>
        PreBooth = 12,
        /// <summary>
        /// 출구무인정산기
        /// </summary>
        AutoBooth = 13
    }

    #endregion ENUM

    /// 2019.04.09 RT 1.0.00 웹통합처리
    /// 2019.05.02 RT 1.0.01 시티벨리 설치할버젼
    /// 2019.05.02 RT 1.0.02 마그네틱할인권 적용버젼
    /// 2019.05.16 RT 1.0.03 이미지경로불러올때
    /// 2019.05.20 RT 1.0.04 재전송부 구현
    /// 2019.06.18 RT 1.0.05 inrego outregon삭제
    /// 2019.06.27 RT 1.0.06 일본향 기능 적용
    /// 2019.08.13 RT 1.0.07 스마트로 TIT_DIP EV-CAT 적용
    /// 2019.08.27 RT 1.0.08 장비테스트 언어번역 추가
    /// 2019.09.06 RT 1.0.09 센트럴폴리스 관련처리
    /// 2019.09.18 RT 1.0.10 길임시장 현금처리
    /// 2019.09.30 RT 1.0.11 길임시장 현금처리 보강
    /// 2019.10.01 RT 1.0.12 시제설정누락처리
    /// 2019.10.08 RT 1.0.13 사전무인정산기 폼보강
    /// 2019.10.30 RT 1.0.14 바코드할인 취소기능추가
    /// 2019.11.05 RT 1.0.15 보관증 사전정산부분 추가
    /// 2019.11.10 RT 1.0.16 통신상태 전송기능
    /// 2019.12.09 RT 1.0.17 영수증 공급가/부가세 출력 적용
    /// 2019.12.20 RT 1.0.18 TMAP연동
    /// 2019.12.24 RT 1.0.19 카드실패전송
    public class NPSYS
    {
        #region ENUM

        public enum FormType
        {
            MENU,
            NONE,
            Magam,
            Login,
            Main,
            Search,
            Select,
            Payment,
            SearchTime,
            Receipt,
            PASSWORD,
            DeviceTest,
            Info
        }

        /// <summary>
        /// 출차 차량에 대한 출차정보전송 주석
        /// </summary>
        public enum EnumMagamType
        {
            /// <summary>
            /// 정산가능시간
            /// </summary>
            NormalTime,

            /// <summary>
            /// 정산중지시간
            /// </summary>
            MagamDelayTime
        }

        public enum BusyType
        {
            /// <summary>
            /// 평상시
            /// </summary>
            None,
            /// <summary>
            /// 관리자모드
            /// </summary>
            ManagerMode,
            /// <summary>
            /// 결제진행중
            /// </summary>
            Paying
        }


        #endregion

        #region Const Fields

        public const string g_AUTOBOOTH1920TYPE = "AB_1920";
        public const string g_AUTOBOOTH1024TYPE = "CAB_1024";
        public const string g_PREBOOTH1024TYPE = "CPB_1024";
        public const string g_PREBOOTH1080TYPE = "PB_1080";
        public const string g_PREBOOTH1920TYPE = "PB_1920";
        public const string g_PREBOOTH1920TYPENEW = "NOMIVIEWPB_1920";
        public const string g_PREBOOTH1080TYPENEW = "NOMIVIEWPB_1080";
        public const string gCardInStep = "카드결제중설명.wav";
        public const string gCardOutStep = "다시카드투입요청설명.wav";
        public const string gCardFullPayStep = "한도초과.wav";
        public const string gCardNotEject = "카드뽑지말아주세요.wav"; // 2016.10.27 KIS_DIP 추가
        public const string gCardEject = "카드를뽑아주세요.wav"; // 2016.10.27 KIS_DIP 추가
        public const string gCardReTry = "결제실패다시카드요청.wav"; // 2016.10.27 KIS_DIP 추가
        public const string gKSNetCardInStep = "끝까지카드넣어주세요.wav"; // KSNET 적용
        public const string SuccessResult = "_SuccessResult";

        private const string m_TIcketDeviceBetstMagnetic = "BESTMAGNETIC";
        private const string m_TIcketDeviceSamhwaBarcode = "SAMHWABARCODE";

        #endregion Const Fields

        #region Static Fields

        [DllImport("WinMM.dll")]
        public static extern long PlaySound(String lpszName, int hModule, int dwFlags);
        public static string Version = "RT 1.0.17";
        public static Dictionary<string, DeviceStatusManagement> gDic_DeviceStatusManageMent = new Dictionary<string, DeviceStatusManagement>(); // 나이스연동
        /// <summary>
        /// 현재 상황이 다른업무로 바쁜지 확인
        /// </summary>
        public static BusyType CurrentBusyType = BusyType.None;
        public static EnumMagamType gCurrentBoothTIme = EnumMagamType.NormalTime;
        /// <summary>
        /// 현재 돈의 정의
        /// </summary>
        public static Monetary gGurrentMoneTary = null;
        public static string gCreditPort1 = "";
        public static string gCreditBaudrate1 = "";
        public static string gCreditPort2 = "";
        public static string gCreditBaudrate2 = "";
        /// <summary>
        /// true일때만 프로그램 종료가능
        /// </summary>
        public static bool gIsApplicationExit = false;
        /// <summary>
        /// 무인정산기 사용여부 false이면 출차차량이 있을시도 요금처리 하지않음
        /// </summary>
        public static bool g_Autoboothenable = true;
        /// <summary>
        /// true면 출차무인으로 기기 사용 false면 사전무인으로 사용
        /// </summary>
        public static bool gIsAutoBooth = true;
        /// <summary>
        /// 요금결제등의 폼이 지금 작업중인지 확인
        /// </summary>
        public static bool gisBusyFormLauncher = false;
        public static bool g_isSetupMode = false;
        public static ConfigID.ErrorRecoveryType ControlBoardRecovery = ConfigID.ErrorRecoveryType.AUTO;
        /// <summary>
        /// 5만원권 사용여부
        /// </summary>
        public static bool SettingUse50000QtyBill = true;
        /// <summary>
        /// 무인정산기 사용여부
        /// </summary>
        public static bool g_GetAutobooth = true;
        public static bool g_UsePrintFullCuting = false;
        /// <summary>
        /// 영수증 공급가/부가세 출력 적용
        /// </summary>
        public static bool gUseReceiptSupplyPrint = true;
        public static int gCenterAliveTime = 240;
        public static bool UseVersionCheck = true;
        public static string gRestFulLocalPort = "0";
        public static string gRESTfulServerIp = "0";
        public static string gRESTfulServerPort = "0";
        public static string gRESTfulVersion = string.Empty;
        public static ConfigID.CarNumberType CurrentCarNumType = ConfigID.CarNumberType.Digit4SetAUTO;
        /// <summary>
        /// true면 분리해서 전광판 보여줌 false면 종전그대로 전체 표출
        /// </summary>
        public static bool useTicketCardSplit = false;
        public static string gVanTerminalId = string.Empty;
        public static string gVanIp = string.Empty;
        public static string gVanPort = string.Empty;
        public static string gVanSaup = string.Empty;
        public static string gCashVanIp = string.Empty;
        public static string gCashVanPort = string.Empty;
        public static string gCashTerminalId = string.Empty;
        public static string gCashSaupNo = string.Empty;
        public static string gVanSoftVersion = string.Empty;
        public static string gVanSerialVersion = string.Empty;
        /// <summary>
        /// TMoney Smartro Sam Slot 구분
        /// </summary>
        private static NPCommon.ConfigID.SAMSLOT gTmoneyTerminalSamSlot = ConfigID.SAMSLOT.NONE;
        public static string TmoneyDogleID = string.Empty;
        public static string TmoneyCatId = string.Empty;
        public static string TmoneySamId = string.Empty;
        public static string TmoneyVanIp = string.Empty;
        public static string TmoneyVanPort = string.Empty;
        /// <summary>
        /// 요금결제시 돈이나 할인권 투입시 무제한 대기상태
        /// </summary>
        public static bool PaymentInsertInfinite = false;
        /// <summary>
        /// 출차시 동영상 출력여부(정기권 회차)
        /// </summary>
        public static bool g_UseOutMovie = false;
        /// <summary>
        /// 증명서 이미지폴더 경로는 보통 \\192.168.1.100\IMAGE\ 등으로 되어있다
        /// </summary>
        public static string mJuminImageFolder = string.Empty;
        public static bool gUseVCatFailRestart = true;
        /// <summary>
        /// 스마트로 VCat신용단말기 음성사용일시 카드삽입 또는 재삽입필요시 음성멘트사용
        /// </summary>
        public static bool gUseVcatVoice = true;
        /// <summary>
        /// 자동마감을 사용유무
        /// </summary>
        public static bool gUseAutoMagam = false;
        /// <summary>
        /// 자동마감 딜레이시간 사용유무 true면 딜레이 사용
        /// </summary>
        public static bool gUseMagmDelay = false;
        public static string gMagamEndTIme = "0000";
        public static string gMagamStartTIme = "0000";
        public static string gMagmDelayEndTime = "0000";
        public static string gMagmDelayStartTime = "0000";

        /// <summary>
        /// 사전무인 할인이후 0원 출차처리여부
        /// </summary>
        public static bool gPayRuleUseDiscountFreeCarQuestionPay = false;

        /// <summary>
        /// 일반차량 사전결제시 결제여부 물어보기
        /// </summary>
        public static bool gPayRuleUsePrePayConfirm = false;

        /// <summary>
        /// 사전정산시 회차차량 요금결제 처리유무 true면 회차차량 요금결제 처리한다
        /// </summary>
        public static bool gPayRuleFreeCarQuestionPay = false;

        /// <summary>
        /// 사전정산시 재정산유무
        /// </summary>
        public static bool gPayRuleUseRePay = false;

        /// <summary>
        /// 사전정산 정기권연장결제 사용유무
        /// </summary>
        public static bool gPayRuleUsePreCarRegExtensionPay = false;

        public static ConfigID.LanguageType CurrentLanguageType = ConfigID.LanguageType.KOREAN;
        public static ConfigID.MoneyType CurrentMoneyType = ConfigID.MoneyType.WON;
        public static bool gUseMultiLanguage = false;

        /// <summary>
        /// 부스가 테스트 모드로 테스트진행중인지 실제 사용중인지 여부
        /// </summary>
        public static bool isBoothRealMode = true;

        /// <summary>
        /// 영수증 신호가 오는 자리수
        /// </summary>
        public static int gReceiptSignalNumber = 9;

        /// <summary>
        /// 장비를 메인에서 체크하겠는지 여부 false면 안함
        /// </summary>
        public static bool DeviceCheck = false;

        /// <summary>
        /// 장비를 메인에서 다시 체크하는 시간
        /// </summary>
        public static int DeviceCheckTime = 0;

        /// <summary>
        /// 장비이상시 정상복구 시도 회수
        /// </summary>
        public static int DeviceResetCount = 0;

        public static string CurrentDirctory = "";
        public static string m_ParkingDbServerIp = "";
        public static string m_ParkingDbServer_Port = "";
        public static string m_ParkingDbServer_Name = "";
        public static string m_ParkingDbServer_UserId = "";
        public static string m_ParkingDbServer_UserPwd = "";

        /// <summary>
        /// T-MAP 사용여부
        /// </summary>
        public static bool gUseTmap = true;

        public static bool gUseCardFailSend = false;
        private static HttpProcess mHttpProcess = new HttpProcess();
        private static FormType mFormType = FormType.Main;

        private static SerialPortCollection mSerialPorts = new SerialPortCollection();  // 시리얼 포트 정보 리스트
        private static DatabaseServerCollection mServers = new DatabaseServerCollection(); // 데이터베이스 서버 정보 리스트
        private static Dictionary<string, string> mPaymentResult = new Dictionary<string, string>(); // 결제 결과 리스트

        private static Image mBaseBackground = null; // 기본 배경화면
        private static ConfigID.BoothType mCurrentBoothType = ConfigID.BoothType.AB_1024;
        private static bool m_UseButtonSound = true;

        /// <summary>
        /// 현재 사용중인 현금영수증 신용서버
        /// </summary>
        private static ConfigID.CardReaderType CURRENTCASHVANTYPE = ConfigID.CardReaderType.VAN_FIRSTDATA;

        private static DiscountReadingFormat CURRENTDiscountReadingFormat = DiscountReadingFormat.TRACK2ISO_TRACK3210;
        private static bool m_UseDicountDisplay = false;
        private static NPCommon.ConfigID.MoneyOutputType mCurrentMoneyOutputType = ConfigID.MoneyOutputType.None;

        /// <summary>
        /// 개발 모드인지 아닌지 검사
        /// </summary>
        private static bool mIsDev = false;

        /// <summary>
        /// 활성화 여부
        /// </summary>
        private static bool mIsActived = false;

        /// <summary>
        /// 환경 설정 파일(sqlite)
        /// </summary>
        private static ConfigDB3I mConfig = null;

        /// <summary>
        /// 정보 로그
        /// </summary>
        private static SQLite mNPPaymentLog = null;

        private static int m_BoothPort = 8000;
        public static bool gUsePreFreeCarNoRecipt = false;

        /// <summary>
        /// 삼성페이결제 사용유무
        /// </summary>
        public static bool gUseSamSungPay = false;

        public static int gDoorSignalNumber = 1;

        #endregion Static Fields

        #region Properties

        /// <summary>
        /// 현재활성화된 폼명칭
        /// </summary>
        public static FormType CurrentFormType
        {
            set { mFormType = value; }
            get { return mFormType; }
        }

        public static ConfigID.BoothType CurrentBoothType
        {
            set { mCurrentBoothType = value; }
            get { return mCurrentBoothType; }
        }

        public static bool UseButtonSound
        {
            get { return m_UseButtonSound; }
            set { value = m_UseButtonSound; }
        }

        /// <summary>
        /// 현재사용중인 현금영수증 Van사 타입을 가져온다 퍼스트데이터,스마트로 등등
        /// </summary>
        public static ConfigID.CardReaderType GetCurrentCashVANType
        {
            get { return CURRENTCASHVANTYPE; }
        }

        /// 현재사용중인 DB값을 리턴한다 테트라텍,넥스파등
        /// </summary>
        public static DiscountReadingFormat GetCurrentDiscountReadingFormat
        {
            get { return CURRENTDiscountReadingFormat; }
        }

        /// <summary>
        /// 요금결제화면에서 할인권 넣은 수량을 보여줄지 여부 true면 보여줌(아주파킹 구자민 대리 요청 ->신한데뷰에서 한시간권으로 몇십시간을 할인받는 경우가 있는대
        /// 몇장 넣었는지를 고객이 모르기때문에 주지 시킬필요가 있다함...
        public static bool g_UseDicountDisplay
        {
            set { m_UseDicountDisplay = value; }
            get { return m_UseDicountDisplay; }
        }

        public static string PreMagamDate
        {
            set;
            get;
        }

        public static NPCommon.ConfigID.MoneyOutputType CurrentMoneyOutputType
        {
            set { mCurrentMoneyOutputType = value; }
            get { return mCurrentMoneyOutputType; }
        }

        public static bool IsDev
        {
            get
            {
                return mIsDev;
            }
            set
            {
                mIsDev = value;
            }
        }

        /// <summary>
        /// 임시파일 저장 경로
        /// </summary>
        public static string TempPath
        {
            get;
            set;
        }

        public static bool IsActived
        {
            get { return mIsActived; }
            set { mIsActived = value; }
        }

        /// <summary>
        /// 기본 배경 화면
        /// </summary>
        public static Image BaseBackground
        {
            get { return mBaseBackground; }
            set { mBaseBackground = value; }
        }

        public static SerialPortCollection SerialPorts
        {
            get { return mSerialPorts; }
        }

        public static DatabaseServerCollection Servers
        {
            get { return mServers; }
        }

        public static Dictionary<string, string> PaymentResult
        {
            get { return mPaymentResult; }
        }

        public static ConfigDB3I Config
        {
            get { return mConfig; }
            set { mConfig = value; }
        }

        public static SQLite NPPaymentLog
        {
            get { return mNPPaymentLog; }
            set { mNPPaymentLog = value; }
        }

        /// <summary>
        /// 환경설정 파일 위치
        /// </summary>
        public static string ConfigFilePath { get; set; }

        public static bool DeviceClosed
        {
            get;
            set;
        }

        public static int CashCreditCount
        {
            set;
            get;
        }

        public static int CashCreditMoney
        {
            set;
            get;
        }

        public static int CashTmoneyCount
        {
            set;
            get;
        }

        public static int CashTmoneyMoney
        {
            set;
            get;
        }

        public static string MainImageLocation
        {
            set;
            get;
        }

        public static string ReceiptPrintOption
        {
            set;
            get;
        }

        /// <summary>
        /// 메시지 표시 시간
        /// </summary>
        public static int SettingMessageTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 동영상 다시 재생시간
        /// </summary>
        public static int SettingMoviePlayTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 입력 제한 시간, 입력 제한 시간 초과 시 초기 화면으로 이동됨
        /// </summary>
        public static int SettingInputTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 차량찾기 입력제한시간
        /// </summary>
        public static int SettingCarSearchTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 차량선택 입력제한시간
        /// </summary>
        public static int SettingCarSelectTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 영수증화면 입력제한시간
        /// </summary>
        public static int SettingReceiptTimeValue
        {
            get;
            set;
        }

        /// <summary>
        /// 입력 제한 시간, 입력 제한 시간 초과 시 초기 화면으로 이동됨
        /// </summary>
        public static int SettingDoorTimeValue
        {
            get;
            set;
        }

        public static string ParkingDbServerIp
        {
            set { m_ParkingDbServerIp = value; }
            get { return m_ParkingDbServerIp; }
        }

        public static string ParkingDbServer_Port
        {
            set { m_ParkingDbServer_Port = value; }
            get { return m_ParkingDbServer_Port; }
        }

        public static string ParkingDbServer_Name
        {
            set { m_ParkingDbServer_Name = value; }
            get { return m_ParkingDbServer_Name; }
        }

        public static string ParkingDbServer_UserId
        {
            set { m_ParkingDbServer_UserId = value; }
            get { return m_ParkingDbServer_UserId; }
        }

        public static string ParkingDbServer_UserPwd
        {
            set { m_ParkingDbServer_UserPwd = value; }
            get { return m_ParkingDbServer_UserPwd; }
        }

        /// <summary>
        /// 주차장코드
        /// </summary>
        public static string ParkCode
        {
            get;
            set;
        }

        //private static string mLprChannelNumber = string.Empty;
        //public static string LprChannelNumber
        //{
        //    set {mLprChannelNumber=value;}
        //    get {return mLprChannelNumber;}
        //}

        /// <summary>
        /// 9:출구LPR, 11:출구전광판,12:사전무인, 13:출구무인
        /// </summary>
        public static int DeviceTypeInt
        {
            get;
            set;
        }

        /// <summary>
        /// 부스 Port
        /// </summary>
        public static int BoothPort
        {
            set { m_BoothPort = value; }
            get { return m_BoothPort; }
        }

        /// <summary>
        /// 부스 ID
        /// </summary>
        public static string BoothID
        {
            get;
            set;
        }

        /// <summary>
        /// 부스 요금체계
        /// </summary>
        public static string BoothFeeCode
        {
            get;
            set;
        }

        /// <summary>
        /// 부스 ID
        /// </summary>
        public static string BoothName
        {
            get;
            set;
        }

        /// <summary>
        /// 부스 ip
        /// </summary>
        public static string BoothIp
        {
            get;
            set;
        }

        /// <summary>
        /// 교통카드 아이디
        /// </summary>
        public static string TMoneyID
        {
            get;
            set;
        }

        //public static string g_OutLprUnitNo
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// 출차 LPR 공유폴더이름
        /// </summary>
        public static string g_OutLprImagePath
        {
            get;
            set;
        }
        public static ConfigID.SAMSLOT TmoneyTerminalSamSlot { get => gTmoneyTerminalSamSlot; set => gTmoneyTerminalSamSlot = value; }

        ///// <summary>
        ///// 출차 LPR 이미지서버 IP
        ///// </summary>
        //public static string g_OutLprImageIp
        //{
        //    get;
        //    set;
        //}

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// 현재 상황이 다른업무로 바쁜지 확인
        /// </summary>
        /// <returns></returns>
        public static BusyType GetCurrentBusy()
        {
            if (CurrentBusyType == BusyType.Paying)
            {
                return CurrentBusyType;
            }
            else if (NPSYS.CurrentFormType == NPSYS.FormType.DeviceTest || NPSYS.CurrentFormType == NPSYS.FormType.Login
                                                || NPSYS.CurrentFormType == NPSYS.FormType.Magam || NPSYS.CurrentFormType == NPSYS.FormType.MENU
                                                || NPSYS.CurrentFormType == NPSYS.FormType.PASSWORD)
            {
                CurrentBusyType = BusyType.ManagerMode;
            }
            else
            {
                CurrentBusyType = BusyType.None;
            }

            return CurrentBusyType;
        }

        /// <summary>
        /// 현재 무인정산기가 정산가능한시간인지 정산불가한시간인지 자동마감처리를 할지에대한 처리함수
        /// </summary>
        /// <param name="pIsJungsanIng">현재 무인정산기가 실제 요금결제중이거나 아니면</param>
        /// <returns></returns>
        public static EnumMagamType GetCurrentBoothTime(bool pIsJungsanIng, Action<string, string> action)
        {
            EnumMagamType currentBoothTime = EnumMagamType.NormalTime;
            if (!NPSYS.gUseAutoMagam)
            {
                gCurrentBoothTIme = currentBoothTime;
                return currentBoothTime;
            }
            if (NPSYS.gUseMagmDelay && NPSYS.IsRegularTime(NPSYS.gMagmDelayStartTime, NPSYS.gMagmDelayEndTime, DateTime.Now.ToString("HHmm"))) // 정산대기를 사용중이고 정산대기시간이면
            {
                // 자동마감사용하고 정산딜레이 사용이며 정산딜레이 사용하면
                currentBoothTime = EnumMagamType.MagamDelayTime;
            }
            if (NPSYS.IsRegularTime(NPSYS.gMagamStartTIme, NPSYS.gMagamEndTIme, DateTime.Now.ToString("HHmm"))) // 마감시간이라면
            {
                if (NPSYS.gUseMagmDelay && pIsJungsanIng) // 마감딜레이가 있다면 정산중이나 이럴때는 정산이 끝나고 마감한다.
                {
                    gCurrentBoothTIme = EnumMagamType.MagamDelayTime;
                    return EnumMagamType.MagamDelayTime;
                }
                else // 마감딜에이가 없다면 정산중이던 말던 걍 마감함...
                {
                    DateTime currentDateTime = DateTime.Now; //  현재시간
                    if (NPSYS.Config.GetValue(ConfigID.MagameEndDate).ToUpper().Trim() != "") //yyyy-MM-dd HH:mm:ss형식임 기존마감이있다면
                    {
                        DateTime preMagamDate = DateTime.ParseExact(NPSYS.Config.GetValue(ConfigID.MagameEndDate).ToUpper().Trim().Replace(" ", ""), "yyyy-MM-ddHH:mm:ss", System.Globalization.CultureInfo.CurrentCulture); // 기존마감데이터일자
                        if (Convert.ToInt32(currentDateTime.ToString("yyyyMMdd")) > Convert.ToInt32(preMagamDate.ToString("yyyyMMdd"))) // 년월일이 기존마감보다 크다면
                        {
                            Print.PrintMagam(true, false, action);
                        }
                    }
                    else //   기존마감이 없다면 바로 마감을해버린다
                    {
                        Print.PrintMagam(true, false, action);
                    }
                }
            }
            gCurrentBoothTIme = currentBoothTime;
            return currentBoothTime;
        }

        /// <summary>
        ///  현재방출가능수량
        /// </summary>
        public static void checkCurrentMoneyCount()
        {
            int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
            int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
            int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
            int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
            int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));
            int cash50MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MinQty));
            int cash100MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MinQty));
            int cash500MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MinQty));
            int cash1000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MinQty));
            int cash5000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MinQty));

            TextCore.INFO(TextCore.INFOS.CHARGE, "FormPaymentMenu | checkCurrentMoneyCount", "[현재 방출가능수량]  5000원수량:" + cash5000SettingQty.ToString() + "  1000원수량:" + cash1000SettingQty.ToString() + "  500원수량:" + cash500SettingQty.ToString() + "  100원수량:" + cash100SettingQty.ToString() + "  50원수량:" + cash50SettingQty.ToString());
        }

        public static bool IsRegularTime(string pRegularTimeStart, string pRegularTimeEnd, string pCurrentHHmm)
        {
            int result = 0;
            if (int.TryParse(pRegularTimeStart, out result) && (int.TryParse(pRegularTimeEnd, out result))) // 숫자값이면
            {
                int intFreeTimeStart = Convert.ToInt32(pRegularTimeStart);
                int intFreeTimeENd = Convert.ToInt32(pRegularTimeEnd);
                int outTime = Convert.ToInt32(pCurrentHHmm.Replace(":", "").SafeSubstring(0, 4)); // 시분
                if (intFreeTimeStart >= intFreeTimeENd)   // 시작시간이크면
                {
                    if (intFreeTimeStart <= outTime || outTime <= intFreeTimeENd) // 규칙에걸림
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // 규칙 11:00~19:00 일때같은경우
                    if (intFreeTimeStart <= outTime && outTime <= intFreeTimeENd) // 규칙에걸림
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 프로그램 버젼정보
        /// </summary>
        /// <returns></returns>
        public static string ProgramVersion()
        {
            string programVesion = "";
            programVesion = NPSYS.BoothName + " " + NPSYS.Version;
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS | ProgramVersion", "프로그램버젼:" + programVesion);
            return programVesion;
        }

        public static void GetCenterAliveTime()
        {
            gCenterAliveTime = NPSYS.Config.GetValue(ConfigID.FeatureSettingCenterAliveTime).Trim() == string.Empty ?
                240 : Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.FeatureSettingCenterAliveTime));
        }

        /// <summary>
        /// 요금결제화면에서 할인권 넣은 수량을 보여줄지 여부 true면 보여줌(아주파킹 구자민 대리 요청 ->신한데뷰에서 한시간권으로 몇십시간을 할인받는 경우가 있는대
        /// 몇장 넣었는지를 고객이 모르기때문에 주지 시킬필요가 있다함...
        /// </summary>
        public static void GetDicountDisplay()
        {
            m_UseDicountDisplay = NPSYS.Config.GetValue(ConfigID.FeatureSettingDiscountDIsplay).ToUpper().Trim() == "Y" ? true : false;
        }

        public static bool GetAutoboothEnable()
        {
            switch (NPSYS.Config.GetValue(ConfigID.FeatureSettingAutoBooth).ToUpper())
            {
                case "Y":
                    g_GetAutobooth = true;
                    NPSYS.g_Autoboothenable = g_GetAutobooth;
                    return true;

                case "N":
                    g_GetAutobooth = false;
                    NPSYS.g_Autoboothenable = g_GetAutobooth;
                    return false;

                default:
                    g_GetAutobooth = true;
                    NPSYS.g_Autoboothenable = g_GetAutobooth;
                    return true;
            }
        }

        /// <summary>
        /// 관리자가 설정한 모든값을 가져와서 다시 변수에 넣는다.
        /// </summary>
        public static void allGetFeatureSeeting()
        {
            string l_MovieStopTime = NPSYS.Config.GetValue(ConfigID.MovieStopTime);
            if (l_MovieStopTime == "")
                l_MovieStopTime = "10";

            NPSYS.SettingMoviePlayTimeValue = Convert.ToInt32(l_MovieStopTime) * 1000;

            // 입력 제한 시간 설정을 가지고 옴.
            string inputTime = NPSYS.Config.GetValue(ConfigID.InputTime);
            if (inputTime == "")
                inputTime = "300";

            NPSYS.SettingInputTimeValue = Convert.ToInt32(inputTime) * 1000;
            NPSYS.SettingCarSearchTimeValue = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.CarSearchTIme).Trim() == string.Empty ? "300" : NPSYS.Config.GetValue(ConfigID.CarSearchTIme).Trim()) * 1000;
            NPSYS.SettingCarSelectTimeValue = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.CarSelectTime).Trim() == string.Empty ? "300" : NPSYS.Config.GetValue(ConfigID.CarSelectTime).Trim()) * 1000;
            NPSYS.SettingReceiptTimeValue = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.RecipetTIme).Trim() == string.Empty ? "300" : NPSYS.Config.GetValue(ConfigID.RecipetTIme).Trim()) * 1000;

            string doortime = NPSYS.Config.GetValue(ConfigID.DoorTime);
            if (doortime == "")
                doortime = "30";

            NPSYS.SettingDoorTimeValue = Convert.ToInt32(doortime) * 1000;

            if (NPSYS.Config.GetValue(ConfigID.MagameEndDate).ToUpper().Trim() != "") // 마감날짜정보를 가져온다
            {
                NPSYS.PreMagamDate = NPSYS.Config.GetValue(ConfigID.MagameEndDate).ToUpper().Trim();
            }
            else
            {
                NPSYS.PreMagamDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            switch (NPSYS.Config.GetValue(ConfigID.FeatureSettingPaymentInsertMoneyTimeInfinite).ToUpper())
            {
                case "Y":
                    NPSYS.PaymentInsertInfinite = true;
                    break;

                case "N":
                    NPSYS.PaymentInsertInfinite = false;
                    break;

                default:
                    NPSYS.PaymentInsertInfinite = false;
                    break;
            }

            LanguageConvert.GetSettingLangaugeData();
            //카드실패전송
            UseCardFailSend();
            //카드실패전송 완료
            //TMAP연동
            GetUseTmap();
            //TMAP연동완료
            GetUseReceiptSupplyPrint();
            GetCenterAliveTime();
            GetRestFulInfo();
            GetCurrentCarNumType();

            TmoneyVanIp = NPSYS.Config.GetValue(ConfigID.TmoneyVanIp).ToUpper();
            TmoneyVanPort = NPSYS.Config.GetValue(ConfigID.TmoneyVanPort).ToUpper();
            TmoneyCatId = NPSYS.Config.GetValue(ConfigID.TmoneyDanID).ToUpper();
            GetAutoboothEnable();
            GetUseBill50000Qty();
            GetDicountDisplay();
            GetUsePrintFullCuting();
            GetUseButtonSound();
            GetuseTicketCardSplit();
            GetCASHVANTYPENAME();
            gVanTerminalId = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
            gVanIp = NPSYS.Config.GetValue(ConfigID.CardVanIp);
            gVanPort = NPSYS.Config.GetValue(ConfigID.CardVanPort);
            TmoneyCatId = NPSYS.Config.GetValue(ConfigID.TmoneyDanID);
            TmoneyDogleID = NPSYS.Config.GetValue(ConfigID.TmoneyGaID);
            TmoneySamId = NPSYS.Config.GetValue(ConfigID.TmoneySamId);
            gCashVanIp = NPSYS.Config.GetValue(ConfigID.CashVanIp);
            gCashVanPort = NPSYS.Config.GetValue(ConfigID.CashVanPort);
            gCashTerminalId = NPSYS.Config.GetValue(ConfigID.CashTerminalId);
            gCashSaupNo = NPSYS.Config.GetValue(ConfigID.CashSaupNo);
            gVanSaup = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardSaupNo).ToUpper().Trim();
            //KOCSE 카드리더기 추가
            gVanSoftVersion = NPSYS.Config.GetValue(ConfigID.FeatureSettingVanSoftWareVersion);
            gVanSerialVersion = NPSYS.Config.GetValue(ConfigID.FeatureSettingVanSerialVersion);
            //KOCSE 카드리더기 추가 주석

            GetDiscountReadingFormat();
            GetBoothRealMode();

            GetReceiptSignalNumber();
            ////Door신호관련
            GetDoorSignalNumber();
            ////Door신호관련 완료
            GetUseOutMovie();

            GetCurrentMoneyOutPutType();
            GetCurrentMoneyType();
            GetDeviceErrorRecoveryType();

            GetUseVCatFailRestart();
            GetVcatUseVoice();

            GetMagamStyle();
            GetUsePrePaySetting();
            SetLanguage();

            //사전정산시 요금없는차량 영수증발급안함 적용
            GetUsePreFreeCarNoRecipt();
            //사전정산시 요금없는차량 영수증발급안함 적용완료

            //삼성페이결제 적용
            GetUseSamSungPay();
            //삼성페이결제 적용완료

            //TMoney Smartro Sam Slot 구분 적용
            if (!Enum.TryParse<ConfigID.SAMSLOT>(NPSYS.Config.GetValue(ConfigID.TmoneyTerminalSamSlot), out gTmoneyTerminalSamSlot))
            {
                gTmoneyTerminalSamSlot = ConfigID.SAMSLOT.NONE;
            }
        }

        /// <summary>
        /// yyyymmddhhmmss 를 변환
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string datestringParser(string date)
        {
            return date.SafeSubstring(0, 4) + "-" + date.SafeSubstring(4, 2) + "-" + date.SafeSubstring(6, 2) + " " + date.SafeSubstring(8, 2) + ":" + date.SafeSubstring(10, 2);
        }

        public static DateTime LongTypeToDateTime(long pLongUnixTime)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds((double)pLongUnixTime);
            dtDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dtDateTime, "Korea Standard Time");
            return dtDateTime;
        }

        /// <summary>
        /// Convert a date time object to Unix time representation.
        /// </summary>
        /// <param name="datetime">The datetime object to convert to Unix time stamp.</param>
        /// <returns>Returns a numerical representation (Unix time) of the DateTime object.</returns>
        public static long DateTimeToLongType(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            sTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(sTime, "Korea Standard Time");
            return (long)(datetime - sTime).TotalSeconds;
        }

        public static void buttonSoundDingDong()
        {
            try
            {
                if (NPSYS.UseButtonSound)
                {
                    PlaySound(CurrentDirctory + @"\ding.wav", 0, 1);
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|SoundbuttonDing", "예외사항:" + ex.ToString());
            }
        }

        public static void PaymentSound()
        {
            PlaySound(CurrentDirctory + @"\sound\chimes.wav", 0, 1);
        }

        public static void LogAdminCash(string pLogDate, AdminCashLogType pLogType,
            int pInQty50, int pInQty100, int pInQty500, int pInQty1000, int pInQty5000,
            int pOutQty50, int pOutQty100, int pOutQty500, int pOutQty1000, int pOutQty5000,
            string pComment
            )
        {
            string sql = "";

            sql = "  INSERT INTO ADMIN_CASH_LOG ( "
                + "         LOG_DATE, LOG_TYPE, "
                + "  	   IN_QTY_50, IN_QTY_100, IN_QTY_500, IN_QTY_1000, IN_QTY_5000, "
                + "  	   OUT_QTY_50, OUT_QTY_100, OUT_QTY_500, OUT_QTY_1000, OUT_QTY_5000, "
                + "  	   COMMENT"
                + "  	   )"
                + "  VALUES ("
                + "         '" + pLogDate + "',"
                + "         '" + pLogType + "',"
                + "         '" + pInQty50 + "',"
                + "         '" + pInQty100 + "',"
                + "         '" + pInQty500 + "',"
                + "         '" + pInQty1000 + "',"
                + "         '" + pInQty5000 + "',"
                + "         '" + pOutQty50 + "',"
                + "         '" + pOutQty100 + "',"
                + "         '" + pOutQty500 + "',"
                + "         '" + pOutQty1000 + "',"
                + "         '" + pOutQty5000 + "',"
                + "         '" + pComment + "'"
                + "         )";

            NPPaymentLog.Execute(sql);
        }

        /// <summary>
        /// 영수증 출력
        /// </summary>
        /// <param name="pInfo"></param>
        public static void RecipePrint(NormalCarInfo pInfo, string pScreenName)
        {
            try
            {
                string dY_UNIT_COMMUTER_RECEIPT = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_COMMUTER_RECEIPT.ToString());
                string dY_UNIT_CARDCANCLE_RECEIPT = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CARDCANCLE_RECEIPT.ToString());
                string dY_UNIT_PARKING_FEE_RECEIPT = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PARKING_FEE_RECEIPT.ToString());
                string dY_UNIT_PARKNAME = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PARKNAME.ToString());
                string dY_UNIT_BUSINESSNUMBER = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_BUSINESSNUMBER.ToString());
                string dY_UNIT_ADDRESS = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_ADDRESS.ToString());
                string dY_UNIT_TELNO = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_TELNO.ToString());
                string dY_UNIT_UNITNAME = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_UNITNAME.ToString());
                string dY_UNIT_MACHINNAME1 = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_MACHINNAME1.ToString());
                string dY_UNIT_MACHINNAME2 = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_MACHINNAME2.ToString());
                string dY_UNIT_CARNO = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CARNO.ToString());
                string dY_CANCLE_DATE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CANCLE_DATE.ToString());
                string dY_PAYDATE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PAYDATE.ToString());
                string dY_TERM_DATE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_TERM_DATE.ToString());
                string dY_PARKINGTIME = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PARKINGTIME.ToString());
                string dY_UNIT_DAYS = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_DAYS.ToString());
                string dY_UNIT_HOURS = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_HOURS.ToString());
                string dY_UNIT_MINUTE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_MINUTE.ToString());
                string dY_UNIT_TERMFEE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_TERMFEE.ToString());
                string dY_UNIT_CANCLE_AMOUNT = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CANCLE_AMOUNT.ToString());
                string dY_UNIT_PARKINGFEE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PARKINGFEE.ToString());
                string dY_UNIT_DISCOUNTFEE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_DISCOUNTFEE.ToString());
                string dY_UNIT_PREPAYD = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PREPAYD.ToString());
                string dY_UNIT_CASH = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CASH.ToString());
                string dY_UNIT_CREDITCARD = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CREDITCARD.ToString());
                string dY_UNIT_TMONEY = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_TMONEY.ToString());
                string dY_UNIT_AMOUNTFEE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_AMOUNTFEE.ToString());
                string dY_UNIT_CASHNAME = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CASHNAME.ToString());
                string dY_UNIT_PAYTYPE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_PAYTYPE.ToString());
                string dY_UNIT_CHANGE = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_CHANGE.ToString());
                string dY_UNIT_COIN_PUT_IN = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_COIN_PUT_IN.ToString());
                string dY_UNIT_THANKYOU_PAY = GetDynamicLanguage(dynamictype.HEADER.DY_UNIT_THANKYOU_PAY.ToString());

                TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "#### 영수증 출력 시작 ####");

                if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK825)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "#### 프린터 타입 : ConfigID.PrinterType.HMK825 ####");
                    int valueSpaceHmk825 = 38;
                    NPSYS.Device.HMC60.FontSize(2, 2);

                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                        || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        //정기권연장 영수증
                        NPSYS.Device.HMC60.Print("   " + dY_UNIT_COMMUTER_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "    " + dY_UNIT_COMMUTER_RECEIPT + "");
                    }
                    else if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                            || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        //신용카드취소 영수증
                        NPSYS.Device.HMC60.Print("   " + dY_UNIT_CARDCANCLE_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "    " + dY_UNIT_CARDCANCLE_RECEIPT + "");
                    } // 정기권관련기능(만료요금부과/연장관련)주석종료
                    else
                    {
                        //주차요금 영수증
                        NPSYS.Device.HMC60.Print("     " + dY_UNIT_PARKING_FEE_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "     " + dY_UNIT_PARKING_FEE_RECEIPT + "");
                    }

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");

                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName));
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));
                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_TELNO + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + dY_UNIT_TELNO + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo));

                    NPSYS.Device.HMC60.Print("   " + dY_UNIT_UNITNAME + ":" + NPSYS.BoothName + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + dY_UNIT_UNITNAME + ":" + NPSYS.BoothName);

                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");

                    NPSYS.Device.HMC60.FontSize(2, 1);

                    string lBoothName = (NPSYS.gIsAutoBooth == true ? "   *" + dY_UNIT_MACHINNAME1 + "*" : "   *" + dY_UNIT_MACHINNAME2 + "*");

                    NPSYS.Device.HMC60.Print(lBoothName + "*\n\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", lBoothName);

                    NPSYS.Device.HMC60.Print(" " + dY_UNIT_CARNO + TextCore.ToRightAlignString(15, pInfo.OutCarNo1) + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " " + dY_UNIT_CARNO + TextCore.ToRightAlignString(15, pInfo.OutCarNo1));

                    NPSYS.Device.HMC60.FontSize(1, 1);

                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                      || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.HMC60.Print("   취소일시" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms).SafeSubstring(0, 5)) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   취소일시" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms).SafeSubstring(0, 5)));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print("   정산일시" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms).SafeSubstring(0, 5)) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   정산일시" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms).SafeSubstring(0, 5)));
                    }

                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        NPSYS.Device.HMC60.Print("   연장기간" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.NextStartYmd) + "~" + NPSYS.ConvetYears_Dash(pInfo.NextExpiredYmd)) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   연장기간" + TextCore.ToRightAlignString(valueSpaceHmk825, NPSYS.ConvetYears_Dash(pInfo.NextStartYmd) + "~" + NPSYS.ConvetYears_Dash(pInfo.NextExpiredYmd)));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print("   주차시간" + TextCore.ToRightAlignString(valueSpaceHmk825, string.Format("{0}일 {1}시간 {2}분", pInfo.ElapsedDay, pInfo.ElapsedHour, pInfo.ElapsedMinute)) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주차시간" + TextCore.ToRightAlignString(valueSpaceHmk825, string.Format("{0}일 {1}시간 {2}분", pInfo.ElapsedDay, pInfo.ElapsedHour, pInfo.ElapsedMinute)));
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료

                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        NPSYS.Device.HMC60.Print("   연장요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   연장요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원"));
                    }
                    else if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.HMC60.Print("   취소금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   취소금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원"));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(" 총주차요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 총주차요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotFee) + "원"));
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료
                    NPSYS.Device.HMC60.Print(" 총할인요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotDc) + "원") + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 총할인요금" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TotDc) + "원"));

                    NPSYS.Device.HMC60.Print("   사전정산" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.RecvAmt) + "원") + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사전정산" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.RecvAmt) + "원"));

                    NPSYS.Device.HMC60.Print("   -------------------------------------------------\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   -------------------------------------------------");

                    NPSYS.Device.HMC60.FontSize(2, 1);

                    string paymentMethod = "";
                    if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                    {
                        paymentMethod = "현금";
                        NPSYS.Device.HMC60.Print(" 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.InComeMoney - pInfo.OutComeMoney) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.InComeMoney - pInfo.OutComeMoney) + "원"));
                    }
                    else if (pInfo.VanAmt > 0)
                    {
                        paymentMethod = "신용카드";
                        NPSYS.Device.HMC60.Print(" 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.VanAmt) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.VanAmt) + "원"));
                    }
                    else if (pInfo.TMoneyPay > 0)
                    {
                        paymentMethod = "교통카드";
                        NPSYS.Device.HMC60.Print(" 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.TMoneyPay) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.TMoneyPay) + "원"));
                    }
                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    {
                        NPSYS.Device.HMC60.FontSize(1, 1);
                    }

                    //if (pInfo.DiscountMoney > 0)
                    //{
                    //    NPSYS.HMC60.Print("   할인명  " + TextCore.ToRightAlignString(valueSpace, pInfo.DiscountName) + "\n");
                    //    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   할인명  " + TextCore.ToRightAlignString(valueSpace, pInfo.DiscountName));
                    //}

                    if (pInfo.VanAmt > 0)
                    {
                        string l_CardNumber = pInfo.VanCardNumber;

                        NPSYS.Device.HMC60.Print("   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod));

                        NPSYS.Device.HMC60.Print("   승인번호" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanRegNo) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인번호" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanRegNo));
                        string lCardName = string.Empty;
                        lCardName = pInfo.VanIssueName;

                        NPSYS.Device.HMC60.Print("   카드명  " + TextCore.ToRightAlignString(valueSpaceHmk825, lCardName) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드명  " + TextCore.ToRightAlignString(valueSpaceHmk825, lCardName));

                        NPSYS.Device.HMC60.Print("   카드번호" + TextCore.ToRightAlignString(valueSpaceHmk825, l_CardNumber) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드번호" + TextCore.ToRightAlignString(valueSpaceHmk825, l_CardNumber));

                        if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                        || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            NPSYS.Device.HMC60.Print("   취소일자" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanCardApproveYmd) + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   취소일자" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanCardApproveYmd));
                        }
                        else
                        {
                            NPSYS.Device.HMC60.Print("   승인일자" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanCardApproveYmd) + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인일자" + TextCore.ToRightAlignString(valueSpaceHmk825, pInfo.VanCardApproveYmd));
                        }

                        //영수증 공급가/부가세 출력 적용
                        if (gUseReceiptSupplyPrint)
                        {
                            int vanTax = pInfo.VanTaxPay;
                            int supply = pInfo.VanAmt - vanTax;
                            NPSYS.Device.HMC60.Print("   공급가액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(supply) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   공급가액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(supply) + "원"));

                            NPSYS.Device.HMC60.Print("   부가세액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(vanTax) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   부가세액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(vanTax) + "원"));
                        }
                        //영수증 공급가/부가세 출력 적용완료

                        if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            NPSYS.Device.HMC60.Print("   취소액  " + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.VanAmt) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   취소액  " + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.VanAmt) + "원"));
                        }
                        else
                        {
                            NPSYS.Device.HMC60.Print("   청구액  " + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.VanAmt) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   청구액  " + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.VanAmt) + "원"));
                        }
                    }

                    if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                    {
                        NPSYS.Device.HMC60.Print("   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod));
                        if (NPSYS.Device.UsingUsingSettingCashReceipt && pInfo.CashReciptNo.Trim().Length > 0)
                        {
                            NPSYS.Device.HMC60.Print("   현금영수증 승인번호" + TextCore.ToRightAlignString(valueSpaceHmk825 - 11, pInfo.CashReciptNo) + "\n");
                            NPSYS.Device.HMC60.Print("   식별정보:[자진발급]" + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   현금영수증 승인번호" + TextCore.ToRightAlignString(valueSpaceHmk825 - 11, pInfo.CashReciptNo));
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   식별정보:[자진발급]");
                        }

                        NPSYS.Device.HMC60.Print("   투입금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.InComeMoney) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   투입금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.InComeMoney) + "원"));

                        NPSYS.Device.HMC60.Print("   거스름돈" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.OutComeMoney) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   거스름돈" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.OutComeMoney) + "원"));
                    }
                    if (pInfo.TMoneyPay > 0)
                    {
                        NPSYS.Device.HMC60.Print("   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk825, paymentMethod));

                        NPSYS.Device.HMC60.Print("   사용금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TMoneyPay) + "원") + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사용금액" + TextCore.ToRightAlignString(valueSpaceHmk825, TextCore.ToCommaString(pInfo.TMoneyPay) + "원"));
                    }

                    NPSYS.Device.HMC60.Print("   =================================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   =================================================");

                    NPSYS.Device.HMC60.Print("   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk825 - 2, Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기") + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk825 - 2, Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기"));

                    NPSYS.Device.HMC60.Print("   이용해 주셔서 감사합니다." + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   이용해 주셔서 감사합니다.");
                }
                else if (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.HMK054)
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "#### 프린터 타입 : ConfigID.PrinterType.HMK054 ####");
                    int valueSpaceHmk054 = 38 - 13;
                    int defaultLent = 8;

                    NPSYS.Device.HMC60.FontSize(2, 2);
                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_COMMUTER_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_COMMUTER_RECEIPT);
                    }
                    else if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                            || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_CARDCANCLE_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CARDCANCLE_RECEIPT);
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_PARKING_FEE_RECEIPT + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PARKING_FEE_RECEIPT);
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료
                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print("====================================\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    NPSYS.Device.HMC60.Print(dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PARKNAME + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotName));
                    NPSYS.Device.HMC60.Print(dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_BUSINESSNUMBER + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));
                    NPSYS.Device.HMC60.Print(dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_ADDRESS + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));
                    NPSYS.Device.HMC60.Print(dY_UNIT_TELNO + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_TELNO + ":" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo));

                    NPSYS.Device.HMC60.Print(dY_UNIT_UNITNAME + ":" + NPSYS.BoothName + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_UNITNAME + ":" + NPSYS.BoothName);

                    NPSYS.Device.HMC60.Print("====================================\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    NPSYS.Device.HMC60.FontSize(2, 1);

                    string lBoothName = (NPSYS.gIsAutoBooth == true ? "*" + dY_UNIT_MACHINNAME1 + "*" : "*" + dY_UNIT_MACHINNAME2 + "*");

                    NPSYS.Device.HMC60.Print(lBoothName + "*\n\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", lBoothName);

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    NPSYS.Device.HMC60.Print(dY_UNIT_CARNO + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.OutCarNo1) + "\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CARNO + TextCore.ToRightAlignString(15, pInfo.OutCarNo1));

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    // 정기권관련기능(만료요금부과/연장관련)

                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                        || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.HMC60.Print(dY_CANCLE_DATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(pInfo.InHms.Trim()).SafeSubstring(0, 5)) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_CANCLE_DATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(pInfo.InHms.Trim()).SafeSubstring(0, 5)));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(dY_PAYDATE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_PAYDATE, defaultLent), NPSYS.ConvetYears_Dash(pInfo.OutYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms.Trim()).SafeSubstring(0, 5)) + "\n");
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_PAYDATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.OutYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms.Trim()).SafeSubstring(0, 5)));
                    }

                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        NPSYS.Device.HMC60.Print(dY_TERM_DATE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_TERM_DATE, defaultLent), NPSYS.ConvetYears_Dash(pInfo.NextStartYmd) + "~" + NPSYS.ConvetYears_Dash(pInfo.NextExpiredYmd)) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_TERM_DATE + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.NextStartYmd) + "~" + NPSYS.ConvetYears_Dash(pInfo.NextExpiredYmd)));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(dY_PARKINGTIME + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_PARKINGTIME, defaultLent), string.Format("{0}" + dY_UNIT_DAYS + " {1}" + dY_UNIT_HOURS + " {2}" + dY_UNIT_MINUTE, pInfo.ElapsedDay, pInfo.ElapsedHour, pInfo.ElapsedMinute)) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_PARKINGTIME + TextCore.ToRightAlignString(valueSpaceHmk054, string.Format("{0}" + dY_UNIT_DAYS + " {1}" + dY_UNIT_HOURS + " {2}" + dY_UNIT_MINUTE, pInfo.ElapsedDay, pInfo.ElapsedHour, pInfo.ElapsedMinute)));
                    }

                    // 정기권관련기능(만료요금부과/연장관련)주석종료
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_TERMFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_TERMFEE, defaultLent), TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_TERMFEE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME));
                    }
                    else if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_CANCLE_AMOUNT + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_CANCLE_AMOUNT, defaultLent), TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CANCLE_AMOUNT + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME));
                    }
                    else
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_PARKINGFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PARKINGFEE, defaultLent), TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PARKINGFEE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotFee) + dY_UNIT_CASHNAME));
                    }

                    NPSYS.Device.HMC60.Print(dY_UNIT_DISCOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_DISCOUNTFEE, defaultLent), TextCore.ToCommaString(pInfo.TotDc) + dY_UNIT_CASHNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_DISCOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotDc) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print(dY_UNIT_PREPAYD + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PREPAYD, defaultLent), TextCore.ToCommaString(pInfo.RecvAmt) + dY_UNIT_CASHNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PREPAYD + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.RecvAmt) + dY_UNIT_CASHNAME));

                    NPSYS.Device.HMC60.Print("------------------------------------\n");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "------------------------------------");

                    NPSYS.Device.HMC60.FontSize(1, 1);
                    string paymentMethod = "";
                    if (pInfo.InComeMoney > 0)
                    {
                        paymentMethod = dY_UNIT_CASH;

                        NPSYS.Device.HMC60.Print(dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_AMOUNTFEE, defaultLent), TextCore.ToCommaString(pInfo.InComeMoney - pInfo.OutComeMoney) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.InComeMoney - pInfo.OutComeMoney) + dY_UNIT_CASHNAME));
                    }
                    else if (pInfo.VanAmt > 0)
                    {
                        paymentMethod = dY_UNIT_CREDITCARD;

                        NPSYS.Device.HMC60.Print(dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_AMOUNTFEE, defaultLent), TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME));
                    }
                    else if (pInfo.TMoneyPay > 0)
                    {
                        paymentMethod = dY_UNIT_TMONEY;
                        NPSYS.Device.HMC60.Print(dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_AMOUNTFEE, defaultLent), TextCore.ToCommaString(pInfo.TMoneyPay) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_AMOUNTFEE + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.TMoneyPay) + dY_UNIT_CASHNAME));
                    }
                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    {
                        NPSYS.Device.HMC60.FontSize(1, 1);
                    }

                    if (pInfo.VanAmt > 0)
                    {
                        string l_CardNumber = pInfo.VanCardNumber;

                        NPSYS.Device.HMC60.Print(dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PAYTYPE, defaultLent), paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));

                        NPSYS.Device.HMC60.Print("승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanRegNo) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanRegNo));
                        string lCardName = string.Empty;

                        lCardName = pInfo.VanIssueName;

                        NPSYS.Device.HMC60.Print("카드명  " + TextCore.ToRightAlignString(valueSpaceHmk054, lCardName) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드명  " + TextCore.ToRightAlignString(valueSpaceHmk054, lCardName));

                        NPSYS.Device.HMC60.Print("카드번호" + TextCore.ToRightAlignString(valueSpaceHmk054, l_CardNumber) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드번호" + TextCore.ToRightAlignString(valueSpaceHmk054, l_CardNumber));

                        if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                            || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            NPSYS.Device.HMC60.Print(dY_CANCLE_DATE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_CANCLE_DATE, defaultLent), pInfo.VanCardApproveYmd) + "\n");
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_CANCLE_DATE + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanCardApproveYmd));
                        }
                        else
                        {
                            NPSYS.Device.HMC60.Print("승인일자" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanCardApproveYmd) + "\n");
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인일자" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanCardApproveYmd));
                        }

                        //영수증 공급가/부가세 출력 적용
                        if (gUseReceiptSupplyPrint)
                        {
                            int vanTax = pInfo.VanTaxPay;
                            int supply = pInfo.VanAmt - vanTax;
                            NPSYS.Device.HMC60.Print("공급가액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(supply) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   공급가액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(supply) + "원"));

                            NPSYS.Device.HMC60.Print("부가세액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(vanTax) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   부가세액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(vanTax) + "원"));
                        }
                        //영수증 공급가/부가세 출력 적용완료

                        if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                            || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            NPSYS.Device.HMC60.Print("취소액  " + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME) + "\n");
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   취소액  " + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME));
                        }
                        else
                        {
                            NPSYS.Device.HMC60.Print("청구액  " + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME) + "\n");
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   청구액  " + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.VanAmt) + dY_UNIT_CASHNAME));
                        }
                    }

                    if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PAYTYPE, defaultLent), paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));
                        if (NPSYS.Device.UsingUsingSettingCashReceipt && pInfo.CashReciptNo.Trim().Length > 0)
                        {
                            NPSYS.Device.HMC60.Print("현금영수증 승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054 - 11, pInfo.CashReciptNo) + "\n");
                            NPSYS.Device.HMC60.Print("식별정보:[자진발급]" + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   현금영수증 승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054 - 11, pInfo.CashReciptNo));
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   식별정보:[자진발급]");
                        }

                        NPSYS.Device.HMC60.Print(dY_UNIT_COIN_PUT_IN + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_COIN_PUT_IN, defaultLent), TextCore.ToCommaString(pInfo.InComeMoney) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_COIN_PUT_IN + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.InComeMoney) + dY_UNIT_CASHNAME));

                        NPSYS.Device.HMC60.Print(dY_UNIT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_CHANGE, defaultLent), TextCore.ToCommaString(pInfo.OutComeMoney) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_CHANGE + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.OutComeMoney) + dY_UNIT_CASHNAME));
                    }
                    if (pInfo.TMoneyPay > 0)
                    {
                        NPSYS.Device.HMC60.Print(dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054 + printLenghtHmc054(dY_UNIT_PAYTYPE, defaultLent), paymentMethod) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_PAYTYPE + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));

                        NPSYS.Device.HMC60.Print("사용금액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TMoneyPay) + dY_UNIT_CASHNAME) + "\n");

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사용금액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TMoneyPay) + dY_UNIT_CASHNAME));
                    }

                    NPSYS.Device.HMC60.Print("====================================\n"); //36개
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    NPSYS.Device.HMC60.Print(DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk054 - 2, Config.GetValue(ConfigID.ParkingLotBoothID) + dY_UNIT_UNITNAME) + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk054 - 2, Config.GetValue(ConfigID.ParkingLotBoothID) + dY_UNIT_UNITNAME));

                    NPSYS.Device.HMC60.Print(dY_UNIT_THANKYOU_PAY + "\n");
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", dY_UNIT_THANKYOU_PAY);
                }
                else
                {
                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", $"#### 프린터 타입 : {NPSYS.Device.UsingSettingPrint.ToString()} ####");
                    int valueSpaceHmk054 = 38 - 13;
                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "정기연장 영수증");
                    }
                    else
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "주차요금 영수증");
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주차장:" + NPSYS.Config.GetValue(ConfigID.ParkingLotName));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사업자:" + NPSYS.Config.GetValue(ConfigID.ParkingLotBusinessNo));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주  소:" + NPSYS.Config.GetValue(ConfigID.ParkingLotAddress));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   전화번호:" + NPSYS.Config.GetValue(ConfigID.ParkingLotTelNo));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   정산기명:" + NPSYS.BoothName);

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    string lBoothName = (NPSYS.gIsAutoBooth == true ? "*출차일반(무인)*" : "*사전일반(무인)*");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", lBoothName);

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 차량번호" + TextCore.ToRightAlignString(15, pInfo.OutCarNo1));

                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   입차일시" + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.InYmd.Trim()) + " " + NPSYS.ConvetDay_Dash(pInfo.InHms.Trim()).SafeSubstring(0, 5)));
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   정산일시" + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.OutYmd) + " " + NPSYS.ConvetDay_Dash(pInfo.OutHms).SafeSubstring(0, 5)));

                    // 정기권관련기능(만료요금부과/연장관련)
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   연장기간" + TextCore.ToRightAlignString(valueSpaceHmk054, NPSYS.ConvetYears_Dash(pInfo.NextStartYmd) + "~" + NPSYS.ConvetYears_Dash(pInfo.NextExpiredYmd)));
                    }
                    else
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   주차시간" + TextCore.ToRightAlignString(valueSpaceHmk054, string.Format("{0}일 {1}시간 {2}분", pInfo.ElapsedDay, pInfo.ElapsedHour, pInfo.ElapsedMinute)));
                    }
                    // 정기권관련기능(만료요금부과/연장관련)주석종료
                    if (pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                    || pInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   연장요금" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotFee) + "원"));
                    }
                    else
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 총주차요금" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotFee) + "원"));
                    }

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "  총할인요금" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TotDc) + "원"));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "  사전정산" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.RecvAmt) + "원"));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "------------------------------------");

                    string paymentMethod = "";
                    if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                    {
                        paymentMethod = "현금";

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.InComeMoney - pInfo.OutComeMoney) + "원"));
                    }
                    else if (pInfo.VanAmt > 0)
                    {
                        paymentMethod = "신용카드";

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.VanAmt) + "원"));
                    }
                    else if (pInfo.TMoneyPay > 0)
                    {
                        paymentMethod = "교통카드";
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", " 정산요금" + TextCore.ToRightAlignString(15, TextCore.ToCommaString(pInfo.TMoneyPay) + "원"));
                    }

                    if (pInfo.VanAmt > 0)
                    {
                        string l_CardNumber = pInfo.VanCardNumber;

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanRegNo));
                        string lCardName = string.Empty;

                        lCardName = pInfo.VanIssueName;

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드명  " + TextCore.ToRightAlignString(valueSpaceHmk054, lCardName));

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   카드번호" + TextCore.ToRightAlignString(valueSpaceHmk054, l_CardNumber));

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   승인일자" + TextCore.ToRightAlignString(valueSpaceHmk054, pInfo.VanCardApproveYmd));

                        //영수증 공급가/부가세 출력 적용
                        if (gUseReceiptSupplyPrint)
                        {
                            int vanTax = pInfo.VanTaxPay;
                            int supply = pInfo.VanAmt - vanTax;
                            //    NPSYS.Device.HMC60.Print("공급가액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(supply) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   공급가액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(supply) + "원"));

                            //   NPSYS.Device.HMC60.Print("부가세액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(vanTax) + "원") + "\n");

                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   부가세액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(vanTax) + "원"));
                        }
                        //영수증 공급가/부가세 출력 적용완료

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   청구액  " + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.VanAmt) + "원"));
                    }
                    if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                    {
                        if (pInfo.InComeMoney - pInfo.OutComeMoney > 0)
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));
                            if (NPSYS.Device.UsingUsingSettingCashReceipt && pInfo.CashReciptNo.Trim().Length > 0)
                            {
                                TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   현금영수증 승인번호" + TextCore.ToRightAlignString(valueSpaceHmk054 - 11, pInfo.CashReciptNo));
                                TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   식별정보:[자진발급]");
                            }
                        }

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   투입금액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.InComeMoney) + "원"));

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   거스름돈" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.OutComeMoney) + "원"));
                    }
                    if (pInfo.TMoneyPay > 0)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   지불방식" + TextCore.ToRightAlignString(valueSpaceHmk054, paymentMethod));

                        TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   사용금액" + TextCore.ToRightAlignString(valueSpaceHmk054, TextCore.ToCommaString(pInfo.TMoneyPay) + "원"));
                    }

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "====================================");

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "   " + DateTime.Now.ToString("yyyy-MM-dd") + TextCore.ToRightAlignString(valueSpaceHmk054 - 2, Config.GetValue(ConfigID.ParkingLotBoothID) + " 정산기"));

                    TextCore.ACTION(TextCore.ACTIONS.USER, "NPSYS|RecipePrint", "이용해 주셔서 감사합니다.");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|RecipePrint", "영수증 출력에러:" + ex.ToString());
            }
            finally
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                {
                    NPSYS.Device.HMC60.Feeding(25);
                }
                System.Threading.Thread.Sleep(200);
                if (NPSYS.g_UsePrintFullCuting)
                {
                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    {
                        NPSYS.Device.HMC60.FullCutting();
                    }
                }
                else
                {
                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    {
                        NPSYS.Device.HMC60.ParticalCutting();
                    }
                }
                try
                {
                    try
                    {
                    }
                    catch (Exception ex)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|RecipePrint", "영수증 상태확인에러:" + ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|RecipePrint", "영수증 상태확인에러:" + ex.ToString());
                }
            }
        }

        public static int printLenghtHmc054(string pData, int pLen)
        {
            return pLen - Encoding.Default.GetByteCount(pData);
        }

        public static string ConvetYears_Dash(string p_Years)
        {
            string _n_years = p_Years?.Replace(" ", "").Trim();
            if (_n_years?.Length == 8)
            {
                return _n_years.SafeSubstring(0, 4) + "-" + _n_years.SafeSubstring(4, 2) + "-" + _n_years.SafeSubstring(6, 2);
            }
            return _n_years?? "";
        }

        public static string ConvetDay_Dash(string p_days)
        {
            string _p_day = p_days?.Replace(" ", "").Trim();
            if (_p_day?.Length == 6)
            {
                return _p_day.SafeSubstring(0, 2) + ":" + _p_day.SafeSubstring(2, 2) + ":" + _p_day.SafeSubstring(4, 2);
            }
            return _p_day?? "";
        }

        public static void ToggleControl(object pControl, bool pPressed)
        {
            Control ctrl = pControl as Control;

            if (ctrl == null)
                return;

            if (pPressed)
            {
                ctrl.BackColor = Color.Blue;
                ctrl.ForeColor = Color.White;
            }
            else
            {
                ctrl.BackColor = SystemColors.Control;
                ctrl.ForeColor = SystemColors.ControlText;
            }
        }

        public static void CloseDeviceAll()
        {
            if (IsDev) return;
            try
            {
                if (!DeviceClosed)
                {
                    NPSYS.Config.Close();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Config.Close");
                    NPSYS.NPPaymentLog.Disconnect();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.NPPaymentLog.Disconnect");
                    bool isConnect = false;

                    // 신분증인식기 적용
                    if (NPSYS.Device.UsingSettingSinbunReader)
                    {
                        NPSYS.Device.SinbunReader.ThreadEnd();
                        if (NPSYS.Device.SinbunReader.IsConnect)
                        {
                            NPSYS.Device.SinbunReader.DisConnect();
                        }
                    }
                    // 신분증인식기 적용완료

                    if ((NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.TItMagnetincDiscount)
                        && NPSYS.Device.gIsUseMagneticReaderDevice) // 2016-01-19 스마트로 추가
                    {
                        try
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "[카드리더기 오른쪽 종료시도]");
                            NPSYS.Device.CardDevice2.TIcektCreditCardCloseDevice();
                        }
                        catch (Exception exs)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CloseDeviceAll", "NPSYS.Device.CardDevice2.TIcektCreditCardCloseDevice" + exs.ToString());
                        }
                    }
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
                    {
                        try
                        {
                            KiccTs141.Disconnect();
                        }
                        catch (Exception exx)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CloseDeviceAll", KiccTs141.Disconnect() + exx.ToString());
                        }
                    }
                    // FIRSTDATA처리
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.FIRSTDATA_DIP)
                    {
                        try
                        {
                            bool resultFirstDataDestrosy = FirstDataDip.Close();
                        }
                        catch (Exception ex)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CloseDeviceAll", ConfigID.CardReaderType.FIRSTDATA_DIP.ToString() + " 종료실패 원인:" + ex.ToString());
                        }
                    }
                    // FIRSTDATA처리 주석처리
                    if (NPSYS.Device.gIsUseDeviceBillDischargeDevice)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "MoneyBillOutDeviice.End 종료시도");
                        try
                        {
                            MoneyBillOutDeviice.End();
                        }
                        catch (Exception sss)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CloseDeviceAll", "[지폐방출기종료]" + sss.ToString());
                        }
                    }
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "5");
                    if (NPSYS.Device.UsingSettingBillReader)
                    {
                        isConnect = NPSYS.Device.BillReader.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "지폐리더기연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.BillReader.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.BillReaderDisconnect");
                        }
                    }
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "6");
                    if (NPSYS.Device.UsingSettingCoinReader)
                    {
                        isConnect = NPSYS.Device.CoinReader.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "동전리더기연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.CoinReader.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "CoinReader.Disconnect");
                        }
                    }
                    if (NPSYS.Device.UsingSettingCoinCharger50)
                    {
                        isConnect = NPSYS.Device.CoinDispensor50.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "동전방출기50연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.CoinDispensor50.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "CoinDispensor50.Disconnect");
                        }
                    }
                    if (NPSYS.Device.UsingSettingCoinCharger100)
                    {
                        isConnect = NPSYS.Device.CoinDispensor100.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "동전방출기100연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.CoinDispensor100.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "CoinDispensor100.Disconnect");
                        }
                    }
                    if (NPSYS.Device.UsingSettingCoinCharger500)
                    {
                        isConnect = NPSYS.Device.CoinDispensor500.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "동전방출기500연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.CoinDispensor500.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "CoinDispensor500.Disconnect");
                        }
                    }

                    if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE)
                    {
                        isConnect = NPSYS.Device.HMC60.IsConnect;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "영수증프린터연결상태:" + isConnect.ToString());
                        if (isConnect)
                        {
                            NPSYS.Device.HMC60.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.HMC60.Disconnect");
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.HMC60.Disconnect:" + "접속상태가 아니라서 종료안함");
                        }
                    }

                    if (NPSYS.Device.UsingSettingControlBoard == ConfigID.ControlBoardType.GOODTECH)
                    {
                        if (NPSYS.Device.DoSensors.IsConnect)
                        {
                            NPSYS.Light_Carddevice_Off();
                            applicationDoevent(6);
                            System.Threading.Thread.Sleep(1000);
                            NPSYS.Device.DoSensors.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.DoSensors.Disconnect");
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.DoSensors.Disconnect:" + "접속상태가 아니라서 종료안함");
                        }
                    }
                    ////바코드모터드리블 사용
                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode)
                    {
                        if (NPSYS.Device.BarcodeSerials.IsConnect)
                        {
                            NPSYS.Device.BarcodeSerials.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.BarcodeSerials.Disconnect");
                        }
                    }
                    else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000)
                    {
                        if (NPSYS.Device.BarcodeMoter.IsConnect)
                        {
                            NPSYS.Device.BarcodeMoter.Disconnect();
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.BarcodeMoter.Disconnect");
                        }
                    }

                    //바코드모터드리블 사용완료

                    if (NPSYS.Device.UsingSettingTmoney && NPSYS.Device.isUseDeviceTMoneyReaderDevice)
                    {
                        NPSYS.Device.T_MoneySmartro.TmoneyClose();
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "NPSYS.Device.T_MoneySmartro.TmoneyClose");
                    }

                    DeviceClosed = true;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|CloseDeviceAll", "모든 장비종료시킴");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|CloseDeviceAll", ex.ToString());
            }
        }

        /// <summary>
        /// 위치좌표를 부모안에서의 위치좌표로 변경시킴
        /// </summary>
        /// <param name="pThis"></param>
        /// <param name="pParent"></param>
        /// <returns></returns>
        public static Point ConvertToParentLocation(Control pThis, Control pParent)
        {
            return new Point(pThis.Location.X - pParent.Location.X, pThis.Location.Y - pParent.Location.Y);
        }

        public static DeviceTypeName GetDeviceTypeInt()
        {
            switch (DeviceTypeInt)
            {
                case (int)DeviceTypeName.OutLpr:
                    return DeviceTypeName.OutLpr;

                case (int)DeviceTypeName.OutDisplay:
                    return DeviceTypeName.OutDisplay;

                case (int)DeviceTypeName.PreBooth:
                    return DeviceTypeName.PreBooth;

                case (int)DeviceTypeName.AutoBooth:
                    return DeviceTypeName.AutoBooth;
            }
            return DeviceTypeName.AutoBooth;
        }

        /// <summary>
        /// 웹 이미지 가져오기
        /// </summary>
        /// <param name="URL"></param>
        public static Bitmap WebImageView(string URL)
        {
            try
            {
                System.Net.WebClient Downloader = new System.Net.WebClient();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS | WebImageView", "[이미지경로불러오기]" + @"http://" + NPSYS.gRESTfulServerIp + ":" + NPSYS.gRESTfulServerPort + URL);

                System.IO.Stream ImageStream = Downloader.OpenRead(@"http://" + NPSYS.gRESTfulServerIp + ":" + NPSYS.gRESTfulServerPort + URL);
                Bitmap DownloadImage = Bitmap.FromStream(ImageStream) as Bitmap;
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS | WebImageView", "[이미지경로불러오기 종료]" + @"http://" + NPSYS.gRESTfulServerIp + ":" + NPSYS.gRESTfulServerPort + URL);
                return DownloadImage;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS | WebImageView", ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 동전리더기 , 지폐리더기, 지폐방출기, 동전방출기 에 장비 정상동작 상태에 따른 현금 결제가 가능할지 판단
        /// </summary>
        /// <returns></returns>
        public static bool IsUsedCashPay()
        {
            return (NPSYS.Device.isUseDeviceBillReaderDevice || NPSYS.Device.isUseDeviceCoinReaderDevice);
        }

        public static void LedLight()
        {
            if (NPSYS.Device.UsingSettingControlBoard != ConfigID.ControlBoardType.GOODTECH)
            {
                return;
            }
            try
            {
                bool F1 = false;
                bool F2 = false;
                bool F3 = false;
                bool F4 = false;
                bool F5 = false;
                bool F6 = false;
                if (NPSYS.Device.gIsUseCreditCardDevice)
                {
                    F1 = NPSYS.Device.UsingSettingCreditCard; // 신용카드 사용시
                    if (NPSYS.Device.UsingSettingCreditCard == true ||  // 신용카드 사용시
                        (NPSYS.Device.UsingSettingDiscountCard == true && NPSYS.useTicketCardSplit == false) || //할인권 사용시 전체전광판 표출시
                        NPSYS.Device.gIsUseMagneticReaderDevice == false && NPSYS.Device.UsingSettingDiscountCard) // 카드리더기 2가 고장이면서 할인권사용시
                    {
                        F2 = true;
                    }
                    if ((NPSYS.Device.UsingSettingDiscountCard == true && NPSYS.useTicketCardSplit == false) || //할인권 사용시 전체전광판 표출시
                        NPSYS.Device.gIsUseMagneticReaderDevice == false && NPSYS.Device.UsingSettingDiscountCard) // 카드리더기 2가 고장이면서 할인권사용시
                    {
                        F3 = true;
                    }
                }

                if (NPSYS.Device.gIsUseMagneticReaderDevice)
                {
                    F4 = NPSYS.Device.UsingSettingDiscountCard; // 할인권 사용시
                    if (NPSYS.Device.UsingSettingDiscountCard == true ||  // 할인권 사용시
                        (NPSYS.Device.UsingSettingCreditCard == true && NPSYS.useTicketCardSplit == false) || //신용카드 사용시 전체전광판 표출시
                        NPSYS.Device.gIsUseCreditCardDevice == false && NPSYS.Device.UsingSettingCreditCard) // 카드리더기 1가 고장이면서 신용카드사용시
                    {
                        F5 = true;
                    }
                    if ((NPSYS.Device.UsingSettingCreditCard == true && NPSYS.useTicketCardSplit == false) || //신용카드 사용시 전체전광판 표출시
                        NPSYS.Device.gIsUseCreditCardDevice == false && NPSYS.Device.UsingSettingCreditCard) // 카드리더기 1가 고장이면서 신용카드사용시
                    {
                        F6 = true;
                    }
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS|LedLight", "LED켤때 상태값:" + "[신용카드상단]" + F1.ToString()
                                                                                       + "[신용카드중단]" + F2.ToString()
                                                                                       + "[신용카드하단]" + F3.ToString()
                                                                                       + "[할인권상단]" + F4.ToString()
                                                                                       + "[할인권중단]" + F5.ToString()
                                                                                       + "[할인권하단]" + F6.ToString());

                NPSYS.Device.DoSensors.DoCoinstrainNewOn(true,
                                                  true,
                                                  true,
                                                  true,
                                                  true,
                                                  NPSYS.Device.isUseDeviceBillReaderDevice,
                                                  NPSYS.Device.isUseDeviceBillReaderDevice,
                                                    //NPSYS.Device.isUseCreditCardDevice1 && NPSYS.UsingPaymentCreditCard  ,
                                                    //(NPSYS.Device.isUseCreditCardDevice1 && NPSYS.Device.UsingSettingDiscountCard) || (NPSYS.Device.isUseCreditCardDevice1 && NPSYS.UsingPaymentCreditCard),
                                                    //NPSYS.Device.isUseCreditCardDevice1 && NPSYS.Device.UsingSettingDiscountCard && (NPSYS.useTicketCardSplit == false),
                                                    //NPSYS.Device.isUseCreditCardDevice2 && NPSYS.Device.UsingSettingDiscountCard,
                                                    //(NPSYS.Device.isUseCreditCardDevice1 && NPSYS.Device.UsingSettingDiscountCard) || (NPSYS.Device.isUseCreditCardDevice1 && NPSYS.UsingPaymentCreditCard),
                                                    //NPSYS.Device.isUseCreditCardDevice2 && NPSYS.UsingPaymentCreditCard && (NPSYS.useTicketCardSplit == false),
                                                    F1,
                                                    F2,
                                                    F3,
                                                    F4,
                                                    F5,
                                                    F6,
                                                  true);
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "NPSYS|LedLight", "LED켤때 에러:" + ex.ToString());
            }
        }

        public static void Light_Carddevice_Off()
        {
            if (NPSYS.Device.UsingSettingControlBoard != ConfigID.ControlBoardType.GOODTECH)
            {
                return;
            }

            try
            {
                NPSYS.Device.DoSensors.DoCoinstrainNewOn(false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            }
            catch (Exception ex)
            {
                TextCore.DeviceError(TextCore.DEVICE.DIDO, "NPSYS|Light_Carddevice_Off", "LED 끄는중 예외사항:" + ex.ToString());
            }
        }

        public static CoinDispensor.CoinDispensorStatusType OutChargeCoin(CoinDispensor pCoinDispenser, int p_OutChargeValue, ref int outcharge)
        {
            outcharge = 0;
            CoinDispensor.CoinDispensorStatusType currentCoinStatusType = CoinDispensor.CoinDispensorStatusType.NONE;
            int l_tenUnitOutcharge = p_OutChargeValue / 10;
            int l_oneUnitOutcharge = p_OutChargeValue % 10;
            if (l_tenUnitOutcharge > 0)
            {
                for (int i = 1; i <= l_tenUnitOutcharge; i++)
                {
                    currentCoinStatusType = (pCoinDispenser.OutCharge(10));
                    applicationDoevent(20);
                    if (currentCoinStatusType != CoinDispensor.CoinDispensorStatusType.OK)
                    {
                        int notOutcoin = 10; // 미방출개수
                        if (currentCoinStatusType == CoinDispensor.CoinDispensorStatusType.RejectCoinStatus)
                        {
                            notOutcoin = pCoinDispenser.RemainCoin();
                            outcharge += 10 - notOutcoin;
                        }
                        switch (pCoinDispenser.CurrentCoinType)
                        {
                            case CoinDispensor.CoinType.Money500Type:
                                TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:10" + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                                break;

                            case CoinDispensor.CoinType.Money100Type:
                                TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:10" + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());

                                break;

                            case CoinDispensor.CoinType.Money50Type:
                                TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:10" + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                                break;
                        }
                        return currentCoinStatusType;
                    }
                    outcharge += 10;
                    switch (pCoinDispenser.CurrentCoinType)
                    {
                        case CoinDispensor.CoinType.Money500Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                            break;

                        case CoinDispensor.CoinType.Money100Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());

                            break;

                        case CoinDispensor.CoinType.Money50Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                            break;
                    }
                }
            }
            if (l_oneUnitOutcharge > 0)
            {
                currentCoinStatusType = pCoinDispenser.OutCharge(l_oneUnitOutcharge);
                applicationDoevent(20);
                if (currentCoinStatusType != CoinDispensor.CoinDispensorStatusType.OK)
                {
                    int notOutcoin = l_oneUnitOutcharge; // 미방출개수
                    if (currentCoinStatusType == CoinDispensor.CoinDispensorStatusType.RejectCoinStatus)
                    {
                        notOutcoin = pCoinDispenser.RemainCoin();
                    }
                    //방출기장애로 잘못된 응답값 받았을경우 방출개수 수정 적용
                    if (l_oneUnitOutcharge < notOutcoin)
                    {
                        notOutcoin = l_oneUnitOutcharge;
                    }
                    //방출기장애로 잘못된 응답값 받았을경우 방출개수 수정 적용완료
                    outcharge += l_oneUnitOutcharge - notOutcoin;
                    switch (pCoinDispenser.CurrentCoinType)
                    {
                        case CoinDispensor.CoinType.Money500Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:" + l_oneUnitOutcharge.ToString() + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                            break;

                        case CoinDispensor.CoinType.Money100Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:" + l_oneUnitOutcharge.ToString() + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());

                            break;

                        case CoinDispensor.CoinType.Money50Type:
                            TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 현재요청개수:" + l_oneUnitOutcharge.ToString() + " 현재 미방출개수" + notOutcoin.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                            break;
                    }
                    return currentCoinStatusType;
                }
                outcharge += l_oneUnitOutcharge;
                switch (pCoinDispenser.CurrentCoinType)
                {
                    case CoinDispensor.CoinType.Money500Type:
                        TextCore.ACTION(TextCore.ACTIONS.COIN500CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                        break;

                    case CoinDispensor.CoinType.Money100Type:
                        TextCore.ACTION(TextCore.ACTIONS.COIN100CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());

                        break;

                    case CoinDispensor.CoinType.Money50Type:
                        TextCore.ACTION(TextCore.ACTIONS.COIN50CHARGER, "NPSYS | OutChargeCoin", "동전방출중 상태:" + currentCoinStatusType.ToString() + " 총 출력동전개수:" + outcharge.ToString());
                        break;
                }
            }
            return currentCoinStatusType;
        }

        public static string SetRightAlign(string sMsg, int nSize)
        {
            string sResult, t = "";

            int nLen = Encoding.Default.GetByteCount(sMsg);
            if (nLen < nSize)
            {
                for (int i = 0; i < nSize - nLen; i++)
                    t += " ";
                sResult = t + sMsg;
            }
            else
            {
                int nPos = nSize - 1;
                sResult = sMsg;
            }

            return sResult;
        }

        /// <summary>
        /// 두개의  시간 사이 간격 계산 분리턴
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="YYMMDD"></param>
        /// <param name="HHMMSS"></param>
        /// <returns></returns>
        public static int ElpaseMinuteReturnInt(string startDate, string YYMMDD, string HHMMSS)
        {
            string _startDate = startDate.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
            int Years = Convert.ToInt32(_startDate.SafeSubstring(0, 4));
            int month = Convert.ToInt32(_startDate.SafeSubstring(4, 2));
            int day = Convert.ToInt32(_startDate.SafeSubstring(6, 2));
            int hour = Convert.ToInt32(_startDate.SafeSubstring(8, 2));
            int minute = Convert.ToInt32(_startDate.SafeSubstring(10, 2));
            int second = Convert.ToInt32(_startDate.SafeSubstring(12, 2));
            string _enddata = (YYMMDD + HHMMSS).Replace(":", "").Replace("-", "").Replace(" ", "").Trim();

            int EndYears = Convert.ToInt32(_enddata.SafeSubstring(0, 4));
            int Endmonth = Convert.ToInt32(_enddata.SafeSubstring(4, 2));
            int Endday = Convert.ToInt32(_enddata.SafeSubstring(6, 2));
            int Endhour = Convert.ToInt32(_enddata.SafeSubstring(8, 2));
            int Endminute = Convert.ToInt32(_enddata.SafeSubstring(10, 2));
            int Endsecond = Convert.ToInt32(_enddata.SafeSubstring(12, 2));

            DateTime oldDate = DateTime.ParseExact(_startDate.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            DateTime newDate = DateTime.ParseExact(_enddata.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            TimeSpan ts = newDate - oldDate;
            int dirrednceInMinute = Convert.ToInt32(ts.TotalMinutes);
            if (dirrednceInMinute <= 0)
            {
                dirrednceInMinute = 1;
            }
            return Convert.ToInt32(dirrednceInMinute);
        }

        /// <summary>
        /// 두개의  시간 사이 간격 계산 분리턴
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="YYMMDD"></param>
        /// <param name="HHMMSS"></param>
        /// <returns></returns>
        public static long ElpaseMinuteReturnLong(string startDate, string YYMMDD, string HHMMSS)
        {
            string _startDate = startDate.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
            int Years = Convert.ToInt32(_startDate.SafeSubstring(0, 4));
            int month = Convert.ToInt32(_startDate.SafeSubstring(4, 2));
            int day = Convert.ToInt32(_startDate.SafeSubstring(6, 2));
            int hour = Convert.ToInt32(_startDate.SafeSubstring(8, 2));
            int minute = Convert.ToInt32(_startDate.SafeSubstring(10, 2));
            int second = Convert.ToInt32(_startDate.SafeSubstring(12, 2));
            string _enddata = (YYMMDD + HHMMSS).Replace(":", "").Replace("-", "").Replace(" ", "").Trim();

            int EndYears = Convert.ToInt32(_enddata.SafeSubstring(0, 4));
            int Endmonth = Convert.ToInt32(_enddata.SafeSubstring(4, 2));
            int Endday = Convert.ToInt32(_enddata.SafeSubstring(6, 2));
            int Endhour = Convert.ToInt32(_enddata.SafeSubstring(8, 2));
            int Endminute = Convert.ToInt32(_enddata.SafeSubstring(10, 2));
            int Endsecond = Convert.ToInt32(_enddata.SafeSubstring(12, 2));

            DateTime oldDate = DateTime.ParseExact(_startDate.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            DateTime newDate = DateTime.ParseExact(_enddata.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
            TimeSpan ts = newDate - oldDate;
            int dirrednceInMinute = Convert.ToInt32(ts.TotalMinutes);
            if (dirrednceInMinute <= 0)
            {
                dirrednceInMinute = 1;
            }
            long _ParkTime = Convert.ToInt64(dirrednceInMinute);

            return _ParkTime;
        }

        /// <summary>
        /// 시간을 비교해서 일을 리턴한다.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="YYMMDD"></param>
        /// <param name="HHMMSS"></param>
        /// <returns></returns>
        public static long ElpaseDay(string startDate, string YYMMDD, string HHMMSS)
        {
            try
            {
                string _startDate = startDate.Replace("-", "").Replace(":", "").Replace(" ", "").Trim();
                int Years = Convert.ToInt32(_startDate.SafeSubstring(0, 4));
                int month = Convert.ToInt32(_startDate.SafeSubstring(4, 2));
                int day = Convert.ToInt32(_startDate.SafeSubstring(6, 2));
                int hour = Convert.ToInt32(_startDate.SafeSubstring(8, 2));
                int minute = Convert.ToInt32(_startDate.SafeSubstring(10, 2));
                int second = Convert.ToInt32(_startDate.SafeSubstring(12, 2));
                string _enddata = (YYMMDD + HHMMSS).Replace(":", "").Replace("-", "").Replace(" ", "").Trim();

                int EndYears = Convert.ToInt32(_enddata.SafeSubstring(0, 4));
                int Endmonth = Convert.ToInt32(_enddata.SafeSubstring(4, 2));
                int Endday = Convert.ToInt32(_enddata.SafeSubstring(6, 2));
                int Endhour = Convert.ToInt32(_enddata.SafeSubstring(8, 2));
                int Endminute = Convert.ToInt32(_enddata.SafeSubstring(10, 2));
                int Endsecond = Convert.ToInt32(_enddata.SafeSubstring(12, 2));

                DateTime oldDate = DateTime.ParseExact(_startDate.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
                DateTime newDate = DateTime.ParseExact(_enddata.SafeSubstring(0, 12), "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);
                TimeSpan ts = newDate - oldDate;
                int dirrednceInMinute = Convert.ToInt32(ts.TotalMinutes);
                if (dirrednceInMinute <= 0)
                {
                    dirrednceInMinute = 1;
                }
                long _ParkTime = Convert.ToInt64(dirrednceInMinute);

                return CalculateElapsedDay(Convert.ToInt64(_ParkTime.ToString("######")));
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|ElpaseDay", ex.ToString());
                return 0;
            }
        }

        public static string MakeStringDate(DateTime pDatetime)
        {
            return pDatetime.ToString("yyyyMMddHHmm") + "00";
        }

        /// <summary>
        /// yyyymmddhhmmss의 문자열을 Datetime으로 변환
        /// </summary>
        /// <param name="pYmdHms"></param>
        /// <returns></returns>
        public static DateTime MakeDateAsStringYmd(string pYmdHms)
        {
            string ymdhms = pYmdHms.Replace("-", "").Replace(":", "").Replace(" ", "");
            DateTime getLastUsedDate = new DateTime(Convert.ToInt32(ymdhms.SafeSubstring(0, 4)), Convert.ToInt32(ymdhms.SafeSubstring(4, 2)), Convert.ToInt32(ymdhms.SafeSubstring(6, 2)),
                                              Convert.ToInt32(ymdhms.SafeSubstring(8, 2)), Convert.ToInt32(ymdhms.SafeSubstring(10, 2)), 00);
            return getLastUsedDate;
        }

        /// <summary>
        /// 분 총 경과시간을 일 시간 분으로 분리 시킴
        /// </summary>
        public static long CalculateElapsedDay(long TotalMinute)
        {
            long mElapsedMinute = 0;
            long mElapsedHour = 0;
            long mElapsedDay = 0;
            if (TotalMinute <= 1)
            {
                TotalMinute = 1;
                mElapsedMinute = 1;
                mElapsedHour = 0;
                mElapsedDay = 0;
                return mElapsedDay;
            }
            try
            {
                long temp = TotalMinute;

                mElapsedMinute = temp % 60;

                temp -= mElapsedMinute;

                temp /= 60;

                mElapsedHour = temp % 24;

                temp -= mElapsedHour;

                temp /= 24;

                mElapsedDay = temp;

                return mElapsedDay;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LPRDbSelect|CalculateElapsedTime", ex.ToString());
                return mElapsedDay;
            }
        }

        /// <summary>
        /// 거스름돈이 없을시 현재 보유현금을 가지고 돈이 부족할시 센터로 알람을 보낸다
        /// </summary>
        public static void NoCheckCargeMoneyOut()
        {
            try
            {
                int cash50SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50SettingQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash50SettingQty));
                int cash100SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100SettingQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash100SettingQty));
                int cash500SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500SettingQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash500SettingQty));
                int cash5000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash5000SettingQty));
                int cash1000SettingQty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash1000SettingQty));
                int cash50MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MinQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash50MinQty));
                int cash100MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MinQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash100MinQty));
                int cash500MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MinQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash500MinQty));
                int cash1000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MinQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash1000MinQty));
                int cash5000MinQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MinQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash5000MinQty));
                int cash50MaxQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash50MaxQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash50MaxQty));
                int cash100MaxQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash100MaxQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash100MaxQty));
                int cash500MaxQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash500MaxQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash500MaxQty));
                int cash1000MaxQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash1000MaxQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash1000MaxQty));
                int cash5000MaxQqty = Convert.ToInt32(NPSYS.Config.GetValue(ConfigID.Cash5000MaxQty) == string.Empty ? "0" : NPSYS.Config.GetValue(ConfigID.Cash5000MaxQty));
                if (NPSYS.Device.UsingSettingBill)
                {
                    MoneyBillOutDeviice.BillDischargeStatusManageMent.setCash(cash5000SettingQty, cash1000SettingQty, cash500SettingQty, cash100SettingQty, cash50SettingQty, cash50MinQqty, cash100MinQqty, cash500MinQqty, cash1000MinQqty, cash5000MinQqty, cash50MaxQqty, cash100MaxQqty, cash500MaxQqty, cash1000MaxQqty, cash5000MaxQqty);
                }
                if (NPSYS.Device.UsingSettingCoinCharger50)
                {
                    NPSYS.Device.CoinDispensor50.CurrentCoinDispensorStatusManagement.setCash(cash5000SettingQty, cash1000SettingQty, cash500SettingQty, cash100SettingQty, cash50SettingQty, cash50MinQqty, cash100MinQqty, cash500MinQqty, cash1000MinQqty, cash5000MinQqty, cash50MaxQqty, cash100MaxQqty, cash500MaxQqty, cash1000MaxQqty, cash5000MaxQqty);
                }
                if (NPSYS.Device.UsingSettingCoinCharger100)
                {
                    NPSYS.Device.CoinDispensor100.CurrentCoinDispensorStatusManagement.setCash(cash5000SettingQty, cash1000SettingQty, cash500SettingQty, cash100SettingQty, cash50SettingQty, cash50MinQqty, cash100MinQqty, cash500MinQqty, cash1000MinQqty, cash5000MinQqty, cash50MaxQqty, cash100MaxQqty, cash500MaxQqty, cash1000MaxQqty, cash5000MaxQqty);
                }
                if (NPSYS.Device.UsingSettingCoinCharger500)
                {
                    NPSYS.Device.CoinDispensor500.CurrentCoinDispensorStatusManagement.setCash(cash5000SettingQty, cash1000SettingQty, cash500SettingQty, cash100SettingQty, cash50SettingQty, cash50MinQqty, cash100MinQqty, cash500MinQqty, cash1000MinQqty, cash5000MinQqty, cash50MaxQqty, cash100MaxQqty, cash500MaxQqty, cash1000MaxQqty, cash5000MaxQqty);
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|NoCheckCargeMoneyOut", ex.ToString());
            }
        }

        public static string GetIpCatuer(string p_CarserverFile)
        {
            string ipCaptuere = p_CarserverFile.Replace("\\", "");
            int index = ipCaptuere.ToUpper().IndexOf("M");
            if (index > 1)
            {
                return ipCaptuere.SafeSubstring(0, index);
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region Private Methods

        private static void GetDeviceErrorRecoveryType()
        {
            //string coinBillRecoveryType = mConfig.GetValue(ConfigID.FeatureSettingCoinBillErrorRecoveryMode);
            //CoinBillMecRecovery = (coinBillRecoveryType.Trim() == string.Empty ? NPCommon.ConfigID.ErrorRecoveryType.MANUAL : (NPCommon.ConfigID.ErrorRecoveryType)Enum.Parse(typeof(NPCommon.ConfigID.ErrorRecoveryType), coinBillRecoveryType.Trim()));

            string DIdoRecoveryType = mConfig.GetValue(ConfigID.FeatureSettingDidoErrorRecoveryMode);
            ControlBoardRecovery = (DIdoRecoveryType.Trim() == string.Empty ? NPCommon.ConfigID.ErrorRecoveryType.MANUAL : (NPCommon.ConfigID.ErrorRecoveryType)Enum.Parse(typeof(NPCommon.ConfigID.ErrorRecoveryType), DIdoRecoveryType.Trim()));
        }

        /// <summary>
        /// 5만원권 사용여부
        /// </summary>
        /// <returns></returns>
        private static bool GetUseBill50000Qty()
        {
            switch (NPSYS.Config.GetValue(ConfigID.FeatureSettingInsert50000Qty).ToUpper())
            {
                case "Y":
                    SettingUse50000QtyBill = true;
                    return true;

                case "N":
                    SettingUse50000QtyBill = false;
                    return false;

                default:
                    SettingUse50000QtyBill = true;
                    return true;
            }
        }

        private static bool GetUsePrintFullCuting()
        {
            switch (NPSYS.Config.GetValue(ConfigID.FeatureSettingFullcuting).ToUpper())
            {
                case "Y":
                    g_UsePrintFullCuting = true;
                    return true;

                case "N":
                    g_UsePrintFullCuting = false;
                    return false;

                default:
                    g_UsePrintFullCuting = false;
                    return false;
            }
        }

        private static void GetUseButtonSound()
        {
            switch (NPSYS.Config.GetValue(ConfigID.FeatureSettingButtonSound).ToUpper())
            {
                case "Y":
                    UseButtonSound = true;
                    break;

                case "N":
                    UseButtonSound = false;
                    break;

                default:
                    UseButtonSound = false;
                    break;
            }
        }

        /// <summary>
        /// 영수증 공급가/부가세 출력
        /// </summary>
        private static void GetUseReceiptSupplyPrint()
        {
            gUseReceiptSupplyPrint = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseReciptSupplyPrint).Trim() == "Y" ? true : false);
        }

        private static void GetRestFulInfo()
        {
            gRestFulLocalPort = NPSYS.Config.GetValue(ConfigID.RESTfulLocalPort);
            gRESTfulServerIp = NPSYS.Config.GetValue(ConfigID.RESTfulServerIp);
            gRESTfulServerPort = NPSYS.Config.GetValue(ConfigID.RESTfulServerPort);
            gRESTfulVersion = NPSYS.Config.GetValue(ConfigID.RESTfulVersion);
        }

        /// <summary>
        /// 현재사용할 현금영수증 VAN타입을 가져온다.예 퍼스트데이타 ,스마트로 등등 밴사
        /// </summary>
        private static void GetCurrentCarNumType()
        {
            string carNumberType = (NPSYS.Config.GetValue(ConfigID.FeatureSettingCarNumberType).Trim() == string.Empty ? NPCommon.ConfigID.CarNumberType.Digit4SetAUTO.ToString() : NPSYS.Config.GetValue(ConfigID.FeatureSettingCarNumberType).Trim());
            ConfigID.CarNumberType lcarNumberType = (ConfigID.CarNumberType)Enum.Parse(typeof(ConfigID.CarNumberType), carNumberType);
            CurrentCarNumType = lcarNumberType;
        }

        /// <summary>
        /// true면 분리해서 전광판 보여줌 false면 종전그대로 전체 표출
        /// </summary>
        private static void GetuseTicketCardSplit()
        {
            string isuseTicketCardSplit = NPSYS.Config.GetValue(ConfigID.FeatureSettingUseCreditAndTIcketSplit).ToUpper().Trim();
            switch (isuseTicketCardSplit)
            {
                case "Y":
                    useTicketCardSplit = true;
                    break;

                default:
                    useTicketCardSplit = false;
                    break;
            }
        }

        //KICC DIP적용

        /// <summary>
        /// 현재사용할 현금영수증 VAN타입을 가져온다.예 퍼스트데이타 ,스마트로 등등 밴사
        /// </summary>
        private static void GetCASHVANTYPENAME()
        {
            string lVANTypeName = (NPSYS.Config.GetValue(ConfigID.FeatureSettingCashSelectVan).Trim() == string.Empty ? "FIRSTDATA" : NPSYS.Config.GetValue(ConfigID.FeatureSettingCashSelectVan).Trim());
            ConfigID.CardReaderType lVANTYPE = (ConfigID.CardReaderType)Enum.Parse(typeof(ConfigID.CardReaderType), lVANTypeName);
            CURRENTCASHVANTYPE = lVANTYPE;
            if (CURRENTCASHVANTYPE == ConfigID.CardReaderType.None)
            {
                NPSYS.Device.UsingUsingSettingCashReceipt = false;
            }
            else
            {
                NPSYS.Device.UsingUsingSettingCashReceipt = true;
            }
        }

        //KICC DIP적용완료

        /// <summary>
        /// 할인권 포맷을 읽어온다.
        /// </summary>
        private static void GetDiscountReadingFormat()
        {
            string lDiscountReadingFormat = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseDiscountFormat).ToUpper().Trim() == string.Empty ? "TRACK2ISO_TRACK3210" : NPSYS.Config.GetValue(ConfigID.FeatureSettingUseDiscountFormat).ToUpper().Trim());
            DiscountReadingFormat lReadingType = (DiscountReadingFormat)Enum.Parse(typeof(DiscountReadingFormat), lDiscountReadingFormat);
            CURRENTDiscountReadingFormat = lReadingType;
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain1080|GetDiscountReadingFormat", "현재할인권포맷:" + CURRENTDiscountReadingFormat.ToString());
        }

        /// <summary>
        /// 출차시 동영상 출력여부(정기권 회차)
        /// </summary>
        private static void GetUseOutMovie()
        {
            g_UseOutMovie = NPSYS.Config.GetValue(ConfigID.FeatureSettingUseOutMovie).ToUpper().Trim() == "Y" ? true : false;
        }

        /// <summary>
        /// 관리자모드 보유현금 방출 옵션값 가져오기
        /// </summary>
        private static void GetCurrentMoneyOutPutType()
        {
            string moneyOutputType = mConfig.GetValue(ConfigID.FeatureSettingMoneyOutputType);
            NPCommon.ConfigID.MoneyOutputType moneyoutEnum = (moneyOutputType.Trim() == string.Empty ? NPCommon.ConfigID.MoneyOutputType.None : (NPCommon.ConfigID.MoneyOutputType)Enum.Parse(typeof(NPCommon.ConfigID.MoneyOutputType), moneyOutputType.Trim()));
            CurrentMoneyOutputType = moneyoutEnum;
        }

        private static void GetCurrentMoneyType()
        {
            string MoneyTypeDataMode = mConfig.GetValue(ConfigID.FeatureSettingMoneyTypeDataMode);
            NPCommon.ConfigID.MoneyType moneyoutEnum = (MoneyTypeDataMode.Trim() == string.Empty ? NPCommon.ConfigID.MoneyType.WON : (NPCommon.ConfigID.MoneyType)Enum.Parse(typeof(NPCommon.ConfigID.MoneyType), MoneyTypeDataMode.Trim()));
            CurrentMoneyType = moneyoutEnum;
        }

        private static void GetJuminImageFolder()
        {
            mJuminImageFolder = NPSYS.Config.GetValue(ConfigID.FeatureSettingJuminImageFolder).Trim();
        }

        /// <summary>
        /// 스마트로 VCat신용단말기 사용시 초기 로드 실패시 무인정산기 종료시킬지여부 true면 종료시킴
        /// </summary>
        /// <returns></returns>
        private static void GetUseVCatFailRestart()
        {
            gUseVCatFailRestart = (NPSYS.Config.GetValue(ConfigID.FeatureSettingVCatFailRestart).Trim() == "N" ? false : true);
        }

        /// <summary>
        /// 스마트로 VCat신용단말기 음성사용일시 카드삽입 또는 재삽입필요시 음성멘트사용
        /// </summary>
        /// <returns></returns>
        private static void GetVcatUseVoice()
        {
            gUseVcatVoice = (NPSYS.Config.GetValue(ConfigID.FeatureSettingVcatUseVoice).Trim() == "N" ? false : true);
        }

        private static void GetMagamStyle()
        {
            gUseAutoMagam = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseAutoMagam).ToUpper() == "Y" ? true : false);
            gUseMagmDelay = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseMagamDelay).ToUpper() == "Y" ? true : false);
            gMagamEndTIme = (mConfig.GetValue(ConfigID.FeatureSettingMagamEndTIme).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamEndTIme).Replace(":", ""));
            gMagamStartTIme = (mConfig.GetValue(ConfigID.FeatureSettingMagamStartTIme).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamStartTIme).Replace(":", ""));
            gMagmDelayEndTime = (mConfig.GetValue(ConfigID.FeatureSettingMagamDelayEndTime).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamDelayEndTime).Replace(":", ""));
            gMagmDelayStartTime = (mConfig.GetValue(ConfigID.FeatureSettingMagamDelayStartTime).ToUpper() == "" ? "0000" : mConfig.GetValue(ConfigID.FeatureSettingMagamDelayStartTime).Replace(":", ""));
        }

        /// 사전무인 설정을 가져옴
        private static void GetUsePrePaySetting()
        {
            gPayRuleUseDiscountFreeCarQuestionPay = (mConfig.GetValue(ConfigID.FeatureSettingUseDiscountFreeCarPay).ToUpper() == "N" ? false : true);
            gPayRuleUsePrePayConfirm = (mConfig.GetValue(ConfigID.FeatureSettingUsePrePayConfirm).ToUpper() == "N" ? false : true);
            gPayRuleFreeCarQuestionPay = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseFreeCarPay).Trim() == "Y" ? true : false);
            gPayRuleUseRePay = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseRepay).Trim() == "Y" ? true : false);
            gPayRuleUsePreCarRegExtensionPay = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseRegExtensionPay).Trim() == "Y" ? true : false);
        }

        private static void SetLanguage()
        {
            gUseMultiLanguage = NPSYS.Config.GetValue(ConfigID.FeatureSettingUseMultiLanguage) == "Y" ? true : false;
            string languageTypeData = NPSYS.Config.GetValue(ConfigID.FeatureSettingLanguage);
            CurrentLanguageType = (ConfigID.LanguageType)Enum.Parse(typeof(ConfigID.LanguageType), (languageTypeData == string.Empty ? ConfigID.LanguageType.KOREAN.ToString() : languageTypeData));
        }

        /// <summary>
        /// 사전정산시 요금없는차량 영수증발급안함
        /// </summary>
        private static void GetUsePreFreeCarNoRecipt()
        {
            gUsePreFreeCarNoRecipt = (mConfig.GetValue(ConfigID.FeatureSettingUsePreFreeCarNoRecipt).ToUpper() == "Y" ? true : false);
        }

        /// <summary>
        /// 삼성페이결제
        /// </summary>
        private static void GetUseSamSungPay()
        {
            gUseSamSungPay = mConfig.GetValue(ConfigID.FeatureSettingUseSamSungPay).ToUpper() == "Y" ? true : false;
        }

        private static void GetBoothRealMode()
        {
            isBoothRealMode = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseRealMode).Trim() == "N" ? false : true);
        }

        private static void GetReceiptSignalNumber()
        {
            gReceiptSignalNumber = Convert.ToInt32((NPSYS.Config.GetValue(ConfigID.FeatureSettingReceiptSingalNumber).ToUpper() == string.Empty ? "9" : NPSYS.Config.GetValue(ConfigID.FeatureSettingReceiptSingalNumber).ToUpper()));
        }

        /// <summary>
        /// Door신호관련
        /// </summary>
        private static void GetDoorSignalNumber()
        {
            gDoorSignalNumber = Convert.ToInt32((NPSYS.Config.GetValue(ConfigID.FeatureSettingDoorSingalNumber).ToUpper() == string.Empty ? "1" : NPSYS.Config.GetValue(ConfigID.FeatureSettingDoorSingalNumber).ToUpper()));
        }

        private static void addRow(DataTable _dt, string p_name, string p_qty)
        {
            if (_dt == null)
                return;
            try
            {
                DataRow r = _dt.NewRow();
                r["Name"] = p_name;
                r["Qty"] = p_qty;
                _dt.Rows.Add(r);
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS|addRow", ex.ToString());
            }
        }

        /// <summary>
        /// Key 값에 해당하는 정보로 설정 언어에 맞게 반환한다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2020-02-06 이재영 Created.
        /// RecipePrint 함수에서 아래와 같은 긴 코드를 쓰고 있어서 별도 함수로 뺐음
        /// </remarks>
        private static string GetDynamicLanguage(string key)
        {
            return NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, key);
        }

        /// <summary>
        /// p_temrm은 1에 0.1초이며 p_term이 되면 작업을 마침
        /// </summary>
        /// <param name="p_term"></param>
        private static void applicationDoevent(int p_term)
        {
            int i = 0;
            while (true)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
                i++;
                if (i > p_term)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// TMAP연동
        /// </summary>
        private static void GetUseTmap()
        {
            gUseTmap = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseTmap).Trim() == "Y" ? true : false);
        }

        /// <summary>
        /// 카드실패전송
        /// </summary>
        private static void UseCardFailSend()
        {
            gUseCardFailSend = (NPSYS.Config.GetValue(ConfigID.FeatureSettingUseCardFailSend).Trim() == "Y" ? true : false);
        }

        private static void GetDiscountType(CloseDiscount pCloseDiscount, bool pIsManualMagam)
        {
            if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
            {
                Print.HmcDiscountPrint(pCloseDiscount.discountTypeName, TextCore.ToCommaString(pCloseDiscount.discountTypeCnt), TextCore.ToCommaString(pCloseDiscount.discountTypeAmt));
            }
            TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(14, pCloseDiscount.discountTypeName.ToString()) + "  " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(pCloseDiscount.discountTypeCnt)) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(pCloseDiscount.discountTypeAmt)));

            if (pCloseDiscount.closeDiscountItem != null && pCloseDiscount.closeDiscountItem.Count > 0)
            {
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    foreach (CloseDiscountItem closediscountDetail in pCloseDiscount.closeDiscountItem)
                    {
                        Print.HmcDiscountPrint(closediscountDetail.discountName, TextCore.ToCommaString(closediscountDetail.discountCnt.ToString()), TextCore.ToCommaString(closediscountDetail.discountAmt));
                    }
                }
                foreach (CloseDiscountItem closediscountDetail in pCloseDiscount.closeDiscountItem)
                {
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + TextCore.ToLeftAlignString(14, closediscountDetail.discountName) + "  " + TextCore.ToRightAlignString(10, TextCore.ToCommaString(closediscountDetail.discountCnt.ToString())) + TextCore.Space(7) + TextCore.ToRightAlignString(10, TextCore.ToCommaString(closediscountDetail.discountAmt)));
                }
                if (NPSYS.Device.UsingSettingPrint != ConfigID.PrinterType.NONE && pIsManualMagam)
                {
                    Print.HmcPrintTwoLine();
                    TextCore.ACTION(TextCore.ACTIONS.MANAGER, "FormAdminCashSetting|PrintMagam", "   " + "===========================================");
                }
            }
        }

        #endregion

        #region 언어변역

        /// <summary>
        /// 화면상에 UI등의 글자에 따른 표출문구 클래스(
        /// </summary>
        public class LanguageConvert
        {
            public enum Header
            {
                transaction,
                dynamictype
            }

            private static transaction mKorTransaction = new transaction();
            private static dynamictype mKorDynamicsType = new dynamictype();
            private static transaction mJpnTransaction = new transaction();
            private static dynamictype mJpnDynamicsType = new dynamictype();

            private static transaction mEngTransaction = new transaction();
            private static dynamictype mEngDynamicsType = new dynamictype();

            private static Dictionary<string, string> gDic_KorTransaction = new Dictionary<string, string>();
            private static Dictionary<string, string> gDic_KorDynamics = new Dictionary<string, string>();
            private static Dictionary<string, string> gDic_JpnTransaction = new Dictionary<string, string>();
            private static Dictionary<string, string> gDic_JpnDynamics = new Dictionary<string, string>();

            private static Dictionary<string, string> gDic_EngTransaction = new Dictionary<string, string>();
            private static Dictionary<string, string> gDic_EngDynamics = new Dictionary<string, string>();

            public static string GetLanguageData(ConfigID.LanguageType pLanguageType, LanguageConvert.Header pHeader, string pKey)
            {
                try
                {
                    string returnData = string.Empty;
                    switch (pLanguageType)
                    {
                        case ConfigID.LanguageType.KOREAN:
                            if (pHeader == Header.transaction)
                            {
                                return gDic_KorTransaction[pKey].Replace("+", "\n");
                            }
                            if (pHeader == Header.dynamictype)
                            {
                                return gDic_KorDynamics[pKey].Replace("+", "\n");
                            }
                            break;

                        case ConfigID.LanguageType.JAPAN:
                            if (pHeader == Header.transaction)
                            {
                                return gDic_JpnTransaction[pKey].Replace("+", "\n");
                            }
                            if (pHeader == Header.dynamictype)
                            {
                                return gDic_JpnDynamics[pKey].Replace("+", "\n");
                            }
                            break;

                        case ConfigID.LanguageType.ENGLISH:
                            if (pHeader == Header.transaction)
                            {
                                return gDic_EngTransaction[pKey].Replace("+", "\n");
                            }
                            if (pHeader == Header.dynamictype)
                            {
                                return gDic_EngDynamics[pKey].Replace("+", "\n");
                            }
                            break;
                    }
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LanguageConvert : GetLanguageData", ex.ToString());
                    return string.Empty;
                }
            }

            public static void GetSettingLangaugeData()
            {
                string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string korpath = System.IO.Path.GetDirectoryName(path) + @"\lang\kor.txt";
                string jpnpath = System.IO.Path.GetDirectoryName(path) + @"\lang\jpn.txt";
                string engpath = System.IO.Path.GetDirectoryName(path) + @"\lang\eng.txt";
                mKorTransaction = new transaction();
                mKorDynamicsType = new dynamictype();
                mJpnTransaction = new transaction();
                mJpnDynamicsType = new dynamictype();
                mEngTransaction = new transaction();
                mEngDynamicsType = new dynamictype();

                gDic_KorTransaction = new Dictionary<string, string>();
                gDic_KorDynamics = new Dictionary<string, string>();
                gDic_JpnTransaction = new Dictionary<string, string>();
                gDic_JpnDynamics = new Dictionary<string, string>();
                gDic_EngTransaction = new Dictionary<string, string>();
                gDic_EngDynamics = new Dictionary<string, string>();

                ReadTxtFileSet(korpath, mKorTransaction, mKorDynamicsType, gDic_KorTransaction, gDic_KorDynamics);
                ReadTxtFileSet(jpnpath, mJpnTransaction, mJpnDynamicsType, gDic_JpnTransaction, gDic_JpnDynamics);
                ReadTxtFileSet(engpath, mEngTransaction, mEngDynamicsType, gDic_EngTransaction, gDic_EngDynamics);
            }

            private static void ReadTxtFileSet(string pFIlePath, transaction pTransaction, dynamictype pDynamicType, Dictionary<string, string> pTrasactionDic, Dictionary<string, string> pDynamicDic)
            {
                using (StreamReader r = new StreamReader(pFIlePath, Encoding.UTF8))
                {
                    string jsonFile = r.ReadToEnd();
                    JObject objPaser = JObject.Parse(jsonFile);
                    var transactionDataObject = objPaser[Header.transaction.ToString()];
                    pTransaction = JsonConvert.DeserializeObject<transaction>(transactionDataObject.ToString());

                    Type type = typeof(transaction);
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        string memberName = prop.Name;
                        string memberValue = string.Empty;
                        try
                        {
                            memberValue = prop.GetValue(pTransaction).ToString();
                        }
                        catch
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "LanguageConvert : GetLangauge", "[자료값없음]" + memberName.ToString());
                        }

                        pTrasactionDic.Add(memberName, memberValue);
                    }

                    var dynamictypeDataObject = objPaser[Header.dynamictype.ToString()];
                    pDynamicType = JsonConvert.DeserializeObject<dynamictype>(dynamictypeDataObject.ToString());

                    Type typeDynamic = typeof(dynamictype);
                    foreach (PropertyInfo prop in typeDynamic.GetProperties())
                    {
                        string memberName = prop.Name;
                        string memberValue = string.Empty;
                        try
                        {
                            memberValue = prop.GetValue(pDynamicType).ToString();
                        }
                        catch
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "LanguageConvert : GetLangauge", "[자료값없음]" + memberName.ToString());
                        }

                        pDynamicDic.Add(memberName, memberValue);
                    }
                }
            }
        }

        #endregion 언어변역

        #region 장비사용여부

        /// <summary>
        /// 장비사용여부 및 정상여부
        /// </summary>
        public class Device
        {
            //private static ObjectClient mObjectClient = new ObjectClient(1); // LPR Object 시리얼 인터페이스
            private static HMC60 mHMC60 = null;			// 영수증 프린터

            private static CoinDispensor mCoinDispensor_50 = null; //동전방출기
            private static CoinDispensor mCoinDispensor_100 = null; //동전방출기
            private static CoinDispensor mCoinDispensor_500 = null; //동전방출기
            private static CoinReader mCoinReader = null; //동전방출기
            private static BillReader mBillReader = null; //동전방출기
            private static GoodTechContorlBoard mDoSensor = null;  // 정산기 불빛제어 및 신호감지
            private static NexpaControlBoard mNexpaDoSensor = null;
            private static TicketCardDevice mCreditDevice2 = null; // 카드리더기 2
            private static TMoney mTMoney = null;
            private static TmoneySmartro mTmoneySmartro = null;
            private static BarcodeSerial mBarcodeSerial = null;
            private static Smartro_TL3500S mTmoneySmartro3500 = null; //TMoney 스마트로(TL3500S) 적용

            //바코드모터드리블 사용
            private static BarcodeMoter mBarcodeMoter = null;

            //바코드모터드리블 사용완료

            private static AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT mSmtSndRcv = null;

            private static AxKisPosAgentLib.AxKisPosAgent mKisPosAgent = null;

            //KICC DIP적용

            private static KICC_TIT mKICC_TIT = null;
            //KICC DIP적용완료

            //스마트로 TIT_DIP EV-CAT 적용
            private static AxDreampos_Ocx.AxDP_Certification_Ocx mSmartro_TITDIP_Evcat = null;

            //스마트로 TIT_DIP EV-CAT 적용완료

            //나이스연동
            private static NiceTcpClinet mNiceTcpClinet = null;

            //나이스연동완료

            // 신분증인식기 적용
            private static SinBunReader mSinbunReader = null;

            // 신분증인식기 적용완료

            private static CreditReaderStatusManageMent mCreditReaderStatusManageMent = null;

            public static string CoinReaderDeviceErrorMessage
            {
                set;
                get;
            }

            public static string BillDischargeDeviceErrorMessage
            {
                set;
                get;
            }

            public static string CoinDischargeDeviceErrorMessage
            {
                set;
                get;
            }

            public static string CoinDischarge50DeviceErrorMessage
            {
                set;
                get;
            }

            public static string CoinDischarge100DeviceErrorMessage
            {
                set;
                get;
            }

            public static string CoinDischarge500DeviceErrorMessage
            {
                set;
                get;
            }

            public static string ReceiptPrintDeviceErrorMessage
            {
                set;
                get;
            }

            public static string CreditCardDeviceErrorMessage1
            {
                set;
                get;
            }

            public static string CreditCardDeviceErrorMessage2
            {
                set;
                get;
            }

            public static string DiscountDeviceErrorMessage
            {
                set;
                get;
            }

            public static string LprDisplayErrorMessage
            {
                set;
                get;
            }

            public static string LprDbErrorMessage
            {
                set;
                get;
            }

            public static string LprErrorMessage
            {
                set;
                get;
            }

            public static string TMONEYDeviceErrorMessage
            {
                set;
                get;
            }

            public static string DIDODeviceErrorMessage
            {
                set;
                get;
            }

            public static string BARCODEDeviceErrorMessage
            {
                set;
                get;
            }

            public static string KungChaDeviceErrorMessage
            {
                set;
                get;
            }

            public static string SerialKeyboardErrorMessage
            {
                set;
                get;
            }

            //견광등 적용

            public static string KungandongErrorMessage
            {
                set;
                get;
            }

            // 견광등 적용관련

            // 신분증인식기 적용
            public static string SinbunReaderErrorMessage
            {
                set;
                get;
            }

            // 신분증인식기 적용완료
            //전동어닝 제어 적용
            public static string ElecAwningErrorMessage
            {
                set;
                get;
            }

            //전동어닝 제어 적용완료

            public static TMoney T_Money
            {
                get { return mTMoney; }
                set { mTMoney = value; }
            }

            public static TmoneySmartro T_MoneySmartro
            {
                get { return mTmoneySmartro; }
                set { mTmoneySmartro = value; }
            }

            /// <summary>
            /// 영수증 프린터
            /// </summary>
            public static HMC60 HMC60
            {
                get { return mHMC60; }
                set { mHMC60 = value; }
            }

            public static CoinReader CoinReader
            {
                get { return mCoinReader; }
                set { mCoinReader = value; }
            }

            public static BillReader BillReader
            {
                get { return mBillReader; }
                set { mBillReader = value; }
            }

            public static CoinDispensor CoinDispensor50
            {
                get { return mCoinDispensor_50; }
                set { mCoinDispensor_50 = value; }
            }

            public static CoinDispensor CoinDispensor100
            {
                get { return mCoinDispensor_100; }
                set { mCoinDispensor_100 = value; }
            }

            public static CoinDispensor CoinDispensor500
            {
                get { return mCoinDispensor_500; }
                set { mCoinDispensor_500 = value; }
            }

            public static NexpaControlBoard NexpaDoSensor
            {
                get { return mNexpaDoSensor; }
                set { mNexpaDoSensor = value; }
            }

            public static GoodTechContorlBoard DoSensors
            {
                get { return mDoSensor; }
                set { mDoSensor = value; }
            }

            public static TicketCardDevice CardDevice2
            {
                get { return mCreditDevice2; }
                set { mCreditDevice2 = value; }
            }

            /// <summary>
            /// 신용카드 관련 장비 에러상태 제어
            /// </summary>
            public static CreditReaderStatusManageMent CreditReaderStatusManageMent
            {
                get { return mCreditReaderStatusManageMent; }
                set { mCreditReaderStatusManageMent = value; }
            }

            public static BarcodeSerial BarcodeSerials
            {
                get { return mBarcodeSerial; }
                set { mBarcodeSerial = value; }
            }

            //바코드모터드리블 사용
            public static BarcodeMoter BarcodeMoter
            {
                get { return mBarcodeMoter; }
                set { mBarcodeMoter = value; }
            }

            //바코드모터드리블 사용완료

            public static AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRcv
            {
                get { return mSmtSndRcv; }
                set { mSmtSndRcv = value; }
            }

            // 2016.10.27 KIS_DIP 추가
            public static AxKisPosAgentLib.AxKisPosAgent KisPosAgent
            {
                get { return mKisPosAgent; }
                set { mKisPosAgent = value; }
            }

            // 2016.10.27  KIS_DIP 추가종료

            //KICC DIP적용

            public static KICC_TIT KICC_TIT
            {
                get { return mKICC_TIT; }
                set { mKICC_TIT = value; }
            }

            //KICC DIP적용완료
            //스마트로 TIT_DIP EV-CAT 적용
            public static AxDreampos_Ocx.AxDP_Certification_Ocx Smartro_TITDIP_Evcat
            {
                get { return mSmartro_TITDIP_Evcat; }
                set { mSmartro_TITDIP_Evcat = value; }
            }

            //스마트로 TIT_DIP EV-CAT 적용완료

            //나이스연동
            public static NiceTcpClinet NiceTcpClinet
            {
                get { return mNiceTcpClinet; }
                set { mNiceTcpClinet = value; }
            }

            //나이스연동완료

            // 신분증인식기 적용
            public static SinBunReader SinbunReader
            {
                get { return mSinbunReader; }
                set { mSinbunReader = value; }
            }

            // 신분증인식기 적용완료

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 지폐방출기 사용여부 설정
            /// </summary>
            public static bool UsingSettingBill
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 지폐리더기 사용여부 설정
            /// </summary>
            public static bool UsingSettingBillReader
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 동전리더기 사용여부 설정
            /// </summary>
            public static bool UsingSettingCoinReader
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 동전방출기 사용여부 설정
            /// </summary>
            public static bool UsingSettingCoinCharger50
            {
                get;
                set;
            }

            public static bool UsingSettingCoinCharger100
            {
                get;
                set;
            }

            public static bool UsingSettingCoinCharger500
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 카드리더기 사용여부 설정
            /// </summary>
            public static bool UsingSettingCardRederDevice
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// OCS 사용여부설정
            /// </summary>
            public static bool UsingSettingOCS
            {
                get;
                set;
            }

            public static bool UsingSettingRestFul
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 영수증프린터 사용여부 설정
            /// </summary>
            public static NPCommon.ConfigID.PrinterType UsingSettingPrint
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// Tmoney 사용여부 설정
            /// </summary>
            public static bool UsingSettingTmoney
            {
                get;
                set;
            }

            public static bool UsingSettingHubulMoney
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 할인권 사용여부 설정
            /// </summary>
            public static bool UsingSettingDiscountCard
            {
                get;
                set;
            }

            //할인권 타입설정 추가
            public static bool UsingSettingMagneticDCTicket
            {
                get;
                set;
            }

            public static bool UsingSettingBarcodeDCTicket
            {
                get;
                set;
            }

            //할인권 타입설정 추가완료

            ////바코드모터드리블 사용

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 할인바코드 사용여부 설정
            /// </summary>
            //public static bool UsingSettingDiscountBarcodeSerial
            //{
            //    get;
            //    set;
            //}

            public static ConfigID.BarcodeReaderType UsingSettingDiscountBarcodeSerial
            {
                get;
                set;
            }

            //바코드모터드리블 사용완료
            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 음성 사용여부 설정
            /// </summary>
            public static bool UsingSettingSoundRead
            {
                get;
                set;
            }

            /// </summary>
            public static NPCommon.ConfigID.CardReaderType UsingSettingCardReadType
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 카드리더기 두번째(오른쪽)사용여부
            /// </summary>
            public static NPCommon.ConfigID.CardReaderType UsingSettingMagneticReadType
            {
                get;
                set;
            }

            /// <summary>
            /// 사용자가 아닌 시스템적으로
            /// 컨트롤보드 사용여무 물음
            /// </summary>
            public static NPCommon.ConfigID.ControlBoardType UsingSettingControlBoard
            {
                get;
                set;
            }

            private static bool mUsingSettingCreditCard = false;

            /// <summary>
            /// 시스템에서 신용카드결제사용여부
            /// </summary>
            public static bool UsingSettingCreditCard
            {
                get { return mUsingSettingCreditCard; }
                set { mUsingSettingCreditCard = value; }
            }

            private static bool mUsingSettingCashReceipt = false;

            /// <summary>
            /// 시스템에서 현금영수증 사용여무
            /// </summary>
            public static bool UsingUsingSettingCashReceipt
            {
                get { return mUsingSettingCashReceipt; }
                set { mUsingSettingCashReceipt = value; }
            }

            // 신분증인식기 적용
            public static bool UsingSettingSinbunReader
            {
                get;
                set;
            }

            /// <summary>
            /// Tmoney Smartro TL3500S
            /// </summary>
            public static Smartro_TL3500S TmoneySmartro3500 { get => mTmoneySmartro3500; set => mTmoneySmartro3500 = value; }

            // 신분증인식기 적용완료

            /// <summary>
            /// 교통카드 정상여부
            /// </summary>
            public static bool isUseDeviceTMoneyReaderDevice = false;

            /// <summary>
            /// 후불교통카드 정상여부
            /// </summary>
            public static bool isUseDeviceHubulReaderDevice = false;

            /// <summary>
            /// 동전인식기 정상여부
            /// </summary>
            public static bool isUseDeviceCoinReaderDevice = false;

            /// <summary>
            /// 지폐인식기 정상여부
            /// </summary>
            public static bool isUseDeviceBillReaderDevice = false;

            /// <summary>
            /// DIDO 정상요부
            /// </summary>
            public static bool gIsUseDidoDevice = false;

            /// <summary>
            /// 신용카드리더기1 정상여부
            /// </summary>
            public static bool gIsUseCreditCardDevice = false;

            /// <summary>
            /// 할인권리더기 정상여부
            /// </summary>
            public static bool gIsUseMagneticReaderDevice = false;

            /// <summary>
            /// 지폐방출기 정상여부
            /// </summary>
            public static bool gIsUseDeviceBillDischargeDevice = false;

            public static bool gIsUseCoinDischarger50Device = false;
            public static bool gIsUseCoinDischarger100Device = false;
            public static bool gIsUseCoinDischarger500Device = false;

            /// <summary>
            /// 영수증프린터 정상여부
            /// </summary>
            public static bool gIsUseReceiptPrintDevice = true;

            /// <summary>
            /// 전광판이 살았는지 여부
            /// </summary>
            public static bool gIsAliveJunganpan = true;

            /// <summary>
            /// 외부DB연결상여부
            /// </summary>
            public static bool gIsUseDBDevice = true;

            /// <summary>
            /// 출차LPR연결여부
            /// </summary>
            public static bool gIsUseLPRDevice = true;

            //나이스연동

            public static bool gIsUseNiceDevice = true;
            //나이스연동 통신정상여부

            /// <summary>
            /// 바코드시리얼 정상연결여부
            /// </summary>
            public static bool gIsUseBarcodeSerial = true;

            // 신분증인식기 적용
            public static bool gIsUseSinbunReader = false;

            // 신분증인식기 적용완료
            /// <summary>

            //KOCSE 카드리더기 추가

            // 2016.10.27 KIS_DIP 추가
            /// <summary>
            /// 키스 / SMARTRO 등 실제 물리적인 카드리더기정상여부
            /// </summary>
            /// <returns></returns>
            public static ConfigID.CardReaderType GetCurrentUseDeviceCard()
            {
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KIS_TIT_DIP_IFM && NPSYS.Device.gIsUseCreditCardDevice
                )
                {
                    return ConfigID.CardReaderType.KIS_TIT_DIP_IFM;
                }
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat && NPSYS.Device.gIsUseCreditCardDevice
               )
                {
                    return ConfigID.CardReaderType.SmartroVCat;
                }
                //KOCSE 카드리더기 추가
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_TCM && NPSYS.Device.gIsUseCreditCardDevice
          )
                {
                    return ConfigID.CardReaderType.KOCES_TCM;
                }
                //KOCSE 카드리더기 추가주석완료
                //KICC DIP적용
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_DIP_IFM && NPSYS.Device.gIsUseCreditCardDevice
                )
                {
                    return ConfigID.CardReaderType.KICC_DIP_IFM;
                }
                //KICC DIP적용완료
                //KICCTS141적용
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KICC_TS141 && NPSYS.Device.gIsUseCreditCardDevice
               )
                {
                    return ConfigID.CardReaderType.KICC_TS141;
                }
                //KICCTS141적용완료
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KOCES_PAYMGATE && NPSYS.Device.gIsUseCreditCardDevice
)
                {
                    return ConfigID.CardReaderType.KOCES_PAYMGATE;
                }
                // FIRSTDATA처리
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.FIRSTDATA_DIP && NPSYS.Device.gIsUseCreditCardDevice
)
                {
                    return ConfigID.CardReaderType.FIRSTDATA_DIP;
                }
                // FIRSTDATA처리 주석완료

                //KSNET
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.KSNET && NPSYS.Device.gIsUseCreditCardDevice)
                {
                    return ConfigID.CardReaderType.KSNET;
                }
                //KSNET 완료
                //스마트로 TIT_DIP EV-CAT 적용
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP && NPSYS.Device.gIsUseCreditCardDevice)
                {
                    return ConfigID.CardReaderType.SMATRO_TIT_DIP;
                }
                //스마트로 TIT_DIP EV-CAT 적용완료
                //Tmoney 스마트로 적용
                else if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TL3500S && NPSYS.Device.gIsUseCreditCardDevice)
                {
                    return ConfigID.CardReaderType.SMATRO_TL3500S;
                }
                //Tmoney 스마트로 적용완료
                else
                {
                    return ConfigID.CardReaderType.None;
                }
            }

            //KOCSE 카드리더기 추가 주석
            // 2016.10.27  KIS_DIP 추가종료

            /// <summary>
            /// NPCONFIG실행파일에서 설정한 장비사용 선택을 읽어온다
            /// 만약 장비사용을 하지 않게 설정되어있다면 장비이상쪽에도 같은 결과를 넣는다
            /// </summary>
            public static void allGetUseDeviceCheck()
            {
                // 시스템 적으로 설정한 사용 여부 설정
                // 시스템 적으로 설정한 사용 여부 설정

                NPSYS.Device.UsingSettingBill = (NPSYS.Config.GetValue(ConfigID.UsingSettingBill) == "Y" ? true : false);
                NPSYS.Device.isUseDeviceBillReaderDevice = NPSYS.Device.UsingSettingBill;

                NPSYS.Device.UsingSettingBillReader = (NPSYS.Config.GetValue(ConfigID.UsingSettingBillReader) == "Y" ? true : false);
                NPSYS.Device.isUseDeviceBillReaderDevice = NPSYS.Device.UsingSettingBillReader;

                NPSYS.Device.UsingSettingCoinReader = (NPSYS.Config.GetValue(ConfigID.UsingSettingCoinReader) == "Y" ? true : false);
                NPSYS.Device.isUseDeviceCoinReaderDevice = NPSYS.Device.UsingSettingCoinReader;

                NPSYS.Device.UsingSettingCoinCharger50 = (NPSYS.Config.GetValue(ConfigID.Cash50Use) == "Y" ? true : false);
                NPSYS.Device.UsingSettingCoinCharger100 = (NPSYS.Config.GetValue(ConfigID.Cash100Use) == "Y" ? true : false);
                NPSYS.Device.UsingSettingCoinCharger500 = (NPSYS.Config.GetValue(ConfigID.Cash500Use) == "Y" ? true : false);

                NPSYS.Device.UsingSettingCardRederDevice = (NPSYS.Config.GetValue(ConfigID.UsingSettingCredit) == "Y" ? true : false);
                NPSYS.Device.gIsUseCreditCardDevice = NPSYS.Device.gIsUseMagneticReaderDevice = NPSYS.Device.UsingSettingCardRederDevice;

                string printer = NPSYS.Config.GetValue(ConfigID.UsingSettingPrinterType).Trim() == string.Empty ? NPCommon.ConfigID.PrinterType.NONE.ToString() : NPSYS.Config.GetValue(ConfigID.UsingSettingPrinterType).Trim(); // 2016-01-19 스마트로 추가
                NPSYS.Device.UsingSettingPrint = (NPCommon.ConfigID.PrinterType)Enum.Parse(typeof(ConfigID.PrinterType), printer);
                NPSYS.Device.gIsUseReceiptPrintDevice = (NPSYS.Device.UsingSettingPrint == ConfigID.PrinterType.NONE ? false : true);
                NPSYS.Device.UsingSettingTmoney = (NPSYS.Config.GetValue(ConfigID.UsingSettingTmoney) == "Y" ? true : false);
                NPSYS.Device.isUseDeviceTMoneyReaderDevice = NPSYS.Device.UsingSettingTmoney;
                NPSYS.Device.UsingSettingHubulMoney = (NPSYS.Config.GetValue(ConfigID.UsingSettingHubulMoney) == "Y" ? true : false);
                NPSYS.Device.isUseDeviceHubulReaderDevice = NPSYS.Device.UsingSettingHubulMoney;

                NPSYS.Device.UsingSettingSoundRead = (NPSYS.Config.GetValue(ConfigID.UsingSettingSoundRead) == "Y" ? true : false);

                NPSYS.Device.UsingSettingDiscountCard = (NPSYS.Config.GetValue(ConfigID.UsingSettingDiscountCard) == "Y" ? true : false);
                //할인권 타입설정 추가
                NPSYS.Device.UsingSettingMagneticDCTicket = (NPSYS.Config.GetValue(ConfigID.UsingSettingMagneticDCTicket) == "Y" ? true : false);
                NPSYS.Device.UsingSettingBarcodeDCTicket = (NPSYS.Config.GetValue(ConfigID.UsingSettingBarcodeDCTicket) == "Y" ? true : false);
                //할인권 타입설정 추가완료

                //바코드모터드리블 사용
                string barcodeType = NPSYS.Config.GetValue(ConfigID.UsingSettingDiscountBarcode);
                if (barcodeType.Trim() == string.Empty)
                {
                    barcodeType = ConfigID.BarcodeReaderType.None.ToString();
                }
                else if (barcodeType.Trim() == "Y")
                {
                    barcodeType = ConfigID.BarcodeReaderType.NormaBarcode.ToString();
                }
                else if (barcodeType.Trim() == "N")
                {
                    barcodeType = ConfigID.BarcodeReaderType.None.ToString();
                }
                NPSYS.Device.UsingSettingDiscountBarcodeSerial = (NPCommon.ConfigID.BarcodeReaderType)Enum.Parse(typeof(NPCommon.ConfigID.BarcodeReaderType), barcodeType); // 2016-01-19 스마트로 추가
                // NPSYS.Device.UsingSettingDiscountBarcodeSerial = (NPSYS.Config.GetValue(ConfigID.UsingSettingDiscountBarcode) == "Y" ? true : false);
                //바코드모터드리블 사용완료

                NPSYS.Device.UsingSettingRestFul = (NPSYS.Config.GetValue(ConfigID.UsingSettingRestFul) == "Y" ? true : false);

                string cardReaderLeft = NPSYS.Config.GetValue(ConfigID.UsingSettingCardRederTypeLeft).Trim() == string.Empty ? NPCommon.ConfigID.CardReaderType.None.ToString() : NPSYS.Config.GetValue(ConfigID.UsingSettingCardRederTypeLeft).Trim(); // 2016-01-19 스마트로 추가
                string cardReaderRight = NPSYS.Config.GetValue(ConfigID.UsingSettingCardRederTypeRight).Trim() == string.Empty ? NPCommon.ConfigID.CardReaderType.None.ToString() : NPSYS.Config.GetValue(ConfigID.UsingSettingCardRederTypeRight).Trim(); // 2016-01-19 스마트로 추가
                NPSYS.Device.UsingSettingCardReadType = (NPCommon.ConfigID.CardReaderType)Enum.Parse(typeof(NPCommon.ConfigID.CardReaderType), cardReaderLeft); // 2016-01-19 스마트로 추가
                NPSYS.Device.UsingSettingMagneticReadType = (NPCommon.ConfigID.CardReaderType)Enum.Parse(typeof(NPCommon.ConfigID.CardReaderType), cardReaderRight); // 2016-01-19 스마트로 추가

                string controlBoardType = string.Empty;
                if (NPSYS.Config.GetValue(ConfigID.UsingSettingControlBoard) == "N" || NPSYS.Config.GetValue(ConfigID.UsingSettingControlBoard) == string.Empty)
                {
                    controlBoardType = ConfigID.ControlBoardType.NONE.ToString();
                }
                else
                {
                    controlBoardType = NPSYS.Config.GetValue(ConfigID.UsingSettingControlBoard);
                }
                NPSYS.Device.UsingSettingControlBoard = (NPCommon.ConfigID.ControlBoardType)Enum.Parse(typeof(NPCommon.ConfigID.ControlBoardType), controlBoardType); // 2016-01-19 스마트로 추가
                // 신분증인식기 적용
                NPSYS.Device.UsingSettingSinbunReader = (NPSYS.Config.GetValue(ConfigID.UsingSettingSinbunReader) == "Y" ? true : false);
                // 신분증인식기 적용완료
            }
        }

        #endregion 장비사용여부

        #region 로컬DB 세팅관련 클래스

        /// <summary>
        /// 로컬DB 세팅관련 클래스
        /// </summary>
        public class LocalDbSetting
        {
            /// <summary>
            /// 로컬Db에서 여러 상태등을 가져와서 변수에 담는다
            /// </summary>
            public bool Load_LocalDbInfo()
            {
                NPSYS.ConfigFilePath = FadeFox.Utility.Config.GetValue(ConfigID.ConfigFilePath);

                if (NPSYS.ConfigFilePath == "")
                {
                    MessageBox.Show("환경설정파일 경로를 설정해 주세요!");
                    return false;
                }

                NPSYS.Config = new ConfigDB3I(NPSYS.ConfigFilePath);
                NPSYS.Config.Open();

                ReadSerialPortInfo();
                ReadServerInfo();

                NPSYS.DeviceClosed = false;

                NPSYS.NPPaymentLog = new FadeFox.Database.SQLite.SQLite();
                NPSYS.NPPaymentLog.Database = NPSYS.Servers[ServerID.NPPaymentLog].ServerDatabase;
                NPSYS.NPPaymentLog.Connect();

                NPSYS.allGetFeatureSeeting();
                NPSYS.Device.allGetUseDeviceCheck();

                NPSYS.ParkCode = NPSYS.Config.GetValue(ConfigID.ParkingLotNo); // 주차장코드
                NPSYS.BoothID = NPSYS.Config.GetValue(ConfigID.ParkingLotBoothID);

                if (NPSYS.Device.UsingSettingRestFul)
                {
                    bool isSuccess = GetParkInfo();
                    return isSuccess;
                }
                else
                {
                    return true;
                }
            }

            public bool GetParkInfo()
            {
                NPHttpControl mNPHttpControl = new NPHttpControl();
                ParkingReceiveData restClassParser = new ParkingReceiveData();
                Certs currentCert = new Certs();
                mNPHttpControl.SetHeader(NPHttpControl.UrlCmdType.certs);
                currentCert = (Certs)restClassParser.SetDataParsing(NPHttpControl.UrlCmdType.certs, mNPHttpControl.SendMethodGet());
                if (currentCert.status.Success)
                {
                    switch (currentCert.nation.code.ToUpper())
                    {
                        case "JP":
                            NPSYS.CurrentLanguageType = ConfigID.LanguageType.KOREAN;
                            break;

                        case "KR":
                            NPSYS.CurrentLanguageType = ConfigID.LanguageType.KOREAN;
                            NPSYS.CurrentMoneyType = ConfigID.MoneyType.WON;
                            break;

                        case "PL":
                            //TODO : 임시 한국어 전용 설정
                            //NPSYS.CurrentLanguageType = ConfigID.LanguageType.ENGLISH;
                            NPSYS.CurrentLanguageType = ConfigID.LanguageType.KOREAN;
                            NPSYS.CurrentMoneyType = ConfigID.MoneyType.WON;
                            break;
                    }
                    DateTime currentDateTime = NPSYS.LongTypeToDateTime(currentCert.date.unixTime);
                    CommonFuction.SetSystemDateTime(currentDateTime.ToString("yyyyMMddHHmmss")); // 시간동기화
                    NPSYS.BoothName = currentCert.unit.name;
                    NPSYS.Config.SetValue(ConfigID.ParkingLotBusinessNo, currentCert.parkingLot.businessNo);
                    NPSYS.Config.SetValue(ConfigID.ParkingLotDaepyo, currentCert.parkingLot.ceoName);
                    NPSYS.Config.SetValue(ConfigID.ParkingLotTelNo, currentCert.parkingLot.phone);
                    NPSYS.Config.SetValue(ConfigID.ParkingLotAddress, currentCert.parkingLot.address);
                    NPSYS.Config.SetValue(ConfigID.ParkingLotName, currentCert.parkingLot.name);
                    NPSYS.Config.SetValue(ConfigID.ParkingLotID, currentCert.parkingLot.id.ToString());
                    NPSYS.Config.SetValue(ConfigID.ParkingLotCode, currentCert.parkingLot.code);
                    NPSYS.gGurrentMoneTary = currentCert.monetary;
                    //NPSYS.CurrentLanguageType = (ConfigID.LanguageType)Enum.Parse(typeof(ConfigID.LanguageType), currentCert.monetary.language.ToUpper());
                    //NPSYS.CurrentMoneyType=(ConfigID.MoneyType)Enum.Parse(typeof(ConfigID.MoneyType), NPSYS.gGurrentMoneTary.unitName);

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "NPSYS | GetParkInfo", "[받아온주차장정보]"
                        + " 사업자:" + currentCert.parkingLot.businessNo
                        + " 대표자:" + currentCert.parkingLot.ceoName
                        + " 전화번호:" + currentCert.parkingLot.phone
                        + " 주차장명:" + currentCert.parkingLot.name
                        + " 주차장주소:" + currentCert.parkingLot.address
                        + " id:" + currentCert.parkingLot.id.ToString()
                        + " 주차장코드:" + currentCert.parkingLot.code);

                    return true;
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "NPSYS | GetParkInfo", "[개국조회]" + currentCert.status.message);
                    return false;
                }
            }

            /// <summary>
            /// 로컬DB에서 저장된 시리얼정보를 가져온다.
            /// </summary>
            /// <returns></returns>
            public bool ReadSerialPortInfo()
            {
                SQLite config = new SQLite();

                config.Database = NPSYS.ConfigFilePath;
                config.Connect();

                //mSerialPorts.

                string sql = string.Empty;

                sql = "  SELECT *"
                    + "    FROM ST_SERIALPORT"
                    + "   WHERE USE_YN = 'Y'";

                try
                {
                    DataTable dt = config.SelectT(sql);

                    if (dt.Rows.Count < 1)
                    {
                        return false;
                    }

                    foreach (DataRow r in dt.Rows)
                    {
                        NPSYS.SerialPorts.Add(
                            r["SERIALPORT_ID"].ToString(),
                            r["SERIALPORT_NAME"].ToString(),
                            r["SERIALPORT_COMMENT"].ToString(),
                            r["PORTNAME"].ToString(),
                            r["BAUDRATE"].ToString(),
                            r["DATABITS"].ToString(),
                            r["PARITY"].ToString(),
                            r["STOPBITS"].ToString(),
                            r["HANDSHAKE"].ToString()
                            );
                    }

                    dt.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|ReadSerialPortInfo", ex.ToString());
                    return false;
                }
                finally
                {
                    config.Disconnect();
                }
            }

            /// <summary>
            /// 로컬DB에서 설정된 DB정보를 가져온다
            /// </summary>
            /// <returns></returns>
            public bool ReadServerInfo()
            {
                SQLite config = new SQLite();
                Rijndael mSecurity = new Rijndael();

                config.Database = NPSYS.ConfigFilePath;
                config.Connect();

                //mSerialPorts.

                string sql = string.Empty;

                sql = "  SELECT *"
                    + "    FROM ST_SERVER";

                try
                {
                    DataTable dt = config.SelectT(sql);

                    if (dt.Rows.Count < 1)
                    {
                        return false;
                    }

                    foreach (DataRow r in dt.Rows)
                    {
                        NPSYS.Servers.Add(
                            r["SERVER_ID"].ToString(),
                            r["SERVER_NAME"].ToString(),
                            r["SERVER_ADDRESS"].ToString(),
                            r["SERVER_USER_ID"].ToString(),
                            mSecurity.Decode(r["SERVER_USER_PASSWORD"].ToString()),
                            r["SERVER_DATABASE"].ToString(),
                            r["DATABASE_LID"].ToString(),
                            r["SERVER_PORT"].ToString()
                        );
                    }

                    dt.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormMain|ReadServerInfo", ex.ToString());
                    return false;
                }
                finally
                {
                    config.Disconnect();
                }
            }
        }

        #endregion 로컬DB 세팅관련 클래스
    }
}