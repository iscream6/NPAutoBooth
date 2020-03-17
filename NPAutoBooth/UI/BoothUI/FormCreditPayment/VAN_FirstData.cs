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
    partial class FormCreditPaymentMenu
    {
        #region FistData DIP적용
        //FIRSTDATA처리 
        private void CardActionFirstDataDip()
        {
            try
            {
                if (mCurrentNormalCarInfo.Current_Money > 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 현금이 있을때 카드가 들어가있다면
                {
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                if (mCurrentNormalCarInfo.PaymentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED) // 결제요금이 0원이고카드가 들어가있다면
                {
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    return;
                }
                //if (mNormalCarInfo.PaymentMoney > 0 && mNormalCarInfo.CurrentMoney == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.None)
                //{
                //    bool isSuceessAccept = KocesTcmMotor.CardAccept();
                //    if (isSuceessAccept)
                //    {
                //        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardReady;
                //    }

                //}
                FirstDataDip.readerStatus currentStatus = FirstDataDip.ReadState();
                if (currentStatus == FirstDataDip.readerStatus.ReaderIcIn)
                {
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CARDINSERTED;
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "FISTDATA_DIP장비상태" + CardDeviceStatus.CardReaderStatus.CARDINSERTED);
                }

                if (mCurrentNormalCarInfo.PaymentMoney > 0 && mCurrentNormalCarInfo.Current_Money == 0 && mCardStatus.currentCardReaderStatus == CardDeviceStatus.CardReaderStatus.CARDINSERTED)
                {

                    Result _CardpaySuccess = m_PayCardandCash.CreditCardPayResult(string.Empty, mCurrentNormalCarInfo);
                    FirstDataDip.CardEject();
                    mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                    if (_CardpaySuccess.Success) // 정상적인 티켓이라면
                    {
                        NPSYS.CashCreditCount += 1;
                        NPSYS.CashCreditMoney += mCurrentNormalCarInfo.VanAmt;
                        paymentControl.Payment = TextCore.ToCommaString(mCurrentNormalCarInfo.PaymentMoney);
                        paymentControl.DiscountMoney = TextCore.ToCommaString(mCurrentNormalCarInfo.TotDc);
                        TextCore.INFO(TextCore.INFOS.CARD_SUCCESS, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "정상적인 카드결제됨");

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
                        TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", "정상적인 카드결제안됨");
                        paymentControl.ErrorMessage = _CardpaySuccess.Message;
                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.None;
                        EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NotCardPay);

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.CARD_FAIL, "FormPaymentMenu | timerFirstData_DIP_IFM_Tick", ex.ToString());
            }

        }


        //FIRSTDATA처리 주석완료
        #endregion
    }
}
