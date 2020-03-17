using FadeFox;
using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.Van;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// SMARTRO 처리
    /// </summary>
    partial class FormCreditPaymentMenu
    {
        #region SMATRO_TIT_DIP_EVCAT

        private void SmatroEVCAT_CardApprovalAction()
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess
                || CurrentNormalCarInfo.VanAmt != 0 || CurrentNormalCarInfo.GetInComeMoney > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (CurrentNormalCarInfo.GetInComeMoney == 0)
            {
                paymentControl.ErrorMessage = string.Empty;
                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[승인쓰레드웨이트]");
                    mCreditCardThreadLock.WaitOne(1000);
                    mCreditCardThreadLock.Reset();
                    if (CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_OutCar
                       || CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.RemoteCancleCard_PreCar)
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 신용카드 취소결제 시작]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                        mSmartro_TITDIP_EVCat.CanclePayment(NPSYS.Device.Smartro_TITDIP_Evcat, CurrentNormalCarInfo.PaymentMoney.ToString(), CurrentNormalCarInfo.VanDate_Cancle.Replace("-", "").Substring(2, 6), CurrentNormalCarInfo.VanRegNo_Cancle);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 신용카드 취소결제 시작]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드요금결제요청 시작]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                        mSmartro_TITDIP_EVCat.CardApproval(NPSYS.Device.Smartro_TITDIP_Evcat, CurrentNormalCarInfo.PaymentMoney.ToString());
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드요금결제요청 종료]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    }
                    mCreditCardThreadLock.Set();
                }
            }
        }

        public void UnsetSmatro_DIPTIT_Evcat()
        {
            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu-CAT || UnsetSmatro_DIPTIT_Evcat ", "스마트로 DIP 기존요금취소 및 카드배출처리");
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                mSmartro_TITDIP_EVCat.CardReaderReset(NPSYS.Device.Smartro_TITDIP_Evcat);
                DateTime startDate = DateTime.Now;
                while (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop)
                {
                    TimeSpan diff = DateTime.Now - startDate;
                    System.Windows.Forms.Application.DoEvents();
                    Thread.Sleep(100);

                    if (Convert.ToInt32(diff.TotalMilliseconds) > 3000)
                    {
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormCreditPaymentMenu | SettingEnableDevice", "[상태요청 시간초과로 카드요금초기화 한걸로 처리 ");
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReset;
                    }
                }
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
            }
            else
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu-CAT || UnsetSmatro_DIPTIT_Evcat ", "스마트로 DIP 기존요금취소 및 카드배출처리안함[이유:요금요청이 이전에 없음]");
            }
        }

        private void SmartroEvcat_QueryResults(object sender, AxDreampos_Ocx.__DP_Certification_Ocx_QueryResultsEvent e)
        {
            try
            {
                if (NPSYS.CurrentFormType != mCurrentFormType)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SmartroEvcat_QueryResults", "[요금폼이 아니라 실행안함 스마트로 최초 응답전문]" + e.returnData);
                    return;
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | SmartroEvcat_QueryResults", "[스마트로 최초 응답전문]" + e.returnData + " 폼타입:" + this.mCurrentFormType.ToString());
                //if (e.returnData == "FALL BACK 발생" || e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                //{
                //    paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요");
                //    TextCore.ACTION(TextCore.ACTIONS.USER, "SMATRO_TIT_DIP_EV-CAT || ParsingData", " 이벤트 [" + e.returnData + "] ");
                //    return;
                //}
                if (e.returnData == "FALL BACK 발생")
                {
                    paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                    return;
                }
                if (e.returnData == "리더기응답" || e.returnData == "GTFBIN")
                {
                    return;
                }
                if (e.returnData.Contains("이미 요청중입니다!"))
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    return;
                }

                if (string.IsNullOrEmpty(e.returnData))
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 응답값 없어 처리를 안함 ");
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "[수신 전문 파싱 시작] " + e.returnData);
                if (e.returnData.Contains(((char)7).ToString()) && e.returnData.Contains(((char)3).ToString()))
                {
                    string recvData = e.returnData.Substring(e.returnData.IndexOf((char)7) + 1, e.returnData.IndexOf((char)3) - e.returnData.IndexOf((char)7) - 1);
                    string[] splitData = Regex.Split(recvData, ((char)6).ToString());

                    mSmartro_TITDIP_EVCat.RecvInfo.ClearRecvData();
                    mSmartro_TITDIP_EVCat.RecvInfo.rUserFixNo = splitData[0];
                    mSmartro_TITDIP_EVCat.RecvInfo.rResultCode = splitData[1];
                    mSmartro_TITDIP_EVCat.RecvInfo.rProcGuBun = splitData[2];
                    mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg = splitData[3];

                    if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "C")
                    {
                        if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "LIVE")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults LIVE", " EV-CAT 데몬 정상실행 확인 ");
                            return;
                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "00")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults 00", " 리더기 정상연결 " + CardDeviceStatus.CardReaderStatus.None.ToString());
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        }
                        else if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg == "RESET")
                        {
                            mSmartro_TITDIP_EVCat.isConnect = true;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults RESET", " 카드리더기 리셋(강제배출) 성공 " + CardDeviceStatus.CardReaderStatus.CardReset.ToString());
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReset;

                            return;
                        }
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "리더기 상태 " + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                        return;
                    }
                    if ((mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "N")
                        && mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg.Length >= 18)
                    {
                        mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);

                        if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "Y")
                        {
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 성공] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 승인번호 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo + "] "
                                + " 승인일시 [" + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate + " " + mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime + "] "
                                + " 매입사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName + "] "
                                + " 발급사명 [" + mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName + "] "
                                + " 결제금액 [" + mSmartro_TITDIP_EVCat.RecvInfo.rPayMoney + "] ");

                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                            paymentControl.ErrorMessage = "결제가성공하였습니다";
                            string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, CurrentNormalCarInfo, MoneyType.CreditCard, CurrentNormalCarInfo.PaymentMoney, 0, "");

                            CurrentNormalCarInfo.VanCheck = 1;
                            CurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            CurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                            CurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                            CurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                            CurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                            CurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                            CurrentNormalCarInfo.VanSupplyPay = 0;
                            CurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(mSmartro_TITDIP_EVCat.RecvInfo.rVat);
                            CurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            CurrentNormalCarInfo.VanBeforeCardPay = CurrentNormalCarInfo.PaymentMoney;
                            CurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            CurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                            CurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            CurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                            //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                            CurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                            CurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            CurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                            CurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                            //만료차량 정기권요금제에서 일반요금제 변경기능주석완료

                            LPRDbSelect.Creditcard_Log_INsert(CurrentNormalCarInfo);
                            CurrentNormalCarInfo.VanAmt = CurrentNormalCarInfo.PaymentMoney;
                            TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제성공");

                            paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney) + "원";
                            paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc) + "원";
                        }
                        else
                        {
                            //카드실패전송

                            bool _isSuccessCreditSmartroParsing = mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);
                            if (_isSuccessCreditSmartroParsing)
                            {
                                CurrentNormalCarInfo.VanCheck = 2;
                                CurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                                CurrentNormalCarInfo.VanCardFullNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                                CurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                                CurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                                CurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                                CurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                                CurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                                CurrentNormalCarInfo.VanSupplyPay = 0;
                                CurrentNormalCarInfo.VanTaxPay = 0;
                                CurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                                CurrentNormalCarInfo.VanBeforeCardPay = 0;
                                CurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                                CurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                                CurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                                CurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                                //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                                CurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                                CurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                                CurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                                CurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                                if (NPSYS.gUseCardFailSend)
                                {
                                    DateTime paydate = DateTime.Now;
                                    //카드실패전송
                                    CurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                                    //카드실패전송 완료
                                    Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                                }
                            }
                            //카드실패전송 완료
                            TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 실패] "
                                + " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                                + " 실패사유 [" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg + "] ");

                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                            //if (!NPSYS.gIsAutoBooth)
                            //{
                            //    if (btn_PrePage.Visible == false)
                            //    {
                            //        btn_PrePage.Visible = true;
                            //    }
                            //    if (btn_home.Visible == false)
                            //    {
                            //        btn_home.Visible = true;
                            //    }
                            //}
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제실패" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                            paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                            PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                            mSmartro_TITDIP_EVCat.StartSoundTick = 7;
                        }
                    }
                    else if (mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "E" || mSmartro_TITDIP_EVCat.RecvInfo.rResultCode == "N")
                    {
                        //카드실패전송

                        bool _isSuccessCreditSmartroParsing = mSmartro_TITDIP_EVCat.ParsingData(splitData[3]);
                        if (_isSuccessCreditSmartroParsing)
                        {
                            CurrentNormalCarInfo.VanCheck = 2;
                            CurrentNormalCarInfo.VanCardNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            CurrentNormalCarInfo.VanCardFullNumber = mSmartro_TITDIP_EVCat.RecvInfo.rCardNum.Trim();
                            CurrentNormalCarInfo.VanRegNo = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptNo.Trim();
                            CurrentNormalCarInfo.VanCardApproveYmd = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate;
                            CurrentNormalCarInfo.VanCardApproveHms = mSmartro_TITDIP_EVCat.RecvInfo.rAcceptTime;
                            CurrentNormalCarInfo.VanRescode = mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode == "00" ? "0000" : mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode;
                            CurrentNormalCarInfo.VanResMsg = mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;

                            CurrentNormalCarInfo.VanSupplyPay = 0;
                            CurrentNormalCarInfo.VanTaxPay = 0;
                            CurrentNormalCarInfo.VanCardName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            CurrentNormalCarInfo.VanBeforeCardPay = 0;
                            CurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            CurrentNormalCarInfo.VanCardApproveHms = DateTime.Now.ToString("HH:mm:ss");
                            CurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(mSmartro_TITDIP_EVCat.RecvInfo.rAcceptDate);
                            CurrentNormalCarInfo.VanCardApprovalHms = DateTime.Now.ToString("HH:mm:ss");

                            CurrentNormalCarInfo.VanIssueCode = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerCode;
                            CurrentNormalCarInfo.VanIssueName = mSmartro_TITDIP_EVCat.RecvInfo.rIssuerName;
                            CurrentNormalCarInfo.VanCardAcquirerCode = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseCode;
                            CurrentNormalCarInfo.VanCardAcquirerName = mSmartro_TITDIP_EVCat.RecvInfo.rPurchaseName;
                            if (NPSYS.gUseCardFailSend)
                            {
                                DateTime paydate = DateTime.Now;

                                CurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;

                                Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                            }
                        }
                        //카드실패전송 완료
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [카드결제/취소 실패] "
                            //+ " VAN사 응답코드 [" + mSmartro_TITDIP_EVCat.RecvInfo.rVanResultCode + "] "
                            + " 실패사유 [" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg + "] ");
                        if (mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg.Contains("동글이 요청 타임 오버"))
                        {
                            mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " [결제요청중 타임오버로 다시결제요청시도] ");
                            return;
                        }
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        //if (!NPSYS.gIsAutoBooth)
                        //{
                        //    if (btn_PrePage.Visible == false)
                        //    {
                        //        btn_PrePage.Visible = true;
                        //    }
                        //    if (btn_home.Visible == false)
                        //    {
                        //        btn_home.Visible = true;
                        //    }
                        //}
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", "카드 결제실패" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg);
                        //paymentControl.ErrorMessage = "카드결제실패:" + mSmartro_TITDIP_EVCat.RecvInfo.rReceiveMsg;
                        paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                        PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        mSmartro_TITDIP_EVCat.StartSoundTick = 7;
                    }
                }
                else
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 비정상 응답 [ " + e.returnData + " ]");
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu || SmartroEvcat_QueryResults ", " 전문 파싱중 예외상황 [ " + ex.Message + " ]");
            }
        }

        #endregion SMATRO_TIT_DIP_EVCAT
        
        #region SMARTRO_VCAT

        /// <summary>
        /// 스마트로 추가 상태변화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmtSndRcv_OnRcvState(object sender, AxSmtSndRcvVCATLib._DSmtSndRcvVCATEvents_OnRcvStateEvent e)
        {
            inputtime = paymentInputTimer;
            string cardState = e.szType.ToString();
            string statusMessage = mSmartroVCat.OnReceiveStatusMessage(cardState);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드동작 변화]"
                                     + " 상태값:" + cardState.ToString()
                                     + " 상태명령:" + statusMessage
                                     + " 카드동작상태:" + mCardStatus.currentCardReaderStatus.ToString()
                                     );
            paymentControl.ErrorMessage = statusMessage;
            if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            {
                if (cardState == "1I")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드결제명령 내린상태로 다시 결제요청하지않음]");
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                }
                if (cardState == "AU")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드결제명령 내린상태로 다시 결제요청하지않음]");
                    SmatroDeveinCancle();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                }
                if (cardState == "CK")
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnRcvState", "[카드가 감지됨]");
                    mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardInsert;
                    PlaySoundCard();
                }
            }
        }

        /// <summary>
        /// 스마트로추가 성공이던 실패든 카드결제 발생시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmtSndRcv_OnTermComplete(object sender, EventArgs e)
        {
            inputtime = paymentInputTimer;
            SmartroVCat.SmatroData currentSmatroReceiveData = new SmartroVCat.SmatroData();
            mSmartroVCat.ReceiveData(NPSYS.Device.SmtSndRcv, 1, ref currentSmatroReceiveData);
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnTermComplete", "[스마트로 응답결과]"
                                    + " 작업내용:" + currentSmatroReceiveData.CurrentCmdTypeName
                                    + " 성공유무:" + currentSmatroReceiveData.Success.ToString()
                                    + " 응답코드:" + currentSmatroReceiveData.ReceiveReturnCode
                                    + " 응답메세지:" + currentSmatroReceiveData.ReceiveReturnMessage
                                    + " 화면메세지:" + currentSmatroReceiveData.ReceiveDisplayMsg
                                    + " 에러메세지:" + currentSmatroReceiveData.ErrorMessage
                                    + " 카드번호:" + currentSmatroReceiveData.ReceiveCardNumber
                                    + " 승인요청금액:" + currentSmatroReceiveData.ReceiveCardAmt
                                    + " 세금:" + currentSmatroReceiveData.ReceiveTaxAmt
                                    + " 봉사료:" + currentSmatroReceiveData.ReceiveBongSaInx
                                    + " 승인일자:" + currentSmatroReceiveData.ReceiveAppYmd
                                    + " 승인시간:" + currentSmatroReceiveData.ReceiveAppHms
                                    + " 필터1:" + currentSmatroReceiveData.ReceiveFIlter1
                                    + " 필터2:" + currentSmatroReceiveData.ReceiveFIlter2
                                    + " 할부개월:" + currentSmatroReceiveData.ReceiveHalbu
                                    + " 매입사코드:" + currentSmatroReceiveData.ReceiveMaipCode
                                    + " 매입사명:" + currentSmatroReceiveData.ReceiveMaipName
                                    + " 발급사코드:" + currentSmatroReceiveData.ReceiveBalgubCode
                                    + " 발급사명:" + currentSmatroReceiveData.ReceiveBalgubName
                                    + " 마스터키:" + currentSmatroReceiveData.ReceiveMasterKey
                                    + " 가맹점번호:" + currentSmatroReceiveData.ReceiveStoreNo
                                    + " 단말기번호:" + currentSmatroReceiveData.ReceiveTermNo
                                    + " 거래고유번호:" + currentSmatroReceiveData.ReceiveUniqueNo
                                    + " 워킹키:" + currentSmatroReceiveData.ReceiveWorkKey
                                    + " 승인번호:" + currentSmatroReceiveData.RecieveApprovalNumber
                                    + " 워킹인덱스:" + currentSmatroReceiveData.WoriingIndex
                                                    );
            switch (currentSmatroReceiveData.CurrentCmdType)
            {
                case SmartroVCat.SmatroData.CMDType.CardApprovalRespone:
                    CardApprovalRespone(currentSmatroReceiveData);
                    break;

                case SmartroVCat.SmatroData.CMDType.ApprovalInitializeRespone:
                    CardInitializeRespone(currentSmatroReceiveData);
                    break;
            }
        }

        /// <summary>
        /// 카드결제 성공또는 실패
        /// </summary>
        /// <param name="pSmartroData"></param>
        private void CardApprovalRespone(SmartroVCat.SmatroData pSmartroData)
        {
            try
            {
                if (pSmartroData.Success)
                {
                    //    CardApprovalRespone 성공유무:True 응답코드:00 응답메세지:정상 화면메세지:정상승인거래
                    //에러메세지: 카드번호:541707********** 승인요청금액:50000 세금:4550 봉사료:0
                    //승인일자:20160120 승인시간:220313 발급사코드: 발급사명: 화면메세지:정상승인거래
                    //필터1: 필터2:1404712223446511 할부개월:00 매입사코드:0505 매입사명:외환카드사 마스터키:A7EB0482C68B8214
                    //가맹점번호:00951685684
                    //단말기번호:2114698013220310
                    //거래고유번호:160120220312 워킹키:1 승인번호:90008063     워킹인덱스:

                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardApprovalRespone", "[카드결제 성공]");
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, CurrentNormalCarInfo, MoneyType.CreditCard, CurrentNormalCarInfo.PaymentMoney, 0, "");
                    string[] lCardNumData = pSmartroData.ReceiveCardNumber.Split('=');
                    if (lCardNumData[0].Length > 13)
                    {
                        CurrentNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    }
                    else
                    {
                        CurrentNormalCarInfo.VanCardNumber = lCardNumData[0];
                    }
                    CurrentNormalCarInfo.VanRegNo = pSmartroData.RecieveApprovalNumber.Trim();
                    CurrentNormalCarInfo.VanDate = pSmartroData.ReceiveAppYmd;
                    CurrentNormalCarInfo.VanRescode = pSmartroData.ReceiveReturnCode;
                    CurrentNormalCarInfo.VanResMsg = pSmartroData.ReceiveReturnMessage;
                    CurrentNormalCarInfo.VanSupplyPay = (Convert.ToInt32(pSmartroData.ReceiveCardAmt) - Convert.ToInt32(pSmartroData.ReceiveTaxAmt));
                    CurrentNormalCarInfo.VanTaxPay = Convert.ToInt32(pSmartroData.ReceiveTaxAmt);
                    CurrentNormalCarInfo.VanCardName = pSmartroData.ReceiveBalgubName;
                    CurrentNormalCarInfo.VanBeforeCardPay = CurrentNormalCarInfo.PaymentMoney;
                    CurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(pSmartroData.ReceiveAppYmd);
                    CurrentNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(pSmartroData.ReceiveAppHms);
                    CurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(pSmartroData.ReceiveAppYmd);
                    CurrentNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(pSmartroData.ReceiveAppHms);

                    CurrentNormalCarInfo.VanIssueCode = pSmartroData.ReceiveBalgubCode;
                    CurrentNormalCarInfo.VanIssueName = pSmartroData.ReceiveBalgubName;
                    CurrentNormalCarInfo.VanCardAcquirerCode = pSmartroData.ReceiveMaipCode;
                    CurrentNormalCarInfo.VanCardAcquirerName = pSmartroData.ReceiveMaipName;

                    CurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(CurrentNormalCarInfo);
                    //LPRDbSelect.SaveCardPay(mNormalCarInfo);
                    //결제완료된 정보를 보내야한다 아니면 가지고 있거나
                    CurrentNormalCarInfo.VanAmt = CurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|CreditCardPayResult", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                    paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                }
                else
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                    string cardApprovalErrorMessage = string.Empty; // 카드승인에러메세지
                    cardApprovalErrorMessage = pSmartroData.ReceiveReturnMessage.Trim() == string.Empty ? pSmartroData.ReceiveDisplayMsg : pSmartroData.ReceiveReturnMessage;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardApprovalRespone", "[카드결제 실패]" + cardApprovalErrorMessage);
                    if (pSmartroData.ReceiveReturnCode == "EC")
                    {
                        paymentControl.ErrorMessage = "결제가 실패하였습니다.카드를 뽑으신후 다시 결제를 시도해 주세요";
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardFrontEjuct;
                        SmatroDeveinCancle();
                        PlaySoundCard();
                    }
                    else if (pSmartroData.ReceiveReturnCode == "HD" || pSmartroData.ReceiveReturnCode == "TA")
                    {
                        paymentControl.ErrorMessage = "한도초과입니다.다른카드를 사용해주세요";
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.FullPay;
                        SmatroDeveinCancle();
                        PlaySoundCard();
                    }
                    else
                    {
                        paymentControl.ErrorMessage = "카드결제실패: " + cardApprovalErrorMessage;
                        mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.CardFrontEjuct;
                        SmatroDeveinCancle();
                        PlaySoundCard();
                    }
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                    mSmartroVCat.StartSoundTick = 7;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | CardApprovalRespone", ex.ToString());
            }
        }

        private void CardInitializeRespone(SmartroVCat.SmatroData pSmartroData)
        {
            if (pSmartroData.Success)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardInitializeRespone", "[카드요금취소성공]");
                if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPayCancle;
                }
            }
            else
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | CardInitializeRespone", "[카드요금취소실패]");
            }
        }

        /// <summary>
        /// 카드취소등의동작시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmtSndRcv_OnTermExit(object sender, EventArgs e)
        {
            inputtime = paymentInputTimer;
            string errorMessage = string.Empty;
            int errorCode;
            errorCode = NPSYS.Device.SmtSndRcv.GetExitErrorCode();
            string errorName = string.Empty;
            switch (errorCode)
            {
                case -9:
                    errorName = "DATA미수신";
                    break;

                case -10:
                    errorName = "VCat Nack수신";
                    break;

                case -11:
                    errorName = "VCat 취소";
                    break;

                case -12:
                    errorName = "전문불량 etx없음";
                    break;

                case -13:
                    errorName = "전문불량 stx없음";
                    break;
            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmtSndRcv_OnTermExit", "[카드비정상동작] 상태코드:" + errorCode.ToString() + " 상태값:" + errorName);

            //if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            //{
            //    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPayCancle;
            //}
        }

        private bool SmatroDeveinCancle()
        {
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[취소쓰레드웨이트]");
            mCreditCardThreadLock.WaitOne(3000);
            mCreditCardThreadLock.Reset();
            if (mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardPaySuccess)
            {
                mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
            }
            mSmartroVCat.StartSoundTick = 0;
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청시작]");
            SmartroVCat.SmatroData smatrodata = mSmartroVCat.DeviceReInitialLizeSync(NPSYS.Device.SmtSndRcv);
            for (int i = 0; i < 16; i++)
            {
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청시작 종료]");
            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[스마트로 VCat거래초기화 요청결과]" + smatrodata.Success.ToString() + " 응답코드:" + smatrodata.ReceiveReturnCode);
            mCreditCardThreadLock.Set();
            return smatrodata.Success;
        }

        private void SmartroCardApprovalAction()
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay
                || CurrentNormalCarInfo.VanAmt != 0 || CurrentNormalCarInfo.Current_Money > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (CurrentNormalCarInfo.Current_Money == 0)
            {
                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SmartroVCat && NPSYS.Device.gIsUseCreditCardDevice
                    || NPSYS.Device.UsingSettingMagneticReadType == ConfigID.CardReaderType.SmartroVCat && NPSYS.Device.gIsUseMagneticReaderDevice)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroDeveinCancle", "[승인쓰레드웨이트]");
                    mCreditCardThreadLock.WaitOne(2000);
                    mCreditCardThreadLock.Reset();
                    System.Threading.Thread.Sleep(100);
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmartroCardApprovalAction", "[신용카드요금결제요청 시작]" + CurrentNormalCarInfo.ReceiveMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    bool isSend = mSmartroVCat.CardApproval(NPSYS.Device.SmtSndRcv, Convert.ToInt32(CurrentNormalCarInfo.ReceiveMoney), 600, ref errorMessage);
                    for (int i = 0; i < 20; i++)
                    {
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();
                    }
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmartroCardApprovalAction", "[신용카드요금결제요청 종료]" + CurrentNormalCarInfo.ReceiveMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    mCreditCardThreadLock.Set();
                }
            }
        }

        private void PlaySoundCard()
        {
            if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.CardInsert)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardInStep);

            }
            else if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.CardFrontEjuct)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardOutStep);

            }
            else if (mSmartroVCat.CurrentVoiceType == SmartroVCat.voiceType.FullPay)
            {
                mSmartroVCat.CurrentVoiceType = SmartroVCat.voiceType.None;
                PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardFullPayStep);

            }
        }

        #endregion SMARTRO_VCAT

        #region SMARTRO_TL3500BS

        /// <summary>
        /// TMoney 스마트로 Receive Data 처리 이벤트 핸들러
        /// </summary>
        /// <param name="pDTO"></param>
        private void TmoneySmartro3500_EventTMoneyData(SmartroDTO pDTO)
        {
            // =========[수신 받은 JOB 코드 목록]=========
            // a : 장치체크 응답전문
            // b : 거래승인 응답전문
            // c : 거래취소 응답전문
            // d : 카드조회 응답전문
            // e : 결제대기 응답전문
            // f : 카드 UID 읽기 응답전문
            // @ : 이벤트 응답전문
            // g : 부가정보 추가 거래승인 응답전문
            // i : 설정 정보 셋팅 응답전문
            // j : 설정 정보 응답전문
            // K : 설정 정보 메모리 WRITING 응답전문
            // I : 마지막 승인 응답전문
            // v : 버전 체크 응답전문
            // s : 화면&음성 설정 응답전문
            // =========[이벤트 코드 목록]=========
            // M : MS카드 인식
            // R : RF카드 인식
            // I : IC카드 인식
            // O : IC카드 제거
            // F : IC카드 FallBack
            // =========[결제/결제취소 Process]=========
            //결제 : 결제대기 요청 -> 결제대기 응답 -> 카드 삽입 이벤트 응답 -> 거래 승인 요청 -> 거래 승인 응답 -> [카드를 제거해주세요] -> 이벤트 응답
            //결제취소 : 결제대기 요청 -> 결제대기 응답 -> 카드 삽입 이벤트 응답 -> 거래 취소 요청 -> 거래 취소 응답 -> [카드를 제거해주세요] -> 이벤트 응답

            try
            {
                Header header = pDTO?.HeaderData as Header;
                if (header != null)
                {
                    switch (header.JobCode)
                    {
                        case "a": //장치체크 응답전문
                            ReceiveDeviceCheck deviceCheck = pDTO.BodyData as ReceiveDeviceCheck;
                            if (deviceCheck != null)
                            {
                                //장치체크 응답전문 처리
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | TmoneySmartro3500_EventTMoneyData",
                                    Smartro_TL3500S.ResponseDeviceCheckHandler(deviceCheck));
                            }
                            else
                            {
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | TmoneySmartro3500_EventTMoneyData",
                                    "장치체크 전문 오류");
                            }
                            break;
                        case "b": //거래승인 응답전문
                        case "c": //거래취소 응답전문
                            ReceiveApproval receiveApproval = pDTO.BodyData as ReceiveApproval;

                            if (CurrentNormalCarInfo.PaymentMoney > 0 && CurrentNormalCarInfo.Current_Money == 0)
                            {
                                NPSYS.CurrentBusyType = NPSYS.BusyType.Paying;
                                Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, CurrentNormalCarInfo, pDTO);
                                NPSYS.CurrentBusyType = NPSYS.BusyType.None;

                                if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                                {
                                    NPSYS.CashCreditCount += 1;
                                    NPSYS.CashCreditMoney += CurrentNormalCarInfo.VanAmt;
                                    paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                                    paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | TmoneySmartro3500_EventTMoneyData", "정상적인 카드결제됨");

                                    if (CurrentNormalCarInfo.PaymentMoney == 0)
                                    {
                                        //0원 결제
                                        CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                                        //0원 결제완료
                                        PaymentComplete();
                                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardApproval;
                                        return;
                                    }
                                }
                                else // 잘못된 티켓
                                {
                                    if (NPSYS.gUseCardFailSend)
                                    {
                                        DateTime paydate = DateTime.Now;
                                        //카드실패전송
                                        CurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                                        //카드실패전송 완료
                                        Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                                    }
                                    //카드실패전송 완료
                                    TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | TmoneySmartro3500_EventTMoneyData", "정상적인 카드결제안됨");
                                    paymentControl.ErrorMessage = _CardpaySuccess.Message;
                                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                                    return;
                                }
                            }

                            break;
                        case "d": //카드조회 응답전문
                            break;
                        case "e": //결제대기 응답전문
                            break;
                        case "f": //카드 UID 읽기 응답전문
                            break;
                        case "@": //이벤트 응답전문
                            // M : MS카드 인식
                            // R : RF카드 인식
                            // I : IC카드 인식
                            // O : IC카드 제거
                            // F : IC카드 FallBack
                            ReceiveEvent receive = pDTO.BodyData as ReceiveEvent;
                            TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | TmoneySmartro3500_EventCode", $"{receive.EventCode}");
                            break;
                        case "g": //부가정보 추가 거래승인 응답전문
                            break;
                        case "i": //설정 정보 셋팅 응답전문
                            break;
                        case "j": //설정 정보 응답전문
                            break;
                        case "K": //설정 정보 메모리 WRITING 응답전문
                            break;
                        case "I": //마지막 승인 응답전문
                            break;
                        case "v": //버전 체크 응답전문
                            break;
                        case "s": //화면 & 음성 설정 응답전문
                            break;
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | TmoneySmartro3500_EventTMoneyData", "[전문수신오류]");
            }
        }

        #endregion

        #region 사용안함

        delegate void Ctrl_Involk(Control ctrl, string text);

        public void setText(Control ctrl, string txtValue)
        {
            if (ctrl.InvokeRequired)
            {
                Ctrl_Involk CI = new Ctrl_Involk(setText);
                ctrl.Invoke(CI, ctrl, txtValue);
            }
            else
            {
                ctrl.Text = txtValue;
            }

        }

        private void SmatroEVCAT_ChangePayMoneyAction()
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardStop || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardReady || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardInitailizeSuccess
                || CurrentNormalCarInfo.VanAmt != 0 || CurrentNormalCarInfo.GetInComeMoney > 0
             || mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess || CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }

            if (CurrentNormalCarInfo.GetInComeMoney == 0)
            {
                paymentControl.ErrorMessage = string.Empty;
                string errorMessage = string.Empty;
                if (NPSYS.Device.UsingSettingCardReadType == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 요금변경 결제요청 시작]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                    mSmartro_TITDIP_EVCat.ChangePayMoney(NPSYS.Device.Smartro_TITDIP_Evcat, CurrentNormalCarInfo.PaymentMoney.ToString());
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | SmatroEVCAT_CardApprovalAction", "[신용카드 요금변경 결제요청 종료]" + CurrentNormalCarInfo.PaymentMoney.ToString() + " 메모리:" + System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64.ToString());
                }
            }
        }

        #endregion
    }
}