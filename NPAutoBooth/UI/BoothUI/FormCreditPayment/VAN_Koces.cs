using FadeFox;
using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    partial class FormCreditPaymentMenu
    {
        private void CardActionKocesPayMGate()
        {
            try
            {
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제금액이0원이고 카드가 들어가있다면
                {
                    //mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                int result = KocesTcmMotor.CardState();
                if (result == 2)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                }
                else
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    if (_CardpaySuccess.Success)
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | OnTimerKocesPayMGateState", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | OnTimerKocesPayMGateState", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;

                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormCreditPaymentMenu | OnTimerKocesPayMGateState ", "예외상황 : " + ex.ToString());
            }

        }

        private void CardActionKocesTcm()
        {

            try
            {
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    KocesTcmMotor.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    KocesTcmMotor.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.None)
                {
                    bool isSuceessAccept = KocesTcmMotor.CardAccept();
                    if (isSuceessAccept)
                    {
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                    }

                }
                int resultCardState = KocesTcmMotor.CardState();
                switch (resultCardState)
                {
                    case 0:
                    case 1:
                        break;
                    case 2:
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                        break;
                    default:
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerKocesTcmState_Tick", "[코세스 장비장애코드]" + resultCardState.ToString());
                        break;
                }
                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    //KOCSE결제시 이미지 추가

                    pic_Wait_MSG_WAIT.BringToFront();
                    pic_Wait_MSG_WAIT.Visible = true;
                    // PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gCardInReading);
                    if (!NPSYS.gIsAutoBooth)
                    {
                        //if (btn_PrePage.Visible == true) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_PrePage.Visible = false; //뉴타입주석
                        //} //뉴타입주석
                        //if (btn_home.Visible == true) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_home.Visible = false; //뉴타입주석
                        //} //뉴타입주석
                    }
                    Application.DoEvents();
                    Thread.Sleep(700);
                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    KocesTcmMotor.CardEject();
                    pic_Wait_MSG_WAIT.SendToBack();
                    pic_Wait_MSG_WAIT.Visible = false;
                    if (!NPSYS.gIsAutoBooth)
                    {
                        //if (btn_PrePage.Visible == false)  //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_PrePage.Visible = true; //뉴타입주석
                        //} //뉴타입주석
                        //if (btn_home.Visible == false) //뉴타입주석
                        //{ //뉴타입주석
                        //    btn_home.Visible = true; //뉴타입주석
                        //} //뉴타입주석
                    }
                    //KOCSE결제시 이미지 추가
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu|timer_CardReader1_Tick", "정상적인 카드결제됨");

                        if (mCurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            mCurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                            //카드실패전송완료
                            PaymentComplete();

                            return;
                        }
                    }
                    else // 잘못된 티켓
                    {
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerKocesTcmState_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch
            {
            }

        }
    }
}
