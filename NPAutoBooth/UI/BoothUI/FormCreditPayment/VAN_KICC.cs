using FadeFox;
using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// KICC 처리
    /// </summary>
    partial class FormCreditPaymentMenu
    {
        private void CardActionKiccDip()
        {
            try
            {
                if (CurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (CurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }

                bool resultCardState = NPSYS.Device.KICC_TIT.GetCardInsert();
                if (resultCardState)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                    KICC_TIT.KICC_TIT_RECV_SUCCESS RECV_SUCCESS = NPSYS.Device.KICC_TIT.GetRecvData();
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "KICC_DIP장비상태" + RECV_SUCCESS.MSG);
                }

                if (CurrentNormalCarInfo.PaymentMoney > 0 && CurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    inputtime = paymentInputTimer;
                    NPSYS.CurrentBusyType = NPSYS.BusyType.Paying;
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, CurrentNormalCarInfo);
                    NPSYS.CurrentBusyType = NPSYS.BusyType.None;

                    NPSYS.Device.KICC_TIT.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += CurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "정상적인 카드결제됨");

                        if (CurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else // 잘못된 티켓
                    {
                        if (CurrentNormalCarInfo.VanRescode != KICC_TIT.KICC_USER_CANCLECODE)
                        {
                            //카드실패전송
                            if (NPSYS.gUseCardFailSend)
                            {
                                DateTime paydate = DateTime.Now;
                                //카드실패전송
                                CurrentNormalCarInfo.PaymentMethod = PaymentType.Fail_Card;
                                //카드실패전송 완료
                                Payment currentCar = mHttpProcess.PaySave(CurrentNormalCarInfo, paydate);
                            }
                            //카드실패전송 완료
                        }
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKICC_DIP_IFM_Tick", ex.ToString());
            }

        }
        public void GetKiccState()
        {

            int cmd = 0;
            int gcd = 0;
            int jcd = 0;
            int rcd = 0;
            byte[] rdata = new byte[4086];
            byte[] rhexadata = new byte[4086];
            int result = KiccTs141.KGetEvent(ref cmd, ref gcd, ref jcd, ref rcd, rdata, rhexadata);
            if (result >= 0)
            {
                string rCmd = cmd.ToString();
                string rGcd = gcd.ToString();
                string rJcd = jcd.ToString();
                string rRcd = rcd.ToString();

                string rData = Encoding.Default.GetString(rdata);
                int subIndex = 0;
                subIndex += 2;
                string returnCode = Encoding.Default.GetString(rdata, subIndex, 4).Trim();


                if (returnCode == "0000")
                {
                    //btnCardApproval.Visible = false;  //뉴타입주석
                    subIndex += 4;
                    string tId = Encoding.Default.GetString(rdata, subIndex, 8).Trim(); //터미널ID
                    subIndex += 8;
                    string wcc = Encoding.Default.GetString(rdata, subIndex, 1).Trim();
                    subIndex += 1;
                    string cardNo = Encoding.Default.GetString(rdata, subIndex, 40).Trim();
                    subIndex += 40;
                    string halbu = Encoding.Default.GetString(rdata, subIndex, 2).Trim();
                    subIndex += 2;
                    string cardMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim();
                    subIndex += 8;
                    string bonsaMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim();
                    subIndex += 8;
                    string VatMoney = Encoding.Default.GetString(rdata, subIndex, 8).Trim(); // vat
                    subIndex += 8;
                    string approvalNo = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); //승인번호
                    subIndex += 12;
                    string approvalDate = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); //승인일시
                    subIndex += 12;
                    string issueCode = Encoding.Default.GetString(rdata, subIndex, 3).Trim(); // 발급사코드
                    subIndex += 3;
                    string issueName = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 발급사명
                    subIndex += 20;
                    string gamangCode = Encoding.Default.GetString(rdata, subIndex, 12).Trim(); // 가맹점코드
                    subIndex += 12;
                    string accquireCode = Encoding.Default.GetString(rdata, subIndex, 3).Trim(); // 매입사코드
                    subIndex += 3;
                    string accquireName = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 매입사명
                    subIndex += 20;
                    string posSequnceNo = Encoding.Default.GetString(rdata, subIndex, 20).Trim(); // 발급사명
                    subIndex += 20;

                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu1920 | GetKiccState", "[카드결제 성공]"
                        + " [TID단말기번호]" + tId
                        + " [승인번호]" + approvalNo
                        + " [승인시각]" + approvalDate
                        + " [매입사명]" + accquireName
                        + " [발급사명]" + issueName
                        + " [포스거래번호]" + posSequnceNo
                        + " [결제금액]" + cardMoney);
                    paymentControl.ErrorMessage = "결제가성공하였습니다";
                    string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    LPRDbSelect.LogMoney(PaymentType.CreditCard, logDate, CurrentNormalCarInfo, MoneyType.CreditCard, CurrentNormalCarInfo.PaymentMoney, 0, "");
                    CurrentNormalCarInfo.VanCheck = 1;
                    string[] lCardNumData = cardNo.Split('=');
                    if (lCardNumData[0].Length > 13)
                    {
                        CurrentNormalCarInfo.VanCardNumber = lCardNumData[0].Substring(0, 4) + "-" + lCardNumData[0].Substring(4, 4) + "-" + lCardNumData[0].Substring(8, 4) + "-" + lCardNumData[0].Substring(12);
                    }
                    else
                    {
                        CurrentNormalCarInfo.VanCardNumber = lCardNumData[0];
                    }
                    approvalDate = "20" + approvalDate;
                    CurrentNormalCarInfo.VanRegNo = approvalNo;
                    CurrentNormalCarInfo.VanDate = approvalDate;
                    CurrentNormalCarInfo.VanRescode = returnCode;
                    CurrentNormalCarInfo.VanResMsg = "정상";
                    int Creditpaymoneys = CurrentNormalCarInfo.PaymentMoney;

                    int taxsResult = (int)(Creditpaymoneys / 11);
                    int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가
                    CurrentNormalCarInfo.VanSupplyPay = SupplyPay;
                    CurrentNormalCarInfo.VanTaxPay = taxsResult;
                    CurrentNormalCarInfo.VanCardName = issueName;
                    CurrentNormalCarInfo.VanBeforeCardPay = Convert.ToInt32(CurrentNormalCarInfo.PaymentMoney);
                    CurrentNormalCarInfo.VanCardApproveYmd = NPSYS.ConvetYears_Dash(approvalDate.Substring(0, 8));
                    CurrentNormalCarInfo.VanCardApproveHms = NPSYS.ConvetDay_Dash(approvalDate.Substring(8));
                    CurrentNormalCarInfo.VanCardApprovalYmd = NPSYS.ConvetYears_Dash(approvalDate.Substring(0, 8));
                    CurrentNormalCarInfo.VanCardApprovalHms = NPSYS.ConvetDay_Dash(approvalDate.Substring(8));

                    //만료차량 정기권요금제에서 일반요금제 변경기능 (매입사정보에 발급사정보들어가던거 수정)
                    CurrentNormalCarInfo.VanIssueCode = issueCode;
                    CurrentNormalCarInfo.VanIssueName = issueName;
                    CurrentNormalCarInfo.VanCardAcquirerCode = accquireCode;
                    CurrentNormalCarInfo.VanCardAcquirerName = accquireName;
                    //만료차량 정기권요금제에서 일반요금제 변경기능주석완료


                    CurrentNormalCarInfo.VanRescode = "0000";
                    LPRDbSelect.Creditcard_Log_INsert(CurrentNormalCarInfo);
                    //LPRDbSelect.SaveCardPay(mNormalCarInfo); 
                    //통합처리로 결제정보를 전달해야한다
                    CurrentNormalCarInfo.VanAmt = CurrentNormalCarInfo.PaymentMoney;
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | GetKiccState", "카드 결제성공");

                    paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                    paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                }
                else // fallback거래등현재 확인결과 9999는 fallback으로보임
                {
                    if (returnCode == "9999")
                    {
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | GetKiccState", "사용자취소]");

                    }
                    else
                    {
                        KiccTs141.Initilize();
                        paymentControl.ErrorMessage = "[카드결제실패]" + "카드를 빼시고 카드결제버튼을 눌러 다시 시도요청";
                        PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardReTry);
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardSoundPlay;
                        KiccTs141.StartSoundTick = 7;
                        TextCore.INFO(TextCore.INFOS.MEMORY, "FormPaymentMenu | GetKiccState", "[KICC카드기기 초기화]" + "원인코드:" + returnCode);
                    }
                }
            }
        }

        #region KICC TS141

        private void btnCardApproval_Click(object sender, EventArgs e)
        {

            if (CurrentNormalCarInfo.PaymentMoney > 0)
            {
                TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | btnCardApproval_Click", "[신용카드 결제버튼누름]");
                //포시즌 카드누를시 화면숨김
                mCardVisible = 10;
                //btnCardApproval.Visible = false;  //뉴타입주석

                //포시즌 카드누를시 화면숨김 주석완료
                if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KICC_TS141)
                {
                    KiccTs141.Initilize();
                    int Creditpaymoneys = CurrentNormalCarInfo.PaymentMoney;

                    int taxsResult = (int)(Creditpaymoneys / 11);
                    int SupplyPay = Creditpaymoneys - Convert.ToInt32(taxsResult); //공급가
                    bool isApproveSuccess = KiccTs141.Approval(Creditpaymoneys.ToString(), taxsResult.ToString());
                    if (isApproveSuccess)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                        paymentControl.ErrorMessage = string.Empty;
                        timerKiccTs141State.Enabled = true;
                        timerKiccTs141State.Start();
                        timerKiccSoundPlay.Enabled = true;
                        timerKiccSoundPlay.Start();
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormCreditPaymentMenu | btnCardApproval_Click", "[신용카드 결제요청됨]");
                    }
                }
                //KSNet 적용
                else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KSNET)
                {

                    KsnetCardAction();
                }
                //KSNet 적용완료

            }
        }



        ////포시즌 카드누를시 화면숨김
        int mCardVisible = 0;
        private void timerCardVisible_Tick(object sender, EventArgs e)
        {
            if (mCardVisible <= 0) // mCardVisible이 0보다적으면
            {
                //if (btnCardApproval.Visible == false) //뉴타입주석
                //{ //뉴타입주석
                //    btnCardApproval.Visible = true; //뉴타입주석
                //} //뉴타입주석
            }
            mCardVisible -= 1;
        }
        //포시즌 카드누를시 화면숨김 주석완료


        private void timerKiccTs141State_Tick(object sender, EventArgs e)
        {
            try
            {
                timerKiccTs141State.Stop();

                GetKiccState();
                if (CurrentNormalCarInfo.PaymentMoney == 0 && CurrentNormalCarInfo.VanAmt > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardPaySuccess)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardPaySuccess;
                    //카드실패전송
                    CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                    //카드실패전송완료
                    PaymentComplete();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | timerKiccTs141State_Tick", ex.ToString());
            }
            finally
            {
                if (CurrentNormalCarInfo.PaymentMoney != 0)
                {
                    timerKiccTs141State.Start();
                }
            }

        }
        private void timerKiccSoundPlay_Tick(object sender, EventArgs e)
        {
            if (mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CardSoundPlay)
            {
                if (KiccTs141.StartSoundTick > 0)
                {
                    KiccTs141.StartSoundTick -= 1;
                    if (KiccTs141.StartSoundTick == 0)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
