using FadeFox.Text;
using System;

namespace NPCommon.Van
{
    public class SmartroVCat
    {
        //리얼IP:127.0.0.1
        //리얼포트:13855
        public enum voiceType
        {
            None,
            CardInsert,
            CardFrontEjuct,
            FullPay
        }
        private int mStartSoundTick = 0;
        public int StartSoundTick
        {
            set { mStartSoundTick = value; }
            get { return mStartSoundTick; }
        }
        private voiceType mCurrentVoiceType = voiceType.None;
        public voiceType CurrentVoiceType
        {
            set { mCurrentVoiceType = value; }
            get { return mCurrentVoiceType; }
        }


        /// <summary>
        /// OnRcvState온 2자리 상태값을 한글로 변경
        /// </summary>
        /// <param name="pStatus"></param>
        /// <returns></returns>
        public string OnReceiveStatusMessage(string pStatus)
        {
            string statusMessage = string.Empty;
            switch (pStatus)
            {
                case "1I":
                    //  currentCardReaderStatus = CardReaderStatus.CardReady;
                    statusMessage = "신용카드 결제시에는 카드를 신용카드 투입구에 삽입후 잠시 기다려주세요";
                    break;
                case "AU":
                    //  currentCardReaderStatus = CardReaderStatus.CardReady;
                    statusMessage = "신용카드 결제시에는 카드를 신용카드 투입구에 삽입후 잠시 기다려주세요";
                    break;

                case "1M":
                    //    currentCardReaderStatus = CardReaderStatus.CardReady;
                    statusMessage = "신용-MS카드대기";
                    break;
                case "EC":
                    statusMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                    break;
                case "1P":
                    statusMessage = "신용-PIN대기";
                    break;
                case "1S":
                    statusMessage = "신용-사인대기";
                    break;
                case "1T":
                    statusMessage = "신용-터치사인대기";
                    break;
                case "2D":
                    statusMessage = "현금영수증-화면패드만대기";
                    break;
                case "2C":
                    statusMessage = "현금영수증-M/S카드번호 대기(화면패드 대기 포함)";
                    break;
                case "2P":
                    statusMessage = "현금영수증-M/S카드번호 대기(화면패드 대기 포함)";
                    break;
                case "2M":
                    statusMessage = "현금영수증-M/S카드번호 및 서명패드번호입력 대기";
                    break;
                case "3C":
                    statusMessage = "포인트-M/S카드 대기";
                    break;
                case "3P":
                    statusMessage = "포인트-비밀번호입력";
                    break;
                case "CK":
                    // statusMessage = "카드 입력대기종료"; // 신용카드 결제 요청시 '1I'가오고 카드를 정상/비정상 어느방향으로 꽃아도 'CK'가온다
                    statusMessage = "카드를 뽑지마시고 기다려주세요.";
                    //     currentCardReaderStatus = CardReaderStatus.CardReadyEnd;
                    break;
                case "PK":
                    statusMessage = "핀입력대기 정상종료";
                    break;
                case "SK":
                    statusMessage = "사인입력대기 정상종료";
                    break;
                case "NK":
                    statusMessage = "번호입력대기 정상종료";
                    break;
                case "TS":
                    //     statusMessage = "VAN서버 통신시작";
                    statusMessage = "카드를 뽑지마시고 기다려주세요.";
                    break;
                case "TE":
                    //       statusMessage = "VAN서버 통신종료";
                    statusMessage = "결제가 진행중입니다.";
                    break;

            }
            return statusMessage;
        }
        private string mVCatIp = string.Empty;
        public string VCatIp
        {
            set { mVCatIp = value; }
            get { return mVCatIp; }
        }
        private int mVCatPort = 0;
        public int VCatPort
        {
            set { mVCatPort = value; }
            get { return mVCatPort; }
        }
        /// <summary>
        /// 거래승인요청
        /// </summary>
        private const string mCmdQApproval = "0101";
        /// <summary>
        /// 거래승인취소요청
        /// </summary>
        private const string mCmdQApprovalCancle = "2101";
        /// <summary>
        /// VCat정보요청
        /// </summary>
        private const string mCmdQVCatGetInfo = "5201";
        /// <summary>
        /// VCat승인환경설정요청
        /// </summary>
        private const string mCmdQVCatSetting = "0910";
        /// <summary>
        /// 연결확인 요청
        /// </summary>
        private const string mCmdQLineCheck = "9001";
        /// <summary>
        /// 거래요청 초기화
        /// </summary>
        private const string mCmdQApprovalInitialize = "9901";
        /// <summary>
        /// 거래승인 응답
        /// </summary>
        private const string mCmdRApproval = "0102";
        /// <summary>
        /// 거래승인취소요청 응답
        /// </summary>
        private const string mCmdRApprovalCancle = "2102";
        /// <summary>
        /// VCat정보요청 응답
        /// </summary>
        private const string mCmdRVCatGetInfo = "5202";
        /// <summary>
        /// VCat승인환경설정요청 응답
        /// </summary>
        private const string mCmdRVCatSetting = "0911";
        /// <summary>
        /// 연결확인 응답
        /// </summary>
        private const string mCmdRLineCheck = "9002";
        /// <summary>
        /// 거래초기화 응답
        /// </summary>
        private const string mCmdRApprovalInitialize = "9902";
        /// <summary>
        /// 거래구분 신용승인
        /// </summary>
        private const string mApprovalTypeOfCard = "01";

        /// <summary>
        /// 거래구분 신용승인
        /// </summary>
        private const string mApprovalTypeOfCash = "02";

        // public enum 
        public enum VanReq
        {
            coReqServiceInx1 = 1  // 각 서비스 유형별 코드 "0101" 거래승인
                                  //                       "2101" 거래승인취소
                                  //                       "0201" 거래승인+보너스 누적승인
                                  //                       "2201" 거래취소+보너스 누적취소
                                  //                       "0301" 보너스 조회
                                  //                       "0303" 보너스 이용
                                  //                       "0305" 보너스 누적
                                  //                       "0401" 수표조회
                                  //                       "0910" VCat승인환경설정
                                  //                       "9001" 연결확인
           ,
            coReqTradeInx2 = 2  // 거래구분코드 "01"=신용, "02"=현금, "03"=은련,  보너스이용="01", 보너스누적="02"
                ,
            coReqAmountInx3 = 3  // 승인요청금액(원거래금액+세금+봉사료)
                ,
            coReqTaxInx4 = 4  // 세금
                ,
            coReqBongSaInx5 = 5  // 봉사료
                ,
            coReqHalbuInx6 = 6  // 현금거래=현금승인유형(거래자구분(1)+확인자구분(2), 현금이외거래=할부개월
                                //                       거래자구분 0=소비자,1=사업자
                                //                       확인자구분 0=신용카드, 1=주민등록번호, 2=사업자번호, 3=기타(핸폰), 4=보너스카드번호
                ,
            coReqPassInx7 = 7  // 비밀번호 암호화여부(1) + 비밀번호(V16)
                               // 비밀번호 V32
                ,
            coReqGoodsInx8 = 8  // 전표에 인쇄할 품목명
                ,
            coReqSignYNInx9 = 9  // 서명여부 1=비서명, 2=서명, 3=단말기설정
                ,
            coReqFiller1Inx10 = 10 // 예비필드1
                ,
            coReqFiller2Inx11 = 11 // 예비필드2
                ,
            coReqAppNoInx12 = 12 // 승인번호 (취소일때만 사용)
                ,
            coReqAppDateInx13 = 13 // 원거래일자 (YYYYMMDD) (취소일때만 사용)
                ,
            coReqCancelGBInx14 = 14 // 현금취소구분 1=일반, 2=오류, 3=기티 (현금취소일때만 사용)
                ,
            coReqBonusGBInx15 = 15 // 보너스구분자
                ,
            coReqBonusUseGBInx16 = 16 // 보너스 사용구분
                ,
            coReqBonusAppNoInx17 = 17 // 보너스 승인번호
                ,
            coReqMasterKeyInx18 = 18 // MasterKey Index (2)
                , coReqWorkingKeyInx19 = 19 // WorkingKey      (16)
        }

        public enum vanRespoen
        {
            coResServiceInx1 = 1  // 각 서비스 유형별 코드 "0102" 거래승인
                                  //                       "2102" 거래승인취소
                                  //                       "0202" 거래승인+보너스 누적승인
                                  //                       "2202" 거래취소+보너스 누적취소
                                  //                       "0302" 보너스 조회
                                  //                       "0304" 보너스 이용
                                  //                       "0306" 보너스 누적
                                  //                       "0402" 수표조회
                                  //                       "5202" VCat정보요청 응답
                                  //                       "0911" VCat승인환경설정 응답
                                  //                       "9002" 연결확인웅덥
           ,
            coResTradeInx2 = 2  // 거래구분코드 "01"=신용, "02"=현금, "03"=은련,  보너스이용="01", 보너스누적="02"
                ,
            coResCardNumInx3 = 3  // 카드번호 16자리
                ,
            coResAmountInx4 = 4  // 승인요청금액(원거래금액+세금+봉사료)
                ,
            coResTaxInx5 = 5  // 세금
                ,
            coResBongSaInx6 = 6  // 봉사료
                ,
            coResHalbuInx7 = 7  // 현금거래=현금승인유형(거래자구분(1)+확인자구분(2), 현금이외거래=할부개월
                                //                       거래자구분 0=소비자,1=사업자
                                //                       확인자구분 0=신용카드, 1=주민등록번호, 2=사업자번호, 3=기타(핸폰), 4=보너스카드번호

                ,
            coResAppNoInx8 = 8  // 승인번호
                ,
            coResAppDateInx9 = 9  // 매출이 발생한 일자 (YYYYMMDD)
                ,
            coResAppTimeInx10 = 10 // 매출이 발생한 시간 (hhmmss)
                ,
            coResUniqueNoInx11 = 11 // 거래고유번호
                ,
            coResStoreNoInx12 = 12 // 가맹점번호
                ,
            coResTermNoInx13 = 13 // 단말기번호
                ,
            coResBalNoInx14 = 14 // 발급사코드(4) + 발급사명(16)
                ,
            coResMaeipInx15 = 15 // 매입사코드(4) + 매입사명(16)
                ,
            coResDDCcodeInx16 = 16 // DDC유무
                ,
            coResResultCodeInx17 = 17 // 응답코드
                ,
            coResDisMsgInx18 = 18 // 화면메세지
                ,
            coResTitleInx19 = 19 // 전표 타이틀
                ,
            coResOutMsgInt20 = 20 // 출력메세지
                ,
            coResMasterKeyInx21 = 21 // MasterKey Index (2)
                ,
            coResWorkingKeyInx22 = 22 // WorkingKey      (16)
                ,
            coResSignDataInx23 = 23 // 서명이미지
                ,
            coResSignInfoInx24 = 24 // 서명이미지 정보

                ,
            coResFiller1Inx25 = 25 // Filler1
                ,
            coResFiller2Inx26 = 26 // Filler2
                ,
            coResBonusNumInx27 = 27 // 보너스카드 번호
                ,
            coResBonusResultCodeInx28 = 28 // 보너스 응답코드
                ,
            coResBonusAppNoInx29 = 29 // 보너스 승인번호
                ,
            coResBonusBalInx30 = 30 // 보너스 발급사 코드(4) + 발급사명(16)
                ,
            coResBonusMaeipInx31 = 31 // 보너스 매입사 코드(4) + 발급사명(16)
                ,
            coResCreatePointInx32 = 32 // 보너스 발생점수
                ,
            coResUsePointInx33 = 33 // 보너스 가용점수
                ,
            coResAddPointInx34 = 34 // 보너스 누적점수
                ,
            coResBonusOutMsgInx35 = 35 // 보너스 출력메세지
            ,
            coResBeforePointInx36 = 36 // 보너스 발생전점수
                ,
            coResBusinessNumInx40 = 40 //	사업자번호	AN	10	가맹점 사업자 정보
                ,
            coResStoreNameInx41 = 41 //	가맹점 명	ANS	V30	가맹점 상호
                ,
            coResOwnerNameInx42 = 42 //	대표자 명	ANS	V12	가맹점 대표자 성명
                ,
            coResStoreTelInx43 = 43	//  가맹점 전화	ANS	V16	가맹점 전화번호
                ,
            coResStoreAddrInx44 = 44	// 가맹점 주소	ANS	V50	가맹점 주소
                , coResVCatVerInx45 = 45 //	단말기 버전	ANS	V15	VCAT버전 정보
        }
        public enum vanErrorCode
        {
            coRET_APPROVAL_OK = 1    // 정상승인
           ,
            coRET_APPROVAL_NOT = -1   // 승인오류
                ,
            coRET_MAKE_ERR = -3   // 송신 DATA 전문구성실패
                ,
            coRET_DATA_ERR = -40  // IP입력값 실패
                ,
            coRET_SERVER_CONNECT_ERR = -30  // 서버 연결 실패
                ,
            coRET_SERVER_SND_ERR = -31  // DATA전송 실패
                ,
            coRET_SERVER_NAK_ERR = -10  // NAK수신
                ,
            coRET_SERVER_FF_ERR = -11  // FF수신
                ,
            coRET_SERVER_ETX_ERR = -12  // ETX Check Fail
                ,
            coRET_SERVER_STX_ERR = -13  // STX Check Fail
                ,
            coRET_SERVER_TIMEOUT_ERR = -9   // TimerOut
                ,
            coRET_SERVER_UNKNOWN_ERR = -99  // 정의되지 않은 오류
                ,
            coRET_REVDATA_ERR = -98  // 서버응답값 오류
                ,
            coRET_REVINDEX_ERR = -97  // 전문수신 INDEX 오류
                ,
            coRET_MEMORY_ERR = -96  // 메모리 호출 오류
                ,
            coRET_DEVICESET_ERR = -95  // 단말기 COM, TCP 선택오류
                , coRET_DATAIN_ERR = -94  // 전문데이터 입력 오류
        }

        public const string coRET_MAKE_ERR_STR = "송신 DATA 전문구성실패";
        public const string coRET_DATA_ERR_STR = "IP입력값 실패";
        public const string coRET_SERVER_CONNECT_ERR_STR = "서버 연결 실패";
        public const string coRET_SERVER_SND_ERR_STR = "DATA전송 실패";
        public const string coRET_SERVER_NAK_ERR_STR = "NAK수신";
        public const string coRET_SERVER_FF_ERR_STR = "FF수신";
        public const string coRET_SERVER_ETX_ERR_STR = "ETX Check Fail";
        public const string coRET_SERVER_STX_ERR_STR = "STX Check Fail";
        public const string coRET_SERVER_TIMEOUT_ERR_STR = "TimerOut";
        public const string coRET_SERVER_UNKNOWN_ERR_STR = "정의되지 않은 오류";
        public const string coRET_REVDATA_ERR_STR = "서버응답값 오류";
        public const string coRET_REVINDEX_ERR_STR = "전문수신 INDEX 오류";
        public const string coRET_MEMORY_ERR_STR = "메모리 호출 오류";
        public const string coRET_DEVICESET_ERR_STR = "단말기 COM, TCP 선택오류";
        public const string coRET_DATAIN_ERR_STR = "전문데이터 입력 오류";

        private bool SetSendData(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int AInx, string AValue, ref string pErrorMessage)
        {
            bool result = false;
            pErrorMessage = string.Empty;
            try
            {
                if (AValue.Length > 0)
                {

                    if (SmtSndRevOcx.SetTRData(Convert.ToInt16(AInx), AValue) == 1)
                    {
                        pErrorMessage = "정상";
                        result = true;

                    }
                    else
                    {
                        switch (AInx)
                        {
                            case (int)VanReq.coReqServiceInx1:
                                pErrorMessage = "서비스 유형별 코드 입력오류";
                                break;
                            case (int)VanReq.coReqTradeInx2:
                                pErrorMessage = "거래구분코드 입력오류";
                                break;
                            case (int)VanReq.coReqAmountInx3:
                                pErrorMessage = "승인요청금액 입력오류";
                                break;
                            case (int)VanReq.coReqTaxInx4:
                                pErrorMessage = "세금 입력오류";
                                break;
                            case (int)VanReq.coReqBongSaInx5:
                                pErrorMessage = "봉사료 입력오류";
                                break;
                            case (int)VanReq.coReqHalbuInx6:
                                pErrorMessage = "현금승인유형 입력오류";
                                break;
                            case (int)VanReq.coReqPassInx7:
                                pErrorMessage = "비밀번호 입력오류";
                                break;
                            case (int)VanReq.coReqGoodsInx8:
                                pErrorMessage = "전표에 인쇄할 품목명 입력오류";
                                break;
                            case (int)VanReq.coReqSignYNInx9:
                                pErrorMessage = "서명여부 입력오류";
                                break;
                            case (int)VanReq.coReqFiller1Inx10:
                                pErrorMessage = "예비필드1 입력오류";
                                break;
                            case (int)VanReq.coReqFiller2Inx11:
                                pErrorMessage = "예비필드2 입력오류";
                                break;
                            case (int)VanReq.coReqAppNoInx12:
                                pErrorMessage = "승인번호 입력오류";
                                break;
                            case (int)VanReq.coReqAppDateInx13:
                                pErrorMessage = "원거래일자 입력오류";
                                break;
                            case (int)VanReq.coReqCancelGBInx14:
                                pErrorMessage = "현금취소구분 입력오류";
                                break;
                            case (int)VanReq.coReqBonusGBInx15:
                                pErrorMessage = "보너스구분자 입력오류";
                                break;
                            case (int)VanReq.coReqBonusUseGBInx16:
                                pErrorMessage = "보너스 사용구분 입력오류";
                                break;
                            case (int)VanReq.coReqBonusAppNoInx17:
                                pErrorMessage = "보너스 승인번호 입력오류";
                                break;
                            case (int)VanReq.coReqMasterKeyInx18:
                                pErrorMessage = "MasterKey Index 입력오류";
                                break;
                            case (int)VanReq.coReqWorkingKeyInx19:
                                pErrorMessage = "WorkingKey 입력오류";
                                break;
                            default:
                                pErrorMessage = "정의 되지 않은 입력오류";
                                break;
                        }

                        result = false;
                    }



                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                pErrorMessage = ex.ToString();
                return result;
            }
        }


        public bool CardApproval(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int pAmount, int pWatiTime, ref string pErrorMessage)
        {
            SmtSndRevOcx.InitData();
            pErrorMessage = string.Empty;
            int confirmoney = pAmount;

            double taxAmualte = ((long)(Math.Floor((decimal)(confirmoney / 1.1))) / 10) * 10;
            double taxsResult = confirmoney - taxAmualte;  // 세금
            double SupplyPay = confirmoney - Convert.ToInt32(taxsResult); //공급가
            int tax = Convert.ToInt32(taxsResult);
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQApproval, ref pErrorMessage)) // 이용
            {

                return false;

            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqTradeInx2, mApprovalTypeOfCard, ref pErrorMessage)) //   신용카드
            {
                return false;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqAmountInx3, confirmoney.ToString().PadLeft(10, '0'), ref pErrorMessage))     // 승인요청금액(원거래금액+세금+봉사료)
            {
                return false;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqTaxInx4, tax.ToString().PadLeft(8, '0'), ref pErrorMessage))    // 세금
            {
                return false;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqBongSaInx5, string.Empty.PadLeft(8, '0'), ref pErrorMessage))  // 봉사료
            {
                return false;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqHalbuInx6, string.Empty.ToString().PadLeft(2, '0'), ref pErrorMessage))  // 할부개월
            {
                return false;
            }


            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqSignYNInx9, "1", ref pErrorMessage))                    // 서명필드 1비서명,2서명
            {
                return false;
            }
            bool isSendSuccess = SendAsyncPosData(SmtSndRevOcx, pWatiTime);
            if (isSendSuccess == false)
            {
                pErrorMessage = "전송오류";
                return false;
            }
            return true;


        }

        //if Not SetSendData(coReqMasterKeyInx, FWorkIngIndex, oErrMsg) then Exit;                 // MasterKey Index (2)
        //if Not SetSendData(coReqWorkingKeyInx,FWorkIngKey, oErrMsg) then Exit;                   // WorkingKey      (16)

        private bool SendAsyncPosData(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int pWaitTIme)
        {
            int resultPosToVacat = 0;
            string errormessage = string.Empty;
            try
            {
                resultPosToVacat = SmtSndRevOcx.SendToVCat(VCatIp, VCatPort, pWaitTIme);
                if (resultPosToVacat > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //if (resultPosToVacat > 0)
                //{
                //    FRevInx = resultPosToVacat;
                //    ReceiveData(SmtSndRevOcx, FRevInx, ref pSmartroData);

                //    if (pSmartroData.ReceiveReturnCode == "00")
                //    {
                //        pSmartroData.Success = true;
                //    }
                //    else
                //    {
                //        pSmartroData.Success = false;
                //        pSmartroData.ErrorMessage = pSmartroData.ReceiveDisplayMsg;

                //    }
                //}
                //else
                //{
                //    switch (resultPosToVacat)
                //    {

                //        case (int)vanErrorCode.coRET_MAKE_ERR:
                //            errormessage = coRET_MAKE_ERR_STR;             // 송신 DATA 전문구성실패
                //            break;
                //        case (int)vanErrorCode.coRET_DATA_ERR: errormessage = coRET_DATA_ERR_STR;             // IP입력값 실패
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_CONNECT_ERR: errormessage = coRET_SERVER_CONNECT_ERR_STR;   // 서버 연결 실패
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_SND_ERR: errormessage = coRET_SERVER_SND_ERR_STR;       // DATA전송 실패
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_NAK_ERR: errormessage = coRET_SERVER_NAK_ERR_STR;       // NAK수신
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_FF_ERR: errormessage = coRET_SERVER_FF_ERR_STR;        // FF수신
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_ETX_ERR: errormessage = coRET_SERVER_ETX_ERR_STR;       // ETX Check Fail
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_STX_ERR: errormessage = coRET_SERVER_STX_ERR_STR;       // STX Check Fail
                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_TIMEOUT_ERR: errormessage = coRET_SERVER_TIMEOUT_ERR_STR;   // TimerOut

                //            break;
                //        case (int)vanErrorCode.coRET_SERVER_UNKNOWN_ERR: errormessage = coRET_SERVER_UNKNOWN_ERR_STR;   // 정의되지 않은 오류
                //            break;
                //        default:
                //            errormessage = coRET_SERVER_UNKNOWN_ERR_STR;   // 정의되지 않은 오류
                //            break;
                //    }
                //}
            }

            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | SendAsyncPosData", ex.ToString());
                return false;
                //result = 0;
                //oErrMsg := 'TCardLinkComm DEVICE 설정오류';
                //LogWrite('Comm', oErrMsg);
            }

        }

        private bool SendSyncPosData(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int pWaitTIme, ref SmatroData pSmartroData)
        {
            int resultPosToVacat = 0;
            int FRevInx = 0;
            string errormessage = string.Empty;
            try
            {
                resultPosToVacat = SmtSndRevOcx.PosToVCATSndRcv(VCatIp, VCatPort, pWaitTIme);
                if (resultPosToVacat > 0)
                {
                    FRevInx = resultPosToVacat;
                    ReceiveData(SmtSndRevOcx, FRevInx, ref pSmartroData);

                    if (pSmartroData.ReceiveReturnCode == "00")
                    {
                        pSmartroData.Success = true;
                        return true;
                    }
                    else
                    {
                        pSmartroData.Success = false;
                        pSmartroData.ErrorMessage = pSmartroData.ReceiveDisplayMsg;
                        return false;

                    }
                }
                else
                {
                    pSmartroData.ReceiveReturnCode = resultPosToVacat.ToString(); ;
                    switch (resultPosToVacat)
                    {

                        case (int)vanErrorCode.coRET_MAKE_ERR:
                            errormessage = coRET_MAKE_ERR_STR;             // 송신 DATA 전문구성실패
                            break;
                        case (int)vanErrorCode.coRET_DATA_ERR:
                            errormessage = coRET_DATA_ERR_STR;             // IP입력값 실패
                            break;
                        case (int)vanErrorCode.coRET_SERVER_CONNECT_ERR:
                            errormessage = coRET_SERVER_CONNECT_ERR_STR;   // 서버 연결 실패
                            break;
                        case (int)vanErrorCode.coRET_SERVER_SND_ERR:
                            errormessage = coRET_SERVER_SND_ERR_STR;       // DATA전송 실패
                            break;
                        case (int)vanErrorCode.coRET_SERVER_NAK_ERR:
                            errormessage = coRET_SERVER_NAK_ERR_STR;       // NAK수신
                            break;
                        case (int)vanErrorCode.coRET_SERVER_FF_ERR:
                            errormessage = coRET_SERVER_FF_ERR_STR;        // FF수신
                            break;
                        case (int)vanErrorCode.coRET_SERVER_ETX_ERR:
                            errormessage = coRET_SERVER_ETX_ERR_STR;       // ETX Check Fail
                            break;
                        case (int)vanErrorCode.coRET_SERVER_STX_ERR:
                            errormessage = coRET_SERVER_STX_ERR_STR;       // STX Check Fail
                            break;
                        case (int)vanErrorCode.coRET_SERVER_TIMEOUT_ERR:
                            errormessage = coRET_SERVER_TIMEOUT_ERR_STR;   // TimerOut

                            break;
                        case (int)vanErrorCode.coRET_SERVER_UNKNOWN_ERR:
                            errormessage = coRET_SERVER_UNKNOWN_ERR_STR;   // 정의되지 않은 오류
                            break;
                        default:
                            errormessage = coRET_SERVER_UNKNOWN_ERR_STR;   // 정의되지 않은 오류
                            break;
                    }
                    pSmartroData.ReceiveReturnMessage = errormessage;
                    return false;
                }
            }

            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | SendSyncPosData", ex.ToString());
                return false;
                //result = 0;
                //oErrMsg := 'TCardLinkComm DEVICE 설정오류';
                //LogWrite('Comm', oErrMsg);
            }

        }


        public SmatroData ReceiveData(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int FRevInx, ref SmatroData pSmartroData)
        {
            string sErrMsg = string.Empty;
            string receiveData = string.Empty;
            string type = string.Empty;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResServiceInx1, ref receiveData))// 응답코드
                type = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResTradeInx2, ref receiveData))// 응답코드
            {
                switch (type)
                {
                    case mCmdRApproval:
                        if (receiveData == mApprovalTypeOfCard)
                        {
                            pSmartroData.CurrentCmdType = SmatroData.CMDType.CardApprovalRespone;
                        }
                        else if (receiveData == mApprovalTypeOfCash)
                        {
                            pSmartroData.CurrentCmdType = SmatroData.CMDType.CashApprovalRespone;
                        }
                        break;
                    case mCmdRApprovalCancle:
                        if (receiveData == mApprovalTypeOfCard)
                        {
                            pSmartroData.CurrentCmdType = SmatroData.CMDType.CardApprovalCancleRespone;
                        }
                        else if (receiveData == mApprovalTypeOfCash)
                        {
                            pSmartroData.CurrentCmdType = SmatroData.CMDType.CashApprovalCancleRespone;
                        }
                        break;
                    case mCmdRLineCheck:
                        pSmartroData.CurrentCmdType = SmatroData.CMDType.LineCheckRespone;
                        break;
                    case mCmdRVCatGetInfo:
                        pSmartroData.CurrentCmdType = SmatroData.CMDType.DeviceInfoRespone;
                        break;
                    case mCmdRVCatSetting:
                        pSmartroData.CurrentCmdType = SmatroData.CMDType.VCatSetInfoRespone;
                        break;
                    case mCmdRApprovalInitialize:
                        pSmartroData.CurrentCmdType = SmatroData.CMDType.ApprovalInitializeRespone;
                        break;
                    default:
                        pSmartroData.CurrentCmdType = SmatroData.CMDType.None;
                        break;
                }
            }

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResResultCodeInx17, ref receiveData))// 응답코드
            {
                pSmartroData.ReceiveReturnCode = receiveData;
            }

            if (pSmartroData.ReceiveReturnCode == "00")
            {
                pSmartroData.Success = true;
            }



            if (pSmartroData.CurrentCmdType == SmatroData.CMDType.DeviceInfoRespone)
            {

                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResBusinessNumInx40, ref receiveData))//	사업자번호	AN	10	가맹점 사업자 정보
                {
                    pSmartroData.DeviceBusinessNum = receiveData;
                }
                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResStoreNameInx41, ref receiveData))//	가맹점 명	ANS	V30	가맹점 상호
                {
                    pSmartroData.DeviceStoreName = receiveData;
                }
                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResOwnerNameInx42, ref receiveData))//	대표자 명	ANS	V12	가맹점 대표자 성명
                {
                    pSmartroData.DeviceOwnerName = receiveData;
                }

                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResStoreTelInx43, ref receiveData))//  가맹점 전화	ANS	V16	가맹점 전화번호
                {
                    pSmartroData.DeviceStoreTel = receiveData;
                }
                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResStoreAddrInx44, ref receiveData))// 가맹점 주소	ANS	V50	가맹점 주소
                {
                    pSmartroData.DeviceStoreAddr = receiveData;
                }
                if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResVCatVerInx45, ref receiveData)) //	단말기 버전	ANS	V15	VCAT버전 정보
                {
                    pSmartroData.DeviceVCatVer = receiveData;
                }
                if (!string.IsNullOrEmpty(pSmartroData.DeviceBusinessNum.Trim()))
                {
                    pSmartroData.ReceiveReturnCode = "00";
                    pSmartroData.Success = true;
                }

            }

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResCardNumInx3, ref receiveData)) // 카드번호 16자리
                pSmartroData.ReceiveCardNumber = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResAmountInx4, ref receiveData))  // 승인요청금액(원거래금액+세금+봉사료)
                pSmartroData.ReceiveCardAmt = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResTaxInx5, ref receiveData))     // 세금
                pSmartroData.ReceiveTaxAmt = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResBongSaInx6, ref receiveData))  // 봉사료
                pSmartroData.ReceiveBongSaInx = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResHalbuInx7, ref receiveData))   // 현금거래=현금승인유형(거래자구분(1)+확인자구분(2), 현금이외거래=할부개월
                pSmartroData.ReceiveHalbu = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResAppNoInx8, ref receiveData))  // 승인번호
                pSmartroData.RecieveApprovalNumber = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResAppDateInx9, ref receiveData))  // 매출이 발생한 일자 (YYYYMMDD)
                pSmartroData.ReceiveAppYmd = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResAppTimeInx10, ref receiveData)) // 매출이 발생한 시간 (hhmmss)
                pSmartroData.ReceiveAppHms = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResUniqueNoInx11, ref receiveData)) // 거래고유번호
                pSmartroData.ReceiveUniqueNo = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResStoreNoInx12, ref receiveData)) // 가맹점번호
                pSmartroData.ReceiveStoreNo = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResTermNoInx13, ref receiveData))// 단말기번호
                pSmartroData.ReceiveTermNo = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResBalNoInx14, ref receiveData))// 발급사코드(4) + 발급사명(16)
            {
                if (receiveData.Length >= 5)
                {
                    pSmartroData.ReceiveBalgubCode = receiveData.Substring(0, 4);
                    pSmartroData.ReceiveBalgubName = receiveData.Substring(4);
                }
            }

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResMaeipInx15, ref receiveData)) // 매입사코드(4) + 매입사명(16)
            {
                if (receiveData.Length >= 5)
                {
                    pSmartroData.ReceiveMaipCode = receiveData.Substring(0, 4);
                    pSmartroData.ReceiveMaipName = receiveData.Substring(4);
                }
            }

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResDDCcodeInx16, ref receiveData))// DDC유무
            {
                switch (receiveData)
                {
                    case "D":
                        pSmartroData.DdcType = SmatroData.DDCTYPE.DDC;
                        break;
                    case "E":
                        pSmartroData.DdcType = SmatroData.DDCTYPE.EDI;
                        break;
                    case "C":
                        pSmartroData.DdcType = SmatroData.DDCTYPE.EDC;
                        break;
                    case "S":
                        pSmartroData.DdcType = SmatroData.DDCTYPE.DESC_ESC;
                        break;

                }

            }

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResDisMsgInx18, ref receiveData)) // 화면메세지
                pSmartroData.ReceiveDisplayMsg = receiveData;

            //if (GetRevData((int)vanRespoen.coResTitleInx19,ref receiveData))// 전표 타이틀
            //   FRevTitle := sValue;

            //if (GetRevData((int)vanRespoen.coResOutMsgInt20,ref receiveData)) // 출력메세지
            //   FPrtMsg := sValue;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResMasterKeyInx21, ref receiveData))// MasterKey Index (2)
                pSmartroData.ReceiveMasterKey = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResWorkingKeyInx22, ref receiveData)) // WorkingKey      (16)
                pSmartroData.ReceiveWorkKey = receiveData;

            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResFiller1Inx25, ref receiveData)) // Filler1
                pSmartroData.ReceiveFIlter1 = receiveData;
            if (GetRevData(SmtSndRevOcx, FRevInx, (int)vanRespoen.coResFiller2Inx26, ref receiveData)) // Filler2
                pSmartroData.ReceiveFIlter2 = receiveData;
            return pSmartroData;
        }


        private bool GetRevData(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, int pFRevInx, int pCmdIndex, ref string pReturnData)
        {
            pReturnData = string.Empty;
            try
            {
                pReturnData = SmtSndRevOcx.GetResDataCount(1, Convert.ToInt16(pCmdIndex));
                return true;
            }
            catch (Exception ex)
            {
                pReturnData = ex.ToString();
                return false;
            }
        }

        public SmatroData LineCheck(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx)
        {
            string errorMessage = string.Empty;
            SmatroData smartroData = new SmatroData();
            try
            {
                SmtSndRevOcx.InitData();
                if (SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQLineCheck, ref errorMessage))
                {
                    SendSyncPosData(SmtSndRevOcx, 3, ref smartroData);
                    return smartroData;
                }

                return smartroData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | LineCheck", ex.ToString());
                return smartroData;
            }

        }

        public SmatroData VCatSetting(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx, string pComPort, string pBardRate)
        {
            string errorMessage = string.Empty;
            SmatroData smartroData = new SmatroData();

            SmtSndRevOcx.InitData();


            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQVCatSetting, ref errorMessage)) // 이용
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }

            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQVCatSetting, ref errorMessage)) // 이용
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, 61, pComPort, ref errorMessage)) // 포트
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, 62, pBardRate, ref errorMessage)) // 바트레이트
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, 63, "9", ref errorMessage)) // 사인패드사용안함
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, 66, "1", ref errorMessage)) // 연동승이거래
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, 67, "1", ref errorMessage)) // 화면숨김
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqFiller1Inx10, " ", ref errorMessage)) // 필터
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }
            if (!SetSendData(SmtSndRevOcx, (int)VanReq.coReqFiller2Inx11, " ", ref errorMessage)) // 필터
            {
                smartroData.ErrorMessage = errorMessage;
                smartroData.IsSendSuccess = false;
                return smartroData;
            }

            SendSyncPosData(SmtSndRevOcx, 3, ref smartroData);
            return smartroData;
        }

        public SmatroData DeviceInfo(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx)
        {
            string errorMessage = string.Empty;
            SmatroData smartroData = new SmatroData();
            try
            {
                SmtSndRevOcx.InitData();
                if (SetSendData(SmtSndRevOcx, 1, mCmdQVCatGetInfo, ref errorMessage))
                {
                    SendSyncPosData(SmtSndRevOcx, 3, ref smartroData);
                    return smartroData;
                }

                return smartroData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | DeviceInfo", ex.ToString());
                return smartroData;
            }

        }
        /// <summary>
        /// 거래요청초기화
        /// </summary>
        /// <param name="SmtSndRevOcx"></param>
        /// <returns></returns>
        public SmatroData DeviceReInitialLize(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx)
        {

            string errorMessage = string.Empty;
            SmatroData smartroData = new SmatroData();
            try
            {
                SmtSndRevOcx.InitData();
                if (SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQApprovalInitialize, ref errorMessage))
                {
                    SendAsyncPosData(SmtSndRevOcx, 3);
                    return smartroData;
                }

                return smartroData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | DeviceReInitialLize", ex.ToString());
                return smartroData;
            }
        }
        /// <summary>
        /// 싱크거래요청초기화
        /// </summary>
        /// <param name="SmtSndRevOcx"></param>
        /// <returns></returns>
        public SmatroData DeviceReInitialLizeSync(AxSmtSndRcvVCATLib.AxSmtSndRcvVCAT SmtSndRevOcx)
        {

            string errorMessage = string.Empty;
            SmatroData smartroData = new SmatroData();
            try
            {
                SmtSndRevOcx.InitData();
                if (SetSendData(SmtSndRevOcx, (int)VanReq.coReqServiceInx1, mCmdQApprovalInitialize, ref errorMessage))
                {
                    SendSyncPosData(SmtSndRevOcx, 20, ref smartroData);
                    return smartroData;
                }

                return smartroData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "SmartroVCat | DeviceReInitialLizeSync", ex.ToString());
                return smartroData;
            }
        }

        public class SmatroData
        {
            public enum CMDType
            {
                /// <summary>
                /// 카드승인응답
                /// </summary>
                CardApprovalRespone,
                /// <summary>
                /// 카드취소응답
                /// </summary>
                CardApprovalCancleRespone,
                /// <summary>
                /// 거래초기화 응답
                /// </summary>
                ApprovalInitializeRespone,
                /// <summary>
                /// 장비정보응답
                /// </summary>
                DeviceInfoRespone,
                /// <summary>
                /// 현금승인응답
                /// </summary>
                CashApprovalRespone,
                /// <summary>
                /// 현금취소응답
                /// </summary>
                CashApprovalCancleRespone,
                /// <summary>
                /// 승인환경설정응답
                /// </summary>
                VCatSetInfoRespone,
                /// <summary>
                /// 라인체크응답
                /// </summary>
                LineCheckRespone,
                None


            }
            private CMDType mCurrentCmdType = CMDType.None;
            /// <summary>
            /// 응답메세지
            /// </summary>
            public CMDType CurrentCmdType
            {
                set
                {
                    mCurrentCmdType = value;
                    mCurrentCmdTypeName = GetCMdTypeName(mCurrentCmdType);
                }

                get { return mCurrentCmdType; }
            }
            private string mCurrentCmdTypeName = string.Empty;
            public string CurrentCmdTypeName
            {
                get { return mCurrentCmdTypeName; }
            }
            /// <summary>
            /// 응답메세지 한글내용
            /// </summary>
            /// <param name="pCMdType"></param>
            /// <returns></returns>
            private string GetCMdTypeName(CMDType pCMdType)
            {
                string cmdTypeMessage = string.Empty;
                switch (pCMdType)
                {
                    case CMDType.ApprovalInitializeRespone:
                        cmdTypeMessage = "거래초기화 응답";
                        break;
                    case CMDType.CardApprovalCancleRespone:
                        cmdTypeMessage = "카드취소 응답";
                        break;
                    case CMDType.CardApprovalRespone:
                        cmdTypeMessage = "카드승인 응답";
                        break;
                    case CMDType.CashApprovalCancleRespone:
                        cmdTypeMessage = "현금취소 응답";
                        break;
                    case CMDType.CashApprovalRespone:
                        cmdTypeMessage = "현금승인 응답";
                        break;
                    case CMDType.DeviceInfoRespone:
                        cmdTypeMessage = "장비정보 응답";
                        break;
                    case CMDType.LineCheckRespone:
                        cmdTypeMessage = "라인체크 응답";
                        break;
                    case CMDType.VCatSetInfoRespone:
                        cmdTypeMessage = "승인환경설정 응답";
                        break;

                    case CMDType.None:
                        cmdTypeMessage = "없음";
                        break;
                    default:
                        cmdTypeMessage = "없음";
                        break;
                }
                return cmdTypeMessage;
            }

            /// <summary>
            /// 응답코드를 응답메세지 형태로 변환
            /// </summary>
            /// <param name="pErrorCode">응답코드</param>
            /// <returns></returns>
            private string MakeMessageReturnCode(string pErrorCode)
            {
                string errorMessage = string.Empty;
                switch (pErrorCode)
                {
                    case "00":
                        errorMessage = "정상";
                        break;
                    case "FE":
                        errorMessage = "카드인식실패(IC/MS모두 실패)";
                        break;
                    case "FF":
                        errorMessage = "거래실패(Application처리실패)";
                        break;
                    case "CB":
                        errorMessage = "FallBack Fail";
                        break;
                    case "F2":
                        errorMessage = "카드 Application없음/CardTImeOut";
                        break;
                    case "F9":
                        errorMessage = "Command ID없음";
                        break;
                    case "F8":
                        errorMessage = "입력 Data불량";
                        break;
                    case "EA":
                        errorMessage = "Inout TimeOut";
                        break;
                    case "EB":
                        errorMessage = "리더기 장비이상";
                        break;
                    case "EC":
                        errorMessage = "IC 카드 제거요망";
                        break;
                    case "ED":
                        errorMessage = "암호화 키 교환 요망";
                        break;
                    case "EE":
                        errorMessage = "암호화 PIN처리실패";
                        break;
                    case "S1":
                        errorMessage = "사인패드장치에러";
                        break;
                    case "L1":
                        errorMessage = "환경파일없음";
                        break;
                    case "L2":
                        errorMessage = "인증파일없음";
                        break;
                    case "L3":
                        errorMessage = "인증오류";
                        break;
                    case "L0":
                        errorMessage = "환경/인증파일없음";
                        break;
                    case "D0":
                        errorMessage = "단말기 포트 미설정";
                        break;
                    case "D1":
                        errorMessage = "무결성오류";
                        break;
                    case "W0":
                        errorMessage = "워킹키오류";
                        break;
                    case "OE":
                        break;
                    case "D9":
                        errorMessage = "전문오류";
                        break;
                    case "UC":
                        errorMessage = "사용자취소";
                        break;
                    case "NR":
                        errorMessage = "초기상태가아님";
                        break;
                    case "AE":
                        errorMessage = "이미 실행중";
                        break;
                    case "VR":
                        errorMessage = "VCAT 재실행 요망";
                        break;
                    case "IC":
                        errorMessage = "서버통신중 초기화 불가";
                        break;



                }
                return errorMessage;
            }
            private bool mIsSendSuccess = true;
            /// <summary>
            /// 전송까지 성공유무 false면 전송실패
            /// </summary>
            public bool IsSendSuccess
            {
                set { mIsSendSuccess = value; }
                get { return mIsSendSuccess; }
            }
            private int mCardAmt = 0;
            /// <summary>
            /// 카드금액
            /// </summary>
            public int CardAmt
            {
                set { mCardAmt = value; }
                get { return mCardAmt; }
            }
            private int mCardTax = 0;
            /// <summary>
            /// 세금
            /// </summary>
            public int CardTax
            {
                set { mCardTax = value; }
                get { return mCardTax; }
            }

            private int mCardBongSaIn = 0;
            /// <summary>
            /// 봉사료
            /// </summary>
            public int CardBongSaIn
            {
                set { mCardBongSaIn = value; }
                get { return mCardBongSaIn; }
            }
            private string mWoriingIndex = string.Empty;
            public string WoriingIndex
            {
                set { mWoriingIndex = value; }
                get { return mWoriingIndex; }
            }
            private int mCardHalbo = 0;
            /// <summary>
            /// 할부개월 0이면 무이자
            /// </summary>
            public int CardHalbo
            {
                set { mCardHalbo = value; }
                get { return mCardHalbo; }
            }
            private string mErrorMessage = string.Empty;
            public string ErrorMessage
            {
                set { mErrorMessage = value; }
                get { return mErrorMessage; }

            }
            private bool mSuccess = false;
            /// <summary>
            /// 정상응답여부
            /// </summary>
            public bool Success
            {
                set { mSuccess = value; }
                get { return mSuccess; }

            }

            private string mReceiveReturnCode = string.Empty;
            /// <summary>
            /// 응답코드
            /// </summary>
            public string ReceiveReturnCode
            {
                set
                {
                    mReceiveReturnCode = value;
                    mReceiveReturnMessage = MakeMessageReturnCode(mReceiveReturnCode);
                }
                get { return mReceiveReturnCode; }

            }

            private string mReceiveReturnMessage = string.Empty;
            /// <summary>
            /// returncode에 대한 상태
            /// </summary>
            public string ReceiveReturnMessage
            {
                set { mReceiveReturnMessage = value; }
                get { return mReceiveReturnMessage; }

            }


            private string mReceiveCardNumber = string.Empty;
            /// <summary>
            /// 카드번호
            /// </summary>
            public string ReceiveCardNumber // 카드번호 16자리
            {
                set { mReceiveCardNumber = value; }
                get { return mReceiveCardNumber; }

            }

            private string mReceiveCardAmt = string.Empty;
            /// <summary>
            /// 응답승인요청금액(원거래금액+세금+봉사료)
            /// </summary>
            public string ReceiveCardAmt // 
            {
                set { mReceiveCardAmt = value; }
                get { return mReceiveCardAmt; }

            }
            private string mReceiveTaxAmt = string.Empty;
            /// <summary>
            /// 응답세금
            /// </summary>
            public string ReceiveTaxAmt //  세금
            {
                set { mReceiveTaxAmt = value; }
                get { return mReceiveTaxAmt; }

            }
            private string mReceiveBongSaInx = string.Empty;
            /// <summary>
            /// 응답봉사료
            /// </summary>
            public string ReceiveBongSaInx //  봉사룔
            {
                set { mReceiveBongSaInx = value; }
                get { return mReceiveBongSaInx; }

            }

            private string mReceiveHalbu = string.Empty;
            /// <summary>
            ///  현금거래=현금승인유형(거래자구분(1)+확인자구분(2), 현금이외거래=할부개월
            /// </summary>
            public string ReceiveHalbu
            {
                set { mReceiveHalbu = value; }
                get { return mReceiveHalbu; }

            }
            private string mRecieveApprovalNumber = string.Empty;
            /// <summary>
            /// 응답승인번호
            /// </summary>
            public string RecieveApprovalNumber
            {
                set { mRecieveApprovalNumber = value; }
                get { return mRecieveApprovalNumber; }

            }
            private string mReceiveAppYmd = string.Empty; // 매출이 발생한 일자 (YYYYMMDD)
            /// <summary>
            /// 매출이발생한일자(YYYYMMDD)
            /// </summary>
            public string ReceiveAppYmd
            {
                set { mReceiveAppYmd = value; }
                get { return mReceiveAppYmd; }

            }

            private string mReceiveAppHms = string.Empty;

            /// <summary>
            ///매출이 발생한 시간 (hhmmss) 
            /// </summary>
            public string ReceiveAppHms
            {
                set { mReceiveAppHms = value; }
                get { return mReceiveAppHms; }

            }

            private string mReceiveUniqueNo = string.Empty;
            /// <summary>
            ///거래고유번호 응답
            /// </summary>
            public string ReceiveUniqueNo
            {
                set { mReceiveUniqueNo = value; }
                get { return mReceiveUniqueNo; }

            }

            private string mReceiveStoreNo = string.Empty;
            /// <summary>
            /// 응답가맹점번호
            /// </summary>
            public string ReceiveStoreNo
            {
                set { mReceiveStoreNo = value; }
                get { return mReceiveStoreNo; }

            }

            private string mReceiveTermNo = string.Empty;
            /// <summary>
            /// 단말기번호 응답
            /// </summary>
            public string ReceiveTermNo
            {
                set { mReceiveTermNo = value; }
                get { return mReceiveTermNo; }

            }

            private string mReceiveBalgubCode = string.Empty;
            /// <summary>
            /// 발급사코드 응답
            /// </summary>
            public string ReceiveBalgubCode
            {
                set { mReceiveBalgubCode = value; }
                get { return mReceiveBalgubCode; }

            }

            private string mReceiveBalgubName = string.Empty;
            /// <summary>
            /// 발급사명
            /// </summary>
            public string ReceiveBalgubName
            {
                set { mReceiveBalgubName = value; }
                get { return mReceiveBalgubName; }

            }

            private string mReceiveMaipCode = string.Empty;
            /// <summary>
            /// 매입사코드
            /// </summary>
            public string ReceiveMaipCode
            {
                set { mReceiveMaipCode = value; }
                get { return mReceiveMaipCode; }

            }

            private string mReceiveMaipName = string.Empty;
            /// <summary>
            /// 매입사명
            /// </summary>
            public string ReceiveMaipName
            {
                set { mReceiveMaipName = value; }
                get { return mReceiveMaipName; }

            }

            private string mReceiveMasterKey = string.Empty;
            public string ReceiveMasterKey
            {
                set { mReceiveMasterKey = value; }
                get { return mReceiveMasterKey; }

            }

            private string mReceiveWorkKey = string.Empty;
            public string ReceiveWorkKey
            {
                set { mReceiveWorkKey = value; }
                get { return mReceiveWorkKey; }

            }
            public enum DDCTYPE
            {
                DDC,
                EDI,
                EDC,
                DESC_ESC
            }
            private DDCTYPE mDdcType = DDCTYPE.DDC;
            public DDCTYPE DdcType
            {
                set { mDdcType = value; }
                get { return mDdcType; }
            }

            private string mReceiveDisplayMsg = string.Empty;
            /// <summary>
            /// 화면메세지
            /// </summary>
            public string ReceiveDisplayMsg
            {
                set { mReceiveDisplayMsg = value; }
                get { return mReceiveDisplayMsg; }

            }

            private string mReceiveFIlter1 = string.Empty;
            //응답필터1
            public string ReceiveFIlter1
            {
                set { mReceiveFIlter1 = value; }
                get { return mReceiveFIlter1; }

            }

            private string mReceiveFIlter2 = string.Empty;
            //리스브 필터2
            public string ReceiveFIlter2
            {
                set { mReceiveFIlter2 = value; }
                get { return mReceiveFIlter2; }

            }

            private string mDeviceBusinessNum = string.Empty;
            /// <summary>
            /// 사업자번호	AN	10	가맹점 사업자 정보
            /// </summary>
            public string DeviceBusinessNum
            {
                set { mDeviceBusinessNum = value; }
                get { return mDeviceBusinessNum; }

            }
            /// <summary>
            /// 가맹점 명	ANS	V30	가맹점 상호
            /// </summary>
            private string mDeviceStoreName = string.Empty;
            public string DeviceStoreName
            {
                set { mDeviceStoreName = value; }
                get { return mDeviceStoreName; }

            }

            private string mDeviceOwnerName = string.Empty;
            /// <summary>
            /// 대표자 명	ANS	V12	가맹점 대표자 성명
            /// </summary>
            public string DeviceOwnerName
            {
                set { mDeviceOwnerName = value; }
                get { return mDeviceOwnerName; }
            }

            private string mDeviceStoreTel = string.Empty;
            /// <summary>
            /// 가맹점 전화	ANS	V16	가맹점 전화번호
            /// </summary>
            public string DeviceStoreTel
            {
                set { mDeviceStoreTel = value; }
                get { return mDeviceStoreTel; }
            }
            private string mDeviceStoreAddr = string.Empty;
            /// <summary>
            /// 가맹점 주소	ANS	V50	가맹점 주소
            /// </summary>
            public string DeviceStoreAddr
            {
                set { mDeviceStoreAddr = value; }
                get { return mDeviceStoreAddr; }
            }

            private string mDeviceVCatVer = string.Empty;
            /// <summary>
            /// 단말기 버전	ANS	V15	VCAT버전 정보
            /// </summary>
            public string DeviceVCatVer
            {
                set { mDeviceVCatVer = value; }
                get { return mDeviceVCatVer; }
            }
        }
    }
}
