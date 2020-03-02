using FadeFox.Text;
using NPCommon;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NPCommon.Van
{
    public class FirstData
    {
        public const string Success = "0000";
        public const string Fail = "-1000";
        public const string PosInitialFail = "-1001";
        public const string PaySetFail = "-1003";
        public const string OcbSetFail = "-1005";
        public const string SignSetFail = "-1007";
        public const string ReceiveFail = "-1008";

        /// <summary>
        /// firstdata(van)사 펌웨어 다운로드 최초 한번은 해야 신용카드 결제가 가능
        /// </summary>
        /// <returns></returns>
        public static bool CreditCardFrimwareLoad()
        {


            // 2016.10.27  KIS_DIP 추가종료
            NPSYS.Device.UsingSettingCreditCard = false;
            bool isSuccess = false;
            if (NPSYS.Device.UsingSettingCreditCard)
            {

                string terminalNo = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardTerminalNo).ToUpper().Trim();
                string saup = NPSYS.Config.GetValue(ConfigID.FeatureSettingCreditCardSaupNo).ToUpper().Trim();
                if (terminalNo.Trim() != "" && saup.Trim() != "")
                {
                    PosDownloadData results = FirstData.PosDownload(terminalNo, saup);

                    string strr = "";

                    strr = "단말기번호 = [" + terminalNo + "\n"
                        + "사업자번호 = [" + saup + "\n"
                        + "  응답코드 = [" + results.ResCode + "]\n"
                        + "  전표출력 Flag = [" + results.PrintFlag + "]\n"
                        + "  응답메시지 = [" + results.ResMsg + "]\n"
                        + "  제품 일련번호 = [" + results.ProductNumber + "]\n"
                        + "  대표자 명 = [" + results.BossName + "]\n"
                        + "  가맹점 명 = [" + results.BusinessName + "]\n"
                        + "  가맹점 주소 = [" + results.BusinessAddress + "]\n"
                        + "  가맹점 전화번호 = [" + results.BusinessTel + "]\n"
                        + "  대리점 전화번호 = [" + results.MemberTel + "]\n";

                    if (results.ResCode.Trim() == "0000")
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "iPos | CreditCardFrimwareLoad", "신용카드펌웨어업데이트성공:" + strr);
                        isSuccess = true;
                        NPSYS.Device.UsingSettingCreditCard = false;
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "iPos | CreditCardFrimwareLoad", "신용카드펌웨어업데이트실패:" + strr);
                        NPSYS.Device.UsingSettingCreditCard = false;
                    }


                }
                else
                {
                    NPSYS.Device.UsingSettingCreditCard = true;
                }

            }
            return isSuccess;
        }

        public enum CreditCardResult
        {
            /// -1000 – 응답코드가 '0000' 이 아님
            /// -1001 – POS Initial, 가맹점 임의정보 설정 실패
            /// -1002 – 싞용카드 정보, 싞용카드 입력구붂 설정 실패
            /// -1003 – 거래금액,할부개월,봉사료,세금 설정 실패
            /// -1005 - OCB카드 정보, OCB카드 입력구붂 설정 실패
            /// -1007 – 사인데이타,사인압축방식,사인MAC 설정 실패
            /// -1008 – 응답코드 획득 실패.
            Success = 0000,/// 
            Fail = -1000, // 응답코드가 '0000' 이 아님
            PosInitialFail = -1001, // POS Initial, 가맹점 임의정보 설정 실패
            PaySetFail = -1003, //거래금액,할부개월,봉사료,세금 설정 실패
            OcbSetFail = -1005, // OCB카드 정보, OCB카드 입력구붂 설정 실패
            SignSetFail = -1007, // 사인데이타,사인압축방식,사인MAC 설정 실패
            ReceiveFail = -1008 // 응답코드 획득 실패.

        }
        public static CreditAuthSimpleExData CreditAuthSimpleEx(string pTerminalNumber, string pCreditInfo, string pCreditInputType, string pTotalAmount, string pTaxAmount)
        {
            try
            {
                CreditAuthSimpleExData data = new CreditAuthSimpleExData();
                data.TerminalNumber = pTerminalNumber;
                data.SequenceNumber = "";
                data.PosInitial = "FDIK__R&D___";
                data.TempInfo = "";
                byte[] tempInfo = new byte[8];
                data.CreditInfo = pCreditInfo;
                data.CreditInputType = pCreditInputType;
                data.InstallPeriod = "";
                byte[] installPreriod = new byte[2];
                data.TotalAmount = pTotalAmount;
                data.ServiceAmount = "";
                byte[] serviceAmount = new byte[10];
                data.TaxAmount = pTaxAmount;
                //byte[] taxAmount = new byte[10];
                data.OcbInfo = "";
                byte[] ocbInfo = new byte[37];
                data.OcbInputType = "";
                byte[] ocbInputType = new byte[1];
                data.SignCompMethod = "";
                byte[] signCompMethod = new byte[2];
                data.SignMac = "";
                byte[] signMac = new byte[32];
                data.SignData = "";
                byte[] signData = new byte[1536];

                StringBuilder printFlag = new StringBuilder(1, 1);
                StringBuilder resCode = new StringBuilder(4, 4);
                StringBuilder authNumber = new StringBuilder(12, 12);
                StringBuilder authDate = new StringBuilder(6, 6);
                StringBuilder memberNumber = new StringBuilder(15, 15);
                StringBuilder ddcFlag = new StringBuilder(1, 1);
                StringBuilder ddcNumber = new StringBuilder(10, 10);
                StringBuilder resMsg = new StringBuilder(32, 32);
                StringBuilder cardName = new StringBuilder(32, 32);
                StringBuilder issuerCode = new StringBuilder(2, 2);
                StringBuilder issuerName = new StringBuilder(8, 8);
                StringBuilder acquirerCode = new StringBuilder(2, 2);
                StringBuilder acquirerName = new StringBuilder(8, 8);
                StringBuilder giftCash = new StringBuilder(9, 9);
                StringBuilder notice = new StringBuilder(20, 20);
                StringBuilder ocbResCode = new StringBuilder(4, 4);
                StringBuilder customerName = new StringBuilder(10, 10);
                StringBuilder addPoint = new StringBuilder(9, 9);
                StringBuilder savePoint = new StringBuilder(9, 9);
                StringBuilder usablePoint = new StringBuilder(9, 9);
                StringBuilder boardMsg = new StringBuilder(112, 112);
                StringBuilder chkFlag = new StringBuilder(1, 1);

                data.ReturnCode = FDIK_CreditAuth_SimpleEx(
                Encoding.Default.GetBytes(data.TerminalNumber),
                Encoding.Default.GetBytes(data.SequenceNumber),
                Encoding.Default.GetBytes(data.PosInitial),
                tempInfo,
                Encoding.Default.GetBytes(data.CreditInfo),
                Encoding.Default.GetBytes(data.CreditInputType),
                installPreriod,
                Encoding.Default.GetBytes(data.TotalAmount),
                serviceAmount,
                Encoding.Default.GetBytes(data.TaxAmount),
                ocbInfo,
                ocbInputType,
                signCompMethod,
                signMac,
                signData,
                printFlag,
                resCode,
                authNumber,
                authDate,
                memberNumber,
                ddcFlag,
                ddcNumber,
                resMsg,
                cardName,
                issuerCode,
                issuerName,
                acquirerCode,
                acquirerName,
                giftCash,
                notice,
                ocbResCode,
                customerName,
                addPoint,
                savePoint,
                usablePoint,
                boardMsg,
                chkFlag
                );

                data.PrintFlag = printFlag.ToString().Trim();
                data.ResCode = resCode.ToString().Trim();
                data.AuthNumber = authNumber.ToString().Trim();
                data.AuthDate = authDate.ToString().Trim();
                data.MemberNumber = memberNumber.ToString().Trim();
                data.DdcFlag = ddcFlag.ToString().Trim();
                data.DdcNumber = ddcNumber.ToString().Trim();
                data.ResMsg = resMsg.ToString().Trim();
                data.CardName = cardName.ToString().Trim();
                data.IssuerCode = issuerCode.ToString().Trim();
                data.IssuerName = issuerName.ToString().Trim();
                data.AcquirerCode = acquirerCode.ToString().Trim();
                data.AcquirerName = acquirerName.ToString().Trim();
                data.GiftCash = giftCash.ToString().Trim();
                data.Notice = notice.ToString().Trim();
                data.OcbResCode = ocbResCode.ToString().Trim();
                data.CustomerName = customerName.ToString().Trim();
                data.AddPoint = addPoint.ToString().Trim();
                data.SavePoint = savePoint.ToString().Trim();
                data.UsablePoint = usablePoint.ToString().Trim();
                data.BoardMsg = boardMsg.ToString().Trim();
                data.ChkFlag = chkFlag.ToString().Trim();

                return data;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "iPos|CreditAuthSimpleExData", ex.ToString());
                return null;
            }
        }

        public static CashReceiptAuthData CashReceiptAuthEx(string pTerminalNumber, string pId_Info, string pId_InputType, string pTotalAmount, string pTaxAmount)
        {
            try
            {
                CashReceiptAuthData data = new CashReceiptAuthData();
                data.TerminalNumber = pTerminalNumber;
                data.SequenceNumber = "";
                data.PosInitial = "FDIK__R&D___";
                data.TempInfo = "";
                byte[] tempInfo = new byte[8];
                data.id_info = pId_Info;
                data._id_input_type = pId_InputType;
                data._tran_type = "00";
                data.TotalAmount = pTotalAmount;
                data.ServiceAmount = "";
                byte[] serviceAmount = new byte[10];
                data.TaxAmount = pTaxAmount;

                StringBuilder printFlag = new StringBuilder(1, 1);
                StringBuilder resCode = new StringBuilder(4, 4);
                StringBuilder authNumber = new StringBuilder(12, 12);
                StringBuilder authDate = new StringBuilder(6, 6);
                StringBuilder business_number = new StringBuilder(15, 15);
                StringBuilder resMsg = new StringBuilder(32, 32);
                StringBuilder boardMsg = new StringBuilder(112, 112);


                data.ReturnCode = FDIK_CashReceiptAuth(
                Encoding.Default.GetBytes(data.TerminalNumber),
                Encoding.Default.GetBytes(data.SequenceNumber),
                Encoding.Default.GetBytes(data.PosInitial),
                tempInfo,
                Encoding.Default.GetBytes(data.id_info),
                Encoding.Default.GetBytes(data._id_input_type),
                Encoding.Default.GetBytes(data._tran_type),
                Encoding.Default.GetBytes(data.TotalAmount),
                serviceAmount,
                Encoding.Default.GetBytes(data.TaxAmount),
                printFlag,
                resCode,
                authNumber,
                authDate,
                business_number,
                resMsg,
                boardMsg

                );

                data.PrintFlag = printFlag.ToString().Trim();
                data.ResCode = resCode.ToString().Trim();
                data.AuthNumber = authNumber.ToString().Trim();
                data.AuthDate = authDate.ToString().Trim();
                data.business_number = business_number.ToString().Trim();
                data.ResMsg = resMsg.ToString().Trim();
                data.BoardMsg = boardMsg.ToString().Trim();

                return data;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "iPos|CashReceiptAuthEx", "예외사항" + ex.ToString());
                return null;
            }
        }


        public static PosDownloadData PosDownload(string pTerminalNumber, string pBusinessNumber)
        {
            PosDownloadData data = new PosDownloadData();

            data.TerminalNumber = pTerminalNumber;
            data.SequenceNumber = "";
            data.PosInitial = "FDIK__R&D___";
            data.TempInfo = "";
            byte[] tempInfo = new byte[8];
            data.BusinessNumber = pBusinessNumber;
            data.PosVersion = "1.0";

            StringBuilder printFlag = new StringBuilder(10, 10);
            StringBuilder resCode = new StringBuilder(10, 10);
            StringBuilder resMsg = new StringBuilder(40, 40);
            StringBuilder productNumber = new StringBuilder(20, 20);
            StringBuilder bossName = new StringBuilder(20, 20);
            StringBuilder businessName = new StringBuilder(30, 30);
            StringBuilder businessAddress = new StringBuilder(50, 50);
            StringBuilder businessTel = new StringBuilder(20, 20);
            StringBuilder memberTel = new StringBuilder(20, 20);

            data.ReturnCode = FDIK_POSDownload(
                Encoding.Default.GetBytes(data.TerminalNumber),
                Encoding.Default.GetBytes(data.SequenceNumber),
                Encoding.Default.GetBytes(data.PosInitial),
                tempInfo,
                Encoding.Default.GetBytes(data.BusinessNumber),
                Encoding.Default.GetBytes(data.PosVersion),
                printFlag,
                resCode,
                resMsg,
                productNumber,
                bossName,
                businessName,
                businessAddress,
                businessTel,
                memberTel);

            data.PrintFlag = printFlag.ToString().Trim();
            data.ResCode = resCode.ToString().Trim();
            data.ResMsg = resMsg.ToString().Trim();
            data.ProductNumber = productNumber.ToString().Trim();
            data.BossName = bossName.ToString().Trim();
            data.BusinessName = businessName.ToString().Trim();
            data.BusinessAddress = businessAddress.ToString().Trim();
            data.BusinessTel = businessTel.ToString().Trim();
            data.MemberTel = memberTel.ToString().Trim();

            return data;
        }

        [DllImport("fdikpos43.dll", EntryPoint = "FDIK_POSDownload")]
        private static extern int FDIK_POSDownload(
            byte[] in_terminal_number,
            byte[] in_sequence_number,
            byte[] in_pos_initial,
            byte[] in_temp_info,
            byte[] in_business_number,
            byte[] in_pos_version,
            StringBuilder out_print_flag,
            StringBuilder out_res_code,
            StringBuilder out_res_msg,
            StringBuilder out_product_seq_number,
            StringBuilder out_boss_name,
            StringBuilder out_business_name,
            StringBuilder out_business_address,
            StringBuilder out_business_tel,
            StringBuilder out_member_tel
            );


        //1번에 해당하는 기능임.(신용승인+OCB,페이백 적립) - Simple
        [DllImport("fdikpos43.dll", EntryPoint = "FDIK_CreditAuth_SimpleEx")]
        private static extern int FDIK_CreditAuth_SimpleEx(
            byte[] in_terminal_number,
            byte[] in_sequence_number,
            byte[] in_pos_initial,
            byte[] in_temp_info,
            byte[] in_credit_info,
            byte[] in_credit_input_type,
            byte[] in_install_period,
            byte[] in_total_amount,
            byte[] in_service_amount,
            byte[] in_tax_amount,
            byte[] in_ocb_info,
            byte[] in_ocb_input_type,
            byte[] in_sign_compress_method,
            byte[] in_sign_mac,
            byte[] in_sign_data,
            StringBuilder out_print_flag,
            StringBuilder out_res_code,
            StringBuilder out_auth_number,
            StringBuilder out_auth_date,
            StringBuilder out_member_number,
            StringBuilder out_ddc_flag,
            StringBuilder out_ddc_number,
            StringBuilder out_res_msg,
            StringBuilder out_card_name,
            StringBuilder out_issuer_code,
            StringBuilder out_issuer_name,
            StringBuilder out_acquirer_code,
            StringBuilder out_acquirer_name,
            StringBuilder out_gift_cash,
            StringBuilder out_notice,
            StringBuilder out_ocb_res_code,
            StringBuilder out_customer_name,
            StringBuilder out_add_point,
            StringBuilder out_save_point,
            StringBuilder out_usable_point,
            StringBuilder out_broad_msg,
            StringBuilder out_chk_flagv
            );

        //1번에 해당하는 기능임.(신용승인+OCB,페이백 적립) - Simple
        [DllImport("fdikpos43.dll", EntryPoint = "FDIK_CashReceiptAuth")]
        private static extern int FDIK_CashReceiptAuth(
            byte[] in_terminal_number,
            byte[] in_sequence_number,
            byte[] in_pos_initial,
            byte[] in_temp_info,
            byte[] in_id_info,
            byte[] in_id_input_type,
            byte[] in_tran_type,
            byte[] in_total_amount,
            byte[] in_service_amount,
            byte[] in_tax_amount,
            StringBuilder out_print_flag,
            StringBuilder out_res_code,
            StringBuilder out_auth_number,
            StringBuilder out_auth_date,
            StringBuilder out_business_number,
            StringBuilder out_res_msg,
            StringBuilder out_broad_msg

            );
    }

    public class PosDownloadData
    {
        /// <summary>
        /// 리턴코드
        /// 0 - 성공
        /// 1 – 자동 망 취소 수행됨
        /// -1000 – 응답코드가 '0000' 이 아님
        /// -1001 – POS Initial, 가맹점 임의정보 설정 실패
        /// -1002 – 사업자 번호, POS Version 설정 실패
        /// -1003 – 응답코드 획득 실패.
        /// 그 외 – 표4_에러코드 표 참조
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
        /// 가맹점 임의 정보
        /// </summary>
        public string TempInfo
        {
            get;
            set;
        }

        /// <summary>
        /// 사업자번호
        /// </summary>
        public string BusinessNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 프로그램 버전
        /// </summary>
        public string PosVersion
        {
            get;
            set;
        }

        /// <summary>
        /// 전표출력Flag, 'O'-전표출력, 'X'-전표미출력
        /// </summary>
        public string PrintFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 응답코드 '0000' 정상승인
        /// </summary>
        public string ResCode
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
        /// 제품일련번호
        /// </summary>
        public string ProductNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 대표자 명
        /// </summary>
        public string BossName
        {
            get;
            set;
        }

        /// <summary>
        /// 가맹점 명
        /// </summary>
        public string BusinessName
        {
            get;
            set;
        }

        /// <summary>
        /// 가맹점 주소
        /// </summary>
        public string BusinessAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 가맹점 전화번호
        /// </summary>
        public string BusinessTel
        {
            get;
            set;
        }

        /// <summary>
        /// 대리점 전화번호
        /// </summary>
        public string MemberTel
        {
            get;
            set;
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
