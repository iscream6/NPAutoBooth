using FadeFox.Text;
using NPCommon;
using NPCommon.DEVICE;
using NPCommon.DTO;
using NPCommon.Van;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPAutoBooth.UI.BoothUC.PaymentUC;

namespace NPAutoBooth.UI
{
    /// <summary>
    /// 지폐 및 동전관련
    /// </summary>
    partial class FormCreditPaymentMenu
    {
        private void StartMoneyInsert()
        {
            try
            {
                if (isOnlyCard())
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "카드결제라 동전지폐 관련시작 모든 작업 시작안함");
                    return;
                }

                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "동전지폐 관련 모든 작업 시작");
                if (!NPSYS.Device.UsingSettingCoinReader && !NPSYS.Device.UsingSettingBill)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|StartMoneyInsert", "지폐/동전 사용안함");

                    return;
                }
                if (NPSYS.Device.isUseDeviceCoinReaderDevice && NPSYS.Device.UsingSettingCoinReader)
                {

                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "동전 입수가능동작 시작진행");
                    CoinReader.CoinReaderStatusType l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "동전 입수가능동작 시작종료:" + l_CoinReaderResult.ToString());
                    if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                    {
                        TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StartMoneyInsert", "다시한번 동전 받는동작 작동");
                        l_CoinReaderResult = NPSYS.Device.CoinReader.EnableCoinRead();
                        if (l_CoinReaderResult != CoinReader.CoinReaderStatusType.OK)
                        {
                            NPSYS.Device.CoinReader.DisableCoinRead();
                            NPSYS.Device.CoinReaderDeviceErrorMessage = l_CoinReaderResult.ToString();
                            TextCore.DeviceError(TextCore.DEVICE.COINREADER, "FormPaymentMenu|StartMoneyInsert", l_CoinReaderResult.ToString());
                            SetLanuageDynamic(NPSYS.CurrentLanguageType);

                            NPSYS.LedLight();
                        }
                    }

                }

                if (NPSYS.Device.isUseDeviceBillReaderDevice && NPSYS.Device.UsingSettingBillReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | StartMoneyInsert", "지폐 입수가능동작 시작진행");
                    BillReader.BillRederStatusType curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | StartMoneyInsert", "지폐 입수가능동작 시작종료 성공유무:" + curentBillRederStatusType.ToString());

                    if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                    {
                        NPSYS.Device.BillReader.Reset();
                        curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StartMoneyInsert", "지폐을 받는 동작 작동 문제가 생겨서 다시시도 성공유무:" + curentBillRederStatusType.ToString());
                        if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                        {
                            NPSYS.Device.BillReader.Reset();
                            curentBillRederStatusType = NPSYS.Device.BillReader.BillEnableAccept();
                            if (NPSYS.Device.isUseDeviceBillReaderDevice == false)
                            {
                                SetLanuageDynamic(NPSYS.CurrentLanguageType);
                                NPSYS.LedLight();
                            }
                        }
                    }
                }
                if (NPSYS.Device.isUseDeviceBillReaderDevice || NPSYS.Device.isUseDeviceCoinReaderDevice)
                {
                    tmrReadAccount.Enabled = true;
                    tmrReadAccount.Start();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymenu|StartInsert", ex.ToString());
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "동전지폐 관련 모든 작업 종료");
            }
        }

        private void StopMoneyInsert()
        {
            try
            {
                if (isOnlyCard())
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StartInsert", "카드만결제라 동전지폐 종료관련 모든 작업 시작안함");
                    return;
                }
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StopMoneyInsert", "동전지폐 관련 모든 작업 시작");
                if (!NPSYS.Device.UsingSettingCoinReader && !NPSYS.Device.UsingSettingBill)
                {
                    TextCore.INFO(TextCore.INFOS.PROGRAM_INFO, "FormPaymentMenu|StartMoneyInsert", "지폐/동전 사용안함");
                    return;
                }

                if (NPSYS.Device.isUseDeviceCoinReaderDevice && NPSYS.Device.UsingSettingCoinReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StopMoneyInsert", "동전을 받는 동작 작동 행위를 멈춤 시작");
                    CoinReader.CoinReaderStatusType Result = NPSYS.Device.CoinReader.DisableCoinRead();
                    TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|StopMoneyInsert", "동전을 받는 동작 작동 행위를 멈춤 종료:" + Result.ToString());
                }
                if (NPSYS.Device.isUseDeviceBillReaderDevice && NPSYS.Device.UsingSettingBillReader)
                {
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StopMoneyInsert", "지폐를 받는 동작 작동 행위를 멈춤 시작");
                    NPSYS.Device.BillReader.BillDIsableAccept();
                    TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|StopMoneyInsert", "지폐를 받는 동작 작동 행위를 멈춤 종료 성공유무:" + NPSYS.Device.BillReader.BillDIsableAccept().ToString());
                }

                if (tmrReadAccount.Enabled == true)
                {
                    tmrReadAccount.Enabled = false;
                    tmrReadAccount.Stop();
                }
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymenu|StopMoneyInsert", ex.ToString());
            }
            finally
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ING, "FormPaymenu|StopMoneyInsert", "동전지폐 관련 모든 작업 종료");
            }
        }

        /// <summary>
        /// 들어온 지폐 및 동전을 로컬 DB에 저장한다.
        /// </summary>
        /// <param name="p_BillVaule"></param>
        private void InsertMoney(string p_BillVaule)
        {
            try
            {
                inputtime = paymentInputTimer;

                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                int inMoney = 0;
                inMoney = Convert.ToInt32(p_BillVaule.ToUpper().Replace("QTY", ""));
                if (inMoney <= 0)
                {
                    TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|InsertMoney", "들어온 돈:0");
                    return;
                }

                paymentControl.ErrorMessage = "현금취소시 카드결제가능 합니다";

                // 스마트로 추가
                if (CurrentNormalCarInfo.Current_Money == 0) // 최초동전넣었다면 취소
                {
                    paymentControl.ButtonEnable(ButtonEnableType.InsertCoin);
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SmartroVCat)
                    {
                        timerSmartroVCat.Enabled = false;
                        timerSmartroVCat.Stop();
                        SmatroDeveinCancle();
                    }
                    // 2016.10.27 KIS_DIP 추가
                    else if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.KIS_TIT_DIP_IFM)
                    {
                        // Kis_TIT_DIpDeveinCancle();

                    }
                    if (NPSYS.Device.GetCurrentUseDeviceCard() == ConfigID.CardReaderType.SMATRO_TIT_DIP)
                    {

                        mCardStatus.currentCardReaderStatus = CardDeviceStatus.CardReaderStatus.CardStop;
                        timerKisCardPay.Enabled = false;
                        timerKisCardPay.Stop();
                        timerSmatro_TITDIP_Evcat.Tick -= timerSmatro_TITDIP_Evcat_Tick;
                        this.timerSmatro_TITDIP_Evcat.Enabled = false;
                        this.timerSmatro_TITDIP_Evcat.Stop();
                        paymentControl.ErrorMessage = string.Empty;
                        UnsetSmatro_DIPTIT_Evcat();

                    }
                    // 2016.10.27  KIS_DIP 추가종료

                }
                InsertMoneyChangeValue(p_BillVaule);
                TextCore.INFO(TextCore.INFOS.PAYINFO, "FormPaymentMenu|InsertMoney", "지불해야할 요금:" + CurrentNormalCarInfo.PaymentMoney.ToString() + "총투입금액:" + CurrentNormalCarInfo.GetInComeMoney.ToString() + " 투입금액:" + inMoney.ToString() + " 거스름돈:" + CurrentNormalCarInfo.GetChargeMoney);
                paymentControl.Payment = TextCore.ToCommaString(CurrentNormalCarInfo.PaymentMoney);
                paymentControl.DiscountMoney = TextCore.ToCommaString(CurrentNormalCarInfo.TotDc);
                // 스마트로 추가 종료

                //BoothControl.ProtocolData sendDiscountProtocol = new BoothControl.ProtocolData();
                //sendDiscountProtocol.CurrentCommand = BoothControl.ProtocolData.Command.GETCURRENT_PAYMONEY;
                //NPSYS.sendJunsanInfo(sendDiscountProtocol, mNormalCarInfo);
                //통합처리 동전들어갔을때 전문보내야함
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|InsertMoney", ex.ToString());
            }
        }

        /// <summary>
        /// 현재 투입금액을 증가시킨다.
        /// </summary>
        /// <param name="message"></param>
        public void InsertMoneyChangeValue(string message)
        {
            if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.WON)
            {
                switch (message.ToUpper())
                {
                    case "50QTY":
                        CurrentNormalCarInfo.InCome50Qty = CurrentNormalCarInfo.InCome50Qty + 1;
                        break;
                    case "100QTY":
                        CurrentNormalCarInfo.InCome100Qty = CurrentNormalCarInfo.InCome100Qty + 1;
                        break;
                    case "500QTY":
                        CurrentNormalCarInfo.InCome500Qty = CurrentNormalCarInfo.InCome500Qty + 1;
                        break;
                    case "1000QTY":
                        CurrentNormalCarInfo.InCome1000Qty = CurrentNormalCarInfo.InCome1000Qty + 1;
                        break;
                    case "5000QTY":
                        CurrentNormalCarInfo.InCome5000Qty = CurrentNormalCarInfo.InCome5000Qty + 1;
                        break;
                    case "10000QTY":
                        CurrentNormalCarInfo.InCome10000Qty = CurrentNormalCarInfo.InCome10000Qty + 1;
                        break;
                    case "50000QTY":
                        CurrentNormalCarInfo.InCome50000Qty = CurrentNormalCarInfo.InCome50000Qty + 1;
                        break;
                }
            }
            else if (NPSYS.CurrentMoneyType == ConfigID.MoneyType.PHP)
            {
                switch (message.ToUpper())
                {
                    case "1QTY":
                        CurrentNormalCarInfo.InCome50Qty = CurrentNormalCarInfo.InCome50Qty + 1;
                        break;
                    case "5QTY":
                        CurrentNormalCarInfo.InCome100Qty = CurrentNormalCarInfo.InCome100Qty + 1;
                        break;
                    case "10QTY":
                        CurrentNormalCarInfo.InCome500Qty = CurrentNormalCarInfo.InCome500Qty + 1;
                        break;
                    case "20QTY":
                        CurrentNormalCarInfo.InCome1000Qty = CurrentNormalCarInfo.InCome1000Qty + 1;
                        break;
                    case "50QTY":
                        CurrentNormalCarInfo.InCome5000Qty = CurrentNormalCarInfo.InCome5000Qty + 1;
                        break;
                    case "100QTY":
                        CurrentNormalCarInfo.InCome10000Qty = CurrentNormalCarInfo.InCome10000Qty + 1;
                        break;
                }
            }
        }

        ///// <summary>
        ///// 지폐를 돈통에 넣는 작업 , 지폐입수후 BillAccept 명령 실행하지 않을시 자동으로 돈이 앞으로 리젝된다
        ///// </summary>
        //private byte[] getBillInsert()
        //{
        //    return NPSYS.Device.BillReader.BillAccept();

        //}

        ///// <summary>
        ///// 지폐를 돈통에 넣지않는 작업 Reject
        ///// </summary>
        //private byte[] getBillReject()
        //{
        //    return NPSYS.Device.BillReader.BillReject();

        //}

        private void tmrReadAccount_Tick(object sender, EventArgs e)
        {
            tmrReadAccount.Stop();

            if (!NPSYS.Device.isUseDeviceBillReaderDevice && !NPSYS.Device.isUseDeviceCoinReaderDevice)  // 지폐 및 동전 리더기 둘다 동시 작동이 안될때
            {
                tmrReadAccount.Enabled = false;
                return;
            }
            try
            {
                tmrReadAccount.Stop();

                string logDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (NPSYS.Device.isUseDeviceCoinReaderDevice)
                {
                    //동전연속투입관련 변경
                    if (NPSYS.Device.CoinReader.mLIstQty.Count > 0)
                    {
                        string coinmessage = NPSYS.Device.CoinReader.mLIstQty[0].ToString();
                        NPSYS.Device.CoinReader.mLIstQty.RemoveAt(0);
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu|tmrReadAccount_Tick", "동전 넣음");
                        TextCore.ACTION(TextCore.ACTIONS.COINREADER, "FormPaymentMenu|tmrReadAccount_Tick", "동전 들어옴");
                        InsertMoney(coinmessage);
                    }
                }

                //동전연속투입관련 변경
                if (BillReader.g_billValue.Trim() != "")
                {
                    string billValue = BillReader.g_billValue;
                    BillReader.g_billValue = "";
                    if (billValue.ToUpper().Trim() == "REJECT")
                    {
                        NPSYS.Device.BillReader.BillReject();
                        TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐불량으로 리젝트");
                    }
                    else
                    {
                        BillReader.BillRederStatusType currentInsertStatus = BillReader.BillRederStatusType.OK;
                        TextCore.ACTION(TextCore.ACTIONS.USER, "FormPaymentMenu|tmrReadAccount_Tick", "지폐 넣음:" + billValue);
                        if (NPSYS.SettingUse50000QtyBill == true)
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillAccept();
                            if (currentInsertStatus == BillReader.BillRederStatusType.OK)
                            {
                                InsertMoney(billValue);
                            }
                            else
                            {
                                currentInsertStatus = NPSYS.Device.BillReader.BillReject();
                                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "지폐불량으로 리젝트:" + currentInsertStatus.ToString());
                            }
                        }
                        else if (billValue.ToUpper() == "50000QTY")
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillReject(); ;
                            TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "5만원권 사용불가처리로 리젝트:" + currentInsertStatus.ToString());
                        }
                        else
                        {
                            currentInsertStatus = NPSYS.Device.BillReader.BillAccept();
                            if (currentInsertStatus == BillReader.BillRederStatusType.OK)
                            {
                                InsertMoney(billValue);
                            }
                            else
                            {
                                currentInsertStatus = NPSYS.Device.BillReader.BillReject();
                                TextCore.ACTION(TextCore.ACTIONS.BILLREADER, "FormPaymentMenu | tmrReadAccount_Tick", "지폐불량으로 리젝트:" + currentInsertStatus.ToString());
                            }
                        }
                    }
                }

                if (CurrentNormalCarInfo.Current_Money > 0)
                {
                    paymentControl.CancelButtonVisible = true;
                }
                else
                {
                    paymentControl.CancelButtonVisible = false;
                }

                if (CurrentNormalCarInfo.Current_Money == 0)
                {
                    tmrReadAccount.Start();
                    return;
                }

                if (CurrentNormalCarInfo.PaymentMoney == 0)
                {
                    //카드실패전송
                    CurrentNormalCarInfo.PaymentMethod = PaymentType.Cash;
                    //카드실패전송완료
                    PaymentComplete();

                    return;
                }

                tmrReadAccount.Start();
                return;
            }
            catch (Exception ex)
            {
                TextCore.INFO(TextCore.INFOS.PROGRAM_ERROR, "FormPaymentMenu|tmrReadAccount_Tick", ex.ToString());
                tmrReadAccount.Start();
            }
            finally
            {

            }
        }
    }
}
