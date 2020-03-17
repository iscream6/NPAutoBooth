using FadeFox;
using FadeFox.Text;
using NPCommon;
using NPCommon.DTO;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// KSNET 처리
    /// </summary>
    partial class FormCreditPaymentMenu
    {
        private void KsnetCardAction()
        {

            //btnCardApproval.Visible = false; //뉴타입주석

            PlayCardVideo(axWindowsMediaPlayer1.URL, NPSYS.gKSNetCardInStep);
            try
            {
                TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | KsnetCardAction", "[카드결제 KSNET장비에 요청]");
                Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, CurrentNormalCarInfo);
                if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                {
                    NPSYS.CashCreditCount += 1;
                    NPSYS.CashCreditMoney += CurrentNormalCarInfo.VanAmt;
                    paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                    TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | btnCardAction_Click", "정상적인 카드결제");
                    if (CurrentNormalCarInfo.PaymentMoney == 0)
                    {
                        //카드실패전송
                        CurrentNormalCarInfo.PaymentMethod = PaymentType.CreditCard;
                        //카드실패전송완료
                        PaymentComplete();

                        return;
                    }
                }
                else if (_CardpaySuccess.Message.Split(':')[0] == "0002")// 
                {
                    return;
                }
                else // 잘못된 티켓
                {
                    TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu|timer_CardReader1_Tick", "정상적인 카드결제안됨" + _CardpaySuccess.Message);
                    paymentControl.ErrorMessage = _CardpaySuccess.Message;
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);
                    return;
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu | btnCardAction_Click", ex.ToString());
            }
            finally
            {
                if (CurrentNormalCarInfo.PaymentMoney != 0)
                {
                    StartPlayVideo();
                }
            }
        }
        //KSNet 적용완료
    }
}
