using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NPCommon.DEVICE
{
    public class SinBunReader
    {
        [DllImport("SmartRail.DLL")]
        public static extern int SR_DeviceOpen(string path);
        [DllImport("SmartRail.DLL")]
        public static extern int SR_ScanIDCard(ref TIDCardInfo info);
        [DllImport("SmartRail.DLL")]
        public static extern int SR_ReadStatusInfo(ref TStatusInfo info);
        [DllImport("SmartRail.DLL")]
        public static extern int SR_DeviceClose();

        private static bool mIsConnect = false;
        private static bool mScanBlock = false;
        private static int mRetryCnt = 5;    // 미인식시 재시도 횟수 
        private static int mRetryNo = 0;     // 인식 재시도 횟수
        private static int mIsLastCard = 0;
        Thread mReadThread;
        public event CardReadData readEvent;
        public delegate void CardReadData(CardInfo info);

        public bool IsConnect
        {
            get { return mIsConnect; }
            set { mIsConnect = value; }
        }

        public static int RetryCnt
        {
            get { return mRetryCnt; }
            set { mRetryCnt = value; }
        }

        #region 인터페이스 구조체 선언부
        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct TIDCardInfo
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string sCardType;    // 카드타입
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string sJuminNum;    // 주민번호
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string sName;        // 이름
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string sYYMM;        // 등록일자(yyyy.MM.dd)
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
            public string sGrade;       // 장애등급
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string sBohunNum;    // 보훈카드번호
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
            public string sCardNum;     // 카드번호
        }

        [StructLayout(LayoutKind.Sequential), Serializable]
        public struct TStatusInfo
        {
            public int sCardOn;     // 카드인식상태
            public int sPhase;      // 인식기진행상태
        }
        #endregion

        #region 인식기 정보 CLASS 선언부
        public class CardInfo
        {
            public ScanResult ScanResult { set; get; }      // 스캔 결과
            public CardType CardType { set; get; }          // 카드형태
            public DiscountType DiscountType { get; set; }  // 할인타입
            public string JuminNum { set; get; }            // 주민번호
            public string Name { set; get; }                // 이름
            public string YYMM { set; get; }                // 주민등록증 발급일자
            public string Grade { set; get; }               // 나이
            public string BohunNum { set; get; }            // 보훈카드
            public string CardNum { set; get; }             // 카드번호

            public static ScanResult GetScanResult(int result)
            {
                switch (result)
                {
                    case -4: return ScanResult.Init;        // 초기치
                    case -3: return ScanResult.Fail;        // 인식실패/TimeOut
                    case -2: return ScanResult.ImageFail;   // 이미지 인식실패
                    case -1: return ScanResult.DevError;    // 장비 오류
                    case 0: return ScanResult.Processing;  // 인식중
                    case 1: return ScanResult.Success;     // 성공
                    case 3: return ScanResult.Forgery;     // 위조
                    default: return ScanResult.Fail;
                }
            }

            public static CardType GetCardType(int result)
            {
                switch (result)
                {
                    case 10: return CardType.Jumin;         // 주민등록증
                    case 11: return CardType.ForeSinGo;     // 재외국인 국내거소신고증
                    case 14: return CardType.ForeJumin;     // 재외국민 주민등록증
                    case 20: return CardType.BokJi;         // 복지카드
                    case 21: return CardType.OldBokJi;      // 구형복지카드
                    case 22: return CardType.NewBokJi;      // 신형복지카드
                    case 30: return CardType.BoHun;         // 보훈카드
                    case 33: return CardType.NewBoHun;      // 신형보훈카드
                    case 40: return CardType.Drive;         // 운전면허증
                    default: return CardType.Error;         // 오류
                }
            }

            public static string GetScanResultText(ScanResult result)
            {
                switch (result)
                {
                    case ScanResult.Init: return "초기치";
                    case ScanResult.Fail: return "인식실패/TimeOut";
                    case ScanResult.ImageFail: return "이미지처리오류";
                    case ScanResult.Processing: return "인식중";
                    case ScanResult.Success: return "성공";
                    case ScanResult.Forgery: return "위조지폐";
                    default: return result.ToString();
                }
            }

            public static string GetCardTypeText(CardType result)
            {
                switch (result)
                {
                    case CardType.Error: return "오류";
                    case CardType.Jumin: return "주민등록증";
                    case CardType.ForeSinGo: return "재외국민국내거소신고증";
                    case CardType.ForeJumin: return "재외국민 주민등록증";
                    case CardType.BokJi: return "복지카드";
                    case CardType.OldBokJi: return "구형복지카드";
                    case CardType.NewBokJi: return "신형복지카드";
                    case CardType.BoHun: return "보훈카드";
                    case CardType.NewBoHun: return "신형보훈카드";
                    case CardType.Drive: return "운전면허증";
                    default: return result.ToString();
                }
            }
        }

        public class StatusInfo
        {
            public CardOnStates CardOn { set; get; }
            public CardPhase Phase { set; get; }
            public int StatusResult { set; get; }

            public static CardOnStates GetCardOnStates(int result)
            {
                switch (result)
                {
                    case -1: return CardOnStates.Error;     // 감지에러
                    case 0: return CardOnStates.Wait;      // 대기
                    case 1: return CardOnStates.On;        // 감지
                    default: return CardOnStates.Error;
                }
            }

            public static CardPhase GetCardPhase(int result)
            {
                switch (result)
                {
                    case -2: return CardPhase.Init;         // 초기치
                    case -1: return CardPhase.DevError;     // 장비오류
                    case 0: return CardPhase.Wait;         // 대기
                    case 1: return CardPhase.Processing;   // 인식중
                    default: return CardPhase.Wait;
                }
            }

            public static string GetCardOnStatesText(CardOnStates result)
            {
                switch (result)
                {
                    case CardOnStates.Error: return "감지에러";
                    case CardOnStates.Wait: return "대기";
                    case CardOnStates.On: return "감지";
                    default: return result.ToString();
                }
            }

            public static string GetCardPhaseText(CardPhase result)
            {
                switch (result)
                {
                    case CardPhase.Init: return "초기치";
                    case CardPhase.DevError: return "장비오류";
                    case CardPhase.Wait: return "대기";
                    case CardPhase.Processing: return "인식중";
                    default: return result.ToString();
                }
            }
        }
        #endregion

        #region 열거형 선언부분
        public enum CardType
        {
            Error = 0,     // 오류
            Jumin = 10,     // 주민등록증
            ForeSinGo = 11,     // 재외국민국내거소신고증
            ForeJumin = 14,     // 재외국민 주민등록증
            BokJi = 20,     // 복지카드
            OldBokJi = 21,     // 구형복지카드
            NewBokJi = 22,     // 신형복지카드
            BoHun = 30,     // 보훈카드
            NewBoHun = 33,     // 신형보훈카드
            Drive = 40      // 운전면허증
        }

        public enum ScanResult
        {
            Init = -4,    // 초기치
            Fail = -3,    // 인식실패/TimeOut
            ImageFail = -2,    // 이미지처리오류
            DevError = -1,    // 장비인식오류
            Processing = 0,    // 인식중
            Success = 1,    // 성공
            Forgery = 3     // 위조지폐
        }

        public enum CardOnStates  // 카드 인식상태
        {
            Error = -1,     // 오류
            Wait = 0,     // 대기
            On = 1      // 감지
        }

        public enum CardPhase  // 카드 진행단계
        {
            Init = -2,  // 초기치
            DevError = -1,  // 장비오류
            Wait = 0,  // 대기
            Processing = 1   // 인식중
        }

        public enum DiscountType
        {
            BokJi = 1,      // 복지카드(장애인)
            BoHun           // 보훈카드
        }
        #endregion

        #region 생성자/소멸자
        public SinBunReader()
        {
        }

        ~SinBunReader()
        {
        }
        #endregion

        #region Thread 관련 함수
        public bool ThreadStart()
        {
            try
            {
                if (mReadThread == null || !mReadThread.IsAlive)
                {
                    mReadThread = new Thread(CardReader);
                    mReadThread.IsBackground = true;
                    mReadThread.Start();

                    TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|ThreadStart", "쓰레드 시작 성공");
                    return true;
                }
                return false;
            }
            catch
            {
                TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|ThreadStart", "쓰레드 시작 실패");
                return false;
            }

        }

        public bool ThreadEnd()
        {
            try
            {
                if (mReadThread.IsAlive)
                {
                    mReadThread.Abort();
                    TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|ThreadStart", "쓰레드 종료 성공");

                    return true;
                }
                return false;
            }
            catch
            {
                TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|ThreadStart", "쓰레드 종료 실패");
                return false;
            }
        }

        public void CardReader()
        {
            while (true)
            {
                if (IsConnect)
                {
                    PreScanCheck();
                    Thread.Sleep(300);
                }
            }
        }
        #endregion

        public bool Connect()
        {
            try
            {
                if (!IsConnect)
                {
                    string path = Application.StartupPath;

                    int result = SR_DeviceOpen(path);
                    if (result > 0)
                    {
                        IsConnect = true;
                        TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|Connect", "연결성공");
                    }
                    else
                    {
                        IsConnect = false;
                        TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|Connect", "연결실패");
                    }
                }
                return IsConnect;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|Connect", ex.ToString());
                return IsConnect;
            }
        }

        public bool DisConnect()
        {
            try
            {
                int result = 0;
                if (IsConnect)
                {
                    result = SR_DeviceClose();
                    if (result > 0)
                    {
                        IsConnect = false;
                        TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|Connect", "연결종료 성공");
                        return true;
                    }
                    else
                    {
                        TextCore.ACTION(TextCore.ACTIONS.SINBUNREADER, "SinBunReader|Connect", "연결종료 실패");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|DisConnect", ex.ToString());
                return false;
            }
        }

        public int ScanCard(ref CardInfo info)
        {
            int result = 0;
            try
            {
                if (IsConnect)
                {
                    TIDCardInfo tcdinfo = new TIDCardInfo();
                    result = SR_ScanIDCard(ref tcdinfo);   // -4:초기치, -3:인식실패, -2:이미지처리오류, -1:장비오류, 0:이미지인식중, 1:정상인식, 3:위조인식
                    PasingCardInfoData(tcdinfo, ref info);
                    info.ScanResult = CardInfo.GetScanResult(result);
                    if (result > 0)
                    {
                        PasingCardInfoData(tcdinfo, ref info);
                        readEvent(info);
                        TextCore.INFO(TextCore.INFOS.SINBUN_SUCCES, "SinBunReader|ScanCard", "인식정보 =>" + " [카드타입]" + CardInfo.GetCardTypeText(info.CardType) + "(" + ((int)info.CardType).ToString() + ")" + " [주민번호]" + info.JuminNum + " [이름]" + info.Name + "[발행일자]" + info.YYMM + " [장애등급]" + info.Grade + " [보훈번호]" + info.BohunNum + " [카드번호]" + info.CardNum);

                        mScanBlock = true;
                    }
                    else
                    {
                        mRetryNo += 1;
                        TextCore.INFO(TextCore.INFOS.SINBUN_FAIL, "SinBunReader|ScanCard", "인식실패 => [인식 재시도](" + mRetryNo.ToString() + " / " + mRetryCnt.ToString() + ") [인식 결과값]" + CardInfo.GetScanResultText(info.ScanResult) + "(" + result.ToString() + ")");
                        if (mRetryNo >= mRetryCnt)
                        {
                            TextCore.INFO(TextCore.INFOS.SINBUN_FAIL, "SinBunReader|ScanCard", "재시도 횟수 초과 - 미인식 카드 ");
                            mScanBlock = true;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|ScanCard", ex.ToString());
                return result;
            }
        }

        public int ReadState(ref StatusInfo info)
        {
            int result = -1;
            try
            {
                if (IsConnect)
                {
                    TStatusInfo tstInfo = new TStatusInfo();
                    result = SR_ReadStatusInfo(ref tstInfo);
                    info.StatusResult = result;
                    PasingStatesData(tstInfo, ref info);

                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|ReadState", ex.ToString());
                return result;
            }
        }

        public bool PreScanCheck()
        {
            try
            {
                if (IsConnect)
                {
                    StatusInfo stInfo = new StatusInfo();
                    CardInfo cdInfo = new CardInfo();
                    if (ReadState(ref stInfo) == 1)
                    {
                        if (mIsLastCard == 1 && stInfo.CardOn == CardOnStates.Wait)
                        {
                            mScanBlock = false;
                            mRetryNo = 0;
                        }
                        mIsLastCard = (int)stInfo.CardOn;

                        if (!mScanBlock && mIsLastCard == 1)
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SinBunReader|PreScanCheck", "인식기정보 =>" + " [인식상태]" + StatusInfo.GetCardOnStatesText(stInfo.CardOn) + "(" + ((int)stInfo.CardOn).ToString() + ")" + " [진행단계]" + StatusInfo.GetCardPhaseText(stInfo.Phase) + "(" + ((int)stInfo.Phase).ToString() + ")");
                            ScanCard(ref cdInfo);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|PreScanCheck", ex.ToString());
                return false;
            }
        }

        public void TestCardScan()
        {
            try
            {
                if (Connect())
                {
                    int result = 0;
                    TIDCardInfo tcdInfo = new TIDCardInfo();
                    CardInfo cdInfo = new CardInfo();
                    result = SR_ScanIDCard(ref tcdInfo);
                    if (result > 0)
                    {
                        PasingCardInfoData(tcdInfo, ref cdInfo);
                        TextCore.INFO(TextCore.INFOS.SINBUN_SUCCES, "SinBunReader|TestCardScan", "인식정보 =>" + " [카드타입]" + CardInfo.GetCardTypeText(cdInfo.CardType) + "(" + ((int)cdInfo.CardType).ToString() + ")" + " [주민번호]" + cdInfo.JuminNum + " [이름]" + cdInfo.Name + "[발행일자]" + cdInfo.YYMM + " [장애등급]" + cdInfo.Grade + " [보훈카드]" + cdInfo.BohunNum + " [카드번호]" + cdInfo.CardNum);
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.SINBUN_FAIL, "SinBunReader|TestCardScan", "인식실패");
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|TestCardScan", ex.ToString());
            }
            finally
            {
                DisConnect();
            }
        }

        public int TestReadState()
        {
            int result = -1;
            try
            {
                if (Connect())
                {
                    StatusInfo stInfo = new StatusInfo();
                    TStatusInfo tstInfo = new TStatusInfo();
                    result = SR_ReadStatusInfo(ref tstInfo);
                    stInfo.StatusResult = result;
                    PasingStatesData(tstInfo, ref stInfo);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "SinBunReader|TestReadState", "인식기정보 =>" + " [인식상태]" + StatusInfo.GetCardOnStatesText(stInfo.CardOn) + "(" + ((int)stInfo.CardOn).ToString() + ")" + " [진행단계]" + StatusInfo.GetCardPhaseText(stInfo.Phase) + "(" + ((int)stInfo.Phase).ToString() + ")");

                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|TestReadState", ex.ToString());
                return result;
            }
            finally
            {
                DisConnect();
            }
        }

        public void PasingCardInfoData(TIDCardInfo tcdInfo, ref CardInfo cdInfo)
        {
            try
            {
                cdInfo.CardType = CardInfo.GetCardType(Int32.Parse(tcdInfo.sCardType));
                cdInfo.JuminNum = tcdInfo.sJuminNum;
                cdInfo.Name = tcdInfo.sName;
                cdInfo.YYMM = tcdInfo.sYYMM;
                cdInfo.Grade = tcdInfo.sGrade;
                cdInfo.BohunNum = tcdInfo.sBohunNum;
                cdInfo.CardNum = tcdInfo.sCardNum;
                if (cdInfo.CardType == CardType.BokJi || cdInfo.CardType == CardType.NewBokJi || cdInfo.CardType == CardType.OldBokJi)
                {
                    cdInfo.DiscountType = DiscountType.BokJi;
                }
                else if (cdInfo.CardType == CardType.BoHun || cdInfo.CardType == CardType.NewBoHun)
                {
                    cdInfo.DiscountType = DiscountType.BoHun;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SinBunReader|PasingCardInfoData", ex.ToString());
            }
        }

        public void PasingStatesData(TStatusInfo tstInfo, ref StatusInfo stInfo)
        {
            stInfo.CardOn = StatusInfo.GetCardOnStates(tstInfo.sCardOn);
            stInfo.Phase = StatusInfo.GetCardPhase(tstInfo.sPhase);
        }

        int cnt = 0;
        public void TestDataSend()
        {
            CardInfo info = new CardInfo();
            info.CardType = CardType.BoHun;
            info.DiscountType = DiscountType.BoHun;
            info.Grade = "3";

            cnt++;
            readEvent(info);
        }
    }
}
