using FadeFox.Text;
using System;
using System.Runtime.InteropServices;
using System.Text;
namespace NPCommon.Van
{
    public class FirstDataDip
    {
        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Status", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Status();

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Create", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Create(string p_pszServer, string p_pszPort);

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Destroy", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Destroy();

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_MkTranId", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_MkTranId(byte[] p_pszOutValueBuffer, int p_inOutValueBufferLen);

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Init", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Init(string p_pszTranId, string p_pszMsgCmd, string p_pszMsgCert);

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Term", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Term();

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Input", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Input(string p_pszKey, string p_pszVal);

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Execute", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Execute();

        [DllImport(@"FirstData\Win4POS\Win4POSDll.dll", EntryPoint = "FDK_WIN4POS_Output", CharSet = System.Runtime.InteropServices.CharSet.None)]
        private static extern int FDK_WIN4POS_Output(string p_pszKey, byte[] p_pszOutValueBuffer, int p_inOutValueBufferLen);

        public static bool Connect(string pIp, string pPort)
        {

            try
            {
                int iRet = FDK_WIN4POS_Create(pIp, pPort);
                if (iRet != 0 && iRet != 1)
                {
                    return false;
                }
                else
                {
                    iRet = FDK_WIN4POS_Status();
                    FDK_WIN4POS_Term();
                    if (iRet != 0)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | Connect", "Win4POS 구동안됨 코드:" + iRet.ToString());
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | Connect", ex.ToString());
                return false;
            }
        }

        public static bool Close()
        {

            try
            {
                int iRet = FDK_WIN4POS_Destroy();
                if (iRet != 0 && iRet != 1)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | Connect", "Win4POS 종료안됨 코드:" + iRet.ToString());
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | Close", ex.ToString());
                return false;
            }
        }

        public enum readerStatus
        {
            Success = 0,
            Fale = -1,
            ReaderIcIn = -113,
        }
        public static readerStatus ReadState()
        {
            try
            {
                int iRet = -1;
                byte[] OutTranId = new byte[256];
                byte[] OutValue = new byte[256];


                iRet = FDK_WIN4POS_Status();

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", "Win4POS 실행 확인");
                    return readerStatus.Fale;
                }

                iRet = FDK_WIN4POS_MkTranId(OutTranId, 50);

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", "고유번호 확인 실패");
                    return readerStatus.Fale;
                }

                //연결 상태 체크 초기화
                iRet = FDK_WIN4POS_Init(OutTranId.ToString(), "CON", "FDK");

                if (iRet != 0)
                {
                    FDK_WIN4POS_Term();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", "Win4POS Init 실패");
                    return readerStatus.Fale;
                }

                //체크 장비 대상 구분 설정
                iRet = FDK_WIN4POS_Input("장치구분", "SCR");
                if (iRet != 0)
                {
                    FDK_WIN4POS_Term();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", "Win4POS SCR 실패");
                    return readerStatus.Fale;
                }


                iRet = FDK_WIN4POS_Execute();


                if (iRet != 0)
                {

                    int returncode = iRet;
                    FDK_WIN4POS_Output("MSG_RET_CODE", OutValue, 256);
                    string errorcode = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);
                    FDK_WIN4POS_Output("MSG_RET_STR", OutValue, 256);
                    string errorMessage = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);

                    FDK_WIN4POS_Term();
                    if (Convert.ToInt32(returncode) == (int)readerStatus.ReaderIcIn)
                    {
                        return readerStatus.ReaderIcIn;
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", "[리턴코드]" + returncode.ToString() + " [에러코드]" + errorcode + " [에러내용]" + errorMessage);
                        return readerStatus.Fale;
                    }

                }
                FDK_WIN4POS_Term();
                return readerStatus.Success;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | ReadState", ex.ToString());
                return readerStatus.Fale;
            }
        }

        public static readerStatus CardEject()
        {
            try
            {
                int iRet = -1;
                byte[] OutTranId = new byte[256];
                byte[] OutValue = new byte[256];


                iRet = FDK_WIN4POS_Status();

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", "Win4POS 실행 확인");
                    return readerStatus.Fale;
                }

                iRet = FDK_WIN4POS_MkTranId(OutTranId, 50);

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", "고유번호 확인 실패");
                    return readerStatus.Fale;
                }

                //연결 상태 체크 초기화
                iRet = FDK_WIN4POS_Init(OutTranId.ToString(), "UTL", "FDK");

                if (iRet != 0)
                {
                    FDK_WIN4POS_Term();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", "Win4POS Init 실패");
                    return readerStatus.Fale;
                }

                //체크 장비 대상 구분 설정
                iRet = FDK_WIN4POS_Input("기능", "SCRUnLock");
                if (iRet != 0)
                {
                    FDK_WIN4POS_Term();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", "Win4POS SCR 실패");
                    return readerStatus.Fale;
                }


                iRet = FDK_WIN4POS_Execute();
                if (iRet != 0)
                {
                    int returncode = iRet;
                    FDK_WIN4POS_Output("MSG_RET_CODE", OutValue, 256);
                    string errorcode = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);
                    FDK_WIN4POS_Output("MSG_RET_STR", OutValue, 256);
                    string errorMessage = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);
                    FDK_WIN4POS_Term();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", "[리턴코드]" + returncode.ToString() + " [에러코드]" + errorcode + " [에러내용]" + errorMessage);
                    return readerStatus.Fale;


                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FirstDataDip | CardEject", "[카드배출성공]");
                FDK_WIN4POS_Term();
                return readerStatus.Success;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CardEject", ex.ToString());
                return readerStatus.Fale;
            }

        }
        public static CreditAuthSimpleExData CreditAuthSimpleEx(string pTerminalNumber, string pTotalAmount, string pTaxAmount = "0")
        {
            int iRet = -1;
            byte[] OutTranId = new byte[256];
            byte[] OutValue = new byte[256];
            string strBizNo = NPSYS.gVanSaup;

            CreditAuthSimpleExData creditAuthSimpleExData = new CreditAuthSimpleExData();
            creditAuthSimpleExData.ResCode = "-1";
            try
            {
                iRet = FDK_WIN4POS_Status();

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "Win4POS 실행 확인");
                    creditAuthSimpleExData.ResMsg = "Win4POS 실행 확인";
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_MkTranId(OutTranId, 50);

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "고유번호 확인 실패");
                    creditAuthSimpleExData.ResMsg = "Win4POS 실행 확인";
                    return creditAuthSimpleExData;
                }

                //결제처리 호출 초기화
                iRet = FDK_WIN4POS_Init(OutTranId.ToString(), "PAY", "FDK");

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "결제명령 PAY 실패");
                    creditAuthSimpleExData.ResMsg = "결제명령 PAY 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                //신용승인 화면 호출
                iRet = FDK_WIN4POS_Input("거래구분", "CRD");
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "결제명령 CRD 실패");
                    creditAuthSimpleExData.ResMsg = "결제명령 CRD 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("사업자번호", strBizNo);
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "사업자번호 실패");
                    creditAuthSimpleExData.ResMsg = "사업자번호 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("요청금액", pTotalAmount.ToString());
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "요청금액 실패");
                    creditAuthSimpleExData.ResMsg = "요청금액 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("봉사료", "0");
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "봉사료 실패");
                    creditAuthSimpleExData.ResMsg = "요청금액 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("세금", pTaxAmount.ToString());
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "세금 실패");
                    creditAuthSimpleExData.ResMsg = "세금 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                //할부 ( 설정 할 경우 신용카드 창에서 입력 불가, 취소시 원거래 할부 개월수 입력 필수 )
                iRet = FDK_WIN4POS_Input("할부", "00");
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "할부 실패");
                    creditAuthSimpleExData.ResMsg = "할부 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Execute();
                if (iRet != 0 && iRet != -203)
                {
                    int returncode = iRet;
                    FDK_WIN4POS_Output("MSG_RET_CODE", OutValue, 256);
                    string errorcode = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);
                    FDK_WIN4POS_Output("MSG_RET_STR", OutValue, 256);
                    string errorMessage = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "[리턴코드]" + returncode.ToString() + " [에러코드]" + errorcode + " [에러내용]" + errorMessage);
                    creditAuthSimpleExData.ResMsg = errorMessage;
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                string receiveData = string.Empty;
                FDK_WIN4POS_Output("프린터출력유무", OutValue, 256);
                receiveData = " 프린터출력유무 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("응답코드", OutValue, 256);
                receiveData += " 응답코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.ResCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                if (iRet == -203) // 사용자취소
                {
                    creditAuthSimpleExData.ResCode = "9999";
                }


                FDK_WIN4POS_Output("승인번호", OutValue, 256);
                receiveData += " 승인번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AuthNumber = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();


                FDK_WIN4POS_Output("승인일자", OutValue, 256);
                receiveData += " 승인일자 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AuthDate = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("가맹점번호", OutValue, 256);
                receiveData += " 가맹점번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("DDCFlag", OutValue, 256);
                receiveData += " DDCFlag :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("DDC전표번호", OutValue, 256);
                receiveData += " DDC전표번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("응답메시지1", OutValue, 256);
                receiveData += " 응답메시지1 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.ResMsg = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                if (iRet == -203) // 사용자취소
                {
                    creditAuthSimpleExData.ResMsg = "사용자취소";
                }
                FDK_WIN4POS_Output("응답메시지2", OutValue, 256);
                receiveData += " 응답메시지2 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("카드명", OutValue, 256);
                receiveData += " 카드명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.CardName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();


                FDK_WIN4POS_Output("발급사코드", OutValue, 256);
                receiveData += " 발급사코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.IssuerCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("발급사명", OutValue, 256);
                receiveData += " 발급사명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.IssuerName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("매입사코드", OutValue, 256);
                receiveData += " 매입사코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AcquirerCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("매입사명", OutValue, 256);
                receiveData += " 매입사명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AcquirerName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("잔액", OutValue, 256);
                receiveData += " 잔액 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("Notice", OutValue, 256);
                receiveData += " Notice" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림1", OutValue, 256);
                receiveData += " 알림1" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림2", OutValue, 256);
                receiveData += " 알림2" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림3", OutValue, 256);
                receiveData += " 알림3" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림4", OutValue, 256);
                receiveData += " 알림4" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("마스킹카드번호", OutValue, 256);
                receiveData += " 마스킹카드번호" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.CardNumber = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("할부", OutValue, 256);
                receiveData += " 할부" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();


                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, " FirstDataDip | CreditAuthSimpleEx", "[카드응답결과]" + receiveData);

                FDK_WIN4POS_Term();
                return creditAuthSimpleExData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, " FirstDataDip | CreditAuthSimpleEx", ex.ToString());
                return null;
            }

        }

        public static CreditAuthSimpleExData CreditAuthSimpleCancleEx(string pTerminalNumber, string pTotalAmount, string pAuthDate, string pAuthNo, string pTaxAmount = "0")
        {
            int iRet = -1;
            byte[] OutTranId = new byte[256];
            byte[] OutValue = new byte[256];
            string strBizNo = NPSYS.gVanSaup;

            CreditAuthSimpleExData creditAuthSimpleExData = new CreditAuthSimpleExData();
            creditAuthSimpleExData.ResCode = "-1";
            try
            {
                iRet = FDK_WIN4POS_Status();

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "Win4POS 실행 확인");
                    creditAuthSimpleExData.ResMsg = "Win4POS 실행 확인";
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_MkTranId(OutTranId, 50);

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "고유번호 확인 실패");
                    creditAuthSimpleExData.ResMsg = "Win4POS 실행 확인";
                    return creditAuthSimpleExData;
                }

                //결제처리 호출 초기화
                iRet = FDK_WIN4POS_Init(OutTranId.ToString(), "PAY", "FDK");

                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "결제명령 PAY 실패");
                    creditAuthSimpleExData.ResMsg = "결제명령 PAY 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                //신용승인 화면 호출
                iRet = FDK_WIN4POS_Input("거래구분", "CRD");
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "결제명령 CRD 실패");
                    creditAuthSimpleExData.ResMsg = "결제명령 CRD 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("사업자번호", strBizNo);
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "사업자번호 실패");
                    creditAuthSimpleExData.ResMsg = "사업자번호 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }


                iRet = FDK_WIN4POS_Input("취소구분", "Y");
                if (iRet != 0)
                {
                    creditAuthSimpleExData.ResMsg = "취소구분 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("요청금액", pTotalAmount.ToString());
                if (iRet != 0)
                {

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "요청금액 실패");
                    creditAuthSimpleExData.ResMsg = "요청금액 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("봉사료", "0");
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "봉사료 실패");
                    creditAuthSimpleExData.ResMsg = "요청금액 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("세금", pTaxAmount.ToString());
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "세금 실패");
                    creditAuthSimpleExData.ResMsg = "세금 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                //할부 ( 설정 할 경우 신용카드 창에서 입력 불가, 취소시 원거래 할부 개월수 입력 필수 )
                iRet = FDK_WIN4POS_Input("할부", "00");
                if (iRet != 0)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "할부 실패");
                    creditAuthSimpleExData.ResMsg = "할부 실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("원거래일자", pAuthDate);
                if (iRet != 0)
                {
                    creditAuthSimpleExData.ResMsg = "원거래일자 입력실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Input("원승인번호", pAuthNo);
                if (iRet != 0)
                {
                    creditAuthSimpleExData.ResMsg = "원승인번호 입력실패";
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                iRet = FDK_WIN4POS_Execute();
                if (iRet != 0 && iRet != -203)
                {
                    int returncode = iRet;
                    FDK_WIN4POS_Output("MSG_RET_CODE", OutValue, 256);
                    string errorcode = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);
                    FDK_WIN4POS_Output("MSG_RET_STR", OutValue, 256);
                    string errorMessage = Encoding.Default.GetString(OutValue).Replace("\0", string.Empty);

                    TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FirstDataDip | CreditAuthSimpleEx", "[리턴코드]" + returncode.ToString() + " [에러코드]" + errorcode + " [에러내용]" + errorMessage);
                    creditAuthSimpleExData.ResMsg = errorMessage;
                    FDK_WIN4POS_Term();
                    return creditAuthSimpleExData;
                }

                string receiveData = string.Empty;
                FDK_WIN4POS_Output("프린터출력유무", OutValue, 256);
                receiveData = " 프린터출력유무 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("응답코드", OutValue, 256);
                receiveData += " 응답코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.ResCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("승인번호", OutValue, 256);
                receiveData += " 승인번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AuthNumber = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("승인일자", OutValue, 256);
                receiveData += " 승인일자 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AuthDate = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("가맹점번호", OutValue, 256);
                receiveData += " 가맹점번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("DDCFlag", OutValue, 256);
                receiveData += " DDCFlag :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("DDC전표번호", OutValue, 256);
                receiveData += " DDC전표번호 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("응답메시지1", OutValue, 256);
                receiveData += " 응답메시지1 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.ResMsg = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();


                FDK_WIN4POS_Output("응답메시지2", OutValue, 256);
                receiveData += " 응답메시지2 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("카드명", OutValue, 256);
                receiveData += " 카드명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.CardName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();


                FDK_WIN4POS_Output("발급사코드", OutValue, 256);
                receiveData += " 발급사코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.IssuerCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("발급사명", OutValue, 256);
                receiveData += " 발급사명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.IssuerName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("매입사코드", OutValue, 256);
                receiveData += " 매입사코드 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AcquirerCode = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("매입사명", OutValue, 256);
                receiveData += " 매입사명 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.AcquirerName = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("잔액", OutValue, 256);
                receiveData += " 잔액 :" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("Notice", OutValue, 256);
                receiveData += " Notice" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림1", OutValue, 256);
                receiveData += " 알림1" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림2", OutValue, 256);
                receiveData += " 알림2" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림3", OutValue, 256);
                receiveData += " 알림3" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("알림4", OutValue, 256);
                receiveData += " 알림4" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("마스킹카드번호", OutValue, 256);
                receiveData += " 마스킹카드번호" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();
                creditAuthSimpleExData.CardNumber = Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                FDK_WIN4POS_Output("할부", OutValue, 256);
                receiveData += " 할부" + Encoding.Default.GetString(OutValue).Replace("\0", "").Trim();

                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, " FirstDataDip | CreditAuthSimpleEx", "[카드응답결과]" + receiveData);

                FDK_WIN4POS_Term();
                return creditAuthSimpleExData;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, " FirstDataDip | CreditAuthSimpleEx", ex.ToString());
                return null;
            }

        }


        public class CreditAuthSimpleExData
        {
            public string CardNumber
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
        }

    }
}
