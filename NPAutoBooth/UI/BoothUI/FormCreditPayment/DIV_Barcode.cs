using FadeFox.Text;
using NPCommon;
using NPCommon.DEVICE;
using NPCommon.DTO;
using NPCommon.DTO.Receive;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// 바코드 처리
    /// </summary>
    partial class FormCreditPaymentMenu
    {

        /// <summary>
        /// 바코드할인 리스트로변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerBarcode_Tick(object sender, EventArgs e)
        {
            if (NPSYS.CurrentFormType != mCurrentFormType)
            {
                return;
            }
            if (CurrentNormalCarInfo.Current_Money > 0)
            {
                return;
            }
            if (CurrentNormalCarInfo.PaymentMoney == 0)
            {
                return;
            }
            //KIS 할인처리시 처리문제

            if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM && mCardStatus.currentCardReaderStatus != CardDeviceStatus.CardReaderStatus.CardReady)
            {
                return;
            }
            //KIS 할인처리시 처리문제주석완료
            //바코드모터드리블 사용
            if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode)
            {
                if (mListBarcodeData.Count > 0)
                {
                    timerBarcode.Stop();
                    string barcodeData = mListBarcodeData[0];
                    mListBarcodeData.RemoveAt(0);
                    BarcodeAction(barcodeData);

                    if (CurrentNormalCarInfo.PaymentMoney != 0)
                    {
                        timerBarcode.Start();
                    }

                }
            }
            else if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
            {
                if (mListBarcodeMotorData.Count > 0)
                {
                    timerBarcode.Stop();
                    if (mListBarcodeMotorData[0].ResultStatus == BarcodeMotorErrorCode.Ok)
                    {
                        string barcodeMotroData = mListBarcodeMotorData[0].Data;
                        //barcodeMotroData = "SF1E";//test
                        mListBarcodeMotorData.RemoveAt(0);
                        BarcodeAction(barcodeMotroData);
                    }
                    else
                    {
                        paymentControl.ErrorMessage = string.Empty;
                        switch (mListBarcodeMotorData[0].ResultStatus)
                        {
                            case BarcodeMotorErrorCode.TicketJamError:
                                paymentControl.ErrorMessage = "용지가 안에 걸렸습니다";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerBarcode_Tick", paymentControl.ErrorMessage);
                                Application.DoEvents();
                                break;
                            case BarcodeMotorErrorCode.TIcketReadError:
                                paymentControl.ErrorMessage = "바코드를 읽지 못하였습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | timerBarcode_Tick", paymentControl.ErrorMessage);
                                // 자동으로 방출된다
                                break;
                            default:
                                paymentControl.ErrorMessage = mListBarcodeMotorData[0].ResultStatus.ToString();
                                break;
                        }
                        mListBarcodeMotorData.RemoveAt(0);
                    }

                    if (CurrentNormalCarInfo.PaymentMoney != 0)
                    {
                        timerBarcode.Start();
                    }

                }
            }
            //바코드모터드리블 사용완료
        }

        void BarcodeSerials_EventBarcode(object sender, string pBarcodeData)
        {
            if (this.Visible == false)
            {
                return;
            }
            mListBarcodeData.Add(pBarcodeData);
        }

        //바코드모터드리블 사용
        void BarcodeMotorSerials_EventBarcode(BarcodeMoter.BarcodeMotorResult pBarcodeMotorResult)
        {
            if (mCurrentFormType != NPSYS.CurrentFormType)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeMotorSerials_EventBarcode", "현재 요금폼이 아니라 바코드 처리안함");
                if (mListBarcodeData != null && mListBarcodeData.Count > 0)
                {
                    mListBarcodeData.Clear();
                }
                return;
            }
            mListBarcodeMotorData.Add(pBarcodeMotorResult);
        }

        private void BarcodeAction(string pBarcodeData)
        {

            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", "[바코드정보 처리] " + pBarcodeData);
            paymentControl.ErrorMessage = string.Empty;
            if (CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Outcar_Season
                || CurrentNormalCarInfo.CurrentCarPayStatus == NormalCarInfo.CarPayStatus.Reg_Precar_Season)
            {
                EventExitPayForm_NextInfo(mCurrentFormType, NPSYS.FormType.Info, InfoStatus.NoRegExtensDiscount);
                return;
            }
            DcDetail precurrentDcdeatil = CurrentNormalCarInfo.ListDcDetail.Find(x => x.DcTkno == pBarcodeData);
            if (precurrentDcdeatil != null && precurrentDcdeatil.DcTkno == pBarcodeData)
            {
                paymentControl.ErrorMessage = NPSYS.LanguageConvert.GetLanguageData(NPSYS.CurrentLanguageType, NPSYS.LanguageConvert.Header.dynamictype, dynamictype.HEADER.DY_FARE_DUPLICATEBARCODE.ToString());
                return;
            }
            if (true) // true
            {

                int pPrePayMoney = CurrentNormalCarInfo.PaymentMoney;
                //DIscountTicketOcs.TIcketReadingResult resultDiscount = mDIscountTicketOcs.DiscountTIcket(DIscountTicketOcs.DIscountTicketType.Barcode, pBarcodeData, mNormalCarInfo, lblErrorMessage, string.Empty, string.Empty, string.Empty, string.Empty);
                Payment paymentAfterDisocunt = mHttpProcess.Discount(CurrentNormalCarInfo, DcDetail.DIscountTicketType.BR, pBarcodeData);

                if (paymentAfterDisocunt.status.Success) // 정상적인 티켓이라면
                {

                    DcDetail dcDetail = new DcDetail();
                    dcDetail.currentDiscountTicketType = DcDetail.DIscountTicketType.BR;
                    dcDetail.DcTkno = pBarcodeData;
                    dcDetail.UseYn = true;
                    CurrentNormalCarInfo.ListDcDetail.Add(dcDetail);
                    CurrentNormalCarInfo.ParkingMin = paymentAfterDisocunt.parkingMin;
                    CurrentNormalCarInfo.TotFee = Convert.ToInt32(paymentAfterDisocunt.totFee);
                    CurrentNormalCarInfo.TotDc = Convert.ToInt32(paymentAfterDisocunt.totDc);
                    CurrentNormalCarInfo.Change = Convert.ToInt32(paymentAfterDisocunt.change);
                    CurrentNormalCarInfo.RecvAmt = Convert.ToInt32(paymentAfterDisocunt.recvAmt); //시제설정누락처리

                    CurrentNormalCarInfo.DcCnt = paymentAfterDisocunt.dcCnt;
                    CurrentNormalCarInfo.RealFee = Convert.ToInt32(paymentAfterDisocunt.realFee);

                    if (pPrePayMoney == CurrentNormalCarInfo.PaymentMoney)
                    {
                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인에 성공했지만 할인금액이 없음");

                    }
                    else
                    {
                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인성공");
                    }
                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                    {
                        BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectRear();
                        if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                        {
                            ejectResult = NPSYS.Device.BarcodeMoter.EjectRear();
                            if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                            {

                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 성공");
                            }
                            else
                            {
                                paymentControl.ErrorMessage = "바코드가 뒤로 배출되지 않습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 실패");
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 뒤로 배출 성공");
                        }
                    }
                    paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);



                    if (pPrePayMoney > CurrentNormalCarInfo.PaymentMoney) // 현재 할인되서 금액이 할인됬다면
                    {
                        BeforeChangePayValueAsCardReader();
                        if (CurrentNormalCarInfo.PaymentMoney == 0)
                        {
                            //카드실패전송
                            CurrentNormalCarInfo.PaymentMethod = PaymentType.DiscountBarcode;
                            //카드실패전송완료
                            PaymentComplete();
                            return;

                        }
                        ChangePayValueAsCardReader();
                    }

                }
                else
                {

                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.NormaBarcode && NPSYS.Device.gIsUseBarcodeSerial)
                    {

                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditDiscountForm | BarcodeAction", "할인실패 에러원인:" + paymentAfterDisocunt.status.code + " 설명:" + paymentAfterDisocunt.status.message);

                        paymentControl.ErrorMessage = paymentAfterDisocunt.status.description;
                        return;

                    }

                    if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                    {
                        BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                        if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                        {
                            ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                            if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                            {

                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 성공");
                            }
                            else
                            {
                                paymentControl.ErrorMessage = "바코드가 앞으로 배출되지 않습니다.";
                                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 실패");
                            }
                        }
                        else
                        {
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드가 앞으로 배출성공.");
                        }
                    }
                }
            }
            else
            {
                if (NPSYS.Device.UsingSettingDiscountBarcodeSerial == ConfigID.BarcodeReaderType.SVS2000 && NPSYS.Device.gIsUseBarcodeSerial)
                {
                    BarcodeMoter.BarcodeMotorResult ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                    if (ejectResult.ResultStatus != BarcodeMotorErrorCode.Ok)
                    {
                        ejectResult = NPSYS.Device.BarcodeMoter.EjectFront();
                        if (ejectResult.ResultStatus == BarcodeMotorErrorCode.Ok || ejectResult.ResultStatus == BarcodeMotorErrorCode.NoTicketError)
                        {

                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 성공");
                        }
                        else
                        {
                            paymentControl.ErrorMessage = "바코드가 앞으로 배출되지 않습니다.";
                            TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드 앞으로 배출 실패");
                        }
                    }
                    else
                    {
                        TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", " 바코드가 앞으로 배출성공.");
                    }
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormCreditPaymentMenu | BarcodeAction", "현재 할인권은 " + paymentControl.ErrorMessage);
            }
        }

        //바코드할인 리스트로변경 주석처리
        //바코드모터드리블 사용완료
    }
}
