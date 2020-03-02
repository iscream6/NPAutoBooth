using FadeFox.Text;
using System;
using System.Text;


namespace NPCommon.Van
{
    //리얼IP 211.192.50.244
    //리얼포트1515
    public class KIS_TITDIPDevice
    {
        private string mVanIP = string.Empty;
        public string VanIP
        {
            set { mVanIP = value; }
            get { return mVanIP; }
        }
        private short mVanPort = 1515;
        public short VanPort
        {
            set { mVanPort = value; }
            get { return mVanPort; }
        }
        private string mInWCC = string.Empty;
        public string InWCC
        {
            set { mInWCC = value; }
            get { return mInWCC; }
        }
        private KisSpec mKisSpec;
        public KisSpec KisSpec
        {
            set { mKisSpec = value; }
            get { return mKisSpec; }
        }
        //private string mInTranCode = string.Empty;
        //public string InWCC
        //{
        //    set { mInWCC = value; }
        //    get { return mInWCC; }
        //}

        private int mStartSoundTick = 0;
        public int StartSoundTick
        {
            set { mStartSoundTick = value; }
            get { return mStartSoundTick; }
        }

        //public bool IsConnect(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        //{
        //    pAxKisPosAgent.Init();

        //    pAxKisPosAgent.inAgentIP = VanIP;

        //    pAxKisPosAgent.inAgentPort = VanPort;

        //    pAxKisPosAgent.inTranCode = "ST";

        //    pAxKisPosAgent.KIS_Approval();

        //    // 연결 상태 확인
        //    if (pAxKisPosAgent.outRtn == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        //public enum CardStates
        //{
        //    NONE,
        //    CardEmpty = 0,
        //    CardInsert = 1,
        //    UnKnownError

        //}
        //public CardStates GetCardState(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        //{
        //    try
        //    {
        //        short cardState = pAxKisPosAgent.KIS_CardCheck();
        //        CardStates currentCardSTate = CardStates.CardEmpty;

        //        switch (cardState)
        //        {
        //            case (short)CardStates.CardEmpty:
        //                currentCardSTate = CardStates.CardEmpty;
        //                break;
        //            case (short)CardStates.CardInsert:
        //                currentCardSTate = CardStates.CardInsert;
        //                break;
        //            default:
        //                currentCardSTate = CardStates.UnKnownError;
        //                break;
        //        }
        //        return currentCardSTate;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | GetCardState", ex.ToString());
        //        return CardStates.UnKnownError;
        //    }
        //}

        //public bool InitialLize(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        //{
        //    try
        //    {
        //        pAxKisPosAgent.Init();

        //        pAxKisPosAgent.inAgentIP = VanIP;

        //        pAxKisPosAgent.inAgentPort = VanPort;
        //        short returnData=pAxKisPosAgent.KIS_Cancel();
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | InitialLize", returnData.ToString());
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | InitialLize", ex.ToString());
        //        return false;
        //    }

        //}
        ///// <summary>
        ///// 승인 요청
        ///// </summary>
        ///// <param name="Amount"></param> 금액
        ///// <param name="InstallmentPlan"></param> 할부기간
        //public bool CardApproval(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent,string pAmount)
        //{
        //    try
        //    {
        //        pAxKisPosAgent.Init();

        //        pAxKisPosAgent.inAgentIP = VanIP;

        //        pAxKisPosAgent.inAgentPort = VanPort;

        //        // 거래구분
        //        pAxKisPosAgent.inTranCode = "D1";

        //        // 승인금액
        //        pAxKisPosAgent.inTranAmt = pAmount;

        //        pAxKisPosAgent.inInstallment = "00";
        //        int vatAmt = (Convert.ToInt32(pAmount) / 11);
        //        pAxKisPosAgent.inVatAmt = vatAmt.ToString();

        //        // 금액확인
        //        if (pAmount.Length > 0)
        //        {
        //            int nAmount = Convert.ToInt32(pAmount);

        //            // 5만원 이상
        //            if (nAmount > 50000)
        //            {
        //                pAxKisPosAgent.inSignYN = "Y";

        //                pAxKisPosAgent.inSignFileName = System.Environment.CurrentDirectory + "\\sign.bmp";
        //            }
        //            // 5만원 이하
        //            else
        //            {
        //                pAxKisPosAgent.inSignYN = "N";

        //                pAxKisPosAgent.inSignFileName = "";
        //            }
        //        }
        //        pAxKisPosAgent.inNonBlockEventYN = "Y";
        //        // 요청
        //        pAxKisPosAgent.KIS_Approval();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardApproval", ex.ToString());
        //        return false;
        //    }

        //}

        ///// <summary>
        ///// 취소 요청
        ///// </summary>
        ///// <param name="Amount"></param>    금액
        ///// <param name="AuthDate"></param>  승인일자 (형식 : YYMMDD)
        ///// <param name="AuthNo"></param>    승인번호 (12자리)
        ////public String SendData_D2(String Amount, String AuthDate, String AuthNo)
        ////{
        ////    GetPosOcxWnd().Init();

        ////    GetPosOcxWnd().inAgentIP = "127.0.0.1";

        ////    GetPosOcxWnd().inAgentPort = 1515;

        ////    // 거래구분
        ////    GetPosOcxWnd().inTranCode = "D2";

        ////    // 취소금액
        ////    GetPosOcxWnd().inTranAmt = Amount;

        ////    // 원승인일자 
        ////    GetPosOcxWnd().inOrgAuthDate = AuthDate;

        ////    // 원승인번호
        ////    GetPosOcxWnd().inOrgAuthNo = AuthNo;

        ////    // 금액확인
        ////    if (Amount.Length > 0)
        ////    {
        ////        int nAmount = Convert.ToInt32(Amount);

        ////        // 5만원 이상
        ////        if (nAmount >= 50000)
        ////        {
        ////            GetPosOcxWnd().inSignYN = "Y";

        ////            GetPosOcxWnd().inSignFileName = System.Environment.CurrentDirectory + "\\sign.bmp";
        ////        }
        ////        // 5만원 이하
        ////        else
        ////        {
        ////            GetPosOcxWnd().inSignYN = "N";

        ////            GetPosOcxWnd().inSignFileName = "";
        ////        }
        ////    }

        ////    // 요청
        ////    GetPosOcxWnd().KIS_Approval();

        ////    // 요청 성공
        ////    if (GetPosOcxWnd().outRtn == 0 && GetPosOcxWnd().outReplyCode == "0000")
        ////    {
        ////        return "0000";
        ////    }
        ////    else
        ////    {
        ////        return GetPosOcxWnd().outReplyCode;
        ////    }
        ////}

        //// <summary>
        //// 취소 요청
        //// </summary>
        //// <param name = "Amount" ></ param > 금액
        //// < param name="AuthDate"></param>  승인일자(형식 : YYMMDD)
        //// <param name = "AuthNo" ></ param > 승인번호(12자리)
        //public bool CardApprovalCancle(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent, string Amount, string AuthDate, string AuthNo)
        //{
        //    try
        //    {
        //        pAxKisPosAgent.Init();

        //        pAxKisPosAgent.inAgentIP = VanIP;

        //        pAxKisPosAgent.inAgentPort = VanPort;

        //        // 거래구분
        //        pAxKisPosAgent.inTranCode = "D2";

        //        // 취소금액
        //        pAxKisPosAgent.inTranAmt = Amount;

        //        // 원승인일자 
        //        pAxKisPosAgent.inOrgAuthDate = AuthDate;

        //        // 원승인번호
        //        pAxKisPosAgent.inOrgAuthNo = AuthNo;

        //        // 서명처리여부
        //        pAxKisPosAgent.inSignYN = "N";

        //        // 서명파일명
        //        pAxKisPosAgent.inSignFileName = "";

        //        // 비동기처리여부
        //        pAxKisPosAgent.inNonBlockEventYN = "Y";
        //        // 금액확인
        //        //if (Amount.Length > 0)
        //        //{
        //        //    int nAmount = Convert.ToInt32(Amount);

        //        //    // 5만원 이상
        //        //    if (nAmount >= 50000)
        //        //    {
        //        //        pAxKisPosAgent.inSignYN = "Y";

        //        //        pAxKisPosAgent.inSignFileName = System.Environment.CurrentDirectory + "\\sign.bmp";
        //        //    }
        //        //    // 5만원 이하
        //        //    else
        //        //    {
        //        //        pAxKisPosAgent.inSignYN = "N";

        //        //        pAxKisPosAgent.inSignFileName = "";
        //        //    }
        //        //}

        //        // 요청
        //        pAxKisPosAgent.KIS_Approval();
        //        return true;
        //        // 요청 성공
        //        //if (pAxKisPosAgent.outRtn == 0 && pAxKisPosAgent.outReplyCode == "0000")
        //        //{
        //        //    return true;
        //        //}
        //        //else
        //        //{
        //        //    return false;
        //        //}
        //    }
        //    catch(Exception ex)
        //    {
        //        TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | SendData_D2", ex.ToString());
        //        return false;
        //    }
        //}

        //KIS_TIT_DIP 삼성페이 결제 적용

        public delegate void CardFinish();
        public CardFinish Del_CardFinish;

        private string mCatID = string.Empty;
        #region 신규
        public KIS_TITDIPDevice()
        {
            KisSpec = new KisSpec();
            KisSpec.Init();
        }

        public bool IsConnect(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            pAxKisPosAgent.Init();

            pAxKisPosAgent.inAgentIP = VanIP;
            pAxKisPosAgent.inAgentPort = VanPort;

            pAxKisPosAgent.inTranCode = "TM";

            pAxKisPosAgent.KIS_Approval_Unit();

            // 연결 상태 확인
            if (pAxKisPosAgent.outRtn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool InitialLize(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                short returnData = pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;
                //pAxKisPosAgent.inAddressNo1 = "3";
                //pAxKisPosAgent.inAddressNo2 = "19200";
                pAxKisPosAgent.inUnitTimeOut = "3";
                pAxKisPosAgent.inTranCode = "TM";

                returnData = pAxKisPosAgent.KIS_Approval_Unit();
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | InitialLize", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | InitialLize", ex.ToString());
                return false;
            }

        }

        public bool StatusCheck(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "3";
                pAxKisPosAgent.inTranCode = "TS";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardStatusCheck", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | InitialLize", ex.ToString());
                return false;
            }

        }

        public bool PowerCheck(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "5";
                pAxKisPosAgent.inTranCode = "TP";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | PowerCheck", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | PowerCheck", ex.ToString());
                return false;
            }

        }

        public bool CardICRead(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent, string pAmont)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inTranAmt = pAmont;
                pAxKisPosAgent.inUnitTimeOut = "10";
                pAxKisPosAgent.inTranCode = "IC";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardICRead", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardICRead", ex.ToString());
                return false;
            }

        }

        public bool CardFBRead(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "10";
                pAxKisPosAgent.inTranCode = "FB";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardFullBack", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardFullBack", ex.ToString());
                return false;
            }

        }

        public bool CardLockEject(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "5";
                pAxKisPosAgent.inTranCode = "TL";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                string result = returnData == 0 ? "성공" : "실패";
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardLockEject", "카드방출명령 내림 결과 : " + result);
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardLockEject", ex.ToString());
                return false;
            }

        }

        public bool CardLockEjectFinish(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "5";
                pAxKisPosAgent.inTranCode = "TL";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardLockEjectFinish", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardLockEjectFinish", ex.ToString());
                return false;
            }

        }

        public bool StatusCheckFinish(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                pAxKisPosAgent.inUnitTimeOut = "5";
                pAxKisPosAgent.inTranCode = "TS";

                short returnData = pAxKisPosAgent.KIS_Approval_Unit();
                //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "KIS_TITDIPDevice | CardFullBack", returnData.ToString());
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardFullBack", ex.ToString());
                return false;
            }

        }

        ///// <summary>
        ///// 승인 요청
        ///// </summary>
        ///// <param name="Amount"></param> 금액
        ///// <param name="InstallmentPlan"></param> 할부기간
        public bool CardApproval(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent, string pAmount)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                //운영서버
                pAxKisPosAgent.inAddressNo1 = "210.112.100.63";
                pAxKisPosAgent.inAddressNo2 = "60201";
                pAxKisPosAgent.inTranCode = "NV";

                KisSpec.Init();
                KisSpec.CatID = NPSYS.gVanTerminalId;
                KisSpec.Installment = "00";
                KisSpec.TotAmt = pAmount;
                int vatAmt = (Convert.ToInt32(pAmount) / 11);
                KisSpec.VatAmt = vatAmt.ToString();
                KisSpec.SvcAmt = "0";
                KisSpec.TranCode = "D1";
                KisSpec.CardNo = string.Empty;
                //kisSpec.OrgAuthDate = string.Empty;
                //kisSpec.OrgAuthNo = string.Empty;
                KisSpec.WCC = InWCC;
                pAxKisPosAgent.inAgentData = KisSpec.MakeReqSpec();

                pAxKisPosAgent.KIS_Approval_Unit();
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardApproval", ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 승인 취소
        /// </summary>
        /// <param name="pAxKisPosAgent"></param>
        /// <param name="pAmount"></param>
        /// <param name="pAuthDate"></param>
        /// <param name="pAuthNo"></param>
        /// <returns></returns>
        public bool CardApprovalCancle(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent, string pAmount, string pAuthDate, string pAuthNo)
        {
            try
            {
                pAxKisPosAgent.Init();

                pAxKisPosAgent.inAgentIP = VanIP;
                pAxKisPosAgent.inAgentPort = VanPort;

                //운영서버
                pAxKisPosAgent.inAddressNo1 = "210.112.100.63";
                pAxKisPosAgent.inAddressNo2 = "60201";
                pAxKisPosAgent.inTranCode = "NV";

                KisSpec.Init();
                KisSpec.CatID = NPSYS.gVanTerminalId;
                KisSpec.Installment = "00";
                KisSpec.TotAmt = pAmount;
                //int vatAmt = (Convert.ToInt32(pAmount) / 11);
                //kisSpec.VatAmt = vatAmt.ToString();
                KisSpec.SvcAmt = "0";
                KisSpec.TranCode = "D2";
                KisSpec.CardNo = string.Empty;
                KisSpec.OrgAuthDate = pAuthDate;
                KisSpec.OrgAuthNo = pAuthNo;
                KisSpec.WCC = InWCC;
                pAxKisPosAgent.inAgentData = KisSpec.MakeReqSpec();

                pAxKisPosAgent.KIS_Approval_Unit();
                return true;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | CardApproval", ex.ToString());
                return false;
            }
        }

        //public KisSpec ResultDataSet(AxKisPosAgentLib.AxKisPosAgent pAxKisPosAgent)
        //{
        //    KisSpec kisSpec = new KisSpec();
        //    return kisSpec.GetResSpec(pAxKisPosAgent.outAgentData);
        //}
        #endregion

    }
    public class KisSpec
    {
        public string TranCode;
        public string PosNo;
        public string TradeSerialNumber;
        public string WCC;
        public string SafeCardICData;
        public string SafeCardMSData;
        public string CardNo;
        public string Installment;
        public string TotAmt;
        public string CatID;
        public string VatAmt;
        public string SvcAmt;
        public string DeviceAuthValue;
        public string VanKey;

        public string OrgAuthNo;
        public string OrgAuthDate;

        public string outTranNo;
        public string outTradeReqDate;
        public string outTradeReqTime;
        public string outReplyCode;
        public string outJanAmt;
        public string outAccepterCode;
        public string outAccepterName;
        public string outAuthNo;
        public string outIssuerCode;
        public string outIssuerName;
        public string outMerchantRegNo;
        public string outReplyMsg1;
        public string outReplyDate;
        public string outEMVData;
        public string outVanKey;

        public KisSpec()
        {
            Init();
        }

        public void Init()
        {
            TranCode = "";
            PosNo = "";
            TradeSerialNumber = "";
            WCC = "";
            SafeCardICData = "";
            SafeCardMSData = "";
            CardNo = "";
            Installment = "";
            TotAmt = "";
            CatID = "";
            VatAmt = "";
            SvcAmt = "";
            DeviceAuthValue = "";
            VanKey = "";

            OrgAuthNo = "";
            OrgAuthDate = "";

            outTranNo = "";
            outTradeReqDate = "";
            outTradeReqTime = "";
            outReplyCode = "";
            outJanAmt = "";
            outAccepterCode = "";
            outAccepterName = "";
            outAuthNo = "";
            outIssuerCode = "";
            outIssuerName = "";
            outMerchantRegNo = "";
            outReplyMsg1 = "";
            outReplyDate = "";
            outEMVData = "";
            outVanKey = "";
        }

        public string MakeReqSpec()
        {
            char STX = (char)0x02;
            char CR = (char)0x0D;
            char ETX = (char)0x03;
            string reqSpec = "";

            if (TranCode.Equals("D1"))
            {
                //STX
                reqSpec += STX.ToString();
                //전문구분
                reqSpec += TranCode.Substring(0, 2);
                //POS번호
                reqSpec += PosNo.PadLeft(3, ' ');
                //일련번호
                reqSpec += TradeSerialNumber.PadLeft(6, '0');
                //거래요청일시
                reqSpec += GetTime_YYYYMMDDhhmmss();
                //WCC
                reqSpec += WCC.PadLeft(1, ' ');
                //카드데이터
                if (CardNo == "")
                {
                    // VanID(2)
                    reqSpec += "09";
                    // 암호화 정보 (96)
                    reqSpec += "[##QS][00][0096]";
                    // 생성된 MAC (8)
                    reqSpec += "[##QS][01][0008]";
                    // 단말기 ID (10)
                    reqSpec += "[##QS][09][0010]";
                    // KSN (20)
                    reqSpec += "[##QS][02][0020]";
                    // TPL (3)
                    reqSpec += "   ";
                    // DIKn 일련번호 (16)
                    reqSpec += "                ";
                    // PMK 유효기간 (6)
                    reqSpec += "[##QS][03][0006]";
                }
                else
                {
                    // VanID(2)
                    reqSpec += "09";
                    //카드번호
                    reqSpec += CardNo.PadRight(159, ' ');
                }
                //할부개월
                reqSpec += Installment.PadLeft(2, '0');
                //거래금액
                reqSpec += TotAmt.PadLeft(8, '0');
                //단말기번호
                reqSpec += CatID.PadRight(10, ' ');
                //Filler
                reqSpec += "".PadLeft(28, ' ');
                //US
                reqSpec += "US";
                //부가세
                reqSpec += VatAmt.PadLeft(8, '0');
                //봉사료
                reqSpec += SvcAmt.PadLeft(8, '0');
                //Filler
                reqSpec += "".PadLeft(284, ' ');

                //하드코딩값
                reqSpec += "#SK";

                //리더기식별번호
                reqSpec += "[##QS][06][0016]";

                //POS식별번호
                reqSpec += "[##QS][07][0016]";

                //Fallback사유
                reqSpec += "[##QS][08][0002]";

                //하드코딩값
                reqSpec += "#E";

                //카드구분자
                reqSpec += "[##QS][04][0004]";

                //EMVData
                reqSpec += "[##QS][05][0257]";

                //VANKEY
                reqSpec += VanKey.PadLeft(16, ' ');
                //CR
                reqSpec += CR.ToString();
                //ETX
                reqSpec += ETX.ToString();
            }
            else if (TranCode.Equals("D2"))
            {
                //STX
                reqSpec += STX.ToString();
                //전문구분
                reqSpec += TranCode.Substring(0, 2);
                //POS번호
                reqSpec += PosNo.PadLeft(3, ' ');
                //일련번호
                reqSpec += TradeSerialNumber.PadLeft(6, '0');
                //거래요청일시
                reqSpec += GetTime_YYYYMMDDhhmmss();
                //WCC
                reqSpec += WCC.PadLeft(1, ' ');
                //카드데이터
                if (CardNo == "")
                {
                    // VanID(2)
                    reqSpec += "09";
                    // 암호화 정보 (96)
                    reqSpec += "[##QS][00][0096]";
                    // 생성된 MAC (8)
                    reqSpec += "[##QS][01][0008]";
                    // 단말기 ID (10)
                    reqSpec += "[##QS][09][0010]";
                    // KSN (20)
                    reqSpec += "[##QS][02][0020]";
                    // TPL (3)
                    reqSpec += "   ";
                    // DIKn 일련번호 (16)
                    reqSpec += "                ";
                    // PMK 유효기간 (6)
                    reqSpec += "[##QS][03][0006]";
                }
                else
                {
                    // VanID(2)
                    reqSpec += "09";
                    //카드번호
                    reqSpec += CardNo.PadRight(159, ' ');
                }
                //할부개월
                reqSpec += Installment.PadLeft(2, '0');
                //거래금액
                reqSpec += TotAmt.PadLeft(8, '0');
                //단말기번호
                reqSpec += CatID.PadRight(10, ' ');
                //원승인 번호
                reqSpec += OrgAuthNo.PadRight(8, ' ');
                //원승인 일자
                reqSpec += OrgAuthDate.PadRight(6, ' ');
                //Filler
                reqSpec += "".PadLeft(14, ' ');
                //US
                reqSpec += "US";
                //부가세
                reqSpec += VatAmt.PadLeft(8, '0');
                //봉사료
                reqSpec += SvcAmt.PadLeft(8, '0');
                //Filler
                reqSpec += "".PadLeft(284, ' ');

                //하드코딩값
                reqSpec += "#SK";

                //리더기식별번호
                reqSpec += "[##QS][06][0016]";

                //POS식별번호
                reqSpec += "[##QS][07][0016]";

                //Fallback사유
                reqSpec += "[##QS][08][0002]";

                //하드코딩값
                reqSpec += "#E";

                //카드구분자
                reqSpec += "[##QS][04][0004]";

                //EMVData
                reqSpec += "[##QS][05][0257]";

                //VANKEY
                reqSpec += VanKey.PadLeft(16, ' ');
                //CR
                reqSpec += CR.ToString();
                //ETX
                reqSpec += ETX.ToString();
            }
            if ((TranCode == "H1") || (TranCode == "H2"))
            {
                //STX
                reqSpec += STX.ToString();
                //전문구분
                reqSpec += TranCode.Substring(0, 2);
                //POS번호
                reqSpec += PosNo.PadLeft(3, ' ');
                //일련번호
                reqSpec += TradeSerialNumber.PadLeft(6, '0');
                //거래요청일시
                reqSpec += GetTime_YYYYMMDDhhmmss();
                //WCC
                reqSpec += WCC.PadLeft(1, ' ');
                //카드데이터
                if (CardNo == "")
                {
                    // VanID(2)
                    reqSpec += "09";
                    // 암호화 정보 (96)
                    reqSpec += "[##QS][00][0096]";
                    // 생성된 MAC (8)
                    reqSpec += "[##QS][01][0008]";
                    // 단말기 ID (10)
                    reqSpec += "[##QS][09][0010]";
                    // KSN (20)
                    reqSpec += "[##QS][02][0020]";
                    // TPL (3)
                    reqSpec += "   ";
                    // DIKn 일련번호 (16)
                    reqSpec += "                ";
                    // PMK 유효기간 (6)
                    reqSpec += "[##QS][03][0006]";
                }
                else
                {
                    // VanID(2)
                    reqSpec += "09";
                    //카드번호
                    reqSpec += CardNo.PadRight(159, ' ');
                }

                //할부개월
                reqSpec += Installment.PadLeft(2, '0');
                //거래금액
                reqSpec += TotAmt.PadLeft(8, '0');
                //단말기번호
                reqSpec += CatID.PadRight(10, ' ');

                // 세금
                reqSpec += VatAmt.PadLeft(8, '0');

                // 셋
                reqSpec += "US";

                // filler
                reqSpec += "".PadLeft(4, ' ');

                // 봉사료
                reqSpec += SvcAmt.PadLeft(8, '0');

                if (TranCode == "H1")
                {
                    // filler
                    reqSpec += "".PadLeft(72, ' ');
                }
                else
                {
                    //원승인 번호
                    reqSpec += OrgAuthNo.PadRight(8, ' ');
                    //원승인 일자
                    reqSpec += OrgAuthDate.PadRight(6, ' ');

                    // 취소사유
                    reqSpec += "1";

                    // filler
                    reqSpec += "".PadLeft(56, ' ');
                }
                //CR
                reqSpec += CR.ToString();
                //ETX
                reqSpec += ETX.ToString();
            }

            return reqSpec;
        }

        public int GetResSpec(string resSpec)
        {
            int nRet = -1;
            try
            {
                byte[] tmpResSpec = Encoding.Default.GetBytes(resSpec);
                if ((TranCode == "D1") || (TranCode == "D2"))
                {
                    //응답코드
                    outReplyCode = getByteString(resSpec, 26, 4);
                    if (resSpec.Length < 125)
                    {
                        outReplyMsg1 = resSpec.Substring(118, 6);
                        return nRet;
                    }
                    //거래일련번호
                    outTranNo = getByteString(resSpec, 6, 6);

                    //거래요청날짜
                    outTradeReqDate = getByteString(resSpec, 12, 8);

                    //거래요청시간
                    outTradeReqTime = getByteString(resSpec, 20, 6);

                    // GIFT카드시 잔액
                    outJanAmt = getByteString(resSpec, 30, 6);

                    // 매입사코드
                    outAccepterCode = getByteString(resSpec, 36, 2);

                    // 매입사명
                    outAccepterName = getByteString(resSpec, 98, 20);

                    // 승인번호
                    outAuthNo = getByteString(resSpec, 70, 8);

                    // 발급사코드
                    outIssuerCode = getByteString(resSpec, 38, 2);

                    // 발급사명
                    outIssuerName = getByteString(resSpec, 78, 20);

                    // 가맹점번호
                    outMerchantRegNo = getByteString(resSpec, 40, 20);

                    // 메세지
                    outReplyMsg1 = getByteString(resSpec, 118, 40);

                    // VAN승인일자
                    outReplyDate = getByteString(resSpec, 163, 8);

                    nRet = 0;
                }
                else if ((TranCode == "H1") || (TranCode == "H2"))
                {
                    //거래일련번호
                    outTranNo = getByteString(resSpec, 6, 6);

                    //거래요청날짜
                    outTradeReqDate = getByteString(resSpec, 12, 8);

                    //거래요청시간
                    outTradeReqTime = getByteString(resSpec, 20, 6);

                    //응답코드
                    outReplyCode = getByteString(resSpec, 26, 4);

                    // 승인번호
                    outAuthNo = "09" + getByteString(resSpec, 70, 8);

                    // 가맹점번호
                    outMerchantRegNo = getByteString(resSpec, 40, 20);

                    // 메세지
                    outReplyMsg1 = getByteString(resSpec, 98, 60);

                    // VAN승인일자
                    outReplyDate = getByteString(resSpec, 163, 8);

                    nRet = 0;
                }

                return nRet;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "KIS_TITDIPDevice | GetResSpec", ex.ToString());
                return nRet;
            }
        }

        private string GetTime_YYYYMMDDhhmmss()
        {
            DateTime CurDate = DateTime.Now;
            string tmpDate = "";

            tmpDate += CurDate.Year.ToString().PadLeft(4);
            tmpDate += CurDate.Month.ToString().PadLeft(2, '0');
            tmpDate += CurDate.Day.ToString().PadLeft(2, '0');
            tmpDate += CurDate.Hour.ToString().PadLeft(2, '0');
            tmpDate += CurDate.Minute.ToString().PadLeft(2, '0');
            tmpDate += CurDate.Second.ToString().PadLeft(2, '0');

            return tmpDate;
        }

        public static String getByteString(String s, int startIdx, int bytes)
        {
            byte[] tmpResSpec = Encoding.Default.GetBytes(s);
            return Encoding.Default.GetString(tmpResSpec, startIdx, bytes);
        }
    }
}
