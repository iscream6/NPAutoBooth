using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.Van;
using System;
using System.Threading;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    partial class FormCreditPaymentMenu
    {
        public bool SetKisDipIFM()
        {
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                timerKisCardPay.Enabled = true;
                if (mCurrentNormalCarInfo.PaymentMoney > 0)
                {
                    NPSYS.Device.KisPosAgent.OnApprovalEnd += new EventHandler(KisPosAgent_OnApprovalEnd);
                    //btnCardAction.Visible = true;
                    paymentControl.ErrorMessage = string.Empty;
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    mKIS_TITDIPDevice.VanIP = NPSYS.gVanIp;
                    mKIS_TITDIPDevice.VanPort = Convert.ToInt16(NPSYS.gVanPort);
                    mKIS_TITDIPDevice.InWCC = "C";
                    mKIS_TITDIPDevice.InitialLize(NPSYS.Device.KisPosAgent);

                    TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | SetKisDipIFM", "[KIS_TIT_DIP 신용카드요금결제시작]true");
                }
            }
            return true;
        }

        public bool UnSetKisDipIFM()
        {
            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
            {
                mKIS_TITDIPDevice.CardLockEject(NPSYS.Device.KisPosAgent);
                NPSYS.Device.KisPosAgent.OnApprovalEnd -= new EventHandler(KisPosAgent_OnApprovalEnd);
            }
            return true;
        }

        private void DoWork(AxKisPosAgentLib.AxKisPosAgent pKisPosAgent)
        {
            System.Threading.Thread.Sleep(200);
            switch (mCardStatus.currentCardReaderStatus)
            {
                case CardDeviceStatus.CardReaderStatus.None:
                    mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[최초 카드상태 체크]");
                    break;

                case CardDeviceStatus.CardReaderStatus.CardReady:
                    if (string.IsNullOrEmpty(pKisPosAgent.outAgentData))
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[outAgentData 데이터 없음]");
                        break;
                    }
                    if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "1")
                    {
                        paymentControl.ErrorMessage = "카드가 삽입되었습니다.";
                        mKIS_TITDIPDevice.PowerCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPowerCheck;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드 삽입됨]");
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "0")
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "C")
                    {
                        paymentControl.ErrorMessage = "카드[MST] 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.InWCC = "S";
                        mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[삼성페이 읽기 진행]");
                    }
                    //else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC != "F")
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    //}
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "00" && pKisPosAgent.outAgentData.Substring(3, 1) == "1" && mKIS_TITDIPDevice.InWCC == "F")
                    {
                        paymentControl.ErrorMessage = "카드[MS] 데이터를 읽는 중 입니다...";
                        mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[MS데이터 읽기 진행]");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "카드 삽입해 주시기 바랍니다...";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                    }
                    break;

                case CardDeviceStatus.CardReaderStatus.CardPowerCheck:
                    //if (pKisPosAgent.outAgentData.Trim() == "1")
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    //    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mNormalCarInfo.PaymentMoney.ToString());
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    //    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    //}
                    //else
                    //{
                    //    paymentControl.ErrorMessage = "카드[IC] 확인 실패 되었습니다.";

                    //    //mKIS_TITDIPDevice.InWCC = "F";
                    //    mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    //}
                    paymentControl.ErrorMessage = "카드[IC] 데이터를 읽는 중 입니다...";
                    mKIS_TITDIPDevice.CardICRead(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReadyEnd;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC데이터 읽기 진행]");
                    break;

                case CardDeviceStatus.CardReaderStatus.CardLockEject:
                    if (pKisPosAgent.outAgentData.Trim() == "1")
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.StatusCheck(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "카드를 천천히 뽑아주세요.";
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                    }
                    break;

                case CardDeviceStatus.CardReaderStatus.CardReadyEnd:
                    if (pKisPosAgent.outAgentData.Trim() == "CF")
                    {
                        //paymentControl.ErrorMessage = "카드[MS] 데이터를 읽는 중 입니다.";
                        mKIS_TITDIPDevice.InWCC = "F";
                        //mKIS_TITDIPDevice.CardFBRead(pKisPosAgent);
                        //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardFullBack;
                        //TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[MS데이터 읽기 진행]");
                        paymentControl.ErrorMessage = "카드[IC] 리딩 실패";
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[IC 카드 리딩 실패]");
                    }
                    else if (pKisPosAgent.outAgentData.Trim().Length == 2)
                    {
                        paymentControl.ErrorMessage = "카드결제 실패";
                        //mKIS_TITDIPDevice.CardLockEjectFinish(pKisPosAgent);
                        mKIS_TITDIPDevice.CardLockEject(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드리딩 실패]");
                    }
                    else
                    {
                        if (pKisPosAgent.outAgentData.Substring(7, 1).Equals("M"))
                        {
                            mKIS_TITDIPDevice.InWCC = "S";
                        }
                        paymentControl.ErrorMessage = "승인 진행 중입니다.";
                        //payData.outReaderData = axKisPosAgent1.outAgentData.Substring(0, 6);
                        if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                           || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                        {
                            mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString(), mCurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), mCurrentNormalCarInfo.VanRegNo_Cancle);
                        }
                        else
                        {
                            mKIS_TITDIPDevice.CardApproval(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                        }
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | DoWork", "[카드 결제요청 진행]");
                    }
                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(false);
                    }
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);

                    break;

                case CardDeviceStatus.CardReaderStatus.CardFullBack:
                    paymentControl.ErrorMessage = "카드[MS] 결제 요청중 입니다.";
                    if (mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || mCurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        mKIS_TITDIPDevice.CardApprovalCancle(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString(), mCurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), mCurrentNormalCarInfo.VanRegNo_Cancle);
                    }
                    else
                    {
                        mKIS_TITDIPDevice.CardApproval(pKisPosAgent, mCurrentNormalCarInfo.PaymentMoney.ToString());
                    }

                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                    break;

                case CardDeviceStatus.CardReaderStatus.CardApproval:
                    KisPosAgent_OnAgtComplete(pKisPosAgent);
                    break;

                case CardDeviceStatus.CardReaderStatus.CardLockEjectFinish:
                    paymentControl.ErrorMessage = "카드를 뽑아주세요.";
                    mKIS_TITDIPDevice.StatusCheckFinish(pKisPosAgent);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish;
                    //PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardNotEject);
                    break;

                case CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish:
                    paymentControl.ErrorMessage = "카드를 뽑아 주세요";
                    if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "0")
                    {
                        mKIS_TITDIPDevice.StatusCheckFinish(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStatusCheckFinish;
                    }
                    else if (pKisPosAgent.outAgentData.Substring(0, 2) == "11" && pKisPosAgent.outAgentData.Substring(13, 1) == "1")
                    {
                        mKIS_TITDIPDevice.CardLockEjectFinish(pKisPosAgent);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEjectFinish;
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "결제가 완료되었습니다";
                    }
                    PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardEject); //일부러주석처리
                    break;
            }

            Thread.Sleep(200);
        }

        private void KisPosAgent_OnAgtComplete(AxKisPosAgentLib.AxKisPosAgent pKisPosAgent)
        {
            try
            {
                mKIS_TITDIPDevice.KisSpec.GetResSpec(pKisPosAgent.outAgentData);

                // 리턴값:0 성공유무:True 응답코드:0000 단밀기번호:90100546 카드번호:541707 할부개월:   결제금액:40000    부가세액:         승인번호:90057260      거래일자:20161027 매입사코드:15 매입사명:하나카드             발급사코드:15 발급사명:하나카드             가맹점번호:8100000637           거래일련번호:       메시지1:승    인                                 메시지1:
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | KisPosAgent_OnAgtComplete", "[KISAGENT 응답결과]"
                                        + " 리턴값:" + pKisPosAgent.outRtn.ToString()
                                        + " 성공유무:" + (mKIS_TITDIPDevice.KisSpec.outReplyCode == "0000" ? true : false).ToString()
                                        + " 응답코드:" + mKIS_TITDIPDevice.KisSpec.outReplyCode
                                        + " 단밀기번호:" + mKIS_TITDIPDevice.KisSpec.CatID
                                        + " 카드번호:" + pKisPosAgent.outCardNo
                                        + " 할부개월:" + pKisPosAgent.outInstallment
                                        + " 결제금액:" + mKIS_TITDIPDevice.KisSpec.TotAmt
                                        + " 부가세액:" + mKIS_TITDIPDevice.KisSpec.VatAmt
                                        + " 승인번호:" + mKIS_TITDIPDevice.KisSpec.outAuthNo
                                        + " 거래일자:" + mKIS_TITDIPDevice.KisSpec.outReplyDate
                                        + " 매입사코드:" + mKIS_TITDIPDevice.KisSpec.outAccepterCode
                                        + " 매입사명:" + mKIS_TITDIPDevice.KisSpec.outAccepterName
                                        + " 발급사코드:" + mKIS_TITDIPDevice.KisSpec.outIssuerCode
                                        + " 발급사명:" + mKIS_TITDIPDevice.KisSpec.outIssuerName
                                        + " 가맹점번호:" + mKIS_TITDIPDevice.KisSpec.outMerchantRegNo
                                        + " 거래일련번호:" + mKIS_TITDIPDevice.KisSpec.outTranNo
                                        + " 메시지1:" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1);
                //+ " 메시지1:" + mKisSpec.outReplyMsg2);

                if (pKisPosAgent.outRtn == 0 && mKIS_TITDIPDevice.KisSpec.outReplyCode == "0000") // 카드결제성공
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | KisPosAgent_OnAgtComplete", "[카드결제 성공]");
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //string[] lCardNumData = mKisSpec.outCardNo.Split('=');
                    //if (lCardNumData[0].Length > 13)
                    //{
                    //    mCurrentNormalCarInfo.CardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    //}
                    //else
                    //{
                    //    mCurrentNormalCarInfo.CardNumber = lCardNumData[0];
                    //}
                    mCurrentNormalCarInfo.VanCardNumber = pKisPosAgent.outCardNo.Trim();
                    mCurrentNormalCarInfo.VanRegNo = mKIS_TITDIPDevice.KisSpec.outAuthNo.Trim();
                    mCurrentNormalCarInfo.VanDate = mKIS_TITDIPDevice.KisSpec.outReplyDate;
                    mCurrentNormalCarInfo.VanRescode = mKIS_TITDIPDevice.KisSpec.outReplyCode;
                    mCurrentNormalCarInfo.VanResMsg = mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    if (mKIS_TITDIPDevice.KisSpec.VatAmt.Trim() == string.Empty)
                    {
                        mKIS_TITDIPDevice.KisSpec.VatAmt = "0";
                    }
                    //mNormalCarInfo.SupplyPay = (Convert.ToInt32(mKisSpec.outTranAmt) - Convert.ToInt32(mKisSpec.outVatAmt));
                    mCurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(mKIS_TITDIPDevice.KisSpec.VatAmt);
                    mCurrentNormalCarInfo.VanSupplyPay = 0;
                    mCurrentNormalCarInfo.VanCardName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    mCurrentNormalCarInfo.VanBeforeCardPay = mCurrentNormalCarInfo.PaymentMoney;
                    mCurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    mCurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                    mCurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mKIS_TITDIPDevice.KisSpec.outReplyDate);
                    mCurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                    mCurrentNormalCarInfo.VanIssueCode = mKIS_TITDIPDevice.KisSpec.outIssuerCode;
                    mCurrentNormalCarInfo.VanIssueName = mKIS_TITDIPDevice.KisSpec.outIssuerName;
                    mCurrentNormalCarInfo.VanCardAcquirerCode = mKIS_TITDIPDevice.KisSpec.outAccepterCode;
                    mCurrentNormalCarInfo.VanCardAcquirerName = mKIS_TITDIPDevice.KisSpec.outAccepterName;

                    mCurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(mCurrentNormalCarInfo);
                    mCurrentNormalCarInfo.VanAmt = mCurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney) + "원";
                    paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc) + "원";
                    mKIS_TITDIPDevice.CardLockEjectFinish(NPSYS.Device.KisPosAgent);
                }
                else if (mKIS_TITDIPDevice.KisSpec.outReplyCode == "8100")// 사용자취소
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    //KIS 할인처리시 처리문제
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess;
                    //응답코드:8000 타입아웃
                    //응답코드:8326 한도초과
                    //KIS 할인처리시 처리문제주석완료

                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(true);
                    }
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    mKIS_TITDIPDevice.InWCC = mKIS_TITDIPDevice.InWCC != "C" ? "C" : mKIS_TITDIPDevice.InWCC;
                    DoWork(pKisPosAgent);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패 사용자취소진행완료" + mKIS_TITDIPDevice.KisSpec.outReplyCode);
                    return;
                }
                else // 카드결제실패
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    if (!NPSYS.gIsAutoBooth)
                    {
                        paymentControl.SetPageChangeButtonVisible(true);
                    }
                    //timerKis_TIT_DIP_IFM.Enabled = true;
                    //timerKis_TIT_DIP_IFM.Start();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | KisPosAgent_OnAgtComplete", "카드 결제실패" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1);
                    paymentControl.ErrorMessage = "카드결제실패:" + mKIS_TITDIPDevice.KisSpec.outReplyMsg1;
                    Application.DoEvents();
                    PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);

                    Thread.Sleep(2000);
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                    mKIS_TITDIPDevice.InWCC = mKIS_TITDIPDevice.InWCC != "C" ? "C" : mKIS_TITDIPDevice.InWCC;
                    DoWork(pKisPosAgent);
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | KisPosAgent_OnAgtComplete", ex.ToString());
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardLockEject;
                DoWork(pKisPosAgent);
            }
        }

        ////KIS 할인처리시 처리문제주석완료
        private void KisPosAgent_OnApprovalEnd(object sender, EventArgs e)
        {
            if (NPSYS.Device.KisPosAgent.outRtn != 0) return;
            else DoWork(NPSYS.Device.KisPosAgent);
        }
    }
}